using Microsoft.EntityFrameworkCore;
using SchoolApi.Models;

namespace SchoolApi.Data;

public class SchoolContext : DbContext
{
    public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
    {
    }

    public DbSet<Classroom> Classrooms => Set<Classroom>();
    public DbSet<Student> Students => Set<Student>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Classroom>().HasData(
            new Classroom { Id = 1, Name = "Class 1" },
            new Classroom { Id = 2, Name = "Class 2" },
            new Classroom { Id = 3, Name = "Class 3" },
            new Classroom { Id = 4, Name = "Class 4" },
            new Classroom { Id = 5, Name = "Class 5" }
        );

        var students = new List<Student>();
        int studentId = 1;
        for (int classId = 1; classId <= 5; classId++)
        {
            for (int i = 1; i <= 4; i++)
            {
                students.Add(new Student
                {
                    Id = studentId,
                    Name = $"Student {studentId}",
                    ClassroomId = classId
                });
                studentId++;
            }
        }

        modelBuilder.Entity<Student>().HasData(students);
    }
}
