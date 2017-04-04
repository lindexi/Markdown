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
    [ViewModel(ViewModel = typeof(MartinRhfinwittModel))]
    public sealed partial class MartinRhfinwittPage : Page
    {
        public MartinRhfinwittPage()
        {
            this.InitializeComponent();
            ViewModel = (MartinRhfinwittModel) DataContext;
            ViewModel.AlexzanderfFrame = AlexzanderPage;
            ViewModel.KaydenSergioFrame = KaydenSergioPage;
            ViewModel.TrenPhillipKarissaFrame = TrenPhillipKarissaPage;
            ViewModel.Read();
        }

        public MartinRhfinwittModel ViewModel { get; set; }
    }

    class BooleanVisibilityConvert:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value as bool? == true)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
