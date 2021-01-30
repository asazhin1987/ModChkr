using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Bimacad.Styles
{
	public class BoolToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
			((Visibility)value) switch
			{
				Visibility.Visible => true,
				Visibility.Hidden => false,
				Visibility.Collapsed => false,
				_ => false,
			};

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
			(bool)value ? Visibility.Visible : Visibility.Collapsed;
	}

	public class VisibilityToBoolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (bool)value ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
			((Visibility)value) switch
			{
				Visibility.Visible => true,
				Visibility.Hidden => false,
				Visibility.Collapsed => false,
				_ => false,
			};
	}

	public class DataGridDetailsWidthConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
		(double)value - 37;

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return new NotImplementedException();
		}
	}

	public class DataGridDetailsHeightConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
			(double)value * 2.0 / 3.0;

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class ClockTimeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
			DateTime.Now.ToString("HH:mm:ss");

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
