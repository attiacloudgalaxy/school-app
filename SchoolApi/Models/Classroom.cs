namespace SchoolApi.Models;

public class Classroom
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<Student> Students { get; set; } = new();
}
