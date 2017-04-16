// lindexi
// 20:47

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using lindexi.uwp.Framework.ViewModel;

namespace produproperty.ViewModel
{
    /// <summary>
    /// </summary>
    public class NoteStorageModel : ViewModelBase
    {
        public NoteStorageModel()
        {
        }



        public override void OnNavigatedFrom(object sender, object obj)
        {
        }

        public override async void OnNavigatedTo(object sender, object obj)
        {
            await Read();
        }

        private async Task Read()
        {
            await NoteGoverment.Notegoverment.Read();
            FolderStorage = new ObservableCollection<ImpliedFolderStorage>();
            if (NoteGoverment.Notegoverment.FolderStorage.Count == 0)
            {
                FoundEmptFolderVisibility = Visibility.Visible;
            }
            else
            {
                FoundEmptFolderVisibility = Visibility.Collapsed;
            }
        }

        public ObservableCollection<ImpliedFolderStorage> FolderStorage
        {
            set;
            get;
        }



        /// <summary>
        ///     新建库
        /// </summary>
        public async void NewStorage()
        {
            FolderPicker pick = new FolderPicker();
            pick.FileTypeFilter.Add(".txt");
            var folder = await pick.PickSingleFolderAsync();
            //NoteGoverment.Notegoverment.NewFolderStorage()
            if (folder != null)
            {
                //如果不为空
                var newFolderStorage = NoteGoverment.Notegoverment.NewFolderStorage(folder);
                NavigateFolder(folder);
                await newFolderStorage;
            }
        }


        public void Navigate(object sender, ItemClickEventArgs e)
        {
            var folder = (ImpliedFolderStorage)e.ClickedItem;
            NavigateFolder(folder.FolderStorage);
        }

        private void NavigateFolder(StorageFolder folder)
        {
            
        }

        public string FoundEmptFolder
        {
            set;
            get;
        } = "没有找到存储文件\r\n请新建或选择一个已有文件夹";

        public Visibility FoundEmptFolderVisibility
        {
            set
            {
                _foundEmptFolderVisibility = value;
                OnPropertyChanged();
            }
            get
            {
                return _foundEmptFolderVisibility;
            }
        }

        private Visibility _foundEmptFolderVisibility;



        /// <summary>
        ///     打开
        /// </summary>
        public void OpenStorage()
        {

        }

        private void ToastText(string str)
        {
            var toastText = Windows.UI.Notifications.
                    ToastTemplateType.ToastText01;
            var content = Windows.UI.Notifications.
                ToastNotificationManager.GetTemplateContent(toastText);
            XmlNodeList xml = content.GetElementsByTagName("text");
            xml[0].AppendChild(content.CreateTextNode(str));
            ToastNotification toast = new ToastNotification(content);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        private async Task<StorageFolder> folder_storage()
        {
            var picker = new FolderPicker();
            picker.FileTypeFilter.Add(".folder");
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.ViewMode = PickerViewMode.Thumbnail;
            var folder = await picker.PickSingleFolderAsync();
            return folder;
        }
    }



    class NavigateComposite : CombinationComposite
    {
        public NavigateComposite(ViewModelBase source,Type navigate, object obj) : base(source)
        {
            _run = Navigate;
            _navigate = navigate;
            _obj = obj;
        }

        private Type _navigate;
        private object _obj;

        private void Navigate(ViewModelBase source, object o)
        {
            var viewModel = (NavigateViewModel)source;
            viewModel.Navigate(_navigate, _obj);
        }
    }
}