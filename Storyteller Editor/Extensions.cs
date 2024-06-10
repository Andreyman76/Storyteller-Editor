using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace StoryTelling.Editor;

public static class Extensions
{
    public static void DrawArrow(this Graphics graphics, PointF from, PointF to, int arrowWidth = 5, int arrowLength = 15)
    {
        graphics.DrawLine(Pens.Red, from, to);
        var vector = new PointF(to.X - from.X, to.Y - from.Y);
        var length = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        vector = new PointF(vector.X / length, vector.Y / length);
        var normal = new PointF(vector.Y * arrowWidth, -vector.X * arrowWidth);

        var position = new PointF(to.X - vector.X * arrowLength, to.Y - vector.Y * arrowLength);
        graphics.DrawLine(Pens.Red, to, new PointF(position.X - normal.X, position.Y - normal.Y));
        graphics.DrawLine(Pens.Red, to, new PointF(position.X + normal.X, position.Y + normal.Y));
    }

    public static byte[]? ToBytes(this Image? image)
    {
        if (image == null)
        {
            return null;
        }

        using var ms = new MemoryStream();
        var format = image.RawFormat;

        if (format.Equals(ImageFormat.MemoryBmp))
        {
            format = ImageFormat.Png;
        }

        image.Save(ms, format);
        var imageBytes = ms.ToArray();

        return imageBytes;
    }

    public static Image? CreateImage(this byte[]? imageBytes)
    {
        if (imageBytes == null)
        {
            return null;
        }

        using var ms = new MemoryStream(imageBytes);
        return new Bitmap(ms);
    }
}