// AcademicJournal.Data/Models/Lesson.cs
using System;
using System.Collections.Generic;

namespace AcademicJournal.Data.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public int TeacherId { get; set; }
        public int GroupId { get; set; }
        public DateTime Date { get; set; }
        public string Topic { get; set; }
        public string Description { get; set; }
        public LessonType Type { get; set; }
        
        // Навигационные свойства
        public virtual Subject Subject { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual Group Group { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }
    }
    
    public enum LessonType
    {
        Lecture,
        Practice,
        Laboratory,
        Seminar,
        Exam
    }
}