using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;

namespace produproperty
{
    class viewModel:ViewModel.notify_property
    {
        public viewModel()
        {
            _m = new model(this);
        }

        public string text
        {
            set
            {
                _m._text = value;
                OnPropertyChanged();
            }
            get
            {
                return _m._text;
            }
        }

        public string name
        {
            set
            {
                _m._name = value;
                OnPropertyChanged();
            }
            get
            {
                return _m._name;
            }
        }

        public bool writetext
        {
            set
            {
                _m._writetext = value;
                OnPropertyChanged();
            }
            get
            {
                return _m._writetext;
            }
        }

        public Action<int, int> selectchange;

        public int select;

        public void clipboard(TextControlPasteEventArgs e)
        {
            if (writetext)
            {
                return;
            }

            e.Handled = true;

            DataPackageView con = Windows.ApplicationModel.DataTransfer.Clipboard.GetContent();
           string str= _m.clipboard(con);
        }

        public async void accessfolder()
        {
            FolderPicker pick = new FolderPicker();
            pick.FileTypeFilter.Add("*");
            StorageFolder folder = await pick.PickSingleFolderAsync();
            if (folder != null)
            {
                _m.accessfolder(folder);
            }
        }

        private model _m;
        
    }
}
