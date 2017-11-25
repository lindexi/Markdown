using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using lindexi.uwp.Framework.ViewModel;
using produproperty.ViewModel;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace produproperty.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    [ViewModel(ViewModel = typeof(KaydenSergioModel))]
    public sealed partial class KaydenSergioPage : Page
    {
        public KaydenSergioPage()
        {
            this.InitializeComponent();
            ViewModel = (KaydenSergioModel) DataContext;
        }

        public KaydenSergioModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = (KaydenSergioModel) e.Parameter;
            DataContext = e.Parameter;

            base.OnNavigatedTo(e);
        }
    }
}
