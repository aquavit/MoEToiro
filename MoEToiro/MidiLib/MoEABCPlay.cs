using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoEABCPlay
{
	/// <summary>
	/// MIDI操作関連クラス
	/// </summary>
	public class MoEABCPlay
	{
		/// <summary>
		/// midi操作ハンドラ
		/// </summary>
		private MidiOutPortHandle handle;


		#region プロパティ

		/// <summary>
		/// MIDIポートの数を返します
		/// </summary>
		public int PortCount
		{
			get
			{
				return
					MidiOutPortHandle.GetPortCount ();
			}
		}

		private Dictionary<int, string> port;

		/// <summary>
		/// ポートの名称リスト
		/// </summary>
		public Dictionary<int,string> Port
		{
			get
			{
				if ( port == null )
				{
					int portNum = 0;
					port = new Dictionary<int, string> ();

					var inf = MidiOutPortHandle.GetPortInformations ();

					foreach ( var v in inf )
					{
						port.Add ( portNum++, v.szPname );
					}
				}
				return port;
			}
		}

		#endregion プロパティ


		/// <summary>
		/// コンストラクタ
		/// MidiOutPortHamdle をインスタンス化
		/// </summary>
		/// <param name="portNum"></param>
		public MoEABCPlay ( )
		{
		}


		/// <summary>
		/// 指定された portNumber でMidiハンドラを初期化
		/// やっておかないと色々できない
		/// </summary>
		/// <param name="portNumber"></param>
		public void InitMidiOut ( int portNumber )
		{
			handle = new MidiOutPortHandle ( portNumber );
		}

		/// <summary>
		/// 全てのMIDIチャンネルのノートを停止します
		/// </summary>
		public void MidiReset ()
		{
			if ( handle != null )
				handle.MidiOutReset ();
		}

		/// <summary>
		/// 必ず閉じましょう
		/// </summary>
		public void Close ()
		{
			if ( handle != null )
				handle.Close ();
		}

		/// <summary>
		/// プログラムチェンジイベントを送信
		/// Channel0 固定
		/// </summary>
		/// <param name="gmPrg"></param>
		public void ProgramChange ( GMProgram gmPrg )
		{
			ProgramChange ( gmPrg, 0 );
		}

		/// <summary>
		/// プログラムチェンジイベントを送信
		/// </summary>
		/// <param name="gmPrg"></param>
		/// <param name="channel">ch番号 0-15 指定</param>
		public void ProgramChange ( GMProgram gmPrg, byte channel )
		{
			if ( channel < 0 && 15 < channel )
			{
				throw new MoEABCPlayException ( "チャンネル数が範囲を超えています[ 0-15 ]" );
			}
			handle.Send ( new byte [] { (byte)( 0xC0 | channel ), (byte)gmPrg } );
		}

		/// <summary>
		/// プログラムチェンジイベントを送信
		/// Channel0 固定
		/// </summary>
		/// <param name="gmPrg"></param>
		public void ProgramChange ( int prgNo )
		{
			ProgramChange ( prgNo, 0 );
		}

		/// <summary>
		/// プログラムチェンジイベントを送信
		/// </summary>
		/// <param name="gmPrg"></param>
		/// <param name="channel">ch番号 0-15 指定</param>
		public void ProgramChange ( int prgNo, byte channel )
		{
			if ( channel < 0 && 15 < channel )
			{
				throw new MoEABCPlayException ( "チャンネル数が範囲を超えています[ 0-15 ]" );
			}
			handle.Send ( new byte [] { (byte)( 0xC0 | channel ), (byte)prgNo } );
		}

		/// <summary>
		/// 発音
		/// </summary>
		/// <param name="scale"></param>
		public void NoteOn ( int scale )
		{
			NoteOn ( scale, 0, 100 );
		}

		/// <summary>
		/// 発音（チャンネル指定）
		/// </summary>
		/// <param name="scale"></param>
		/// <param name="channel"></param>
		public void NoteOn ( int scale, byte channel, byte velocity )
		{
			// チャンネル範囲チェック
			if ( channel < 0 && 15 < channel )
			{
				throw new MoEABCPlayException ( "チャンネル数が範囲を超えています[ 0-15 ]" );
			}

			// 音階範囲チェック（必要？）
			if ( scale < 0 && 127 < scale )
			{
				throw new MoEABCPlayException ( "音階が表現できる範囲を超えています[ 0-127 ]" );
			}

			handle.Send ( new byte [] { (byte)( 0x90 | channel ), (byte)scale, velocity } );
		}

		/// <summary>
		/// 消音
		/// </summary>
		/// <param name="scale"></param>
		public void NoteOff ( int scale )
		{
			NoteOff ( scale, 0 );
		}

		/// <summary>
		/// 消音（チャンネル指定）
		/// </summary>
		/// <param name="scale"></param>
		/// <param name="channel"></param>
		public void NoteOff ( int scale, byte channel )
		{
			// チャンネル範囲チェック
			if ( channel < 0 && 15 < channel )
			{
				throw new MoEABCPlayException ( "チャンネル数が範囲を超えています[ 0-15 ]" );
			}

			// 音階範囲チェック（必要？）
			if ( scale < 0 && 127 < scale )
			{
				throw new MoEABCPlayException ( "音階が表現できる範囲を超えています[ 0-127 ]" );
			}

			handle.Send ( new byte [] { (byte)( 0x90 | channel ), (byte)scale, 0x00 } );
		}

        public void Volume(int vol, byte channel)
        {
            // チャンネル範囲チェック
            if (channel < 0 && 15 < channel)
            {
                throw new MoEABCPlayException("チャンネル数が範囲を超えています[ 0-15 ]");
            }

            // 音階範囲チェック（必要？）
            if (vol < 0 && 127 < vol)
            {
                throw new MoEABCPlayException("音階が表現できる範囲を超えています[ 0-127 ]");
            }

            handle.Send(new byte[] { (byte)(0xB0 | channel), (byte)0x0, (byte)vol });
        }
	}
}
