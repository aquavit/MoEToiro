using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoEABCPlay
{
	/// <summary>  
	/// マルチメディアリソースの割り当てに失敗したことを示すクラスです。  
	/// </summary>  
	internal class MMAllocatedException : MMException
	{
		/// <summary>  
		/// MMAllocatedExceptionのインスタンスを作成します。  
		/// </summary>  
		internal MMAllocatedException () : base ( "指定されたリソースは既に割り当てられています。" ) { }
	}
}
