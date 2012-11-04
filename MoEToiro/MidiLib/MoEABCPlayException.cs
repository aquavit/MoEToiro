using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoEABCPlay
{
	/// <summary>
	/// MoEABCPlay の例外クラス
	/// </summary>
	internal class MoEABCPlayException : System.Exception
	{
		/// <summary>  
		/// MMExceptionのインスタンスを作成します。  
		/// </summary>  
		internal MoEABCPlayException () : base ( "予期せぬが発生しました。" ) { }

		/// <summary>  
		/// MMExceptionのインスタンスを作成します。  
		/// </summary>  
		internal MoEABCPlayException ( string message ) : base ( message ) { }

	}
}
