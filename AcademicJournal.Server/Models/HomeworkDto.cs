using System;

namespace AcademicJournal.Server.Models
{
    public class HomeworkDto
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? FileId { get; set; }
    }
}