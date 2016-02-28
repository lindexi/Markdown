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

        //private void select(int index, int length)
        //{
        //    text.SelectionStart = index;
        //    text.SelectionLength = length;

        //}

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

            //Frame frame = Window.Current.Content as Frame;
            //Panel grid = frame.Content as Panel;
            //foreach (var temp in grid.Children)
            //{

            //}
        }

        private void selectext(object sender, RoutedEventArgs e)
        {
            view.@select = text.SelectionStart;
            view.select_length = text.SelectionLength;
        }

        private void motify_file(object sender, SelectionChangedEventArgs e)
        {
            var file = list_view.SelectedItem as file_storage;
            if (file != null)
            {
                view.open_file(file.file);
            }
        }


    }
}
