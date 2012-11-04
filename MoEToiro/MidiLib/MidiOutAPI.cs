using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MoEABCPlay
{
	/// <summary>  
	/// MIDI出力APIの宣言です。  
	/// </summary>  
	internal static class MidiOutApi
	{
		/// <summary>  
		/// MIDI出力ポートの数を取得します。  
		/// </summary>  
		[DllImport ( "winmm.dll", EntryPoint = "midiOutGetNumDevs" )]
		[return: MarshalAs ( UnmanagedType.U4 )]
		internal static extern uint midiOutGetNumDevs ();

		/// <summary>  
		/// MIDI出力ポートの情報を取得します。  
		/// </summary>  
		[DllImport ( "winmm.dll", EntryPoint = "midiOutGetDevCapsA", CharSet = CharSet.Ansi )]
		[return: MarshalAs ( UnmanagedType.U4 )]
		internal static extern MMResult midiOutGetDevCapsA ( [MarshalAs ( UnmanagedType.U4 )] uint uDeviceID, ref MidiOutCapsA pMidiOutCaps, [MarshalAs ( UnmanagedType.U4 )] uint cbMidiOutCaps );

		/// <summary>  
		/// MIDI出力ポートを開きます。  
		/// </summary>  
		[DllImport ( "winmm.dll", EntryPoint = "midiOutOpen" )]
		[return: MarshalAs ( UnmanagedType.U4 )]
		internal static extern MMResult midiOutOpen ( [MarshalAs ( UnmanagedType.SysUInt )] ref IntPtr lphMidiOut, [MarshalAs ( UnmanagedType.U4 )] uint uDeviceID, [MarshalAs ( UnmanagedType.FunctionPtr )] Delegate dwCallback, [MarshalAs ( UnmanagedType.U4 )] uint dwInstance, [MarshalAs ( UnmanagedType.U4 )] MidiPortOpenFlag dwFlags );

		/// <summary>  
		/// MIDI出力ポートを閉じます。  
		/// </summary>  
		[DllImport ( "winmm.dll", EntryPoint = "midiOutClose" )]
		[return: MarshalAs ( UnmanagedType.U4 )]
		internal static extern MMResult midiOutClose ( [MarshalAs ( UnmanagedType.SysUInt )] IntPtr hMidiOut );

		/// <summary>
		/// 指定された MIDI 出力デバイスに対するすべての MIDI チャネルの音符（ノート）をすべてオフにします。
		/// </summary>
		[DllImport ( "winmm.dll", EntryPoint = "midiOutReset" )]
		[return: MarshalAs ( UnmanagedType.U4 )]
		internal static extern MMResult midiOutReset ( [MarshalAs ( UnmanagedType.SysUInt )] IntPtr hMidiOut );

		/// <summary>  
		/// MIDIショートメッセージを送信します。  
		/// </summary>  
		[DllImport ( "winmm.dll", EntryPoint = "midiOutShortMsg" )]
		[return: MarshalAs ( UnmanagedType.U4 )]
		internal static extern MMResult midiOutShortMsg ( [MarshalAs ( UnmanagedType.SysUInt )] IntPtr hMidiOut, [MarshalAs ( UnmanagedType.U4 )] uint dwMsg );

		/// <summary>  
		/// MIDIロングメッセージを送信します。  
		/// </summary>  
		[DllImport ( "winmm.dll", EntryPoint = "midiOutLongMsg" )]
		[return: MarshalAs ( UnmanagedType.U4 )]
		internal static extern MMResult midiOutLongMsg ( [MarshalAs ( UnmanagedType.SysUInt )] IntPtr hMidiOut, ref MidiHdr lpMidiOutHdr, [MarshalAs ( UnmanagedType.U4 )] uint uSize );

		/// <summary>  
		/// MIDI出力バッファを登録します。  
		/// </summary>  
		[DllImport ( "winmm.dll", EntryPoint = "midiOutPrepareHeader" )]
		[return: MarshalAs ( UnmanagedType.U4 )]
		internal static extern MMResult midiOutPrepareHeader ( [MarshalAs ( UnmanagedType.SysUInt )] IntPtr hMidiOut, ref MidiHdr lpMidiOutHdr, [MarshalAs ( UnmanagedType.U4 )] uint uSize );

		/// <summary>  
		/// MIDI出力バッファの登録を解除します。  
		/// </summary>  
		[DllImport ( "winmm.dll", EntryPoint = "midiOutUnprepareHeader" )]
		[return: MarshalAs ( UnmanagedType.U4 )]
		internal static extern MMResult midiOutUnprepareHeader ( [MarshalAs ( UnmanagedType.SysUInt )] IntPtr hMidiOut, ref MidiHdr lpMidiOutHdr, [MarshalAs ( UnmanagedType.U4 )] uint uSize );  

	}
}
