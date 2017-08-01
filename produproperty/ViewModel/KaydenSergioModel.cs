using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using lindexi.uwp.Framework.ViewModel;

namespace produproperty.ViewModel
{
    /// <summary>
    /// ²à±ß
    /// </summary>
    public class KaydenSergioModel : ViewModelBase
    {
        public ObservableCollection<FileMariyah> File { set; get; } = new ObservableCollection<FileMariyah>();
        private FileMariyah _haiden;

        private bool _fileHaiden;
        private void ZunigaHarrison()
        {
            _fileHaiden = true;
            //Send.Invoke(this, new OpkaseyMessage(this, Haiden));
            
        }

        public FileMariyah Haiden
        {
            set
            {
                _haiden = value;
                ZunigaHarrison();
                OnPropertyChanged();
            }
            get
            {
                return _haiden;
            }
        }

        public void ReceiveMessage(object sender, IMessage o)
        {
            if (o is OpkaseyMessage)
            {
                if (_fileHaiden)
                {
                    _fileHaiden = false;
                    return;
                }
                OpkaseyMessage message = (OpkaseyMessage)o;
                if (File.Any(temp => temp == message.File))
                {

                }
                //Haiden = message.File;
                //_fileHaiden = true;
                _haiden = message.File;
                OnPropertyChanged(nameof(Haiden));
            }
        }

        public override void OnNavigatedFrom(object sender, object obj)
        {
        }

        public override void OnNavigatedTo(object sender, object obj)
        {
            List<FileMariyah> file = obj as List<FileMariyah>;
            if (file == null)
            {
                return;
            }
            _file = file;
            foreach (var temp in file)
            {
                File.Add(temp);
            }
        }

        private List<FileMariyah> _file;

        public void OpenFile()
        {

        }
    }
}