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
using AwesomeEngine.Items;
using XNAnimation;

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
            saveDialog.InitialDirectory = gameEditor.Content.RootDirectory;
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
            if (posX.Enabled)
            {
                if (transBSphere.CheckState == CheckState.Checked)
                {
                    BoundingSphere bSphere = gameEditor.GetCursor().BoundingSphere;
                    Vector3 offset = new Vector3((float)posX.Value, (float)posY.Value, (float)posZ.Value);
                    bSphere.Center = gameEditor.GetCursor().Position + offset;
                    gameEditor.GetCursor().BoundingSphere = bSphere;
                }
                else
                    gameEditor.GetCursor().Position = new Vector3((float)posX.Value, (float)posY.Value, (float)posZ.Value);
                Console.WriteLine("Value Changed");
            }
        }

        private void posY_ValueChanged(object sender, EventArgs e)
        {
            if (posY.Enabled)
            {
                if (transBSphere.CheckState == CheckState.Checked)
                {
                    BoundingSphere bSphere = gameEditor.GetCursor().BoundingSphere;
                    Vector3 offset = new Vector3((float)posX.Value, (float)posY.Value, (float)posZ.Value);
                    bSphere.Center = gameEditor.GetCursor().Position + offset;
                    gameEditor.GetCursor().BoundingSphere = bSphere;
                }
                else
                    gameEditor.GetCursor().Position = new Vector3((float)posX.Value, (float)posY.Value, (float)posZ.Value);
            }
        }

        private void posZ_ValueChanged(object sender, EventArgs e)
        {
            if (posZ.Enabled)
            {
                if (transBSphere.CheckState == CheckState.Checked)
                {
                    BoundingSphere bSphere = gameEditor.GetCursor().BoundingSphere;
                    Vector3 offset = new Vector3((float)posX.Value, (float)posY.Value, (float)posZ.Value);
                    bSphere.Center = gameEditor.GetCursor().Position + offset;
                    gameEditor.GetCursor().BoundingSphere = bSphere;
                }
                else
                    gameEditor.GetCursor().Position = new Vector3((float)posX.Value, (float)posY.Value, (float)posZ.Value);
            }
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

            if (transBSphere.CheckState == CheckState.Checked)
            {
                float scaleOffset = (float)scaleX.Value;
                gameEditor.GetCursor().BSphereScale = scaleOffset;
            }
            else
                gameEditor.GetCursor().Scale = new Vector3((float)scaleX.Value, (float)scaleY.Value, (float)scaleZ.Value);
        }

        private void scaleY_ValueChanged_1(object sender, EventArgs e)
        {
            if (uniScale.CheckState == CheckState.Checked)
            {
                scaleX.Value = scaleY.Value;
                scaleZ.Value = scaleY.Value;
            }
            if (transBSphere.CheckState == CheckState.Checked)
            {
                float scaleOffset = (float)scaleY.Value;
                gameEditor.GetCursor().BSphereScale = scaleOffset;
            }
            else
                gameEditor.GetCursor().Scale = new Vector3((float)scaleX.Value, (float)scaleY.Value, (float)scaleZ.Value);
        }

        private void scaleZ_ValueChanged_1(object sender, EventArgs e)
        {
            if (uniScale.CheckState == CheckState.Checked)
            {
                scaleX.Value = scaleZ.Value;
                scaleY.Value = scaleZ.Value;
            }
            if (transBSphere.CheckState == CheckState.Checked)
            {
                float scaleOffset = (float)scaleZ.Value;
                gameEditor.GetCursor().BSphereScale = scaleOffset;
            }
            else
                gameEditor.GetCursor().Scale = new Vector3((float)scaleX.Value, (float)scaleY.Value, (float)scaleZ.Value);
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            String fileName = gameEditor.GetCursor().FileName;
            if(fileName.Contains("item"))
            {
                Item item;

                if(fileName.Contains("battery"))
                {
                    item = new BatteryItem((Game)gameEditor, gameEditor.GetCursor());
                }

                else if(fileName.Contains("flashlight"))
                {
                    item = new FlashLightItem((Game)gameEditor, gameEditor.GetCursor());
                }

                else if(fileName.Contains("fuse"))
                {
                    item = new FuseItem((Game)gameEditor, gameEditor.GetCursor());
                }

                if (item != null)
                    gameEditor.GetScene().SceneGraph.AddItem(item);
            }

                //If bad stuff starts to happen check this block
            else if(fileName.Contains("ent"))
            {
                LogicEntity ent;

                if(fileName.Contains("spawn"))
                {
                    SkinnedModel enemymodel = gameEditor.GetContent().Load<SkinnedModel>("shadowmonster");
                    ent = new  SpawnEntity((Game)gameEditor, gameEditor.GetCursor().Model, gameEditor.GetCursor().Position, enemymodel);
                }

                else if (fileName.Contains("trigger"))
                {
                    ent = new TriggerEntity((Game)gameEditor, gameEditor.GetCursor().Model, gameEditor.GetCursor().Position);
                }

                
            }
            gameEditor.GetScene().SceneGraph.AddObject(gameEditor.GetCursor());
            ;
            gameEditor.GetScene().SceneGraph.AddEntity();
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

        private void transBSphere_CheckedChanged(object sender, EventArgs e)
        {
            if (transBSphere.CheckState == CheckState.Checked)
            {
                posX.Enabled = false;
                posY.Enabled = false;
                posZ.Enabled = false;

                scaleX.Enabled = false;
                scaleX.Enabled = false;
                scaleX.Enabled = false;

                posX.Value = (decimal)gameEditor.GetCursor().BoundingSphere.Center.X;
                posY.Value = (decimal)gameEditor.GetCursor().BoundingSphere.Center.Y;
                posZ.Value = (decimal)gameEditor.GetCursor().BoundingSphere.Center.Z;

                scaleX.Value = (decimal)gameEditor.GetCursor().BSphereScale;
                scaleY.Value = (decimal)gameEditor.GetCursor().BSphereScale;
                scaleZ.Value = (decimal)gameEditor.GetCursor().BSphereScale;

                posX.Enabled = true;
                posY.Enabled = true;
                posZ.Enabled = true;

                scaleX.Enabled = true;
                scaleX.Enabled = true;
                scaleX.Enabled = true;
            }

            else
            {
                posX.Enabled = false;
                posY.Enabled = false;
                posZ.Enabled = false;

                scaleX.Enabled = false;
                scaleX.Enabled = false;
                scaleX.Enabled = false;

                posX.Value = (decimal)gameEditor.GetCursor().Position.X;
                posY.Value = (decimal)gameEditor.GetCursor().Position.Y;
                posZ.Value = (decimal)gameEditor.GetCursor().Position.Z;
                
                scaleX.Value = (decimal)gameEditor.GetCursor().Scale.X;
                scaleY.Value = (decimal)gameEditor.GetCursor().Scale.Y;
                scaleZ.Value = (decimal)gameEditor.GetCursor().Scale.Z;

                posX.Enabled = true;
                posY.Enabled = true;
                posZ.Enabled = true;

                scaleX.Enabled = true;
                scaleX.Enabled = true;
                scaleX.Enabled = true;
            }
        }
        
    }
}
