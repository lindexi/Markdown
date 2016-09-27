using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml.Media.Imaging;
using Newtonsoft.Json;

namespace produproperty.ViewModel
{
    public class NoteGoverment
    {
        public NoteGoverment()
        {

        }

        public List<ImpliedFolderStorage> FolderStorage
        {
            set;
            get;
        }

        public async Task Read()
        {
            // StorageItemAccessList
            //StorageItemAccessList
            //StorageApplicationPermissions.FutureAccessList.Add()

            //folder 
            //data

            try
            {
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                StorageFile file = await folder.GetFileAsync("data");
                var json = JsonSerializer.Create();
                FolderStorage = json.Deserialize<List<ImpliedFolderStorage>>(new JsonTextReader(
                    new StreamReader(await file.OpenStreamForReadAsync())));
                ReadFolderPick();
            }
            catch (FileNotFoundException)
            {

            }
        }

        private async void ReadFolderPick()
        {
            foreach (var temp in FolderStorage)
            {
                try
                {
                    temp.FolderStorage =
                        await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(temp.Token);
                }
                catch (Exception)
                {

                }
            }

            foreach (var temp in FolderStorage)
            {
                //image
                try
                {
                    StorageFolder folder = temp.FolderStorage;
                    string str = "image";
                    folder = await folder.GetFolderAsync(str);
                    StorageFile file = await folder.GetFileAsync(str + ".png");
                    BitmapImage image = new BitmapImage();
                    await image.SetSourceAsync(await file.OpenAsync(FileAccessMode.Read));
                    temp.Image = image;
                }
                catch 
                {
                   
                }
            }
        }

    



        public async Task NewFolderStorage(StorageFolder storageFolder)
        {
            //放入记录
            ImpliedFolderStorage folder = new ImpliedFolderStorage(storageFolder);
            FolderStorage.Add(folder);
            folder.Token = StorageApplicationPermissions.FutureAccessList.Add(storageFolder);
            await Storage();
        }

        public async Task Storage()
        {
            var json = JsonSerializer.Create();
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.CreateFileAsync("temp");
            json.Serialize(new JsonTextWriter(
                new StreamWriter(
                    await file.OpenStreamForWriteAsync())), FolderStorage);
            await file.MoveAsync(folder, "data", NameCollisionOption.ReplaceExisting);
        }


        private static NoteGoverment _noteGoverment;

        public static NoteGoverment Notegoverment
        {
            set
            {
                _noteGoverment = value;
            }
            get
            {
                return _noteGoverment ?? (_noteGoverment = new NoteGoverment());
            }
        }
    }
}