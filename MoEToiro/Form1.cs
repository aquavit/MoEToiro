using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using Midi;

namespace MoEToiro
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void 終了ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings s = Properties.Settings.Default;
            s.Save();
            Application.Exit();
        }

        private string openFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        private string saveFolder= Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        private string batchSave = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "もえといろ");
        private MidiSequence currentMidiFile = null;
        private MoEABCScore.MoEABCScore currentScore = null;
        private void ファイルを開くOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings s = Properties.Settings.Default;
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "MIDIファイル(*.mid)|*.mid|すべてのファイル(*.*)|*.*";
            dlg.InitialDirectory = openFolder;
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                openFolder = System.IO.Path.GetDirectoryName(dlg.FileName);
                processFiles(dlg.FileNames);
                s.openFrom = openFolder;
                s.Save();
            }
            
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] fns = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            processFiles(fns);
        }

        private void 終了QToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private string guessMoEScoreFolder()
        {
            string[] candidates = {
                                      "C:\\Willoo\\Master of Epic\\userdata\\music",
                                      "C:\\Rosso Index\\Master of Epic\\userdata\\music",
                                      "C:\\Gonzo Rosso\\Master of Epic\\userdata\\music",
                                      "C:\\Program Files\\Rosso Index\\Master of Epic\\userdata\\music",
                                      "C:\\Program Files\\Gonzo Rosso\\Master of Epic\\userdata\\music",
                                      "C:\\Program Files\\Hudson\\MasterofEpic\\userdata\\music"
                                  };
            foreach (string dir in candidates)
            {
                if (Directory.Exists(dir))
                    return dir;
            }
            return null;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Properties.Settings s = Properties.Settings.Default;
            if (Directory.Exists(s.saveTo))
            {
                saveFolder = s.saveTo;
            }
            else
            {
                string guess = guessMoEScoreFolder();
                if (guess != null)
                {
                    saveFolder = guess;
                }
                s.saveTo = saveFolder;
                s.Save();
            }
            if (Directory.Exists(s.openFrom))
            {
                openFolder = s.openFrom;
            }
            
            MidiSequence.optimizeWs = s.optimizeWs;
            MidiSequence.quantizationDivisor = s.quantizationDivisor;
            MidiSequence.fusionThreshold = s.fusionThreshold;
            try
            {
                MidiSequence.quantizationMode = (MidiSequence.QuantizationMode)Enum.Parse(typeof(MidiSequence.QuantizationMode), s.quantizationMode);
            }
            catch (Exception)
            {
                MidiSequence.quantizationMode = MidiSequence.QuantizationMode.QuantumStep;
            }
            MidiSequence.restRemovalThreshold = s.restRemovalThreshold;
            try
            {
                MidiSequence.restRemovalMode = (MidiSequence.RestRemovalMode)Enum.Parse(typeof(MidiSequence.RestRemovalMode), s.restRemovalMode);
            }
            catch (Exception)
            {
                MidiSequence.restRemovalMode = MidiSequence.RestRemovalMode.AbsorbToLeadingNote;
            }

            try
            {
                MidiSequence.partDivisionPolicy = (MidiSequence.PartDivisionPolicy)Enum.Parse(typeof(MidiSequence.PartDivisionPolicy), s.partDivisionPolicy);
            }
            catch (Exception)
            {
                MidiSequence.partDivisionPolicy = MidiSequence.PartDivisionPolicy.Session;
            }
            try
            {
                MidiSequence.velocityKeepingPolicy = (MidiSequence.VelocityKeepingPolicy)Enum.Parse(typeof(MidiSequence.VelocityKeepingPolicy), s.velocityKeepingPolicy);
            }
            catch (Exception)
            {
                MidiSequence.velocityKeepingPolicy = MidiSequence.VelocityKeepingPolicy.None;
            }
            try
            {
                MidiSequence.shortNoteAdjustment = (MidiSequence.ShortNoteAdjustment)Enum.Parse(typeof(MidiSequence.ShortNoteAdjustment), s.shortNoteAdjustment);
            }
            catch (Exception)
            {
                MidiSequence.shortNoteAdjustment = MidiSequence.ShortNoteAdjustment.Discard;
            }
            addTabPage("パート #1", "");
            updateControls();

        }

        private void updateControls()
        {
            optimizeWs.Checked = MidiSequence.optimizeWs;

            switch (MidiSequence.quantizationDivisor)
            {
                case 2:
                    quantizationDivisor.SelectedIndex = 0;
                    break;
                case 4:
                    quantizationDivisor.SelectedIndex = 1;
                    break;
                case 8:
                    quantizationDivisor.SelectedIndex = 2;
                    break;
                case 16:
                    quantizationDivisor.SelectedIndex = 3;
                    break;
                case 0:
                    quantizationDivisor.SelectedIndex = 4;
                    break;
                default:
                    quantizationDivisor.SelectedIndex = 2;
                    break;
            }

            switch (MidiSequence.fusionThreshold)
            {
                case 2:
                    fusion.SelectedIndex = 0;
                    break;
                case 4:
                    fusion.SelectedIndex = 1;
                    break;
                case 8:
                    fusion.SelectedIndex = 2;
                    break;
                case 16:
                    fusion.SelectedIndex = 3;
                    break;
                case 32:
                    fusion.SelectedIndex = 4;
                    break;
                case 64:
                    fusion.SelectedIndex = 5;
                    break;
                case 0:
                    fusion.SelectedIndex = 6;
                    break;
                default:
                    fusion.SelectedIndex = 3;
                    break;
            }

            bool enableShortNoteAdjustment = true;
            switch (MidiSequence.quantizationMode)
            {
                case MidiSequence.QuantizationMode.ScoreLike:
                    quantizationMode.SelectedIndex = 0;
                    break;
                case MidiSequence.QuantizationMode.No5th:
                    quantizationMode.SelectedIndex = 1;
                    break;
                case MidiSequence.QuantizationMode.No3rd5th:
                    quantizationMode.SelectedIndex = 2;
                    break;
                case MidiSequence.QuantizationMode.NoDotBelow16th:
                    quantizationMode.SelectedIndex = 3;
                    break;
                case MidiSequence.QuantizationMode.NoDotBelow8th:
                    quantizationMode.SelectedIndex = 4;
                    break;
                case MidiSequence.QuantizationMode.QuantumStep:
                    quantizationMode.SelectedIndex = 5;
                    enableShortNoteAdjustment = true;
                    break;
                case MidiSequence.QuantizationMode.NoRestriction:
                    quantizationMode.SelectedIndex = 6;
                    break;
                default:
                    quantizationMode.SelectedIndex = 5;
                    break;

            }
            shortNoteAdjustment.Enabled = enableShortNoteAdjustment;
            Queue<int> q = new Queue<int>();
            
            switch (MidiSequence.restRemovalThreshold)
            {
                case 2:
                    restThreshold.SelectedIndex = 0;
                    break;
                case 4:
                    restThreshold.SelectedIndex = 1;
                    break;
                case 8:
                    restThreshold.SelectedIndex = 2;
                    break;
                case 16:
                    restThreshold.SelectedIndex = 3;
                    break;
                case 0:
                    restThreshold.SelectedIndex = 4;
                    break;
            }

            switch (MidiSequence.restRemovalMode)
            {
                case MidiSequence.RestRemovalMode.AbsorbToLeadingNote:
                    restAbsorption.SelectedIndex = 0;
                    break;
                case MidiSequence.RestRemovalMode.Remove:
                    restAbsorption.SelectedIndex = 1;
                    break;
            }

            switch (MidiSequence.partDivisionPolicy)
            {
                case MidiSequence.PartDivisionPolicy.SoloMelody:
                    sessionMode.SelectedIndex = 0;
                    break;
                case MidiSequence.PartDivisionPolicy.DrumOnly:
                    sessionMode.SelectedIndex = 1;
                    break;
                case MidiSequence.PartDivisionPolicy.PercussionOnly:
                    sessionMode.SelectedIndex = 2;
                    break;
                case MidiSequence.PartDivisionPolicy.Trio:
                    sessionMode.SelectedIndex = 3;
                    break;
                case MidiSequence.PartDivisionPolicy.Session:
                    sessionMode.SelectedIndex = 4;
                    break;
                case MidiSequence.PartDivisionPolicy.SessionWithPartitioning:
                    sessionMode.SelectedIndex = 5;
                    break;
            }

            switch (MidiSequence.velocityKeepingPolicy)
            {
                case MidiSequence.VelocityKeepingPolicy.None:
                    velocity.SelectedIndex = 0;
                    break;
                case MidiSequence.VelocityKeepingPolicy.RoundBy50:
                    velocity.SelectedIndex = 1;
                    break;
                case MidiSequence.VelocityKeepingPolicy.RoundBy20:
                    velocity.SelectedIndex = 2;
                    break;
                case MidiSequence.VelocityKeepingPolicy.RoundBy10:
                    velocity.SelectedIndex = 3;
                    break;
                case MidiSequence.VelocityKeepingPolicy.Literal:
                    velocity.SelectedIndex = 4;
                    break;
                default:
                    velocity.SelectedIndex = 0;
                    break;
            }

            switch (MidiSequence.shortNoteAdjustment) {
                case MidiSequence.ShortNoteAdjustment.Discard:
                    shortNoteAdjustment.SelectedIndex = 0;
                    break;
                case MidiSequence.ShortNoteAdjustment.Adjust:
                    shortNoteAdjustment.SelectedIndex = 1;
                    break;
                default:
                    shortNoteAdjustment.SelectedIndex = 0;
                    break;
            }
        }

        private string makeTitle(MidiSequence.MidiTrack track)
        {
            string inst = track.InstrumentName;
            if (inst == null || inst.Length == 0)
            {
                inst = "_パート #" + track.TrackNumber;
            }
            else
            {
                inst += "_" + track.TrackNumber;
            }

            return this.titleText.Text + "_" + inst;
        }
        private void addTrack(MidiSequence.MidiTrack track)
        {
            string title = makeTitle(track);

            TabPage p = addTabPage(title, track.toMoEAbc());
            p.Tag = track;
        }

        private TabPage addTabPage(string title, string text)
        {
            TabPage p = new TabPage(title);
            parts.TabPages.Add(p);
            p.AllowDrop = true;
            p.DragEnter += Form1_DragEnter;
            p.DragDrop += Form1_DragDrop;

            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.FlowDirection = FlowDirection.TopDown;
            p.Controls.Add(panel);
            panel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel.Dock = DockStyle.Fill;

            TextBox titletb = new TextBox();
            titletb.Name = "title";
            titletb.Text = title;
            titletb.TextChanged += (sender, obj) => 
            {
                p.Text = titletb.Text;
            };
            titletb.Width = panel.ClientSize.Width * 10 / 8;
            panel.Controls.Add(titletb);

            RichTextBox tb = new RichTextBox();
            tb.Name = "score";
            tb.Text = text;
            tb.Multiline = true;
            tb.AcceptsTab = true;
            tb.MaxLength = 0;
            tb.TextChanged += (sender, obj) => { showFileSize(); };
            tb.AllowDrop = true;
            tb.DragEnter += Form1_DragEnter;
            tb.DragDrop += Form1_DragDrop;
            panel.Controls.Add(tb);

            tb.Size = new Size(panel.ClientSize.Width - 8, panel.ClientSize.Height - (titletb.Size.Height + 16)); ;

            return p;
        }
        private void doRebuild()
        {
            if (currentMidiFile == null)
                return;

            int select = parts.SelectedIndex;
            if (select < 0)
                select = 0;
            parts.TabPages.Clear();
            currentMidiFile.recalc();
            foreach (MidiSequence.MidiTrack track in currentMidiFile.Tracks)
            {
                if (track.isUsable())
                {
                    addTrack(track);
                }
            }
            if (select >= 0 && select < parts.TabPages.Count)
            {
                parts.SelectedIndex = select;
            }
            showFileSize();
            currentScore = (MoEABCScore.MoEABCScore)currentMidiFile.Score.Clone();
            //StreamWriter w = new StreamWriter(@"j:\debug.txt");
            //w.Write(currentScore.ToString());
            //w.Close();
        }
        public void openFile(string fileName)
        {
            try
            {
                currentMidiFile = new MidiSequence(fileName);
                titleText.Text = Path.GetFileNameWithoutExtension(fileName);
                doRebuild();
            }
            catch (Exception)
            {
                filesize.ForeColor = Color.Red;
                filesize.Text = fileName + ": これはMIDIじゃないよ";
            }
        }
        public void processFiles(string[] fileNames)
        {
            if (fileNames == null || fileNames.Length == 0)
                return;
            if (fileNames.Length == 1)
            {
                openFile(fileNames[0]);
                return;
            }

            string batchdir = Path.Combine(saveFolder, "batch");
            if (!Directory.Exists(batchdir))
            {
                Directory.CreateDirectory(batchdir);
            }
            foreach (string fn in fileNames)
            {
                try
                {
                    openFile(fn);
                    string path = Path.Combine(batchdir, Path.GetFileNameWithoutExtension(fn));
                    path = Path.ChangeExtension(fn, "abcp");
                    saveProject(path);
                }
                catch (Exception)
                {
                }
            }
        }

        private void rebuild_Click(object sender, EventArgs e)
        {
            doRebuild();
        }

        private void 保存SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Control[] cs = this.parts.SelectedTab.Controls.Find("score", true);
            if (cs == null || cs.Length <= 0)
            {
                MessageBox.Show("パートの楽譜が見つかりません", "予期しないエラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string t = cs[0].Text;
            if (t == null)
                t = "";

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "ABCファイル(*.abc)|*.abc|テキストファイル(*.txt)|*.txt|すべてのファイル(*.*)|*.*";
            cs = this.parts.SelectedTab.Controls.Find("title", true);
            if (cs == null || cs.Length == 0)
                dlg.FileName = "music.abc";
            else
            {
                Control c = cs[0];
                dlg.FileName = c.Text + ".abc";
            }
            dlg.InitialDirectory = saveFolder;
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                saveFolder = Path.GetDirectoryName(dlg.FileName);
                Properties.Settings s = Properties.Settings.Default;
                s.saveTo = saveFolder;
                try
                {
                    StreamWriter w = new StreamWriter(dlg.FileName, false, Encoding.GetEncoding("shift_jis"));
                    using (w)
                    {
                        w.Write(t);
                    }
                    s.Save();                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void saveProject(string path)
        {
            try
            {
                StreamWriter w = new StreamWriter(path, false, Encoding.GetEncoding("shift_jis"));
                using (w)
                {
                    w.WriteLine("[Public]");
                    w.WriteLine("Solo=255");
                    for (int i = 0; i < parts.TabPages.Count; i++)
                    {
                        Control[] cs = parts.TabPages[i].Controls.Find("score", true);
                        if (cs == null || cs.Length <= 0)
                        {
                            filesize.Text = "予期しないエラー: パートの楽譜が見つかりません";
                            filesize.ForeColor = Color.Red;
                        }
                        string t = cs[0].Text;
                        string filename = "パート" + (i + 1);
                        try
                        {
                            filename = parts.TabPages[i].Text;
                            filename = Path.ChangeExtension(filename, ".abc");
                            string filepath = Path.Combine(Path.GetDirectoryName(path), filename);
                            StreamWriter wp = new StreamWriter(filepath, false, Encoding.GetEncoding("shift_jis"));
                            using (wp)
                            {
                                wp.Write(t);
                            }
                        }
                        catch (Exception)
                        {
                            try
                            {
                                filename = "パート" + (i + 1);
                                filename = Path.ChangeExtension(filename, ".abc");
                                string filepath = Path.Combine(Path.GetDirectoryName(path), filename);
                                StreamWriter wp = new StreamWriter(filepath, false, Encoding.GetEncoding("shift_jis"));
                                using (wp)
                                {
                                    wp.Write(t);
                                }
                            }
                            catch (Exception)
                            {
                                filesize.Text = "エラー: パートの保存に失敗しました";
                                filesize.ForeColor = Color.Red;
                            }
                        }
                        int program = 0;
                        if (parts.TabPages[i].Tag != null && parts.TabPages[i].Tag is MidiSequence.MidiTrack)
                        {
                            program = Math.Max(((MidiSequence.MidiTrack)parts.TabPages[i].Tag).Instrument, 0);
                        }
                        w.WriteLine("[Tab" + (i + 1) + "]");
                        w.WriteLine("TabName=" + filename);
                        w.WriteLine("Program=" + program);
                        w.WriteLine("Session=1");
                        w.WriteLine("FilePath=.\\" + filename);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void プロジェクトを保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "ABCプロジェクトファイル(*.abcp)|*.abcp|テキストファイル(*.txt)|*.txt|すべてのファイル(*.*)|*.*";
            dlg.FileName = "session.abcp";
            if (currentMidiFile != null)
            {
                if (titleText.Text.Length > 0)
                    dlg.FileName = titleText.Text + ".abcp";
                else
                    dlg.FileName = Path.GetFileNameWithoutExtension(currentMidiFile.FileName) + ".abcp";
            }
            dlg.InitialDirectory = saveFolder;
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                saveFolder = Path.GetDirectoryName(dlg.FileName);
                Properties.Settings s = Properties.Settings.Default;
                s.saveTo = saveFolder;
                saveProject(dlg.FileName);
                s.Save();
            }
        }

        private void optimizeWs_CheckedChanged(object sender, EventArgs e)
        {
            
            MidiSequence.optimizeWs = optimizeWs.Checked;
            Properties.Settings s = Properties.Settings.Default;
            s.optimizeWs = optimizeWs.Checked;
            s.Save();
        }

        private void quantization_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings s = Properties.Settings.Default;
            switch (quantizationDivisor.SelectedIndex)
            {
                case 0:
                    MidiSequence.quantizationDivisor = 2;
                    s.quantizationDivisor = 2;
                    break;
                case 1:
                    MidiSequence.quantizationDivisor = 4;
                    s.quantizationDivisor = 4;
                    break;
                case 2:
                    MidiSequence.quantizationDivisor = 8;
                    s.quantizationDivisor = 8;
                    break;
                case 3:
                    MidiSequence.quantizationDivisor = 16;
                    s.quantizationDivisor = 16;
                    break;
                case 4:
                    MidiSequence.quantizationDivisor = 0;
                    s.quantizationDivisor = 0;
                    break;
                default:
                    MidiSequence.quantizationDivisor = 16;
                    s.quantizationDivisor = 16;
                    break;
            }
            s.Save();
        }

        private void fusion_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings s = Properties.Settings.Default;
            switch (fusion.SelectedIndex)
            {
                case 0:
                    MidiSequence.fusionThreshold = 2;
                    s.fusionThreshold = 2;
                    break;
                case 1:
                    MidiSequence.fusionThreshold = 4;
                    s.fusionThreshold = 4;
                    break;
                case 2:
                    MidiSequence.fusionThreshold = 8;
                    s.fusionThreshold = 8;
                    break;
                case 3:
                    MidiSequence.fusionThreshold = 16;
                    s.fusionThreshold = 16;
                    break;
                case 4:
                    MidiSequence.fusionThreshold = 32;
                    s.fusionThreshold = 32;
                    break;
                case 5:
                    MidiSequence.fusionThreshold = 64;
                    s.fusionThreshold = 64;
                    break;
                case 6:
                    MidiSequence.fusionThreshold = 0;
                    s.fusionThreshold = 0;
                    break;
                default:
                    MidiSequence.fusionThreshold = 0;
                    s.fusionThreshold = 0;
                    break;
            }
            s.Save();
        }

        private void restThreshold_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings s = Properties.Settings.Default;
            switch (restThreshold.SelectedIndex)
            {
                case 0:
                    MidiSequence.restRemovalThreshold = 2;
                    s.restRemovalThreshold = 2;
                    break;
                case 1:
                    MidiSequence.restRemovalThreshold = 4;
                    s.restRemovalThreshold = 4;
                    break;
                case 2:
                    MidiSequence.restRemovalThreshold = 8;
                    s.restRemovalThreshold = 8;
                    break;
                case 3:
                    MidiSequence.restRemovalThreshold = 16;
                    s.restRemovalThreshold = 16;
                    break;
                case 4:
                    MidiSequence.restRemovalThreshold = 0;
                    s.restRemovalThreshold = 0;
                    break;
                default:
                    MidiSequence.restRemovalThreshold = 0;
                    s.restRemovalThreshold = 0;
                    break;
            }
            s.Save();
        }

        private void restAbsorption_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings s = Properties.Settings.Default;
            switch (restAbsorption.SelectedIndex)
            {
                case 0:
                    MidiSequence.restRemovalMode = MidiSequence.RestRemovalMode.AbsorbToLeadingNote;
                    s.restRemovalMode = MidiSequence.RestRemovalMode.AbsorbToLeadingNote.ToString();
                    break;
                case 1:
                    MidiSequence.restRemovalMode = MidiSequence.RestRemovalMode.Remove;
                    s.restRemovalMode = MidiSequence.RestRemovalMode.Remove.ToString();
                    break;
                default:
                    MidiSequence.restRemovalMode = MidiSequence.RestRemovalMode.AbsorbToLeadingNote;
                    s.restRemovalMode = MidiSequence.RestRemovalMode.AbsorbToLeadingNote.ToString();
                    break;
            }
            s.Save();
        }

        private void quantizationMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings s = Properties.Settings.Default;
            bool enableShortNoteAdjustment = false;
            switch (quantizationMode.SelectedIndex)
            {
                case 0:
                    MidiSequence.quantizationMode = MidiSequence.QuantizationMode.ScoreLike;
                    s.quantizationMode = MidiSequence.QuantizationMode.ScoreLike.ToString();
                    break;
                case 1:
                    MidiSequence.quantizationMode = MidiSequence.QuantizationMode.No5th;
                    s.quantizationMode = MidiSequence.QuantizationMode.No5th.ToString();
                    break;
                case 2:
                    MidiSequence.quantizationMode = MidiSequence.QuantizationMode.No3rd5th;
                    s.quantizationMode = MidiSequence.QuantizationMode.No3rd5th.ToString();
                    break;
                case 3:
                    MidiSequence.quantizationMode = MidiSequence.QuantizationMode.NoDotBelow16th;
                    s.quantizationMode = MidiSequence.QuantizationMode.NoDotBelow16th.ToString();
                    break;
                case 4:
                    MidiSequence.quantizationMode = MidiSequence.QuantizationMode.NoDotBelow8th;
                    s.quantizationMode = MidiSequence.QuantizationMode.NoDotBelow8th.ToString();
                    break;
                case 5:
                    MidiSequence.quantizationMode = MidiSequence.QuantizationMode.QuantumStep;
                    s.quantizationMode = MidiSequence.QuantizationMode.QuantumStep.ToString();
                    enableShortNoteAdjustment = true;
                    break;
                case 6:
                    MidiSequence.quantizationMode = MidiSequence.QuantizationMode.NoRestriction;
                    s.quantizationMode = MidiSequence.QuantizationMode.NoDotBelow8th.ToString();
                    break;
                default:
                    MidiSequence.quantizationMode = MidiSequence.QuantizationMode.QuantumStep;
                    s.quantizationMode = MidiSequence.QuantizationMode.QuantumStep.ToString();
                    break;
            }
            shortNoteAdjustment.Enabled = enableShortNoteAdjustment;
            s.Save();
        }

        private void parts_SelectedIndexChanged(object sender, EventArgs e)
        {
            showFileSize();
        }

        private void showFileSize()
        {
            if (this.parts.SelectedIndex < 0 || this.parts.SelectedTab == null)
                return;
            Control[] cs = this.parts.SelectedTab.Controls.Find("score", true);
            if (cs == null || cs.Length <= 0)
            {
                return;
            }
            string t = cs[0].Text;
            if (t == null)
                t = "";
            int len = Encoding.Default.GetBytes(t).Length;
            filesize.Text = "ファイルサイズ:" + len + " バイト";
            if (len > 20000)
            {
                filesize.ForeColor = Color.Red;
            }
            else
            {
                filesize.ForeColor = Color.Black;
            }
        }
        private void sessionMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings s = Properties.Settings.Default;
            switch (sessionMode.SelectedIndex)
            {
                case 0:
                    MidiSequence.partDivisionPolicy = MidiSequence.PartDivisionPolicy.SoloMelody;
                    s.partDivisionPolicy = MidiSequence.PartDivisionPolicy.SoloMelody.ToString();
                    break;
                case 1:
                    MidiSequence.partDivisionPolicy = MidiSequence.PartDivisionPolicy.DrumOnly;
                    s.partDivisionPolicy = MidiSequence.PartDivisionPolicy.DrumOnly.ToString();
                    break;
                case 2:
                    MidiSequence.partDivisionPolicy = MidiSequence.PartDivisionPolicy.PercussionOnly;
                    s.partDivisionPolicy = MidiSequence.PartDivisionPolicy.PercussionOnly.ToString();
                    break;
                case 3:
                    MidiSequence.partDivisionPolicy = MidiSequence.PartDivisionPolicy.Trio;
                    s.partDivisionPolicy = MidiSequence.PartDivisionPolicy.Trio.ToString();
                    break;
                case 4:
                    MidiSequence.partDivisionPolicy = MidiSequence.PartDivisionPolicy.Session;
                    s.partDivisionPolicy = MidiSequence.PartDivisionPolicy.Session.ToString();
                    break;
                case 5:
                    MidiSequence.partDivisionPolicy = MidiSequence.PartDivisionPolicy.SessionWithPartitioning;
                    s.partDivisionPolicy = MidiSequence.PartDivisionPolicy.SessionWithPartitioning.ToString();
                    break;
                default:
                    MidiSequence.partDivisionPolicy = MidiSequence.PartDivisionPolicy.Session;
                    s.partDivisionPolicy = MidiSequence.PartDivisionPolicy.Session.ToString();
                    break;
            }
            s.Save();
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }

        }

        private void parts_DragDrop(object sender, DragEventArgs e)
        {
            Form1_DragDrop(sender, e);
        }

        private void parts_DragEnter(object sender, DragEventArgs e)
        {
            Form1_DragEnter(sender, e);
        }

        private void titleText_TextChanged(object sender, EventArgs e)
        {
            foreach (TabPage tp in this.parts.TabPages)
            {
                if (tp.Tag != null && tp.Tag is MidiSequence.MidiTrack)
                {
                    Control[] cs = tp.Controls.Find("title", true);
                    if (cs == null || cs.Length == 0)
                        continue;
                    Control c = cs[0];

                    MidiSequence.MidiTrack track = (MidiSequence.MidiTrack)tp.Tag;

                    c.Text = makeTitle(track);
                }
            }


        }

        private void velocity_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings s = Properties.Settings.Default;
            switch (velocity.SelectedIndex)
            {
                case 0:
                    MidiSequence.velocityKeepingPolicy = MidiSequence.VelocityKeepingPolicy.None;
                    s.velocityKeepingPolicy = MidiSequence.VelocityKeepingPolicy.None.ToString();
                    break;
                case 1:
                    MidiSequence.velocityKeepingPolicy = MidiSequence.VelocityKeepingPolicy.RoundBy50;
                    s.velocityKeepingPolicy = MidiSequence.VelocityKeepingPolicy.RoundBy50.ToString();
                    break;
                case 2:
                    MidiSequence.velocityKeepingPolicy = MidiSequence.VelocityKeepingPolicy.RoundBy20;
                    s.velocityKeepingPolicy = MidiSequence.VelocityKeepingPolicy.RoundBy20.ToString();
                    break;
                case 3:
                    MidiSequence.velocityKeepingPolicy = MidiSequence.VelocityKeepingPolicy.RoundBy10;
                    s.velocityKeepingPolicy = MidiSequence.VelocityKeepingPolicy.RoundBy10.ToString();
                    break;
                case 4:
                    MidiSequence.velocityKeepingPolicy = MidiSequence.VelocityKeepingPolicy.Literal;
                    s.velocityKeepingPolicy = MidiSequence.VelocityKeepingPolicy.Literal.ToString();
                    break;
                default:
                    MidiSequence.velocityKeepingPolicy = MidiSequence.VelocityKeepingPolicy.None;
                    s.velocityKeepingPolicy = MidiSequence.VelocityKeepingPolicy.None.ToString();
                    break;
            }

        }

        private void testPlay_Click(object sender, EventArgs e)
        {
            if (currentScore == null)
                return;
            frmABCDebugger form = new frmABCDebugger(currentScore);
            if (form.IsDisposed)
                return;
            form.Show(this);
        }

        private void バージョンン情報ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAboutDialog();
        }

        private void ShowAboutDialog() { 
            Assembly entryAssembly = Assembly.GetEntryAssembly(); 
            object[] copyrightArray = entryAssembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false); 
            string copyright; 
            if ((copyrightArray != null) && (copyrightArray.Length > 0)) 
            { 
                copyright = ((AssemblyCopyrightAttribute)copyrightArray[0]).Copyright; 
            } 
            else 
            { 
                copyright = ""; 
            } 
            string message = string.Format("{0} Version {1}\n{2}", Application.ProductName, Application.ProductVersion, copyright); 
            MessageBox.Show(message, "バージョン情報", MessageBoxButtons.OK, MessageBoxIcon.None); 
        }

        private void shortNoteAdjustment_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings s = Properties.Settings.Default;
            switch (shortNoteAdjustment.SelectedIndex)
            {
                case 0:
                    MidiSequence.shortNoteAdjustment = MidiSequence.ShortNoteAdjustment.Discard;
                    s.shortNoteAdjustment = MidiSequence.ShortNoteAdjustment.Discard.ToString();
                    break;
                case 1:
                    MidiSequence.shortNoteAdjustment = MidiSequence.ShortNoteAdjustment.Adjust;
                    s.shortNoteAdjustment = MidiSequence.ShortNoteAdjustment.Adjust.ToString();
                    break;
                default:
                    MidiSequence.shortNoteAdjustment = MidiSequence.ShortNoteAdjustment.Discard;
                    s.shortNoteAdjustment = MidiSequence.ShortNoteAdjustment.Discard.ToString();
                    break;
            }

        }
    }
}
