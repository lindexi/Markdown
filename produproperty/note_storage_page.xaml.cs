// lindexi
// 20:47

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using produproperty.ViewModel;

namespace produproperty
{
    public partial class NoteStoragePage : Page
    {
        /// <summary>
        ///     程序打开第一界面
        /// </summary>
        public NoteStoragePage()
        {
            View = new NoteStorage();
            InitializeComponent();
            View.NavigateFolder = navigate_folder;
        }

        private NoteStorage View
        {
            set;
            get;
        }

        private void navigate_folder(StorageFolder folder)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame?.Navigate(typeof(WinmainPage), folder);
        }
    }
}