using System;

namespace AcademicJournal.Server.Models
{
    public class AddHomeworkDto
    {
        public int SubjectId { get; set; }
        public int GroupId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public int? FileId { get; set; }
    }
}