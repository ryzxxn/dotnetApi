using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Course
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required int CourseID { get; set; }

    [Required]
    [StringLength(100)]
    public required string CourseName { get; set; }

    [Required]
    [StringLength(100)]
    public required string CourseDescription { get; set; }

    [Required]
    public required int CourseUnits { get; set; }

    [Required]
    [ForeignKey("Department")]
    public required int DepartmentID { get; set; }

    [Required]
    [ForeignKey("Instructor")]
    public required int InstructorID { get; set; }

    // Navigation properties for Department and Instructor
    public virtual Department? Department { get; set; }
    public virtual Instructor? Instructor { get; set; }
}