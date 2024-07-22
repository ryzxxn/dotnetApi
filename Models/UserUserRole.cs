using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class UserUserRole
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserUserRoletID { get; set; }

    [Required]
    [ForeignKey("User")]
    public required int UserID { get; set; }

    [Required]
    [ForeignKey("UserRole")]
    public required int UserRoleID { get; set; }

    public required virtual User User { get; set; }
    public required virtual UserRole UserRole { get; set; }
}