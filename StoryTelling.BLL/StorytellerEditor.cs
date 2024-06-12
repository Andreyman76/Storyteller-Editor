using Microsoft.EntityFrameworkCore;
using StoryTelling.BLL.Entities;
using StoryTelling.DAL.ExportModel;
using StoryTelling.DAL.ProjectModel;
using System.Drawing;

namespace StoryTelling.BLL;

public class StorytellerEditor(string dbFileName)
{
    public Node? TransitionFrom { get; set; }
    public Size GraphImageSize { get; set; }
    public bool HasRoot => string.IsNullOrWhiteSpace(_rootNodeName) == false;
    public bool HasSelectedNode => _selectedNode != null;
    public string SelectedNodeName => _selectedNode?.Name ?? string.Empty;

    public event EventHandler<GraphChangedEventArgs>? GraphChanged;
    public event EventHandler<SelectedTransitionsChangedEventArgs>? SelectedTransitionsChanged;
    public event EventHandler<SelectedNodeChangedEventArgs>? SelectedNodeChanged;

    private readonly string _dbFileName = dbFileName;
    private readonly List<Node> _nodes = [];
    private readonly List<Transition> _transitions = [];

    private Node? _selectedNode;
    private int _nodesCounter;
    private Node? _grabbed;
    private Point _pivotOffset = new();
    private int _fontSize = 16;
    private Point _offsetStart = new();
    private bool _changingOffset;
    private Font _currentFont = new("Arial", 16);
    private string? _rootNodeName;

    public void CreateNewProject()
    {
        File.Delete(_dbFileName);

        using var context = new ProjectContext(_dbFileName);
        context.Database.EnsureCreated();
        context.SaveChanges();
    }

    public void OpenProject(string fileName)
    {
        File.Copy(fileName, _dbFileName, true);

        using var context = new ProjectContext(_dbFileName);
        var settings = context.Settings.Include(x => x.RootNode).First();

        _rootNodeName = settings.RootNode?.Name;
        _fontSize = settings.FontSize;
        _nodesCounter = settings.NodesCounter;
        _currentFont = new Font("Arial", _fontSize);

        _nodes.Clear();

        var nodes = context.Nodes.ToArray();

        foreach (var node in nodes)
        {
            _nodes.Add(node);
        }

        UpdateTransitions();

        _selectedNode = null;
        DisplayNode(null);
        DrawNodes();
    }

    public void SaveProject(string fileName)
    {
        using var context = new ProjectContext(_dbFileName);
        var settings = context.Settings.ToArray();
        context.RemoveRange(settings);

        var rootNode = context.Nodes.Where(x => x.Name == _rootNodeName).FirstOrDefault();

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

        File.Copy(_dbFileName, fileName, true);
    }

    public void Export(string fileName)
    {
        using var projectContext = new ProjectContext(_dbFileName);

        var settings = projectContext.Settings.
            AsNoTracking().
            Include(x => x.RootNode).
            First();

        var nodes = projectContext.Nodes.
            AsNoTracking().
            ToArray();

        var transitions = projectContext.Transitions.
            AsNoTracking().
            Include(x => x.From).
            Include(x => x.To).
        ToArray();

        File.Delete(fileName);
        using var exportContext = new ExportContext(fileName);
        exportContext.Database.EnsureCreated();

        var exportNodes = new List<ExportNode>();
        var exportTransitions = new List<ExportTransition>();

        foreach (var transition in transitions)
        {
            if (exportNodes.Any(x => x.Id == transition.From.Id) == false)
            {
                exportNodes.Add(new ExportNode
                {
                    Id = transition.From.Id,
                    Name = transition.From.Name,
                    Text = transition.From.Text,
                    Image = transition.From.Image,
                    IsRoot = transition.From.Id == settings.RootNode!.Id
                });
            }

            if (exportNodes.Any(x => x.Id == transition.To.Id) == false)
            {
                exportNodes.Add(new ExportNode
                {
                    Id = transition.To.Id,
                    Name = transition.To.Name,
                    Text = transition.To.Text,
                    Image = transition.To.Image,
                    IsRoot = transition.To.Id == settings.RootNode!.Id,
                });
            }

            exportContext.Transitions.Add(new ExportTransition
            {
                Id = transition.Id,
                Name = transition.Name,
                From = exportNodes.First(x => x.Id == transition.From.Id),
                To = exportNodes.First(x => x.Id == transition.To.Id)
            });
        }

        exportContext.SaveChanges();
    }

    public void AddNewNode()
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
            _rootNodeName = node.Name;
        }

        _nodes.Add(node);
        DrawNodes();

        using var context = new ProjectContext(_dbFileName);

        var n = new ProjectNode
        {
            Name = node.Name
        };

        context.Nodes.Add(n);
        context.SaveChanges();
    }

    public bool ChangeSelectedNodeName(string name)
    {
        if (_selectedNode == null)
        {
            return true;
        }

        if (_nodes.Find(x => x.Name == name) != null)
        {
            return false;
        }

        using var context = new ProjectContext(_dbFileName);
        var node = context.Nodes.Where(x => x.Name == _selectedNode.Name).First();
        node.Name = name;
        context.SaveChanges();

        if (_selectedNode.Name == _rootNodeName)
        {
            _rootNodeName = name;
        }

        _selectedNode.Name = name;

        UpdateTransitions();
        DrawNodes();
        DisplayNode(_selectedNode);

        return true;
    }

    public void ChangeSelectedNodeText(string text)
    {
        if (_selectedNode == null)
        {
            return;
        }

        using var context = new ProjectContext(_dbFileName);

        var node = context.Nodes.Where(x => x.Name == _selectedNode.Name).First();
        node.Text = text;
        context.SaveChanges();
    }

    public void ChangeSelectedNodeImage(Image? img)
    {
        if (_selectedNode == null)
        {
            return;
        }

        var blob = img?.ToBytes();
        using var context = new ProjectContext(_dbFileName);

        var node = context.Nodes.Where(x => x.Name == _selectedNode.Name).First();
        node.Image = blob;
        context.SaveChanges();

        _selectedNode = node;

        DisplayNode(_selectedNode);
    }

    public void MarkSelectedNodeAsRoot()
    {
        if (_selectedNode == null)
        {
            return;
        }

        _rootNodeName = _selectedNode.Name;
        DrawNodes();
    }

    public void RemoveSelectedNode()
    {
        if (_selectedNode == null)
        {
            return;
        }

        using var context = new ProjectContext(_dbFileName);
        var node = context.Nodes.Where(x => x.Name == _selectedNode.Name).First();
        context.Nodes.Remove(node);
        context.SaveChanges();

        if (_selectedNode.Name == _rootNodeName)
        {
            _rootNodeName = null;
        }

        _nodes.Remove(_selectedNode);
        _selectedNode = null;

        UpdateTransitions();
        DisplayNode(null);
        DrawNodes();
    }

    public bool AddNewTransition(string name, Node transitionTo)
    {
        if (TransitionFrom == null)
        {
            return true;
        }

        if (_transitions.Find(x => x.From == TransitionFrom.Name && x.Name == name) != null)
        {
            return false;
        }

        using var context = new ProjectContext(_dbFileName);

        var firstNode = context.Nodes.Where(x => x.Name == TransitionFrom.Name).First();
        var secondNode = context.Nodes.Where(x => x.Name == transitionTo.Name).First();

        var transition = new ProjectTransition
        {
            From = firstNode,
            To = secondNode,
            Name = name
        };

        context.Transitions.Add(transition);
        context.SaveChanges();

        UpdateTransitions();
        DisplayTransitions(_selectedNode);

        return true;
    }

    public bool ChangeTransitionName(Transition transition, string name)
    {
        if (_transitions.Find(x => x.From == transition.From && x.Name == name) != null)
        {
            return false;
        }

        using var context = new ProjectContext(_dbFileName);
        var t = context.Transitions.Where(x => x.Name == transition.Name).First();
        t.Name = name;
        context.SaveChanges();

        UpdateTransitions();
        DisplayTransitions(_selectedNode);

        return true;
    }

    public void RemoveTransition(Transition transition)
    {
        using var context = new ProjectContext(_dbFileName);
        var t = context.Transitions.Where(x => x.Name == transition.Name).First();
        context.Remove(t);
        context.SaveChanges();

        UpdateTransitions();
        DisplayTransitions(_selectedNode);
        DrawNodes();
    }

    public void GrabNode(Point position)
    {
        _grabbed = FindNode(position);

        if (_selectedNode != _grabbed)
        {
            _selectedNode = _grabbed;
            DisplayNode(_selectedNode);
        }

        if (_grabbed != null)
        {
            _pivotOffset = new Point(position.X - _grabbed.Position.X, position.Y - _grabbed.Position.Y);
        }
    }

    public void UngrabNode()
    {
        _grabbed = null;
    }

    public void StartMoveGraph(Point position)
    {
        _changingOffset = true;
        _offsetStart = position;
    }

    public void StopMoveGraph()
    {
        _changingOffset = false;
    }

    public void MovePointer(Point position, bool moveGraph)
    {
        if (moveGraph)
        {
            if (_changingOffset)
            {
                var p = new Point(position.X - _offsetStart.X, position.Y - _offsetStart.Y);
                _offsetStart = position;

                foreach (var node in _nodes)
                {
                    node.Position = new Point(node.Position.X + p.X, node.Position.Y + p.Y);
                }
            }
        }

        if (_grabbed != null)
        {
            _grabbed.Position = new Point(position.X - _pivotOffset.X, position.Y - _pivotOffset.Y);
        }

        DrawNodes(TransitionFrom != null ? position : null);
    }

    public void ChangeCurrentFontSize(int delta)
    {
        _fontSize += delta;

        if (_fontSize < 1)
        {
            _fontSize = 1;
        }

        _currentFont = new Font("Arial", _fontSize);

        DrawNodes();
    }

    public void RefereshImage()
    {
        DrawNodes();
    }

    public Node? FindNode(Point position)
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

    private void DisplayNode(Node? node)
    {
        if (node == null)
        {
            SelectedNodeChanged?.Invoke(this,
                new SelectedNodeChangedEventArgs
                {
                    SelectedNode = null
                });

            return;
        }

        using var context = new ProjectContext(_dbFileName);
        var n = context.Nodes.AsNoTracking().Where(x => x.Name == node.Name).First();

        SelectedNodeChanged?.Invoke(this, new SelectedNodeChangedEventArgs
        {
            SelectedNode = n
        });

        DisplayTransitions(node);
    }

    private void DrawNodes(Point? position = null)
    {
        var img = new Bitmap(GraphImageSize.Width, GraphImageSize.Height);
        using var g = Graphics.FromImage(img);

        foreach (var node in _nodes)
        {
            var isRoot = string.IsNullOrWhiteSpace(_rootNodeName) == false && node.Name == _rootNodeName;

            g.DrawNode(node, _currentFont, isRoot);
        }

        foreach (var transition in _transitions)
        {
            var from = _nodes.Find(x => x.Name == transition.From);
            var to = _nodes.Find(x => x.Name == transition.To);

            if (from != null && to != null)
            {
                g.DrawArrow(from.GetPointOnBorder(to.Center()), to.GetPointOnBorder(from.Center()));
            }
        }

        if (position != null)
        {
            g.DrawArrow(TransitionFrom!.GetPointOnBorder(position.Value), position.Value);
        }

        GraphChanged?.Invoke(this, new GraphChangedEventArgs { Image = img });
    }

    private void UpdateTransitions()
    {
        _transitions.Clear();
        using var context = new ProjectContext(_dbFileName);

        var transitions = context.Transitions.
            AsNoTracking().
            Include(x => x.From).
            Include(x => x.To).
            ToArray();

        foreach (var transition in transitions)
        {
            _transitions.Add(transition);
        }
    }

    private void DisplayTransitions(Node? node)
    {
        var transitions = _transitions.FindAll(x => x.From == node?.Name);

        SelectedTransitionsChanged?.Invoke(this,
            new SelectedTransitionsChangedEventArgs
            {
                Transitions = transitions
            });
    }
}