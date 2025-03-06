using System;

namespace AcademicJournal.Server.Models
{
    public class AttendanceDto
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public string LessonTopic { get; set; }
        public string SubjectName { get; set; }
        public DateTime LessonDate { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public bool IsPresent { get; set; }
        public string Comment { get; set; }
        public DateTime RecordedAt { get; set; }
    }
}