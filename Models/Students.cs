public class Student
{
    public int StudentID { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? EnrollmentDate { get; set; }
    public string? GraduationDate { get; set; }
    public ICollection<StudentCourse>? StudentCourses { get; set; }
}