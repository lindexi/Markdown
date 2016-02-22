using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;

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
            //text = "";

            file_storage_colleciton();
        }

        public ObservableCollection<filestr> file_observable_collection { set; get; } =
            new ObservableCollection<filestr>();

        public string text
        {
            set
            {
                textStack.Push(_text);
                _text = value;
                OnPropertyChanged();
            }
            get { return _text; }
        }

        public string title
        {
            set
            {
                _title = value;
                motify_file();
                OnPropertyChanged();
            }
            get
            {
                return _title;
            }
        }

        public StorageFile file
        {
            set
            {
                if (value != null)
                {
                    _file = value;
                    file_deserialize();
                    OnPropertyChanged();
                }
            }
            get { return _file; }
        }

        public int select;

        public int select_length;
        public Action<int, int> selectchange;
        private readonly Random ran;
        private readonly Stack<string> textStack;
        private StorageFile _file;

        private StorageFolder _folder;

        private string _text;
        private string _title;

        private async void file_storage_colleciton()
        {
            var str = "默认";
            if (_folder == null)
            {
                var folder = ApplicationData.Current.LocalFolder;
                _folder =
                    await
                        folder.CreateFolderAsync(str, CreationCollisionOption.OpenIfExists);
            }

            var filel = await _folder.GetFilesAsync();
            var readme = false;

            foreach (var temp in filel.Where(temp => temp.FileType == ".md"))
            {
                if (temp.Name == "README.md")
                {
                    file = temp;
                    readme = true;
                }
                file_observable_collection.Add(new filestr(temp));
            }

            if (!readme)
            {
                _file = await _folder.CreateFileAsync("README.md");
                str = "";
                text = str;
                file_serialization();
            }
        }

        public void bold_text()
        {
            if (select_length > 0)
            {
                var str = spilt_text(text, @select, select_length);
                text = str[0] + "**" + str[1] + "**" + str[2];
            }
            else
            {
                text = text.Insert(@select, "****");
            }
            selectchange(@select + 2, 0);
        }


        public void cancel_text()
        {
            _text = textStack.Pop();
            OnPropertyChanged("text");
        }

        public async Task<string> clipboard(DataPackageView con)
        {
            var str = string.Empty;
            //文本
            if (con.Contains(StandardDataFormats.Text))
            {
                str = await con.GetTextAsync();
                return str;
            }

            //图片
            if (con.Contains(StandardDataFormats.Bitmap))
            {
                var img = await con.GetBitmapAsync();
                var imgstream = await img.OpenReadAsync();
                var decoder =
                    await BitmapDecoder.CreateAsync(imgstream);
                var pxprd =
                    await
                        decoder.GetPixelDataAsync(BitmapPixelFormat.Bgra8,
                            BitmapAlphaMode.Straight,
                            new BitmapTransform(),
                            ExifOrientationMode.RespectExifOrientation,
                            ColorManagementMode.DoNotColorManage);
                var buffer = pxprd.DetachPixelData();

                var file = await image_storage();

                using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var encoder =
                        await
                            BitmapEncoder.CreateAsync(
                                BitmapEncoder.PngEncoderId, fileStream);
                    encoder.SetPixelData(BitmapPixelFormat.Bgra8,
                        BitmapAlphaMode.Straight, decoder.PixelWidth, decoder.PixelHeight,
                        decoder.DpiX, decoder.DpiY, buffer);
                    await encoder.FlushAsync();

                    str = $"![这里写图片描述](image/{file.Name})\n";
                }
            }

            //文件
            if (con.Contains(StandardDataFormats.StorageItems))
            {
                var filelist = await con.GetStorageItemsAsync();
                var file = filelist.OfType<StorageFile>().First();
                return await imgfolder(file);
            }

            return str;
        }

        public void clipboard_substitution(string str)
        {
            var str_spilt = spilt_text(text, select, select_length);
            text = str_spilt[0] + str + str_spilt[2];
        }

        private string text_line(string text, int select)
        {
            if (select < 0)
            {
                select = 0;
            }
            var str = text.Split('\n');
            foreach (var temp in str)
            {
                select -= temp.Length;
                if (select <= 0)
                {
                    return temp;
                }
            }
            return "";
        }

        private async void file_serialization()
        {
            using (var transaction = await file.OpenTransactedWriteAsync())
            {
                using (var dataWriter = new DataWriter(transaction.Stream))
                {
                    dataWriter.WriteString(title + text);
                    transaction.Stream.Size = await dataWriter.StoreAsync();
                    await transaction.CommitAsync();
                }
            }
        }

        private async void motify_file()
        {
            if (file == null && !string.IsNullOrEmpty(title))
            {
                file = await _folder.CreateFileAsync(title);
            }
            if (title != file.Name)
            {
                await _file.RenameAsync(title, NameCollisionOption.GenerateUniqueName);
                title = file.Name;
            }
        }

        private async void file_deserialize()
        {
            title = file.Name;
            using (var readStream = await file.OpenAsync(FileAccessMode.Read))
            {
                using (var dataReader = new DataReader(readStream))
                {
                    var size = readStream.Size;
                    if (size <= uint.MaxValue)
                    {
                        var numBytesLoaded = await dataReader.LoadAsync((uint) size);
                        text = dataReader.ReadString(numBytesLoaded);
                        if (text_line(text, 0) == "#" + title)
                        {
                            var i = text.IndexOf('\n');
                            if (i > 0)
                            {
                                text = text.Substring(i);
                            }
                        }
                    }
                }
            }
        }

        private string[] spilt_text(string text, int select_index, int select_length)
        {
            if (select_index >= text.Length)
            {
                return new[] {text, string.Empty, string.Empty};
            }
            var str1 = text.Substring(0, select_index);
            if (select_index + select_length >= text.Length)
            {
                return new[] {str1, text.Substring(select_index), string.Empty};
            }
            var str2 = text.Substring(select_index, select_length);
            var str3 = text.Substring(select_index + select_length);
            return new[] {str1, str2, str3};
        }

        private async void file_storage()
        {
            var file = await _folder.GetFilesAsync();
        }

        private async Task<StorageFile> image_storage()
        {
            var folder = await _folder.CreateFolderAsync(_file.Name, CreationCollisionOption.OpenIfExists);

            var file =
                await
                    folder.CreateFileAsync(
                        DateTime.Now.Year + DateTime.Now.Month.ToString() + DateTime.Now.Day +
                        DateTime.Now.Hour + DateTime.Now.Minute +
                        ran.Next()%10000 + ".png", CreationCollisionOption.GenerateUniqueName);
            return file;
        }

        private async Task<string> imgfolder(StorageFile file)
        {
            var str = _file.Name;
            StorageFolder image = null;
            try
            {
                image = await _folder.GetFolderAsync(str);
            }
            catch
            {
            }
            if (image == null)
            {
                image = await _folder.CreateFolderAsync(str, CreationCollisionOption.OpenIfExists);
            }
            file = await file.CopyAsync(image, file.Name, NameCollisionOption.GenerateUniqueName);

            if (file.FileType == ".png" || file.FileType == ".jpg" || file.FileType == ".gif")
            {
                str = $"![这里写图片描述](image/{file.Name})\n\n";
                return str;
            }
            str = $"[{file.Name}](image/{file.Name})\n\n";
            return str;
        }
    }

    public class filestr
    {
        public filestr(StorageFile file)
        {
            if (file != null)
            {
                this.file = file;
                name = file.Name;
            }
        }

        public string name { set; get; }

        public StorageFile file { set; get; }
    }
}