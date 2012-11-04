using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**
 * MIDIから変換した楽譜をプログラムで視覚化したり演奏する用のデータ構造。
 * いちいちテキストからパーズしてるとやってられないので導入。
 * 
 * 全てのプロパティは読み出し専用で、setter や (外から見える)コンストラクタは存在しない。
 * (編集したいという要望が出るのはわかってるけど、dirtyフラグの管理とか面倒なことになるので)
 * 
 * データ構造はこんな感じ:
 * 
 * // 「譜面」全体に対応するクラス。これが1つのMIDIファイルの変換結果で、階層構造の最上位。
 * MoEABCScore {
 *     List<MoEABCPart> Parts;  // 「譜面」は0個以上の「パート」からなる
 *     string SourceFileName;   //  変換元MIDIのファイル名。フルパスで入ってると思うけど過信してはいけない。
 * }
 * 
 * // 1つの「パート」に対応するクラス。「パート」は1つのタブに表示される単位。
 * MoEABCPart {
 *     // 「パート」は0個以上の「トラック」からなる。
 *     // MoEABCの仕様では1パートに2トラックまでだが、その制限は特に反映しない。
 *     // (数を決め打つのはやめて、ちゃんと foreach (...) で最後まで回しましょうってこと)
 *     List<MoEABCTrack> Tracks;
 *
 *     // このパートの楽器番号
 *     // 値は: 0～127 = General MIDI 仕様と同じ割り当て。 200 = ドラム, 201 = パーカッション
 *     int Instrument; 
 *     // Instrument に対応する楽器の名前
 *     string InstrumentName;
 * }
 * 
 * // 1つの「トラック」に対応するクラス。「トラック」は "{...}" で囲まれる単位。
 * MoEABCTrack {
 *     // 「トラック」は0個以上の「符」からなる
 *     List<MoEABCNote> Notes;
 *     // 【重要】ここから大事なのでよく読みましょう
 *     // 「符」は音符・休符の他、テンポ指定・ベロシティ指定等のコントロール情報も含みます
 * }
 * 
 * // 1つの「符」に対応するクラス。これが階層構造の末端。
 * // これ自体は抽象クラスで、派生で「音符」「和音」「休符」「テンポ指定」等に分かれる
 * abstract MoEABCNote {
 *     int LengthNumerator; // 音符長の分子
 *     int LengthDenominator; // 音符長の分母
 *     // テキスト表現で "ほげn/m" という符の、n, m の値がそのまま入ります
 *     // コントロール情報については、ダミーの値として 分子=0, 分母=1 が入ります
 *     // n/m は「基準音符長」(<L:1/X>で指定されるもの) の相対値であることに注意。
 *     // 現状、このツールで変換した譜面は、<L:1/4> (四分音符基準、例えば "1/2" なら四分音符の 1/2 = 八分音符)
 *     // で固定ですが、それをあてこむのはやめよう。
 *     // あと、13/65 とか、半端な数字になることもあるので、
 *     // switch { case 2: ... case 4: ... } みたいなのもやめよう。
 * }
 * 
 * // 「音符」に対応するクラス。一番基本
 * MoEABCScaleNote : MoEABCNote {
 *     int Scale; 
 *     // 音階。0～127まで。曲調とか一切関係なく絶対音階であることに注意
 *     // 0 が一番低いC, 127 が一番高い G, 60 がいわゆる「ド」のC
 *     
 *     string ScaleName;
 *     // 音階のテキスト表現。"C" とか "^F" とか
 *     // #, ♭は全てハ長調基準でついていることに注意
 *     
 *     bool Tie;
 *     // タイがついている符なら true, そうでなければ false
 *     // 「Tie == true なのに続くべき音符がない」ことはあまりないと思うが、
 *     // もしそういう事態が起こったら、Tie == false と同様に扱う。
 * }
 * 
 * // 「休符」に対応するクラス。
 * MoEABCRestNote : MoEABCNote {
 *     // 長さしか情報がないので追加のプロパティはなし
 * }
 * 
 * // 「和音」に対応するクラス。
 * MoEABCChordNote : MoEABCNote {
 *     List<MoEABCScaleNote> Notes;
 *     // 和音を構成する音階のリスト
 *     // 「要素が一つもない」リストはまずありえないが (あったらバグ)、「一つしかない」ものは普通にあるので注意
 *     // リストの各要素の音階が、それぞれの長さだけ鳴る。次の音を鳴らすタイミングは、この和音自体の分子・分母で決まる。
 *     // 例： [C1/2 E1/2]1/4 D1/2 は、
 *     //      C ----
 *     //      E ----
 *     //      D   ----
 *     // こういうタイミングで重なる
 * }
 * 
 * // 「基準音符値設定」に対応するクラス。"<L:n/m>"
 * // 現状では、各トラックの最初に一度 "<L:1/4>" が来るだけのはず
 * MoEABCBaseSpecifierNote : MoEABCNote {
 *     // 長さしか情報がないので追加のプロパティはなし
 * }
 * 
 * // 「テンポ設定」に対応するクラス。"<Q:n>"
 * // これは現状でも1トラック中で変化しうる
 * MoEABCTempoNote : MoEABCNote {
 *     int Value; 
 *     // 設定値。32～255
 * }
 * 
 * // 以下は、現状の変換では出現しないが、カタチだけ定義
 * 
 * // ベロシティ指定 "<v:n>"
 * MoEABCVelocityNote : MoEABCNote {
 *     int Value; 
 *     // 設定値。1～127
 * }
 *
 * // ボリューム指定 "<V:n>"
 * MoEABCVolumeNote : MoEABCNote {
 *     int Value; 
 *     // 設定値。0～127
 * }
 * 
 * // 曲調指定 "<K:ほげ>"
 * // MIDIが絶対音階なので、曲調を扱うことはまずないと思われる
 * MoEABCKeyNote : MoEABCNote {
 *     string Value;
 *     // 設定値。"C#" とか "A#m" とかテキスト表現のままでいいや...どうせ使わないし
 * }
 * 
 * 
 * 多分こんな感じのコードになるよ
 * 
 * foreach (MoEABCNote note in track.Notes) 
 * {
 *     if (note is MoEABCScaleNote)
 *     {
 *         ....
 *     }
 *     else if (note is MoEABCRestNote)
 *     {
 *         ....
 *     }
 *     ....
 * }
 * 
 * ようするに typecase で回してね。
 * enum とか visitor の方がいいですか？
 * 
 */


namespace MoEABCScore
{
    public class ParseException : Exception
    {
    }
	/// <summary>
	/// 「譜面」全体に対応するクラス。これが1つのMIDIファイルの変換結果で、階層構造の最上位
	/// </summary>
	public class MoEABCScore
	{
		private List<MoEABCPart> parts;  // 「譜面」は0個以上の「パート」からなる
		private string sourceFileName;   //  変換元MIDIのファイル名。フルパスで入ってると思うけど過信してはいけない。
		/// <summary>
		/// 「譜面」は0個以上の「パート」からなる
		/// </summary>
		public List<MoEABCPart> Parts
		{
			get { return new List<MoEABCPart> ( this.parts ); }
		}
		/// <summary>
		/// 変換元MIDIのファイル名。フルパスで入ってると思うけど過信してはいけない
		/// </summary>
		public string SourceFileName
		{
			get { return this.sourceFileName; }
		}
		internal MoEABCScore ( string sourceName )
		{
			sourceFileName = sourceName;
			parts = new List<MoEABCPart> ();
		}
		internal void addPart ( MoEABCPart part )
		{
			parts.Add ( part );
		}

		public override string ToString ()
		{
			StringBuilder sb = new StringBuilder ();
			foreach ( MoEABCPart p in parts )
			{
				sb.Append ( p.ToString () );
			}
			return sb.ToString ();
		}

        internal MoEABCScore Clone()
        {
            MoEABCScore ret = new MoEABCScore(sourceFileName);
            foreach (MoEABCPart p in this.parts)
            {
                ret.addPart(p.Clone());
            }
            return ret;
        }

        public void parse(List<String> partsText)
        {
            if (partsText.Count != parts.Count)
            {
                throw new ParseException();
            }
 
            List<Tuple<MoEABCPart, string>> zip = parts.Zip(partsText, (p, s) => new Tuple<MoEABCPart, string>(p, s)).ToList();
            zip.ForEach((pair) =>
            {
                pair.Item1.parse(pair.Item2);
            });
        }
    }

	/// <summary>
	/// 1つの「パート」に対応するクラス。「パート」は1つのタブに表示される単位。
	/// </summary>
	public class MoEABCPart
	{
		private List<MoEABCTrack> tracks;
		/// <summary>
		/// 「パート」は0個以上の「トラック」からなる。
		/// MoEABCの仕様では1パートに2トラックまでだが、その制限は特に反映しない。
		/// (数を決め打つのはやめて、ちゃんと foreach (...) で最後まで回しましょうってこと)
		/// </summary>
		public List<MoEABCTrack> Tracks
		{
			get { return new List<MoEABCTrack> ( this.tracks ); }
		}

		private int instrument;
		/// <summary>
		/// このパートの楽器番号
		/// 値は: 0～127 = General MIDI 仕様と同じ割り当て。 200 = ドラム, 201 = パーカッション
		/// </summary>
		public int Instrument
		{
			get { return this.instrument; }
		}
		private string instrumentName;
		/// <summary>
		/// Instrument に対応する楽器の名前
		/// </summary>
		public string InstrumentName
		{
			get { return this.instrumentName; }
		}

        private int channel;
        public int Channel
        {
            get { return this.channel; }
        }
		internal MoEABCPart ( int inst, string instname, int ch)
		{
			instrument = inst;
			instrumentName = instname;
			tracks = new List<MoEABCTrack> ();
            this.channel = ch;
		}
		internal void addTrack ( MoEABCTrack track )
		{
			tracks.Add ( track );
		}

		public override string ToString ()
		{
			StringBuilder sb = new StringBuilder ();
			sb.Append ( "%% Part: " + this.InstrumentName + System.Environment.NewLine );
			foreach ( MoEABCTrack t in tracks )
			{
				sb.Append ( t.ToString () );
			}
			return sb.ToString ();
		}

        internal MoEABCPart Clone()
        {
            MoEABCPart ret = new MoEABCPart(this.instrument, this.instrumentName, this.channel);
            foreach (MoEABCTrack t in this.tracks) 
            {
                ret.addTrack(t.Clone());
            }
            return ret;
        }

        public void parse(List<char> seq)
        {
            tracks.ForEach((tr) => { seq = tr.parse(seq); });
        }
        internal void parse(string partText)
        {
            parse(partText.ToList());
        }
    }

	/// <summary>
	/// 1つの「トラック」に対応するクラス。「トラック」は "{...}" で囲まれる単位。
	/// </summary>
	public class MoEABCTrack
	{
		private List<MoEABCNote> notes;
		/// <summary>
		/// 「トラック」は0個以上の「符」からなる
		/// </summary>
		public List<MoEABCNote> Notes
		{
			get { return new List<MoEABCNote> ( this.notes ); }
		}
		// 【重要】ここから大事なのでよく読みましょう
		// 「符」は音符・休符の他、テンポ指定・ベロシティ指定等のコントロール情報も含みます

        //public List<MoEABCNote> mergeTies()
        //{
        //    List<MoEABCNote> ret = new List<MoEABCNote>();
        //    MoEABCNote[] arr = this.notes.ToArray();
        //    HashSet<MoEABCNote> merged = new HashSet<MoEABCNote>();

        //    Func<int, int, int> gcd = null;
        //    gcd = (x, y) =>
        //    {
        //        if (x < y) return gcd(y, x);
        //        int r = x % y;
        //        if (r == 0) return y;
        //        return gcd(y, r);
        //    };
        //    Func<Tuple<int, int>, Tuple<int, int>, Tuple<int, int>> addfrac = (q1, q2) =>
        //    {
        //        int d = q1.Item2 * q2.Item2;
        //        int n1 = q1.Item1 * q2.Item2;
        //        int n2 = q2.Item1 * q1.Item2;
        //        int n = n1 + n2;
        //        int g = gcd(n, d);
        //        n /= g;
        //        d /= g;
        //        return new Tuple<int, int>(n, d);
        //    };
        //    for (int i = 0; i < arr.Length; i++)
        //    {
        //        MoEABCNote note = arr[i];
        //        if (merged.Contains(note))
        //            continue;
        //        Tuple<int, int> acctime;
        //        Func<MoEABCScaleNote, MoEABCScaleNote> merge = (nt) =>
        //        {
        //            if (!nt.Tie)
        //                return nt;
        //            MoEABCScaleNote r = nt;
        //            Action<MoEABCScaleNote> doit = (nt2) =>
        //            {
        //                int d = r.LengthDenominator * nt2.LengthDenominator;
        //                int n1 = r.LengthNumerator * nt2.LengthDenominator;
        //                int n2 = nt2.LengthNumerator * r.LengthDenominator;
        //                int n = n1 + n2;
        //                int g = gcd(n, d);
        //                n /= g;
        //                d /= g;
        //                r = new MoEABCScaleNote(n, d, nt.Scale, nt.ScaleName, false);
        //                merged.Add(nt2);
        //            };
        //            for (int j = i + 1; j < arr.Length; j++)
        //            {
        //                MoEABCNote note2 = arr[j];
        //                if (note2 is MoEABCScaleNote)
        //                {
        //                    MoEABCScaleNote sn = (MoEABCScaleNote)note2;
        //                    if (sn.Scale == nt.Scale)
        //                    {
        //                        doit(sn);
        //                        acctime.Add(new Tuple<int, int>(sn.LengthNumerator, sn.LengthDenominator));
        //                        if (!sn.Tie)
        //                            return r;
        //                    }
        //                    else
        //                    {
        //                        return r;
        //                    }
        //                }
        //                else if (note2 is MoEABCChordNote)
        //                {
        //                    MoEABCChordNote cn = (MoEABCChordNote)note2;
        //                    bool found = false;
        //                    foreach (MoEABCScaleNote sn in cn.Notes)
        //                    {
        //                        if (sn.Scale == nt.Scale)
        //                        {
        //                            found = true;
        //                            doit(sn);
        //                            acctime.Add(new Tuple<int, int>(cn.LengthNumerator, cn.LengthDenominator));
        //                            if (!sn.Tie)
        //                                return r;
        //                        }
        //                    }
        //                    if (!found)
        //                        return r;
        //                }
        //                else
        //                {
        //                    return r;
        //                }
        //            }
        //            return r;
        //        };

        //        if (note is MoEABCScaleNote)
        //        {
        //            ret.Add(merge((MoEABCScaleNote)note));
        //        }
        //        else if (note is MoEABCChordNote)
        //        {
        //            List<MoEABCScaleNote> ns = new List<MoEABCScaleNote>();
        //            foreach (MoEABCScaleNote sn in ((MoEABCChordNote)note).Notes)
        //            {
        //                if (merged.Contains(sn))
        //                    continue;
        //                ns.Add(merge(sn));
        //            }
        //            if (ns.Count > 0)
        //            {
        //                ret.Add(new MoEABCChordNote(note.LengthNumerator, note.LengthDenominator, ns));
        //            }
        //        }
        //        else
        //        {
        //            ret.Add(note);
        //        }
        //    }
        //    return ret;
        //}
		internal MoEABCTrack ()
		{
			notes = new List<MoEABCNote> ();
		}
		internal void addNote ( MoEABCNote note )
		{
			notes.Add ( note );
		}
		public override string ToString ()
		{
			StringBuilder sb = new StringBuilder ();
			sb.Append ( "{" + System.Environment.NewLine );
			foreach ( MoEABCNote n in notes )
			{
				sb.Append ( n.ToString () );
			}
			sb.Append ( System.Environment.NewLine + "}" );
			return sb.ToString ();
		}

        internal MoEABCTrack Clone()
        {
            MoEABCTrack ret = new MoEABCTrack();
            foreach (MoEABCNote n in this.notes)
            {
                ret.addNote(n.Clone());
            }
            return ret;
        }

        internal List<char> parse(List<char> seq)
        {
            // ここを埋めましょう。
            // seq[0] からパーズを開始して、トラックの終了までパーズする
            // トラック終了箇所の次の文字から後を、返り値として返す。
            throw new NotImplementedException();
        }
    }

	/// <summary>
	/// 1つの「符」に対応するクラス。これが階層構造の末端。 
	/// これ自体は抽象クラスで、派生で「音符」「和音」「休符」「テンポ指定」等に分かれる 
	/// テキスト表現で "ほげn/m" という符の、n, m の値がそのまま入ります
	/// コントロール情報については、ダミーの値として 分子=0, 分母=1 が入ります
	/// n/m は「基準音符長」(<L:1/X>で指定されるもの) の相対値であることに注意。
	/// 現状、このツールで変換した譜面は、<L:1/4> (四分音符基準、例えば "1/2" なら四分音符の 1/2 = 八分音符)
	/// で固定ですが、それをあてこむのはやめよう。
	/// あと、13/65 とか、半端な数字になることもあるので、
	/// switch { case 2: ... case 4: ... } みたいなのもやめよう。
	/// </summary>
	public abstract class MoEABCNote
	{
        internal abstract MoEABCNote Clone();
		private int lengthNumerator; // 音符長の分子
		private int lengthDenominator; // 音符長の分母
		/// <summary>
		/// 音符長の分子
		/// </summary>
		public int LengthNumerator
		{
			get { return this.lengthNumerator; }
		}
		/// <summary>
		/// 音符長の分母
		/// </summary>
		public int LengthDenominator
		{
			get { return this.lengthDenominator; }
		}
		protected MoEABCNote ( int n, int m )
		{
			lengthNumerator = n;
			lengthDenominator = m;
		}

		internal string getLengthCode ()
		{
			return lengthNumerator.ToString () + "/" + lengthDenominator.ToString ();
		}
	}

	/// <summary>
	/// 「音符」に対応するクラス。一番基本 
	/// </summary>
	public class MoEABCScaleNote : MoEABCNote
	{
		private int scale;
		/// <summary>
		/// 音階。0～127まで。曲調とか一切関係なく絶対音階であることに注意
		/// 0 が一番低いC, 127 が一番高い G, 60 がいわゆる「ド」のC
		/// </summary>
		public int Scale
		{
			get { return this.scale; }
		}

		private string scaleName;
		/// <summary>
		/// 音階のテキスト表現。"C" とか "^F" とか
		/// #, ♭は全てハ長調基準でついていることに注意
		/// </summary>
		public string ScaleName
		{
			get { return this.scaleName; }
		}

        private bool tie;
        public bool Tie
        {
            get { return this.tie; }
        }
		internal MoEABCScaleNote ( int n, int m, int s, string sn, bool tie)
			: base ( n, m )
		{
            this.tie = tie;
			scale = s;
			scaleName = sn;
		}
		public override string ToString ()
		{
			return this.ScaleName + this.getLengthCode ();
		}

        internal override MoEABCNote Clone()
        {
            return new MoEABCScaleNote(this.LengthNumerator, this.LengthDenominator, this.scale, this.scaleName, this.tie);
        }
	}

	/// <summary>
	/// 「休符」に対応するクラス。 
	/// 長さしか情報がないので追加のプロパティはなし
	/// </summary>
	public class MoEABCRestNote : MoEABCNote
	{
		internal MoEABCRestNote ( int n, int m ) : base ( n, m ) { }
		public override string ToString ()
		{
			return "z" + this.getLengthCode ();
		}
        internal override MoEABCNote Clone()
        {
            return new MoEABCRestNote(this.LengthNumerator, this.LengthDenominator);
        }
    }

	/// <summary>
	/// 「和音」に対応するクラス。
	/// </summary>
	public class MoEABCChordNote : MoEABCNote
	{
		private List<MoEABCScaleNote> notes;
		/// <summary>
		/// 和音を構成する音階のリスト
		/// 「要素が一つもない」リストはまずありえないが (あったらバグ)、「一つしかない」ものは普通にあるので注意
		/// リストの各要素の音階が、それぞれの長さだけ鳴る。次の音を鳴らすタイミングは、この和音自体の分子・分母で決まる。
		/// 例： [C1/2 E1/2]1/4 D1/2 は、
		///      C ----
		///      E ----
		///      D   ----
		/// こういうタイミングで重なる
		/// </summary>
		public List<MoEABCScaleNote> Notes
		{
			get { return new List<MoEABCScaleNote> ( this.notes ); }
		}

		internal MoEABCChordNote ( int n, int m, List<MoEABCScaleNote> ns )
			: base ( n, m )
		{
			notes = new List<MoEABCScaleNote> ( ns );
		}
		public override string ToString ()
		{
			StringBuilder sb = new StringBuilder ();
			sb.Append ( '[' );
			foreach ( MoEABCScaleNote sn in notes )
			{
				sb.Append ( sn.ToString () );
				sb.Append ( ' ' );
			}
			sb.Append ( ']' );
			sb.Append ( this.getLengthCode () );
			return sb.ToString ();
		}
        internal override MoEABCNote Clone()
        {
            List<MoEABCScaleNote> ns = new List<MoEABCScaleNote>();
            foreach (MoEABCScaleNote n in this.notes)
            {
                ns.Add((MoEABCScaleNote)n.Clone());
            }
            MoEABCChordNote ret = new MoEABCChordNote(this.LengthNumerator, this.LengthDenominator, ns);
            return ret;
        }
    }

	/// <summary>
	/// 「基準音符値設定」に対応するクラス。&lt;L:n/m&gt;
	/// 現状では、各トラックの最初に一度 &lt;L:1/4&gt; が来るだけのはず
	/// 長さしか情報がないので追加のプロパティはなし
	/// </summary>
	public class MoEABCBaseSpecifierNote : MoEABCNote
	{
		internal MoEABCBaseSpecifierNote ( int n, int m )
			: base ( n, m )
		{
		}
		public override string ToString ()
		{
			return "<L:" + this.getLengthCode () + ">";
		}
        internal override MoEABCNote Clone()
        {
            return new MoEABCBaseSpecifierNote(this.LengthNumerator, this.LengthDenominator);
        }
    }

	/// <summary>
	/// 「テンポ設定」に対応するクラス。"&lt;Q:n&gt;"
	/// これは現状でも1トラック中で変化しうる
	/// </summary>
	public class MoEABCTempoNote : MoEABCNote
	{
		private int value;
		// 設定値。32～255
		public int Value
		{
			get { return this.value; }
		}
		internal MoEABCTempoNote ( int val )
			: base ( 0, 1 )
		{
			value = val;
		}
		public override string ToString ()
		{
			return "<Q:" + this.value + ">";
		}
        internal override MoEABCNote Clone()
        {
            return new MoEABCTempoNote(this.value);
        }
    }

	/// <summary>
	/// ベロシティ指定 "&lt;v:n&gt;" 
	/// </summary>
    public class MoEABCVelocityNote : MoEABCNote
    {
        private int value;
        // 設定値。1～127
        public int Value
        {
            get { return this.value; }
        }
        internal MoEABCVelocityNote(int val)
            : base(0, 1)
        {
            value = val;
        }
        public override string ToString()
        {
            return "<v:" + this.value + ">";
        }
        internal override MoEABCNote Clone()
        {
            return new MoEABCVelocityNote(this.value);
        }
    }

    // 以下は、現状の変換では出現しないが、カタチだけ定義

    /// <summary>
	/// ボリューム指定 "%lt;V:n%gt;"
	/// </summary>
	public class MoEABCVolumeNote : MoEABCNote
	{
		private int value;
		// 設定値。1～127
		public int Value
		{
			get { return this.value; }
		}
		internal MoEABCVolumeNote ( int val )
			: base ( 0, 1 )
		{
			value = val;
		}
		public override string ToString ()
		{
			return "<V:" + this.value + ">";
		}
        internal override MoEABCNote Clone()
        {
            return new MoEABCVolumeNote(this.value);
        }
	}

	/// <summary>
	/// 曲調指定 "&lt;K:ほげ&gt;"
	/// MIDIが絶対音階なので、曲調を扱うことはまずないと思われる
	/// </summary>
	public class MoEABCKeyNote : MoEABCNote
	{
		private string value;
		public string Value
		{
			get { return this.value; }
		}
		// 設定値。"C#" とか "A#m" とかテキスト表現のままでいいや...どうせ使わないし
		internal MoEABCKeyNote ( string val )
			: base ( 0, 1 )
		{
			value = val;
		}
		public override string ToString ()
		{
			return "<K:" + this.value + ">";
		}
        internal override MoEABCNote Clone()
        {
            return new MoEABCKeyNote(this.value);
        }
	}
}
