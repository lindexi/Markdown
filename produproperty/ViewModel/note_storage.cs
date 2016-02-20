using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace produproperty.ViewModel
{

    /// <summary>
    ///
    /// </summary>
    public class note_storage
    {
        public note_storage()
        {

        }
        /// <summary>
        /// 新建库
        /// </summary>
        public async void new_storage()
        {
            navigate_folder(await folder_storage());
        }
        /// <summary>
        /// 打开
        /// </summary>
        public async void open_storage()
        {
            navigate_folder(await folder_storage());
        }

        /// <summary>
        /// 
        /// </summary>
        public Action<StorageFolder> navigate_folder;

        private async Task<StorageFolder> folder_storage()
        {
            Windows.Storage.Pickers.FolderPicker picker;
            picker = new Windows.Storage.Pickers.FolderPicker();
            picker.FileTypeFilter.Add("*.folder");
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            var folder = await picker.PickSingleFolderAsync();
            return folder;
        }
    }

}
