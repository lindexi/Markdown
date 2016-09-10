// lindexi
// 20:47

using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace produproperty.ViewModel
{
    /// <summary>
    /// </summary>
    public class NoteStorage
    {
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
}