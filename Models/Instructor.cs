using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Instructor
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required int InstructorID { get; set; }

    [Required]
    [StringLength(100)]
    public required string FirstName { get; set; }

    [Required]
    [StringLength(100)]
    public required string LastName { get; set; }

    [Required]
    [StringLength(25)]
    public required string Status { get; set; }

    [Required]
    [StringLength(20)]
    public required string HireDate { get; set; }

    [Required]
    public required int AnnualSalary { get; set; }

    [Required]
    [ForeignKey("Department")]
    public required int DepartmentID { get; set; }

    // Navigation property for Department
    public virtual Department? Department { get; set; }
}