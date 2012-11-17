using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Interval;

namespace Midi
{

    enum SequenceType
    {
        Smtpe,
        Ppqn
    }

    class MidiStreamUtil
    {
        public class UnexpectedEof : Exception
        {
        }

        public static bool findMarker(FileStream fs, string marker)
        {
            foreach (char c in marker) {
                if (fs.ReadByte() != c) return false;
            }
            return true;
        }


        public static uint readu32(FileStream fs)
        {
            byte[] data = new byte[4];
            int result = fs.Read(data, 0, data.Length);
            if (result != data.Length)
                throw new UnexpectedEof();

            if (BitConverter.IsLittleEndian)
                Array.Reverse(data);
            return BitConverter.ToUInt32(data, 0);
        }
        public static int read32(FileStream fs)
        {
            byte[] data = new byte[4];
            int result = fs.Read(data, 0, data.Length);
            if (result != data.Length)
                throw new UnexpectedEof();

            if (BitConverter.IsLittleEndian)
                Array.Reverse(data);
            return BitConverter.ToInt32(data, 0);
        }
        public static ushort readu16(FileStream fs)
        {
            byte[] data = new byte[2];
            int result = fs.Read(data, 0, data.Length);
            if (result != data.Length)
                throw new UnexpectedEof();

            if (BitConverter.IsLittleEndian)
                Array.Reverse(data);
            return BitConverter.ToUInt16(data, 0);
        }
        public static short read16(FileStream fs)
        {
            byte[] data = new byte[2];
            int result = fs.Read(data, 0, data.Length);
            if (result != data.Length)
                throw new UnexpectedEof();

            if (BitConverter.IsLittleEndian)
                Array.Reverse(data);
            return BitConverter.ToInt16(data, 0);
        }
    }

    public class MidiSequence
    {
        public static readonly string[] INSTRUMENTS = {
                                                    "アコースティックピアノ",
                                                    "ブライトピアノ",
                                                    "エレクトリックグランドピアノ",
                                                    "ホンキートンクピアノ",
                                                    "エレクトリックピアノ",
                                                    "エレクトリックピアノ2",
                                                    "ハープシコード",
                                                    "クラビネット",
                                                    "チェレスタ",
                                                    "グロッケンシュピール",
                                                    "オルゴール",
                                                    "ヴィブラフォン",
                                                    "マリンバ",
                                                    "シロフォン",
                                                    "チューブラーベル",
                                                    "ダルシマー",
                                                    "ドローバーオルガン",
                                                    "パーカッシブオルガン",
                                                    "ロックオルガン",
                                                    "チャーチオルガン",
                                                    "リードオルガン",
                                                    "アコーディオン",
                                                    "ハーモニカ",
                                                    "タンゴアコーディオン",
                                                    "アコースティックギター（ナイロン弦）",
                                                    "アコースティックギター（スチール弦）",
                                                    "ジャズギター",
                                                    "クリーンギター",
                                                    "ミュートギター",
                                                    "オーバードライブギター",
                                                    "ディストーションギター",
                                                    "ギターハーモニクス",
                                                    "アコースティックベース",
                                                    "フィンガー・ベース",
                                                    "ピック・ベース",
                                                    "フレットレスベース",
                                                    "スラップベース 1",
                                                    "スラップベース 2",
                                                    "シンセベース 1",
                                                    "シンセベース 2",
                                                    "ヴァイオリン",
                                                    "ヴィオラ",
                                                    "チェロ",
                                                    "コントラバス",
                                                    "トレモロ",
                                                    "ピッチカート",
                                                    "ハープ",
                                                    "ティンパニ",
                                                    "ストリングアンサンブル 1",
                                                    "ストリングアンサンブル 2",
                                                    "シンセストリングス 1",
                                                    "シンセストリングス 2",
                                                    "声「あー」",
                                                    "声「おー」",
                                                    "シンセヴォイス",
                                                    "オーケストラヒット",
                                                    "トランペット",
                                                    "トロンボーン",
                                                    "チューバ",
                                                    "ミュートトランペット",
                                                    "フレンチ・ホルン",
                                                    "ブラスセクション",
                                                    "シンセブラス 1",
                                                    "シンセブラス 2",
                                                    "ソプラノサックス",
                                                    "アルトサックス",
                                                    "テナーサックス",
                                                    "バリトンサックス",
                                                    "オーボエ",
                                                    "イングリッシュホルン",
                                                    "ファゴット",
                                                    "クラリネット",
                                                    "ピッコロ",
                                                    "フルート",
                                                    "リコーダー",
                                                    "パンフルート",
                                                    "茶瓶",
                                                    "尺八",
                                                    "口笛",
                                                    "オカリナ",
                                                    "矩形波",
                                                    "ノコギリ波",
                                                    "カリオペ",
                                                    "チフ",
                                                    "チャラング",
                                                    "声",
                                                    "フィフスズ",
                                                    "バス + リード",
                                                    "ファンタジア",
                                                    "ウォーム",
                                                    "ポリシンセ",
                                                    "クワイア",
                                                    "ボウ",
                                                    "メタリック",
                                                    "ハロー",
                                                    "スウィープ",
                                                    "雨",
                                                    "サウンドトラック",
                                                    "クリスタル",
                                                    "アトモスフィア",
                                                    "ブライトネス",
                                                    "ゴブリン",
                                                    "エコー",
                                                    "サイファイ",
                                                    "シタール",
                                                    "バンジョー",
                                                    "三味線",
                                                    "琴",
                                                    "カリンバ",
                                                    "バグパイプ",
                                                    "フィドル",
                                                    "シャハナーイ",
                                                    "ティンクルベル",
                                                    "アゴゴ",
                                                    "スチールドラム",
                                                    "ウッドブロック",
                                                    "太鼓",
                                                    "メロディックタム",
                                                    "シンセドラム",
                                                    "逆シンバル",
                                                    "ギターフレットノイズ",
                                                    "ブレスノイズ",
                                                    "海岸",
                                                    "鳥の囀り",
                                                    "電話のベル",
                                                    "ヘリコプター",
                                                    "拍手",
                                                    "銃声",
                                                   };

        string fileName = null;

        public string FileName
        {
            get { return this.fileName; }
        }
        class MidiFileProperties
        {
            uint headerLength = 0;

            int format = -1;
            public int Format
            {
                get
                {
                    return this.format;
                }
                set
                {
                    if (value < 0 || value > 3)
                        throw new Exception("Invalid MIDI format: " + value.ToString());
                    if (value == 0 && this.TrackCount > 1)
                        throw new Exception("Track count mismatches the format");
                    this.format = value;
                }
            }

            int trackCount = 0;
            public int TrackCount
            {
                get
                {
                    return this.trackCount;
                }
                set
                {
                    if (value < 0)
                        throw new Exception("Invalid track count: " + value.ToString());
                    if (value > 1 && this.Format == 0)
                        throw new Exception("Track count mismatches the format");
                    this.trackCount = value;
                }

            }

            int division = 0;
            SequenceType seqType;
            public int Division
            {
                get
                {
                    return this.division;
                }
                set
                {
                    byte[] data = BitConverter.GetBytes((short)value);
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(data);
                    if ((sbyte)data[0] < 0)
                    {
                        this.seqType = SequenceType.Smtpe;
                        this.division = -(sbyte)data[0] * (sbyte)data[1];
                    }
                    else
                    {
                        this.seqType = SequenceType.Ppqn;
                        this.division = value;
                        
                    }

                }
            }

            public SequenceType SeqType
            {
                get
                {
                    return this.seqType;
                }
            }


            public MidiFileProperties(FileStream fs)
            {
                if (!MidiStreamUtil.findMarker(fs, "MThd")) 
                    throw new Exception("Error: MIDI file header not found");

                try
                {
                    this.headerLength = MidiStreamUtil.readu32(fs);
                    this.Format = MidiStreamUtil.readu16(fs);
                    this.TrackCount = MidiStreamUtil.readu16(fs);
                    this.Division = MidiStreamUtil.readu16(fs);
                }
                catch (MidiStreamUtil.UnexpectedEof)
                {
                    throw new Exception("Error: MIDI file header length broken");
                }
            }

            public override string ToString()
            {
                return
                    "Header Length: " + headerLength.ToString() + Environment.NewLine +
                    "Format: " + Format.ToString() + Environment.NewLine +
                    "Track Count: " + TrackCount.ToString() + Environment.NewLine +
                    "Division: " + Division.ToString() + Environment.NewLine +
                    "Sequence Type: " + seqType.ToString();
            }
        }
        MidiFileProperties properties = null;

        public static int quantizationDivisor = 16;  // Quantization の単位となる拍数: 4 = 16分音符 8 = 32分音符 16 = 64分音符 ...
        public static int fusionThreshold = 0;     // 2つ以上の音色を和音としてまとめる閾値の拍数: 0 = 単位時間, 4 = 1/16, 8 = 1/32, 16 = 1/64, ...
        public static bool optimizeWs = false; // true => 空白除去
        public static bool reflectPedalSustantion = true; // true => Sustain Pedal を変換に反映
        // 以下はもう使わない
//        public static bool optimizeLen = false; // true => 音符長の最頻値に合わせてテンポ調整
//        public static bool splitTrack = false; // true => 音の重なる部分を別トラックに分割

        public enum QuantizationMode
        {
            ScoreLike,
            No5th,
            No3rd5th,
            NoDotBelow16th,
            NoDotBelow8th,
            QuantumStep,
            NoRestriction
        };
        public static QuantizationMode quantizationMode = QuantizationMode.QuantumStep;

        public enum RestRemovalMode
        {
            AbsorbToLeadingNote,
            Remove,
        };
        public static RestRemovalMode restRemovalMode = RestRemovalMode.AbsorbToLeadingNote;
        public static int restRemovalThreshold = 0;

        public enum PartDivisionPolicy
        {
            SoloMelody, // メロディパートのみすべてマージ (ソロ)
            DrumOnly,       // ドラムパートのみ
            PercussionOnly, // パーカッションのみ
            Trio, // セッション、メロディはすべてマージ
            Session, // セッション、入力ファイルのトラックに忠実に分割
            SessionWithPartitioning // セッション、重複する音を複数トラックに分割
        }
        public static PartDivisionPolicy partDivisionPolicy = PartDivisionPolicy.Session;

        public enum VelocityKeepingPolicy
        {
            None,
            RoundBy10, // 10刻み
            RoundBy20, // 20刻み
            RoundBy50, // 40刻み
            Literal,
        }
        public static VelocityKeepingPolicy velocityKeepingPolicy = VelocityKeepingPolicy.None;

        public enum ShortNoteAdjustment
        {
            Discard,
            Adjust
        }
        public static ShortNoteAdjustment shortNoteAdjustment = ShortNoteAdjustment.Discard;

        public abstract class Note
        {
            private int startTick, len, channel;
            protected MidiTrack parent;
            internal bool allowModify = true;
            public Note(MidiTrack parent, int tick, int len, int ch)
            {
                this.parent = parent;
                this.startTick = tick;
                this.len = len;
                this.channel = ch;
            }

            public MidiTrack Parent
            {
                get { return this.parent; }
            }
            public int Channel
            {
                get { return this.channel; }
            }
            private int quantizedStartTick = -1;
            public int QuantizedStartTick
            {
                get { return quantizedStartTick; }
            }
            public void quantize(int q)
            {
                quantizedStartTick = q;
            }

            public int Length
            {
                get { return this.len; }
                set 
                {
                    Debug.Assert(this.allowModify);
                    this.len = value; 
                }
            }
            private int quantizedLength = -1;
            public int QuantizedLength
            {
                get { return this.quantizedLength; }
                set 
                {
                    Debug.Assert(this.allowModify);
                    this.quantizedLength = value; 
                }
            }

            public int StartTick
            {
                get { return this.startTick; }
            }

            internal static Tuple<int, int> calcLen(int len, int qnt) {
                decimal minStep = quantizationDivisor <= 0 ? 1.0m : (decimal)qnt / (decimal)quantizationDivisor;
                int n = (int)Math.Round((decimal)len / minStep);
                if (n == 0)
                    return null;

                Func<int, int, int> gcd = null;
                gcd = (x, y) =>
                {
                    if (x < y) return gcd(y, x);
                    int r = x % y;
                    if (r == 0) return y;
                    return gcd(y, r);
                };

                int g = gcd(len, qnt);
                int a = len / g;
                int b = qnt / g;
                return new Tuple<int, int>(a, b);
            }
            public static String calcLenCode(int len, int qnt)
            {
                Tuple<int, int> p = calcLen(len, qnt);
                if (p == null)
                    return null;
                int a = p.Item1;
                int b = p.Item2;
                if (a == 1 && b == 1) return "";
                if (a == 1) return "/" + (b == 2 ? "" : b.ToString());
                if (b == 1) return a.ToString();
                return a.ToString() + "/" + (b == 2 ? "" : b.ToString());
            }
            internal static Tuple<int, int> parseLenCode(string s)
            {
                try
                {
                    if (s == null || s.Equals(""))
                    return new Tuple<int, int>(1, 1);
                    if (s.Equals("/"))
                        return new Tuple<int,int>(1, 2);
                    if (s.IndexOf('/') < 0)
                        return new Tuple<int, int>(Int32.Parse(s), 1);
                    char[] delim = { '/' };
                    string[] ns = s.Split(delim, 2, StringSplitOptions.None);
                    if (ns.Length == 1)
                    {
                        return new Tuple<int, int>(Int32.Parse(ns[0]), 1);
                    }
                    string ns1 = ns[0];
                    string ns2 = ns[1];
                    int n1 = (ns1.Equals("") ? 1 : Int32.Parse(ns1));
                    int n2 = (ns2.Equals("") ? 2 : Int32.Parse(ns2));
                    return new Tuple<int, int>(n1, n2);
                }
                catch (Exception)
                {
                    return new Tuple<int, int>(0, 1);
                }
            }
            private string forcedLencode = null;
            internal void forceLenCode(string c)
            {
                Debug.Assert(this.allowModify);
                forcedLencode = c;
            }
            internal void clearLenCode()
            {
                Debug.Assert(this.allowModify);
                forcedLencode = null;
            }
            public string getLenCode(int qnt)
            {
                if (forcedLencode != null)
                    return forcedLencode;
                string ret = calcLenCode(this.quantizedLength, qnt);
                return ret;
            }
            public abstract string purge(int t);
            public abstract Note clone(MidiTrack parent, int tick, int len);
        }

        public class DummyNote : Note, ICloneable
        {
            public DummyNote(MidiTrack parent, int tick, int len, int ch)
                : base(parent, tick, len, ch)
            {
            }
            public override string purge(int t)
            {
                return "";
            }
            public override Note clone(MidiTrack parent, int tick, int len)
            {
                Note ret = new DummyNote(parent, tick, len, this.Channel);
                ret.allowModify = true;
                return ret;
            }

            public object Clone()
            {
                Note ret = (Note)MemberwiseClone();
                ret.allowModify = true;
                return ret;
            }
        }
        public class ScaleNote : Note, ICloneable
        {
            private static readonly int MIN_SCALE = 12, MAX_SCALE = 119;

            private static string[] CODES = { "C", "^C", "D", "^D", "E", "F", "^F", "G", "^G", "A", "^A", "B" };

            int scale;
            int instrument;
            bool tie;
            int velocity;

            public ScaleNote(MidiTrack parent, int tick, int scale, int len, int vel, int instrument, int ch)
                : base(parent, tick, len, ch)
            {
                this.scale = scale;
                this.instrument = instrument;
                this.tie = false;
                this.velocity = vel;
            }

            public int Instrument
            {
                get { return this.instrument; }
                set 
                {
                    Debug.Assert(this.allowModify);
                    this.instrument = value; 
                }
            }
            public int Velocity
            {
                get { return this.velocity; }
                set 
                {
                    Debug.Assert(this.allowModify);
                    this.velocity = value; 
                }
            }
            public int Scale
            {
                get { return this.scale; }
                set 
                {
                    Debug.Assert(this.allowModify);
                    this.scale = value; 
                }
            }
            public bool Tie 
            {
                get { return this.tie; }
                set 
                {
                    Debug.Assert(this.allowModify);
                    this.tie = value;
                }
            }

            internal string getScaleCode()
            {
                int s = scale;
                while (s < MIN_SCALE)
                    s += 12;
                while (s > MAX_SCALE)
                    s -= 12;

                int oct = s / 12;

                // s ∈ {12...127} && oct ∈ {1...9}
                string octsym = "";
                bool octhigh = false;
                switch (oct)
                {
                    case 1:
                        octsym = ",,,,";
                        break;
                    case 2:
                        octsym = ",,,";
                        break;
                    case 3:
                        octsym = ",,";
                        break;
                    case 4:
                        octsym = ",";
                        break;
                    case 6:
                        octhigh = true;
                        break;
                    case 7:
                        octhigh = true;
                        octsym = "'";
                        break;
                    case 8:
                        octhigh = true;
                        octsym = "''";
                        break;
                    case 9:
                        octhigh = true;
                        octsym = "'''";
                        break;
                }

                String code = CODES[(s - MIN_SCALE) % 12];

                if (octhigh)
                    code = code.ToLowerInvariant();
                return code + octsym;
            }

            public override string purge(int qnt)
            {
                string lensym = getLenCode(qnt);

                if (lensym == null)
                    return "";

                string code = getScaleCode();
                if (code == null)
                    return "";
                return code + lensym + (tie ? "-" : "");
            }
            public override Note clone(MidiTrack parent, int tick, int len)
            {
                Note ret = new ScaleNote(parent, tick, this.scale, len, this.velocity, this.instrument, this.Channel);
                ret.allowModify = true;
                return ret;
            }
            public virtual object Clone()
            {
                Note ret = (Note)MemberwiseClone();
                ret.allowModify = true;
                return ret;
            }
        }

        public class DrumNote : ScaleNote, ICloneable
        {
            public DrumNote(MidiTrack parent, int tick, int scale, int len, int vel, int instrument, int ch)
                : base(parent, tick, scale, len, vel, -1, ch)
            {
            }

            public override string purge(int qnt)
            {
                int s = this.Scale;
                // ↓関係ありました！
                //// ドラムは音符長関係ない
                string lensym = getLenCode(qnt);

                string code = null;
                switch (s) 
                {
                    case 35:
                        code = "_C";
                        break;
                    case 36:
                        code = "C";
                        break;
                    case 37:
                        code = "^C";
                        break;
                    case 38:
                        code = "D";
                        break;
                    case 39:
                        code = "^D";
                        break;
                    case 40:
                        code = "E";
                        break;
                    case 41:
                        code = "F";
                        break;
                    case 42:
                        code = "^F";
                        break;
                    case 43:
                        code = "G";
                        break;
                    case 44:
                        code = "^G";
                        break;
                    case 45:
                        code = "A";
                        break;
                    case 46:
                        code = "^A";
                        break;
                    case 47:
                        code = "B";
                        break;
                    case 48:
                        code = "c";
                        break;
                    case 49:
                        code = "^c";
                        break;
                    case 50:
                        code = "d";
                        break;
                    case 51:
                        code = "^d";
                        break;
                    case 52:
                        code = "e";
                        break;
                    case 53:
                        code = "f";
                        break;
                    case 54:
                        code = "^f";
                        break;
                    case 55:
                        code = "g";
                        break;
                    case 56:
                        code = "^g";
                        break;
                    case 57:
                        code = "a";
                        break;
                    case 58:
                        code = "^a";
                        break;
                    case 59:
                        code = "b";
                        break;
                }

                if (lensym == null)
                    return "";

                if (code == null)
                    return "";
                return code + lensym + (this.Tie ? "-" : "");
            }
            public override Note clone(MidiTrack parent, int tick, int len)
            {
                Note ret = new DrumNote(parent, tick, this.Scale, len, this.Velocity, this.Instrument, this.Channel);
                ret.allowModify = true;
                return ret;
            }
            public override object Clone()
            {
                Note ret = (Note)MemberwiseClone();
                ret.allowModify = true;
                return ret;
            }
        }

        public class PercussionNote : ScaleNote, ICloneable
        {
            public PercussionNote(MidiTrack parent, int tick, int scale, int len, int vel, int instrument, int ch)
                : base(parent, tick, scale, len, vel, -1, ch)
            {
            }

            public override string purge(int qnt)
            {
                int s = this.Scale;

                string lensym = getLenCode(qnt);

                string code = null;
                switch (s)
                {
                    case 60:
                        code = "C";
                        break;
                    case 61:
                        code = "^C";
                        break;
                    case 62:
                        code = "D";
                        break;
                    case 63:
                        code = "^D";
                        break;
                    case 64:
                        code = "E";
                        break;
                    case 65:
                        code = "F";
                        break;
                    case 66:
                        code = "^F";
                        break;
                    case 67:
                        code = "G";
                        break;
                    case 68:
                        code = "^G";
                        break;
                    case 69:
                        code = "A";
                        break;
                    case 70:
                        code = "^A";
                        break;
                    case 71:
                        code = "B";
                        break;
                    case 72:
                        code = "c";
                        break;
                    case 73:
                        code = "^c";
                        break;
                    case 74:
                        code = "d";
                        break;
                    case 75:
                        code = "^d";
                        break;
                    case 76:
                        code = "e";
                        break;
                    case 77:
                        code = "f";
                        break;
                    case 78:
                        code = "^f";
                        break;
                    case 79:
                        code = "g";
                        break;
                    case 80:
                        code = "^g";
                        break;
                    case 81:
                        code = "a";
                        break;
                }

                if (code == null)
                    return "";

                return code + lensym + (this.Tie ? "-" : "");
            }
            public override Note clone(MidiTrack parent, int tick, int len)
            {
                Note ret = new PercussionNote(parent, tick, this.Scale, len, this.Velocity, this.Instrument, this.Channel);
                ret.allowModify = true;
                return ret;
            }
            public override object Clone()
            {
                Note ret = (Note)MemberwiseClone();
                ret.allowModify = true;
                return ret;
            }
        }

        public class RestNote : Note, ICloneable
        {
            public RestNote(MidiTrack parent, int tick, int len, int ch)
                : base(parent, tick, len, ch)
            {
            }
            public override string  purge(int qnt)
            {
                string lensym = getLenCode(qnt);

                if (lensym == null)
                    return "";

                return "z" + lensym;
            }
            public override Note clone(MidiTrack parent, int tick, int len)
            {
                Note ret = new RestNote(parent, tick, len, this.Channel);
                ret.allowModify = true;
                return ret;
            }
            public object Clone()
            {
                Note ret = (Note)MemberwiseClone();
                ret.allowModify = true;
                return ret;
            }
        }

        public class TempoChangeNote : Note, ICloneable
        {
            int tempo;
            public int Tempo {
                get { return this.tempo; }
            }

            public TempoChangeNote(MidiTrack parent, int tick, int tempo, int ch)
                : base(parent, tick, 0, ch)
            {
                this.tempo = tempo;
            }
            public override string purge(int t) {
                return "";
            }
            public override Note clone(MidiTrack parent, int tick, int len)
            {
                Note ret = new TempoChangeNote(parent, tick, this.tempo, this.Channel);
                ret.allowModify = true;
                return ret;
            }
            public object Clone()
            {
                Note ret = (Note)MemberwiseClone();
                ret.allowModify = true;
                return ret;
            }
        }

        public class VolumeNote : Note, ICloneable
        {
            int value;
            public int Value
            {
                get { return this.value; }
            }
            public VolumeNote(MidiTrack parent, int tick, int val, int ch)
                : base(parent, tick, 0, ch)
            {
                this.value = val;
            }
            public override string purge(int t)
            {
                return "";
            }
            public override Note clone(MidiTrack parent, int tick, int len)
            {
                Note ret =  new VolumeNote(parent, tick, this.value, this.Channel);
                ret.allowModify = true;
                return ret;
            }
            public object Clone()
            {
                Note ret = (Note)MemberwiseClone();
                ret.allowModify = true;
                return ret;
            }
        }

        public static Func<Note, int, bool> filter = null;
        public class MidiTrack
        {
            // このトラックに含まれる全ての音符のリスト
            // このリストはファイル読み込み時に作成され、以後変化しない
            private List<Note> notes = new List<Note>();
            internal List<Note> Notes
            {
                get { return this.notes; }
            }
            // Tick数と、その瞬間に鳴らされる音との対応表
            // この表はクォンタイゼーションの計算ごとに変化する
            private Dictionary<int, List<Note>> soundTable = new Dictionary<int, List<Note>>();

            public int NumNotes
            {
                get { return notes.Count; }
            }

            private MoEABCScore.MoEABCPart moeABCPart;
            internal virtual MoEABCScore.MoEABCPart asMoEABCPart()
            {
                return moeABCPart;
            }
            private void doAdd(Note n)
            {

                List<Note> l;

                if (soundTable.TryGetValue(n.QuantizedStartTick, out l))
                {
                    l.Add(n);
                }
                else
                {
                    l = new List<Note>();
                    l.Add(n);
                    soundTable[n.QuantizedStartTick] = l;
                }
            }
            private void addTableEntry(Note n, int maxlen)
            {

                if (maxlen == 0)
                {
                    doAdd(n);
                    return;
                }

                int l = n.Length;

                if (l <= maxlen)
                {
                    doAdd(n);
                    return;
                }
                int t = n.QuantizedStartTick;
                while (l > maxlen)
                {
                    Note n2 = n.clone(this, t, maxlen);
                    n2.quantize(t);

                    doAdd(n2);
                    l -= maxlen;
                    t += maxlen;
                    if (n2 is ScaleNote && l > 0)
                    {
                        ((ScaleNote)n2).Tie = true;
                    }
                }
                if (l > 0)
                {
                    Note n2 = n.clone(this, t, l);
                    n2.quantize(t);
                    doAdd(n2);
                }
            }
             

            private int trackNumber;
            public int TrackNumber
            {
                get { return this.trackNumber;  }
                set { this.trackNumber = value; }

            }

            private int length;
            public int Length
            {
                get { return this.length; }
            }

            private string title;
            public string Title {
                get { return this.title; }
                set { this.title = value; }
            }

            private int representativeInstrument = -1;
            public int Instrument
            {
                get { return this.representativeInstrument < 0 ? 0 : this.representativeInstrument; }
                set { this.representativeInstrument = value; }
            }
            private string instrumentName = "";
            public string InstrumentName {
                get 
                { 
                    if (this.instrumentName != null && !this.instrumentName.Equals(""))
                        return this.instrumentName;
                    if (this.Instrument >= 0 && this.Instrument < MidiSequence.INSTRUMENTS.Length)
                        return MidiSequence.INSTRUMENTS[this.Instrument];

                    if (this.Instrument == 200)
                        return "ドラムセット";
                    if (this.Instrument == 201)
                        return "パーカッションセット";

                    return "";
                }
                set { this.instrumentName = value; }
            }
            //private int tempo;
            //public int Tempo
            //{
            //    get { return this.tempo; }
            //    set { this.tempo = value; }
            //}

            private readonly MidiSequence parent;
            public MidiSequence Parent
            {
                get { return parent; }
            }

            public MidiTrack(int n, FileStream fs, MidiSequence parent)
            {
                this.parent = parent;
//                this.tempo = defaultTempo;
                this.trackNumber = n;
                MidiStreamUtil.findMarker(fs, "MTrk");
                this.length = MidiStreamUtil.read32(fs);

                byte[] data = new byte[this.Length];
                int r = fs.Read(data, 0, data.Length);
                if (r < 0)
                    throwEofException();

                Func<int, int, int, int, int, int, Note> allocNote;
                // トラック番号じゃなくてチャンネルで判定した方が正しいらしい？ので下はなかったことに
                //if (n == 10)
                //{
                //    // トラック10番は打楽器とみなして、ストリーム解析にフックをかける。
                //    // 直接 new DrumNote()/PercussionNote() ではなく、チャンネルを強制的に10番にする。
                //    allocNote = (start, scale, len, vel, inst, ch) => new ScaleNote(start, scale, len, vel, inst, 10);
                //}
                //else
                //{
                allocNote = (start, scale, len, vel, inst, ch) => new ScaleNote(this, start, scale, len, vel, inst, ch);
                //}
                parse(data, allocNote);
                int c = 0;
                foreach (Note nt in notes.Where((nt) => nt is ScaleNote))
                {
                    c = nt.Channel;
                    break;
                }
                this.moeABCPart = new MoEABCScore.MoEABCPart(this.Instrument, this.InstrumentName, c);
                moeABCPart = new MoEABCScore.MoEABCPart(this.Instrument, this.InstrumentName, c);

                // これ以降、各Noteの変更を禁止
                foreach (Note nt in notes)
                {
                    nt.allowModify = false;
                }
            }

            public MidiTrack(MidiTrack source)
            {
                this.parent = source.parent;
                this.trackNumber = source.TrackNumber;
                this.notes = new List<Note>(source.notes.Select((n) => n.clone(this, n.StartTick, n.Length)));
                this.Instrument = source.Instrument;
                this.InstrumentName = source.InstrumentName;
                this.Title = source.Title;
                int c = 0;
                foreach (Note n in notes.Where((n) => n is ScaleNote))
                {
                    c = n.Channel;
                    break;
                }
                this.moeABCPart = new MoEABCScore.MoEABCPart(this.Instrument, this.InstrumentName, c);
                moeABCPart = new MoEABCScore.MoEABCPart(this.Instrument, this.InstrumentName, c);
            }

            public MidiTrack(int p, List<Note> notes, MidiSequence midiSequence)
            {
                this.trackNumber = p;
                this.notes = new List<Note>(notes.Select((n) => n.clone(this, n.StartTick, n.Length)));
                this.parent = midiSequence;
                this.representativeInstrument = 0;
                int c = 0;
                foreach (Note n in notes.Where((n) => n is ScaleNote))
                {
                    c = n.Channel;
                    break;
                }
                this.moeABCPart = new MoEABCScore.MoEABCPart(this.Instrument, this.InstrumentName, c);
                moeABCPart = new MoEABCScore.MoEABCPart(this.Instrument, this.InstrumentName, c);
            }
            private void throwEofException()
            {
                throw new Exception("Unexpected End-of-File at track " + this.TrackNumber.ToString());
            }

//            private int latestStartTick = 0;
//            private int numNotes = 0;
            private void parse(byte[] data, Func<int, int, int, int, int, int, Note> allocNote)
            {
//                int instrument = 0;
                int index = 0;
                int tick = 0;

                int ev = 0;

                const int MAX_MIDI_CH = 16;
                const int NUM_SCALE = 128;
                int[,] startTickTable = new int[MAX_MIDI_CH,NUM_SCALE];
                bool[,] sustentionStateTable = new bool[MAX_MIDI_CH, NUM_SCALE];

                int[] instruments = new int[MAX_MIDI_CH];
                
                for (int i = 0; i<MAX_MIDI_CH; i++) 
                {
                    instruments[i] = 0;
                    for (int j = 0; j < NUM_SCALE; j++) 
                    {
                        startTickTable[i,j] = -1;
                        sustentionStateTable[i, j] = false;
                    }
                }

                Func<int> readVariableLengthValue = () => {
                    // Read a variable-length value (1 - 4 bytes)

                    if (index >= data.Length)
                        throwEofException();

                    int r = data[index++];
                    if ((r & 0x80) == 0x80)
                    {
                        r &= 0x7F;
                        int tmp;

                        do
                        {
                            if (index >= data.Length)
                                throwEofException();
                            tmp = data[index++];
                            r <<= 7;
                            r |= (tmp & 0x7F);
                        } while ((tmp & 0x80) == 0x80);
                    }
                    return r;
                };

                Func<bool> parseEvent = null;
                int velocity = 100;

                bool pedalSastaining = false;

                parseEvent = () =>
                {

                    if (ev >= 0x80 && ev <= 0xEF)
                    {
                        int command = ev & 0xF0;
                        int ch = ev & 0x0F;
                        int param1 = data[index++];
                        int param1Idx = index - 1;  // the index of param1
                        int param2 = -1;
                        if (command != 0xC0 && command != 0xD0)
                            param2 = data[index++];

                        Action checkParam = () => { if (param1 >= 0x80) throw new Exception("Data value out-of-range at the byte " + param1Idx.ToString() + " of track " + this.TrackNumber.ToString()); };

                        if (command == 0x80 || (command == 0x90 && param2 == 0))
                        {
                            // Note Off
                            checkParam();
                            int startTick = startTickTable[ch, param1];
                            if (startTick >= 0 && !(reflectPedalSustantion && sustentionStateTable[ch,param1]))
                            {
                                notes.Add(allocNote(startTick, param1, tick - startTick, velocity, instruments[ch], ch));
#if DEBUG
                                System.Console.WriteLine("Note Added 1:" + tick + ", " + ch + ", " + param1);
#endif
                                startTickTable[ch, param1] = -1;
                                sustentionStateTable[ch, param1] = false;
                            }
#if DEBUG
                            if ((reflectPedalSustantion && sustentionStateTable[ch, param1]))
                            {
                                System.Console.WriteLine("Suspending: " + tick + ", " + ch + ", " + param1);
                            }
#endif
                        }
                        else if (command == 0x90)
                        {
                            // Note On

                            if (reflectPedalSustantion && sustentionStateTable[ch, param1])
                            {
                                checkParam();
                                int startTick = startTickTable[ch, param1];
                                if (startTick >= 0)
                                {
                                    notes.Add(allocNote(startTick, param1, tick - startTick, velocity, instruments[ch], ch));
#if DEBUG
                                    System.Console.WriteLine("Note Added 2:" + tick + ", " + ch + ", " + param1);
#endif
                                }
                            }
                            startTickTable[ch, param1] = tick;
                            sustentionStateTable[ch, param1] = pedalSastaining;
                            velocity = Math.Min(127, Math.Max(param2, 0));
                        }
                        else if (command == 0xB0)
                        {
                            // Control change
                            if (param1 == 120 || param1 == 123)
                            {
                                // All sounds/notes off
                                for (int chnl = 0; chnl < MAX_MIDI_CH; chnl++)
                                {
                                    for (int scale = 0; scale < NUM_SCALE; scale++)
                                    {
                                        int startTick = startTickTable[chnl, scale];
                                        if (startTick >= 0)
                                        {
                                            notes.Add(allocNote(startTick, scale, tick - startTick, velocity, instruments[chnl], chnl));
#if DEBUG
                                            System.Console.WriteLine("Note Added 3:" + tick + ", " + chnl + ", " + scale);
#endif
                                            startTickTable[chnl, scale] = -1;
                                            sustentionStateTable[chnl, scale] = false;
                                        }
                                    }
                                }
                            }
                            else if (param1 == 0x07)
                            {
                                // Volume
                                notes.Add(new VolumeNote(this, tick, param2, ch));

                            }
                            else if (param1 == 64)
                            {
                                // Sustain pedal
                                bool prevState = pedalSastaining;
                                pedalSastaining = (param2 >= 64);
                                if (prevState == false && pedalSastaining == true)
                                {
                                    // Mark currently ringing notes as sustained
                                    for (int chnl = 0; chnl < MAX_MIDI_CH; chnl++)
                                    {
                                        for (int scale = 0; scale < NUM_SCALE; scale++)
                                        {
                                            int startTick = startTickTable[chnl, scale];
                                            if (startTick >= 0)
                                            {
#if DEBUG
                                                System.Console.WriteLine("Suspend:" + tick + ", " + chnl + ", " + scale);
#endif
                                                sustentionStateTable[chnl, scale] = true;
                                            }
                                        }
                                    }
                                }
                                if (prevState == true && pedalSastaining == false)
                                {
                                    // All pending notes off
                                    for (int chnl = 0; chnl < MAX_MIDI_CH; chnl++)
                                    {
                                        for (int scale = 0; scale < NUM_SCALE; scale++)
                                        {
                                            if (sustentionStateTable[chnl, scale])
                                            {
                                                int startTick = startTickTable[chnl, scale];
                                                if (startTick >= 0)
                                                {
                                                    notes.Add(allocNote(startTick, scale, tick - startTick, velocity, instruments[chnl], chnl));
#if DEBUG
                                                    System.Console.WriteLine("Note Added 4:" + tick + ", " + chnl + ", " + scale);
#endif
                                                    startTickTable[chnl, scale] = -1;
                                                    sustentionStateTable[chnl, scale] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        else if (command == 0xC0)
                        {
                            // Program change
                            instruments[ch] = param1;
                            if (this.representativeInstrument < 0)
                            {
                                this.representativeInstrument = instruments[ch];
                            }
                        }
                    }
                    else if (ev == 0xFF)
                    {
                        // Meta events
                        int type = data[index++];
                        if (type == 0x2F)
                        {
                            // Reached to the end of the track
                            return true;
                        }

                        if (type == 0x51)
                        {
                            // Tempo change
                            int len = readVariableLengthValue();
                            Debug.Assert(len == 3);
                            int t1 = data[index++];
                            int t2 = data[index++];
                            int t3 = data[index++];
                            int tempo = (t1 << 16) + (t2 << 8) + t3;
//                            qnt = parent.getQuaterNotesTick(tempo);
                            for (int i = 0; i < MAX_MIDI_CH; i++)
                            {
                                notes.Add(new TempoChangeNote(this, tick, tempo, i));
                            }
                        }
                        else if (type == 0x03) 
                        {
                            // Title/Track name
                            int len = readVariableLengthValue();
                            byte[] splice = new byte[len];
                            Array.Copy(data, index, splice, 0, len);
                            this.title = Encoding.Default.GetString(splice);
                            index += len;
                        }
                        else if (type == 0x04) 
                        {
                            // Instrument name
                            int len = readVariableLengthValue();
                            byte[] splice = new byte[len];
                            Array.Copy(data, index, splice, 0, len);
                            this.instrumentName = Encoding.Default.GetString(splice);
                            index += len;
                        }
                        else
                        {
                            // Just skip the meta-data
                            int len = readVariableLengthValue();
                            index += len;
                        }
                    }
                    else if (ev == 0xF0)
                    {
                        int len = readVariableLengthValue();
                        index += len;
                    }
                    else if (ev == 0xF7)
                    {
                        if ((data[index] & 0x80) == 0x80)
                        {
                            // an escaped event
                            ev = data[index++];
                            return parseEvent();
                        }
                        else
                        {
                            int len = readVariableLengthValue();
                            index += len;
                        }

                    }
                    else if (ev == 0xF2)
                    {
                        index += 2;
                    }
                    else if (ev >= 0xF4)
                    {
                        //  Messages between 0xF4 and 0xF6 contains no data, just skip
                    }
                    else if (ev == 0xF1 || ev == 0xF3)
                    {
                        index++;
                    }
                    else if (ev >= 0xF8 && ev <= 0xFE)
                    {
                        // So-called "System Realtime Messages", no data
                    }
                    else
                    {
                        throw new Exception("Invalid message " + ev.ToString("X") + " found at the offset " + index.ToString() + " of the track " + this.TrackNumber.ToString());
                    }

                    return false;
                };

                while (index < data.Length)
                {
                    try
                    {
                        tick += readVariableLengthValue();

                        if ((data[index] & 0x80) == 0x80)
                        {
                            // status changed
                            ev = data[index++];
                        }

                        if(parseEvent()) break;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new Exception("Unexpected End-of-Track at track " + this.TrackNumber.ToString());
                    }
                }

                // All pending sounds/notes off
                for (int chnl = 0; chnl < MAX_MIDI_CH; chnl++)
                {
                    for (int scale = 0; scale < NUM_SCALE; scale++)
                    {
                        int startTick = startTickTable[chnl, scale];
                        if (startTick >= 0)
                        {
                            notes.Add(allocNote(startTick, scale, tick - startTick, velocity, instruments[chnl], chnl));
                            startTickTable[chnl, scale] = -1;
                            sustentionStateTable[chnl, scale] = false;
                        }
                    }
                }
            }

            private static void quantizeLength(Note t, int qnt)
            {
                decimal[] note_ratios;
                string[] codes;
                if (MidiSequence.quantizationMode == QuantizationMode.ScoreLike)
                {
                    note_ratios  = new decimal[]{
                                                 0.0625m, 0.0833333m, 0.09375m, 0.1m, 0.125m, 0.16667m, 0.1875m, 0.2m, 0.25m, 0.33333m, 0.375m, 0.4m, 0.5m, 0.666666m, 0.75m, 0.8m, 1.0m, 1.33333m, 1.5m, 1.6m, 2.0m, 2.66667m, 3.0m, 3.2m, 4.0m
                                             };
                    codes = new string[]{
                                                 "/16", "/12", "3/32", "/10", "/8", "/6", "3/16", "/5", "/4", "/3", "3/8", "2/5", "/2", "2/3", "3/4", "4/5", "", "4/3", "3/2", "8/5", "2", "8/3", "3", "16/5", "4"
                                             };
                }
                else if (MidiSequence.quantizationMode == QuantizationMode.No5th)
                {
                    note_ratios = new decimal[]{
                                                 0.0625m, 0.0833333m, 0.09375m, 0.125m, 0.16667m, 0.1875m, 0.25m, 0.33333m, 0.375m, 0.5m, 0.666666m, 0.75m, 1.0m, 1.33333m, 1.5m, 2.0m, 2.66667m, 3.0m, 4.0m
                                             };
                    codes = new string[]{
                                                 "/16", "/12", "3/32", "/8", "/6", "3/16", "/4", "/3", "3/8", "/2", "2/3", "3/4", "", "4/3", "3/2", "2", "8/3", "3", "4"
                                             };
                }
                else if (MidiSequence.quantizationMode == QuantizationMode.No3rd5th)
                {
                    note_ratios = new decimal[]{
                                                 0.0625m, 0.09375m, 0.125m, 0.1875m, 0.25m, 0.375m, 0.5m, 0.75m, 1.0m, 1.5m, 2.0m, 3.0m, 4.0m
                                             };
                    codes = new string[]{
                                                 "/16", "3/32", "/8", "3/16", "/4", "3/8", "/2", "3/4", "", "3/2", "2", "3", "4"
                                             };
                }
                else if (MidiSequence.quantizationMode == QuantizationMode.NoDotBelow16th)
                {
                    note_ratios = new decimal[]{
                                                 0.0625m, 0.125m, 0.25m, 0.375m, 0.5m, 0.75m, 1.0m, 1.5m, 2.0m, 3.0m, 4.0m
                                             };
                    codes = new string[]{
                                                 "/16",  "/8",  "/4", "3/8", "/2", "3/4", "", "3/2", "2", "3", "4"
                                             };
                }
                else if (MidiSequence.quantizationMode == QuantizationMode.NoDotBelow8th)
                {
                    note_ratios = new decimal[]{
                                                 0.0625m, 0.125m, 0.25m, 0.5m, 0.75m, 1.0m, 1.5m, 2.0m, 3.0m, 4.0m
                                             };
                    codes = new string[]{
                                                 "/16",  "/8",  "/4", "/2", "3/4", "", "3/2", "2", "3", "4"
                                             };
                }
                else if (MidiSequence.quantizationMode == QuantizationMode.QuantumStep)
                {
                    decimal minStep = MidiSequence.quantizationDivisor <= 0 ? 1.0m : (decimal)qnt / (decimal)MidiSequence.quantizationDivisor;
                    t.QuantizedLength = (int)(Math.Round((decimal)t.Length / minStep) * minStep);
                    if (t.QuantizedLength == 0 && MidiSequence.shortNoteAdjustment == ShortNoteAdjustment.Adjust)
                    {
                        t.QuantizedLength = (int)Math.Round((decimal)qnt / 16.0m);
                    }
                    t.clearLenCode();
                    return;
                }
                else
                {
                    t.QuantizedLength = t.Length;
                    t.clearLenCode();
                    return;
                }

                decimal r = (decimal)t.Length / (decimal)qnt;
                if (r > note_ratios[note_ratios.Length - 1])
                {
                    t.QuantizedLength = (int)(note_ratios[note_ratios.Length - 1]*qnt);
                    return;
//                    throw new Exception("Length violation:" + t.ToString());
                }
                decimal nr = note_ratios[0];
                decimal mindiff = 100000.0m;
                for (int i = 0; i < note_ratios.Length; i++)
                {
                    decimal d = Math.Abs(note_ratios[i] - r);
                    if (d < mindiff)
                    {
                        mindiff = d;
                        nr = note_ratios[i];
                        t.forceLenCode(codes[i]);
                    }
                }
                t.QuantizedLength = (int)(nr * qnt);
            }

            private void quantize()
            {
                int tempo = 60 * 1000 * 1000 / 100; // Defaults to 100 BPM. parent.properties.SeqType == Ppqn の場合 (ほとんどすべての場合) 意味がないので適当

                int prevStartTick = -1;
                int quantizedPrevTick = -1;
                int quantum = 1;
                int threshold = 1;

                int qnt = 0;
                Action tempoChanged = () => 
                {
                    qnt = parent.getQuaterNotesTick(tempo);
                    quantum = MidiSequence.quantizationDivisor <= 0 ? 1 : qnt / MidiSequence.quantizationDivisor;
                    threshold = fusionThreshold <= 0 ? 1 : qnt / fusionThreshold;
                };
                tempoChanged();
                Action<Note> doit = (t) =>
                {
                    int s = t.StartTick;
                    int diff = s % quantum;
                    s -= diff;
                    if (diff*2 >= quantum)
                        s += quantum;
                    t.quantize(s);
                    prevStartTick = t.StartTick;
                    quantizedPrevTick = s;
                };

                notes.Sort((n1, n2) => n1.StartTick.CompareTo(n2.StartTick));

                foreach (Note n in notes)
                {
                    if (n is TempoChangeNote) 
                    {
                        tempo = ((TempoChangeNote)n).Tempo;
                        tempoChanged();
                    }
                    if (prevStartTick >= 0)
                    {
                        int diff = n.StartTick - prevStartTick;
                        if (diff <= threshold)
                        {
                            // 直前の音と同時に鳴らす
                            n.quantize(quantizedPrevTick);
                        }
                        else
                        {
                            doit(n);
                        }
                    }
                    else
                    {
                        doit(n);
                    }
                    addTableEntry(n, qnt * 4);
                }

                int minRest = MidiSequence.restRemovalThreshold <= 0 ? 1 : qnt / MidiSequence.restRemovalThreshold; // 休符の最小単位の tick count

                // 各音符の長さをクォンタイズする
                // 「短い休符は直前の音符に吸収」モードの処理はここで行う
                int[] keys = soundTable.Keys.ToArray();
                Array.Sort(keys);

                for (int i = 0; i < keys.Length; i++)
                {
                    int startTick = keys[i];
                    List<Note> v = soundTable[startTick];
                    Debug.Assert(v.Count > 0);

                    if (MidiSequence.restRemovalMode == MidiSequence.RestRemovalMode.AbsorbToLeadingNote && i < keys.Length - 1)
                    {
                        int nextTick = keys[i + 1];

                        // 1. リスト中の音符のうち、
                        //       終了時刻が次の音の開始時刻を越えない」物
                        //    を取り出す
                        // 2. 1. の音符を終了時刻の昇順で並び替える
                        List<Note> a = v.Where((t) => startTick + t.QuantizedLength <= nextTick).ToList();
                        a.Sort((t1, t2) => t1.QuantizedLength - t2.QuantizedLength);

                        // 3. "次の音符の開始時刻 - リストの最後の要素の終了時刻" が休符の長さ
                        //    この長さが閾値 minRest より小さいなら、リスト中の各音符の長さに加算する
                        int rest = a.Count == 0 ? 0 : nextTick - (startTick + a.ElementAt(a.Count - 1).QuantizedLength);
                        Note restNote = new RestNote(this, nextTick, rest, 0);
                        quantizeLength(restNote, qnt);
                        if (restNote.QuantizedLength < minRest)
                        {
                            v.ForEach((t) =>
                            {
                                if (a.Contains(t))
                                {
                                    t.Length += restNote.Length;
                                    t.QuantizedLength += restNote.QuantizedLength;
                                }
                            });
                        }
                    }

                    v.ForEach((t) => quantizeLength(t, qnt));
                }
            }

            public static int getBpm(int tempo)
            {
                if (tempo == 0) return 0;
                return 60000000 / tempo;
            }
            public override string ToString()
            {
                return
                    "Track No.: " + this.TrackNumber.ToString() + Environment.NewLine +
                    "Length: " + this.Length.ToString() + Environment.NewLine +
                    //"Tempo: " + this.Tempo.ToString() + "(BPM:" + getBpm().ToString() + ")" + Environment.NewLine +
                    "--Begin Sound Events--" + Environment.NewLine +
                    dumpTable() +
                    "--End Sound Events--";
            }

            public string dumpTable()
            {
                if (!isUsable())
                    return "";
                int qnt = parent.getQuaterNotesTick(60 * 1000 * 1000 / 100);
                int[] keys = soundTable.Keys.ToArray();
                Array.Sort(keys);
                StringBuilder sb = new StringBuilder();
                foreach (int k in keys)
                {
                    List<Note> l = soundTable[k];
                    foreach (Note t in l)
                    {
                        if (t is ScaleNote)
                        {
                            sb.Append(k.ToString() + "(" + t.StartTick + ") " + ((ScaleNote)t).purge(qnt) + " " + t.Length.ToString() + Environment.NewLine);
                        }
                        else if (t is TempoChangeNote)
                        {
                            sb.Append(k.ToString() + "(" + t.StartTick + ") Tempo Change: " + ((TempoChangeNote)t).Tempo.ToString() + Environment.NewLine);
                        }
                    }
                }
                return sb.ToString();
            }

            private class NoteInterval : Interval<int>
            {
                List<Note> notes;

                public List<Note> Notes
                {
                    get { return this.notes; }
                }
                public NoteInterval(int s, List<Note> ns, int len) : base(s, s+len)
                {
                    notes = ns;
                }
            }

            public List<MidiTrack> partition()
            {
                recalc();
                List<MidiTrack> ret = new List<MidiTrack>();
                List<List<Note>> notesl = soundTable.Values.Select((l) => l.Where((n) => !(n is TempoChangeNote)).ToList()).Where((l) => l.Count > 0).ToList();
                List<Note> tempoNotes = soundTable.Values.Select((l) => l.Where((n) => n is TempoChangeNote).ToList()).SelectMany((x) => x).ToList();
                NoteInterval[] intervals = new NoteInterval[notesl.Count];

                int i = 0;
                foreach (List<Note> notes in notesl)
                {
                    Note longest = null;
                    foreach (Note n in notes)
                    {
                        if (longest == null || longest.QuantizedLength < n.QuantizedLength)
                        {
                            longest = n;
                        }
                    }

                    intervals[i] = new NoteInterval(longest.QuantizedStartTick, notes, longest.QuantizedLength);
                    i++;
                }

                int k = Interval.MaximalStableSet.partition(intervals);
                IEnumerable<IGrouping<int, NoteInterval>> groups = intervals.GroupBy((iv) => iv.Channel);
                foreach (IGrouping<int, NoteInterval> group in groups)
                {
                    List<Note> notes = new List<Note>();
                    foreach (NoteInterval niv in group)
                    {
                        notes.AddRange(niv.Notes);
                    }
                    notes.AddRange(tempoNotes);
                    MidiTrack tr = new MidiTrack(TrackNumber, notes, this.parent);
                    tr.Title = this.Title;
                    tr.Instrument = this.Instrument;
                    tr.InstrumentName = this.InstrumentName;
                    ret.Add(tr);
                }
                return ret;
            }
            internal virtual string toMoEAbc()
            {
                recalc();
//                int bpm = getBpm(tempo);
                int baseNote = 4; // 基準音符長： 4 => 4分音符, 8 => 8分音符...

                bool optimizews = MidiSequence.optimizeWs;

                if (soundTable.Count == 0)
                    return "";

                if (!isUsable())
                    return "";

                // BPMが途中で変化する曲があるので、このオプションは廃止 (もともと効果が薄かったし)
                //if (MidiSequence.optimizeLen)
                //{
                //    // 出現する音符・休符長の最頻値を求め、その長さを基準にテンポを再調整する。
                //    Dictionary<string, int> hist = new Dictionary<string, int>();
                //    Action<string> inc = (s) =>
                //    {
                //        if (s == null)
                //            return;
                //        int curr;
                //        if (hist.TryGetValue(s, out curr))
                //        {
                //            hist[s] = curr + 1;
                //        }
                //        else
                //        {
                //            hist[s] = 1;
                //        }
                //    };
                //    soundTable.Values.ToList().ForEach((l) => l.ForEach((t) => inc(t.getLenCode(qnt))));
                //    int maxval = hist.Values.Max();

                //    string maxlen = hist.Where((kv) => kv.Value == maxval).First().Key;
                //    string[] dd = maxlen.Split(new char[] { '/' }, 2);

                //    Action<int, int> doAdjust = (n, d) =>
                //    {
                //        // n/d 分音符 を 4分音符として BPM と quarterNoteTick (4分音符相当のTick数) を再計算する
                //        // ただし、再計算後の値が整数にならない場合は最適化をあきらめる
                //        int newBpm = bpm * d;
                //        if (newBpm % n != 0)
                //            return;
                //        newBpm /= n;
                //        int newQnt = qnt * n;
                //        if (newQnt % d != 0)
                //            return;

                //        // BPM値は255が最大なので、それを超える場合は基準音符長を変更してテンポ調整
                //        // 基準音符長でも調整できないならあきらめる
                //        int newBaseNote = baseNote;
                //        while (newBpm > 255)
                //        {
                //            if (newBpm % 2 != 0)
                //                return;
                //            newBpm /= 2;
                //            newBaseNote *= 2;
                //            if (newBaseNote > 64)
                //                return;
                //        }
                //        qnt = newQnt / d;
                //        bpm = newBpm;
                //        baseNote = newBaseNote;
                //        recalc(qnt);
                //        return;
                //    };
                //    if (dd.Length == 1)
                //    {
                //        try
                //        {
                //            int d = int.Parse(dd[0]);
                //            doAdjust(d, 1);
                //        }
                //        catch (Exception)
                //        {
                //            // just do nothing
                //        }
                //    }
                //    else if (dd.Length == 2 && dd[0].Length == 0)
                //    {
                //        try
                //        {
                //            int d = int.Parse(dd[1]);
                //            doAdjust(1, d);
                //        }
                //        catch (Exception)
                //        {
                //            // just do nothing
                //        }
                //    }
                //    else if (dd.Length == 2)
                //    {
                //        try
                //        {
                //            int d1 = int.Parse(dd[0]);
                //            int d2 = int.Parse(dd[1]);
                //            doAdjust(d1, d2);
                //        }
                //        catch (Exception)
                //        {
                //            // just do nothing
                //        }
                //    }
                //}

                //}
                int c = 0;
                foreach (Note n in notes.Where((n) => n is ScaleNote))
                {
                    c = n.Channel;
                    break;
                }
                this.moeABCPart = new MoEABCScore.MoEABCPart(this.Instrument, this.InstrumentName, c);
                if (partitionByScale())
                {
                    Func<Note, bool> isHigh = (n) =>
                    {
                        if (n is ScaleNote)
                        {
                            return (((ScaleNote)n).Scale >= 60);
                        }
                        return true;
                    };
                    Func<Note, bool> isLow = (n) =>
                    {
                        if (n is ScaleNote)
                        {
                            return (((ScaleNote)n).Scale < 60);
                        }
                        return true;
                    };
                    Dictionary<int, List<Note>> high = new Dictionary<int,List<Note>>(), low = new Dictionary<int,List<Note>>();
                    foreach (KeyValuePair<int, List<Note>> kv in soundTable)
                    {
                        List<Note> h = kv.Value.Where(isHigh).ToList();
                        List<Note> l = kv.Value.Where(isLow).ToList();
                        if (h.Count > 0)
                        {
                            high[kv.Key] = h;
                        }
                        if (l.Count > 0)
                        {
                            low[kv.Key] = l;
                        }
                    }
                    MoEABCScore.MoEABCTrack th = new MoEABCScore.MoEABCTrack();
                    MoEABCScore.MoEABCTrack tl = new MoEABCScore.MoEABCTrack();
                    string ret = MidiTrack.toMoEAbc(this, high, baseNote, parent,  th) + MidiTrack.toMoEAbc(this, low, baseNote, parent, tl);
                    if (Encoding.GetEncoding("shift_jis").GetByteCount(ret) <= 20000)
                    {
                        parent.Score.addPart(moeABCPart);
                        moeABCPart.addTrack(th);
                        moeABCPart.addTrack(tl);
                        return ret;
                    }
                }
                MoEABCScore.MoEABCTrack tr = new MoEABCScore.MoEABCTrack();
                moeABCPart.addTrack(tr);
                parent.Score.addPart(moeABCPart);
                return MidiTrack.toMoEAbc(this, soundTable, baseNote, parent,  tr);

            }

            internal Func<bool> partitionByScale = () => { return true; };

            // 音程をもたないトラックははじく
            public bool isUsable()
            {
                return MidiTrack.isUsable(this.notes);
            }
            private static bool isUsable(IEnumerable<Note> notes)
            {
                int sum = notes.Where((t) => !(t is TempoChangeNote) && !(t is VolumeNote)).Select((t) => t.QuantizedLength).Sum();
                if (sum == 0)
                    return false;
                return true;
            }
            protected static string toMoEAbc(MidiTrack track, Dictionary<int,List<MidiSequence.Note>> table, int baseNote, MidiSequence parent, MoEABCScore.MoEABCTrack moeAbcTrack)
            {
                if (!isUsable( table.Values.SelectMany((x) => x) ) )
                {
                    return "";
                }
                int tempo = 60 * 1000 * 1000 / 100; // Defaults to 100 BPM. parent.properties.SeqType == Ppqn の場合 (ほとんどすべての場合) 意味がないので適当
                int qnt = parent.getQuaterNotesTick(tempo);
                int minRest = MidiSequence.restRemovalThreshold <= 0 ? 1 : qnt / MidiSequence.restRemovalThreshold; // 休符の最小単位の tick count
                int maxRest = qnt * 4;  // 全音符相当の tick count

                table = filterTable(table, qnt);

                StringBuilder sb = new StringBuilder();
                sb.Append('{');
                if (!MidiSequence.optimizeWs)
                    sb.Append(Environment.NewLine);

                sb.Append("<L:1/");
                sb.Append(baseNote.ToString());
                sb.Append('>');
                moeAbcTrack.addNote(new MoEABCScore.MoEABCBaseSpecifierNote(1, baseNote));
                if (!MidiSequence.optimizeWs)
                    sb.Append(Environment.NewLine);
                //sb.Append("<Q:");
                //sb.Append(bpm.ToString());
                //sb.Append('>');
                if (!MidiSequence.optimizeWs)
                    sb.Append(Environment.NewLine);

                Action<int> purgeRest = (z) =>
                {
                    // 休符を入れる。ただし、音の間隔が最小単位より短い時は無視する
                    if (z >= minRest)
                    {
                        int rem = z;
                        while (rem > maxRest)
                        {
                            sb.Append("z4");
                            moeAbcTrack.addNote(new MoEABCScore.MoEABCRestNote(4, 1));
                            rem -= maxRest;
                        }

                        if (rem > 0)
                        {
                            Note znote = new RestNote(track, -1, rem, 0);
                            quantizeLength(znote, qnt);

//                            if (MidiSequence.filter != null && !MidiSequence.filter(znote, qnt))
//                                return;
                            string lensym = znote.getLenCode(qnt);
                            if (lensym != null)
                            {
                                Tuple<int, int> p = Note.parseLenCode(lensym);
                                sb.Append('z');
                                sb.Append(lensym);
                                moeAbcTrack.addNote(new MoEABCScore.MoEABCRestNote(p.Item1, p.Item2));
                                if (!MidiSequence.optimizeWs)
                                    sb.Append(' ');
                            }
                        }
                    }
                };

                int lastVelocity = -1;
                Action<int> purgeVelocity = (v) =>
                {
                    if (MidiSequence.velocityKeepingPolicy == VelocityKeepingPolicy.None)
                        return;
                    else if (MidiSequence.velocityKeepingPolicy == VelocityKeepingPolicy.RoundBy10) 
                    {
                        v = (int)(Math.Round((decimal)v / 10.0m) * 10);
                    }
                    else if (MidiSequence.velocityKeepingPolicy == VelocityKeepingPolicy.RoundBy20)
                    {
                        v = (int)(Math.Round((decimal)v / 20.0m) * 20);
                    }
                    else if (MidiSequence.velocityKeepingPolicy == VelocityKeepingPolicy.RoundBy50)
                    {
                        v = (int)(Math.Round((decimal)v / 50.0m) * 50);
                    }
                    else
                    {
                        // v = v;
                    }
                    if (lastVelocity != v)
                    {
                        lastVelocity = v;
                        sb.Append("<v:" + v + ">");
                        moeAbcTrack.addNote(new MoEABCScore.MoEABCVelocityNote(v));
                        if (!MidiSequence.optimizeWs)
                            sb.Append(' ');
                    }
                };
                int[] keys = table.Keys.ToArray();
                if (keys.Length == 0)
                    return "";
                Array.Sort(keys);

                purgeRest(keys[0] - parent.getStartTick());
                for (int i = 0; i < keys.Length; i++)
                {
                    int startTick = keys[i];
                    List<Note> v = table[startTick].Where((t) => t is ScaleNote || t is RestNote).ToList();
//                    Debug.Assert(v.Count > 0);
                    List<Note> vol = table[startTick].Where((t) => t is VolumeNote).ToList();
                    List<Note> tempoChange = table[startTick].Where((t) => t is TempoChangeNote).ToList();
                    if (tempoChange.Count >= 1) {
                        int prevTempo = tempo;
                        // 同じタイミングで2個以上テンポ変化があったら？知らん！
                        tempo = ((TempoChangeNote)tempoChange[0]).Tempo;
                        qnt = parent.getQuaterNotesTick(tempo);
                        if (tempo != prevTempo)
                        {
                            int val = getBpm(tempo);
                            sb.Append("<Q:" + val + ">");
                            moeAbcTrack.addNote(new MoEABCScore.MoEABCTempoNote(val));
                        }
                    }
                    if (vol.Count >= 1)
                    {
                        sb.Append("<V:" + ((VolumeNote)vol[0]).Value + ">");
                    }
                    int rest = 0;
                    string chordLen = null;
                    if (i < keys.Length - 1)
                    {
                        int nextTick = keys[i + 1];

                        // ソロ用に複数トラックをまとめた時などで、和音内に同じ音階が複数存在することがあるので
                        // そういうものは1つにまとめる。音符長は、その音階の最も長いものに合わせる
                        // 例 [C2 E2 G2 C3] => [C2 E2 G2]
                        List<Note> vmerged = new List<Note>();
                        IEnumerable<IGrouping<int, Note>> groupByScale = v.GroupBy((n) => (n is ScaleNote) ? ((ScaleNote)n).Scale : -1);
                        foreach (IGrouping<int, Note> g in groupByScale)
                        {
                            int maxLenOfTheScale = g.Max((n) => n.QuantizedLength);
                            Note nrep = g.ElementAt(0);
                            nrep.QuantizedLength = maxLenOfTheScale;
                            vmerged.Add(nrep);
                        }
                        v = vmerged;
                        if (v.Count > 0)
                        {
                            // リスト中の音符を、次の規則で並び替える
                            // 1. 音符を 
                            //    A:終了時刻が次の音の開始時刻を「越える」物
                            //    B:「越えない」物
                            // に分ける
                            // 2. B の音符を終了時刻の昇順で並び替える
                            // 3. append A B
                            // 「和音の次の音を鳴らすタイミングは、和音の最後に記述された音符長で決まる」という仕様なので、
                            // この並びにすることである程度音の重なりに対応できる、はず
                            List<Note> a = v.Where((t) => startTick + t.QuantizedLength > nextTick).ToList();
                            List<Note> b = v.Where((t) => startTick + t.QuantizedLength <= nextTick).ToList();

                            Comparison<Note> compareByLenThenScale = (n1, n2) =>
                            {
                                int r = n1.QuantizedLength.CompareTo(n2.QuantizedLength);
                                if (r != 0)
                                    return r;
                                if (n1 is ScaleNote && n2 is ScaleNote)
                                {
                                    return ((ScaleNote)n1).Scale.CompareTo(((ScaleNote)n2).Scale);
                                }
                                return 0;

                            };
                            a.Sort(compareByLenThenScale);
                            b.Sort(compareByLenThenScale);

                            // 4. B:終了時刻が次の音の開始時刻を「越えない」物 が存在しない場合、
                            // 和音全体の長さ "[...]n/m" でタイミングをとる
                            if (b.Count == 0)
                            {
                                Note dummy = new DummyNote(track, startTick, nextTick - startTick, 0);
                                quantizeLength(dummy, qnt);
                                chordLen = dummy.getLenCode(qnt);
                                if (chordLen != null && chordLen.Equals(""))
                                    chordLen = "1";
                            }

                            a.InsertRange(a.Count, b);
                            v = table[startTick] = a;

                            rest = nextTick - (startTick + a.ElementAt(a.Count - 1).QuantizedLength);
                        }
                        else
                        {
                            rest = nextTick - startTick;
                        }
                    }

                    if (v.Count > 1 || chordLen != null)
                    {
                        int vel = (int)v.Where((t) => t is ScaleNote).Select((t) => ((ScaleNote)t).Velocity).Average();
                        purgeVelocity(vel);
                        string s = String.Join("", v.Select((t) => t.purge(qnt)));
                        if (!s.Equals(""))
                        {
                            sb.Append('[');
                            sb.Append(s);
                            sb.Append(']');
                            Tuple<int, int> p;
                            if (chordLen != null)
                            {
                                p = Note.parseLenCode(chordLen);
                                sb.Append(chordLen);
                            }
                            else
                            {
                                p = Note.parseLenCode(v[v.Count - 1].getLenCode(qnt));
                            }
                            List<MoEABCScore.MoEABCScaleNote> sns = v.Where((t) => t.purge(qnt).Length > 0).Select((t) => {
                                Tuple<int, int> pc = Note.parseLenCode(t.getLenCode(qnt));
                                ScaleNote sn = (ScaleNote)t;
                                return new MoEABCScore.MoEABCScaleNote(pc.Item1, pc.Item2, sn.Scale, sn.getScaleCode(), sn.Tie);
                            }).ToList();
                            MoEABCScore.MoEABCChordNote cn = new MoEABCScore.MoEABCChordNote(p.Item1, p.Item2, sns);
                            moeAbcTrack.addNote(cn);
                        }
                    }
                    else if (v.Count > 0)
                    {
                        purgeVelocity(((ScaleNote)v[0]).Velocity);
                        string vstr = v[0].purge(qnt);
                        if (vstr.Length > 0)
                        {
                            sb.Append(vstr);
                            Tuple<int, int> p = Note.parseLenCode(v[0].getLenCode(qnt));
                            int s = ((ScaleNote)v[0]).Scale;
                            string sn = ((ScaleNote)v[0]).getScaleCode();
                            moeAbcTrack.addNote(new MoEABCScore.MoEABCScaleNote(p.Item1, p.Item2, s, sn, ((ScaleNote)v[0]).Tie));
                        }
                    }
                    purgeRest(rest);

                    // ここは「和音にタイを入れられない」時代のコード
                    //if (rest <minRest && v.Count == 1)
                    //{
                    //    // タイ処理
                    //    List<Note> tiedNotes = new List<Note>();
                    //    Note firstNote = v.ElementAt(0);
                    //    tiedNotes.Add(firstNote);
                    //    for (int j = i + 1; j < keys.Length; j++)
                    //    {
                    //        int nextTick = keys[j];
                    //        List<Note> v2 = table[nextTick];
                    //        if (v2.Count == 1)
                    //        {
                    //            Note nextNote = v2.ElementAt(0);
                    //            if (nextNote.Scale == firstNote.Scale && (firstNote.QuantizedStartTick + firstNote.QuantizedLength) > nextTick)
                    //            {
                    //                tiedNotes.Add(nextNote);
                    //                i++;
                    //            }
                    //            else
                    //            {
                    //                Note lastNote = tiedNotes.ElementAt(tiedNotes.Count - 1);
                    //                rest = nextTick - (lastNote.StartTick + lastNote.QuantizedLength);
                    //                break;
                    //            }
                    //        }
                    //        else
                    //            break;
                    //    }
                    //    sb.Append(String.Join("-", tiedNotes.Select((t) => t.purge(qnt)).Where((w) => w.Length>0)));
                    //    if (!MidiSequence.optimizeWs)
                    //        sb.Append(' ');
                    //    purgeRest(rest);

                    //    if (!MidiSequence.optimizeWs)
                    //        sb.Append(' ');
                    //}
                    //else
                    //{
                    //    // タイのない通常の譜
                    //    if (v.Count > 1)
                    //    {
                    //        string s = String.Join("", v.Select((t) => t.purge(qnt)));
                    //        if (!s.Equals(""))
                    //        {
                    //            sb.Append('[');
                    //            sb.Append(s);
                    //            sb.Append(']');
                    //        }
                    //    }
                    //    else
                    //    {
                    //        sb.Append(v.ElementAt(0).purge(qnt));
                    //    }

                    //    if (!MidiSequence.optimizeWs)
                    //        sb.Append(' ');

                    //    purgeRest(rest);
                    //}
                    
                }

                if (!MidiSequence.optimizeWs)
                    sb.Append(Environment.NewLine);
                sb.Append('}');
                if (!MidiSequence.optimizeWs)
                    sb.Append(Environment.NewLine);


                return sb.ToString();

            }

            private static Dictionary<int, List<Note>> filterTable(Dictionary<int, List<Note>> table, int qnt)
            {
                if (MidiSequence.filter == null)
                    return table;
                Dictionary<int, List<Note>> newTable = new Dictionary<int, List<Note>>();
                foreach (KeyValuePair<int, List<Note>> pair in table)
                {
                    List<Note> lnew = pair.Value.Where((n) => MidiSequence.filter(n, qnt)).ToList();
                    if (lnew.Count == 0)
                        continue;
                    newTable[pair.Key] = lnew;
                }
                return newTable;
            }

            internal virtual void recalc()
            {
                soundTable.Clear();
                quantize();
            }

            public int getStartTick()
            {
                if (soundTable.Values.Count == 0)
                    return int.MaxValue;
                return soundTable.Keys.Min();

            }

        //    struct MidiEvent
        //    {
        //        public int tick;
        //        public int rawTick;
        //        public int scale;
        //        public bool on;
        //    };
        //    public string dumpToMtx()
        //    {
        //        if (!IsSelected)
        //            return "";
        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("Track_Start");
        //        sb.Append(Environment.NewLine);

        //        List<MidiEvent> list = new List<MidiEvent>();
        //        int[] keys = parent.soundTable.Keys.ToArray();
        //        Array.Sort(keys);
        //        foreach (int k in keys)
        //        {
        //            List<Note> l = parent.soundTable[k];
        //            foreach (Note n in l)
        //            {
        //                MidiEvent e1 = new MidiEvent();
        //                e1.tick = n.QuantizedStartTick;
        //                e1.rawTick = n.StartTick;
        //                e1.scale = n.Scale;
        //                e1.on = true;

        //                MidiEvent e2 = new MidiEvent();
        //                e2.tick = n.QuantizedStartTick + n.QuantizedLength;
        //                e2.rawTick = n.StartTick + n.Length;
        //                e2.scale = n.Scale;
        //                e2.on = false;

        //                list.Add(e1);
        //                list.Add(e2);
        //            }
        //        }

        //        list.Sort((e1, e2) => e1.tick.CompareTo(e2.tick));
        //        foreach (MidiEvent e in list)
        //        {
        //            sb.Append(e.tick.ToString());
        //            sb.Append(' ');
        //            sb.Append(e.on ? "n_on:1 " : "n_off:1 ");
        //            sb.Append(e.scale.ToString());
        //            sb.Append(" 64 % tick before quantization = ");
        //            sb.Append(e.rawTick.ToString());
        //            sb.Append(Environment.NewLine);
        //        }
        //        sb.Append("Track_End");
        //        sb.Append(Environment.NewLine);
        //        return sb.ToString();
        //    }

            internal virtual void merge(MidiTrack track, Func<Note, bool> filter)
            {
                Func<Note, bool> filter_;
                // トラックi が時刻0で独自にテンポ調整を行っているなら、
                // 指揮トラックの時刻0のテンポ調整は除外する
                IEnumerable<Note> atZero = notes.Where((n) => n.StartTick == 0 && n is TempoChangeNote);
                if (atZero.ToList().Count > 0)
                {
                    filter_ = (n) =>
                    {
                        if (n.StartTick == 0 && n is TempoChangeNote) { return false; }
                        return filter(n);
                    };
                } 
                else 
                {
                    filter_ = filter;
                }
                this.notes = this.notes.Concat(track.notes.Where(filter_).Select((n) => n.clone(this, n.StartTick, n.Length))).ToList();

            }
            internal virtual void filter(Func<Note, bool> filter)
            {
                this.notes = this.notes.Where(filter).ToList();

            }
            //public IEnumerable<Note> getNotesAt(int tick)
            //{
            //    try
            //    {
            //        return soundTable[tick];
            //    }
            //    catch (Exception)
            //    {
            //        return new List<Note>();
            //    }
            //}
        }

        internal class TrackPair : MidiTrack 
        {
            private MidiTrack t1, t2;
                internal TrackPair(int n, MidiTrack t1, MidiTrack t2) : base(n, t1.Notes.Concat(t2.Notes).ToList(), t1.Parent)
            {
                this.Instrument = t1.Instrument;
                this.InstrumentName = t1.InstrumentName;
                this.Title = t1.Title;

                this.t1 = t1;
                this.t2 = t2;
                partitionByScale = () => { return false; };
                this.t1.partitionByScale = () => { return false; };
                this.t2.partitionByScale = () => { return false; };
            }
            internal override string toMoEAbc()
            {
                t1.partitionByScale = () => false;
                return t1.toMoEAbc() + t2.toMoEAbc();
            }
            internal override void recalc()
            {
                t1.recalc();
                t2.recalc();
            }
            internal override MoEABCScore.MoEABCPart asMoEABCPart()
            {
                Debug.Assert(this.t1.asMoEABCPart().Tracks.Count <= 1 && this.t2.asMoEABCPart().Tracks.Count <= 1);
                MoEABCScore.MoEABCPart part = new MoEABCScore.MoEABCPart(this.Instrument, this.InstrumentName, this.t1.asMoEABCPart().Channel);
                foreach (MoEABCScore.MoEABCTrack tr in this.t1.asMoEABCPart().Tracks) {
                    part.addTrack(tr);
                }
                foreach (MoEABCScore.MoEABCTrack tr in this.t2.asMoEABCPart().Tracks)
                {
                    part.addTrack(tr);
                }
                return part;
            }

            internal override void filter(Func<Note, bool> filter)
            {
                if (t1 != null)
                    t1.filter(filter);
                if (t2 != null)
                    t2.filter(filter);
            }
            internal override void merge(MidiTrack track, Func<Note, bool> filter)
            {
                if (t1 != null)
                    t1.merge(track, filter);
                if (t2 != null)
                    t2.merge(track, filter);
            }
        }

        private List<MidiTrack> physicalTracks = new List<MidiTrack>();
        List<MidiTrack> tracks = new List<MidiTrack>();
        public List<MidiTrack> Tracks
        {
            get { return tracks; }
        }

        private MoEABCScore.MoEABCScore score = null;
        public MoEABCScore.MoEABCScore Score
        {
            get { return this.score; }
        }
        public int getStartTick()
        {
            return tracks.Min((t) => t.getStartTick());
        }
        private void recalcTracks()
        {
            tracks.Sort((t1, t2) => t1.TrackNumber.CompareTo(t2.TrackNumber));
            // 中身は MidiTrack.recalc() で alloc される
            this.score = new MoEABCScore.MoEABCScore(this.FileName);

            foreach (MidiTrack t in tracks)
            {
                t.recalc();
            }
        }
        public MidiSequence(string fileName)
        {
            this.fileName = fileName;
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
//            int defaultTempo = 0;
            using (fs)
            {
                this.properties = new MidiFileProperties(fs);

                for (int i = 1; i <= this.properties.TrackCount; i++)
                {
                    MidiTrack trk = new MidiTrack(i, fs, this);
            
                    this.physicalTracks.Add(trk);
//                    if (defaultTempo == 0)
//                        defaultTempo = trk.Tempo;
                }
            }

            recalc();
        }

        public void recalc()
        {
            if (this.physicalTracks.Count == 0)
                return;
            this.tracks.Clear();

            List<Note> drumNotes = new List<Note>();
            MidiTrack conductor = null;
            if (this.properties.Format == 0)
            {
                this.tracks.Add(new MidiTrack(this.physicalTracks[0]));
            }
            else
            {
                // Format が 1 か 2 なら、トラック1は指揮トラック
                conductor = this.physicalTracks[0];
                // 他にもトラックがあるなら、それらの各々に指揮トラックのテンポ調整をマージする。
                for (int i = 0; i < this.physicalTracks.Count; i++)
                {
                    MidiTrack track = new MidiTrack(this.physicalTracks[i]);
                    track.merge(conductor, (n) => (n is TempoChangeNote));
                    
                    // チャンネル9番の符をdrumNotes に追加
                    drumNotes.AddRange(track.Notes.Where((n) => n.Channel == 9));
                    track.filter((n) => n.Channel != 9);
                    this.tracks.Add(track);
                }
            }
            // ドラム・パーカッションのトラックを作成
            int tn = this.tracks.Where((tr) => tr.isUsable()).Max((t) => t.TrackNumber);

            MidiTrack drumTrack = null;
            MidiTrack percTrack = null;
            try
            {
//              MidiTrack track10 = this.tracks.Where((t) => t.TrackNumber == 10).First();
//                this.tracks.SelectMany((tr) => tr.Notes)
                IEnumerable<Note> drum = drumNotes.Where((n) =>
                {
                    if (n is ScaleNote)
                    {
                        int s = ((ScaleNote)n).Scale;
                        return (s >= 35 && s <= 59);
                    }
                    return true;
                }).Select((n) =>
                {
                    if (n is ScaleNote)
                    {
                        ScaleNote sn = (ScaleNote)n;
                        return new DrumNote(sn.Parent, sn.StartTick, sn.Scale, sn.Length, sn.Velocity, 200, 9);
                    }
                    return n;
                });
                drumTrack = new MidiTrack(tn + 1, drum.ToList(), this);
                drumTrack.Title = "ドラムセット";
                drumTrack.Instrument = 200;

                IEnumerable<Note> perc = drumNotes.Where((n) =>
                {
                    if (n is ScaleNote)
                    {
                        int s = ((ScaleNote)n).Scale;
                        return (s >= 60 && s <= 81);
                    }
                    return true;
                }).Select((n) =>
                {
                    if (n is ScaleNote)
                    {
                        ScaleNote sn = (ScaleNote)n;
                        return new PercussionNote(sn.Parent, sn.StartTick, sn.Scale, sn.Length, sn.Velocity, 200, 9);
                    }
                    return n;
                });
                percTrack = new MidiTrack(tn + 2, perc.ToList(), this);
                percTrack.Title = "パーカッションセット";
                percTrack.Instrument = 201;

                if (conductor != null)
                {
                    drumTrack.merge(conductor, (n) => (n is TempoChangeNote));
                    percTrack.merge(conductor, (n) => (n is TempoChangeNote));
                }
            }
            catch (Exception)
            {
            }

            if (partDivisionPolicy == PartDivisionPolicy.Session || partDivisionPolicy == PartDivisionPolicy.SessionWithPartitioning)
            {
                // ドラム以外の符を (トラック番号, チャネル, 楽器) で分割
                List<Note> nonDrumNotes = this.tracks.Select((tr) => tr.Notes).SelectMany((n) => n).Where((n) => n.Channel != 9).ToList();
                List<ScaleNote> nonDrumScaleNotes = nonDrumNotes.Where((n) => n is ScaleNote).Select((n) => (ScaleNote)n).ToList();
                IEnumerable<IGrouping<Tuple<int, int, int>, ScaleNote>> partitionByTrackAndInstruments = nonDrumScaleNotes.GroupBy((n) => new Tuple<int, int, int>(n.Parent.TrackNumber, n.Channel, n.Instrument));
                int pn = 1;
                List<MidiTrack> partitoned = new List<MidiTrack>();
                foreach (IGrouping<Tuple<int, int, int>, Note> g in partitionByTrackAndInstruments)
                {
                    MidiTrack tr = new MidiTrack(pn, g.ToList(), this);
                    if (!tr.isUsable())
                        continue;
                    tr.Instrument = g.Key.Item3;
                    List<Note> controlNotes = this.tracks.Where((t) => t.TrackNumber == g.Key.Item1).Select((t) => t.Notes).SelectMany((n) => n).Where((n) => !(n is ScaleNote) && n.Channel == g.Key.Item2).ToList();
                    MidiTrack ctr = new MidiTrack(pn, controlNotes, this);
                    tr.merge(ctr, (n) => true);
                    pn++;
                    partitoned.Add(tr);
                }

                this.tracks = partitoned;
                if (partDivisionPolicy == PartDivisionPolicy.SessionWithPartitioning)
                {
                    int trackNum = 1;
                    List<MidiTrack> partitionedTracks = new List<MidiTrack>();
                    foreach (MidiTrack tr in this.tracks)
                    {
                        List<MidiTrack> partitioned = tr.partition();
                        int i = 0;
                        while (i < partitioned.Count)
                        {
                            MidiTrack t1 = partitioned[i++];
                            if (i < partitioned.Count)
                            {
                                MidiTrack t2 = partitioned[i++];
                                partitionedTracks.Add(new TrackPair(trackNum++, t1, t2));
                            }
                            else
                            {
                                t1.TrackNumber = trackNum++;
                                partitionedTracks.Add(t1);
                            }
                        }
                    }
                    this.tracks = partitionedTracks;
                }
                if (drumTrack != null)
                {
                    drumTrack.TrackNumber = pn++;
                    this.tracks.Add(drumTrack);
                }
                if (percTrack != null)
                {
                    percTrack.TrackNumber = pn;
                    this.tracks.Add(percTrack);
                }
                recalcTracks();
                return;
            }
            if (partDivisionPolicy == PartDivisionPolicy.SoloMelody || partDivisionPolicy == PartDivisionPolicy.Trio)
            {
                // 全てのメロディをマージ
                MidiTrack meloTrack;
                if (this.properties.Format == 0) {
                    meloTrack = this.physicalTracks[0];
                } else {
                    meloTrack = new MidiTrack(1, new List<Note>(), this);
                    foreach (MidiTrack t in this.tracks)
                    {
                        Func<Note, bool> filter = (n) =>
                        {
                            if (n is TempoChangeNote)
                            {
                                return (n.StartTick == 0);
                            }
                            return (n.Channel != 9);
                        };
                        meloTrack.merge(t, filter);
                    }
                    meloTrack.merge(conductor, (n) => (n is TempoChangeNote));
                    
                }
                this.tracks.Clear();
                this.tracks.Add(meloTrack);
                if (partDivisionPolicy == PartDivisionPolicy.Trio)
                {
                    if (drumTrack != null)
                    {
                        drumTrack.TrackNumber = 2;
                        this.tracks.Add(drumTrack);
                    }
                    if (percTrack != null)
                    {
                        percTrack.TrackNumber = 3;
                        this.tracks.Add(percTrack);
                    }
                }
                recalcTracks();
                return;
            }

            if (partDivisionPolicy == PartDivisionPolicy.DrumOnly)
            {
                this.tracks.Clear();
                if (drumTrack != null)
                {
                    drumTrack.TrackNumber = 1;
                    this.tracks.Add(drumTrack);
                }
                recalcTracks();
                return;
            }
            if (partDivisionPolicy == PartDivisionPolicy.PercussionOnly)
            {
                this.tracks.Clear();
                if (percTrack != null)
                {
                    percTrack.TrackNumber = 1;
                    this.tracks.Add(percTrack);
                }
                recalcTracks();
                return;
            }
        }

        // 4分音符に相当するTick数を返す
        // properties.SeqType == Ppqn の場合 (ほとんどこちら)、単に properties.Division を返す
        // properties.SeqType == Smtpe の場合 (めったにない)、4分音符の物理時間 tempo (マイクロ秒) を元に算出する
        public int getQuaterNotesTick(int tempo)
        {
            if (this.properties.SeqType == SequenceType.Ppqn)
                return this.properties.Division;
            return this.properties.Division * tempo / 1000000;
        }
        public override string ToString()
        {
            return
                "File: " + fileName + Environment.NewLine +
                this.properties.ToString() + Environment.NewLine +
                String.Join(Environment.NewLine, tracks.Select(x => x.ToString()));
        }

        //public string toMoEAbc(IEnumerable<MidiTrack> tracks)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    foreach (MidiTrack t in tracks)
        //    {
        //        sb.Append(toMoEAbc(t));
        //    }
        //    return sb.ToString();
        //}

        //public string toMoEAbc(MidiTrack t)
        //{
        //    int tempo = 500000; // 120 bpm by default;
        //    if (t.Tempo == 0)
        //        t.Tempo = tempo;
        //    else
        //        tempo = t.Tempo;
        //    return t.toMoEAbc(t.Parent);
        //}
        //public string toMoEAbc()
        //{
        //    return toMoEAbc(this.tracks);
        //}

    //    public string dumpToMtx()
    //    {
    //        int qnt = 0;
    //        foreach (MidiTrack t in tracks) 
    //        {
    //            int tempo = t.Tempo;
    //            if (tempo > 0) {
    //                qnt = getQuaterNotesTick(tempo);
    //                break;
    //            }
    //        }
    //        if (qnt == 0)
    //            qnt = 480;

    //        StringBuilder sb = new StringBuilder();
    //        sb.Append("HEADER f1 t");
    //        sb.Append(tracks.Count.ToString());
    //        sb.Append(" d");
    //        sb.Append(qnt.ToString());
    //        sb.Append(Environment.NewLine);

    //        foreach (MidiTrack t in tracks)
    //        {
    //            sb.Append(t.dumpToMtx());
    //        }

    //        return sb.ToString();
    //    }
    }
}
