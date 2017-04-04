using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using lindexi.uwp.Framework.ViewModel;

namespace produproperty.ViewModel
{
    public class MartinRhfinwittModel : NavigateViewModel
    {
        public MartinRhfinwittModel()
        {
        }

        public KaydenSergioModel KaydenSergioModel
        {
            get; set;
        }

        public TrenPhillipKarissaModel TrenPhillipKarissaModel
        {
            get; set;
        }

        public AlexzanderModel AlexzanderModel
        {
            get; set;
        }

        public Frame KaydenSergioFrame
        {
            set; get;
        }

        public Frame TrenPhillipKarissaFrame
        {
            set; get;
        }

        public Frame AlexzanderfFrame
        {
            set; get;
        }



        public override void OnNavigatedFrom(object sender, object obj)
        {
            AlexzanderModel.OnNavigatedFrom(this, obj);
            TrenPhillipKarissaModel.OnNavigatedFrom(this, obj);
            KaydenSergioModel.OnNavigatedFrom(this, obj);
        }

        public override void OnNavigatedTo(object sender, object obj)
        {
            AlexzanderModel = new AlexzanderModel();
            TrenPhillipKarissaModel = new TrenPhillipKarissaModel();
            KaydenSergioModel = new KaydenSergioModel();

            ViewModel = new List<ViewModelPage>();

            ViewModel.Add(new ViewModelPage(AlexzanderModel));
            ViewModel.Add(new ViewModelPage(TrenPhillipKarissaModel));
            ViewModel.Add(new ViewModelPage(KaydenSergioModel));

            foreach (var temp in Application.Current.GetType().GetTypeInfo().Assembly.DefinedTypes.Where(temp => temp.IsSubclassOf(typeof(Page))))
            {
                //获取特性，特性有包含ViewModel
                var p = temp.GetCustomAttribute<ViewModelAttribute>();

                var viewmodel = this.ViewModel.FirstOrDefault(t => t.Equals(p?.ViewModel));
                if (viewmodel != null)
                {
                    viewmodel.Page = temp.AsType();
                }
            }

            //AlexzanderModel.OnNavigatedTo(this, obj);
            //TrenPhillipKarissaModel.OnNavigatedTo(this, obj);
            //KaydenSergioModel.OnNavigatedTo(this, obj);

            //Read();
        }


        private List<FileMariyah> File { set; get; } = new List<FileMariyah>();

        public async void Read()
        {
            FolderPicker pick = new FolderPicker();
            pick.FileTypeFilter.Add(".txt");
            var folder = await pick.PickSingleFolderAsync();
            File.AddRange((await folder.GetFilesAsync()).Select(temp=>new FileMariyah(temp)));
            await Navigate();
        }


        private async Task Navigate()
        {
            await Navigate(AlexzanderModel.GetType(), File, AlexzanderfFrame);
            await Navigate(TrenPhillipKarissaModel.GetType(), File, TrenPhillipKarissaFrame);
            await Navigate(KaydenSergioModel.GetType(), File, KaydenSergioFrame);
        }

    }

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

    /// <summary>
    /// 侧边
    /// </summary>
    public class KaydenSergioModel : ViewModelBase
    {
        public ObservableCollection<FileMariyah> File { set; get; } = new ObservableCollection<FileMariyah>();

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

    public class TrenPhillipKarissaModel : ViewModelBase
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
