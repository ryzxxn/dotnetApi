using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserID { get; set; }

    [Required]
    [StringLength(100)]
    public required string Username { get; set; }

    [Required]
    [StringLength(100)]
    public required string Password { get; set; }

    [Required]
    [StringLength(100)]
    public required string Email { get; set; }

    [Required]
    public required bool IsActive {get; set;}

    [Required]
    [StringLength(100)]
    public required string Status { get; set; }

    [Required]
    [StringLength(20)]
    public required string CreatedDate { get; set; }

    [Required]
    [StringLength(20)]
    public required string ModifiedDate { get; set; }

    public required virtual ICollection<UserUserRole> UserUserRoles { get; set; }
}