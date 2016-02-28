// lindexi
// 9:05

#region

using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

#endregion

namespace produproperty.ViewModel
{
    internal class viewModel : notify_property
    {
        public viewModel()
        {
            _m = new model(this);
            OnPropertyChanged("text");
            OnPropertyChanged("name");

            object temp;
            if (ApplicationData.Current.LocalSettings.Values.TryGetValue("width", out temp))
            {
                width = temp as string;
            }
            else
            {
                width = "20";
            }
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

        public string addressfolder
        {
            set
            {
                OnPropertyChanged();
            }
            get
            {
                return _m.folder.Path;
            }
        }

        public string width
        {
            set
            {
                try
                {
                    int temp;
                    temp = Convert.ToInt32(value);
                    _width = temp;
                    OnPropertyChanged();
                    ApplicationData.Current.LocalSettings.Values["width"] = value;
                }
                catch
                {
                }
            }
            get
            {
                return _width.ToString();
            }
        }

        public string advertisement
        {
            set
            {
                _advertisement = value;
                OnPropertyChanged();
                ApplicationData.Current.LocalSettings.Values["advertisement"] = value;
            }
            get
            {
                if (string.IsNullOrEmpty(_advertisement))
                {
                    object temp;
                    if (ApplicationData.Current.LocalSettings.Values.TryGetValue("advertisement", out temp))
                    {
                        advertisement = temp as string;
                    }
                    else
                    {
                        advertisement = " ";
                    }
                }
                return _advertisement;
            }
        }

        public int select;

        public Action<int, int> selectchange;
        private string _advertisement;

        private model _m;
        private int _width;

        public async void clipboard(TextControlPasteEventArgs e)
        {
            if (writetext)
            {
                return;
            }

            e.Handled = true;
            DataPackageView con = Clipboard.GetContent();
            string str = await _m.clipboard(con);
            tianjia(str);
        }

        public async void storage()
        {
            if (writetext)
            {
                return;
            }
            await _m.storage();

            //_m.Current_Suspending(this, new object() as SuspendingEventArgs);  
        }

        public async void dropimg(object sender, DragEventArgs e)
        {
            if (writetext)
            {
                return;
            }
            DragOperationDeferral defer = e.GetDeferral();
            try
            {
                DataPackageView dataView = e.DataView;
                string str = await _m.clipboard(dataView);
                tianjia(str);
            }
            finally
            {
                defer.Complete();
            }
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
            addressfolder = string.Empty;
        }

        public async void file_open()
        {
            FileOpenPicker pick = new FileOpenPicker();
            //显示方式
            pick.ViewMode = PickerViewMode.Thumbnail;
            //选择最先的位置
            pick.SuggestedStartLocation =
                PickerLocationId.PicturesLibrary;
            //后缀名
            pick.FileTypeFilter.Add(".txt");
            pick.FileTypeFilter.Add(".md");

            StorageFile file = await pick.PickSingleFileAsync();

            if (file != null)
            {
                _m.open_file(file);
            }
        }

        public void tianjia(string str)
        {
            int n;
            n = select;
            int i;
            for (i = 0; n > 0 && i < text.Length; i++)
            {
                if (text[i] != '\r') //&& text[i] != '\n')
                {
                    n--;
                }
            }
            text = text.Insert(i, str);
            str = str.Replace("\r", "");
            n = select + str.Length;
            if (n > text.Length)
            {
                n = text.Length;
            }
            selectchange(n, 0);

            //string t = text.Replace("\r\n", "\n");
            //t = t.Insert(select, str);
            //text = t.Replace("\n", "\r\n");
        }
    }
}