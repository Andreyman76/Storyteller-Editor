using StoryTelling.BLL.Entities;

namespace StoryTelling.BLL;

public class SelectedTransitionsChangedEventArgs : EventArgs
{
    public required IEnumerable<Transition> Transitions { get; set; }
}