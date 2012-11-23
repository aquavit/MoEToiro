using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace MoEToiro
{
	public partial class frmABCDebugger : Form
	{

		#region インナークラス

		public class Instrument
		{
			public int No { set; get; }
			public string Name
			{
				get
				{
					switch ( this.No )
					{
						case 200:
							return "ドラムセット";
						case 201:
							return "パーカッションセット";
						default:
							return Midi.MidiSequence.INSTRUMENTS [ this.No ];
					}
				}
			}
			public Instrument ( int no ) { this.No = no; }
		}
		public static List<Instrument> GetInstrumentAtDataSource ()
		{
			List<Instrument> lst = new List<Instrument> ();

			for ( int i = 0; i < Midi.MidiSequence.INSTRUMENTS.Length; i++ )
			{
				lst.Add ( new Instrument ( i ) );
			}
			lst.Add ( new Instrument ( 200 ) );	// ドラムセット
			lst.Add ( new Instrument ( 201 ) ); // パーカッションセット
			return lst;
		}


		/// <summary>
		/// ♪
		/// </summary>
		public class PlayNote
		{
			/// <summary>
			/// 音程
			/// </summary>
			public int Scale { set; get; }
			/// <summary>
			/// 分子
			/// </summary>
			public int LengthNumerator { set; get; }
			/// <summary>
			/// 分母
			/// </summary>
			public int LengthDenominator { set; get; }
			/// <summary>
			/// 音が鳴っている時間
			/// </summary>
			public decimal OnTime { set; get; }
			/// <summary>
			/// 音を消すまでの時間
			/// </summary>
//			public decimal OffTime { set; get; }
			/// <summary>
			/// 曲開始からの経過時間
			/// </summary>
			public decimal ElapsedTime { set; get; }
			/// <summary>
			/// リスト中この音程が既に鳴らされた後か否か
			/// </summary>
			public bool IsElapsed { set; get; }
			/// <summary>
			/// リスト中この音程が既に止められたか否か
			/// </summary>
			public bool IsNoteOff { set; get; }

            /// <summary>
            /// ベロシティ
            /// </summary>
            public int Velocity { set; get; }

            /// <summary>
            /// タイで次の符に続くか
            /// </summary>
            public bool Tie { set; get; }

            /// <summary>
			/// コンストラクタ
			/// </summary>
			public PlayNote ()
			{
			}

			/// <summary>
			/// コンストラクタ
			/// </summary>
			public PlayNote ( int scale, int lengthNumerator, int lengthDenominator, decimal onTime, decimal offTime, decimal elapsedTime, int vel, bool tie)
			{
				this.Scale = scale;
				this.LengthNumerator = lengthDenominator;
				this.LengthDenominator = lengthDenominator;
				this.OnTime = onTime;
//				this.OffTime = offTime;

				this.ElapsedTime = elapsedTime;
                this.Velocity = vel;
                this.Tie = tie;
			}

			/// <summary>
			/// コンストラクタ
			/// </summary>
			public PlayNote ( PlayNote note )
			{
				this.Scale = note.Scale;
				this.LengthNumerator = note.LengthNumerator;
				this.LengthDenominator = note.LengthDenominator;
				this.OnTime = note.OnTime;
//				this.OffTime = note.OffTime;
				this.ElapsedTime = note.ElapsedTime;
				this.IsElapsed = note.IsElapsed;
				this.IsNoteOff = note.IsNoteOff;
                this.Velocity = note.Velocity;
                this.Tie = note.Tie;
            }
		}

		/// <summary>
		/// ♪コレクション
		/// </summary>
		public class PlayNoteCollection : List<PlayNote>
		{
			/// <summary>
			/// 演奏するチャンネル番号
			/// </summary>
			[System.ComponentModel.DisplayName ( "Ch" )]
			public int Channel { set; get; }

			/// <summary>
			/// 演奏する楽器の番号
			/// （プログラム番号）
			/// </summary>
			[System.ComponentModel.DisplayName ( "楽器" )]
			public int Instrument { set; get; }

			/// <summary>
			/// 演奏する楽器名
			/// </summary>
			[System.ComponentModel.DisplayName ( "楽器名" )]
			public string InstrumentName
			{
				get
				{
					return
						Midi.MidiSequence.INSTRUMENTS [ this.Instrument ];
				}
			}

			/// <summary>
			/// この楽譜を演奏するか否か
			/// </summary>
			[System.ComponentModel.DisplayName ( "演奏する" )]
			public bool DoPlay { set; get; }

			/// <summary>
			/// この楽譜を表示するか否か
			/// </summary>
			[System.ComponentModel.DisplayName ( "表示する" )]
			public bool IsVisible { set; get; }

			/// <summary>
			/// この譜面の演奏時間
			/// </summary>
			public decimal PlayingTime { set; get; }

			/// <summary>
			/// この譜面が演奏し終わっているかを返す
			/// </summary>
			public bool IsEnd
			{
				get
				{
					var check =
						from r in this
						where !( r.IsElapsed && r.IsNoteOff )
						select r;
					foreach ( var c in check )
					{
						return false;
					}
					return true;
				}
			}
		}

        /// <summary>
        /// ♪コレクションの束
        /// 一つのパート対して演奏有効・可視・終了判定を持つ
        /// </summary>
        public class PlayTrackCollection : List<PlayNoteCollection>
        {
            /// <summary>
            /// 演奏するチャンネル番号
            /// </summary>
            [System.ComponentModel.DisplayName("Ch")]
            public int Channel {
                get
                {
                    if (this.Count <= 0)
                        return 0;
                    return this[0].Channel;
                }
                set
                {
                    foreach (var t in this)
                    {
                        t.Channel = value;
                    }
                }
            }

            /// <summary>
            /// 演奏する楽器の番号
            /// （プログラム番号）
            /// </summary>
            [System.ComponentModel.DisplayName("楽器")]
            public int Instrument
            {
                get
                {
                    if (this.Count <= 0)
                        return 0;
                    return this[0].Instrument;
                }
                set
                {
                    foreach (var t in this)
                    {
                        t.Instrument = value;
                    }
                }
            }

            /// <summary>
            /// 演奏する楽器名
            /// </summary>
            public string InstrumentName
            {
                get 
                {
                    if (this.Count <= 0)
                        return "";
                    return this[0].InstrumentName;
                }
            }
            public bool DoPlay
            {
                get
                {
                    var check = from r in this
                                where r.DoPlay
                                select r;
                    foreach (var c in check)
                    {
                        return true;
                    }
                    return false;
                }
                set
                {
                    foreach (var r in this)
                    {
                        r.DoPlay = value;
                    }
                }
            }
            public bool IsVisible
            {
                get
                {
                    var check = from r in this
                                where r.IsVisible
                                select r;
                    foreach (var c in check)
                    {
                        return true;
                    }
                    return false;
                }
                set
                {
                    foreach (var r in this)
                    {
                        r.IsVisible = value;
                    }
                }
            }
            public bool IsEnd
            {
                get
                {
                    var check =
                        from r in this
                        where !r.IsEnd && r.DoPlay
                        select r;
                    foreach (var c in check)
                    {
                        return false;
                    }
                    return true;
                }
            }
        }

		/// <summary>
		/// トラックの束
		/// 全譜面に対して終了判定を持つ
		/// </summary>
		public class PlayPartCollection : List<PlayTrackCollection>
		{
			/// <summary>
			/// 全トラックが終了しているかを返す
			/// </summary>
			public bool IsEnd
			{
				get
				{
					var check =
						from r in this
						where !r.IsEnd && r.DoPlay
						select r;
					foreach ( var c in check )
					{
						return false;
					}
					return true;
				}
			}
		}


		/// <summary>
		/// 譜面描画用のブラシとペンのコレクション
		/// ｃｈ数分作って保持しておく
		/// </summary>
		private class ScaleBrushPenCollection : List<ScaleBrushPen> { };
		private class ScaleBrushPen
		{
			public Brush Base { set; get; }
			public Pen Line { set; get; }
			public ScaleBrushPen ( Brush b, Pen p )
			{
				this.Base = b;
				this.Line = p;
			}
		}

		#endregion インナークラス

		#region クラス内変数

		const int secondLength = 96;

		/// <summary>
		/// もらったデータ
		/// </summary>
		private MoEABCScore.MoEABCScore _score;
		/// <summary>
		/// メニュー用あまり意味は無い
		/// </summary>
		private Dictionary<int, string> ports = new Dictionary<int, string> ();
		/// <summary>
		/// 演奏中判定フラグ
		/// </summary>
		private bool isPlaying = false;
		/// <summary>
		/// ポーズ判定フラグ
		/// </summary>
		private bool isPause = false;
		/// <summary>
		/// 演奏開始時間（system）
		/// </summary>
		private long startTime = 0;
		/// <summary>
		/// 演奏経過時間（system）
		/// </summary>
		private long elapsedMilliseconds = 0;
		/// <summary>
		/// ポーズ用のテンポラリタイム
		/// </summary>
		private long tmpTime = 0;

		/// <summary>
		/// midiOut関連操作クラス
		/// </summary>
		MoEABCPlay.MoEABCPlay play = new MoEABCPlay.MoEABCPlay ();

        bool[,] tieFlags = new bool[16, 128];
		/// <summary>
		/// noteOn 管理用タイマー
		/// </summary>
		List<Timer> noteOnTimers = new List<Timer> ();
		/// <summary>
		/// noteOff 管理用タイマー
		/// </summary>
		List<Timer> noteOffTimers = new List<Timer> ();
		/// <summary>
		/// noteOnタイマーとnoteOffタイマーの紐付け用辞書
		/// </summary>
		Dictionary<Timer, Timer> noteTimers = new Dictionary<Timer, Timer> ();

		/// <summary>
		/// トラック管理
		/// </summary>
		PlayPartCollection playParts = new PlayPartCollection ();
		/// <summary>
		/// 譜面管理
		/// </summary>
		PlayNoteCollection playNote = new PlayNoteCollection ();
		/// <summary>
		/// noteOnタイマーと譜面の紐付け用辞書
		/// </summary>
		Dictionary<Timer, PlayNoteCollection> NoteKeyTimers = new Dictionary<Timer, PlayNoteCollection> ();
		/// <summary>
		/// noteOffタイマーと譜面の紐付け用辞書
		/// </summary>
		Dictionary<Timer, PlayNoteCollection> NoteOffKeyTimers = new Dictionary<Timer, PlayNoteCollection> ();


		/// <summary>
		/// 演奏しない譜面描画用のペン＆ブラシ
		/// </summary>
		ScaleBrushPen grayBrushPen = new ScaleBrushPen ( new SolidBrush ( Color.LightGray ), new Pen ( Color.DimGray ) );
		/// <summary>
		/// 演奏しない譜面が鳴ってる時描画用のブラシ＆ブラシ
		/// </summary>
		ScaleBrushPen noteOnGrayBrushPen = new ScaleBrushPen ( new SolidBrush ( Color.DimGray ), new Pen ( Color.LightGray ) );
		/// <summary>
		/// 譜面描画用のペン＆ブラシ
		/// </summary>
		ScaleBrushPenCollection scaleBrushPens = new ScaleBrushPenCollection ();
		/// <summary>
		/// 譜面が鳴ってる時描画用のペン＆ブラシ
		/// </summary>
		ScaleBrushPenCollection noteOnScaleBrushPens = new ScaleBrushPenCollection ();



		#endregion クラス内変数

        private static frmABCDebugger theInstance = null;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public frmABCDebugger ( MoEABCScore.MoEABCScore score )
		{
            if (theInstance != null && !theInstance.IsDisposed)
            {
                theInstance.Activate();
                this.Close();
                return;
            } 
            theInstance = this;
			_score = score;
			play = new MoEABCPlay.MoEABCPlay ();
			play.InitMidiOut ( 0 );

            try
            {
                InitializeComponent();
            }
            catch (Exception)
            {
                this.Close();
                if (theInstance != null && !theInstance.IsDisposed)
                {
                    theInstance.Activate();
                }
                return;
            }
			this.MouseEnter += ( sender, e ) =>
			{
				this.Focus ();
			};
			this.MouseWheel += ( sender, e ) =>
			{
				if ( Math.Abs ( e.Delta ) < 120 ) return;

				ScrollableControl control = (ScrollableControl)this.pnlScore; // スクロールさせたい画面を指定
				var scroll = control.VerticalScroll;

				var maximum = 1 + scroll.Maximum - scroll.LargeChange; // ユーザが取り得る最大値
				var delta = -( e.Delta / 120 ) * scroll.SmallChange;
				var offset = Math.Min ( Math.Max ( scroll.Value + delta, scroll.Minimum ), maximum );

				scroll.Value = offset;
				scroll.Value = offset;
			};
			this.MaximumSizeChanged += ( sender, e ) =>
			{
				picKeyBord.Refresh ();
			};

			// もらったスコアを編集
			ModifyScore ();

			// 描画系
			InitRender ();
		}

		/// <summary>
		/// 演奏用タイマー
		/// 念のためｃｈ別に分けてあるが動作は同じ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timNoteOn_Tick ( object sender, EventArgs e )
		{
			Timer ontim = (Timer)sender;
			var note = NoteKeyTimers [ ontim ];

			elapsedMilliseconds = DateTime.Now.Ticks / 10000 - startTime + tmpTime;

			var notes =
				from n in note
				where !n.IsElapsed
				&& n.ElapsedTime * 1000 < elapsedMilliseconds
				select n;
			foreach ( var n in notes )
			{
				if ( note.DoPlay && n.Scale >= 0 )
				{
                    if (tieFlags[note.Channel, n.Scale] == false)
                    {
                        play.NoteOn(n.Scale, (byte)note.Channel, (byte)n.Velocity);
                    }
                    tieFlags[note.Channel, n.Scale] = n.Tie;
				}
                else if (n.Scale == -2)
                {
                    // Scale == -2 をボリューム調整ノートとみなす。ad hoc
                    play.Volume(n.Velocity, (byte)note.Channel);
                }
				n.IsElapsed = true;
			}
			txtDebugText.Text = elapsedMilliseconds.ToString();
			tslblTime.Text = string.Format ( "{0:d2}'{1:d2}.{2:d3}", elapsedMilliseconds / 1000 / 60, elapsedMilliseconds / 1000 % 60, elapsedMilliseconds % 1000 );
		}

		/// <summary>
		/// 演奏用タイマー（NoteOff用）
		/// 念のためｃｈ別に管理しているが動作は同じ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timNoteOff_Tick ( object sender, EventArgs e )
		{
			Timer tim = (Timer)sender;

			var note = NoteOffKeyTimers [ tim ];
			elapsedMilliseconds = DateTime.Now.Ticks / 10000 - startTime + tmpTime;
			var notes =
				from n in note
				where !n.IsNoteOff
				&& n.IsElapsed
				&& ( n.ElapsedTime + n.OnTime ) * 1000 < elapsedMilliseconds
				select n;

			foreach ( var n in notes )
			{
				if ( n.Scale >= 0)
				{
                    if (tieFlags[note.Channel, n.Scale] == false)
                    {
                        play.NoteOff(n.Scale, (byte)note.Channel);
                    }
				}
				n.IsNoteOff = true;
			}

			// 曲の終了判定 IsElapsed ∧ IsNoteOff 以外の値が存在しなければ全音完走
			if ( playParts.IsEnd )
			{
				// NoteOn、NoteOff すべてのタイマーを切る
				foreach ( var t in noteOnTimers )
				{
					t.Enabled = false;
					noteTimers [ t ].Enabled = false;
				}
				// 演奏中フラグを下ろす
				isPlaying = false;
				isPause = false;
				// 経過時間、開始時間をクリア
				startTime = 0;
				elapsedMilliseconds = 0;
				tmpTime = 0;
				dgvPartData.Enabled = true;
			}
		}



		/// <summary>
		/// フォームを閉じる際にちゃんと後始末しておく
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmABCDebugger_FormClosing ( object sender, FormClosingEventArgs e )
		{
			foreach ( var t in noteOnTimers )
			{
				t.Enabled = false;
				noteTimers [ t ].Enabled = false;
			}
			play.MidiReset ();
			play.Close ();
		}

		/// <summary>
		/// ピクチャーボックスの描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void picMain_Paint ( object sender, PaintEventArgs e )
		{
			DrawScore ( e.Graphics );
		}

		/// <summary>
		/// 演奏中の自動スクロール制御
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timAutoScroll_Tick ( object sender, EventArgs e )
		{
			if ( !isPlaying ) return;
			picMain.Refresh ();
		}

		/// <summary>
		/// キーボード部分を描画
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void picKeyBord_Paint ( object sender, PaintEventArgs e )
		{
			DrawKeyBord ( e.Graphics );
		}

		/// <summary>
		/// 縦スクロールで鍵盤と譜面の位置をあわせる
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pnlScore_Scroll ( object sender, ScrollEventArgs e )
		{
			picKeyBord.Location = new Point ( 0, picMain.Bounds.Y );
		}

		/// <summary>
		/// スペース押下でプレイ／停止の切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmABCDebugger_KeyDown ( object sender, KeyEventArgs e )
		{
			if ( e.KeyCode == Keys.Space )
			{
				// 演奏中なら一時停止する
				if ( isPlaying )
				{
					stop ();
				}
				else
				{
					start ();
				}
			}
		}

		/// <summary>
		/// 譜面クリックで演奏再開場所を設定する
		/// 演奏中の場合はなにもしない
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void picMain_MouseClick ( object sender, MouseEventArgs e )
		{
			// 演奏中なら無視する
			if ( isPlaying ) return;

			// 左クリックで演奏再開場所を設定する
			if ( e.Button == System.Windows.Forms.MouseButtons.Left )
			{
				elapsedMilliseconds = decimal.ToInt64 ( (decimal)e.X / secondLength * 1000 );
				tmpTime = elapsedMilliseconds;

				// 途中再生の準備
				PlayTracksReset ();

				isPause = true;

				// 現在位置含め再描画
				picMain.Refresh ();

				tslblTime.Text = string.Format ( "{0:d2}'{1:d2}.{2:d3}", elapsedMilliseconds / 1000 / 60, elapsedMilliseconds / 1000 % 60, elapsedMilliseconds % 1000 );
			}
		}

		/// <summary>
		/// 再描画用
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmABCDebugger_ResizeEnd ( object sender, EventArgs e )
		{
			picMain.Refresh ();
			picKeyBord.Refresh ();
		}

		/// <summary>
		/// 停止
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void txbtnStop_Click ( object sender, EventArgs e )
		{
			stop ();
			startTime = 0;
			elapsedMilliseconds = 0;
			tmpTime = 0;
			PlayTracksReset ();

			picMain.Refresh ();
		}

		/// <summary>
		/// 一時停止
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tsbtnPause_Click ( object sender, EventArgs e )
		{
			stop ();
			picMain.Refresh ();
		}

		/// <summary>
		/// 開始
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tsbtnStart_Click ( object sender, EventArgs e )
		{
			start ();
		}

		#region プライベートメソッド


		/// <summary>
		/// 初期描画 
		/// </summary>
		private void InitRender ()
		{
			// 音源を調べてメニューを作る
			int count = play.PortCount;
			ports = play.Port;
			ToolStripMenuItem mi;
			foreach ( var v in play.Port )
			{
				mi = new ToolStripMenuItem ( v.Value );
				mi.Click += delegate
				{
					if ( mi.Checked ) return;

					foreach ( ToolStripMenuItem m in tsmiSoundScore.DropDownItems )
					{
						m.Checked = ( m == mi );
					}
				};
				tsmiSoundScore.DropDownItems.Add ( mi );
			}
			// １件目をチェックしておく
			if ( tsmiSoundScore.DropDownItems.Count > 0 )
				( (ToolStripMenuItem)tsmiSoundScore.DropDownItems [ 0 ] ).Checked = true;

			// ダブルバッファリング有効？なんだか効いていない風
			this.DoubleBuffered = true;

			// ブラシを作っておきましょう
			InitScaleBrush ();

			// トラックの関連データをGridVeiwに表示設定
			InitDataGrid ();
		}

		/// <summary>
		/// トラックの関連データをGridVeiwに表示設定
		/// </summary>
		private void InitDataGrid ()
		{
			dgvPartData.AllowUserToAddRows = false;
			dgvPartData.AllowUserToDeleteRows = false;
			dgvPartData.AllowUserToResizeRows = false;
			dgvPartData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgvPartData.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
			dgvPartData.RowHeadersVisible = false;

			dgvPartData.AutoGenerateColumns = false;
			dgvPartData.Columns.Clear ();

			DataGridViewColumn col = new DataGridViewTextBoxColumn ();
			col.HeaderText = "Ch";
			col.DataPropertyName = "Channel";
			col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			col.ReadOnly = true;
			dgvPartData.Columns.Add ( col );

			col = new DataGridViewComboBoxColumn ();
			col.Name = "combo";
			col.HeaderText = "楽器";
			col.DataPropertyName = "Instrument";
			col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			( (DataGridViewComboBoxColumn)col ).DataSource = GetInstrumentAtDataSource ();
			( (DataGridViewComboBoxColumn)col ).DisplayMember = "Name";
			( (DataGridViewComboBoxColumn)col ).ValueMember = "No";
			( (DataGridViewComboBoxColumn)col ).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			dgvPartData.Columns.Add ( col );

			col = new DataGridViewCheckBoxColumn ();
			col.HeaderText = "演奏する";
			col.DataPropertyName = "DoPlay";
			col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			dgvPartData.Columns.Add ( col );

			col = new DataGridViewCheckBoxColumn ();
			col.HeaderText = "表示する";
			col.DataPropertyName = "IsVisible";
			col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			dgvPartData.Columns.Add ( col );

			// トラックの関連データをGridVeiwに表示設定
			dgvPartData.DataSource = playParts;

			// １クリックでDDLがｵｰﾌﾟﾝﾇするように小細工
			dgvPartData.CellEnter += ( sender, e ) =>
			{
				DataGridView dgv = (DataGridView)sender;

				if ( dgv.Columns [ e.ColumnIndex ].Name == "combo" &&
				   dgv.Columns [ e.ColumnIndex ] is DataGridViewComboBoxColumn )
				{
					SendKeys.Send ( "{F4}" );
				}
			};

			// ドラムパートの場合に楽器変更を許可しない
			dgvPartData.DataBindingComplete += ( sender, e ) =>
			{
				DataGridView dgv = (DataGridView)sender;
				foreach ( DataGridViewRow row in dgv.Rows )
				{
					PlayTrackCollection d = (PlayTrackCollection)row.DataBoundItem;
					if ( d.Channel == 9 )
					{
						row.Cells [ "combo" ].ReadOnly = true;
					}
				}
			};
		}

		/// <summary>
		/// もらった譜面を元に使いやすいように加工する
		/// 
		/// </summary>
		private void ModifyScore ()
		{
			PlayNote baseNote = new PlayNote ();
//			PlayNote baseTempo = new PlayNote ();
			decimal elapsedTime = new decimal ();
			int ch = -1; // チャンネル番号
			/*
			 * tempo / 60 : 一秒間に叩く基準音符値の数
			 * つまり。tempo：120の場合 120 / 60 = 2 となり、１秒間に叩く基準音符値は２つとなる
			 * 基準音符値が 1/4 の場合、四分音符２つで１秒、つまり、基準音符１つ分の長さは0.5秒となる（ 1秒 / 2つ = 1つ0.5秒）
			 * こいつに対する相対的な音符の長さが
			 * LengthNumerator / LengthDenominator になる
			 * 例えば。tempo120 で、最初の音が1/4なら
			 * 基準音符の1/4であるから、1/4 * 1/4 で 1/16 となり、１６分音符の長さは 0.5秒 * 1 / 4 で 0.125 秒となる
			 * 全音符等の場合は LengthNumerator / LengthDenominator の分子 > 分母 となるだけで、計算式は変わらない
			 * 全音符：0.5 * 16 / 4 = 2秒
			*/

			foreach ( var part in _score.Parts )
			{
				// ドラム考慮ｃｈ設定もう少しなんとかしたい
                //if ( part.Instrument == 200 || part.Instrument == 201 )
                //{
                //    ch = 9;
                //}
                //else
                //{
                //    ch++;
                //    if ( ch == 9 ) ch++;
                //}
                ch = part.Channel;
                var playTracks = new PlayTrackCollection();
                foreach (var track in part.Tracks)
				{
					playNote = new PlayNoteCollection ();
					decimal baseScaleTime = new decimal ();
					elapsedTime = new decimal ();

                    int velocity = 100;
					foreach ( var note in track.Notes )
					{
						if ( baseScaleTime == 0 )
						{
							baseScaleTime = 1 / ( (decimal)( 120 / 60 ) );
						}
						// 音符
						if ( note is MoEABCScore.MoEABCScaleNote )
						{
							playNote.Add (
								new PlayNote (
									( (MoEABCScore.MoEABCScaleNote)note ).Scale,
									note.LengthNumerator,
									note.LengthDenominator,
									baseScaleTime * note.LengthNumerator / note.LengthDenominator,
									baseScaleTime * note.LengthNumerator / note.LengthDenominator,
									elapsedTime, velocity,((MoEABCScore.MoEABCScaleNote)note).Tie ));
							elapsedTime += baseScaleTime * note.LengthNumerator / note.LengthDenominator;
						}
						// 休符
						else if ( note is MoEABCScore.MoEABCRestNote )
						{
							playNote.Add (
								new PlayNote (
									-1, note.LengthNumerator,
									note.LengthDenominator,
									baseScaleTime * note.LengthNumerator / note.LengthDenominator,
									baseScaleTime * note.LengthNumerator / note.LengthDenominator,
									elapsedTime, velocity, false));
							elapsedTime += baseScaleTime * note.LengthNumerator / note.LengthDenominator;
						}
						// 和音
						else if ( note is MoEABCScore.MoEABCChordNote )
						{
							foreach ( var n in ( (MoEABCScore.MoEABCChordNote)note ).Notes )
							{
								playNote.Add (
									new PlayNote (
										n.Scale,
										n.LengthNumerator,
										n.LengthDenominator,
										baseScaleTime * n.LengthNumerator / n.LengthDenominator,
										baseScaleTime * n.LengthNumerator / n.LengthDenominator,
										elapsedTime, velocity, n.Tie) );
							}
							elapsedTime += baseScaleTime * note.LengthNumerator / note.LengthDenominator;
						}
						// 「基準音符値設定」に対応するクラス。"<L:n/m>"
						// 現状では、各トラックの最初に一度 "<L:1/4>" が来るだけのはず
						else if ( note is MoEABCScore.MoEABCBaseSpecifierNote )
						{
							baseNote.LengthNumerator = note.LengthNumerator;
							baseNote.LengthDenominator = note.LengthDenominator;
						}
						// テンポ
						else if ( note is MoEABCScore.MoEABCTempoNote )
						{
                            //baseTempo.LengthNumerator = note.LengthNumerator;
                            //baseTempo.LengthDenominator = note.LengthDenominator;
							baseScaleTime = 1 / ( (decimal)( (MoEABCScore.MoEABCTempoNote)note ).Value / 60 );
						}
						else if ( note is MoEABCScore.MoEABCVelocityNote )
						{
                            velocity = ((MoEABCScore.MoEABCVelocityNote)note).Value;
						}
						else if ( note is MoEABCScore.MoEABCVolumeNote )
						{
                            playNote.Add(
                                new PlayNote(
                                    -2, note.LengthNumerator,
                                    note.LengthDenominator,
                                    baseScaleTime * note.LengthNumerator / note.LengthDenominator,
                                    baseScaleTime * note.LengthNumerator / note.LengthDenominator,
                                    elapsedTime, ((MoEABCScore.MoEABCVolumeNote)note).Value, false));
                        }
						else if ( note is MoEABCScore.MoEABCKeyNote )
						{
						}
					}
					// 一つも鳴らす音がなければ無視する
					if ( playNote.Count == 0 ) continue;

					// トラックに追加
					playNote.PlayingTime = elapsedTime;
					playNote.Instrument = part.Instrument;
					playNote.Channel = ch;
					playNote.DoPlay = true;
					playNote.IsVisible = true;

					playTracks.Add ( playNote );
					// タイマーを作成＆ヒモ付
					Timer onTim = new Timer ();
					onTim.Tick += new EventHandler ( timNoteOn_Tick );
					onTim.Interval = 10;
					noteOnTimers.Add ( onTim );

					// onタイマーと譜面のヒモ付
					NoteKeyTimers.Add ( onTim, playNote );

					Timer offTim = new Timer ();
					offTim.Tick += new EventHandler ( timNoteOff_Tick );
					offTim.Interval = 10;
					noteOffTimers.Add ( offTim );

					// on/off のヒモ付
					noteTimers.Add ( onTim, offTim );

					NoteOffKeyTimers.Add ( offTim, playNote );
				}
                playParts.Add(playTracks);
			}

			// 全部出来たのでとりあえず全トラックの時間を出力してみる
			StringBuilder sb = new StringBuilder ();

			int i = 1;
			foreach ( var v in playParts )
			{
                foreach (var t in v)
                {
                    sb.AppendFormat("{0}:", i++);
                    sb.AppendLine(t.PlayingTime.ToString());
                }
			}
			txtDebugText.Text = sb.ToString ();
		}


		/// <summary>
		/// 鍵盤を描画する
		/// </summary>
		/// <param name="g"></param>
		private void DrawKeyBord ( Graphics g )
		{
			int height = 13;
			int noteHeight = 8;

			Brush bbrush = new SolidBrush ( Color.Black );
			Brush wbrush = new SolidBrush ( Color.White );
			Font f = new Font ( "Times New Roman", 9 );

			Rectangle rect;

			int adjustment = 0;
			int adjustTotal = 0;

			picKeyBord.Height = ( height + 1 ) * 75 - 20;

			// 白鍵描画
			for ( int i = 0; i < 75; i++ )
			{
				if ( i == 0 )
				{
					adjustment = 2;
				}
				else if ( i % 7 == 2 ) adjustment = 1;
				else if ( i % 7 == 3 ) adjustment = -2;
				else if ( i % 7 == 6 ) adjustment = -2;
				else if ( i % 7 == 0 ) adjustment = -2;
				else adjustment = 0;

				rect = new Rectangle ( 0, 0 + height * i - adjustTotal, 64, height - adjustment );
				g.FillRectangle ( wbrush, rect );
				g.DrawRectangle ( new Pen ( Color.Black ), rect );
				if ( i % 7 == 4 ) g.DrawString ( String.Format ( "C{0}", 9 - (int)( i / 7 ) ), f, bbrush, 40, 0 + height * i - adjustTotal );

				if ( adjustment != 0 ) adjustTotal += adjustment;
			}

			// 黒鍵描画
			for ( int i = 0; i < 128; i++ )
			{
				switch ( i % 12 )
				{
					case 1:		// 黒鍵
					case 4:
					case 6:
					case 9:
					case 11:
						rect = new Rectangle ( 0, noteHeight * i, 32, noteHeight );
						g.FillRectangle ( bbrush, rect );
						g.DrawRectangle ( new Pen ( Color.Black ), rect );
						break;
					default:	// 白鍵
						break;
				}
			}
		}

		/// <summary>
		/// スコアを描画する
		/// </summary>
		/// <param name="g"></param>
		private void DrawScore ( Graphics g )
		{
			const int height = 13;
			const int noteHeight = 8;

			Brush bbrush = new SolidBrush ( Color.Black );
			Brush wbrush = new SolidBrush ( Color.White );
			Brush gbrush = new SolidBrush ( Color.LightGray );
			Brush pbrush = new SolidBrush ( Color.LightPink );
			Font f = new Font ( "Times New Roman", 9 );

			Rectangle rect;

			// 演奏時間を元に picMain の大きさを決める
            var playTime = playParts.SelectMany((ts) => ts).Select((t) => t.PlayingTime).Max();
                //( from s in playParts
                //  select s.PlayingTime ).Max ();
			picMain.Width = (int)( playTime * secondLength ) + 24;
			picMain.Height = ( height + 1 ) * 75 - 20;

			// 音色欄の線を描画
			for ( int i = 0; i < 128; i++ )
			{
				rect = new Rectangle ( 0, 0 + noteHeight * i, picMain.Width, noteHeight );

				switch ( i % 12 )
				{
					case 1:		// 黒鍵
					case 4:
					case 6:
					case 9:
					case 11:
						g.FillRectangle ( gbrush, rect );
						g.DrawRectangle ( new Pen ( Color.Gray ), rect );
						break;
					default:	// 白鍵
						g.FillRectangle ( wbrush, rect );
						g.DrawRectangle ( new Pen ( Color.Gray ), rect );
						break;
				}
			}

			int scaleX, scaleY;
			// 実際の譜面をトラック別に描画する
			foreach ( var tracks in playParts )
			{
                foreach (var track in tracks)
                {
                    if (!track.IsVisible) continue;
                    foreach (var note in track)
                    {
                        if (note.Scale == -1) continue;
                        scaleX = 0 + (int)(secondLength * note.ElapsedTime);
                        scaleY = 1016 - (int)(note.Scale % 12 * noteHeight + (note.Scale / 12) * noteHeight * 12);
                        rect = new Rectangle(scaleX, scaleY, (int)(note.OnTime * secondLength), 7);

                        Brush b; Pen p;
                        if (note.IsElapsed && !note.IsNoteOff)
                        {
                            b = track.DoPlay ? noteOnScaleBrushPens[track.Channel % noteOnScaleBrushPens.Count].Base : noteOnGrayBrushPen.Base;
                            p = track.DoPlay ? noteOnScaleBrushPens[track.Channel % noteOnScaleBrushPens.Count].Line : noteOnGrayBrushPen.Line;
                        }
                        else
                        {
                            b = track.DoPlay ? scaleBrushPens[track.Channel % scaleBrushPens.Count].Base : grayBrushPen.Base;
                            p = track.DoPlay ? scaleBrushPens[track.Channel % scaleBrushPens.Count].Line : grayBrushPen.Line;
                        }

                        g.FillRectangle(b, rect);
                        g.DrawRectangle(p, rect);
                    }
                }
			}

			// タイムライン 1秒間に secondLength 移動するとして、現在の経過時間をミリセックで掛けてあげるとX位置
			int timeLine = (int)( secondLength * elapsedMilliseconds / 1000 );
			rect = new Rectangle ( timeLine, (int)g.ClipBounds.Y, 1, (int)g.ClipBounds.Height );
			g.DrawLine ( new Pen ( Color.Black ), rect.X, 0, rect.X, picMain.Height );

			// 自動スクロール
			if ( timeLine > g.ClipBounds.X + g.ClipBounds.Width )
			{
				ScrollableControl control = (ScrollableControl)this.pnlScore; // スクロールさせたい画面を指定
				var scroll = control.HorizontalScroll;
				var maximum = 1 + scroll.Maximum - scroll.LargeChange; // ユーザが取り得る最大値
				int val = (int)g.ClipBounds.Width;
				var offset = Math.Min ( Math.Max ( scroll.Value + val, scroll.Minimum ), maximum );

				scroll.Value = offset;
			}
		}


		/// <summary>
		/// ブラシとペンのセットを初期設定しておく
		/// やっつけ
		/// </summary>
		private void InitScaleBrush ()
		{
			scaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.LightPink ), new Pen ( Color.Red ) ) );
			scaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.LightCyan ), new Pen ( Color.Blue ) ) );
			scaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.LightGreen ), new Pen ( Color.Green ) ) );
			scaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.LightPink ), new Pen ( Color.Crimson ) ) );
			scaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.Turquoise ), new Pen ( Color.Blue ) ) );
			scaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.LemonChiffon ), new Pen ( Color.DarkGoldenrod ) ) );
			scaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.SlateBlue ), new Pen ( Color.Indigo ) ) );
			scaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.SeaShell ), new Pen ( Color.Orange ) ) );
			scaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.MediumOrchid ), new Pen ( Color.MediumVioletRed ) ) );
			scaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.Coral ), new Pen ( Color.Chocolate ) ) );
			scaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.AliceBlue ), new Pen ( Color.Aqua ) ) );
			scaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.PaleGreen ), new Pen ( Color.YellowGreen ) ) );
			scaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.Khaki ), new Pen ( Color.Goldenrod ) ) );
			scaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.OrangeRed ), new Pen ( Color.IndianRed ) ) );
			scaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.Aquamarine ), new Pen ( Color.DodgerBlue ) ) );
			scaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.LightGoldenrodYellow ), new Pen ( Color.Olive ) ) );
			scaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.Salmon ), new Pen ( Color.Tomato ) ) );

			noteOnScaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.Red ), new Pen ( Color.LightPink ) ) );
			noteOnScaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.Blue ), new Pen ( Color.LightCyan ) ) );
			noteOnScaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.Green ), new Pen ( Color.LightGreen ) ) );
			noteOnScaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.Crimson ), new Pen ( Color.LightPink ) ) );
			noteOnScaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.Blue ), new Pen ( Color.Turquoise ) ) );
			noteOnScaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.DarkGoldenrod ), new Pen ( Color.LemonChiffon ) ) );
			noteOnScaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.Indigo ), new Pen ( Color.SlateBlue ) ) );
			noteOnScaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.Orange ), new Pen ( Color.SeaShell ) ) );
			noteOnScaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.MediumVioletRed ), new Pen ( Color.MediumOrchid ) ) );
			noteOnScaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.Chocolate ), new Pen ( Color.Coral ) ) );
			noteOnScaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.Aqua ), new Pen ( Color.AliceBlue ) ) );
			noteOnScaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.YellowGreen ), new Pen ( Color.PaleGreen ) ) );
			noteOnScaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.Goldenrod ), new Pen ( Color.Khaki ) ) );
			noteOnScaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.IndianRed ), new Pen ( Color.OrangeRed ) ) );
			noteOnScaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.DodgerBlue ), new Pen ( Color.Aquamarine ) ) );
			noteOnScaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.Olive ), new Pen ( Color.LightGoldenrodYellow ) ) );
			noteOnScaleBrushPens.Add ( new ScaleBrushPen ( new SolidBrush ( Color.Tomato ), new Pen ( Color.Salmon ) ) );
		}


		/// <summary>
		/// 最初にプログラムチェンジをして
		/// その後NoteOnタイマーを起動
		/// その他フラグとか設定
		/// </summary>
		private void start ()
		{
			// プログラムチェンジ
			foreach ( var ts in playParts )
			{
                foreach (var t in ts)
                {
                    play.ProgramChange(t.Instrument, (byte)t.Channel);
                }
			}

            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 128; j++)
                {
                    tieFlags[i, j] = false;
                }
            }
			// 念のため全部止める
			play.MidiReset ();

			foreach ( var t in noteOnTimers )
			{
				t.Enabled = true;
				noteTimers [ t ].Enabled = true;
			}
			// 開始時間保持
			startTime = DateTime.Now.Ticks / 10000;

			// ド新規
			if ( !isPause )
			{
				// 経過時間クリア
				elapsedMilliseconds = 0;
			}
			// 演奏中
			isPlaying = true;
			isPause = false;

			// スクロール描画用タイマー起動
			timAutoScroll.Enabled = true;

			dgvPartData.Enabled = false;

			// お望みなら再開位置へ画面表示をスクロール
			pnlScore.HorizontalScroll.Value = (int)( elapsedMilliseconds * secondLength / 1000 );

		}

		/// <summary>
		/// noteTimer をとめて演奏をストップ
		/// 途中再生の準備もしておく（PlayTracksReset をコール）
		/// あとフラグとか
		/// </summary>
		private void stop ()
		{
			play.MidiReset ();
			tmpTime += DateTime.Now.Ticks / 10000 - startTime;
			startTime = 0;
			foreach ( var t in noteOnTimers )
			{
				t.Enabled = false;
				noteTimers [ t ].Enabled = false;
			}
			isPlaying = false;
			isPause = true;

			// 途中再生の準備
			PlayTracksReset ();

			// スクロール描画用タイマーストップ
			timAutoScroll.Enabled = false;

			dgvPartData.Enabled = true;
		}

		/// <summary>
		/// 一時停止からの復帰を行うための設定
		/// 巻き戻される可能性もあるので、一旦全ての譜面のフラグを初期化する
		/// 設定されている再開時間以前の♪たちに終了判定をたてておく
		/// </summary>
		private void PlayTracksReset ()
		{
			// すべての楽譜のフラグを初期化する
			foreach ( var ts in playParts )
			{
                foreach (var t in ts)
                {
                    foreach (var n in t)
                    {
                        n.IsElapsed = false;
                        n.IsNoteOff = false;
                    }
                }
			}

			// 設定した経過時間以前のフラグをすべて立てる（終わったことに）
			var w =
				from t in playParts.SelectMany((ts) => ts)
				from n in t
				where ( n.ElapsedTime + n.OnTime ) * 1000 < elapsedMilliseconds
				select n;
			foreach ( var n in w )
			{
				n.IsElapsed = true;
				n.IsNoteOff = true;
			}
		}

		#endregion プライベートメソッド

	}
}
