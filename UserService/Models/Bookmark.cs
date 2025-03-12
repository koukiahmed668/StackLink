using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Models;

public class Bookmark
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string RepositoryName { get; set; } = string.Empty;

    [Required]
    public string RepositoryUrl { get; set; } = string.Empty;

    [ForeignKey("User")]
    public Guid UserId { get; set; }

    // Navigation Property
    public User User { get; set; } = null!;
}
