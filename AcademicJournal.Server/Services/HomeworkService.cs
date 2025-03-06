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
    public class HomeworkService : IHomeworkService
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;

        public HomeworkService(ApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<PagedResult<HomeworkDto>> GetHomeworksAsync(HomeworkFilterDto filter)
        {
            var query = _context.Homeworks
                .Include(h => h.Subject)
                .Include(h => h.Group)
                .AsQueryable();
                
            // Применяем фильтрацию
            if (filter.SubjectId.HasValue)
                query = query.Where(h => h.SubjectId == filter.SubjectId.Value);
                
            if (filter.GroupId.HasValue)
                query = query.Where(h => h.GroupId == filter.GroupId.Value);
                
            if (filter.StartDate.HasValue)
                query = query.Where(h => h.CreatedAt >= filter.StartDate.Value);
                
            if (filter.EndDate.HasValue)
                query = query.Where(h => h.CreatedAt <= filter.EndDate.Value);
                
            if (filter.OnlyActive)
                query = query.Where(h => h.Deadline >= DateTime.Now);
                
            // Применяем сортировку
            query = filter.SortByDate switch
            {
                SortDirection.Ascending => query.OrderBy(h => h.CreatedAt),
                SortDirection.Descending => query.OrderByDescending(h => h.CreatedAt),
                _ => query.OrderByDescending(h => h.CreatedAt)
            };
            
            // Реализация пагинации
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
                
            var dtos = items.Select(h => new HomeworkDto
            {
                Id = h.Id,
                SubjectId = h.SubjectId,
                SubjectName = h.Subject?.Name,
                GroupId = h.GroupId,
                GroupName = h.Group?.Name,
                Title = h.Title,
                Description = h.Description,
                Deadline = h.Deadline,
                CreatedAt = h.CreatedAt,
                FileId = h.FileId
            }).ToList();
            
            return new PagedResult<HomeworkDto>
            {
                Items = dtos,
                TotalCount = totalCount,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }

        public async Task<HomeworkDto> GetHomeworkByIdAsync(int id)
        {
            var homework = await _context.Homeworks
                .Include(h => h.Subject)
                .Include(h => h.Group)
                .FirstOrDefaultAsync(h => h.Id == id);
                
            if (homework == null)
                return null;
                
            return new HomeworkDto
            {
                Id = homework.Id,
                SubjectId = homework.SubjectId,
                SubjectName = homework.Subject?.Name,
                GroupId = homework.GroupId,
                GroupName = homework.Group?.Name,
                Title = homework.Title,
                Description = homework.Description,
                Deadline = homework.Deadline,
                CreatedAt = homework.CreatedAt,
                FileId = homework.FileId
            };
        }

        public async Task<HomeworkDto> AddHomeworkAsync(AddHomeworkDto homeworkDto)
        {
            var homework = new Homework
            {
                SubjectId = homeworkDto.SubjectId,
                GroupId = homeworkDto.GroupId,
                Title = homeworkDto.Title,
                Description = homeworkDto.Description,
                Deadline = homeworkDto.Deadline,
                CreatedAt = DateTime.Now,
                FileId = homeworkDto.FileId
            };
            
            _context.Homeworks.Add(homework);
            await _context.SaveChangesAsync();
            
            // Загружаем связанные данные
            await _context.Entry(homework).Reference(h => h.Subject).LoadAsync();
            await _context.Entry(homework).Reference(h => h.Group).LoadAsync();
            
            // Отправляем уведомление группе студентов
            // Раскомментируйте после реализации SendHomeworkNotificationAsync
            // await _notificationService.SendHomeworkNotificationAsync(homework.GroupId.ToString(), homework.Id);
            
            return new HomeworkDto
            {
                Id = homework.Id,
                SubjectId = homework.SubjectId,
                SubjectName = homework.Subject?.Name,
                GroupId = homework.GroupId,
                GroupName = homework.Group?.Name,
                Title = homework.Title,
                Description = homework.Description,
                Deadline = homework.Deadline,
                CreatedAt = homework.CreatedAt,
                FileId = homework.FileId
            };
        }

        public async Task UpdateHomeworkAsync(UpdateHomeworkDto homeworkDto)
        {
            var homework = await _context.Homeworks.FindAsync(homeworkDto.Id);
            if (homework == null)
                throw new KeyNotFoundException($"Homework with ID {homeworkDto.Id} not found");
                
            homework.Title = homeworkDto.Title;
            homework.Description = homeworkDto.Description;
            homework.Deadline = homeworkDto.Deadline;
            homework.FileId = homeworkDto.FileId;
            
            await _context.SaveChangesAsync();
        }

        public async Task DeleteHomeworkAsync(int id)
        {
            var homework = await _context.Homeworks.FindAsync(id);
            if (homework == null)
                throw new KeyNotFoundException($"Homework with ID {id} not found");
                
            _context.Homeworks.Remove(homework);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<HomeworkDto>> GetHomeworksBySubjectAsync(int subjectId, HomeworkFilterDto filter)
        {
            filter.SubjectId = subjectId;
            return await GetHomeworksAsync(filter);
        }

        public async Task<PagedResult<HomeworkDto>> GetHomeworksByGroupAsync(int groupId, HomeworkFilterDto filter)
        {
            filter.GroupId = groupId;
            return await GetHomeworksAsync(filter);
        }
    }
}