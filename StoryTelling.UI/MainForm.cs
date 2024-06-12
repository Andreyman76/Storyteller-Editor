using Microsoft.VisualBasic;
using StoryTelling.BLL;
using StoryTelling.BLL.Entities;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace StoryTelling.UI;

public partial class MainForm : Form
{
    private readonly StorytellerEditor _editor = new("Latest project.stp");
    private readonly Image _blankImage = new Bitmap(100, 100);

    public MainForm()
    {
        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);
        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);
        InitializeComponent();
        WindowState = FormWindowState.Maximized;

        _editor.GraphImageSize = graphPicture.Size;
        _editor.GraphChanged += OnGraphChanged;
        _editor.SelectedTransitionsChanged += OnSelectedTransitionsChanged;
        _editor.SelectedNodeChanged += OnSelectedNodeChanged;
    }

    private void OnSelectedNodeChanged(object? sender, SelectedNodeChangedEventArgs e)
    {
        if (e.SelectedNode == null)
        {
            storyGroup.Visible = false;
            return;
        }

        idLabel.Text = "Id: " + e.SelectedNode.Name;
        textBox.Text = e.SelectedNode.Text;
        previewPicture.Image = e.SelectedNode.Image?.CreateImage() ?? _blankImage;

        storyGroup.Visible = true;
    }

    private void OnSelectedTransitionsChanged(object? sender, SelectedTransitionsChangedEventArgs e)
    {
        transitionsList.DataSource = e.Transitions;
    }

    private void OnGraphChanged(object? sender, GraphChangedEventArgs e)
    {
        graphPicture.Image = e.Image;
    }

    private void OnFormLoad(object sender, EventArgs e)
    {
        storyGroup.Visible = false;
        _editor.CreateNewProject();
    }

    private void OnCreateNewToolStripMenuItemClick(object sender, EventArgs e)
    {
        _editor.AddNewNode();
    }

    private void OnFormClosing(object sender, FormClosingEventArgs e)
    {
        var result = MessageBox.Show(
            MyStrings.SaveBeforeClosing,
            MyStrings.Confirm,
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Information
            );

        if (result == DialogResult.Yes)
        {
            SaveProject();
        }
        else if (result == DialogResult.Cancel)
        {
            e.Cancel = true;
        }
    }

    private void OnGraphPictureMouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            _editor.GrabNode(e.Location);
            return;
        }

        if (e.Button == MouseButtons.Middle)
        {
            _editor.StartMoveGraph(e.Location);
            return;
        }

        if (e.Button == MouseButtons.Right)
        {
            if (_editor.TransitionFrom == null)
            {
                _editor.TransitionFrom = _editor.FindNode(e.Location);
            }
            else
            {
                var transitionTo = _editor.FindNode(e.Location);

                if (transitionTo != null)
                {
                    var name = Interaction.InputBox(MyStrings.SetTransitionName.Replace("@from", _editor.TransitionFrom.Name).Replace("@to", transitionTo.Name), "Storyteller Editor");

                    if (ValidateStringLength(name, 50) == false)
                    {
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(name) == false)
                    {
                        if (_editor.AddNewTransition(name, transitionTo) == false)
                        {
                            MessageBox.Show(
                                MyStrings.TransitionExists.Replace("@id", _editor.TransitionFrom.Name),
                                MyStrings.Error,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                                );
                        }
                    }
                }

                _editor.TransitionFrom = null;
            }

            _editor.RefereshImage();
        }
    }

    private void OnGraphPictureMouseUp(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            _editor.UngrabNode();
        }
        else if (e.Button == MouseButtons.Middle)
        {
            _editor.StopMoveGraph();
        }
    }

    private void OnGraphPictureMouseMove(object sender, MouseEventArgs e)
    {
        _editor.MovePointer(e.Location, e.Button == MouseButtons.Middle);
    }

    private void OnTextBoxTextChanged(object sender, EventArgs e)
    {
        _editor.ChangeSelectedNodeText(textBox.Text);
    }

    private void OnChangeIdButtonClick(object sender, EventArgs e)
    {
        if (_editor.HasSelectedNode == false)
        {
            return;
        }

        var newName = Interaction.InputBox(MyStrings.EnterNewStoryId.Replace("@id", _editor.SelectedNodeName), "Storyteller Editor");

        if (ValidateStringLength(newName, 50) == false)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(newName) == false)
        {
            if (_editor.ChangeSelectedNodeName(newName) == false)
            {
                MessageBox.Show(
                    MyStrings.StoryExists.Replace("@id", newName),
                    MyStrings.Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
            }
        }
    }

    private void OnSelectImageButtonClick(object sender, EventArgs e)
    {
        selectImageFileDialog.Filter = MyStrings.ImageFiles;
        selectImageFileDialog.Title = MyStrings.SelectImage;
        selectImageFileDialog.FileName = string.Empty;

        if (selectImageFileDialog.ShowDialog() == DialogResult.OK)
        {
            using var stream = File.OpenRead(selectImageFileDialog.FileName);
            var img = Image.FromStream(stream);
            _editor.ChangeSelectedNodeImage(img);
        }
    }

    private void OnPasteFromClipboardButtonClick(object sender, EventArgs e)
    {
        var img = Clipboard.GetImage();

        if (img != null)
        {
            _editor.ChangeSelectedNodeImage(img);
        }
    }

    private void OnRemoveImageButtonClick(object sender, EventArgs e)
    {
        _editor.ChangeSelectedNodeImage(null);
    }

    private void OnMarkAsRootButtonClick(object sender, EventArgs e)
    {
        _editor.MarkSelectedNodeAsRoot();
    }

    private void OnRemoveSelectedToolStripMenuItemClick(object sender, EventArgs e)
    {
        _editor.RemoveSelectedNode();
    }

    private void OnGraphPictureMouseWheel(object sender, MouseEventArgs e)
    {
        _editor.ChangeCurrentFontSize(e.Delta > 0 ? 1 : -1);
    }

    private void OnChangeTransitionNameButtonClick(object sender, EventArgs e)
    {
        var index = transitionsList.SelectedIndex;

        if (index < 0)
        {
            MessageBox.Show(
                MyStrings.NeedSelectTransition,
                MyStrings.Info,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
                );
            return;
        }

        var transition = transitionsList.Items[index] as Transition ?? throw new Exception("Convert to Transition failed");
        var newName = Interaction.InputBox(MyStrings.NewTransitionName.Replace("@transition", transition?.ToString()), "Storyteller Editor");

        if (ValidateStringLength(newName, 50) == false)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(newName) == false)
        {
            if (_editor.ChangeTransitionName(transition!, newName) == false)
            {
                MessageBox.Show(
                    MyStrings.TransitionExists.Replace("@id", transition?.From),
                    MyStrings.Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
            }
        }
    }

    private void OnRemoveTransitionButtonClick(object sender, EventArgs e)
    {
        var index = transitionsList.SelectedIndex;

        if (index < 0)
        {
            MessageBox.Show(
                MyStrings.NeedSelectTransition,
                MyStrings.Info,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
                );

            return;
        }

        var transition = transitionsList.Items[index] as Transition ?? throw new Exception("Convert to Transition failed");

        _editor.RemoveTransition(transition!);
    }

    private void OnSaveProjectToolStripMenuItemClick(object sender, EventArgs e)
    {
        SaveProject();
    }

    private void SaveProject()
    {
        saveProjectFileDialog.Title = MyStrings.SaveProject;
        saveProjectFileDialog.Filter = MyStrings.ProjectFile;
        saveProjectFileDialog.FileName = string.Empty;

        if (saveProjectFileDialog.ShowDialog() == DialogResult.OK)
        {
            _editor.SaveProject(saveProjectFileDialog.FileName);

            MessageBox.Show(
                MyStrings.ProjectSaved,
                MyStrings.Info,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
                );
        }
    }

    private void OnOpenProjectToolStripMenuItemClick(object sender, EventArgs e)
    {
        openProjectFileDialog.Title = MyStrings.OpenProject;
        openProjectFileDialog.Filter = MyStrings.ProjectFile;
        openProjectFileDialog.FileName = string.Empty;

        if (openProjectFileDialog.ShowDialog() == DialogResult.OK)
        {
            _editor.OpenProject(openProjectFileDialog.FileName);
        }
    }

    private void OnExportToolStripMenuItemClick(object sender, EventArgs e)
    {
        if (_editor.HasRoot == false)
        {
            MessageBox.Show(
                MyStrings.NoRoot,
                MyStrings.Error,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
                );

            return;
        }

        exportFileDialog.Title = MyStrings.Export;
        exportFileDialog.Filter = MyStrings.ExportFile;
        exportFileDialog.FileName = string.Empty;

        if (exportFileDialog.ShowDialog() == DialogResult.OK)
        {
            _editor.Export(exportFileDialog.FileName);

            MessageBox.Show(
                MyStrings.Exported,
                MyStrings.Info,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
                );
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

    private void OnEnglishToolStripMenuItemClick(object sender, EventArgs e)
    {
        ChangeLanguage("en-US");
    }

    private void OnRussianToolStripMenuItemClick(object sender, EventArgs e)
    {
        ChangeLanguage("ru-RU");
    }

    private void OnGraphPictureSizeChanged(object sender, EventArgs e)
    {
        _editor.GraphImageSize = graphPicture.Size;
    }

    private static bool ValidateStringLength(string str, int maxLength)
    {
        if (str.Length > maxLength)
        {
            MessageBox.Show(
                MyStrings.TooLong,
                MyStrings.Warning,
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
                );

            return false;
        }

        return true;
    }
}