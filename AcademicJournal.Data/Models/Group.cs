namespace AcademicJournal.Data.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int YearOfStudy { get; set; }
        public string Specialization { get; set; }
        
        // Навигационные свойства
        public ICollection<Student> Students { get; set; }
    }
}