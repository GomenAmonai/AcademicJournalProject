using System;

namespace AcademicJournal.Server.Models
{
    public class AttendanceFilterDto
    {
        public int? LessonId { get; set; }
        public int? StudentId { get; set; }
        public int? SubjectId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsPresent { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}