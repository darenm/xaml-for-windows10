using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TabsAndPivotsDemo.Controls
{
    public sealed partial class TabHeader : UserControl
    {
        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register(nameof(Glyph), typeof (string),
            typeof (TabHeader), null);

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(nameof(Label), typeof(string),
            typeof(TabHeader), null);


        public TabHeader()
        {
            InitializeComponent();
            DataContext = this;
        }

        public string Glyph
        {
            get { return GetValue(GlyphProperty) as string; }
            set { SetValue(GlyphProperty, value); }
        }

        public string Label
        {
            get { return GetValue(LabelProperty) as string; }
            set { SetValue(LabelProperty, value); }
        }
    }
}