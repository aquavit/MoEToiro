using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoEABCPlay
{
	/// <summary>  
	/// MMResultの拡張クラスです。  
	/// </summary>  
	internal static class MMResultExtensions
	{
		/// <summary>  
		/// MMResultがNoErrorでない場合にエラーを発生させます。  
		/// </summary>  
		internal static void Throw ( this MMResult result )
		{
			switch ( result )
			{
				case MMResult.NoError:
					return;
				case MMResult.InvalidDeviceID:
					throw new ArgumentOutOfRangeException ();
				case MMResult.Allocated:
					throw new MMAllocatedException ();
				default:
					throw new MMException ();
			}
		}
	}
}
