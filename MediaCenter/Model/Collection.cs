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
        private string _url;
        public string Url {
            get {
                return _url;
            }
            set {
                _url = value;
                OnPropertyChanged(nameof(Url));
            }
        }
        private string _poster;
        public string Poster {
            get {
                return _poster;
            }
            set {
                _poster = value;
                OnPropertyChanged(nameof(Poster));
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
        public Collection(string title, string url, string poster) {
            Title = title;
            Url = url;
            Poster = poster;
            //List = list;
        }


    }


}
