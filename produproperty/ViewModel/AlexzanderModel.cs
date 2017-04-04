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

    }
}