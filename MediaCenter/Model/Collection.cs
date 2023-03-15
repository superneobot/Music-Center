using MediaCenter.ViewModel;
using System.Collections.ObjectModel;

namespace MediaCenter.Model {
    public class Collection : ViewModelBase {
        private string _title;
        public string Title {
            get {
                return _title;
            }
            set {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        private string _avatar;
        public string Avatar {
            get {
                return _avatar;
            }
            set {
                _avatar = value;
                OnPropertyChanged(nameof(Avatar));
            }
        }
        private ObservableCollection<DataSource> _list;
        public ObservableCollection<DataSource> List {
            get {
                return _list;
            }
            set {
                _list = value;
                OnPropertyChanged(nameof(List));
            }
        }
        public Collection() { }
        public Collection(string title, string avatar, ObservableCollection<DataSource> list) {
            Title = title;
            Avatar = avatar;
            List = list;
        }


    }


}
