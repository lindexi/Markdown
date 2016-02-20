using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace produproperty
{
    public partial class note_storage_page : Windows.UI.Xaml.Controls.Page
    {
        ViewModel.note_storage view;
        /// <summary>
        /// 程序打开第一界面
        /// </summary>
        public note_storage_page()
        {
            view = new ViewModel.note_storage();
            InitializeComponent();
            view.navigate_folder = navigate_folder;
        }
        private void navigate_folder(Windows.Storage.StorageFolder folder)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(winmain_page), folder);
        }
    }
}
