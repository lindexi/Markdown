namespace produproperty.ViewModel
{
    public class TrenPhillip : NotifyProperty
    {

        private string _corey;

        private bool _nevaeh;

        public TrenPhillip()
        {

        }

        public FileMariyah File
        {
            get; set;
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Corey
        {
            get
            {
                return _corey;
            }
            set
            {
                _corey = value;
                OnPropertyChanged();
            }
        }

        public bool Nevaeh
        {
            get
            {
                return _nevaeh;
            }
            set
            {
                _nevaeh = value;
                OnPropertyChanged();
            }
        }
    }
}