using System;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Newtonsoft.Json;

namespace produproperty
{
    public class ImpliedFolderStorage : ViewModel.NotifyProperty,IEquatable<ImpliedFolderStorage>
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

        public bool Equals(ImpliedFolderStorage other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(FolderStorage, other.FolderStorage);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ImpliedFolderStorage) obj);
        }

        public override int GetHashCode()
        {
            return FolderStorage?.GetHashCode() ?? 0;
        }
    }
}