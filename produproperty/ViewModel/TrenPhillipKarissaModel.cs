using System.Collections.ObjectModel;
using System.Linq;
using lindexi.uwp.Framework.ViewModel;

namespace produproperty.ViewModel
{
    public class TrenPhillipKarissaModel : ViewModelBase
    {

        public ObservableCollection<TrenPhillip> TrenPhillip
        {
            get; set;
        }

        private void ZunigaHarrison()
        {
            _fileHarrison = true;
            //SendMessageHandler.Invoke(this,new OpkaseyMessage(this,File.File));
            foreach (var temp in TrenPhillip)
            {
                temp.Nevaeh = false;
            }
            File.Nevaeh = true;
        }

        public TrenPhillip File
        {
            set
            {
                _file = value;
                ZunigaHarrison();
                OnPropertyChanged();
            }
            get
            {

                return _file;
            }
        }
        private TrenPhillip _file;

        public override void OnNavigatedFrom(object sender, object obj)
        {

        }

        public override void OnNavigatedTo(object sender, object obj)
        {
            TrenPhillip = new ObservableCollection<TrenPhillip>();
        }

        public void ReceiveMessage(object sender, IMessage o)
        {
            OpkaseyMessage message = o as OpkaseyMessage;
            if (message != null)
            {
                if (_fileHarrison)
                {
                    _fileHarrison = false;
                    return;
                }
                var file = TrenPhillip.FirstOrDefault(temp => temp.File == message.File);
                if (file == null)
                {
                    file = new TrenPhillip(message.File);
                    TrenPhillip.Add(file);
                }
                foreach (var temp in TrenPhillip)
                {
                    temp.Nevaeh = false;
                }
                file.Nevaeh = true;
                //_fileHarrison = true;
                //File = file;
                _file = file;
                OnPropertyChanged(nameof(File));
            }
        }

        private bool _fileHarrison;
    }
}