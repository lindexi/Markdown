// lindexi
// 20:47

#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using lindexi.uwp.Framework.ViewModel;
using produproperty.View;

#endregion

namespace produproperty.ViewModel
{
    public class ViewModel : NavigateViewModel
    {
        public ViewModel()
        {

        }

        public override void OnNavigatedFrom(object sender, object obj)
        {
        }

        public override void OnNavigatedTo(object sender, object obj)
        {
            if (obj is Frame)
            {
                Content = (Frame)obj;
            }

#if NOGUI
#else
            Content.Navigate(typeof(produproperty.View.SplashPage));
#endif
            if (this.ViewModel == null)
            {
                this.ViewModel = new List<ViewModelPage>();
                //加载所有ViewModel
                var applacationAssembly = Application.Current.GetType().GetTypeInfo().Assembly;

                foreach (var temp in applacationAssembly.DefinedTypes.Where(temp => temp.IsSubclassOf(typeof(ViewModelBase))))
                {
                    this.ViewModel.Add(new ViewModelPage(temp.AsType()));
                }

                foreach (var temp in applacationAssembly.DefinedTypes.Where(temp => temp.IsSubclassOf(typeof(Page))))
                {
                    //获取特性，特性有包含ViewModel
                    var p = temp.GetCustomAttribute<ViewModelAttribute>();

                    var viewmodel = this.ViewModel.FirstOrDefault(t => t.Equals(p?.ViewModel));
                    if (viewmodel != null)
                    {
                        viewmodel.Page = temp.AsType();
                    }

                }

                foreach (var temp in applacationAssembly.DefinedTypes.Where(temp => temp.IsSubclassOf(typeof(Composite))))
                {
                    try
                    {
                        Composite.Add((Composite) temp.AsType().GetConstructor(Type.EmptyTypes).Invoke(null));
                    }
                    catch 
                    {
                        
                    }
                }
            }
            Navigate(typeof(NoteStorageModel), null);
        }
    }

    //    public class ViewModel:NavigateViewModel
    //    {
    //        public ViewModel()
    //        {
    //            Send += Receive;
    //        }

    //        private string _str;


    //        public string Str
    //        {
    //            set
    //            {
    //                _str = value;
    //                OnPropertyChanged();
    //            }
    //            get { return _str; }
    //        }

    //        public override void OnNavigatedFrom(object obj)
    //        {
    //        }

    //        public override void OnNavigatedTo(object obj)
    //        {
    //            Content = (Frame)Window.Current.Content;
    //#if NOGUI
    //#else
    //            //Content.Navigate(typeof(SplashPage));
    //#endif
    //            if (ViewModel == null)
    //            {
    //                ViewModel = new List<ViewModelPage>();
    //                //加载所有ViewModel
    //                var applacationAssembly = Application.Current.GetType().GetTypeInfo().Assembly;

    //                foreach (var temp in applacationAssembly.DefinedTypes.Where(temp => temp.IsSubclassOf(typeof(ViewModelBase))))
    //                {
    //                    ViewModel.Add(new ViewModelPage(temp.AsType()));
    //                }

    //                foreach (var temp in applacationAssembly.DefinedTypes.Where(temp => temp.IsSubclassOf(typeof(Page))))
    //                {
    //                    //获取特性，特性有包含ViewModel
    //                    var p = temp.GetCustomAttribute<ViewModelAttribute>();

    //                    var viewmodel = ViewModel.FirstOrDefault(t => t.Equals(p?.ViewModel));
    //                    if (viewmodel != null)
    //                    {
    //                        viewmodel.Page = temp.AsType();
    //                    }
    //                }

    //                Composite = new List<Composite>();
    //                foreach (var temp in applacationAssembly.DefinedTypes.Where(temp => temp.IsSubclassOf(typeof(Composite))))
    //                {
    //                    Composite.Add((Composite)temp.AsType().GetConstructor(Type.EmptyTypes)?.Invoke(null));
    //                }
    //            }

    //            //Read();
    //        }

    //        public async Task Read()
    //        {
    //            FileSavePicker pick=new FileSavePicker();
    //            pick.FileTypeChoices.Add("txt",new List<string>(){".txt"});
    //            var file = await pick.PickSaveFileAsync();
    //            await FileIO.WriteTextAsync(file, Str);
    //        }
    //    }

    //    public abstract class NavigateViewModel : ViewModelBase, INavigato
    //    {
    //        public Frame Content
    //        {
    //            set;
    //            get;
    //        }

    //        public ViewModelBase this[string str]
    //        {
    //            get { return ViewModel.FirstOrDefault(temp => temp.Key == str)?.ViewModel; }
    //        }

    //        public List<ViewModelPage> ViewModel
    //        {
    //            set;
    //            get;
    //        }

    //        public async void Navigate(Type viewModel, object paramter)
    //        {
    //            _viewModel?.OnNavigatedFrom(null);
    //            ViewModelPage view = ViewModel.Find(temp => temp.Equals(viewModel));
    //            await view.Navigate(Content, paramter);
    //            view.ViewModel.Send += Receive;
    //            _viewModel = view.ViewModel;
    //        }
    //        //当前ViewModel
    //        private ViewModelBase _viewModel;
    //    }

    //    public class ViewModelAttribute : Attribute
    //    {
    //        public Type ViewModel { get; set; }
    //    }



    //    public class ViewModelPage : IEquatable<Type>
    //    {
    //        public ViewModelPage()
    //        {
    //            //if (ViewModel == null)
    //            //{
    //            //    //ViewModel=View.GetConstructor(null)
    //            //}
    //        }

    //        public ViewModelPage(Type viewModel)
    //        {
    //            _viewModel = viewModel;
    //            Key = _viewModel.Name;
    //        }

    //        public ViewModelPage(Type viewModel, Type page)
    //        {
    //            _viewModel = viewModel;
    //            Page = page;
    //            Key = _viewModel.Name;
    //        }

    //        public ViewModelPage(ViewModelBase viewModel, Type page)
    //        {
    //            ViewModel = viewModel;
    //            Page = page;
    //            Key = viewModel.GetType().Name;
    //            _viewModel = viewModel.GetType();
    //        }

    //        public string Key
    //        {
    //            set;
    //            get;
    //        }


    //        public ViewModelBase ViewModel
    //        {
    //            set;
    //            get;
    //        }

    //        public Type Page
    //        {
    //            set;
    //            get;
    //        }

    //        public async Task Navigate(Frame content, object paramter)
    //        {
    //            if (ViewModel == null)
    //            {
    //                ViewModel = (ViewModelBase)_viewModel.GetConstructor(Type.EmptyTypes).Invoke(null);
    //            }
    //            ViewModel.OnNavigatedTo(paramter);
    //#if NOGUI
    //            return;
    //#endif
    //            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
    //                () =>
    //                {
    //                    content.Navigate(Page, ViewModel);
    //                });
    //        }



    //        private Type _viewModel;

    //        protected bool Equals(ViewModelPage other)
    //        {
    //            return _viewModel == other._viewModel;
    //        }

    //        public override bool Equals(object obj)
    //        {
    //            if (ReferenceEquals(null, obj)) return false;
    //            if (ReferenceEquals(this, obj)) return true;
    //            if (obj.GetType() != this.GetType()) return false;
    //            return Equals((ViewModelPage)obj);
    //        }

    //        public override int GetHashCode()
    //        {
    //            return _viewModel?.GetHashCode() ?? 0;
    //        }

    //        public bool Equals(Type other)
    //        {
    //            return _viewModel == other;
    //        }
    //    }
    //    public interface INavigato
    //    {
    //        Frame Content
    //        {
    //            set;
    //            get;
    //        }

    //        void Navigate(Type viewModel, object parameter);
    //    }

    //    public abstract class ViewModelBase : ViewModelMessage, INavigable
    //    {
    //        /// <summary>
    //        /// 从其他页面跳转出
    //        /// 需要释放页面
    //        /// </summary>
    //        /// <param name="source"></param>
    //        /// <param name="e"></param>
    //        public abstract void OnNavigatedFrom(object obj);
    //        /// <summary>
    //        /// 从其他页面跳转到
    //        /// 在这里初始化页面
    //        /// </summary>
    //        /// <param name="source"></param>
    //        /// <param name="e"></param>
    //        public abstract void OnNavigatedTo(object obj);
    //    }

    //    /// <summary>
    //    /// 接收发送信息
    //    /// </summary>
    //    interface IAdapterMessage
    //    {
    //        /// <summary>
    //        /// 发送信息
    //        /// </summary>
    //        EventHandler<Message> Send { set; get; }
    //        /// <summary>
    //        /// 接收信息
    //        /// </summary> 
    //        /// <param name="source"></param>
    //        /// <param name="message"></param>
    //        void Receive(object source, Message message);
    //    }
    //    public interface INavigable
    //    {
    //        /// <summary>
    //        ///     不使用这个页面
    //        ///     清理页面
    //        /// </summary>
    //        /// <param name="obj"></param>
    //        void OnNavigatedFrom(object obj);

    //        /// <summary>
    //        ///     跳转到
    //        /// </summary>
    //        /// <param name="obj"></param>
    //        void OnNavigatedTo(object obj);
    //    }

    //    public abstract class ViewModelMessage : IAdapterMessage, INotifyPropertyChanged
    //    {
    //        /// <summary>
    //        /// 发送信息
    //        /// </summary>
    //        public EventHandler<Message> Send { get; set; }

    //        /// <summary>
    //        /// 接收信息
    //        /// </summary>
    //        /// <param name="source"></param>
    //        /// <param name="message"></param>
    //        public virtual void Receive(object source, Message message)
    //        {
    //            if (Composite != null)
    //            {
    //                foreach (var temp in Composite)
    //                {
    //                    if (message.GetType() == temp.Message)
    //                    {
    //                        temp.Run(this, message);
    //                    }
    //                }
    //            }
    //        }
    //        /// <summary>
    //        /// 命令合成
    //        /// 全部调用发送信息的处理在<see cref="Composite"/>
    //        /// </summary>
    //        protected static List<Composite> Composite { set; get; }

    //        public event PropertyChangedEventHandler PropertyChanged;

    //        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    //        {
    //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    //        }
    //    }

    //    public class Composite
    //    {
    //        public Type Message { get; set; }
    //        public string Key { get; set; }

    //        public virtual void Run(object sender, Message o)
    //        {

    //        }
    //    }

    //    internal class viewModel : NotifyProperty
    //    {
    //        public viewModel()
    //        {
    //            _m = new model(this);
    //            OnPropertyChanged("text");
    //            OnPropertyChanged("name");

    //            object temp;
    //            if (ApplicationData.Current.LocalSettings.Values.TryGetValue("width", out temp))
    //            {
    //                Width = temp as string;
    //            }
    //            else
    //            {
    //                Width = "20";
    //            }

    //            Advertisement = @"
    //作者：lindexi_gd
    //邮箱：lindexi_gd@163.com
    //博客地址：http://blog.csdn.net/lindexi_gd   在原博客看会有好的排版";
    //        }

    //        public string Text
    //        {
    //            set
    //            {
    //                _m._text = value;
    //                OnPropertyChanged();
    //            }
    //            get
    //            {
    //                return _m._text;
    //            }
    //        }

    //        public string Name
    //        {
    //            set
    //            {
    //                _m._name = value;
    //                OnPropertyChanged();
    //            }
    //            get
    //            {
    //                return _m._name;
    //            }
    //        }

    //        public bool Writetext
    //        {
    //            set
    //            {
    //                _m._writetext = value;
    //                OnPropertyChanged();
    //            }
    //            get
    //            {
    //                return _m._writetext;
    //            }
    //        }

    //        public string Addressfolder
    //        {
    //            set
    //            {
    //                OnPropertyChanged();
    //            }
    //            get
    //            {
    //                return _m.folder.Path;
    //            }
    //        }

    //        public string Width
    //        {
    //            set
    //            {
    //                try
    //                {
    //                    int temp;
    //                    temp = Convert.ToInt32(value);
    //                    _width = temp;
    //                    OnPropertyChanged();
    //                    ApplicationData.Current.LocalSettings.Values["width"] = value;
    //                }
    //                catch
    //                {
    //                }
    //            }
    //            get
    //            {
    //                return _width.ToString();
    //            }
    //        }

    //        public string Advertisement
    //        {
    //            set
    //            {
    //                _advertisement = value;
    //                OnPropertyChanged();
    //                ApplicationData.Current.LocalSettings.Values["advertisement"] = value;
    //            }
    //            get
    //            {
    //                if (string.IsNullOrEmpty(_advertisement))
    //                {
    //                    object temp;
    //                    if (ApplicationData.Current.LocalSettings.Values.TryGetValue("advertisement", out temp))
    //                    {
    //                        Advertisement = temp as string;
    //                    }
    //                    else
    //                    {
    //                        Advertisement = " ";
    //                    }
    //                }
    //                return _advertisement;
    //            }
    //        }

    //        public Action<int, int> Selectchange
    //        {
    //            set;
    //            get;
    //        }

    //        public async void Clipboard(TextControlPasteEventArgs e)
    //        {
    //            if (Writetext)
    //            {
    //                return;
    //            }

    //            e.Handled = true;
    //            DataPackageView con = Windows.ApplicationModel.DataTransfer.Clipboard.GetContent();
    //            string str = await _m.clipboard(con);
    //            Tianjia(str);
    //        }

    //        public async void Storage()
    //        {
    //            if (Writetext)
    //            {
    //                return;
    //            }
    //            await _m.storage();

    //            //_m.Current_Suspending(this, new object() as SuspendingEventArgs);  
    //        }

    //        public async void Dropimg(object sender, DragEventArgs e)
    //        {
    //            if (Writetext)
    //            {
    //                return;
    //            }
    //            DragOperationDeferral defer = e.GetDeferral();
    //            try
    //            {
    //                DataPackageView dataView = e.DataView;
    //                string str = await _m.clipboard(dataView);
    //                Tianjia(str);
    //            }
    //            finally
    //            {
    //                defer.Complete();
    //            }
    //        }

    //        public async void Accessfolder()
    //        {
    //            FolderPicker pick = new FolderPicker();
    //            pick.FileTypeFilter.Add("*");
    //            StorageFolder folder = await pick.PickSingleFolderAsync();
    //            if (folder != null)
    //            {
    //                _m.accessfolder(folder);
    //            }
    //            Addressfolder = string.Empty;
    //        }

    //        public async void file_open()
    //        {
    //            FileOpenPicker pick = new FileOpenPicker();
    //            //显示方式
    //            pick.ViewMode = PickerViewMode.Thumbnail;
    //            //选择最先的位置
    //            pick.SuggestedStartLocation =
    //                PickerLocationId.PicturesLibrary;
    //            //后缀名
    //            pick.FileTypeFilter.Add(".txt");
    //            pick.FileTypeFilter.Add(".md");

    //            StorageFile file = await pick.PickSingleFileAsync();

    //            if (file != null)
    //            {
    //                _m.open_file(file);
    //            }
    //        }

    //        public void Tianjia(string str)
    //        {
    //            int n;
    //            n = Select;
    //            int i;
    //            for (i = 0; (n > 0) && (i < Text.Length); i++)
    //            {
    //                if (Text[i] != '\r') //&& text[i] != '\n')
    //                {
    //                    n--;
    //                }
    //            }
    //            Text = Text.Insert(i, str);
    //            str = str.Replace("\r", "");
    //            n = Select + str.Length;
    //            if (n > Text.Length)
    //            {
    //                n = Text.Length;
    //            }
    //            Selectchange(n, 0);

    //            //string t = text.Replace("\r\n", "\n");
    //            //t = t.Insert(select, str);
    //            //text = t.Replace("\n", "\r\n");
    //        }

    //        private string _advertisement;

    //        private model _m;
    //        private int _width;

    //        public int Select;
    //    }
}