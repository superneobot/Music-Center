using MediaCenter.ViewModel;
using System;
using System.Windows.Media.Imaging;

namespace MediaCenter.Model {
    [Serializable]
    public class MusicFile : ViewModelBase {
        private int _id;
        public int Id {
            get { return _id; }
            set {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
        private string _title;
        /// <summary>
        /// Заголовок
        /// </summary>
        public string Title {
            get { return _title; }
            set {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        private string _artist;
        /// <summary>
        /// Исполнитель
        /// </summary>
        public string Artist {
            get { return _artist; }
            set {
                _artist = value;
                OnPropertyChanged(nameof(Artist));
            }
        }
        private string _album;
        /// <summary>
        /// Альбом
        /// </summary>
        public string Album {
            get { return _album; }
            set {
                _album = value;
                OnPropertyChanged(nameof(Album));
            }
        }
        private string _poster;
        /// <summary>
        /// Постер
        /// </summary>
        public string Poster {
            get { return _poster; }
            set {
                _poster = value;
                OnPropertyChanged(nameof(Poster));
            }
        }
        private string _duration;
        /// <summary>
        /// Длительность
        /// </summary>
        public string Duration {
            get { return _duration; }
            set {
                _duration = value;
                OnPropertyChanged(nameof(Duration));
            }
        }
        private string _filePath;
        /// <summary>
        /// Путь к файлу (локальный)
        /// </summary>
        public string FilePath {
            get { return _filePath; }
            set {
                _filePath = value;
                OnPropertyChanged(nameof(FilePath));
            }
        }
        private string _source;
        /// <summary>
        /// Путь к файлу (внешний)
        /// </summary>
        public string Source {
            get { return _source; }
            set {
                _source = value;
                OnPropertyChanged(nameof(Source));
            }
        }
        private string _url;
        /// <summary>
        /// Url файла
        /// </summary>
        public string URL {
            get { return _url; }
            set {
                _url = value;
                OnPropertyChanged(nameof(URL));
            }
        }
        private bool _liked;
        /// <summary>
        /// Статус файла (нравится или нет)
        /// </summary>
        public bool Liked {
            get { return _liked; }
            set {
                _liked = value;
                OnPropertyChanged(nameof(Liked));
            }
        }
        private BitmapSource likedState;
        /// <summary>
        /// Тест 
        /// </summary>
        public BitmapSource LikedState {
            get {
                return likedState;
            }
            set {
                likedState = value;
                OnPropertyChanged("LikedState");
            }
        }
        private bool _isPlay;
        /// <summary>
        /// Файл воспроизводится
        /// </summary>
        public bool IsPlay {
            get {
                return _isPlay;
            }
            set {
                _isPlay = value;
                OnPropertyChanged(nameof(IsPlay));
            }
        }
        private bool _isPaused;
        /// <summary>
        /// Файл на паузе
        /// </summary>
        public bool IsPaused {
            get {
                return _isPaused;
            }
            set {
                _isPaused = value;
                OnPropertyChanged(nameof(IsPaused));
            }
        }
        private Location _location;
        /// <summary>
        /// Расположение файла
        /// </summary>
        public Location Location {
            get {
                return _location;
            }
            set {
                _location = value;
                OnPropertyChanged(nameof(Location));
            }
        }
        private bool _download;
        /// <summary>
        /// Статус загрузки
        /// </summary>
        public bool Download {
            get {
                return _download;
            }
            set {
                _download = value;
                OnPropertyChanged(nameof(Download));
            }
        }
        public MusicFile() { }
        /// <summary>
        /// Добавление файла из сети
        /// </summary>
        /// <param name="title">Заголовок</param>
        /// <param name="artist">Артист</param>
        /// <param name="album">Альбом</param>
        /// <param name="poster">Постер</param>
        /// <param name="duration">Длительность</param>
        /// <param name="source">Путь к файлу (внешний)</param>
        /// <param name="url">Путь к странице с файлом</param>
        /// <param name="location">Расположение файла</param>
        public MusicFile(string title, string artist, string album, string poster, string duration, string source, string url, Location location) {
            _title = title;
            _artist = artist;
            _album = album;
            _poster = poster;
            _duration = duration;
            _source = source;
            _url = url;
            _location = location;
            _id++;
        }
        /// <summary>
        /// Добавление файла локально (тест)
        /// </summary>
        /// <param name="title"></param>
        /// <param name="artist"></param>
        /// <param name="filepath"></param>
        public MusicFile(string title, string artist, string filepath) {
            _title = title;
            _artist = artist;
            _filePath = filepath;
        }
        /// <summary>
        /// Добавление файла локально и чтение информации из его тега
        /// </summary>
        /// <param name="file">Путь к файлу</param>
        public MusicFile(string file) {
            getTitle(file);
            getArtist(file);
            getAlbum(file);
            getDuration(file);
            getPoster(file);
            getFilePath(file);
            getState(file);
            Location = Location.Local;
            _id++;
        }
        /// <summary>
        /// Чтение из тега заголовка
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        private void getTitle(string path) {
            var file = TagLib.File.Create(path);
            var title = file.Tag.Title;
            if (title == null)
                _title = System.IO.Path.GetFileNameWithoutExtension(path);
            else
                _title = file.Tag.Title.Replace("[muzmo.ru]", "");
        }
        /// <summary>
        /// Чтение из тега артиста
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        private void getArtist(string path) {
            var file = TagLib.File.Create(path);
            var art = file.Tag.FirstPerformer;
            if (art == null)
                _artist = "";
            else
                _artist = art.Replace("[mp3xa.cc]", "")
                    .Replace("&amp;", "&")
                    .Replace("[drivemusic.me]", "")
                    .Replace("[mp3xa.me]", "")
                    .Replace("[muzmo.ru]", "");
        }
        /// <summary>
        /// Чтение из тега альбома
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        private void getAlbum(string path) {
            var file = TagLib.File.Create(path);
            _album = file.Tag.Album;
        }
        /// <summary>
        /// Получить полный путь к файлу
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        private void getFilePath(string path) {
            _filePath = System.IO.Path.GetFullPath(path);
        }
        /// <summary>
        /// Получить из тега длительность
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        private void getDuration(string path) {
            var duration = "00:00";
            if (path != null) {
                var file = TagLib.File.Create(path);
                duration = file.Properties.Duration.ToString(@"mm\:ss");
            }
            _duration = duration;
        }
        /// <summary>
        /// Стандартный постер файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        private void getPoster(string path) {
            _poster = $@"/mp3_file.png";
            //_poster = "/play.png";
        }
        /// <summary>
        /// Проверка на лайк
        /// </summary>
        /// <param name="file">Путь к файлу</param>
        private void getState(string file) {
            if (Liked) {
                LikedState = new BitmapImage(new Uri(AppContext.BaseDirectory + @"/Images/like.png", UriKind.Absolute));
            } else {
                LikedState = null;
            }
        }

        //public override bool Equals(object obj) {
        //    var item = obj as MusicFile;
        //    if (item == null)
        //        return false;
        //    return this.FilePath == item.FilePath;
        //}

        //public override int GetHashCode() {
        //    return FilePath.GetHashCode();
        //}

        //public override bool Equals(object obj) {
        //    var item = obj as MusicFile;
        //    if (item == null) {
        //        return false;
        //    }
        //    return this.Id == item.Id;
        //}
        //public override int GetHashCode() {
        //    return this.Id.GetHashCode();
        //}
    }
}
