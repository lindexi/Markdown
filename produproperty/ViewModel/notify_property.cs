using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace produproperty.ViewModel
{
    /// <summary>
    ///     提供继承通知UI改变值
    /// </summary>
    public class notify_property : INotifyPropertyChanged
    {
        private readonly StringBuilder _reminder;

        public notify_property()
        {
            _reminder = new StringBuilder();
        }

        /// <summary>
        ///     一直添加value
        /// </summary>
        public string reminder
        {
            set
            {
                _reminder.Clear();
                _reminder.Append(value);
                OnPropertyChanged("reminder");
            }
            get
            {
                return _reminder.ToString();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void UpdateProper<T>(ref T properValue, T newValue, [CallerMemberName] string properName = "")
        {
            if (Equals(properValue, newValue))
                return;

            properValue = newValue;
            if (properName != null)
            {
                OnPropertyChanged(name: properName);
            }
        }


        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}