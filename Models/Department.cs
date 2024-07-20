using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Department
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DepartmentID { get; set; }

    [Required]
    [StringLength(100)]
    public required string DepartmentName { get; set; }

    // Navigation properties for Course and Instructor
    public required virtual ICollection<Course> Courses { get; set; }
    public required virtual ICollection<Instructor> Instructors { get; set; }
}
