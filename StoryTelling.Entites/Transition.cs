using StoryTelling.DAL;

namespace StoryTelling.Entities;

public class Transition(string name, string from, string to)
{
    public string Name { get; } = name;
    public string From { get; } = from;
    public string To { get; } = to;

    public override string ToString()
    {
        return $"\"{Name}\" -> \"{To}\"";
    }

    public static implicit operator Transition(StoryTransition transition) 
    {
        return new Transition(transition.Name, transition.From.Name, transition.To.Name);
    }
}