using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;

namespace MoEABCPlay
{

	/// <summary>  
	/// MIDI-OUTポートを抽象化するクラスです。  
	/// </summary>  
	public sealed class MidiOutPortHandle : IDisposable
	{
		#region プライベート変数

		IntPtr hMidiOut = IntPtr.Zero;
		bool disposed = false;
		string name;
		static int maxBufferSize = 64 * 1024;
		static uint hdrSize = (uint)Marshal.SizeOf ( typeof ( MidiHdr ) );
		
		#endregion プライベート変数


		#region プロパティ

		/// <summary>  
		/// ポートの名前です。  
		/// </summary>  
		internal string Name
		{
			get
			{
				CheckDisposed ();
				return name;
			}
		}

		#endregion プロパティ

		/// <summary>  
		/// コンストラクタ
		/// 指定した番号のポートを作成します。  
		/// </summary>  
		internal MidiOutPortHandle ( int portNum )
		{
			name = GetPortInformation ( portNum ).szPname;
			MidiOutApi.midiOutOpen ( ref hMidiOut, (uint)portNum, null, 0, MidiPortOpenFlag.NoCallback ).Throw ();
		}


		#region デストラクタ

		/// <summary>  
		/// MIDIポートハンドルを解放します。  
		/// </summary>  
		~MidiOutPortHandle ()
		{
			Release ( true );
		}

		/// <summary>  
		/// MIDIポートハンドルを開放します。  
		/// </summary>  
		internal void Close ()
		{
			CheckDisposed ();
			Dispose ();
		}

		/// <summary>  
		/// MIDIポートハンドルを開放します。  
		/// </summary>  
		public void Dispose ()
		{
			Release ( false );
		}

		void Release ( bool finalizing )
		{
			if ( disposed ) return;

			if ( !finalizing )
			{
				GC.SuppressFinalize ( this );
			}
			disposed = true;
			MidiOutApi.midiOutReset ( hMidiOut ).Throw ();
			MidiOutApi.midiOutClose ( hMidiOut ).Throw ();
		}

		void CheckDisposed ()
		{
			if ( disposed )
			{
				throw new ObjectDisposedException ( name );
			}
		} 

		#endregion デストラクタ

		/// <summary>  
		/// MIDI出力ポートの数を取得します。  
		/// </summary>  
		internal static int GetPortCount ()
		{
			return (int)MidiOutApi.midiOutGetNumDevs ();
		}

		/// <summary>  
		/// 指定されたMIDI出力ポートの情報を取得します。  
		/// </summary>  
		internal static MidiOutCapsA GetPortInformation ( int portNum )
		{
			var caps = new MidiOutCapsA ();
			MidiOutApi.midiOutGetDevCapsA ( (uint)portNum, ref caps, (uint)Marshal.SizeOf ( typeof ( MidiOutCapsA ) ) ).Throw ();

			return caps;
		}

		/// <summary>
		/// 実行環境のMIDIポートの情報をすべて取得します
		/// </summary>
		/// <returns></returns>
		internal static List<MidiOutCapsA> GetPortInformations ()
		{
			int portCount = GetPortCount ();
			List<MidiOutCapsA> retValue = new List<MidiOutCapsA> ();

			for ( int i = 0; i < portCount; i++ )
			{
				var caps = new MidiOutCapsA ();
				MidiOutApi.midiOutGetDevCapsA ( (uint)i, ref caps, (uint)Marshal.SizeOf ( typeof ( MidiOutCapsA ) ) ).Throw ();
				retValue.Add ( caps );
			}
			return retValue;
		}

		/// <summary>
		/// 全てのMIDIチャンネルのノートを停止します
		/// </summary>
		public void MidiOutReset ()
		{
			MidiOutApi.midiOutReset ( hMidiOut ).Throw ();
		}

		/// <summary>  
		/// MIDIデータを送信します。  
		/// </summary>  
		internal void Send ( byte [] data )
		{
			CheckDisposed ();
			data.CheckNotNull ();
			if ( data.Length == 0 )
			{
				return;
			}
			if ( data.Length <= 4 )
			{
				SendShortMessage ( data );
			}
			else
			{
				SendLongMessage ( data );
			}
		}

		/// <summary>  
		/// 4バイト以内のMIDIメッセージ(Short Message)を送信します。  
		/// </summary>  
		void SendShortMessage ( byte [] data )
		{
			uint message = 0;

			for ( int i = 0; i < data.Length; i++ )
			{
				message |= ( (uint)data [ i ] ) << ( i * 8 );
			}

			MidiOutApi.midiOutShortMsg ( hMidiOut, message );
		}

		/// <summary>  
		/// 5バイト以上のMIDIメッセージ(Long Message)を送信します。  
		/// </summary>  
		void SendLongMessage ( byte [] data )
		{
			if ( data.Length > maxBufferSize )
			{
				throw new InvalidOperationException ();
			}

			MidiHdr hdr = new MidiHdr ();
			hdr.dwReserved = new IntPtr [ 8 ];

			GCHandle dataHandle = GCHandle.Alloc ( data, GCHandleType.Pinned );

			try
			{
				hdr.lpData = dataHandle.AddrOfPinnedObject ();
				hdr.dwBufferLength = (uint)data.Length;
				hdr.dwFlags = 0;

				SendBuffer ( hdr );
			}
			finally
			{
				dataHandle.Free ();
			}
		}

		void SendBuffer ( MidiHdr hdr )
		{
			MidiOutApi.midiOutPrepareHeader ( hMidiOut, ref hdr, hdrSize ).Throw ();
			while ( ( hdr.dwFlags & MidiHdrFlag.Prepared ) != MidiHdrFlag.Prepared )
			{
				Thread.Sleep ( 1 );
			}

			MidiOutApi.midiOutLongMsg ( hMidiOut, ref hdr, hdrSize ).Throw ();

			while ( ( hdr.dwFlags & MidiHdrFlag.Done ) != MidiHdrFlag.Done )
			{
				Thread.Sleep ( 1 );
			}
			MidiOutApi.midiOutUnprepareHeader ( hMidiOut, ref hdr, hdrSize ).Throw ();
		} 

	}
}
