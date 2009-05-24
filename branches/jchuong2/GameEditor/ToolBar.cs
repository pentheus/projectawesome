using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace GameEditor
{
    public partial class ToolBar : Form
    {
        GameEditor gameEditor;
        
        public ToolBar(GameEditor gameEditor)
        {
            InitializeComponent();
            this.gameEditor = gameEditor;
            openDialog.InitialDirectory = gameEditor.Content.RootDirectory;
        }

        public TreeView TreeView
        {
            get { return treeView1; }
        }

        private void ToolBar_Load(object sender, EventArgs e)
        {
            
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openDialog.ShowDialog();
            
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            Console.WriteLine(openDialog.FileName);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
          
        }



    }
}
