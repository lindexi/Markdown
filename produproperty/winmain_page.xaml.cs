using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using produproperty.ViewModel;
namespace produproperty
{
    public partial class winmain_page : Page
    {
        private winmain view;
        public winmain_page()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Windows.Storage.StorageFolder)
            {
                view=new winmain(e.Parameter as Windows.Storage.StorageFolder);
            }
            else
            {
                view=new winmain(null);
            }
            view.selectchange = select;
            //base.OnNavigatedTo(e);
        }

        private void select(int index, int length)
        {
            text.SelectionStart = index;
            text.SelectionLength = length;
        }
    }
}
