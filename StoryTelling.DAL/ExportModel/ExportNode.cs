using System.ComponentModel.DataAnnotations;

namespace StoryTelling.DAL.ExportModel;

public class ExportNode
{
    public int Id { get; set; }

    [StringLength(50)]
    public required string Name { get; set; }
    public string Text { get; set; } = string.Empty;
    public byte[]? Image { get; set; }
    public bool IsRoot { get; set; }
}