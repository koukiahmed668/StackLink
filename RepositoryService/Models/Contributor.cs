using System.ComponentModel.DataAnnotations;

namespace RepositoryService.Models;

public class Contributor
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string ProfileUrl { get; set; } = string.Empty;

    [Required]
    public Guid RepositoryId { get; set; }
}
