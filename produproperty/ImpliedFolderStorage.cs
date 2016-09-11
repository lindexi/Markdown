using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace produproperty
{
    public class ImpliedFolderStorage
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

        public StorageFolder FolderStorage
        {
            set;
            get;
        }

        public BitmapImage Image
        {
            set;
            get;
        }
    }
}