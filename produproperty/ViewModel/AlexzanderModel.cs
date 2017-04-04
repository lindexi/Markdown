using System;
using Windows.Storage;
using lindexi.uwp.Framework.ViewModel;

namespace produproperty.ViewModel
{
    public class AlexzanderModel : ViewModelBase
    {
        private string _str;
        public string Str
        {
            set
            {
                _str = value;
                OnPropertyChanged();
            }
            get
            {
                return _str;
            }
        }

        public override void OnNavigatedFrom(object sender, object obj)
        {

        }

        public override void OnNavigatedTo(object sender, object obj)
        {

        }

        public override void ReceiveMessage(object sender, Message o)
        {
            //if (o is OpkaseyMessage)
            //{
            //    OpkaseyMessage message = (OpkaseyMessage)o;
            //    ReadHarrison = true;
            //    Read(message.File);
            //}
        }

        private async void Read(FileMariyah message)
        {
            var file = message.File;
            Str = await FileIO.ReadTextAsync(file);
            ReadHarrison = false;
        }

        private bool _readHarrison = true;

        public bool ReadHarrison
        {
            set
            {
                _readHarrison = value;
                OnPropertyChanged();
            }
            get
            {
                return _readHarrison;
            }
        }
    }
}