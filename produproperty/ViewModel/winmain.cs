using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Graphics.Imaging;
using Windows.Storage;

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
            text = "";
        }

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
                OnPropertyChanged();
            }
            get { return _title; }
        }

        public int select;

        public int select_length;

        public Action<int, int> selectchange;

        private readonly StorageFolder _folder;
        private readonly Random ran;
        private readonly Stack<string> textStack;
        private StorageFile _file;

        private string _text;
        private string _title;


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

        private string text_line()
        {
            return "";
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

            if (file.FileType == ".png" || file.FileType == ".jpg")
            {
                str = $"![这里写图片描述](image/{file.Name})\r\n\r\n";
                return str;
            }
            str = $"[{file.Name}](image/{file.Name})\r\n\r\n";
            return str;
        }
    }
}