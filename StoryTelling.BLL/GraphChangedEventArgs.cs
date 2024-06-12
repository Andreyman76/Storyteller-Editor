using System.Drawing;

namespace StoryTelling.BLL;

public class GraphChangedEventArgs : EventArgs
{
    public required Bitmap Image { get; set; }
}