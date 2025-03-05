namespace AcademicJournal.Data.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public int TeacherId { get; set; }
        public int Value { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        
        // Навигационные свойства
        public Student Student { get; set; }
        public Subject Subject { get; set; }
        public Teacher Teacher { get; set; }
    }
}