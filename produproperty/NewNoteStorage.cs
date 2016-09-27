using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace produproperty
{
    public class NewNoteStorage
    {
        public NewNoteStorage()
        {
        }

        public void Storage(StorageFolder folder)
        {

        }

        private void ImageStorage()
        {

        }

        private async Task RStorage(StorageFolder folder)
        {
            //name
            //作者
            //简介
            //目录
            //脚注
            string emptyStr = "<!--{0}-->\r\n";
            StringBuilder str = new StringBuilder();
            //<!--目录-->

            str.Append(string.Format(emptyStr, "name"));
            str.Append(folder.Name + "\r\n");
            str.Append(string.Format(emptyStr, "name"));

            str.Append(string.Format(emptyStr, "作者"));
            str.Append(AuthoStr + "\r\n");
            str.Append(string.Format(emptyStr, "作者"));

            str.Append(string.Format(emptyStr, "简介"));
            str.Append(Introduction);
            str.Append(string.Format(emptyStr, "简介"));

            str.Append(string.Format(emptyStr, "目录"));
            foreach (var temp in FolderStorage)
            {
                str.Append(string.Format("[{0}]({1})\r\n", temp.Name, temp.FolderStorage.Path.Replace(
                    folder.Path, ".")));
            }
            str.Append(string.Format(emptyStr, "目录"));

            str.Append(string.Format(emptyStr, "脚注"));
            str.Append(Artis);
            str.Append(string.Format(emptyStr, "脚注"));

            StorageFile file = await folder.CreateFileAsync("README.md", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, str.ToString());
        }

        public List<ImpliedFolderStorage> FolderStorage
        {
            set;
            get;
        }=new List<ImpliedFolderStorage>();

        public string AuthoStr
        {
            set;
            get;
        }

        public string Introduction
        {
            set;
            get;
        }

        public string Artis
        {
            set;
            get;
        }

        public BitmapImage Image
        {
            set;
            get;
        }

    }
}
