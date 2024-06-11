namespace StoryTelling.DAL.Project;

public class ProjectSettings
{
    public int Id { get; set; }
    public int FontSize { get; set; }
    public int NodesCounter { get; set; }
    public ProjectNode? RootNode { get; set; }
}