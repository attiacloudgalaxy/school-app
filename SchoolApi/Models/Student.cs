using System.Text.Json.Serialization;

namespace SchoolApi.Models;

public class Student
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public int ClassroomId { get; set; }
    
    [JsonIgnore]
    public Classroom? Classroom { get; set; }
}
