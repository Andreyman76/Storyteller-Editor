using StoryTelling.DAL.ProjectModel;
namespace StoryTelling.BLL;

public class SelectedNodeChangedEventArgs
{
    public ProjectNode? SelectedNode { get; set; }
}