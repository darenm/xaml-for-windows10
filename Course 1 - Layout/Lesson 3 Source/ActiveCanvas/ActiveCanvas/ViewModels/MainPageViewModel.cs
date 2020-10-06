using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Graphics.Canvas;
using Template10.Mvvm;

namespace ActiveCanvas.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private ImageSource _currentImageSource;
        private string _imageName = "No Name";

        private DelegateCommand _openImageCommand;
        private DelegateCommand _saveImageCommand;

        public string ImageName
        {
            get { return _imageName; }
            set { Set(ref _imageName, value); }
        }

        public ImageSource CurrentImageSource
        {
            get { return _currentImageSource; }
            set { Set(ref _currentImageSource, value); }
        }

        public DelegateCommand OpenImageCommand
        {
            get
            {
                return _openImageCommand ?? (_openImageCommand = new DelegateCommand(
                    async () =>
                    {
                        var openPicker = new FileOpenPicker
                        {
                            ViewMode = PickerViewMode.Thumbnail,
                            SuggestedStartLocation = PickerLocationId.PicturesLibrary
                        };
                        openPicker.FileTypeFilter.Add(".jpg");
                        openPicker.FileTypeFilter.Add(".jpeg");
                        openPicker.FileTypeFilter.Add(".png");
                        var result = await openPicker.PickSingleFileAsync();
                        if (result == null)
                        {
                            return;
                        }

                        using (var fileStream = await result.OpenAsync(FileAccessMode.Read))
                        {
                            // Set the image source to the selected bitmap 
                            var imageSource = new BitmapImage();
                            await imageSource.SetSourceAsync(fileStream);
                            CurrentImageSource = imageSource;
                            ImageName = result.DisplayName;
                            SaveImageCommand.RaiseCanExecuteChanged();
                        }
                    }));
            }
        }

        public DelegateCommand SaveImageCommand
        {
            get
            {
                return _saveImageCommand ?? (_saveImageCommand = new DelegateCommand(
                    async () =>
                    {
                        if (RenderImageCallback == null)
                        {
                            return;
                        }

                        var savePicker = new FileSavePicker
                        {
                            SuggestedStartLocation = PickerLocationId.PicturesLibrary
                        };
                        savePicker.FileTypeChoices.Add("Images", new[] {".png"});
                        var result = await savePicker.PickSaveFileAsync();
                        if (result == null)
                        {
                            return;
                        }

                        var renderTarget = await RenderImageCallback();

                        using (var stream = await result.OpenAsync(FileAccessMode.ReadWrite))
                        {
                            await renderTarget.SaveAsync(stream, CanvasBitmapFileFormat.Png, 1f);
                        }
                    },
                    () => CurrentImageSource != null));
            }
        }

        public Func<Task<CanvasRenderTarget>> RenderImageCallback { get; set; }
    }
}