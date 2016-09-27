using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Newtonsoft.Json;

namespace produproperty
{
    public class ImpliedFolderStorage : ViewModel.NotifyProperty
    {
        public ImpliedFolderStorage(StorageFolder folderStorage)
        {
            FolderStorage = folderStorage;
            Name = folderStorage.Name;
        }

        public string Name
        {
            set;
            get;
        }
        [JsonIgnore]
        public StorageFolder FolderStorage
        {
            set;
            get;
        }
        [JsonIgnore]
        public BitmapImage Image
        {
            set
            {
                _image = value;
                OnPropertyChanged();
            }
            get
            {
                return _image;
            }
        }

        private BitmapImage _image;
        public string Token
        {
            set;
            get;
        }
    }
}