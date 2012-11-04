using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoEABCPlay
{
	/// <summary>  
	/// winmm.dllの呼び出し時に発生するエラーを表すクラスです。  
	/// </summary>  
	internal class MMException : System.Exception
	{
		/// <summary>  
		/// MMExceptionのインスタンスを作成します。  
		/// </summary>  
		internal MMException () : base ( "マルチメディアエラーが発生しました。" ) { }

		/// <summary>  
		/// MMExceptionのインスタンスを作成します。  
		/// </summary>  
		internal MMException ( string message ) : base ( message ) { }
	}
}
