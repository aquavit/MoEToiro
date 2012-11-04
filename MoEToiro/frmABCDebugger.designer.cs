namespace MoEToiro
{
	partial class frmABCDebugger
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose ( bool disposing )
		{
			if ( disposing && ( components != null ) )
			{
				components.Dispose ();
			}
			base.Dispose ( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent ()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmABCDebugger));
            this.txtDebugText = new System.Windows.Forms.TextBox();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.tsmiOption = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSoundScore = new System.Windows.Forms.ToolStripMenuItem();
            this.timAutoScroll = new System.Windows.Forms.Timer(this.components);
            this.sstMain = new System.Windows.Forms.StatusStrip();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.pnlBottomRight = new System.Windows.Forms.Panel();
            this.dgvPartData = new System.Windows.Forms.DataGridView();
            this.pnlBottomLeft = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlScore = new System.Windows.Forms.Panel();
            this.picMain = new System.Windows.Forms.PictureBox();
            this.pnlKeyBord = new System.Windows.Forms.Panel();
            this.picKeyBord = new System.Windows.Forms.PictureBox();
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.txbtnStop = new System.Windows.Forms.ToolStripButton();
            this.tsbtnPause = new System.Windows.Forms.ToolStripButton();
            this.tsbtnStart = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tslblTime = new System.Windows.Forms.ToolStripLabel();
            this.mnuMain.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.pnlBottomRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPartData)).BeginInit();
            this.pnlBottomLeft.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlScore.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMain)).BeginInit();
            this.pnlKeyBord.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picKeyBord)).BeginInit();
            this.tsMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtDebugText
            // 
            this.txtDebugText.Location = new System.Drawing.Point(4, 4);
            this.txtDebugText.Multiline = true;
            this.txtDebugText.Name = "txtDebugText";
            this.txtDebugText.Size = new System.Drawing.Size(196, 34);
            this.txtDebugText.TabIndex = 0;
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiOption});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(831, 26);
            this.mnuMain.TabIndex = 1;
            this.mnuMain.Text = "menuStrip1";
            // 
            // tsmiOption
            // 
            this.tsmiOption.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSoundScore});
            this.tsmiOption.Name = "tsmiOption";
            this.tsmiOption.Size = new System.Drawing.Size(80, 22);
            this.tsmiOption.Text = "オプション";
            // 
            // tsmiSoundScore
            // 
            this.tsmiSoundScore.Name = "tsmiSoundScore";
            this.tsmiSoundScore.Size = new System.Drawing.Size(100, 22);
            this.tsmiSoundScore.Text = "音源";
            // 
            // timAutoScroll
            // 
            this.timAutoScroll.Interval = 1;
            this.timAutoScroll.Tick += new System.EventHandler(this.timAutoScroll_Tick);
            // 
            // sstMain
            // 
            this.sstMain.Location = new System.Drawing.Point(0, 503);
            this.sstMain.Name = "sstMain";
            this.sstMain.Size = new System.Drawing.Size(831, 22);
            this.sstMain.TabIndex = 6;
            this.sstMain.Text = "statusStrip1";
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.pnlBottomRight);
            this.pnlBottom.Controls.Add(this.pnlBottomLeft);
            this.pnlBottom.Controls.Add(this.panel1);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 339);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(831, 164);
            this.pnlBottom.TabIndex = 7;
            // 
            // pnlBottomRight
            // 
            this.pnlBottomRight.Controls.Add(this.dgvPartData);
            this.pnlBottomRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottomRight.Location = new System.Drawing.Point(200, 10);
            this.pnlBottomRight.Name = "pnlBottomRight";
            this.pnlBottomRight.Size = new System.Drawing.Size(631, 154);
            this.pnlBottomRight.TabIndex = 10;
            // 
            // dgvPartData
            // 
            this.dgvPartData.AllowUserToAddRows = false;
            this.dgvPartData.AllowUserToDeleteRows = false;
            this.dgvPartData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPartData.Dock = System.Windows.Forms.DockStyle.Left;
            this.dgvPartData.Location = new System.Drawing.Point(0, 0);
            this.dgvPartData.Name = "dgvPartData";
            this.dgvPartData.RowTemplate.Height = 21;
            this.dgvPartData.Size = new System.Drawing.Size(348, 154);
            this.dgvPartData.TabIndex = 8;
            this.dgvPartData.TabStop = false;
            // 
            // pnlBottomLeft
            // 
            this.pnlBottomLeft.Controls.Add(this.txtDebugText);
            this.pnlBottomLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlBottomLeft.Location = new System.Drawing.Point(0, 10);
            this.pnlBottomLeft.Name = "pnlBottomLeft";
            this.pnlBottomLeft.Size = new System.Drawing.Size(200, 154);
            this.pnlBottomLeft.TabIndex = 9;
            this.pnlBottomLeft.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(831, 10);
            this.panel1.TabIndex = 5;
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.pnlScore);
            this.pnlMain.Controls.Add(this.pnlKeyBord);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 57);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(831, 282);
            this.pnlMain.TabIndex = 8;
            // 
            // pnlScore
            // 
            this.pnlScore.AutoScroll = true;
            this.pnlScore.Controls.Add(this.picMain);
            this.pnlScore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlScore.Location = new System.Drawing.Point(65, 0);
            this.pnlScore.Name = "pnlScore";
            this.pnlScore.Size = new System.Drawing.Size(766, 282);
            this.pnlScore.TabIndex = 3;
            this.pnlScore.Scroll += new System.Windows.Forms.ScrollEventHandler(this.pnlScore_Scroll);
            // 
            // picMain
            // 
            this.picMain.Location = new System.Drawing.Point(0, 0);
            this.picMain.Name = "picMain";
            this.picMain.Size = new System.Drawing.Size(337, 50);
            this.picMain.TabIndex = 0;
            this.picMain.TabStop = false;
            this.picMain.Paint += new System.Windows.Forms.PaintEventHandler(this.picMain_Paint);
            this.picMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picMain_MouseClick);
            // 
            // pnlKeyBord
            // 
            this.pnlKeyBord.Controls.Add(this.picKeyBord);
            this.pnlKeyBord.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlKeyBord.Location = new System.Drawing.Point(0, 0);
            this.pnlKeyBord.Name = "pnlKeyBord";
            this.pnlKeyBord.Size = new System.Drawing.Size(65, 282);
            this.pnlKeyBord.TabIndex = 2;
            // 
            // picKeyBord
            // 
            this.picKeyBord.Location = new System.Drawing.Point(0, 0);
            this.picKeyBord.Name = "picKeyBord";
            this.picKeyBord.Size = new System.Drawing.Size(65, 50);
            this.picKeyBord.TabIndex = 1;
            this.picKeyBord.TabStop = false;
            this.picKeyBord.Paint += new System.Windows.Forms.PaintEventHandler(this.picKeyBord_Paint);
            // 
            // tsMain
            // 
            this.tsMain.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txbtnStop,
            this.tsbtnPause,
            this.tsbtnStart,
            this.toolStripSeparator1,
            this.tslblTime});
            this.tsMain.Location = new System.Drawing.Point(0, 26);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(831, 31);
            this.tsMain.TabIndex = 9;
            this.tsMain.Text = "toolStrip1";
            // 
            // txbtnStop
            // 
            this.txbtnStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.txbtnStop.Image = ((System.Drawing.Image)(resources.GetObject("txbtnStop.Image")));
            this.txbtnStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.txbtnStop.Name = "txbtnStop";
            this.txbtnStop.Size = new System.Drawing.Size(28, 28);
            this.txbtnStop.Text = "停止";
            this.txbtnStop.Click += new System.EventHandler(this.txbtnStop_Click);
            // 
            // tsbtnPause
            // 
            this.tsbtnPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnPause.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnPause.Image")));
            this.tsbtnPause.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnPause.Name = "tsbtnPause";
            this.tsbtnPause.Size = new System.Drawing.Size(28, 28);
            this.tsbtnPause.Text = "一時停止";
            this.tsbtnPause.Click += new System.EventHandler(this.tsbtnPause_Click);
            // 
            // tsbtnStart
            // 
            this.tsbtnStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnStart.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnStart.Image")));
            this.tsbtnStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnStart.Name = "tsbtnStart";
            this.tsbtnStart.Size = new System.Drawing.Size(28, 28);
            this.tsbtnStart.Text = "開始";
            this.tsbtnStart.Click += new System.EventHandler(this.tsbtnStart_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // tslblTime
            // 
            this.tslblTime.Name = "tslblTime";
            this.tslblTime.Size = new System.Drawing.Size(64, 28);
            this.tslblTime.Text = "00\'00.000";
            // 
            // frmABCDebugger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(831, 525);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.sstMain);
            this.Controls.Add(this.tsMain);
            this.Controls.Add(this.mnuMain);
            this.KeyPreview = true;
            this.MainMenuStrip = this.mnuMain;
            this.Name = "frmABCDebugger";
            this.Text = "frmABCDebugger";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmABCDebugger_FormClosing);
            this.ResizeEnd += new System.EventHandler(this.frmABCDebugger_ResizeEnd);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmABCDebugger_KeyDown);
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottomRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPartData)).EndInit();
            this.pnlBottomLeft.ResumeLayout(false);
            this.pnlBottomLeft.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.pnlScore.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picMain)).EndInit();
            this.pnlKeyBord.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picKeyBord)).EndInit();
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtDebugText;
		private System.Windows.Forms.MenuStrip mnuMain;
		private System.Windows.Forms.ToolStripMenuItem tsmiOption;
		private System.Windows.Forms.ToolStripMenuItem tsmiSoundScore;
		private System.Windows.Forms.Timer timAutoScroll;
		private System.Windows.Forms.StatusStrip sstMain;
		private System.Windows.Forms.Panel pnlBottom;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel pnlMain;
		private System.Windows.Forms.PictureBox picMain;
		private System.Windows.Forms.PictureBox picKeyBord;
		private System.Windows.Forms.Panel pnlScore;
		private System.Windows.Forms.Panel pnlKeyBord;
		private System.Windows.Forms.DataGridView dgvPartData;
		private System.Windows.Forms.Panel pnlBottomRight;
		private System.Windows.Forms.Panel pnlBottomLeft;
		private System.Windows.Forms.ToolStrip tsMain;
		private System.Windows.Forms.ToolStripButton tsbtnStart;
		private System.Windows.Forms.ToolStripButton txbtnStop;
		private System.Windows.Forms.ToolStripButton tsbtnPause;
		private System.Windows.Forms.ToolStripLabel tslblTime;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
	}
}