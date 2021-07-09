
namespace Penguin__REMS_Project
{
    partial class Remsform
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lidarConfigTb = new MetroFramework.Controls.MetroTabControl();
            this.metroTabPage1 = new MetroFramework.Controls.MetroTabPage();
            this.startBtn = new MetroFramework.Controls.MetroButton();
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.metroTabPage5 = new MetroFramework.Controls.MetroTabPage();
            this.sickFLPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.metroTabPage7 = new MetroFramework.Controls.MetroTabPage();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.metroPanel2 = new MetroFramework.Controls.MetroPanel();
            this.lidarNameCbx = new MetroFramework.Controls.MetroComboBox();
            this.lidarTypeCbx = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel10 = new MetroFramework.Controls.MetroLabel();
            this.lidarIPTxt = new MetroFramework.Controls.MetroTextBox();
            this.lidarPortTxt = new MetroFramework.Controls.MetroTextBox();
            this.resetLidarBtn = new MetroFramework.Controls.MetroButton();
            this.addLidarBtn = new MetroFramework.Controls.MetroButton();
            this.metroLabel7 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel8 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel9 = new MetroFramework.Controls.MetroLabel();
            this.metroPanel3 = new MetroFramework.Controls.MetroPanel();
            this.pullFrameBtn = new MetroFramework.Controls.MetroButton();
            this.pingNewLidarBtn = new MetroFramework.Controls.MetroButton();
            this.lidarConfigLogview = new System.Windows.Forms.TextBox();
            this.lidarPicInfoTl = new MetroFramework.Controls.MetroTile();
            this.metroTabPage6 = new MetroFramework.Controls.MetroTabPage();
            this.threeDLidarFLPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.metroTabPage2 = new MetroFramework.Controls.MetroTabPage();
            this.metroTabPage3 = new MetroFramework.Controls.MetroTabPage();
            this.metroTabPage4 = new MetroFramework.Controls.MetroTabPage();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.metroTile1 = new MetroFramework.Controls.MetroTile();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel6 = new MetroFramework.Controls.MetroLabel();
            this.lidarConfigTb.SuspendLayout();
            this.metroTabPage1.SuspendLayout();
            this.metroTabPage5.SuspendLayout();
            this.metroTabPage7.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.metroPanel2.SuspendLayout();
            this.metroPanel3.SuspendLayout();
            this.metroTabPage6.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lidarConfigTb
            // 
            this.lidarConfigTb.Controls.Add(this.metroTabPage1);
            this.lidarConfigTb.Controls.Add(this.metroTabPage7);
            this.lidarConfigTb.Controls.Add(this.metroTabPage5);
            this.lidarConfigTb.Controls.Add(this.metroTabPage6);
            this.lidarConfigTb.Controls.Add(this.metroTabPage2);
            this.lidarConfigTb.Controls.Add(this.metroTabPage3);
            this.lidarConfigTb.Controls.Add(this.metroTabPage4);
            this.lidarConfigTb.Location = new System.Drawing.Point(11, 88);
            this.lidarConfigTb.Name = "lidarConfigTb";
            this.lidarConfigTb.SelectedIndex = 0;
            this.lidarConfigTb.Size = new System.Drawing.Size(1176, 636);
            this.lidarConfigTb.TabIndex = 0;
            // 
            // metroTabPage1
            // 
            this.metroTabPage1.Controls.Add(this.tableLayoutPanel1);
            this.metroTabPage1.HorizontalScrollbarBarColor = true;
            this.metroTabPage1.Location = new System.Drawing.Point(4, 35);
            this.metroTabPage1.Name = "metroTabPage1";
            this.metroTabPage1.Size = new System.Drawing.Size(1168, 597);
            this.metroTabPage1.TabIndex = 0;
            this.metroTabPage1.Text = "Main";
            this.metroTabPage1.VerticalScrollbarBarColor = true;
            // 
            // startBtn
            // 
            this.startBtn.Location = new System.Drawing.Point(3, 3);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(149, 23);
            this.startBtn.TabIndex = 3;
            this.startBtn.Text = "Start Scan";
            this.startBtn.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // metroButton1
            // 
            this.metroButton1.Location = new System.Drawing.Point(158, 3);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(88, 23);
            this.metroButton1.TabIndex = 2;
            this.metroButton1.Text = "Stop Scans";
            // 
            // metroTabPage5
            // 
            this.metroTabPage5.BackgroundImage = global::Penguin__REMS_Project.Properties.Resources.isckLMS;
            this.metroTabPage5.Controls.Add(this.sickFLPanel);
            this.metroTabPage5.HorizontalScrollbarBarColor = true;
            this.metroTabPage5.Location = new System.Drawing.Point(4, 35);
            this.metroTabPage5.Name = "metroTabPage5";
            this.metroTabPage5.Size = new System.Drawing.Size(1168, 519);
            this.metroTabPage5.TabIndex = 4;
            this.metroTabPage5.Text = "2D Sick Lidar";
            this.metroTabPage5.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTabPage5.VerticalScrollbarBarColor = true;
            // 
            // sickFLPanel
            // 
            this.sickFLPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sickFLPanel.Location = new System.Drawing.Point(0, 0);
            this.sickFLPanel.Name = "sickFLPanel";
            this.sickFLPanel.Size = new System.Drawing.Size(1168, 519);
            this.sickFLPanel.TabIndex = 6;
            // 
            // metroTabPage7
            // 
            this.metroTabPage7.Controls.Add(this.flowLayoutPanel1);
            this.metroTabPage7.HorizontalScrollbarBarColor = true;
            this.metroTabPage7.Location = new System.Drawing.Point(4, 35);
            this.metroTabPage7.Name = "metroTabPage7";
            this.metroTabPage7.Size = new System.Drawing.Size(1168, 519);
            this.metroTabPage7.TabIndex = 6;
            this.metroTabPage7.Text = "Lidar Config";
            this.metroTabPage7.VerticalScrollbarBarColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.metroPanel2);
            this.flowLayoutPanel1.Controls.Add(this.metroPanel3);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1168, 519);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // metroPanel2
            // 
            this.metroPanel2.BackColor = System.Drawing.Color.White;
            this.metroPanel2.Controls.Add(this.lidarNameCbx);
            this.metroPanel2.Controls.Add(this.lidarTypeCbx);
            this.metroPanel2.Controls.Add(this.metroLabel10);
            this.metroPanel2.Controls.Add(this.lidarIPTxt);
            this.metroPanel2.Controls.Add(this.lidarPortTxt);
            this.metroPanel2.Controls.Add(this.resetLidarBtn);
            this.metroPanel2.Controls.Add(this.addLidarBtn);
            this.metroPanel2.Controls.Add(this.metroLabel7);
            this.metroPanel2.Controls.Add(this.metroLabel8);
            this.metroPanel2.Controls.Add(this.metroLabel9);
            this.metroPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroPanel2.HorizontalScrollbarBarColor = true;
            this.metroPanel2.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel2.HorizontalScrollbarSize = 10;
            this.metroPanel2.Location = new System.Drawing.Point(3, 3);
            this.metroPanel2.Name = "metroPanel2";
            this.metroPanel2.Size = new System.Drawing.Size(318, 513);
            this.metroPanel2.TabIndex = 4;
            this.metroPanel2.VerticalScrollbarBarColor = true;
            this.metroPanel2.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel2.VerticalScrollbarSize = 10;
            // 
            // lidarNameCbx
            // 
            this.lidarNameCbx.FormattingEnabled = true;
            this.lidarNameCbx.ItemHeight = 23;
            this.lidarNameCbx.Items.AddRange(new object[] {
            "Leinshen C16",
            "Sick LMS511 Lite",
            "Sick LMS511 Pro",
            "Sick MRS611",
            "Velodyne VLP 16"});
            this.lidarNameCbx.Location = new System.Drawing.Point(121, 15);
            this.lidarNameCbx.Name = "lidarNameCbx";
            this.lidarNameCbx.Size = new System.Drawing.Size(183, 29);
            this.lidarNameCbx.Sorted = true;
            this.lidarNameCbx.TabIndex = 13;
            // 
            // lidarTypeCbx
            // 
            this.lidarTypeCbx.FormattingEnabled = true;
            this.lidarTypeCbx.ItemHeight = 23;
            this.lidarTypeCbx.Items.AddRange(new object[] {
            "2D",
            "3D"});
            this.lidarTypeCbx.Location = new System.Drawing.Point(121, 132);
            this.lidarTypeCbx.Name = "lidarTypeCbx";
            this.lidarTypeCbx.Size = new System.Drawing.Size(183, 29);
            this.lidarTypeCbx.TabIndex = 12;
            // 
            // metroLabel10
            // 
            this.metroLabel10.AutoSize = true;
            this.metroLabel10.Location = new System.Drawing.Point(14, 142);
            this.metroLabel10.Name = "metroLabel10";
            this.metroLabel10.Size = new System.Drawing.Size(43, 19);
            this.metroLabel10.TabIndex = 11;
            this.metroLabel10.Text = "Type :";
            // 
            // lidarIPTxt
            // 
            this.lidarIPTxt.Location = new System.Drawing.Point(121, 54);
            this.lidarIPTxt.Name = "lidarIPTxt";
            this.lidarIPTxt.Size = new System.Drawing.Size(183, 23);
            this.lidarIPTxt.TabIndex = 5;
            // 
            // lidarPortTxt
            // 
            this.lidarPortTxt.Location = new System.Drawing.Point(121, 99);
            this.lidarPortTxt.Name = "lidarPortTxt";
            this.lidarPortTxt.Size = new System.Drawing.Size(183, 23);
            this.lidarPortTxt.TabIndex = 6;
            // 
            // resetLidarBtn
            // 
            this.resetLidarBtn.Location = new System.Drawing.Point(14, 199);
            this.resetLidarBtn.Name = "resetLidarBtn";
            this.resetLidarBtn.Size = new System.Drawing.Size(139, 23);
            this.resetLidarBtn.TabIndex = 10;
            this.resetLidarBtn.Text = "Reset";
            this.resetLidarBtn.Click += new System.EventHandler(this.ResetLidarBtn_Click);
            // 
            // addLidarBtn
            // 
            this.addLidarBtn.Location = new System.Drawing.Point(165, 199);
            this.addLidarBtn.Name = "addLidarBtn";
            this.addLidarBtn.Size = new System.Drawing.Size(139, 23);
            this.addLidarBtn.TabIndex = 9;
            this.addLidarBtn.Text = "Add";
            this.addLidarBtn.Click += new System.EventHandler(this.AddLidarBtn_Click);
            // 
            // metroLabel7
            // 
            this.metroLabel7.AutoSize = true;
            this.metroLabel7.Location = new System.Drawing.Point(14, 99);
            this.metroLabel7.Name = "metroLabel7";
            this.metroLabel7.Size = new System.Drawing.Size(34, 19);
            this.metroLabel7.TabIndex = 7;
            this.metroLabel7.Text = "Port";
            // 
            // metroLabel8
            // 
            this.metroLabel8.AutoSize = true;
            this.metroLabel8.Location = new System.Drawing.Point(14, 58);
            this.metroLabel8.Name = "metroLabel8";
            this.metroLabel8.Size = new System.Drawing.Size(77, 19);
            this.metroLabel8.TabIndex = 3;
            this.metroLabel8.Text = "Ip Adresse :";
            // 
            // metroLabel9
            // 
            this.metroLabel9.AutoSize = true;
            this.metroLabel9.ForeColor = System.Drawing.Color.White;
            this.metroLabel9.Location = new System.Drawing.Point(14, 19);
            this.metroLabel9.Name = "metroLabel9";
            this.metroLabel9.Size = new System.Drawing.Size(45, 19);
            this.metroLabel9.Style = MetroFramework.MetroColorStyle.Silver;
            this.metroLabel9.TabIndex = 2;
            this.metroLabel9.Text = "Name";
            this.metroLabel9.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // metroPanel3
            // 
            this.metroPanel3.Controls.Add(this.pullFrameBtn);
            this.metroPanel3.Controls.Add(this.pingNewLidarBtn);
            this.metroPanel3.Controls.Add(this.lidarConfigLogview);
            this.metroPanel3.Controls.Add(this.lidarPicInfoTl);
            this.metroPanel3.HorizontalScrollbarBarColor = true;
            this.metroPanel3.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel3.HorizontalScrollbarSize = 10;
            this.metroPanel3.Location = new System.Drawing.Point(327, 3);
            this.metroPanel3.Name = "metroPanel3";
            this.metroPanel3.Size = new System.Drawing.Size(827, 513);
            this.metroPanel3.TabIndex = 5;
            this.metroPanel3.VerticalScrollbarBarColor = true;
            this.metroPanel3.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel3.VerticalScrollbarSize = 10;
            // 
            // pullFrameBtn
            // 
            this.pullFrameBtn.Location = new System.Drawing.Point(177, 452);
            this.pullFrameBtn.Name = "pullFrameBtn";
            this.pullFrameBtn.Size = new System.Drawing.Size(122, 51);
            this.pullFrameBtn.TabIndex = 5;
            this.pullFrameBtn.Text = "Pull Frane";
            this.pullFrameBtn.Click += new System.EventHandler(this.PullFrameBtn_Click);
            // 
            // pingNewLidarBtn
            // 
            this.pingNewLidarBtn.Location = new System.Drawing.Point(3, 452);
            this.pingNewLidarBtn.Name = "pingNewLidarBtn";
            this.pingNewLidarBtn.Size = new System.Drawing.Size(122, 51);
            this.pingNewLidarBtn.TabIndex = 4;
            this.pingNewLidarBtn.Text = "Ping";
            this.pingNewLidarBtn.Visible = false;
            this.pingNewLidarBtn.Click += new System.EventHandler(this.PingNewLidarBtn_Click);
            // 
            // lidarConfigLogview
            // 
            this.lidarConfigLogview.Location = new System.Drawing.Point(319, 15);
            this.lidarConfigLogview.Multiline = true;
            this.lidarConfigLogview.Name = "lidarConfigLogview";
            this.lidarConfigLogview.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.lidarConfigLogview.Size = new System.Drawing.Size(492, 488);
            this.lidarConfigLogview.TabIndex = 3;
            // 
            // lidarPicInfoTl
            // 
            this.lidarPicInfoTl.Location = new System.Drawing.Point(21, 15);
            this.lidarPicInfoTl.Name = "lidarPicInfoTl";
            this.lidarPicInfoTl.Size = new System.Drawing.Size(218, 165);
            this.lidarPicInfoTl.Style = MetroFramework.MetroColorStyle.White;
            this.lidarPicInfoTl.TabIndex = 2;
            this.lidarPicInfoTl.UseTileImage = true;
            // 
            // metroTabPage6
            // 
            this.metroTabPage6.Controls.Add(this.threeDLidarFLPanel);
            this.metroTabPage6.HorizontalScrollbarBarColor = true;
            this.metroTabPage6.Location = new System.Drawing.Point(4, 35);
            this.metroTabPage6.Name = "metroTabPage6";
            this.metroTabPage6.Size = new System.Drawing.Size(1168, 519);
            this.metroTabPage6.TabIndex = 5;
            this.metroTabPage6.Text = "3D Lidar";
            this.metroTabPage6.VerticalScrollbarBarColor = true;
            // 
            // threeDLidarFLPanel
            // 
            this.threeDLidarFLPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.threeDLidarFLPanel.Location = new System.Drawing.Point(0, 0);
            this.threeDLidarFLPanel.Name = "threeDLidarFLPanel";
            this.threeDLidarFLPanel.Size = new System.Drawing.Size(1168, 519);
            this.threeDLidarFLPanel.TabIndex = 7;
            // 
            // metroTabPage2
            // 
            this.metroTabPage2.HorizontalScrollbarBarColor = true;
            this.metroTabPage2.Location = new System.Drawing.Point(4, 35);
            this.metroTabPage2.Name = "metroTabPage2";
            this.metroTabPage2.Size = new System.Drawing.Size(1168, 519);
            this.metroTabPage2.TabIndex = 1;
            this.metroTabPage2.Text = "Talon Config";
            this.metroTabPage2.VerticalScrollbarBarColor = true;
            // 
            // metroTabPage3
            // 
            this.metroTabPage3.CustomBackground = true;
            this.metroTabPage3.HorizontalScrollbarBarColor = true;
            this.metroTabPage3.Location = new System.Drawing.Point(4, 35);
            this.metroTabPage3.Name = "metroTabPage3";
            this.metroTabPage3.Size = new System.Drawing.Size(1168, 519);
            this.metroTabPage3.TabIndex = 2;
            this.metroTabPage3.Text = "Camera";
            this.metroTabPage3.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTabPage3.VerticalScrollbarBarColor = true;
            // 
            // metroTabPage4
            // 
            this.metroTabPage4.HorizontalScrollbarBarColor = true;
            this.metroTabPage4.Location = new System.Drawing.Point(4, 35);
            this.metroTabPage4.Name = "metroTabPage4";
            this.metroTabPage4.Size = new System.Drawing.Size(1168, 519);
            this.metroTabPage4.TabIndex = 3;
            this.metroTabPage4.Text = "Log View";
            this.metroTabPage4.VerticalScrollbarBarColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(20, 60);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1167, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(97, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.groupBox1);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(1162, 513);
            this.flowLayoutPanel2.TabIndex = 4;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 87.01299F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.98701F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1168, 597);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.startBtn);
            this.flowLayoutPanel3.Controls.Add(this.metroButton1);
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(3, 522);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(1162, 72);
            this.flowLayoutPanel3.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.metroLabel6);
            this.groupBox1.Controls.Add(this.metroLabel5);
            this.groupBox1.Controls.Add(this.metroLabel4);
            this.groupBox1.Controls.Add(this.metroLabel3);
            this.groupBox1.Controls.Add(this.metroLabel2);
            this.groupBox1.Controls.Add(this.metroLabel1);
            this.groupBox1.Controls.Add(this.metroTile1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(344, 242);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // metroTile1
            // 
            this.metroTile1.Location = new System.Drawing.Point(6, 19);
            this.metroTile1.Name = "metroTile1";
            this.metroTile1.Size = new System.Drawing.Size(152, 217);
            this.metroTile1.TabIndex = 0;
            this.metroTile1.Text = "metroTile1";
            this.metroTile1.TileImage = global::Penguin__REMS_Project.Properties.Resources.isckLMS;
            this.metroTile1.UseTileImage = true;
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(177, 19);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(23, 19);
            this.metroLabel1.TabIndex = 1;
            this.metroLabel1.Text = "IP:";
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(175, 106);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(39, 19);
            this.metroLabel2.TabIndex = 2;
            this.metroLabel2.Text = "Type:";
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.Location = new System.Drawing.Point(175, 205);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(39, 19);
            this.metroLabel3.TabIndex = 3;
            this.metroLabel3.Text = "Data:";
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.Location = new System.Drawing.Point(245, 19);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(83, 19);
            this.metroLabel4.TabIndex = 4;
            this.metroLabel4.Text = "metroLabel4";
            // 
            // metroLabel5
            // 
            this.metroLabel5.AutoSize = true;
            this.metroLabel5.Location = new System.Drawing.Point(245, 106);
            this.metroLabel5.Name = "metroLabel5";
            this.metroLabel5.Size = new System.Drawing.Size(83, 19);
            this.metroLabel5.TabIndex = 5;
            this.metroLabel5.Text = "metroLabel5";
            // 
            // metroLabel6
            // 
            this.metroLabel6.AutoSize = true;
            this.metroLabel6.Location = new System.Drawing.Point(245, 205);
            this.metroLabel6.Name = "metroLabel6";
            this.metroLabel6.Size = new System.Drawing.Size(83, 19);
            this.metroLabel6.TabIndex = 6;
            this.metroLabel6.Text = "metroLabel6";
            // 
            // Remsform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1207, 767);
            this.Controls.Add(this.lidarConfigTb);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Remsform";
            this.Text = "Penguin Rems";
            this.TextAlign = System.Windows.Forms.VisualStyles.HorizontalAlign.Right;
            this.lidarConfigTb.ResumeLayout(false);
            this.metroTabPage1.ResumeLayout(false);
            this.metroTabPage5.ResumeLayout(false);
            this.metroTabPage7.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.metroPanel2.ResumeLayout(false);
            this.metroPanel2.PerformLayout();
            this.metroPanel3.ResumeLayout(false);
            this.metroPanel3.PerformLayout();
            this.metroTabPage6.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroTabControl lidarConfigTb;
        private MetroFramework.Controls.MetroTabPage metroTabPage1;
        private MetroFramework.Controls.MetroButton startBtn;
        private MetroFramework.Controls.MetroButton metroButton1;
        private MetroFramework.Controls.MetroTabPage metroTabPage2;
        private MetroFramework.Controls.MetroTabPage metroTabPage3;
        private MetroFramework.Controls.MetroTabPage metroTabPage4;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private MetroFramework.Controls.MetroTabPage metroTabPage5;
        private MetroFramework.Controls.MetroTabPage metroTabPage6;
        private System.Windows.Forms.FlowLayoutPanel sickFLPanel;
        private System.Windows.Forms.FlowLayoutPanel threeDLidarFLPanel;
        private MetroFramework.Controls.MetroTabPage metroTabPage7;
        private MetroFramework.Controls.MetroPanel metroPanel2;
        private MetroFramework.Controls.MetroComboBox lidarTypeCbx;
        private MetroFramework.Controls.MetroLabel metroLabel10;
        private MetroFramework.Controls.MetroButton resetLidarBtn;
        private MetroFramework.Controls.MetroButton addLidarBtn;
        private MetroFramework.Controls.MetroLabel metroLabel7;
        private MetroFramework.Controls.MetroTextBox lidarPortTxt;
        private MetroFramework.Controls.MetroTextBox lidarIPTxt;
        private MetroFramework.Controls.MetroLabel metroLabel8;
        private MetroFramework.Controls.MetroLabel metroLabel9;
        private MetroFramework.Controls.MetroPanel metroPanel3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private MetroFramework.Controls.MetroButton pullFrameBtn;
        private MetroFramework.Controls.MetroButton pingNewLidarBtn;
        private System.Windows.Forms.TextBox lidarConfigLogview;
        private MetroFramework.Controls.MetroTile lidarPicInfoTl;
        private MetroFramework.Controls.MetroComboBox lidarNameCbx;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private MetroFramework.Controls.MetroLabel metroLabel6;
        private MetroFramework.Controls.MetroLabel metroLabel5;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroTile metroTile1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
    }
}

