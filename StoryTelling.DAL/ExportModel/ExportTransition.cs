using System.ComponentModel.DataAnnotations;

namespace StoryTelling.DAL.ExportModel;

public class ExportTransition
{
    public int Id { get; set; }

    [StringLength(50)]
    public required string Name { get; set; }
    public required ExportNode From { get; set; }
    public required ExportNode To { get; set; }
}
