using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModelChecker.WEB.Util
{
	public static class PageHelper
	{
		public static decimal GetPercDec(int qnt, int allqnt)
		{
			if (allqnt == 0 || qnt == 0)
				return 0;
			var result = ((decimal)qnt / allqnt);
			return result; //< 0.1m ? 0.1m : result;
		}
			

		public static string GetOpacity(int qnt, int allqnt) =>
			GetPercDec(qnt, allqnt).ToString().Replace(",", ".");
	}
}