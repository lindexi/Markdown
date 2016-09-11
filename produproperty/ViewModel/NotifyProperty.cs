// lindexi
// 20:47

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace produproperty.ViewModel
{
    /// <summary>
    ///     提供继承通知UI改变值
    /// </summary>
    public class NotifyProperty : INotifyPropertyChanged
    {
        public NotifyProperty()
        {
            _reminder = new StringBuilder();
        }

        /// <summary>
        ///     一直添加value
        /// </summary>
        public string Reminder
        {
            set
            {
                _reminder.Clear();
                _reminder.Append(value);
                OnPropertyChanged();
            }
            get
            {
                return _reminder.ToString();
            }
        }

        public void UpdateProper<T>(ref T properValue, T newValue, [CallerMemberName] string properName = "")
        {
            if (Equals(properValue, newValue))
            {
                return;
            }

            properValue = newValue;
            if (properName != null)
            {
                OnPropertyChanged(properName);
            }
        }


        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private readonly StringBuilder _reminder;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}