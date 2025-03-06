namespace AcademicJournal.Server.Models
{
    public class AddGradeDto
    {
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public double Value { get; set; }
        public string Description { get; set; }
    }
}