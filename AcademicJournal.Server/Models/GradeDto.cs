using System;

namespace AcademicJournal.Server.Models
{
    public class GradeDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public double Value { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}