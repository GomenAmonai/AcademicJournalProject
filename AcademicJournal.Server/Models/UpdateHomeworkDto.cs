using System;

namespace AcademicJournal.Server.Models
{
    public class UpdateHomeworkDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public int? FileId { get; set; }
    }
}