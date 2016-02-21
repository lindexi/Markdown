using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;

namespace produproperty.ViewModel
{

    /// <summary>
    ///
    /// </summary>
    public class winmain : notify_property
    {
        public winmain(StorageFolder folder)
        {
            ran=new Random();
            _folder = folder;
            textStack=new Stack<string>();

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
            get
            {
                return _text;
            }
        }

        public Action<int, int> selectchange;

        public int select;

        public int select_length;

        public void cancel_text()
        {
            _text = textStack.Pop();
            OnPropertyChanged("text");
        }

        public async Task<string> clipboard(DataPackageView con)
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
                var imgstream = await img.OpenReadAsync();
                Windows.Graphics.Imaging.BitmapDecoder decoder =
                    await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(imgstream);
                Windows.Graphics.Imaging.PixelDataProvider pxprd =
                    await
                        decoder.GetPixelDataAsync(Windows.Graphics.Imaging.BitmapPixelFormat.Bgra8,
                            Windows.Graphics.Imaging.BitmapAlphaMode.Straight,
                            new Windows.Graphics.Imaging.BitmapTransform(),
                            Windows.Graphics.Imaging.ExifOrientationMode.RespectExifOrientation,
                            Windows.Graphics.Imaging.ColorManagementMode.DoNotColorManage);
                byte[] buffer = pxprd.DetachPixelData();

                var file = await image_storage();

                using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var encoder =
                        await
                            Windows.Graphics.Imaging.BitmapEncoder.CreateAsync(
                                Windows.Graphics.Imaging.BitmapEncoder.PngEncoderId, fileStream);
                    encoder.SetPixelData(Windows.Graphics.Imaging.BitmapPixelFormat.Bgra8,
                        Windows.Graphics.Imaging.BitmapAlphaMode.Straight, decoder.PixelWidth, decoder.PixelHeight,
                        decoder.DpiX, decoder.DpiY, buffer);
                    await encoder.FlushAsync();

                    str = $"![这里写图片描述](image/{file.Name})\n";
                }
            }

            //文件
            if (con.Contains(StandardDataFormats.StorageItems))
            {
                var filelist = await con.GetStorageItemsAsync();
                StorageFile file = filelist.OfType<StorageFile>().First();
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

        private string[] spilt_text(string text,int select_index,int select_length)
        {
            if (select_index >= text.Length)
            {
                return null;
            }
            string str1 = text.Substring(0, select_index);
            string str2 = text.Substring(select_index, select_length);
            string str3 = text.Substring(select_index + select_length);
            return new[] {str1, str2, str3};
        }

        private async void file_storage()
        {
            var file =await _folder.GetFilesAsync();

        }
        
        private async Task<StorageFile> image_storage()
        {
            StorageFolder folder = await _folder.CreateFolderAsync(_file.Name, CreationCollisionOption.OpenIfExists);

            StorageFile file =
                await
                    folder.CreateFileAsync(
                        DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() +
                        DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() +
                        (ran.Next()%10000).ToString() + ".png", CreationCollisionOption.GenerateUniqueName);
            return file;
        }

        private async Task<string> imgfolder(StorageFile file)
        {
            string str = this._file.Name;
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
            else
            {
                str = $"[{file.Name}](image/{file.Name})\r\n\r\n";
                return str;
            }
        }

        private string _text;
        private readonly Stack<string> textStack; 

        private StorageFolder _folder;
        private StorageFile _file;
        private readonly Random ran;
    }

}


