using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Template10.Utils;

namespace TabsAndPivotsDemo.Controls
{
    public class PivotAutoHeightHeader : Pivot
    {
        public PivotAutoHeightHeader() : base()
        {
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var headers = XamlUtil.AllChildren<PivotHeaderItem>(this);
            foreach (var headerItem in headers)
            {
                headerItem.Height = double.NaN;
            }
        }
    }
}
