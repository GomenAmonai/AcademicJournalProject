using System;

namespace AcademicJournal.Data.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        
        public int LessonId { get; set; }
        public virtual Lesson Lesson { get; set; }
        
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
        
        public bool IsPresent { get; set; }
        public string Comment { get; set; }
        
        public DateTime RecordedAt { get; set; }
    }
}