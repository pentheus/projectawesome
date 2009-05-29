namespace GameEditor
{
    partial class ToolBar
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        //
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Props");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Items");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Logic Entities");
            this.menuBar = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.objectGroupBox = new System.Windows.Forms.GroupBox();
            this.setGeoButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.uniScale = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.scaleZ = new System.Windows.Forms.NumericUpDown();
            this.scaleY = new System.Windows.Forms.NumericUpDown();
            this.scaleX = new System.Windows.Forms.NumericUpDown();
            this.scale_label = new System.Windows.Forms.Label();
            this.rotZ = new System.Windows.Forms.NumericUpDown();
            this.rotY = new System.Windows.Forms.NumericUpDown();
            this.rotX = new System.Windows.Forms.NumericUpDown();
            this.rot_label = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.posZ = new System.Windows.Forms.NumericUpDown();
            this.posY = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.pos_label = new System.Windows.Forms.Label();
            this.posX = new System.Windows.Forms.NumericUpDown();
            this.button2 = new System.Windows.Forms.Button();
            this.createButton = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.openDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveDialog = new System.Windows.Forms.SaveFileDialog();
            this.menuBar.SuspendLayout();
            this.panel1.SuspendLayout();
            this.objectGroupBox.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scaleZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scaleY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scaleX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.posZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.posY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.posX)).BeginInit();
            this.SuspendLayout();
            // 
            // menuBar
            // 
            this.menuBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.menuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuBar.Location = new System.Drawing.Point(0, 0);
            this.menuBar.Name = "menuBar";
            this.menuBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuBar.Size = new System.Drawing.Size(298, 24);
            this.menuBar.TabIndex = 0;
            this.menuBar.Text = "menuBar";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.objectGroupBox);
            this.panel1.Location = new System.Drawing.Point(0, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(298, 495);
            this.panel1.TabIndex = 1;
            // 
            // objectGroupBox
            // 
            this.objectGroupBox.Controls.Add(this.setGeoButton);
            this.objectGroupBox.Controls.Add(this.panel2);
            this.objectGroupBox.Controls.Add(this.button2);
            this.objectGroupBox.Controls.Add(this.createButton);
            this.objectGroupBox.Controls.Add(this.treeView1);
            this.objectGroupBox.Location = new System.Drawing.Point(12, 28);
            this.objectGroupBox.Name = "objectGroupBox";
            this.objectGroupBox.Size = new System.Drawing.Size(274, 401);
            this.objectGroupBox.TabIndex = 1;
            this.objectGroupBox.TabStop = false;
            this.objectGroupBox.Text = "Objects";
            // 
            // setGeoButton
            // 
            this.setGeoButton.Location = new System.Drawing.Point(6, 196);
            this.setGeoButton.Name = "setGeoButton";
            this.setGeoButton.Size = new System.Drawing.Size(166, 23);
            this.setGeoButton.TabIndex = 26;
            this.setGeoButton.Text = "Set World Geometry";
            this.setGeoButton.UseVisualStyleBackColor = true;
            this.setGeoButton.Click += new System.EventHandler(this.setGeoButton_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.uniScale);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.scaleZ);
            this.panel2.Controls.Add(this.scaleY);
            this.panel2.Controls.Add(this.scaleX);
            this.panel2.Controls.Add(this.scale_label);
            this.panel2.Controls.Add(this.rotZ);
            this.panel2.Controls.Add(this.rotY);
            this.panel2.Controls.Add(this.rotX);
            this.panel2.Controls.Add(this.rot_label);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.posZ);
            this.panel2.Controls.Add(this.posY);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.pos_label);
            this.panel2.Controls.Add(this.posX);
            this.panel2.Location = new System.Drawing.Point(6, 225);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(262, 170);
            this.panel2.TabIndex = 25;
            // 
            // uniScale
            // 
            this.uniScale.AutoSize = true;
            this.uniScale.Location = new System.Drawing.Point(7, 129);
            this.uniScale.Name = "uniScale";
            this.uniScale.Size = new System.Drawing.Size(92, 17);
            this.uniScale.TabIndex = 46;
            this.uniScale.Text = "Uniform Scale";
            this.uniScale.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(178, 105);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(14, 13);
            this.label9.TabIndex = 45;
            this.label9.Text = "Z";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(176, 66);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(14, 13);
            this.label8.TabIndex = 44;
            this.label8.Text = "Z";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 105);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(14, 13);
            this.label7.TabIndex = 43;
            this.label7.Text = "X";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(91, 105);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 13);
            this.label6.TabIndex = 42;
            this.label6.Text = "Y";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(91, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 41;
            this.label5.Text = "Y";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 40;
            this.label4.Text = "X";
            // 
            // scaleZ
            // 
            this.scaleZ.DecimalPlaces = 4;
            this.scaleZ.Increment = new decimal(new int[] {
            5,
            0,
            0,
            196608});
            this.scaleZ.Location = new System.Drawing.Point(196, 103);
            this.scaleZ.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.scaleZ.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            196608});
            this.scaleZ.Name = "scaleZ";
            this.scaleZ.Size = new System.Drawing.Size(60, 20);
            this.scaleZ.TabIndex = 39;
            this.scaleZ.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.scaleZ.ValueChanged += new System.EventHandler(this.scaleZ_ValueChanged_1);
            // 
            // scaleY
            // 
            this.scaleY.DecimalPlaces = 4;
            this.scaleY.Increment = new decimal(new int[] {
            5,
            0,
            0,
            196608});
            this.scaleY.Location = new System.Drawing.Point(111, 103);
            this.scaleY.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.scaleY.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            196608});
            this.scaleY.Name = "scaleY";
            this.scaleY.Size = new System.Drawing.Size(61, 20);
            this.scaleY.TabIndex = 38;
            this.scaleY.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.scaleY.ValueChanged += new System.EventHandler(this.scaleY_ValueChanged_1);
            // 
            // scaleX
            // 
            this.scaleX.DecimalPlaces = 4;
            this.scaleX.Increment = new decimal(new int[] {
            5,
            0,
            0,
            196608});
            this.scaleX.Location = new System.Drawing.Point(24, 103);
            this.scaleX.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.scaleX.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            196608});
            this.scaleX.Name = "scaleX";
            this.scaleX.Size = new System.Drawing.Size(61, 20);
            this.scaleX.TabIndex = 37;
            this.scaleX.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.scaleX.ValueChanged += new System.EventHandler(this.scaleX_ValueChanged_1);
            // 
            // scale_label
            // 
            this.scale_label.AutoSize = true;
            this.scale_label.Location = new System.Drawing.Point(4, 87);
            this.scale_label.Name = "scale_label";
            this.scale_label.Size = new System.Drawing.Size(34, 13);
            this.scale_label.TabIndex = 36;
            this.scale_label.Text = "Scale";
            // 
            // rotZ
            // 
            this.rotZ.DecimalPlaces = 2;
            this.rotZ.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.rotZ.Location = new System.Drawing.Point(196, 64);
            this.rotZ.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.rotZ.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            -2147483648});
            this.rotZ.Name = "rotZ";
            this.rotZ.Size = new System.Drawing.Size(60, 20);
            this.rotZ.TabIndex = 35;
            this.rotZ.ValueChanged += new System.EventHandler(this.rotZ_ValueChanged_1);
            // 
            // rotY
            // 
            this.rotY.DecimalPlaces = 2;
            this.rotY.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.rotY.Location = new System.Drawing.Point(111, 64);
            this.rotY.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.rotY.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            -2147483648});
            this.rotY.Name = "rotY";
            this.rotY.Size = new System.Drawing.Size(61, 20);
            this.rotY.TabIndex = 34;
            this.rotY.ValueChanged += new System.EventHandler(this.rotY_ValueChanged_1);
            // 
            // rotX
            // 
            this.rotX.DecimalPlaces = 2;
            this.rotX.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.rotX.Location = new System.Drawing.Point(24, 64);
            this.rotX.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.rotX.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            -2147483648});
            this.rotX.Name = "rotX";
            this.rotX.Size = new System.Drawing.Size(61, 20);
            this.rotX.TabIndex = 33;
            this.rotX.ValueChanged += new System.EventHandler(this.rotX_ValueChanged_1);
            // 
            // rot_label
            // 
            this.rot_label.AutoSize = true;
            this.rot_label.Location = new System.Drawing.Point(4, 48);
            this.rot_label.Name = "rot_label";
            this.rot_label.Size = new System.Drawing.Size(47, 13);
            this.rot_label.TabIndex = 32;
            this.rot_label.Text = "Rotation";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(91, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 31;
            this.label3.Text = "Y";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(178, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "Z";
            // 
            // posZ
            // 
            this.posZ.DecimalPlaces = 2;
            this.posZ.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.posZ.Location = new System.Drawing.Point(196, 25);
            this.posZ.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.posZ.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            -2147483648});
            this.posZ.Name = "posZ";
            this.posZ.Size = new System.Drawing.Size(60, 20);
            this.posZ.TabIndex = 29;
            this.posZ.ValueChanged += new System.EventHandler(this.posZ_ValueChanged);
            // 
            // posY
            // 
            this.posY.DecimalPlaces = 2;
            this.posY.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.posY.Location = new System.Drawing.Point(111, 25);
            this.posY.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.posY.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            -2147483648});
            this.posY.Name = "posY";
            this.posY.Size = new System.Drawing.Size(61, 20);
            this.posY.TabIndex = 28;
            this.posY.ValueChanged += new System.EventHandler(this.posY_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "X";
            // 
            // pos_label
            // 
            this.pos_label.AutoSize = true;
            this.pos_label.Location = new System.Drawing.Point(7, 9);
            this.pos_label.Name = "pos_label";
            this.pos_label.Size = new System.Drawing.Size(44, 13);
            this.pos_label.TabIndex = 26;
            this.pos_label.Text = "Position";
            // 
            // posX
            // 
            this.posX.DecimalPlaces = 2;
            this.posX.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.posX.Location = new System.Drawing.Point(24, 25);
            this.posX.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.posX.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            -2147483648});
            this.posX.Name = "posX";
            this.posX.Size = new System.Drawing.Size(61, 20);
            this.posX.TabIndex = 25;
            this.posX.ValueChanged += new System.EventHandler(this.posX_ValueChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(179, 48);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "Delete";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // createButton
            // 
            this.createButton.Location = new System.Drawing.Point(180, 19);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(75, 23);
            this.createButton.TabIndex = 8;
            this.createButton.Text = "Create";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // treeView1
            // 
            this.treeView1.FullRowSelect = true;
            this.treeView1.Location = new System.Drawing.Point(6, 19);
            this.treeView1.Name = "treeView1";
            treeNode4.Name = "Props";
            treeNode4.Text = "Props";
            treeNode5.Name = "Items";
            treeNode5.Text = "Items";
            treeNode6.Name = "LogicEntities";
            treeNode6.Text = "Logic Entities";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode5,
            treeNode6});
            this.treeView1.Size = new System.Drawing.Size(168, 174);
            this.treeView1.TabIndex = 0;
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_AfterSelect);
            // 
            // openDialog
            // 
            this.openDialog.FileName = "openFileDialog1";
            this.openDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openDialog_FileOK);
            // 
            // saveDialog
            // 
            this.saveDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.saveDialog_FileOk);
            // 
            // ToolBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 519);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuBar);
            this.MainMenuStrip = this.menuBar;
            this.Name = "ToolBar";
            this.Text = "ToolBar";
            this.Load += new System.EventHandler(this.ToolBar_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.keyDown);
            this.menuBar.ResumeLayout(false);
            this.menuBar.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.objectGroupBox.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scaleZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scaleY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scaleX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.posZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.posY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.posX)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuBar;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openDialog;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.GroupBox objectGroupBox;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox uniScale;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown scaleZ;
        private System.Windows.Forms.NumericUpDown scaleY;
        private System.Windows.Forms.NumericUpDown scaleX;
        private System.Windows.Forms.Label scale_label;
        private System.Windows.Forms.NumericUpDown rotZ;
        private System.Windows.Forms.NumericUpDown rotY;
        private System.Windows.Forms.NumericUpDown rotX;
        private System.Windows.Forms.Label rot_label;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown posZ;
        private System.Windows.Forms.NumericUpDown posY;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label pos_label;
        private System.Windows.Forms.NumericUpDown posX;
        private System.Windows.Forms.Button setGeoButton;
        private System.Windows.Forms.SaveFileDialog saveDialog;
    }
}