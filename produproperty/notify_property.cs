﻿using System.ComponentModel;
using System.Text;

namespace ViewModel
{
    /// <summary>
    /// 提供继承通知UI改变值
    /// </summary>
    public class notify_property : INotifyPropertyChanged
    {
        public notify_property()
        {
            _reminder = new StringBuilder();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 一直添加value
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

        public void UpdateProper<T>(ref T properValue, T newValue, [System.Runtime.CompilerServices.CallerMemberName] string properName = "")
        {
            if (object.Equals(properValue, newValue))
                return;

            properValue = newValue;
            OnPropertyChanged(properName);
        }
        public void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }


        private StringBuilder _reminder;
    }
}
