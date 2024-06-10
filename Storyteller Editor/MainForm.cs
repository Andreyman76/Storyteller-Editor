using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using StoryTelling.DAL;
using StoryTelling.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;


namespace StoryTelling.Editor;

public partial class MainForm : Form
{
    private readonly string _dbFileName = "Latest project.stp";
    private readonly Image _blankImage = new Bitmap(100, 100);
    private readonly List<Node> _nodes = [];
    private int _nodesCounter = 0;
    private Node _selected = null;
    private Node _grabbed = null;
    private Point _pivotOffset;
    private Node _transitionFrom = null;
    private readonly List<Transition> _transitions = [];
    private int _fontSize = 16;
    private Point _offsetStart = new();
    private bool _changingOffset = false;

    public MainForm()
    {
        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);
        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);
        InitializeComponent();
    }

    private void Form1Load(object sender, EventArgs e)
    {
        File.Delete(_dbFileName);
        storyGroup.Visible = false;

        using var context = new StoryContext(_dbFileName);
        context.Database.EnsureCreated();
        context.SaveChanges();
    }

    private static void DrawArrow(PointF from, PointF to, Graphics g)
    {
        g.DrawLine(Pens.Red, from, to);
        var vect = new PointF(to.X - from.X, to.Y - from.Y);
        var length = (float)Math.Sqrt(vect.X * vect.X + vect.Y * vect.Y);
        vect = new PointF(vect.X / length, vect.Y / length);
        var normal = new PointF(vect.Y * 5, -vect.X * 5);

        var pos = new PointF(to.X - vect.X * 15, to.Y - vect.Y * 15);
        g.DrawLine(Pens.Red, to, new PointF(pos.X - normal.X, pos.Y - normal.Y));
        g.DrawLine(Pens.Red, to, new PointF(pos.X + normal.X, pos.Y + normal.Y));
    }

    private void DrawNodes()
    {
        Image img = new Bitmap(graphPicture.Width, graphPicture.Height);
        var g = Graphics.FromImage(img);

        foreach (var node in _nodes)
        {
            node.Draw(g);
        }

        foreach (var transition in _transitions)
        {
            var from = _nodes.Find(x => x.Name == transition.From);
            var to = _nodes.Find(x => x.Name == transition.To);

            DrawArrow(from.GetBorderPoint(to.Center()), to.GetBorderPoint(from.Center()), g);
        }

        graphPicture.Image = img;
    }

    public void UpdateTransitions()
    {
        _transitions.Clear();
        using var context = new StoryContext(_dbFileName);
        var transitions = context.Transitions.Include(x => x.From).Include(x => x.To).ToArray();

        foreach (var transition in transitions)
        {
            _transitions.Add(transition);
        }
    }

    private static bool ValidateStringLength(string str, int maxLength)
    {
        if (str.Length > maxLength)
        {
            MessageBox.Show(MyStrings.TooLong);
            return false;
        }

        return true;
    }

    private void CreateNewToolStripMenuItemClick(object sender, EventArgs e)
    {
        string id;

        do
        {
            id = $"story{_nodesCounter++}";
        }
        while (_nodes.Find(x => x.Name == id) != null);

        var node = new Node(id, new Point(0, 0));

        if (_nodesCounter == 1)
        {
            Node.Root = node.Name;
        }

        _nodes.Add(node);
        DrawNodes();

        using var context = new StoryContext(_dbFileName);

        var n = new StoryNode
        {
            Name = node.Name
        };

        context.Nodes.Add(n);
        context.SaveChanges();
    }

    private void Form1FormClosing(object sender, FormClosingEventArgs e)
    {
        if (MessageBox.Show(MyStrings.SaveBeforeClosing, MyStrings.Confirm, MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
            SaveProjectToolStripMenuItemClick(sender, e);
        }
    }

    private Node GetNode(Point position)
    {
        foreach (var node in _nodes)
        {
            if (node.Intersects(position))
            {
                return node;
            }
        }

        return null;
    }

    private void DisplayTransitions(Node node)
    {
        var transitions = _transitions.FindAll(x => x.From == node.Name);

        transitionsList.Items.Clear();

        foreach (var transition in transitions)
        {
            transitionsList.Items.Add(transition);
        }
    }

    private void DisplayNode(Node node)
    {
        if (node == null)
        {
            storyGroup.Visible = false;
            return;
        }

        idLabel.Text = node.Name;
        transitionsList.Items.Clear();

        using var context = new StoryContext(_dbFileName);
        var n = context.Nodes.AsNoTracking().Where(x => x.Name == node.Name).First();

        textBox.Text = n.Text;

        if (n.Image == null)
        {
            previewPicture.Image = _blankImage;
        }
        else
        {
            previewPicture.Image = BytesToImage(n.Image);
        }

        DisplayTransitions(node);

        storyGroup.Visible = true;
    }

    private void GraphPictureMouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            _grabbed = GetNode(e.Location);

            if (_selected != _grabbed)
            {
                _selected = _grabbed;
                DisplayNode(_selected);
            }

            if (_grabbed != null)
            {
                _pivotOffset = new Point(e.X - _grabbed.Position.X, e.Y - _grabbed.Position.Y);
            }

            return;
        }

        if (e.Button == MouseButtons.Middle)
        {
            _changingOffset = true;
            _offsetStart = e.Location;
            return;
        }

        if (e.Button == MouseButtons.Right)
        {
            if (_transitionFrom == null)
            {
                _transitionFrom = GetNode(e.Location);
            }
            else
            {
                var transitionTo = GetNode(e.Location);

                if (transitionTo != null)
                {
                    var name = Interaction.InputBox(MyStrings.SetTransitionName.Replace("@from", _transitionFrom.Name).Replace("@to", transitionTo.Name));

                    if (!ValidateStringLength(name, 50))
                    {
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(name) == false)
                    {
                        if (_transitions.Find(x => x.From == _transitionFrom.Name && x.Name == name) != null)
                        {
                            MessageBox.Show(MyStrings.TransitionExists.Replace("@id", _transitionFrom.Name));
                            return;
                        }

                        using var context = new StoryContext(_dbFileName);

                        var firstNode = context.Nodes.Where(x => x.Name == _transitionFrom.Name).First();
                        var secondNode = context.Nodes.Where(x => x.Name == transitionTo.Name).First();

                        var transition = new StoryTransition
                        {
                            From = firstNode,
                            To = secondNode,
                            Name = name
                        };

                        context.Transitions.Add(transition);
                        context.SaveChanges();

                        UpdateTransitions();
                        DisplayTransitions(_selected);
                    }
                }

                _transitionFrom = null;
            }

            DrawNodes();
        }
    }

    private void GraphPictureMouseUp(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            _grabbed = null;
            return;
        }

        if (e.Button == MouseButtons.Middle)
        {
            _changingOffset = false;
            return;
        }
    }

    private void GraphPictureMouseMove(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Middle)
        {
            if (_changingOffset)
            {
                var p = new Point(e.X - _offsetStart.X, e.Y - _offsetStart.Y);
                _offsetStart = e.Location;

                foreach (var node in _nodes)
                {
                    node.Position = new Point(node.Position.X + p.X, node.Position.Y + p.Y);
                }

                DrawNodes();
            }
        }

        if (_grabbed != null)
        {
            _grabbed.Position = new Point(e.X - _pivotOffset.X, e.Y - _pivotOffset.Y);
            DrawNodes();
        }

        if (_transitionFrom != null)
        {
            DrawNodes();
            DrawArrow(_transitionFrom.GetBorderPoint(e.Location), e.Location, Graphics.FromImage(graphPicture.Image));
        }
    }

    private void TextBoxTextChanged(object sender, EventArgs e)
    {
        using var context = new StoryContext(_dbFileName);

        var node = context.Nodes.Where(x => x.Name == _selected.Name).First();
        node.Text = textBox.Text;
        context.SaveChanges();
    }

    private void ChangeIdButtonClick(object sender, EventArgs e)
    {
        var newName = Interaction.InputBox(MyStrings.EnterNewStoryId.Replace("@id", _selected.Name));

        if (!ValidateStringLength(newName, 50))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(newName) == false)
        {
            if (_nodes.Find(x => x.Name == newName) != null)
            {
                MessageBox.Show(MyStrings.StoryExists.Replace("@id", newName));
                return;
            }

            using var context = new StoryContext(_dbFileName);
            var node = context.Nodes.Where(x => x.Name == _selected.Name).First();
            node.Name = newName;
            context.SaveChanges();

            if (_selected.Name == Node.Root)
            {
                Node.Root = newName;
            }

            _selected.Name = newName;
            idLabel.Text = newName;

            UpdateTransitions();
            DrawNodes();
        }
    }

    private static byte[] ImageToBytes(Image image)
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

    private static Bitmap BytesToImage(byte[] imageBytes)
    {
        using var ms = new MemoryStream(imageBytes);
        return new Bitmap(ms);
    }

    private void SetImageToSelectedNode(Image img)
    {
        var blob = ImageToBytes(img);
        using var context = new StoryContext(_dbFileName);

        var node = context.Nodes.Where(x => x.Name == _selected.Name).First();
        node.Image = blob;
        context.SaveChanges();

        _selected = node;

        if (node.Image == null)
        {
            previewPicture.Image = _blankImage;
        }
        else
        {
            previewPicture.Image = BytesToImage(node.Image);
        }
    }

    private void SelectImageButtonClick(object sender, EventArgs e)
    {
        selectImageFileDialog.FileName = "";
        selectImageFileDialog.Filter = MyStrings.ImageFiles;
        selectImageFileDialog.Title = MyStrings.SelectImage;

        if (selectImageFileDialog.ShowDialog() == DialogResult.OK)
        {
            var stream = File.OpenRead(selectImageFileDialog.FileName);
            var img = Image.FromStream(stream);
            SetImageToSelectedNode(img);

            stream.Close();
            stream.Dispose();
        }
    }

    private void PasteFromClipboardButtonClick(object sender, EventArgs e)
    {
        var img = Clipboard.GetImage();

        if (img != null)
        {
            SetImageToSelectedNode(img);
        }
    }

    private void RemoveImageButtonClick(object sender, EventArgs e)
    {
        SetImageToSelectedNode(null);
    }

    private void MarkAsRootButtonClick(object sender, EventArgs e)
    {
        Node.Root = _selected.Name;
        DrawNodes();
    }

    private void RemoveSelectedToolStripMenuItemClick(object sender, EventArgs e)
    {
        using var context = new StoryContext(_dbFileName);
        var node = context.Nodes.Where(x => x.Name == _selected.Name).First();
        context.Nodes.Remove(node);
        context.SaveChanges();

        if (_selected.Name == Node.Root)
        {
            Node.Root = null;
        }

        _nodes.Remove(_selected);
        _selected = null;

        UpdateTransitions();
        DisplayNode(null);
        DrawNodes();
    }

    private void GraphPictureMouseWheel(object sender, MouseEventArgs e)
    {
        _fontSize += e.Delta > 0 ? 1 : -1;

        if (_fontSize < 1)
        {
            _fontSize = 1;
        }

        Node.CurrentFont = new Font("Arial", _fontSize);

        DrawNodes();
    }

    private void ChangeTransitionNameButtonClick(object sender, EventArgs e)
    {
        var index = transitionsList.SelectedIndex;

        if (index < 0)
        {
            MessageBox.Show(MyStrings.NeedSelectTransition);
            return;
        }

        var transition = transitionsList.Items[index] as Transition;
        var newName = Interaction.InputBox(MyStrings.NewTransitionName.Replace("@transition", transition.ToString()));

        if (!ValidateStringLength(newName, 50))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(newName) == false)
        {
            if (_transitions.Find(x => x.From == transition.From && x.Name == newName) != null)
            {
                MessageBox.Show(MyStrings.TransitionExists.Replace("@id", transition.From));
                return;
            }

            using var context = new StoryContext(_dbFileName);
            var t = context.Transitions.Where(x => x.Name == transition.Name).First();
            t.Name = newName;
            context.SaveChanges();

            UpdateTransitions();
            DisplayTransitions(_selected);
        }
    }

    private void RemoveTransitionButtonClick(object sender, EventArgs e)
    {
        var index = transitionsList.SelectedIndex;

        if (index < 0)
        {
            MessageBox.Show(MyStrings.NeedSelectTransition);
            return;
        }

        var transition = transitionsList.Items[index] as Transition;

        using var context = new StoryContext(_dbFileName);
        var t = context.Transitions.Where(x => x.Name == transition.Name).First();
        context.Remove(t);
        context.SaveChanges();

        UpdateTransitions();
        DisplayTransitions(_selected);
        DrawNodes();
    }

    private void SaveProjectToolStripMenuItemClick(object sender, EventArgs e)
    {
        saveProjectFileDialog.Title = MyStrings.SaveProject;
        saveProjectFileDialog.DefaultExt = "stp";
        saveProjectFileDialog.Filter = MyStrings.ProjectFile;

        if (saveProjectFileDialog.ShowDialog() == DialogResult.OK)
        {
            using var context = new StoryContext(_dbFileName);
            var settings = context.ProjectSettings.ToArray();
            context.RemoveRange(settings);

            var rootNode = context.Nodes.Where(x => x.Name == Node.Root).FirstOrDefault();

            context.Add(new ProjectSettings
            {
                RootNode = rootNode,
                FontSize = _fontSize,
                NodesCounter = _nodesCounter
            });

            foreach (var node in _nodes)
            {
                var n = context.Nodes.Where(x => x.Name == node.Name).First();
                n.X = node.Position.X;
                n.Y = node.Position.Y;
            }

            context.SaveChanges();

            File.Copy(_dbFileName, saveProjectFileDialog.FileName, true);

            MessageBox.Show(MyStrings.ProjectSaved);
        }
    }

    private void OpenProjectToolStripMenuItemClick(object sender, EventArgs e)
    {
        openProjectFileDialog.Title = MyStrings.OpenProject;
        openProjectFileDialog.FileName = string.Empty;
        openProjectFileDialog.Filter = MyStrings.ProjectFile;

        if (openProjectFileDialog.ShowDialog() == DialogResult.OK)
        {
            File.Copy(openProjectFileDialog.FileName, _dbFileName, true);

            using var context = new StoryContext(_dbFileName);
            var settings = context.ProjectSettings.Include(x => x.RootNode).First();

            Node.Root = settings.RootNode?.Name;
            _fontSize = settings.FontSize;
            _nodesCounter = settings.NodesCounter;
            Node.CurrentFont = new Font("Arial", _fontSize);

            _nodes.Clear();

            var nodes = context.Nodes.ToArray();

            foreach (var node in nodes)
            {
                _nodes.Add(node);
            }

            UpdateTransitions();

            _selected = null;
            DisplayNode(null);
            DrawNodes();
        }
    }

    private void ExportToolStripMenuItemClick(object sender, EventArgs e)
    {
        if (Node.Root == null)
        {
            MessageBox.Show(MyStrings.NoRoot);
            return;
        }

        exportFileDialog.Title = MyStrings.Export;
        exportFileDialog.DefaultExt = "story";
        exportFileDialog.Filter = MyStrings.ExportFile;

        if (exportFileDialog.ShowDialog() == DialogResult.OK)
        {
            File.Copy(_dbFileName, exportFileDialog.FileName, true);
            MessageBox.Show(MyStrings.Exported);
        }
    }

    private void ChangeLanguage(string language)
    {
        if (language != Properties.Settings.Default.Language)
        {
            Properties.Settings.Default.Language = language;
            Properties.Settings.Default.Save();
            Close();
        }
    }

    private void EnglishToolStripMenuItemClick(object sender, EventArgs e)
    {
        ChangeLanguage("en-US");
    }

    private void RussianToolStripMenuItemClick(object sender, EventArgs e)
    {
        ChangeLanguage("ru-RU");
    }
}