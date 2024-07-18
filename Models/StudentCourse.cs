public class StudentCourse
{
    public required int StudentID { get; set; }
    public required Student Student { get; set; }

    public required int CourseID { get; set; }
    public required Course Course { get; set; }
}