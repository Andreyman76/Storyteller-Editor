namespace StoryTelling.DAL;

public class ProjectSettings
{
    public int Id { get; set; }
    public int FontSize { get; set; }
    public int NodesCounter { get; set; }
    public StoryNode? RootNode { get; set; }
}