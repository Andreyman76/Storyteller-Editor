using System.ComponentModel.DataAnnotations;

namespace StoryTelling.DAL;

public class StoryNode
{
    public int Id { get; set; }

    [StringLength(50)]
    public required string Name { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public string Text { get; set; } = string.Empty;
    public byte[]? Image { get; set; }
}