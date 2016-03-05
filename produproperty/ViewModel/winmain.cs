// lindexi
// 9:47

#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;

#endregion

namespace produproperty.ViewModel
{
    /// <summary>
    /// </summary>
    public class winmain : notify_property
    {
        public winmain(StorageFolder folder)
        {
            ran = new Random();

            _folder = folder;

            textStack = new Stack<string>();

            select = 0;
            select_length = 0;
            //title = "无标题";
            //str = "";

            file_storage_colleciton(_folder);
        }

        public ObservableCollection<file_storage> file_observable_collection
        {
            set;
            get;
        } =
            new ObservableCollection<file_storage>();

        public string text
        {
            set
            {
                textStack.Push(_text);
                _text = value;
                OnPropertyChanged();
            }
            get
            {
                return _text;
            }
        }

        public string title
        {
            set
            {
                _title = value;
                //motify_file();
                motify();
                //_file_storage.name = value;
                OnPropertyChanged();
            }
            get
            {
                if (_title == null)
                {
                    _title = "README.md";
                }
                if (!_title.EndsWith(".md"))
                {
                    _title += ".md";
                }
                return _title;
            }
        }

        public file_storage file
        {
            set
            {
                if (value != null)
                {
                    _file = value;
                    //file_deserialize();
                    OnPropertyChanged();
                }
            }
            get
            {
                return _file;
            }
        }

        public string advertisement
        {
            set
            {
                value = "";
            }
            get
            {
                object temp;
                if (ApplicationData.Current.LocalSettings.Values.TryGetValue("advertisement", out temp))
                {
                    return temp as string;
                }
                else
                {
                    return " ";
                }
            }
        }

        public int select;

        public int select_length;
        public Action<int, int> selectchange;
        private readonly Random ran;
        private readonly Stack<string> textStack;
        private file_storage _file;

        private file_storage _file_storage;

        private StorageFolder _folder;

        private string _text;
        private string _title;

        private void motify()
        {
            file?.motify(_title);
            toc(_file_storage, _folder);
        }

        private async void file_storage_colleciton(StorageFolder folder)
        {
            string str = "默认";
            await file_null(str);

            IReadOnlyList<StorageFile> filel = await folder.GetFilesAsync();
            //bool readme = false;
            file_observable_collection.Clear();

            foreach (StorageFile temp in filel.Where(temp => temp.FileType == ".md"))
            {
                if (temp.Name == "README.md")
                {
                    //if (file == null)
                    //{
                    //    file = temp;
                    //}
                    if (_file_storage == null)
                    {
                        _file_storage = new file_storage(temp, folder);
                    }
                    //readme = true;
                }
                file_observable_collection.Add(new file_storage(temp, folder));
            }

            if (_file_storage == null)
            {
                //_file = await _folder.CreateFileAsync("README.md");
                //file = null;
                string temp = "README.md";
                _file_storage =
                    new file_storage(await _folder.CreateFileAsync(temp, CreationCollisionOption.OpenIfExists), folder);
                //title = "README.md";
                //str = "";
                //text = str;
                // _file_storage.name = title;
                file_observable_collection.Add(_file_storage);
            }
            if (file == null)
            {
                file = _file_storage;
                title = file.name;
                text = await file_deserialize(file.file);
            }
            toc(_file_storage, folder);
        }

        private async Task file_null(string str)
        {
            if (_folder == null)
            {
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                _folder =
                    await
                        folder.CreateFolderAsync(str, CreationCollisionOption.OpenIfExists);
            }
        }

        public void bold_text()
        {
            if (select_length > 0)
            {
                string[] str = spilt_text(text, @select, select_length);
                text = str[0] + "**" + str[1] + "**" + str[2];
            }
            else
            {
                text = text.Insert(@select, "****");
            }
            selectchange(@select + 2, 0);
        }

        public async void open_file(file_storage temp)
        {
            file_serialization(file.file, "#" + file.name + "\n\n" + text);
            file = temp;
            text = await file_deserialize(file.file);
            //file_serialization();
            //this.file = file;
            //file_deserialize();
        }

        public async void dropimg(object sender, DragEventArgs e)
        {
            DragOperationDeferral defer = e.GetDeferral();
            try
            {
                DataPackageView data_view = e.DataView;
                string str = await clipboard(data_view);
                clipboard_substitution(str);
            }
            finally
            {
                defer.Complete();
            }
        }

        //private void tianjia(string str)
        //{
        //    string[] spilt = spilt_text();
        //    str = spilt[0] + str + spilt[2];

        //}

        private string[] spilt_text()
        {
            return spilt_text(text, @select, select_length);
        }

        private async void toc(file_storage _file_storage, StorageFolder folder)
        {
            StringBuilder str = new StringBuilder();
            foreach (file_storage temp in file_observable_collection)
            {
                //* [语言无关](#语言无关)
                str.Append($"* [{temp.name}]({temp.name})");
            }

            string string_temp = await file_deserialize(_file_storage.file);

            int n = string_temp.IndexOf("\n\n");
            if (n > 0)
            {
                file_serialization(_file_storage.file, str.ToString() + "\n\n" + string_temp.Substring(n));
            }
        }

        public void new_file()
        {
            file_serialization(file.file, "#" + title + "\n\n" + text);
            _file_storage = new file_storage(null, _folder);
            title = "请输入标题";
            text = "";
            textStack.Clear();
        }

        public void cancel_text()
        {
            _text = textStack.Pop();
            OnPropertyChanged("str");
        }

        private async Task<string> clipboard(DataPackageView con)
        {
            string str = string.Empty;
            //文本
            if (con.Contains(StandardDataFormats.Text))
            {
                str = await con.GetTextAsync();
                return str;
            }

            //图片
            if (con.Contains(StandardDataFormats.Bitmap))
            {
                RandomAccessStreamReference img = await con.GetBitmapAsync();
                IRandomAccessStreamWithContentType imgstream = await img.OpenReadAsync();
                BitmapDecoder decoder =
                    await BitmapDecoder.CreateAsync(imgstream);
                PixelDataProvider pxprd =
                    await
                        decoder.GetPixelDataAsync(BitmapPixelFormat.Bgra8,
                            BitmapAlphaMode.Straight,
                            new BitmapTransform(),
                            ExifOrientationMode.RespectExifOrientation,
                            ColorManagementMode.DoNotColorManage);
                byte[] buffer = pxprd.DetachPixelData();

                StorageFile temp = await image_storage(file);

                using (IRandomAccessStream file_stream = await temp.OpenAsync(FileAccessMode.ReadWrite))
                {
                    BitmapEncoder encoder =
                        await
                            BitmapEncoder.CreateAsync(
                                BitmapEncoder.PngEncoderId, file_stream);
                    encoder.SetPixelData(BitmapPixelFormat.Bgra8,
                        BitmapAlphaMode.Straight, decoder.PixelWidth, decoder.PixelHeight,
                        decoder.DpiX, decoder.DpiY, buffer);
                    await encoder.FlushAsync();

                    str = $"![这里写图片描述]({file.folder}/{temp.Name})\n";
                    return str;
                }
            }

            //文件
            if (con.Contains(StandardDataFormats.StorageItems))
            {
                str = (await con.GetStorageItemsAsync()).OfType<StorageFile>()
                    .Aggregate("", (current, temp) => current + imgfolder(file, temp));
                //StorageFile file = filelist.OfType<StorageFile>().First();
                //return await imgfolder(file);
            }

            return str;
        }

        private void clipboard_substitution(string str)
        {
            string[] str_spilt = spilt_text(text, select, select_length);
            text = str_spilt[0] + str.Replace("\r", "") + str_spilt[2];
            selectchange(str_spilt[0].Length + str.Length, 0);
        }

        private string text_line(string text, int select)
        {
            if (select < 0)
            {
                select = 0;
            }
            string[] str = text.Split('\n');
            foreach (string temp in str)
            {
                select -= temp.Length;
                if (select <= 0)
                {
                    return temp;
                }
            }
            return "";
        }

        private async void file_serialization(StorageFile temp, string str)
        {
            int n;
            n = str.IndexOf(advertisement);
            if (n < 0)
            {
                str += advertisement;
            }

            //str = "#" + title + "\n" + str;
            str = str.Replace("\n", "\n\n");
            str = str.Replace("\n\n\n\n", "\n\n");
            str = str.Replace("\n", "\r\n");
            //StorageFile temp = _file_storage.file;
            if (temp != null)
            {
                using (StorageStreamTransaction transaction = await temp.OpenTransactedWriteAsync())
                {
                    using (DataWriter data_writer = new DataWriter(transaction.Stream))
                    {
                        data_writer.WriteString(str);
                        transaction.Stream.Size = await data_writer.StoreAsync();
                        await transaction.CommitAsync();
                    }
                }
            }
        }

        //private async void motify_file()
        //{
        //    if (file == null && !string.IsNullOrEmpty(title))
        //    {
        //        file = await _folder.CreateFileAsync(title, CreationCollisionOption.GenerateUniqueName);
        //        title = file.Name;
        //        file_storage_colleciton();
        //    }
        //    if (title != file?.Name)
        //    {
        //        await _file.RenameAsync(title, NameCollisionOption.GenerateUniqueName);
        //        title = file?.Name;
        //        file_storage_colleciton();
        //    }
        //}

        private async Task<string> file_deserialize(StorageFile temp)
        {
            //StorageFile temp = file.file;
            //title = temp.Name;
            string str = "";
            using (IRandomAccessStream read_stream = await temp.OpenAsync(FileAccessMode.Read))
            {
                using (DataReader data_reader = new DataReader(read_stream))
                {
                    ulong size = read_stream.Size;
                    if (size <= uint.MaxValue)
                    {
                        str = data_reader.ReadString(await data_reader.LoadAsync((uint) size));
                        str = str.Replace("\r", "");
                        str = str.Replace("\n\n", "\n");
                        if (text_line(str, 0) == "#" + title)
                        {
                            int i = str.IndexOf('\n');
                            if (i > 0)
                            {
                                str = str.Substring(i);
                            }
                        }
                    }
                }
            }
            return str;
        }

        private string[] spilt_text(string text, int select_index, int select_length)
        {
            if (select_index >= text.Length)
            {
                return new[]
                {
                    text, string.Empty, string.Empty
                };
            }
            string str1 = text.Substring(0, select_index);
            if (select_index + select_length >= text.Length)
            {
                return new[]
                {
                    str1, text.Substring(select_index), string.Empty
                };
            }
            string str2 = text.Substring(select_index, select_length);
            string str3 = text.Substring(select_index + select_length);
            return new[]
            {
                str1, str2, str3
            };
        }

        private async void file_storage()
        {
            IReadOnlyList<StorageFile> file = await _folder.GetFilesAsync();
        }

        private async Task<StorageFile> image_storage(file_storage temp)
        {
            StorageFolder folder = temp.folder;
            //= await _folder.CreateFolderAsync(_file.Name, CreationCollisionOption.OpenIfExists);

            StorageFile file =
                await
                    folder.CreateFileAsync(
                        DateTime.Now.Year + DateTime.Now.Month.ToString() + DateTime.Now.Day +
                        DateTime.Now.Hour + DateTime.Now.Minute +
                        ran.Next()%10000 + ".png", CreationCollisionOption.GenerateUniqueName);
            return file;
        }

        private async Task<string> imgfolder(file_storage temp, StorageFile file) //StorageFile file)
        {
            //string str = _file.Name;
            //StorageFolder image = null;
            //try
            //{
            //    image = await _folder.GetFolderAsync(str);
            //}
            //catch
            //{
            //}
            //if (image == null)
            //{
            //    image = await _folder.CreateFolderAsync(str, CreationCollisionOption.OpenIfExists);
            //}
            string str;
            file = await file.CopyAsync(temp.folder, file.Name, NameCollisionOption.GenerateUniqueName);

            if (file.FileType == ".png" || file.FileType == ".jpg" || file.FileType == ".gif")
            {
                str = $"![这里写图片描述]({temp.folder}/{file.Name})\n\n";
                return str;
            }
            str = $"[{file.Name}]({temp.folder}/{file.Name})\n\n";
            return str;
        }
    }

    public class file_storage : notify_property
    {
        //public file_storage(StorageFile file)
        //{
        //    if (file != null)
        //    {
        //        this.file = file;
        //        name = file.Name;
        //    }
        //}

        public file_storage(StorageFile file, StorageFolder folder)
        {
            if (file != null)
            {
                this.file = file;
                name = file.Name;
                _b = storage(folder);
            }
            else
            {
                _b = storage(folder);
            }
        }

        public string name
        {
            set
            {
                //if (!value.EndsWith(".md"))
                //{
                //    value += ".md";
                //}
                //if (value != file.Name)
                //{

                //}
                motify(value);
            }
            get
            {
                return file.Name;
            }
        }

        public StorageFile file
        {
            set;
            get;
        }

        public StorageFolder folder
        {
            set;
            get;
        }

        private Task _b;

        private async Task storage(StorageFolder folder)
        {
            if (string.IsNullOrEmpty(name))
            {
                string str = "请输入标题.md";
                file = await folder.CreateFileAsync(str, CreationCollisionOption.GenerateUniqueName);
                //name = file.Name;
                int n;
                n = name.IndexOf(".md");
                if (n > 0)
                {
                    str = name.Substring(0, n);
                }
                this.folder = await folder.CreateFolderAsync(str, CreationCollisionOption.GenerateUniqueName);
            }
            else
            {
                if (name == "README.md")
                {
                    this.folder = await folder.CreateFolderAsync("image", CreationCollisionOption.OpenIfExists);
                }
                else
                {
                    this.folder = await folder.CreateFolderAsync(name, CreationCollisionOption.OpenIfExists);
                }
            }
        }

        public async void motify(string str)
        {
            if (!str.EndsWith(".md"))
            {
                str += ".md";
            }
            if (file == null)
            {
                try
                {
                    await _b;
                }
                catch
                {
                }
            }
            if (file == null)
            {
                return;
            }
            if (str != name)
            {
                await file.RenameAsync(str, NameCollisionOption.GenerateUniqueName);
                //name = file.Name;
                if (str == "README.md")
                {
                    str = "image";
                }
                else
                {
                    int n;
                    n = name.IndexOf(".md");
                    if (n > 0)
                    {
                        str = name.Substring(0, n);
                    }
                }
                await folder.RenameAsync(str, NameCollisionOption.GenerateUniqueName);
                OnPropertyChanged("name");
            }
        }
    }
}