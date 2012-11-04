namespace MoEToiro
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.restAbsorption = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.restThreshold = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.quantizationMode = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.fusion = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.quantizationDivisor = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.parts = new System.Windows.Forms.TabControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ファイルFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ファイルを開くOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.プロジェクトを保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.終了QToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ヘルプToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.バージョンン情報ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rebuild = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.filesize = new System.Windows.Forms.ToolStripStatusLabel();
            this.optimizeWs = new System.Windows.Forms.CheckBox();
            this.sessionMode = new System.Windows.Forms.ComboBox();
            this.titleText = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.velocity = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.testPlay = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.shortNoteAdjustment = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // restAbsorption
            // 
            this.restAbsorption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.restAbsorption.FormattingEnabled = true;
            this.restAbsorption.Items.AddRange(new object[] {
            "直前の音のテヌートに変換",
            "消去する"});
            this.restAbsorption.Location = new System.Drawing.Point(787, 350);
            this.restAbsorption.Name = "restAbsorption";
            this.restAbsorption.Size = new System.Drawing.Size(170, 20);
            this.restAbsorption.TabIndex = 6;
            this.restAbsorption.SelectedIndexChanged += new System.EventHandler(this.restAbsorption_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(709, 353);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 12);
            this.label7.TabIndex = 32;
            this.label7.Text = "未満の休符を";
            // 
            // restThreshold
            // 
            this.restThreshold.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.restThreshold.FormattingEnabled = true;
            this.restThreshold.Items.AddRange(new object[] {
            "8分音符",
            "16分音符",
            "32分音符",
            "64分音符",
            "単位時間"});
            this.restThreshold.Location = new System.Drawing.Point(616, 350);
            this.restThreshold.Name = "restThreshold";
            this.restThreshold.Size = new System.Drawing.Size(87, 20);
            this.restThreshold.TabIndex = 5;
            this.restThreshold.SelectedIndexChanged += new System.EventHandler(this.restThreshold_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(599, 221);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(132, 12);
            this.label6.TabIndex = 30;
            this.label6.Text = "タイミング合わせのオプション";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(599, 183);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 12);
            this.label5.TabIndex = 28;
            this.label5.Text = "出力サイズ最適化";
            // 
            // quantizationMode
            // 
            this.quantizationMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.quantizationMode.FormattingEnabled = true;
            this.quantizationMode.Items.AddRange(new object[] {
            "全音符～64分・付点・3連・5連",
            "5連符なし",
            "3連・5連符なし",
            "3連・5連符と16分以下の付点なし",
            "3連・5連符と8分以下の付点なし",
            "グリッド単位",
            "制限なし"});
            this.quantizationMode.Location = new System.Drawing.Point(681, 262);
            this.quantizationMode.Name = "quantizationMode";
            this.quantizationMode.Size = new System.Drawing.Size(191, 20);
            this.quantizationMode.TabIndex = 7;
            this.quantizationMode.SelectedIndexChanged += new System.EventHandler(this.quantizationMode_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(611, 265);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 12);
            this.label4.TabIndex = 26;
            this.label4.Text = "音符の長さを";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(709, 322);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(163, 12);
            this.label3.TabIndex = 25;
            this.label3.Text = "未満の間隔の音を和音にまとめる";
            // 
            // fusion
            // 
            this.fusion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fusion.FormattingEnabled = true;
            this.fusion.Items.AddRange(new object[] {
            "8分音符",
            "16分音符",
            "32分音符",
            "64分音符",
            "128分音符",
            "256分音符",
            "単位時間"});
            this.fusion.Location = new System.Drawing.Point(616, 319);
            this.fusion.Name = "fusion";
            this.fusion.Size = new System.Drawing.Size(87, 20);
            this.fusion.TabIndex = 4;
            this.fusion.SelectedIndexChanged += new System.EventHandler(this.fusion_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(743, 239);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 12);
            this.label2.TabIndex = 23;
            this.label2.Text = "境界のグリッドに揃える";
            // 
            // quantizationDivisor
            // 
            this.quantizationDivisor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.quantizationDivisor.FormattingEnabled = true;
            this.quantizationDivisor.Items.AddRange(new object[] {
            "8分音符",
            "16分音符",
            "32分音符",
            "64分音符",
            "制限なし"});
            this.quantizationDivisor.Location = new System.Drawing.Point(650, 236);
            this.quantizationDivisor.Name = "quantizationDivisor";
            this.quantizationDivisor.Size = new System.Drawing.Size(87, 20);
            this.quantizationDivisor.TabIndex = 3;
            this.quantizationDivisor.SelectedIndexChanged += new System.EventHandler(this.quantization_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(599, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 19;
            this.label1.Text = "セッション分割";
            // 
            // parts
            // 
            this.parts.AllowDrop = true;
            this.parts.Location = new System.Drawing.Point(12, 72);
            this.parts.Name = "parts";
            this.parts.SelectedIndex = 0;
            this.parts.Size = new System.Drawing.Size(563, 633);
            this.parts.TabIndex = 0;
            this.parts.SelectedIndexChanged += new System.EventHandler(this.parts_SelectedIndexChanged);
            this.parts.DragDrop += new System.Windows.Forms.DragEventHandler(this.parts_DragDrop);
            this.parts.DragEnter += new System.Windows.Forms.DragEventHandler(this.parts_DragEnter);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルFToolStripMenuItem,
            this.ヘルプToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(994, 26);
            this.menuStrip1.TabIndex = 35;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ファイルFToolStripMenuItem
            // 
            this.ファイルFToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルを開くOToolStripMenuItem,
            this.保存SToolStripMenuItem,
            this.プロジェクトを保存ToolStripMenuItem,
            this.終了QToolStripMenuItem});
            this.ファイルFToolStripMenuItem.Name = "ファイルFToolStripMenuItem";
            this.ファイルFToolStripMenuItem.Size = new System.Drawing.Size(85, 22);
            this.ファイルFToolStripMenuItem.Text = "ファイル(&F)";
            // 
            // ファイルを開くOToolStripMenuItem
            // 
            this.ファイルを開くOToolStripMenuItem.Name = "ファイルを開くOToolStripMenuItem";
            this.ファイルを開くOToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.ファイルを開くOToolStripMenuItem.Text = "ファイルを開く(&O)...";
            this.ファイルを開くOToolStripMenuItem.Click += new System.EventHandler(this.ファイルを開くOToolStripMenuItem_Click);
            // 
            // 保存SToolStripMenuItem
            // 
            this.保存SToolStripMenuItem.Name = "保存SToolStripMenuItem";
            this.保存SToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.保存SToolStripMenuItem.Text = "ABCファイルを保存(&S)...";
            this.保存SToolStripMenuItem.Click += new System.EventHandler(this.保存SToolStripMenuItem_Click);
            // 
            // プロジェクトを保存ToolStripMenuItem
            // 
            this.プロジェクトを保存ToolStripMenuItem.Name = "プロジェクトを保存ToolStripMenuItem";
            this.プロジェクトを保存ToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.プロジェクトを保存ToolStripMenuItem.Text = "ABCプロジェクトを保存(&S)...";
            this.プロジェクトを保存ToolStripMenuItem.Click += new System.EventHandler(this.プロジェクトを保存ToolStripMenuItem_Click);
            // 
            // 終了QToolStripMenuItem
            // 
            this.終了QToolStripMenuItem.Name = "終了QToolStripMenuItem";
            this.終了QToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.終了QToolStripMenuItem.Text = "終了(&Q)";
            this.終了QToolStripMenuItem.Click += new System.EventHandler(this.終了QToolStripMenuItem_Click);
            // 
            // ヘルプToolStripMenuItem
            // 
            this.ヘルプToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.バージョンン情報ToolStripMenuItem});
            this.ヘルプToolStripMenuItem.Name = "ヘルプToolStripMenuItem";
            this.ヘルプToolStripMenuItem.Size = new System.Drawing.Size(56, 22);
            this.ヘルプToolStripMenuItem.Text = "ヘルプ";
            // 
            // バージョンン情報ToolStripMenuItem
            // 
            this.バージョンン情報ToolStripMenuItem.Name = "バージョンン情報ToolStripMenuItem";
            this.バージョンン情報ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.バージョンン情報ToolStripMenuItem.Text = "バージョン情報...";
            this.バージョンン情報ToolStripMenuItem.Click += new System.EventHandler(this.バージョンン情報ToolStripMenuItem_Click);
            // 
            // rebuild
            // 
            this.rebuild.Location = new System.Drawing.Point(601, 445);
            this.rebuild.Name = "rebuild";
            this.rebuild.Size = new System.Drawing.Size(94, 23);
            this.rebuild.TabIndex = 8;
            this.rebuild.Text = "再変換 (&R)";
            this.rebuild.UseVisualStyleBackColor = true;
            this.rebuild.Click += new System.EventHandler(this.rebuild_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.filesize});
            this.statusStrip1.Location = new System.Drawing.Point(0, 707);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(994, 23);
            this.statusStrip1.TabIndex = 37;
            this.statusStrip1.Text = "ファイルサイズ: ";
            // 
            // filesize
            // 
            this.filesize.AutoSize = false;
            this.filesize.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.filesize.Name = "filesize";
            this.filesize.Size = new System.Drawing.Size(979, 18);
            this.filesize.Spring = true;
            this.filesize.Text = "ファイルサイズ:";
            this.filesize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // optimizeWs
            // 
            this.optimizeWs.AutoSize = true;
            this.optimizeWs.Location = new System.Drawing.Point(616, 198);
            this.optimizeWs.Name = "optimizeWs";
            this.optimizeWs.Size = new System.Drawing.Size(115, 16);
            this.optimizeWs.TabIndex = 2;
            this.optimizeWs.Text = "余分な空白を除去";
            this.optimizeWs.UseVisualStyleBackColor = true;
            this.optimizeWs.CheckedChanged += new System.EventHandler(this.optimizeWs_CheckedChanged);
            // 
            // sessionMode
            // 
            this.sessionMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sessionMode.FormattingEnabled = true;
            this.sessionMode.Items.AddRange(new object[] {
            "ソロ (メロディ)",
            "ソロ (ドラム)",
            "ソロ (パーカッション)",
            "トリオ (メロディ+ドラム+パーカッション)",
            "セッション",
            "セッション (重複音をトラック分割)"});
            this.sessionMode.Location = new System.Drawing.Point(601, 87);
            this.sessionMode.Name = "sessionMode";
            this.sessionMode.Size = new System.Drawing.Size(216, 20);
            this.sessionMode.TabIndex = 38;
            this.sessionMode.SelectedIndexChanged += new System.EventHandler(this.sessionMode_SelectedIndexChanged);
            // 
            // titleText
            // 
            this.titleText.Location = new System.Drawing.Point(58, 47);
            this.titleText.Name = "titleText";
            this.titleText.Size = new System.Drawing.Size(350, 19);
            this.titleText.TabIndex = 41;
            this.titleText.TextChanged += new System.EventHandler(this.titleText_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 50);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 12);
            this.label9.TabIndex = 42;
            this.label9.Text = "タイトル";
            // 
            // velocity
            // 
            this.velocity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.velocity.FormattingEnabled = true;
            this.velocity.Items.AddRange(new object[] {
            "出力しない",
            "チョーいいかげん (50刻み)",
            "すごくいいかげん (20刻み)",
            "いいかげん (10刻み)",
            "正確に出力"});
            this.velocity.Location = new System.Drawing.Point(601, 133);
            this.velocity.Name = "velocity";
            this.velocity.Size = new System.Drawing.Size(216, 20);
            this.velocity.TabIndex = 44;
            this.velocity.SelectedIndexChanged += new System.EventHandler(this.velocity_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(599, 118);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 12);
            this.label10.TabIndex = 43;
            this.label10.Text = "音量";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(611, 239);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 12);
            this.label8.TabIndex = 45;
            this.label8.Text = "音符を";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(878, 265);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(33, 12);
            this.label11.TabIndex = 46;
            this.label11.Text = "にする";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(599, 300);
            this.label12.Name = "label12";
            this.label12.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label12.Size = new System.Drawing.Size(95, 12);
            this.label12.TabIndex = 47;
            this.label12.Text = "細かい間隔の調整";
            // 
            // testPlay
            // 
            this.testPlay.Location = new System.Drawing.Point(601, 485);
            this.testPlay.Name = "testPlay";
            this.testPlay.Size = new System.Drawing.Size(94, 23);
            this.testPlay.TabIndex = 48;
            this.testPlay.Text = "試演(&T)";
            this.testPlay.UseVisualStyleBackColor = true;
            this.testPlay.Click += new System.EventHandler(this.testPlay_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(614, 384);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(101, 12);
            this.label13.TabIndex = 49;
            this.label13.Text = "64分より短い音符を";
            // 
            // shortNoteAdjustment
            // 
            this.shortNoteAdjustment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.shortNoteAdjustment.FormattingEnabled = true;
            this.shortNoteAdjustment.Items.AddRange(new object[] {
            "切り捨てる",
            "64分音符にする"});
            this.shortNoteAdjustment.Location = new System.Drawing.Point(721, 381);
            this.shortNoteAdjustment.Name = "shortNoteAdjustment";
            this.shortNoteAdjustment.Size = new System.Drawing.Size(114, 20);
            this.shortNoteAdjustment.TabIndex = 50;
            this.shortNoteAdjustment.SelectedIndexChanged += new System.EventHandler(this.shortNoteAdjustment_SelectedIndexChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(638, 405);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(251, 12);
            this.label14.TabIndex = 51;
            this.label14.Text = "※「音符の長さをグリッド単位に」した場合にだけ有効";
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 730);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.shortNoteAdjustment);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.testPlay);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.velocity);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.titleText);
            this.Controls.Add(this.sessionMode);
            this.Controls.Add(this.optimizeWs);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.rebuild);
            this.Controls.Add(this.parts);
            this.Controls.Add(this.restAbsorption);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.restThreshold);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.quantizationMode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.fusion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.quantizationDivisor);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "もえといろ (MIDI → MoE-ABC変換ツール)";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox restAbsorption;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox restThreshold;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox quantizationMode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox fusion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox quantizationDivisor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl parts;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ファイルFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ファイルを開くOToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存SToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem プロジェクトを保存ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 終了QToolStripMenuItem;
        private System.Windows.Forms.Button rebuild;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.CheckBox optimizeWs;
        private System.Windows.Forms.ComboBox sessionMode;
        private System.Windows.Forms.ToolStripStatusLabel filesize;
        private System.Windows.Forms.TextBox titleText;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox velocity;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button testPlay;
        private System.Windows.Forms.ToolStripMenuItem ヘルプToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem バージョンン情報ToolStripMenuItem;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox shortNoteAdjustment;
        private System.Windows.Forms.Label label14;
    }
}

