// lindexi
// 20:47

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace produproperty.ViewModel
{
    /// <summary>
    /// </summary>
    public class NoteStorage
    {
        public NoteStorage()
        {
            Read();
        }

        private void Read()
        {
            
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

    public class NoteGoverment
    {
        public NoteGoverment()
        {

        }

        public List<ImpliedFolderStorage> FolderStorage
        {
            set;
            get;
        }

        private void Read()
        {
            
        }

        public void NewFolderStorage(StorageFolder storageFolder)
        {
            //放入记录

        }


        private static NoteGoverment _noteGoverment;

        public static NoteGoverment Notegoverment
        {
            set
            {
                _noteGoverment = value;
            }
            get
            {
                return _noteGoverment??(_noteGoverment=new NoteGoverment());
            }
        }
    }
}