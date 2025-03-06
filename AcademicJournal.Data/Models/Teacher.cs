namespace AcademicJournal.Data.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Qualifications { get; set; }
        public DateTime HireDate { get; set; }
        
        // Навигационные свойства
        public User User { get; set; }
        
        // Коллекция предметов, которые ведет преподаватель
        public ICollection<TeacherSubject> TeacherSubjects { get; set; }
    }
}