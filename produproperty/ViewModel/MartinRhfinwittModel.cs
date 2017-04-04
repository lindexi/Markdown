using System;
using System.Collections.Generic;
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

            foreach (var temp in Application.Current.GetType().GetTypeInfo().Assembly.DefinedTypes.Where(temp => temp.IsSubclassOf(typeof(Composite))))
            {
                Composite.Add((Composite)temp.AsType().GetConstructor(Type.EmptyTypes).Invoke(null));
            }

            SendMessageHandler = ReceiveMessage;

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
            File.AddRange((await folder.GetFilesAsync()).Select(temp => new FileMariyah(temp)));
            await Navigate();

            var file = File.FirstOrDefault(temp => string.Equals(temp.Name, "README.md", StringComparison.CurrentCultureIgnoreCase));
            if (file != null)
            {
                SendMessageHandler.Invoke(this, new OpkaseyMessage(this, file));
            }
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


        private async Task Navigate()
        {
            await Navigate(AlexzanderModel.GetType(), File, AlexzanderfFrame);
            await Navigate(TrenPhillipKarissaModel.GetType(), File, TrenPhillipKarissaFrame);
            await Navigate(KaydenSergioModel.GetType(), File, KaydenSergioFrame);
        }

    }

    class OpkaseyMessage : Message
    {
        public OpkaseyMessage(ViewModelBase source, FileMariyah file) : base(source)
        {
            File = file;
        }

        public FileMariyah File
        {
            get; set;
        }
    }

    class OpkaseyComposite : Composite
    {
        public OpkaseyComposite()
        {
            Message = typeof(OpkaseyMessage);
        }

        public override void Run(ViewModelBase source, Message o)
        {
            MartinRhfinwittModel viewModel = source as MartinRhfinwittModel;
            OpkaseyMessage message = o as OpkaseyMessage;
            if (viewModel != null && message != null)
            {
                viewModel.TrenPhillipKarissaModel.ReceiveMessage(viewModel, message);
                viewModel.AlexzanderModel.ReceiveMessage(viewModel, message);
                viewModel.KaydenSergioModel.ReceiveMessage(viewModel, message);
            }
        }
    }
}
