// AcademicJournal.Data/Models/Attendance.cs
using System;

namespace AcademicJournal.Data.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public int StudentId { get; set; }
        public bool IsPresent { get; set; }
        public string Comment { get; set; }
        public DateTime RecordedAt { get; set; }
        
        // Навигационные свойства
        public virtual Subject Subject { get; set; }
        public virtual Student Student { get; set; }
    }
}