using System.Drawing;


namespace Storyteller_Editor
{
    public class Node
    {
        public static Font CurrentFont { get; set; } = new Font("Arial", 16);
        public static string Root { get; set; } = null;
        public Image Preview { get; private set; } = null;
        public string Id { get; set; }
        public Point Position { get; set; }

        private RectangleF _rect;

        public Node(string id, Point position)
        {
            Id = id;
            Position = position;
        }

        public void Draw(Graphics g)
        {
            SizeF size = g.MeasureString(Id, CurrentFont);

            if (Root != null && Id == Root)
            {
                g.FillRectangle(Brushes.Green, Position.X, Position.Y, size.Width, size.Height);
            }

            g.DrawString(Id, CurrentFont, Brushes.Black, Position);
            g.DrawRectangle(Pens.Black, Position.X, Position.Y, size.Width, size.Height);
            _rect = new RectangleF(Position.X, Position.Y, size.Width, size.Height);
        }

        public PointF Center()
        {
            return new PointF((_rect.X * 2 + _rect.Width) / 2, (_rect.Y * 2 + _rect.Height) / 2);
        }

        public PointF GetBorderPoint(PointF from)
        {
            var center = Center();

            var p1 = new PointF();
            var p2 = new PointF();

            if (from.Y < _rect.Top)
            {
                p1 = new PointF(_rect.Left, _rect.Top);
                p2 = new PointF(_rect.Right, _rect.Top);

            }
            else if (from.Y > _rect.Bottom)
            {
                p1 = new PointF(_rect.Left, _rect.Bottom);
                p2 = new PointF(_rect.Right, _rect.Bottom);
            }

            var result = IntersectionPoint(p1, p2, from, center);

            if (result.X >= _rect.Left && result.X <= _rect.Right)
            {
                return result;
            }

            if (from.X < _rect.Left)
            {
                p1 = new PointF(_rect.Left, _rect.Top);
                p2 = new PointF(_rect.Left, _rect.Bottom);
            }
            else if (from.X > _rect.Right)
            {
                p1 = new PointF(_rect.Right, _rect.Top);
                p2 = new PointF(_rect.Right, _rect.Bottom);
            }

            result = IntersectionPoint(p1, p2, from, center);

            return result;
        }

        public bool Intersects(Point p)
        {
            return _rect.Contains(p);
        }

        public void SetPreview(Image img)
        {
            Image preview = null;

            if (img != null)
            {
                preview = new Bitmap(100, 100);
                var g = Graphics.FromImage(preview);
                g.DrawImage(img, new Rectangle(0, 0, 100, 100), new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
            }

            Preview = preview;
        }

        private static PointF IntersectionPoint(PointF from1, PointF to1, PointF from2, PointF to2)
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
            var y = 0f;

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
}
