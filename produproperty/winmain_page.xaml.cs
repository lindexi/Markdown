using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
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
            _ctrl = false;
            App.Current.Suspending += suspend;
        }

        private async void suspend(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();
            MessageDialog message_dialog = new MessageDialog("当前还在运行，确定退出", "退出");
            message_dialog.Commands.Add(new UICommand("确定", cmd => { }, "退出"));
            message_dialog.Commands.Add(new UICommand("取消", cmd => { }));
            message_dialog.DefaultCommandIndex = 0;
            message_dialog.CancelCommandIndex = 1;
            IUICommand result = await message_dialog.ShowAsync();
            if (result.Id as string == "退出")
            {

            }

            deferral.Complete();
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
            TextBox text_box=sender as TextBox;
            if (text_box == null)
            {
               
            }
            else
            {
                view.@select = text.SelectionStart;
                view.select_length = text.SelectionLength;
            }

            //view.reminder = view.text_line(view.text, text.SelectionStart);
            //try
            //{
            //    view.reminder = view.text[text.SelectionStart].ToString();
            //    if (view.text[text.SelectionStart] == '\n')
            //    {
            //        view.reminder = "\\n";
            //    }
            //}
            //catch
            //{
                
            //}
        }

        private void motify_file(object sender, SelectionChangedEventArgs e)
        {
            file_storage file = list_view.SelectedItem as file_storage;
            if (file != null)
            {
                view.open_file(file);
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

        private void keydown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Control)
            {
                _ctrl = true;
            }
            if (_ctrl)
            {
                switch (e.Key)
                {
                    case VirtualKey.S:view.storage();
                        break;
                    case VirtualKey.B:
                        view.bold_text();
                        break;
                    case VirtualKey.K:
                        view.mt();
                        break;

                    case VirtualKey.Q:
                       
                        break;
                }
            }
        }

        private void keyup(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Control)
            {
                _ctrl = false;
            }

        }

        //private void storage(object sender, RoutedEventArgs e)
        //{
            
        //}
        private bool _ctrl;

        private void textstorage(object sender, TextChangedEventArgs e)
        {
            if (view.text != text.Text.Replace("\r", ""))
            {
                view.text = text.Text.Replace("\r", "");
            }
        }
    }
}
