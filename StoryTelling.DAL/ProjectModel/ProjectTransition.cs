using System.ComponentModel.DataAnnotations;

namespace StoryTelling.DAL.ProjectModel;

public class ProjectTransition
{
    public int Id { get; set; }

    [StringLength(50)]
    public required string Name { get; set; }
    public required ProjectNode From { get; set; }
    public required ProjectNode To { get; set; }
}