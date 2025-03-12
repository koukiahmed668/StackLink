using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Models;

public class Profile
{
    [Key, ForeignKey("User")]
    public Guid UserId { get; set; }

    [Required, MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    public string? Bio { get; set; }

    // New Fields for Networking
    public string? Location { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? GitHubUrl { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Property
    public User User { get; set; } = null!;
}
