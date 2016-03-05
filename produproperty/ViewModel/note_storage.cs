using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace produproperty.ViewModel
{
    /// <summary>
    /// </summary>
    public class note_storage
    {
        /// <summary>
        /// </summary>
        public Action<StorageFolder> navigate_folder;

        /// <summary>
        ///     新建库
        /// </summary>
        public async void new_storage()
        {
            StorageFolder folder = await folder_storage();
            if (folder != null)
            {
                navigate_folder(folder);
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
                navigate_folder(folder);
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