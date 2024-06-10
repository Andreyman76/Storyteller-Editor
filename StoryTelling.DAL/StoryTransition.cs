using System.ComponentModel.DataAnnotations;

namespace StoryTelling.DAL;

public class StoryTransition
{
    public int Id { get; set; }

    [StringLength(50)]
    public required string Name { get; set; }
    public required StoryNode From { get; set; }
    public required StoryNode To { get; set; }
}