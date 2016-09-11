// lindexi
// 20:47

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
    internal class viewModel : NotifyProperty
    {
        public viewModel()
        {
            _m = new model(this);
            OnPropertyChanged("text");
            OnPropertyChanged("name");

            object temp;
            if (ApplicationData.Current.LocalSettings.Values.TryGetValue("width", out temp))
            {
                Width = temp as string;
            }
            else
            {
                Width = "20";
            }

            Advertisement = @"
作者：lindexi_gd
邮箱：lindexi_gd@163.com
博客地址：http://blog.csdn.net/lindexi_gd   在原博客看会有好的排版";
        }

        public string Text
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

        public string Name
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

        public bool Writetext
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

        public string Addressfolder
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

        public string Width
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

        public string Advertisement
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
                        Advertisement = temp as string;
                    }
                    else
                    {
                        Advertisement = " ";
                    }
                }
                return _advertisement;
            }
        }

        public Action<int, int> Selectchange
        {
            set;
            get;
        }

        public async void Clipboard(TextControlPasteEventArgs e)
        {
            if (Writetext)
            {
                return;
            }

            e.Handled = true;
            DataPackageView con = Windows.ApplicationModel.DataTransfer.Clipboard.GetContent();
            string str = await _m.clipboard(con);
            Tianjia(str);
        }

        public async void Storage()
        {
            if (Writetext)
            {
                return;
            }
            await _m.storage();

            //_m.Current_Suspending(this, new object() as SuspendingEventArgs);  
        }

        public async void Dropimg(object sender, DragEventArgs e)
        {
            if (Writetext)
            {
                return;
            }
            DragOperationDeferral defer = e.GetDeferral();
            try
            {
                DataPackageView dataView = e.DataView;
                string str = await _m.clipboard(dataView);
                Tianjia(str);
            }
            finally
            {
                defer.Complete();
            }
        }

        public async void Accessfolder()
        {
            FolderPicker pick = new FolderPicker();
            pick.FileTypeFilter.Add("*");
            StorageFolder folder = await pick.PickSingleFolderAsync();
            if (folder != null)
            {
                _m.accessfolder(folder);
            }
            Addressfolder = string.Empty;
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

        public void Tianjia(string str)
        {
            int n;
            n = Select;
            int i;
            for (i = 0; (n > 0) && (i < Text.Length); i++)
            {
                if (Text[i] != '\r') //&& text[i] != '\n')
                {
                    n--;
                }
            }
            Text = Text.Insert(i, str);
            str = str.Replace("\r", "");
            n = Select + str.Length;
            if (n > Text.Length)
            {
                n = Text.Length;
            }
            Selectchange(n, 0);

            //string t = text.Replace("\r\n", "\n");
            //t = t.Insert(select, str);
            //text = t.Replace("\n", "\r\n");
        }

        private string _advertisement;

        private model _m;
        private int _width;

        public int Select;
    }
}