using System.Text.RegularExpressions;

namespace AcademicJournal.Data.Models;

public class Student
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int GroupId { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public string StudentNumber { get; set; }

    //навигационные свйоства
    //связь 1 к 1
    public User User { get; set; }

    //связь много к 1 
    public Group Group { get; set; }

    //связнь 1 ко многим
    public ICollection<Grade> Grades { get; set; }
    public ICollection<Attendance> Attendances { get; set; }
    public ICollection<HomeworkSubmission> HomeworkSubmissions { get; set; }
}
