using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;


namespace Storyteller_Editor
{
    public partial class Form1 : Form
    {
        private string _dbFileName = "Latest project.stp";
        private SQLiteConnection _connection;
        private Image _blankImage = new Bitmap(100, 100);
        private List<Node> _nodes = new List<Node>();
        private int _nodesCounter = 0;
        private Node _selected = null;
        private Node _grabbed = null;
        private Point _pivotOffset;
        private Node _transitionFrom = null;
        private List<Transition> _transitions = new List<Transition>();
        private int _fontSize = 16;
        private Point _offsetStart = new Point();
        private bool _changingOffset = false;

        public Form1()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);
            InitializeComponent();
        }

        private void Form1Load(object sender, EventArgs e)
        {
            File.Delete(_dbFileName);
            storyGroup.Visible = false;
            _connection = new SQLiteConnection($"Data Source={_dbFileName}");
            _connection.Open();

            var createTables = _connection.CreateCommand();
            createTables.CommandText = "PRAGMA foreign_keys=on;" +
                                       "CREATE TABLE project_settings(" +
                                       "font_size INTEGER NOT NULL," +
                                       "nodes_counter INTEGER NOT NULL," +
                                       "root_node VARCHAR(50));" +
                                       "CREATE TABLE stories(" +
                                       "id VARCHAR(50) PRIMARY KEY NOT NULL," +
                                       "x INTEGER," +
                                       "y INTEGER," +
                                       "text TEXT NOT NULL," +
                                       "image BLOB);" +
                                       "CREATE TABLE transitions(" +
                                       "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                                       "name VARCHAR(50) NOT NULL," +
                                       "from_id VARCHAR(50) NOT NULL," +
                                       "to_id VARCHAR(50) NOT NULL," +
                                       "FOREIGN KEY (from_id) REFERENCES stories (id) ON UPDATE CASCADE ON DELETE CASCADE," +
                                       "FOREIGN KEY (to_id) REFERENCES stories (id) ON UPDATE CASCADE ON DELETE CASCADE);";

            createTables.ExecuteNonQuery();
        }

        private void DrawArrow(PointF from, PointF to, Graphics g)
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
                var from = _nodes.Find(x => x.Id == transition.From);
                var to = _nodes.Find(x => x.Id == transition.To);

                DrawArrow(from.GetBorderPoint(to.Center()), to.GetBorderPoint(from.Center()), g);
            }

            graphPicture.Image = img;
        }

        public void UpdateTransitions()
        {
            _transitions.Clear();
            var getTransitions = _connection.CreateCommand();
            getTransitions.CommandText = "SELECT * FROM transitions;";
            var reader = getTransitions.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var id = int.Parse(reader["id"].ToString());
                    var name = reader["name"].ToString();
                    var fromId = reader["from_id"].ToString();
                    var toId = reader["to_id"].ToString();
                    _transitions.Add(new Transition(id, name, fromId, toId));
                }
            }
        }

        private bool ValidateStringLength(string str, int maxLength)
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
            while (_nodes.Find(x => x.Id == id) != null);

            var node = new Node(id, new Point(0, 0));

            if (_nodesCounter == 1)
            {
                Node.Root = node.Id;
            }

            _nodes.Add(node);
            DrawNodes();

            var addStory = _connection.CreateCommand();
            addStory.CommandText = "INSERT INTO stories(id, text) values(@id, '');";
            addStory.Parameters.AddWithValue("id", node.Id);
            addStory.ExecuteNonQuery();
        }

        private void Form1FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show(MyStrings.SaveBeforeClosing, MyStrings.Confirm, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SaveProjectToolStripMenuItemClick(sender, e);
            }

            _connection.Close();
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
            var transitions = _transitions.FindAll(x => x.From == node.Id);

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

            idLabel.Text = node.Id;
            transitionsList.Items.Clear();

            var selectNode = _connection.CreateCommand();
            selectNode.CommandText = "SELECT text from stories WHERE id = @id;";
            selectNode.Parameters.AddWithValue("id", node.Id);
            var text = (string)selectNode.ExecuteScalar();
            textBox.Text = text;

            previewPicture.Refresh();
            previewPicture.Image = node.Preview ?? _blankImage;

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
                        var name = Interaction.InputBox(MyStrings.SetTransitionName.Replace("@from", _transitionFrom.Id).Replace("@to", transitionTo.Id));

                        if (!ValidateStringLength(name, 50))
                        {
                            return;
                        }

                        if (name != "")
                        {
                            if (_transitions.Find(x => x.From == _transitionFrom.Id && x.Name == name) != null)
                            {
                                MessageBox.Show(MyStrings.TransitionExists.Replace("@id", _transitionFrom.Id));
                                return;
                            }

                            var addTransition = _connection.CreateCommand();
                            addTransition.CommandText = "INSERT INTO transitions(name, from_id, to_id) VALUES(@name, @from_id, @to_id);";
                            addTransition.Parameters.AddWithValue("name", name);
                            addTransition.Parameters.AddWithValue("from_id", _transitionFrom.Id);
                            addTransition.Parameters.AddWithValue("to_id", transitionTo.Id);
                            addTransition.ExecuteNonQuery();

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
            var updateText = _connection.CreateCommand();
            updateText.CommandText = "UPDATE stories SET text = @text WHERE id = @id;";
            updateText.Parameters.AddWithValue("text", textBox.Text);
            updateText.Parameters.AddWithValue("id", _selected.Id);
            updateText.ExecuteNonQuery();
        }

        private void ChangeIdButtonClick(object sender, EventArgs e)
        {
            var newId = Interaction.InputBox(MyStrings.EnterNewStoryId.Replace("@id", _selected.Id));

            if (!ValidateStringLength(newId, 50))
            {
                return;
            }

            if (newId != "")
            {               
                if (_nodes.Find(x => x.Id == newId) != null)
                {
                    MessageBox.Show(MyStrings.StoryExists.Replace("@id", newId));
                    return;
                }

                var updateId = _connection.CreateCommand();
                updateId.CommandText = "UPDATE stories SET id = @new_id WHERE id = @old_id;";
                updateId.Parameters.AddWithValue("old_id", _selected.Id);
                updateId.Parameters.AddWithValue("new_id", newId);
                updateId.ExecuteNonQuery();

                if (_selected.Id == Node.Root)
                {
                    Node.Root = newId;
                }

                _selected.Id = newId;
                idLabel.Text = newId;

                UpdateTransitions();
                DrawNodes();
            }
        }

        private byte[] ImageToBytes(Image image)
        {
            if (image == null)
            {
                return null;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                var format = image.RawFormat;

                if(format.Equals(ImageFormat.MemoryBmp))
                {
                    format = ImageFormat.Png;
                }

                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                return imageBytes;
            }
        }

        private Image BytesToImage(byte[] imageBytes)
        {
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = new Bitmap(ms);

            return image;
        }

        private void SetImageToSelectedNode(Image img)
        {
            var blob = ImageToBytes(img);
            var setImage = _connection.CreateCommand();
            setImage.CommandText = "UPDATE stories SET image = @image WHERE id = @id;";
            setImage.Parameters.AddWithValue("id", _selected.Id);
            setImage.Parameters.AddWithValue("image", blob);
            setImage.ExecuteNonQuery();

            _selected.SetPreview(img);
            previewPicture.Image = _selected.Preview ?? _blankImage;
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
            Node.Root = _selected.Id;
            DrawNodes();
        }

        private void RemoveSelectedToolStripMenuItemClick(object sender, EventArgs e)
        {
            var remove = _connection.CreateCommand();
            remove.CommandText = "DELETE FROM stories WHERE id = @id;";
            remove.Parameters.AddWithValue("id", _selected.Id);
            remove.ExecuteNonQuery();

            if (_selected.Id == Node.Root)
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

            if (newName != "")
            {
                if (_transitions.Find(x => x.From == transition.From && x.Name == newName) != null)
                {
                    MessageBox.Show(MyStrings.TransitionExists.Replace("@id", transition.From));
                    return;
                }

                var changeName = _connection.CreateCommand();
                changeName.CommandText = "UPDATE transitions SET name = @name WHERE id = @id;";
                changeName.Parameters.AddWithValue("id", transition.Id);
                changeName.Parameters.AddWithValue("name", newName);
                changeName.ExecuteNonQuery();

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
            var removeTransition = _connection.CreateCommand();
            removeTransition.CommandText = "DELETE FROM transitions WHERE id = @id;";
            removeTransition.Parameters.AddWithValue("id", transition.Id);
            removeTransition.ExecuteNonQuery();

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
                var saveSettings = _connection.CreateCommand();
                saveSettings.CommandText = "DELETE FROM project_settings;" +
                                           "INSERT INTO project_settings(font_size, nodes_counter, root_node)" +
                                           "VALUES (@font_size, @nodes_counter, @root_node);";

                saveSettings.Parameters.AddWithValue("font_size", _fontSize);
                saveSettings.Parameters.AddWithValue("nodes_counter", _nodesCounter);
                saveSettings.Parameters.AddWithValue("root_node", Node.Root);

                saveSettings.ExecuteNonQuery();

                foreach (var node in _nodes)
                {
                    var addPosition = _connection.CreateCommand();
                    addPosition.CommandText = "UPDATE stories SET x = @x, y = @y WHERE id = @id;";
                    addPosition.Parameters.AddWithValue("id", node.Id);
                    addPosition.Parameters.AddWithValue("x", node.Position.X);
                    addPosition.Parameters.AddWithValue("y", node.Position.Y);
                    addPosition.ExecuteNonQuery();
                }

                _connection.Close();
                File.Copy(_dbFileName, saveProjectFileDialog.FileName, true);

                MessageBox.Show(MyStrings.ProjectSaved);
                _connection.Open();

                var foreignKeys = _connection.CreateCommand();
                foreignKeys.CommandText = "PRAGMA foreign_keys=on;";
                foreignKeys.ExecuteNonQuery();
            }
        }

        private void OpenProjectToolStripMenuItemClick(object sender, EventArgs e)
        {
            _connection.Close();

            openProjectFileDialog.Title = MyStrings.OpenProject;
            openProjectFileDialog.FileName = "";
            openProjectFileDialog.Filter = MyStrings.ProjectFile;

            if (openProjectFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.Copy(openProjectFileDialog.FileName, _dbFileName, true);

                _connection.Open();

                var getSettings = _connection.CreateCommand();
                getSettings.CommandText = "PRAGMA foreign_keys=on; SELECT * FROM project_settings;";
                var reader = getSettings.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    _fontSize = int.Parse(reader["font_size"].ToString());
                    Node.CurrentFont = new Font("Arial", _fontSize);
                    _nodesCounter = int.Parse(reader["nodes_counter"].ToString());
                    Node.Root = reader["root_node"].ToString();
                }

                _nodes.Clear();

                var getNodes = _connection.CreateCommand();
                getNodes.CommandText = "SELECT id, x, y, image FROM stories;";
                reader = getNodes.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var id = reader["id"].ToString();
                        var x = int.Parse(reader["x"].ToString());
                        var y = int.Parse(reader["y"].ToString());
                        Image image = null;

                        if (reader["image"].GetType() != typeof(DBNull))
                        {
                            image = BytesToImage((byte[])reader["image"]);
                        }

                        var node = new Node(id, new Point(x, y));
                        node.SetPreview(image);

                        _nodes.Add(node);
                    }
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
                File.Delete(exportFileDialog.FileName);

                var connection = new SQLiteConnection($"Data Source={exportFileDialog.FileName}");
                connection.Open();

                var createTables = connection.CreateCommand();
                createTables.CommandText = "CREATE TABLE metadata(" +
                                           "root_node VARCHAR(50) NOT NULL);" +
                                           "CREATE TABLE stories(" +
                                           "id VARCHAR(50) PRIMARY KEY NOT NULL," +
                                           "text TEXT NOT NULL," +
                                           "image BLOB);" +
                                           "CREATE TABLE transitions(" +
                                           "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                                           "name VARCHAR(50) NOT NULL," +
                                           "from_id VARCHAR(50) NOT NULL," +
                                           "to_id VARCHAR(50) NOT NULL);";

                createTables.ExecuteNonQuery();

                var metadata = connection.CreateCommand();
                metadata.CommandText = "INSERT INTO metadata VALUES(@root_node);";
                metadata.Parameters.AddWithValue("root_node", Node.Root);
                metadata.ExecuteNonQuery();

                var getStories = _connection.CreateCommand();
                getStories.CommandText = "SELECT id, text, image FROM stories;";
                var reader = getStories.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var id = reader["id"];
                        var text = reader["text"];
                        var image = reader["image"];

                        var insert = connection.CreateCommand();
                        insert.CommandText = "INSERT INTO stories VALUES(@id, @text, @image);";
                        insert.Parameters.AddWithValue("id", id);
                        insert.Parameters.AddWithValue("text", text);
                        insert.Parameters.AddWithValue("image", image);
                        insert.ExecuteNonQuery();
                    }
                }

                foreach (var transition in _transitions)
                {
                    var insert = connection.CreateCommand();
                    insert.CommandText = "INSERT INTO transitions(name, from_id, to_id) VALUES(@name, @from_id, @to_id);";
                    insert.Parameters.AddWithValue("name", transition.Name);
                    insert.Parameters.AddWithValue("from_id", transition.From);
                    insert.Parameters.AddWithValue("to_id", transition.To);
                    insert.ExecuteNonQuery();
                }

                connection.Close();
                connection.Dispose();

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
}
