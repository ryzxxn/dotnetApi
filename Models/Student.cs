using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Student
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int StudentID { get; set; }

    [Required]
    [StringLength(100)]
    public required string FirstName { get; set; }

    [Required]
    [StringLength(100)]
    public required string LastName { get; set; }

    [Required]
    [StringLength(20)]
    public required string EnrollmentDate { get; set; }

    [Required]
    [StringLength(20)]
    public required string GraduationDate { get; set; }

    public required virtual ICollection<StudentCourse> StudentCourses { get; set; }
}