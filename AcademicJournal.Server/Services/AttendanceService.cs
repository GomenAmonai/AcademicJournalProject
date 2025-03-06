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
    public class AttendanceService : IAttendanceService
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;

        public AttendanceService(ApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<PagedResult<AttendanceDto>> GetAttendancesAsync(AttendanceFilterDto filter)
        {
            var query = _context.Attendances
                .Include(a => a.Lesson)
                    .ThenInclude(l => l.Subject)
                .Include(a => a.Student)
                    .ThenInclude(s => s.User)
                .AsQueryable();
                
            // Применяем фильтрацию
            if (filter.LessonId.HasValue)
                query = query.Where(a => a.LessonId == filter.LessonId.Value);
                
            if (filter.StudentId.HasValue)
                query = query.Where(a => a.StudentId == filter.StudentId.Value);
                
            if (filter.SubjectId.HasValue)
                query = query.Where(a => a.Lesson.SubjectId == filter.SubjectId.Value);
                
            if (filter.StartDate.HasValue)
                query = query.Where(a => a.Lesson.Date >= filter.StartDate.Value);
                
            if (filter.EndDate.HasValue)
                query = query.Where(a => a.Lesson.Date <= filter.EndDate.Value);
                
            if (filter.IsPresent.HasValue)
                query = query.Where(a => a.IsPresent == filter.IsPresent.Value);
                
            // Применяем сортировку по дате занятия
            query = query.OrderByDescending(a => a.Lesson.Date);
            
            // Реализация пагинации
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
                
            var dtos = items.Select(a => new AttendanceDto
            {
                Id = a.Id,
                LessonId = a.LessonId,
                LessonTopic = a.Lesson?.Topic,
                SubjectName = a.Lesson?.Subject?.Name,
                LessonDate = a.Lesson?.Date ?? DateTime.MinValue,
                StudentId = a.StudentId,
                StudentName = a.Student?.User?.FullName ?? a.Student?.User?.Username ?? $"Student ID: {a.StudentId}",
                IsPresent = a.IsPresent,
                Comment = a.Comment,
                RecordedAt = a.RecordedAt
            }).ToList();
            
            return new PagedResult<AttendanceDto>
            {
                Items = dtos,
                TotalCount = totalCount,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }

        public async Task<AttendanceDto> GetAttendanceByIdAsync(int id)
        {
            var attendance = await _context.Attendances
                .Include(a => a.Lesson)
                    .ThenInclude(l => l.Subject)
                .Include(a => a.Student)
                    .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(a => a.Id == id);
                
            if (attendance == null)
                return null;
                
            return new AttendanceDto
            {
                Id = attendance.Id,
                LessonId = attendance.LessonId,
                LessonTopic = attendance.Lesson?.Topic,
                SubjectName = attendance.Lesson?.Subject?.Name,
                LessonDate = attendance.Lesson?.Date ?? DateTime.MinValue,
                StudentId = attendance.StudentId,
                StudentName = attendance.Student?.User?.FullName ?? attendance.Student?.User?.Username ?? $"Student ID: {attendance.StudentId}",
                IsPresent = attendance.IsPresent,
                Comment = attendance.Comment,
                RecordedAt = attendance.RecordedAt
            };
        }

        public async Task<AttendanceDto> AddAttendanceAsync(AddAttendanceDto attendanceDto)
        {
            var attendance = new Attendance
            {
                LessonId = attendanceDto.LessonId,
                StudentId = attendanceDto.StudentId,
                IsPresent = attendanceDto.IsPresent,
                Comment = attendanceDto.Comment,
                RecordedAt = DateTime.Now
            };
            
            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();
            
            // Загружаем связанные данные
            await _context.Entry(attendance).Reference(a => a.Lesson).LoadAsync();
            await _context.Entry(attendance.Lesson).Reference(l => l.Subject).LoadAsync();
            await _context.Entry(attendance).Reference(a => a.Student).LoadAsync();
            if (attendance.Student != null)
            {
                await _context.Entry(attendance.Student).Reference(s => s.User).LoadAsync();
            }
            
            // Отправляем уведомление студенту
            // Если вы добавите метод SendAttendanceNotificationAsync в NotificationService
            // await _notificationService.SendAttendanceNotificationAsync(attendance.StudentId.ToString(), attendance.LessonId, attendance.IsPresent);
            
            return new AttendanceDto
            {
                Id = attendance.Id,
                LessonId = attendance.LessonId,
                LessonTopic = attendance.Lesson?.Topic,
                SubjectName = attendance.Lesson?.Subject?.Name,
                LessonDate = attendance.Lesson?.Date ?? DateTime.MinValue,
                StudentId = attendance.StudentId,
                StudentName = attendance.Student?.User?.FullName ?? attendance.Student?.User?.Username ?? $"Student ID: {attendance.StudentId}",
                IsPresent = attendance.IsPresent,
                Comment = attendance.Comment,
                RecordedAt = attendance.RecordedAt
            };
        }

        public async Task UpdateAttendanceAsync(UpdateAttendanceDto attendanceDto)
        {
            var attendance = await _context.Attendances.FindAsync(attendanceDto.Id);
            if (attendance == null)
                throw new KeyNotFoundException($"Attendance with ID {attendanceDto.Id} not found");
                
            attendance.IsPresent = attendanceDto.IsPresent;
            attendance.Comment = attendanceDto.Comment;
            
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAttendanceAsync(int id)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null)
                throw new KeyNotFoundException($"Attendance with ID {id} not found");
                
            _context.Attendances.Remove(attendance);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<AttendanceDto>> GetAttendancesByLessonAsync(int lessonId, AttendanceFilterDto filter)
        {
            filter.LessonId = lessonId;
            return await GetAttendancesAsync(filter);
        }

        public async Task<PagedResult<AttendanceDto>> GetAttendancesByStudentAsync(int studentId, AttendanceFilterDto filter)
        {
            filter.StudentId = studentId;
            return await GetAttendancesAsync(filter);
        }

        public async Task<PagedResult<AttendanceDto>> GetAttendancesBySubjectAsync(int subjectId, AttendanceFilterDto filter)
        {
            filter.SubjectId = subjectId;
            return await GetAttendancesAsync(filter);
        }
    }
}