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

namespace produproperty.ViewModel
{
    /// <summary>
    /// </summary>
    public class NoteStorageModel : NotifyProperty
    {
        public NoteStorageModel()
        {
            //Read();
        }

        public async void Navigateto()
        {
            //

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
        /// </summary>
        public Action<StorageFolder> NavigateFolder
        {
            set;
            get;
        }

        /// <summary>
        ///     新建库
        /// </summary>
        public async void new_storage()
        {
            StorageFolder folder = await folder_storage();
            if (folder != null)
            {
                NavigateFolder(folder);
            }
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
        public async void open_storage()
        {
            StorageFolder folder = await folder_storage();
            if (folder != null)
            {
                NavigateFolder(folder);
            }
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
}