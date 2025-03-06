// AcademicJournal.Data/Models/Notification.cs
using System;

namespace AcademicJournal.Data.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public NotificationType Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public string RelatedEntityType { get; set; }
        public int? RelatedEntityId { get; set; }
        
        // Навигационное свойство
        public virtual User User { get; set; }
    }
    
    public enum NotificationType
    {
        Grade,
        Homework,
        Attendance,
        System,
        Message
    }
}