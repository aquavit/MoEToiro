using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoEABCPlay
{
	/// <summary>  
	/// 値の範囲をチェックする拡張クラスです。  
	/// </summary>  
	internal static class Bounder
	{
		/// <summary>  
		/// オブジェクトがnull参照の時にエラーを発生させます。  
		/// </summary>  
		internal static void CheckNotNull ( this object target )
		{
			if ( target == null )
			{
				throw new ArgumentNullException ();
			}
		}
	}
}
