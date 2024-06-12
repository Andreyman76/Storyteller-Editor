using StoryTelling.DAL.ProjectModel;
using System.Drawing;

namespace StoryTelling.BLL.Entities;

public class Node(string name, Point position)
{
    public string Name { get; set; } = name;
    public Point Position { get; set; } = position;
    public RectangleF Border { get; set; }

    public static implicit operator Node(ProjectNode node)
    {
        return new Node(node.Name, new Point(node.X, node.Y));
    }

    public PointF Center()
    {
        return new PointF((Border.X * 2 + Border.Width) / 2, (Border.Y * 2 + Border.Height) / 2);
    }

    public PointF GetPointOnBorder(PointF from)
    {
        var center = Center();

        var p1 = new PointF();
        var p2 = new PointF();

        if (from.Y < Border.Top)
        {
            p1 = new PointF(Border.Left, Border.Top);
            p2 = new PointF(Border.Right, Border.Top);

        }
        else if (from.Y > Border.Bottom)
        {
            p1 = new PointF(Border.Left, Border.Bottom);
            p2 = new PointF(Border.Right, Border.Bottom);
        }

        var result = GetIntersectionPoint(p1, p2, from, center);

        if (result.X >= Border.Left && result.X <= Border.Right)
        {
            return result;
        }

        if (from.X < Border.Left)
        {
            p1 = new PointF(Border.Left, Border.Top);
            p2 = new PointF(Border.Left, Border.Bottom);
        }
        else if (from.X > Border.Right)
        {
            p1 = new PointF(Border.Right, Border.Top);
            p2 = new PointF(Border.Right, Border.Bottom);
        }

        result = GetIntersectionPoint(p1, p2, from, center);

        return result;
    }

    public bool Intersects(Point p)
    {
        return Border.Contains(p);
    }

    private static PointF GetIntersectionPoint(PointF from1, PointF to1, PointF from2, PointF to2)
    {
        var x1 = from1.X;
        var y1 = from1.Y;
        var x2 = to1.X;
        var y2 = to1.Y;

        var x3 = from2.X;
        var y3 = from2.Y;
        var x4 = to2.X;
        var y4 = to2.Y;

        var x = ((x1 * y2 - x2 * y1) * (x4 - x3) - (x3 * y4 - x4 * y3) * (x2 - x1)) / ((y1 - y2) * (x4 - x3) - (y3 - y4) * (x2 - x1));
        float y;

        if (x3 == x4)
        {
            y = y1;
        }
        else
        {
            y = ((y3 - y4) * x - (x3 * y4 - x4 * y3)) / (x4 - x3);
        }

        return new PointF(-x, y);
    }
}