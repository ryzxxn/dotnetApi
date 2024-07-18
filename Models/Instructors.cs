public class Instructor
{
    public int InstructorID { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Status { get; set; }
    public string? HireDate { get; set; }
    public int AnnualSalary { get; set; }
    public Department? Department { get; set; }
    public ICollection<Department>? Departments { get; set; }
    public ICollection<Course>? Courses { get; set; }
    public ICollection<Student>? Students { get; set; }

}