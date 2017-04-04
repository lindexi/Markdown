using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lindexi.uwp.Framework.ViewModel;

namespace produproperty.ViewModel
{
    class MartinRhfinwittModel : ViewModelBase
    {
        public KaydenSergioModel KaydenSergioModel
        {
            get; set;
        }
        public TrenPhillipKarissaModel TrenPhillipKarissaModel
        {
            get; set;
        }
        public override void OnNavigatedFrom(object sender, object obj)
        {
        }

        public override void OnNavigatedTo(object sender, object obj)
        {
        }
    }

    class AlexzanderModel : ViewModelBase
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

    /// <summary>
    /// 侧边
    /// </summary>
    class KaydenSergioModel : ViewModelBase
    {
        public override void OnNavigatedFrom(object sender, object obj)
        {
        }

        public override void OnNavigatedTo(object sender, object obj)
        {
        }
    }

    class TrenPhillipKarissaModel : ViewModelBase
    {

        public ObservableCollection<TrenPhillip> TrenPhillip
        {
            get; set;
        }

        public override void OnNavigatedFrom(object sender, object obj)
        {

        }

        public override void OnNavigatedTo(object sender, object obj)
        {

        }
    }
}
