using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Bimacad.Styles
{
	public class TitleBar : Control
	{
		public string Title
		{
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		public ImageSource Icon
		{
			get { return (ImageSource)GetValue(IconProperty); }
			set { SetValue(IconProperty, value); }
		}

		public bool MinimizeButton
		{
			get { return (bool)GetValue(MinimizeButtonProperty); }
			set { SetValue(MinimizeButtonProperty, value); }
		}

		Button closeButton;
		Button minimizeButton;

		public TitleBar()
		{
			this.MouseLeftButtonDown += new MouseButtonEventHandler(OnTitleBarLeftButtonDown);
			this.MouseDoubleClick += new MouseButtonEventHandler(TitleBar_MouseDoubleClick);
			this.Loaded += new RoutedEventHandler(TitleBar_Loaded);
		}

		void TitleBar_Loaded(object sender, RoutedEventArgs e)
		{
			closeButton = (Button)this.Template.FindName("CloseButton", this);
			minimizeButton = (Button)this.Template.FindName("MinimizeButton", this);
			minimizeButton.Click += new RoutedEventHandler(MinimizeButton_Click);
			closeButton.Click += new RoutedEventHandler(CloseButton_Click);
		}


		static TitleBar()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(TitleBar), new FrameworkPropertyMetadata(typeof(TitleBar)));
		}

		#region event handlers

		void OnTitleBarLeftButtonDown(object sender, MouseEventArgs e)
		{
			if (this.TemplatedParent is Window window)
			{
				window.DragMove();
			}
		}

		void TitleBar_MouseDoubleClick(object sender, MouseButtonEventArgs e){}

		void CloseButton_Click(object sender, RoutedEventArgs e)
		{
			if (this.TemplatedParent is Window window)
			{
				window.Close();
			}
		}

		void MinimizeButton_Click(object sender, RoutedEventArgs e)
		{
			if (this.TemplatedParent is Window window)
			{
				window.WindowState = WindowState.Minimized;
			}
		}

		#endregion


		#region dependency properties

		public static readonly DependencyProperty TitleProperty =
		   DependencyProperty.Register(
			   "Title", typeof(string), typeof(TitleBar));

		public static readonly DependencyProperty IconProperty =
		   DependencyProperty.Register(
			   "Icon", typeof(ImageSource), typeof(TitleBar));

		public static readonly DependencyProperty MinimizeButtonProperty =
		   DependencyProperty.Register(
			   "MinimizeButton", typeof(bool), typeof(TitleBar));

		#endregion
	}
}
