using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using produproperty.ViewModel;
namespace produproperty
{
    public partial class winmain_page : Page
    {
        private winmain view;
        public winmain_page()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            StorageFolder folder = e.Parameter as StorageFolder;
            view = folder != null ? new winmain(folder) : new winmain(null);
            view.selectchange = text.Select;

            list_view.ItemsSource = view.file_observable_collection;
        }

        private async void talk(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(text.Text))
            {
                return;
            }
            AppBarButton button = sender as AppBarButton;
            if (button == null)
            {
                return;
            }
            button.IsEnabled = false;

            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            SpeechSynthesisStream stream = await synthesizer.SynthesizeTextToStreamAsync(text.Text);
            mediaelement.SetSource(stream, stream.ContentType);
            mediaelement.Play();
            button.IsEnabled = true;
        }

        private void selectext(object sender, RoutedEventArgs e)
        {
            view.@select = text.SelectionStart;
            view.select_length = text.SelectionLength;
        }

        private void motify_file(object sender, SelectionChangedEventArgs e)
        {
            file_storage file = list_view.SelectedItem as file_storage;
            if (file != null)
            {
                view.open_file(file.file);
            }
        }


        private void Grid_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;
            if (e.DragUIOverride != null)
            {
                e.DragUIOverride.Caption = "打开";
            }
            e.Handled = true;
        }
    }
}
