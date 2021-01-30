using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfStyles
{
    public class ImageButton : System.Windows.Controls.Button
    {
        static ImageButton()
        {
            //This OverrideMetadata call tells the system that this element wants to provide a style that is different than its base class.
            //This style is defined in themes\generic.xaml
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
        }


        #region properties

        public string ImageOver
        {
            get { return (string)GetValue(ImageOverProperty); }
            set { SetValue(ImageOverProperty, value); }
        }

        public string ImageNormal
        {
            get { return (string)GetValue(ImageNormalProperty); }
            set { SetValue(ImageNormalProperty, value); }
        }

        public string ImageDown
        {
            get { return (string)GetValue(ImageDownProperty); }
            set { SetValue(ImageDownProperty, value); }
        }


        #endregion

        #region dependency properties

        public static readonly DependencyProperty ImageNormalProperty =
           DependencyProperty.Register(
               "ImageNormal", typeof(string), typeof(ImageButton));


        public static readonly DependencyProperty ImageOverProperty =
          DependencyProperty.Register(
              "ImageOver", typeof(string), typeof(ImageButton));

        public static readonly DependencyProperty ImageDownProperty =
        DependencyProperty.Register(
            "ImageDown", typeof(string), typeof(ImageButton));

        #endregion

    }
}
