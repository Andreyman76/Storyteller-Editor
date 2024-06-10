
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
        this.graphPicture = new System.Windows.Forms.PictureBox();
        this.menuStrip1 = new System.Windows.Forms.MenuStrip();
        this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.saveProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.openProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.storyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.createNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.removeSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.languageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.russianToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.graphGroup = new System.Windows.Forms.GroupBox();
        this.storyGroup = new System.Windows.Forms.GroupBox();
        this.changeIdButton = new System.Windows.Forms.Button();
        this.idLabel = new System.Windows.Forms.Label();
        this.pasteFromClipboardButton = new System.Windows.Forms.Button();
        this.markAsRootButton = new System.Windows.Forms.Button();
        this.removeTransitionButton = new System.Windows.Forms.Button();
        this.changeTransitionNameButton = new System.Windows.Forms.Button();
        this.removeImageButton = new System.Windows.Forms.Button();
        this.label3 = new System.Windows.Forms.Label();
        this.transitionsList = new System.Windows.Forms.ListBox();
        this.previewPicture = new System.Windows.Forms.PictureBox();
        this.selectImageButton = new System.Windows.Forms.Button();
        this.textBox = new System.Windows.Forms.RichTextBox();
        this.label2 = new System.Windows.Forms.Label();
        this.label1 = new System.Windows.Forms.Label();
        this.selectImageFileDialog = new System.Windows.Forms.OpenFileDialog();
        this.saveProjectFileDialog = new System.Windows.Forms.SaveFileDialog();
        this.openProjectFileDialog = new System.Windows.Forms.OpenFileDialog();
        this.exportFileDialog = new System.Windows.Forms.SaveFileDialog();
        ((System.ComponentModel.ISupportInitialize)(this.graphPicture)).BeginInit();
        this.menuStrip1.SuspendLayout();
        this.graphGroup.SuspendLayout();
        this.storyGroup.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.previewPicture)).BeginInit();
        this.SuspendLayout();
        // 
        // graphPicture
        // 
        this.graphPicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        resources.ApplyResources(this.graphPicture, "graphPicture");
        this.graphPicture.Name = "graphPicture";
        this.graphPicture.TabStop = false;
        this.graphPicture.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnGraphPictureMouseDown);
        this.graphPicture.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnGraphPictureMouseMove);
        this.graphPicture.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnGraphPictureMouseUp);
        this.graphPicture.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.OnGraphPictureMouseWheel);
        // 
        // menuStrip1
        // 
        this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
        this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        this.fileToolStripMenuItem,
        this.storyToolStripMenuItem,
        this.languageToolStripMenuItem});
        resources.ApplyResources(this.menuStrip1, "menuStrip1");
        this.menuStrip1.Name = "menuStrip1";
        // 
        // fileToolStripMenuItem
        // 
        this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
        this.saveProjectToolStripMenuItem,
        this.openProjectToolStripMenuItem,
        this.exportToolStripMenuItem});
        this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
        resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
        // 
        // saveProjectToolStripMenuItem
        // 
        this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
        resources.ApplyResources(this.saveProjectToolStripMenuItem, "saveProjectToolStripMenuItem");
        this.saveProjectToolStripMenuItem.Click += new System.EventHandler(this.OnSaveProjectToolStripMenuItemClick);
        // 
        // openProjectToolStripMenuItem
        // 
        this.openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
        resources.ApplyResources(this.openProjectToolStripMenuItem, "openProjectToolStripMenuItem");
        this.openProjectToolStripMenuItem.Click += new System.EventHandler(this.OnOpenProjectToolStripMenuItemClick);
        // 
        // exportToolStripMenuItem
        // 
        this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
        resources.ApplyResources(this.exportToolStripMenuItem, "exportToolStripMenuItem");
        this.exportToolStripMenuItem.Click += new System.EventHandler(this.OnExportToolStripMenuItemClick);
        // 
        // storyToolStripMenuItem
        // 
        this.storyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
        this.createNewToolStripMenuItem,
        this.removeSelectedToolStripMenuItem});
        this.storyToolStripMenuItem.Name = "storyToolStripMenuItem";
        resources.ApplyResources(this.storyToolStripMenuItem, "storyToolStripMenuItem");
        // 
        // createNewToolStripMenuItem
        // 
        this.createNewToolStripMenuItem.Name = "createNewToolStripMenuItem";
        resources.ApplyResources(this.createNewToolStripMenuItem, "createNewToolStripMenuItem");
        this.createNewToolStripMenuItem.Click += new System.EventHandler(this.OnCreateNewToolStripMenuItemClick);
        // 
        // removeSelectedToolStripMenuItem
        // 
        this.removeSelectedToolStripMenuItem.Name = "removeSelectedToolStripMenuItem";
        resources.ApplyResources(this.removeSelectedToolStripMenuItem, "removeSelectedToolStripMenuItem");
        this.removeSelectedToolStripMenuItem.Click += new System.EventHandler(this.OnRemoveSelectedToolStripMenuItemClick);
        // 
        // languageToolStripMenuItem
        // 
        this.languageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
        this.englishToolStripMenuItem,
        this.russianToolStripMenuItem});
        this.languageToolStripMenuItem.Name = "languageToolStripMenuItem";
        resources.ApplyResources(this.languageToolStripMenuItem, "languageToolStripMenuItem");
        // 
        // englishToolStripMenuItem
        // 
        this.englishToolStripMenuItem.Name = "englishToolStripMenuItem";
        resources.ApplyResources(this.englishToolStripMenuItem, "englishToolStripMenuItem");
        this.englishToolStripMenuItem.Click += new System.EventHandler(this.OnEnglishToolStripMenuItemClick);
        // 
        // russianToolStripMenuItem
        // 
        this.russianToolStripMenuItem.Name = "russianToolStripMenuItem";
        resources.ApplyResources(this.russianToolStripMenuItem, "russianToolStripMenuItem");
        this.russianToolStripMenuItem.Click += new System.EventHandler(this.OnRussianToolStripMenuItemClick);
        // 
        // graphGroup
        // 
        this.graphGroup.Controls.Add(this.graphPicture);
        resources.ApplyResources(this.graphGroup, "graphGroup");
        this.graphGroup.Name = "graphGroup";
        this.graphGroup.TabStop = false;
        // 
        // storyGroup
        // 
        this.storyGroup.Controls.Add(this.changeIdButton);
        this.storyGroup.Controls.Add(this.idLabel);
        this.storyGroup.Controls.Add(this.pasteFromClipboardButton);
        this.storyGroup.Controls.Add(this.markAsRootButton);
        this.storyGroup.Controls.Add(this.removeTransitionButton);
        this.storyGroup.Controls.Add(this.changeTransitionNameButton);
        this.storyGroup.Controls.Add(this.removeImageButton);
        this.storyGroup.Controls.Add(this.label3);
        this.storyGroup.Controls.Add(this.transitionsList);
        this.storyGroup.Controls.Add(this.previewPicture);
        this.storyGroup.Controls.Add(this.selectImageButton);
        this.storyGroup.Controls.Add(this.textBox);
        this.storyGroup.Controls.Add(this.label2);
        this.storyGroup.Controls.Add(this.label1);
        resources.ApplyResources(this.storyGroup, "storyGroup");
        this.storyGroup.Name = "storyGroup";
        this.storyGroup.TabStop = false;
        // 
        // changeIdButton
        // 
        resources.ApplyResources(this.changeIdButton, "changeIdButton");
        this.changeIdButton.Name = "changeIdButton";
        this.changeIdButton.UseVisualStyleBackColor = true;
        this.changeIdButton.Click += new System.EventHandler(this.OnChangeIdButtonClick);
        // 
        // idLabel
        // 
        resources.ApplyResources(this.idLabel, "idLabel");
        this.idLabel.Name = "idLabel";
        // 
        // pasteFromClipboardButton
        // 
        resources.ApplyResources(this.pasteFromClipboardButton, "pasteFromClipboardButton");
        this.pasteFromClipboardButton.Name = "pasteFromClipboardButton";
        this.pasteFromClipboardButton.UseVisualStyleBackColor = true;
        this.pasteFromClipboardButton.Click += new System.EventHandler(this.OnPasteFromClipboardButtonClick);
        // 
        // markAsRootButton
        // 
        resources.ApplyResources(this.markAsRootButton, "markAsRootButton");
        this.markAsRootButton.Name = "markAsRootButton";
        this.markAsRootButton.UseVisualStyleBackColor = true;
        this.markAsRootButton.Click += new System.EventHandler(this.OnMarkAsRootButtonClick);
        // 
        // removeTransitionButton
        // 
        resources.ApplyResources(this.removeTransitionButton, "removeTransitionButton");
        this.removeTransitionButton.Name = "removeTransitionButton";
        this.removeTransitionButton.UseVisualStyleBackColor = true;
        this.removeTransitionButton.Click += new System.EventHandler(this.OnRemoveTransitionButtonClick);
        // 
        // changeTransitionNameButton
        // 
        resources.ApplyResources(this.changeTransitionNameButton, "changeTransitionNameButton");
        this.changeTransitionNameButton.Name = "changeTransitionNameButton";
        this.changeTransitionNameButton.UseVisualStyleBackColor = true;
        this.changeTransitionNameButton.Click += new System.EventHandler(this.OnChangeTransitionNameButtonClick);
        // 
        // removeImageButton
        // 
        resources.ApplyResources(this.removeImageButton, "removeImageButton");
        this.removeImageButton.Name = "removeImageButton";
        this.removeImageButton.UseVisualStyleBackColor = true;
        this.removeImageButton.Click += new System.EventHandler(this.OnRemoveImageButtonClick);
        // 
        // label3
        // 
        resources.ApplyResources(this.label3, "label3");
        this.label3.Name = "label3";
        // 
        // transitionsList
        // 
        this.transitionsList.FormattingEnabled = true;
        resources.ApplyResources(this.transitionsList, "transitionsList");
        this.transitionsList.Name = "transitionsList";
        // 
        // previewPicture
        // 
        this.previewPicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        resources.ApplyResources(this.previewPicture, "previewPicture");
        this.previewPicture.Name = "previewPicture";
        this.previewPicture.TabStop = false;
        // 
        // selectImageButton
        // 
        resources.ApplyResources(this.selectImageButton, "selectImageButton");
        this.selectImageButton.Name = "selectImageButton";
        this.selectImageButton.UseVisualStyleBackColor = true;
        this.selectImageButton.Click += new System.EventHandler(this.OnSelectImageButtonClick);
        // 
        // textBox
        // 
        resources.ApplyResources(this.textBox, "textBox");
        this.textBox.Name = "textBox";
        this.textBox.TextChanged += new System.EventHandler(this.OnTextBoxTextChanged);
        // 
        // label2
        // 
        resources.ApplyResources(this.label2, "label2");
        this.label2.Name = "label2";
        // 
        // label1
        // 
        resources.ApplyResources(this.label1, "label1");
        this.label1.Name = "label1";
        // 
        // selectImageFileDialog
        // 
        this.selectImageFileDialog.FileName = "openFileDialog1";
        // 
        // openProjectFileDialog
        // 
        this.openProjectFileDialog.FileName = "openFileDialog2";
        // 
        // Form1
        // 
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
        resources.ApplyResources(this, "$this");
        this.Controls.Add(this.storyGroup);
        this.Controls.Add(this.graphGroup);
        this.Controls.Add(this.menuStrip1);
        this.MainMenuStrip = this.menuStrip1;
        this.Name = "Form1";
        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
        this.Load += new System.EventHandler(this.OnFormLoad);
        ((System.ComponentModel.ISupportInitialize)(this.graphPicture)).EndInit();
        this.menuStrip1.ResumeLayout(false);
        this.menuStrip1.PerformLayout();
        this.graphGroup.ResumeLayout(false);
        this.storyGroup.ResumeLayout(false);
        this.storyGroup.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.previewPicture)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.PictureBox graphPicture;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.GroupBox graphGroup;
    private System.Windows.Forms.GroupBox storyGroup;
    private System.Windows.Forms.RichTextBox textBox;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
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
    private System.Windows.Forms.Label idLabel;
    private System.Windows.Forms.ToolStripMenuItem languageToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem englishToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem russianToolStripMenuItem;
}

