using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public string? TechStack { get; set; }
    public string? PortfolioUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relationships
    public Profile? Profile { get; set; }  // 1-to-1 with Profile
    public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
}
