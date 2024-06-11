
namespace StoryTelling.Editor;

partial class MainForm
{
    /// <summary>
    /// Обязательная переменная конструктора.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Освободить все используемые ресурсы.
    /// </summary>
    /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Код, автоматически созданный конструктором форм Windows

    /// <summary>
    /// Требуемый метод для поддержки конструктора — не изменяйте 
    /// содержимое этого метода с помощью редактора кода.
    /// </summary>
    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        graphPicture = new System.Windows.Forms.PictureBox();
        menuStrip1 = new System.Windows.Forms.MenuStrip();
        fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        saveProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        openProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        storyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        createNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        removeSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        languageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        russianToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        graphGroup = new System.Windows.Forms.GroupBox();
        storyGroup = new System.Windows.Forms.GroupBox();
        tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
        removeTransitionButton = new System.Windows.Forms.Button();
        markAsRootButton = new System.Windows.Forms.Button();
        changeTransitionNameButton = new System.Windows.Forms.Button();
        pasteFromClipboardButton = new System.Windows.Forms.Button();
        transitionsList = new System.Windows.Forms.ListBox();
        label3 = new System.Windows.Forms.Label();
        changeIdButton = new System.Windows.Forms.Button();
        idLabel = new System.Windows.Forms.Label();
        removeImageButton = new System.Windows.Forms.Button();
        previewPicture = new System.Windows.Forms.PictureBox();
        label2 = new System.Windows.Forms.Label();
        textBox = new System.Windows.Forms.RichTextBox();
        selectImageButton = new System.Windows.Forms.Button();
        selectImageFileDialog = new System.Windows.Forms.OpenFileDialog();
        saveProjectFileDialog = new System.Windows.Forms.SaveFileDialog();
        openProjectFileDialog = new System.Windows.Forms.OpenFileDialog();
        exportFileDialog = new System.Windows.Forms.SaveFileDialog();
        tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
        ((System.ComponentModel.ISupportInitialize)graphPicture).BeginInit();
        menuStrip1.SuspendLayout();
        graphGroup.SuspendLayout();
        storyGroup.SuspendLayout();
        tableLayoutPanel2.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)previewPicture).BeginInit();
        tableLayoutPanel1.SuspendLayout();
        SuspendLayout();
        // 
        // graphPicture
        // 
        graphPicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        resources.ApplyResources(graphPicture, "graphPicture");
        graphPicture.Name = "graphPicture";
        graphPicture.TabStop = false;
        graphPicture.MouseDown += OnGraphPictureMouseDown;
        graphPicture.MouseMove += OnGraphPictureMouseMove;
        graphPicture.MouseUp += OnGraphPictureMouseUp;
        graphPicture.MouseWheel += OnGraphPictureMouseWheel;
        // 
        // menuStrip1
        // 
        menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
        menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem, storyToolStripMenuItem, languageToolStripMenuItem });
        resources.ApplyResources(menuStrip1, "menuStrip1");
        menuStrip1.Name = "menuStrip1";
        // 
        // fileToolStripMenuItem
        // 
        fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { saveProjectToolStripMenuItem, openProjectToolStripMenuItem, exportToolStripMenuItem });
        fileToolStripMenuItem.Name = "fileToolStripMenuItem";
        resources.ApplyResources(fileToolStripMenuItem, "fileToolStripMenuItem");
        // 
        // saveProjectToolStripMenuItem
        // 
        saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
        resources.ApplyResources(saveProjectToolStripMenuItem, "saveProjectToolStripMenuItem");
        saveProjectToolStripMenuItem.Click += OnSaveProjectToolStripMenuItemClick;
        // 
        // openProjectToolStripMenuItem
        // 
        openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
        resources.ApplyResources(openProjectToolStripMenuItem, "openProjectToolStripMenuItem");
        openProjectToolStripMenuItem.Click += OnOpenProjectToolStripMenuItemClick;
        // 
        // exportToolStripMenuItem
        // 
        exportToolStripMenuItem.Name = "exportToolStripMenuItem";
        resources.ApplyResources(exportToolStripMenuItem, "exportToolStripMenuItem");
        exportToolStripMenuItem.Click += OnExportToolStripMenuItemClick;
        // 
        // storyToolStripMenuItem
        // 
        storyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { createNewToolStripMenuItem, removeSelectedToolStripMenuItem });
        storyToolStripMenuItem.Name = "storyToolStripMenuItem";
        resources.ApplyResources(storyToolStripMenuItem, "storyToolStripMenuItem");
        // 
        // createNewToolStripMenuItem
        // 
        createNewToolStripMenuItem.Name = "createNewToolStripMenuItem";
        resources.ApplyResources(createNewToolStripMenuItem, "createNewToolStripMenuItem");
        createNewToolStripMenuItem.Click += OnCreateNewToolStripMenuItemClick;
        // 
        // removeSelectedToolStripMenuItem
        // 
        removeSelectedToolStripMenuItem.Name = "removeSelectedToolStripMenuItem";
        resources.ApplyResources(removeSelectedToolStripMenuItem, "removeSelectedToolStripMenuItem");
        removeSelectedToolStripMenuItem.Click += OnRemoveSelectedToolStripMenuItemClick;
        // 
        // languageToolStripMenuItem
        // 
        languageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { englishToolStripMenuItem, russianToolStripMenuItem });
        languageToolStripMenuItem.Name = "languageToolStripMenuItem";
        resources.ApplyResources(languageToolStripMenuItem, "languageToolStripMenuItem");
        // 
        // englishToolStripMenuItem
        // 
        englishToolStripMenuItem.Name = "englishToolStripMenuItem";
        resources.ApplyResources(englishToolStripMenuItem, "englishToolStripMenuItem");
        englishToolStripMenuItem.Click += OnEnglishToolStripMenuItemClick;
        // 
        // russianToolStripMenuItem
        // 
        russianToolStripMenuItem.Name = "russianToolStripMenuItem";
        resources.ApplyResources(russianToolStripMenuItem, "russianToolStripMenuItem");
        russianToolStripMenuItem.Click += OnRussianToolStripMenuItemClick;
        // 
        // graphGroup
        // 
        graphGroup.Controls.Add(graphPicture);
        resources.ApplyResources(graphGroup, "graphGroup");
        graphGroup.Name = "graphGroup";
        graphGroup.TabStop = false;
        // 
        // storyGroup
        // 
        storyGroup.Controls.Add(tableLayoutPanel2);
        resources.ApplyResources(storyGroup, "storyGroup");
        storyGroup.Name = "storyGroup";
        storyGroup.TabStop = false;
        // 
        // tableLayoutPanel2
        // 
        resources.ApplyResources(tableLayoutPanel2, "tableLayoutPanel2");
        tableLayoutPanel2.Controls.Add(removeTransitionButton, 4, 9);
        tableLayoutPanel2.Controls.Add(markAsRootButton, 4, 4);
        tableLayoutPanel2.Controls.Add(changeTransitionNameButton, 4, 8);
        tableLayoutPanel2.Controls.Add(pasteFromClipboardButton, 0, 5);
        tableLayoutPanel2.Controls.Add(transitionsList, 0, 8);
        tableLayoutPanel2.Controls.Add(label3, 0, 7);
        tableLayoutPanel2.Controls.Add(changeIdButton, 0, 1);
        tableLayoutPanel2.Controls.Add(idLabel, 0, 0);
        tableLayoutPanel2.Controls.Add(removeImageButton, 0, 6);
        tableLayoutPanel2.Controls.Add(previewPicture, 2, 4);
        tableLayoutPanel2.Controls.Add(label2, 0, 2);
        tableLayoutPanel2.Controls.Add(textBox, 0, 3);
        tableLayoutPanel2.Controls.Add(selectImageButton, 0, 4);
        tableLayoutPanel2.Name = "tableLayoutPanel2";
        // 
        // removeTransitionButton
        // 
        resources.ApplyResources(removeTransitionButton, "removeTransitionButton");
        removeTransitionButton.Name = "removeTransitionButton";
        removeTransitionButton.UseVisualStyleBackColor = true;
        removeTransitionButton.Click += OnRemoveTransitionButtonClick;
        // 
        // markAsRootButton
        // 
        resources.ApplyResources(markAsRootButton, "markAsRootButton");
        markAsRootButton.Name = "markAsRootButton";
        markAsRootButton.UseVisualStyleBackColor = true;
        markAsRootButton.Click += OnMarkAsRootButtonClick;
        // 
        // changeTransitionNameButton
        // 
        resources.ApplyResources(changeTransitionNameButton, "changeTransitionNameButton");
        changeTransitionNameButton.Name = "changeTransitionNameButton";
        changeTransitionNameButton.UseVisualStyleBackColor = true;
        changeTransitionNameButton.Click += OnChangeTransitionNameButtonClick;
        // 
        // pasteFromClipboardButton
        // 
        resources.ApplyResources(pasteFromClipboardButton, "pasteFromClipboardButton");
        pasteFromClipboardButton.Name = "pasteFromClipboardButton";
        pasteFromClipboardButton.UseVisualStyleBackColor = true;
        pasteFromClipboardButton.Click += OnPasteFromClipboardButtonClick;
        // 
        // transitionsList
        // 
        tableLayoutPanel2.SetColumnSpan(transitionsList, 3);
        resources.ApplyResources(transitionsList, "transitionsList");
        transitionsList.FormattingEnabled = true;
        transitionsList.Name = "transitionsList";
        tableLayoutPanel2.SetRowSpan(transitionsList, 2);
        // 
        // label3
        // 
        resources.ApplyResources(label3, "label3");
        label3.Name = "label3";
        // 
        // changeIdButton
        // 
        resources.ApplyResources(changeIdButton, "changeIdButton");
        changeIdButton.Name = "changeIdButton";
        changeIdButton.UseVisualStyleBackColor = true;
        changeIdButton.Click += OnChangeIdButtonClick;
        // 
        // idLabel
        // 
        resources.ApplyResources(idLabel, "idLabel");
        tableLayoutPanel2.SetColumnSpan(idLabel, 5);
        idLabel.Name = "idLabel";
        // 
        // removeImageButton
        // 
        resources.ApplyResources(removeImageButton, "removeImageButton");
        removeImageButton.Name = "removeImageButton";
        removeImageButton.UseVisualStyleBackColor = true;
        removeImageButton.Click += OnRemoveImageButtonClick;
        // 
        // previewPicture
        // 
        previewPicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        resources.ApplyResources(previewPicture, "previewPicture");
        previewPicture.Name = "previewPicture";
        tableLayoutPanel2.SetRowSpan(previewPicture, 3);
        previewPicture.TabStop = false;
        // 
        // label2
        // 
        resources.ApplyResources(label2, "label2");
        label2.Name = "label2";
        // 
        // textBox
        // 
        tableLayoutPanel2.SetColumnSpan(textBox, 5);
        resources.ApplyResources(textBox, "textBox");
        textBox.Name = "textBox";
        textBox.TextChanged += OnTextBoxTextChanged;
        // 
        // selectImageButton
        // 
        resources.ApplyResources(selectImageButton, "selectImageButton");
        selectImageButton.Name = "selectImageButton";
        selectImageButton.UseVisualStyleBackColor = true;
        selectImageButton.Click += OnSelectImageButtonClick;
        // 
        // selectImageFileDialog
        // 
        selectImageFileDialog.FileName = "openFileDialog1";
        // 
        // openProjectFileDialog
        // 
        openProjectFileDialog.FileName = "openFileDialog2";
        // 
        // tableLayoutPanel1
        // 
        resources.ApplyResources(tableLayoutPanel1, "tableLayoutPanel1");
        tableLayoutPanel1.Controls.Add(graphGroup, 0, 0);
        tableLayoutPanel1.Controls.Add(storyGroup, 1, 0);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        // 
        // MainForm
        // 
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
        resources.ApplyResources(this, "$this");
        Controls.Add(tableLayoutPanel1);
        Controls.Add(menuStrip1);
        MainMenuStrip = menuStrip1;
        Name = "MainForm";
        FormClosing += OnFormClosing;
        Load += OnFormLoad;
        ((System.ComponentModel.ISupportInitialize)graphPicture).EndInit();
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        graphGroup.ResumeLayout(false);
        storyGroup.ResumeLayout(false);
        tableLayoutPanel2.ResumeLayout(false);
        tableLayoutPanel2.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)previewPicture).EndInit();
        tableLayoutPanel1.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.PictureBox graphPicture;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.GroupBox graphGroup;
    private System.Windows.Forms.GroupBox storyGroup;
    private System.Windows.Forms.RichTextBox textBox;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label idLabel;
    private System.Windows.Forms.ListBox transitionsList;
    private System.Windows.Forms.PictureBox previewPicture;
    private System.Windows.Forms.Button selectImageButton;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.OpenFileDialog selectImageFileDialog;
    private System.Windows.Forms.Button removeImageButton;
    private System.Windows.Forms.Button removeTransitionButton;
    private System.Windows.Forms.Button changeTransitionNameButton;
    private System.Windows.Forms.Button markAsRootButton;
    private System.Windows.Forms.SaveFileDialog saveProjectFileDialog;
    private System.Windows.Forms.OpenFileDialog openProjectFileDialog;
    private System.Windows.Forms.SaveFileDialog exportFileDialog;
    private System.Windows.Forms.Button pasteFromClipboardButton;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveProjectToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem openProjectToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem storyToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem createNewToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem removeSelectedToolStripMenuItem;
    private System.Windows.Forms.Button changeIdButton;
    private System.Windows.Forms.ToolStripMenuItem languageToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem englishToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem russianToolStripMenuItem;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
}

