using System;
using System.Windows.Forms;
using EPocalipse.Json.Viewer;
using System.IO;
using System.ComponentModel;

namespace EPocalipse.Json.JsonView
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            JsonViewer.PropertyChanged += new PropertyChangedEventHandler(JsonViewer_PropertyChanged);
        }

        private void JsonViewer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Mode")
            {
                switch (JsonViewer.Mode)
                {
                    case 0:
                        toolStripStatusLabel1.Text = "Ready";
                        break;
                    case 1:
                        toolStripStatusLabel1.Text = "Waiting for input";
                        break;
                    case 2:
                        toolStripStatusLabel1.Text = "Parsing JSON";
                        break;
                    case 3:
                        toolStripStatusLabel1.Text = "Errors occurred while parsing JSON";
                        break;
                }
                
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            for (int i = 1; i < args.Length; i++)
            {
                string arg = args[i];
                if (arg.Equals("/c", StringComparison.OrdinalIgnoreCase))
                {
                    LoadFromClipboard();
                }
                else if (File.Exists(arg))
                {
                    LoadFromFile(arg);
                }
            }
        }

        private void LoadFromFile(string fileName)
        {
            string json = File.ReadAllText(fileName);
            JsonViewer.ShowTab(Tabs.Viewer);
            JsonViewer.refreshFromString(json);
        }

        private void LoadFromClipboard()
        {
            string json = Clipboard.GetText();
            if (!String.IsNullOrEmpty(json))
            {
                JsonViewer.ShowTab(Tabs.Viewer);
                JsonViewer.Json = json;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            JsonViewer.ShowTab(Tabs.Text);
        }

        /// <summary>
        /// Closes the program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Menu item File > Exit</remarks>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Open File Dialog for JSON files or Yahoo! Pipe files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Menu item File > Open</remarks>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter =
               "json files (*.json)|*.json|Yahoo! Pipe files (*.run)|*.run|All files (*.*)|*.*";
            dialog.InitialDirectory = Application.StartupPath;
            dialog.Title = "Select a JSON file";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.LoadFromFile(dialog.FileName);
            }
        }

        /// <summary>
        /// Launches About JSONViewer dialog box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Menu item Help > About JSON Viewer</remarks>
        private void aboutJSONViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutJsonViewer().ShowDialog();
        }

        /// <summary>
        /// Selects all text in the textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Menu item Edit > Select All</remarks>
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Control c;
            c = this.JsonViewer.Controls.Find("txtJson", true)[0];
            ((RichTextBox)c).SelectAll();
        }

        /// <summary>
        /// Deletes selected text in the textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Menu item Edit > Delete</remarks>
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Control c;
            c = this.JsonViewer.Controls.Find("txtJson", true)[0];
            if (((RichTextBox)c).SelectionLength > 0)
                _ = ((RichTextBox)c).SelectedText;
            else
                _ = ((RichTextBox)c).Text;
            ((RichTextBox)c).SelectedText = "";
        }

        /// <summary>
        /// Pastes text in the clipboard into the textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Menu item Edit > Paste</remarks>
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Control c;
            c = this.JsonViewer.Controls.Find("txtJson", true)[0];
            ((RichTextBox)c).Paste();
        }

        /// <summary>
        /// Copies text in the textbox into the clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Menu item Edit > Copy</remarks>
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Control c;
            c = this.JsonViewer.Controls.Find("txtJson", true)[0];
            ((RichTextBox)c).Copy();
        }

        /// <summary>
        /// Cuts selected text from the textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Menu item Edit > Cut</remarks>
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Control c;
            c = this.JsonViewer.Controls.Find("txtJson", true)[0];
            ((RichTextBox)c).Cut();
        }

        /// <summary>
        /// Undo the last action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Menu item Edit > Undo</remarks>
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Control c;
            c = this.JsonViewer.Controls.Find("txtJson", true)[0];
            ((RichTextBox)c).Undo();
        }

        /// <summary>
        /// Displays the find prompt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Menu item Viewer > Find</remarks>
        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Control c;
            c = this.JsonViewer.Controls.Find("pnlFind", true)[0];
            ((Panel)c).Visible = true;
            Control t;
            t = this.JsonViewer.Controls.Find("txtFind", true)[0];
            ((TextBox)t).Focus();
        }

        /// <summary>
        /// Expands the selected node and its children
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Menu item Viewer > Expand Selected</remarks>
        /// <!---->
        private void expandSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Control c;
            c = this.JsonViewer.Controls.Find("tvJson", true)[0];
            ((TreeView)c).BeginUpdate();
            try
            {
                if (((TreeView)c).SelectedNode != null)
                {
                    TreeNode topNode = ((TreeView)c).TopNode;
                    ((TreeView)c).SelectedNode.ExpandAll();
                    ((TreeView)c).TopNode = topNode;
                }
            }
            finally
            {
                ((TreeView)c).EndUpdate();
            }
        }

        /// <summary>
        /// Copies a node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Menu item Viewer > Copy</remarks>
        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Control c;
            c = this.JsonViewer.Controls.Find("tvJson", true)[0];
            TreeNode node = ((TreeView)c).SelectedNode;
            if (node != null)
            {
                Clipboard.SetText(node.Text);
            }
        }

        /// <summary>
        /// Copies just the node's value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Menu item Viewer > Copy Value</remarks>
        /// <!-- JsonViewerTreeNode had to be made public to be accessible here -->
        private void copyValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Control c;
            c = this.JsonViewer.Controls.Find("tvJson", true)[0];
            JsonViewerTreeNode node = (JsonViewerTreeNode)((TreeView)c).SelectedNode;
            if (node != null && node.JsonObject.Value != null)
            {
                Clipboard.SetText(node.JsonObject.Value.ToString());
            }
        }

        /// <summary>
        /// Expands all nodes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Menu item Viewer > Expand All</remarks>
        /// <!---->
        private void expandAllMenuItem_Click(object sender, EventArgs e)
        {
            Control c;
            c = this.JsonViewer.Controls.Find("tvJson", true)[0];
            ((TreeView)c).BeginUpdate();
            try
            {
                if (((TreeView)c).SelectedNode != null)
                {
                    TreeNode node = ((TreeView)c).SelectedNode;
                    while (node.Parent != null)
                    {
                        node = node.Parent;
                    }

                    node.ExpandAll();
                }
            }
            finally
            {
                ((TreeView)c).EndUpdate();
            }
        }

        /// <summary>
        /// Collapses the selected node and its children
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Menu item Viewer > Collapse Selected</remarks>
        /// <!---->
        private void collapseSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Control c;
            c = this.JsonViewer.Controls.Find("tvJson", true)[0];
            ((TreeView)c).BeginUpdate();
            try
            {
                if (((TreeView)c).SelectedNode != null)
                {
                    TreeNode topNode = ((TreeView)c).TopNode;
                    ((TreeView)c).SelectedNode.Collapse(false);
                    ((TreeView)c).TopNode = topNode;
                }
            }
            finally
            {
                ((TreeView)c).EndUpdate();
            }
        }

        /// <summary>
        /// Collapses all nodes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Menu item Viewer > Collapse All</remarks>
        /// <!---->
        private void collapseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Control c;
            c = this.JsonViewer.Controls.Find("tvJson", true)[0];
            ((TreeView)c).BeginUpdate();
            try
            {
                if (((TreeView)c).SelectedNode != null)
                {
                    TreeNode node = ((TreeView)c).SelectedNode;
                    while (node.Parent != null)
                    {
                        node = node.Parent;
                    }

                    node.Collapse(false);
                }
            }
            finally
            {
                ((TreeView)c).EndUpdate();
            }
        }
    }
}