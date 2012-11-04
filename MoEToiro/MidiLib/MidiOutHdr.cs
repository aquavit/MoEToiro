using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MoEABCPlay
{
	/// <summary>  
	/// MIDIHDR構造体のマネージド実装です。  
	/// </summary>  
	[StructLayout ( LayoutKind.Sequential )]
	internal struct MidiHdr
	{
		/// <summary>  
		/// MIDIデータのポインタです。  
		/// </summary>  
		[MarshalAs ( UnmanagedType.SysUInt )]
		internal IntPtr lpData;
		/// <summary>  
		/// バッファのサイズです。  
		/// </summary>  
		[MarshalAs ( UnmanagedType.U4 )]
		internal uint dwBufferLength;
		/// <summary>  
		/// 実際に入力されたデータのサイズです。  
		/// </summary>  
		[MarshalAs ( UnmanagedType.U4 )]
		internal uint dwBytesRecorded;
		/// <summary>  
		/// dwUser値です。  
		/// </summary>  
		[MarshalAs ( UnmanagedType.U4 )]
		internal uint dwUser;
		/// <summary>  
		/// MIDIヘッダーの状態を表します。  
		/// </summary>  
		[MarshalAs ( UnmanagedType.U4 )]
		internal MidiHdrFlag dwFlags;
		/// <summary>  
		/// lpNext値です。  
		/// </summary>  
		[MarshalAs ( UnmanagedType.SysUInt )]
		internal IntPtr lpNext;
		/// <summary>  
		/// reserved値です。  
		/// </summary>  
		[MarshalAs ( UnmanagedType.SysUInt )]
		internal IntPtr reserved;
		/// <summary>  
		/// dwOffset値です。  
		/// </summary>  
		[MarshalAs ( UnmanagedType.U4 )]
		internal uint dwOffset;
		/// <summary>  
		/// dwReserved値です。  
		/// </summary>  
		[MarshalAs ( UnmanagedType.ByValArray, SizeConst = 8 )]
		internal IntPtr [] dwReserved;
	}
}
