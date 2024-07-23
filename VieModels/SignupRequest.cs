using System.ComponentModel.DataAnnotations;

public class SignupRequest
{
    [Required]
    [StringLength(100)]
    public required string Email { get; set; }

    [Required]
    [StringLength(100)]
    public required string Password { get; set; }
}
