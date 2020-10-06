using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.DirectX;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using ActiveCanvas.ViewModels;
using Microsoft.Graphics.Canvas;

namespace ActiveCanvas.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            MyInkCanvas.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Pen |
                                                        CoreInputDeviceTypes.Touch;
            Loaded += (sender, args) =>
            {
                ViewModel.RenderImageCallback = SaveImage;
            };
        }

        public async Task<CanvasRenderTarget> SaveImage()
        {
            var device = CanvasDevice.GetSharedDevice();
            var renderTarget = new CanvasRenderTarget(device, (int)ImageFrame.ActualWidth,
                (int)ImageFrame.ActualHeight, 96);
            var bitmap = new RenderTargetBitmap();
            ImageFrame.UpdateLayout();
            await bitmap.RenderAsync(ImageFrame);
            var pixels = (await bitmap.GetPixelsAsync()).ToArray();

            var cBitmap = CanvasBitmap.CreateFromBytes(
                device,
                pixels,
                bitmap.PixelWidth,
                bitmap.PixelHeight,
                DirectXPixelFormat.B8G8R8A8UIntNormalized,
                DisplayInformation.GetForCurrentView().LogicalDpi);

            using (var ds = renderTarget.CreateDrawingSession())
            {
                ds.Clear(Colors.White);
                ds.DrawImage(cBitmap);
                ds.DrawInk(MyInkCanvas.InkPresenter.StrokeContainer.GetStrokes());
            }
            return renderTarget;
        }

        public MainPageViewModel ViewModel => DataContext as MainPageViewModel;
    }
}