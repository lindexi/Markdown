// lindexi
// 20:47

using produproperty.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace produproperty
{
    /// <summary>
    ///     可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            text.Paste += Text_Paste;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is viewModel)
            {
                view = e.Parameter as viewModel;
                DataContext = view;
            }
            else
            {
                view = new viewModel();
                view.Selectchange = selectchange;
                this.DataContext = view;
            }
        }

        private bool _ctrl;
        private viewModel view;

        private void Text_Paste(object sender, TextControlPasteEventArgs e)
        {
            view.Clipboard(e);
        }

        private void Grid_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
            e.DragUIOverride.Caption = "打开";
            e.Handled = true;
        }

        private void text_SelectionChanged(object sender, RoutedEventArgs e)
        {
            view.Select = text.SelectionStart;
        }

        private void selectchange(int select, int selecti)
        {
            text.SelectionStart = select;
            text.SelectionLength = selecti;
        }

        private void text_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            string str;
            if (e.Key.Equals(VirtualKey.Control))
            {
                _ctrl = true;
            }
            else if ((e.Key == VirtualKey.V) && _ctrl)
            {
            }

            if (_ctrl)
            {
                if (e.Key == VirtualKey.Z)
                {
                }
                else if (e.Key == VirtualKey.K)
                {
                    str = "\n```C#\n\n\n\n```\n";
                    view.Tianjia(str);
                    text.SelectionStart -= 6;
                }
                else if (e.Key == VirtualKey.S)
                {
                    view.Storage();
                }
            }

            //e.Handled = true;
        }

        private void text_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key.Equals(VirtualKey.Control))
            {
                _ctrl = false;
            }
        }

        private void option(object sender, RoutedEventArgs e)
        {
            //view.storage();
            Frame frame = Window.Current.Content as Frame;
            frame.Navigate(typeof(option), view);
        }
    }
}