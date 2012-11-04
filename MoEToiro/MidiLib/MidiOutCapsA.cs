using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MoEABCPlay
{
	/// <summary>  
	/// MIDI出力ポートの情報を表します。  
	/// </summary>  
	[StructLayout ( LayoutKind.Sequential, CharSet = CharSet.Ansi )]
	internal struct MidiOutCapsA
	{
		/// <summary>  
		/// MIDIハードウェアのメーカーIDです。  
		/// </summary>  
		[MarshalAs ( UnmanagedType.U2 )]
		internal ushort wMid;
		/// <summary>  
		/// Product IDです。  
		/// </summary>  
		[MarshalAs ( UnmanagedType.U2 )]
		internal ushort wPid;
		/// <summary>  
		/// ドライバーのバージョンです。  
		/// </summary>  
		[MarshalAs ( UnmanagedType.U4 )]
		internal uint vDriverVersion;
		/// <summary>  
		/// ポートの名前です。  
		/// </summary>  
		[MarshalAs ( UnmanagedType.ByValTStr, SizeConst = MidiPortConst.MaxPNameLen )]
		internal string szPname;
		/// <summary>  
		/// wTechnology値です。  
		/// </summary>  
		[MarshalAs ( UnmanagedType.U2 )]
		internal MidiModuleType wTechnology;
		/// <summary>  
		/// 最大ボイス数を取得します。  
		/// </summary>  
		[MarshalAs ( UnmanagedType.U2 )]
		internal ushort wVoices;
		/// <summary>  
		/// 最大同時発音数を取得します。  
		/// </summary>  
		[MarshalAs ( UnmanagedType.U2 )]
		internal ushort wNotes;
		/// <summary>  
		/// チャンネルマスクを取得します。  
		/// </summary>  
		[MarshalAs ( UnmanagedType.U2 )]
		internal ushort wChannelMask;
		/// <summary>  
		/// dwSupport値です。  
		/// </summary>  
		[MarshalAs ( UnmanagedType.U4 )]
		internal MidiPortCapability dwSupport;

		/// <summary>  
		/// チャンネルマスクを取得します。  
		/// </summary>  
		internal bool [] GetChannelMask ()
		{
			bool [] mask = new bool [ 16 ];
			for ( int i = 0; i < 16; i++ )
			{
				mask [ i ] = ( wChannelMask & ( 1 << i ) ) != 0;
			}
			return mask;
		}

		/// <summary>  
		/// チャンネルマスクを設定します。  
		/// </summary>  
		internal void SetChannelMask ( byte ch, bool value )
		{
			wChannelMask &= (ushort)( 0xFFFF - 1 << ch );
			wChannelMask |= value ? (ushort)( 1 << ch ) : (ushort)0;
		}

		/// <summary>  
		/// デバイスドライバのメジャーバージョンを取得します。  
		/// </summary>  
		internal byte MajorVersion
		{
			get { return (byte)( vDriverVersion >> 8 ); }
		}

		/// <summary>  
		/// デバイスドライバのマイナーバージョンを取得します。  
		/// </summary>  
		internal byte MinorVersion
		{
			get { return (byte)( vDriverVersion & 0xFF ); }
		}
	}
}
