using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class UserRole
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserRoleID { get; set; }

    [Required]
    [StringLength(100)]
    public required string UserRoleName { get; set; }

    public required virtual ICollection<UserUserRole> UserUserRoles { get; set; }
}