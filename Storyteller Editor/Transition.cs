
namespace Storyteller_Editor
{
    public class Transition
    {
        public string Name { get; }
        public string From { get; }
        public string To { get; }
        public int Id { get; }

        public Transition(int id, string name, string from, string to)
        {
            Id = id;
            Name = name;
            From = from;
            To = to;
        }

        public override string ToString()
        {
            return $"\"{Name}\" -> \"{To}\"";
        }
    }
}
