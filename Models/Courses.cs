public class Course
{
    public int CourseID { get; set; }
    public required string CourseName { get; set; }
    public required string? CourseDescription { get; set; }
    public required int CourseUnits { get; set; }
    public required int DepartmentID { get; set; }
    public required int InstructorID { get; set; }
    public required Department? Department { get; set; }
    public required Instructor? Instructor { get; set; }
    public required ICollection<StudentCourse> StudentCourses { get; set; }
    public required ICollection<Instructor>? Instructors { get; set; }
    public required ICollection<Department>? Departments { get; set; }
}