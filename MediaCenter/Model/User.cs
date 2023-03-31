using MediaCenter.ViewModel;
using System.Drawing;

namespace MediaCenter.Model {
    public class User : ViewModelBase {
        private int _id;
        public int Id {
            get { return _id; }
            set {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }

        }
        private string _login;
        public string Login {
            get { return _login; }
            set {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }
        private string _password;
        public string Password {
            get { return _password; }
            set {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        private Image _avatar;
        public Image Avatar {
            get { return _avatar; }
            set {
                _avatar = value;
                OnPropertyChanged(nameof(Avatar));
            }
        }

        public User() { }
        public User(string login, string password) {
            Login = login;
            Password = password;
            string username = System.Environment.UserName;
            Avatar = MediaCenter.Models.Avatar.GetUserTile(username);
        }
    }
}
