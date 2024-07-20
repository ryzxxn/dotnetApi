using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class StudentCourse
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CID { get; set; }

    [Required]
    [ForeignKey("Student")]
    public required int StudentID { get; set; }

    [Required]
    [ForeignKey("Course")]
    public required int CourseID { get; set; }

    public required virtual Student Student { get; set; }
    public required virtual Course Course { get; set; }
}
