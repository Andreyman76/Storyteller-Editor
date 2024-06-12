using StoryTelling.BLL.Entities;

namespace StoryTelling.BLL;

public class TransitionsChangedEventArgs : EventArgs
{
    public required IEnumerable<Transition> Transitions { get; set; }
}