using Windows.Storage;

namespace produproperty.ViewModel
{
    public class FileMariyah
    {
        public FileMariyah()
        {
        }

        public FileMariyah(StorageFile file)
        {
            File = file;
        }

        public string Name
        {
            get { return File.Name; }
        }

        public StorageFile File
        {
            get; set;
        }
    }
}