using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using AwesomeEngine;
using Microsoft.Xna.Framework;

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
            saveDialog.ShowDialog();
        }

        private void treeView1_AfterSelect(object sender, TreeNodeMouseClickEventArgs e)
        {
            gameEditor.SetCursorModel(e.Node.Name);      
        }

        private void keyDown(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }


        private void posX_ValueChanged(object sender, EventArgs e)
        {
            gameEditor.GetCursor().Position = new Vector3((float)posX.Value, (float)posY.Value, (float)posZ.Value);
        }

        private void posY_ValueChanged(object sender, EventArgs e)
        {
            gameEditor.GetCursor().Position = new Vector3((float)posX.Value, (float)posY.Value, (float)posZ.Value);
        }

        private void posZ_ValueChanged(object sender, EventArgs e)
        {
            gameEditor.GetCursor().Position = new Vector3((float)posX.Value, (float)posY.Value, (float)posZ.Value);
        }

        private void rotX_ValueChanged_1(object sender, EventArgs e)
        {
            gameEditor.GetCursor().Rotation = new Vector3((float)rotX.Value, (float)rotY.Value, (float)rotZ.Value);
        }

        private void rotY_ValueChanged_1(object sender, EventArgs e)
        {
            gameEditor.GetCursor().Rotation = new Vector3((float)rotX.Value, (float)rotY.Value, (float)rotZ.Value);
        }

        private void rotZ_ValueChanged_1(object sender, EventArgs e)
        {
            gameEditor.GetCursor().Rotation = new Vector3((float)rotX.Value, (float)rotY.Value, (float)rotZ.Value);
        }

        private void scaleX_ValueChanged_1(object sender, EventArgs e)
        {
            if (uniScale.CheckState == CheckState.Checked)
            {
                scaleY.Value = scaleX.Value;
                scaleZ.Value = scaleX.Value;
            }
            gameEditor.GetCursor().Scale = new Vector3((float)scaleX.Value, (float)scaleY.Value, (float)scaleZ.Value);
        }

        private void scaleY_ValueChanged_1(object sender, EventArgs e)
        {
            if (uniScale.CheckState == CheckState.Checked)
            {
                scaleX.Value = scaleY.Value;
                scaleZ.Value = scaleY.Value;
            }
            gameEditor.GetCursor().Scale = new Vector3((float)scaleX.Value, (float)scaleY.Value, (float)scaleZ.Value);
        }

        private void scaleZ_ValueChanged_1(object sender, EventArgs e)
        {
            if (uniScale.CheckState == CheckState.Checked)
            {
                scaleX.Value = scaleZ.Value;
                scaleY.Value = scaleZ.Value;
            }
            gameEditor.GetCursor().Scale = new Vector3((float)scaleX.Value, (float)scaleY.Value, (float)scaleZ.Value);
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            gameEditor.GetScene().SceneGraph.AddObject(gameEditor.GetCursor());
        }

        private void setGeoButton_Click(object sender, EventArgs e)
        {
            gameEditor.SetWorldGeometry();
        }
        
        private void openDialog_FileOK(object sender, CancelEventArgs e)
        {
            Octree scene;

            scene = gameEditor.GetSceneParser().ReadScene(openDialog.FileName);

            gameEditor.SetScene(scene);
        }

        private void saveDialog_FileOk(object sender, CancelEventArgs e)
        {
            gameEditor.GetSceneParser().SaveScene(gameEditor.GetScene().SceneGraph, saveDialog.FileName);
        }
    }
}
