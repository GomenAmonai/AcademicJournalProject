using AcademicJournal.Data;
using AcademicJournal.Data.Models;
using AcademicJournal.Server.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademicJournal.Server.Services
{
    public class GradeService : IGradeService
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;

        public GradeService(ApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<PagedResult<GradeDto>> GetStudentGradesAsync(string studentId, GradeFilterDto filter)
        {
            var query = _context.Grades
                .Include(g => g.Subject)
                .Where(g => g.StudentId.ToString() == studentId);
                
            // Применяем фильтрацию
            if (filter.SubjectId.HasValue)
                query = query.Where(g => g.SubjectId == filter.SubjectId.Value);
                
            if (filter.StartDate.HasValue)
                query = query.Where(g => g.Date >= filter.StartDate.Value);
                
            if (filter.EndDate.HasValue)
                query = query.Where(g => g.Date <= filter.EndDate.Value);
                
            // Применяем сортировку
            query = filter.SortByDate switch
            {
                SortDirection.Ascending => query.OrderBy(g => g.Date),
                SortDirection.Descending => query.OrderByDescending(g => g.Date),
                _ => query.OrderByDescending(g => g.Date)
            };
            
            // Реализация пагинации
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
                
            var dtos = items.Select(g => new GradeDto
            {
                Id = g.Id,
                StudentId = g.StudentId,
                SubjectId = g.SubjectId,
                SubjectName = g.Subject?.Name,
                Value = g.Value, // Используем тот же тип, что и в модели
                Description = g.Description,
                Date = g.Date
            }).ToList();
            
            return new PagedResult<GradeDto>
            {
                Items = dtos,
                TotalCount = totalCount,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }

        public async Task<GradeDto> GetGradeByIdAsync(int id)
        {
            var grade = await _context.Grades
                .Include(g => g.Subject)
                .FirstOrDefaultAsync(g => g.Id == id);
                
            if (grade == null)
                return null;
                
            return new GradeDto
            {
                Id = grade.Id,
                StudentId = grade.StudentId,
                SubjectId = grade.SubjectId,
                SubjectName = grade.Subject?.Name,
                Value = grade.Value, // Используем тот же тип, что и в модели
                Description = grade.Description,
                Date = grade.Date
            };
        }

        public async Task<GradeDto> AddGradeAsync(AddGradeDto gradeDto)
        {
            // Объявляем переменную grade перед её использованием
            var grade = new Grade
            {
                StudentId = gradeDto.StudentId,
                SubjectId = gradeDto.SubjectId,
                Value = Convert.ToInt32(gradeDto.Value), // Явное преобразование double в int
                Description = gradeDto.Description,
                Date = DateTime.Now
            };
            
            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();
            
            // Загружаем Subject, чтобы получить имя предмета
            await _context.Entry(grade).Reference(g => g.Subject).LoadAsync();
            
            // Отправляем уведомление студенту
            await _notificationService.SendGradeNotificationAsync(grade.StudentId.ToString(), grade.Id);
            
            return new GradeDto
            {
                Id = grade.Id,
                StudentId = grade.StudentId,
                SubjectId = grade.SubjectId,
                SubjectName = grade.Subject?.Name,
                Value = grade.Value, // Используем тот же тип, что и в модели
                Description = grade.Description,
                Date = grade.Date
            };
        }

        public async Task UpdateGradeAsync(UpdateGradeDto gradeDto)
        {
            var grade = await _context.Grades.FindAsync(gradeDto.Id);
            if (grade == null)
                throw new KeyNotFoundException($"Grade with ID {gradeDto.Id} not found");
                
            grade.Value = Convert.ToInt32(gradeDto.Value); // Явное преобразование double в int
            grade.Description = gradeDto.Description;
            
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGradeAsync(int id)
        {
            var grade = await _context.Grades.FindAsync(id);
            if (grade == null)
                throw new KeyNotFoundException($"Grade with ID {id} not found");
                
            _context.Grades.Remove(grade);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<GradeDto>> GetGradesBySubjectAsync(int subjectId, GradeFilterDto filter)
        {
            var query = _context.Grades
                .Include(g => g.Subject)
                .Include(g => g.Student)
                .Where(g => g.SubjectId == subjectId);
                
            if (filter.StartDate.HasValue)
                query = query.Where(g => g.Date >= filter.StartDate.Value);
                
            if (filter.EndDate.HasValue)
                query = query.Where(g => g.Date <= filter.EndDate.Value);
                
            // Применяем сортировку
            query = filter.SortByDate switch
            {
                SortDirection.Ascending => query.OrderBy(g => g.Date),
                SortDirection.Descending => query.OrderByDescending(g => g.Date),
                _ => query.OrderByDescending(g => g.Date)
            };
            
            // Реализация пагинации
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
                
            var dtos = items.Select(g => new GradeDto
            {
                Id = g.Id,
                StudentId = g.StudentId,
                SubjectId = g.SubjectId,
                SubjectName = g.Subject?.Name,
                Value = g.Value, // Используем тот же тип, что и в модели
                Description = g.Description,
                Date = g.Date
            }).ToList();
            
            return new PagedResult<GradeDto>
            {
                Items = dtos,
                TotalCount = totalCount,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }
    }
}