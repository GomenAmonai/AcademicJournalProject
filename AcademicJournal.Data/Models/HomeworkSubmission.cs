namespace AcademicJournal.Data.Models
{
    public enum SubmissionStatus
    {
        Submitted,
        Checked,
        Returned
    }
    
    public class HomeworkSubmission
    {
        public int Id { get; set; }
        public int HomeworkId { get; set; }
        public int StudentId { get; set; }
        public DateTime SubmissionDate { get; set; }
        public SubmissionStatus Status { get; set; }
        public int? Grade { get; set; }
        public string TeacherComment { get; set; }
        public string FileSubmissionPath { get; set; }
        
        // Навигационные свойства
        public Homework Homework { get; set; }
        public Student Student { get; set; }
    }
}