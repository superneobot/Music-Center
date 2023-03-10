using MediaCenter.ViewModel;
using System;
public enum SourceType {
    Music,
    Collection
}
namespace MediaCenter.Model {
    [Serializable]
    public class DataSource : ViewModelBase {
        private SourceType _type;
        public SourceType Type {
            get { return _type; }
            set {
                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }
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
        private string likedState;
        /// <summary>
        /// Тест 
        /// </summary>
        public string LikedState {
            get {
                if (_liked) {
                    likedState = @"/Resources/like.png";
                } else {
                    likedState = @"/Resources/not_like.png";
                }
                return likedState;
            }
            set {
                if (_liked) {
                    likedState = @"/Resources/like.png";
                } else {
                    likedState = @"/Resources/not_like.png";
                }
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
        private bool _localFile;
        public bool LocalFile {
            get {
                return _localFile;
            }
            set {
                _localFile = value;
                OnPropertyChanged(nameof(LocalFile));
            }
        }
        public DataSource(string title, string poster, string url) {
            Title = title;
            Poster = poster;
            FilePath = url;
            Type = SourceType.Collection;
        }
        public DataSource() { }
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
        public DataSource(string title, string artist, string album, string poster, string duration, string source, Location location) {
            _title = title;
            _artist = artist;
            _album = album;
            _poster = poster;
            _duration = duration;
            _filePath = source;
            // _url = url;
            _location = location;
            _id++;
            Type = SourceType.Music;
            LocalFile = false;
        }
        ///// <summary>
        ///// Добавление файла локально (тест)
        ///// </summary>
        ///// <param name="title"></param>
        ///// <param name="artist"></param>
        ///// <param name="filepath"></param>
        //public MusicFile(string title, string artist, string filepath) {
        //    _title = title;
        //    _artist = artist;
        //    _filePath = filepath;
        //}
        /// <summary>
        /// Добавление файла локально и чтение информации из его тега
        /// </summary>
        /// <param name="file">Путь к файлу</param>
        public DataSource(string file) {
            getTitle(file);
            getArtist(file);
            getAlbum(file);
            getDuration(file);
            getPoster(file);
            getFilePath(file);
            getState(file);
            LocalFile = true;
            Location = Location.Local;
            Type = SourceType.Music;
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
            _poster = $@"/Resources/music.png";
            //_poster = "";
        }
        /// <summary>
        /// Проверка на лайк
        /// </summary>
        /// <param name="file">Путь к файлу</param>
        private void getState(string file) {
            if (Liked) {
                LikedState = @"/Resources/like.png";
            } else {
                LikedState = @"/Resources/not_like.png";
            }
        }

        public override bool Equals(object obj) {
            var item = obj as DataSource;
            if (item == null)
                return false;
            return this.Title == item.Title | this.Artist == item.Artist;
        }



        public override int GetHashCode() {
            return this.Title.GetHashCode();
        }

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
