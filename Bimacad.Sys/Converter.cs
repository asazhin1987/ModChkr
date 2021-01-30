
namespace Bimacad.Sys
{
	public static class Converter
	{
		public const double Fut = 304.8;
		public const double Metre = 3.28;

		public static double ToDouble(string strValue)
		{
			if (!double.TryParse(strValue, out double doubleValue))
				if (!double.TryParse(strValue.Replace('.', ','), out doubleValue))
					if (!double.TryParse(strValue.Replace(',', '.'), out doubleValue))
						return -999;
			return doubleValue;
		}

	}
}
