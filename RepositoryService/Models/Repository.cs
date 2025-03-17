using System.ComponentModel.DataAnnotations;

namespace RepositoryService.Models;

public class Repository
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Owner { get; set; } = string.Empty;

    [Required]
    public string Url { get; set; } = string.Empty;

    public int Stars { get; set; }
    public string? Language { get; set; }
    public string? Description { get; set; }
}
