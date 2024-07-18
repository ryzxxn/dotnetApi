public class Course
{
    public int CourseID { get; set; }
    public string? CourseName { get; set; }
    public string? CourseDescription { get; set; }
    public int CourseUnits { get; set; }
    public int DepartmentID { get; set; }
    public int InstructorID { get; set; }
    public Department? Department { get; set; }
    public Instructor? Instructor { get; set; }
    public ICollection<StudentCourse>? StudentCourses { get; set; }
}