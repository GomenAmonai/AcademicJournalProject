namespace AcademicJournal.Data.Models
{
    public class Student
    {
        public int Id { get; set; }
        
        // Внешний ключ к таблице Users
        public int UserId { get; set; }
        
        // Внешний ключ к таблице Groups
        public int GroupId { get; set; }
        
        // Дата зачисления
        public DateTime EnrollmentDate { get; set; }
        
        // Номер студенческого билета
        public string StudentNumber { get; set; }
        
        // Навигационные свойства
        // Связь с пользователем (1 к 1)
        public User User { get; set; }
        
        // Связь с группой (много к 1)
        public Group Group { get; set; }
        
        // Коллекции связанных объектов (1 ко многим)
        public ICollection<Grade> Grades { get; set; }
        public ICollection<Attendance> Attendances { get; set; }
        public ICollection<HomeworkSubmission> HomeworkSubmissions { get; set; }
    }
}