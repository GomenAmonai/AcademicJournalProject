namespace AcademicJournal.Data.Models
{
    public class Homework
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public int GroupId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string FileAttachmentPath { get; set; }
        
        // Навигационные свойства
        public Subject Subject { get; set; }
        public Group Group { get; set; }
        public ICollection<HomeworkSubmission> Submissions { get; set; }
    }
}