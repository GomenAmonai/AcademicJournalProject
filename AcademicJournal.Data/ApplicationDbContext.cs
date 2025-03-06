// AcademicJournal.Data/ApplicationDbContext.cs
using AcademicJournal.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AcademicJournal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Homework> Homeworks { get; set; }
        public DbSet<HomeworkSubmission> HomeworkSubmissions { get; set; }
        public DbSet<FileEntity> Files { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        // AcademicJournal.Data/ApplicationDbContext.cs
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Настройка отношений между таблицами
    modelBuilder.Entity<Student>()
        .HasOne<User>()
        .WithOne()
        .HasForeignKey<Student>(s => s.UserId);

    modelBuilder.Entity<Teacher>()
        .HasOne<User>()
        .WithOne()
        .HasForeignKey<Teacher>(t => t.UserId);

    modelBuilder.Entity<Student>()
        .HasOne<Group>()
        .WithMany()
        .HasForeignKey(s => s.GroupId);

    modelBuilder.Entity<Grade>()
        .HasOne<Student>()
        .WithMany()
        .HasForeignKey(g => g.StudentId);

    modelBuilder.Entity<Grade>()
        .HasOne<Subject>()
        .WithMany()
        .HasForeignKey(g => g.SubjectId);

    modelBuilder.Entity<Homework>()
        .HasOne<Subject>()
        .WithMany()
        .HasForeignKey(h => h.SubjectId);

    modelBuilder.Entity<Homework>()
        .HasOne<Group>()
        .WithMany()
        .HasForeignKey(h => h.GroupId);

    modelBuilder.Entity<HomeworkSubmission>()
        .HasOne<Homework>()
        .WithMany()
        .HasForeignKey(hs => hs.HomeworkId);

    modelBuilder.Entity<HomeworkSubmission>()
        .HasOne<Student>()
        .WithMany()
        .HasForeignKey(hs => hs.StudentId);

    modelBuilder.Entity<HomeworkSubmission>()
        .HasOne<FileEntity>()
        .WithMany()
        .HasForeignKey(hs => hs.FileId);

    modelBuilder.Entity<Lesson>()
        .HasOne<Subject>()
        .WithMany()
        .HasForeignKey(l => l.SubjectId);

    modelBuilder.Entity<Lesson>()
        .HasOne<Teacher>()
        .WithMany()
        .HasForeignKey(l => l.TeacherId);

    modelBuilder.Entity<Lesson>()
        .HasOne<Group>()
        .WithMany()
        .HasForeignKey(l => l.GroupId);

    modelBuilder.Entity<Attendance>()
        .HasOne<Lesson>()
        .WithMany()
        .HasForeignKey(a => a.LessonId);

    modelBuilder.Entity<Attendance>()
        .HasOne<Student>()
        .WithMany()
        .HasForeignKey(a => a.StudentId);

    modelBuilder.Entity<Notification>()
        .HasOne<User>()
        .WithMany()
        .HasForeignKey(n => n.UserId);

    modelBuilder.Entity<FileEntity>()
        .HasOne<User>()
        .WithMany()
        .HasForeignKey(f => f.UploadedById);
    
    // Явное определение отношения между User и Student
    modelBuilder.Entity<User>()
        .HasOne<Student>()
        .WithOne(s => s.User)
        .HasForeignKey<Student>(s => s.UserId);

    // Явное определение отношения между User и Teacher
    modelBuilder.Entity<User>()
        .HasOne<Teacher>()
        .WithOne(t => t.User)
        .HasForeignKey<Teacher>(t => t.UserId);

    // Остальные отношения...
    modelBuilder.Entity<Student>()
        .HasOne<Group>()
        .WithMany()
        .HasForeignKey(s => s.GroupId);
}

    }
}