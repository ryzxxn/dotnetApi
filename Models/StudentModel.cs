namespace dotnetApi.Models
{
    public class Student
    {
        public Guid? StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? Class { get; set; }
        public int? RollNumber { get; set; }
        public int? Age { get; set; }
        public int? Marks { get; set; }
    }
}
