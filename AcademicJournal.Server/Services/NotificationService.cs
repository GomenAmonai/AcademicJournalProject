using AcademicJournal.Data;
using AcademicJournal.Data.Models;
using AcademicJournal.Server.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AcademicJournal.Server.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ApplicationDbContext _context;

        public NotificationService(IHubContext<NotificationHub> hubContext, ApplicationDbContext context)
        {
            _hubContext = hubContext;
            _context = context;
        }

        public async Task SendGradeNotificationAsync(string studentId, int gradeId)
        {
            // Получаем данные об оценке из базы
            var grade = await _context.Grades
                .Include(g => g.Subject)
                .FirstOrDefaultAsync(g => g.Id == gradeId);

            if (grade != null)
            {
                // Создаем объект уведомления для отправки клиенту
                var notification = new
                {
                    GradeId = grade.Id,
                    SubjectName = grade.Subject?.Name ?? "Предмет",
                    Value = grade.Value,
                    Description = grade.Description
                };

                // Отправляем уведомление через SignalR
                await _hubContext.Clients.Group($"User_{studentId}")
                    .SendAsync("ReceiveNotification", "NewGrade", notification);

                // Сохраняем уведомление в базе данных
                _context.Notifications.Add(new Notification
                {
                    UserId = int.Parse(studentId),
                    Title = $"Новая оценка по предмету {grade.Subject?.Name ?? "Предмет"}",
                    Content = $"Вы получили оценку {grade.Value}",
                    Type = NotificationType.Grade,
                    CreatedAt = DateTime.Now,
                    IsRead = false,
                    RelatedEntityType = "Grade",
                    RelatedEntityId = grade.Id
                });

                await _context.SaveChangesAsync();
            }
        }

        public async Task SendHomeworkNotificationAsync(string groupId, int homeworkId)
        {
            // Получаем данные о домашнем задании
            var homework = await _context.Homeworks
                .Include(h => h.Subject)
                .FirstOrDefaultAsync(h => h.Id == homeworkId);

            if (homework != null)
            {
                // Получаем список студентов в группе
                var students = await _context.Students
                    .Where(s => s.GroupId.ToString() == groupId)
                    .ToListAsync();

                // Создаем объект уведомления
                var notification = new
                {
                    HomeworkId = homework.Id,
                    SubjectName = homework.Subject?.Name ?? "Предмет",
                    Title = homework.Title,
                    Deadline = homework.Deadline // Используем правильное название свойства
                };

                // Отправляем уведомление каждому студенту и сохраняем в базе
                foreach (var student in students)
                {
                    // Отправляем через SignalR
                    await _hubContext.Clients.Group($"User_{student.UserId}")
                        .SendAsync("ReceiveNotification", "NewHomework", notification);

                    // Сохраняем в базе
                    _context.Notifications.Add(new Notification
                    {
                        UserId = student.UserId,
                        Title = $"Новое домашнее задание по предмету {homework.Subject?.Name ?? "Предмет"}",
                        Content = $"Назначено новое задание: {homework.Title}. Срок сдачи: {homework.Deadline:dd.MM.yyyy}", // Используем правильное название свойства
                        Type = NotificationType.Homework,
                        CreatedAt = DateTime.Now,
                        IsRead = false,
                        RelatedEntityType = "Homework",
                        RelatedEntityId = homework.Id
                    });
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task SendPersonalNotificationAsync(string userId, string title, string message)
        {
            // Отправляем через SignalR
            await _hubContext.Clients.Group($"User_{userId}")
                .SendAsync("ReceiveNotification", "Personal", new { Title = title, Message = message });

            // Сохраняем в базе
            _context.Notifications.Add(new Notification
            {
                UserId = int.Parse(userId),
                Title = title,
                Content = message,
                Type = NotificationType.System,
                CreatedAt = DateTime.Now,
                IsRead = false
            });

            await _context.SaveChangesAsync();
        }
    }
}