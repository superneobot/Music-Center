using AngleSharp;
using MediaCenter.Model;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
public enum CenterState {
    Playlist = 0,
    Downloaded = 1,
    Search = 2,
    Favorite = 3,
    Radio = 4
}
namespace MediaCenter.ViewModel {
    public class MainViewModel : ViewModelBase {
        #region collections
        private ObservableCollection<MusicFile> _currentPlayList;
        public ObservableCollection<MusicFile> CurrentPlayList {
            get {
                return _currentPlayList;
            }
            set {
                _currentPlayList = value;
                OnPropertyChanged(nameof(CurrentPlayList));
            }
        }
        private ObservableCollection<MusicFile> _musicFiles;
        public ObservableCollection<MusicFile> MusicFiles {
            get { return _musicFiles; }
            set {
                _musicFiles = value;
                OnPropertyChanged(nameof(MusicFiles));
            }
        }
        private ObservableCollection<MusicFile> _downloadedFiles;
        public ObservableCollection<MusicFile> DownloadedFiles {
            get { return _downloadedFiles; }
            set {
                _downloadedFiles = value;
                OnPropertyChanged(nameof(DownloadedFiles));
            }
        }
        private ObservableCollection<MusicFile> _searchList;
        public ObservableCollection<MusicFile> SearchList {
            get { return _searchList; }
            set {
                _searchList = value;
                OnPropertyChanged(nameof(SearchList));
            }
        }
        private ObservableCollection<MusicFile> _favoriteList;
        public ObservableCollection<MusicFile> FavoriteList {
            get { return _favoriteList; }
            set {
                _favoriteList = value;
                OnPropertyChanged(nameof(FavoriteList));
            }
        }
        public ObservableCollection<MusicFile> RadioList;

        #endregion
        #region program_param
        private double _progress = 0;
        public double LoadProgress {
            get {
                if (_progress == 0) {
                    _progress = 0;
                }
                return _progress;
            }
            set {
                _progress = value;
                OnPropertyChanged(nameof(LoadProgress));
            }
        }
        private double _loadMaximum;
        public double LoadMaximum {
            get {
                return _loadMaximum;
            }
            set {
                _loadMaximum = value;
                OnPropertyChanged(nameof(LoadMaximum));
            }
        }
        private bool isTopMost;
        public bool IsTopMost {
            get { return isTopMost; }
            set {
                isTopMost = value;
                OnPropertyChanged(nameof(IsTopMost));
            }
        }
        private Visibility _clearVisible;
        public Visibility ClearVisible {
            get {
                return _clearVisible;
            }
            set {
                _clearVisible = value;
                OnPropertyChanged(nameof(ClearVisible));
            }
        }
        private string _state;
        public string State {
            get {
                return _state;
            }
            set {
                _state = value;
                OnPropertyChanged(nameof(State));
            }
        }
        private CenterState CenterState { get; set; } = CenterState.Playlist;
        private DispatcherTimer Timer;
        private int _allpages;
        private object _url = "https://ru.hitmotop.com/search?q=";
        private string _pageTitle;
        public string PageTitle {
            get {
                return _pageTitle;
            }
            set {
                _pageTitle = value;
                OnPropertyChanged(nameof(PageTitle));
            }
        }
        #endregion
        #region app_main_param
        //
        private bool IsPlay = false;
        public Player Player;
        //
        private string _searchText;
        public string SearchText {
            get { return _searchText; }
            set {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }
        private MusicFile _selectedMusic;
        public MusicFile SelectedMusic {
            get {
                return _selectedMusic;
            }
            set {
                _selectedMusic = value;
                OnPropertyChanged(nameof(SelectedMusic));
            }
        }
        private int _selectedMusicIndex;
        public int SelectedMusicIndex {
            get {
                return _selectedMusicIndex;
            }
            set {
                _selectedMusicIndex = value;
                OnPropertyChanged(nameof(SelectedMusicIndex));
            }
        }
        private MusicFile _playingMusic;
        public MusicFile PlayingMusic {
            get {
                return _playingMusic;
            }
            set {
                _playingMusic = value;
                OnPropertyChanged(nameof(PlayingMusic));
            }
        }
        #endregion
        #region media_center_settings
        public string Title {
            get {
                return PlayingMusic?.Title;
            }
        }
        public string Artist {
            get {
                return PlayingMusic?.Artist;
            }
        }
        public string Album {
            get {
                return PlayingMusic?.Album;
            }
        }
        private long _value;
        public long Value {
            get {
                return _value;
            }
            set {
                _value = value;
                OnPropertyChanged(nameof(Value));
                //Player.setPosition(value);
            }
        }
        private string _timeTip;
        public string TimeTip {
            get {
                return _timeTip;
            }
            set {
                _timeTip = value;
                OnPropertyChanged(nameof(TimeTip));
            }
        }
        private long _maximum;
        public long Maximum {
            get {
                if (_maximum == 0) {
                    _maximum = 1;
                }
                return _maximum;
            }
            set {
                _maximum = value;
                OnPropertyChanged(nameof(Maximum));
            }
        }
        private string _time;
        public string Time {
            get {
                if (_time == null)
                    _time = "00:00";
                return _time = Player.getPositionString();
            }
            set {
                _time = value;
                _time = Player.getPositionString();
                OnPropertyChanged(nameof(Time));
            }
        }
        private string _duration;
        public string Duration {
            get {
                if (_duration == null) {
                    _duration = "00:00";
                }
                return _duration;
            }
            set {
                _duration = Player.getTotalTimeString();
                OnPropertyChanged(nameof(Duration));
            }
        }
        private string _playBtn;
        public string PlayBtn {
            get {
                if (Player.State == NAudio.Wave.PlaybackState.Stopped) {
                    _playBtn = "/play.png";// new BitmapImage(new Uri(AppContext.BaseDirectory + "Images/play.png"));
                }
                return _playBtn;
            }
            set {
                _playBtn = value;
                OnPropertyChanged(nameof(PlayBtn));
            }
        }
        private string _thubmPlayText;
        public string ThumbPlayText {
            get {
                if (_thubmPlayText == null) {
                    _thubmPlayText = "Play";
                }
                return _thubmPlayText;
            }
            set {
                _thubmPlayText = value;
                OnPropertyChanged(nameof(ThumbPlayText));
            }
        }
        private string _playingFile;
        public string PlayingFile {
            get {
                if (_playingFile == null) {
                    _playingFile = "Media Center";
                }
                return _playingFile;
            }
            set {
                _playingFile = value;
                OnPropertyChanged(nameof(PlayingFile));
            }
        }
        private long _volume;
        public long Volume {
            get {
                return _volume;
            }
            set {
                _volume = value;
                var s = (float)_volume / 100;
                Player.setVolume(s);
                OnPropertyChanged(nameof(Volume));
            }
        }
        private int _searchProgress;
        public int SearchProgress {
            get {
                if (_searchProgress == 0) {
                    _searchProgress = 0;
                }
                return _searchProgress;
            }
            set {
                _searchProgress = value;
                OnPropertyChanged(nameof(SearchProgress));
            }
        }
        private int _searchMax;
        private int _history_index;
        private MusicFile download_file;

        public int SearchMax {
            get {
                if (_searchMax == 0) {
                    _searchMax = 0;
                }
                return _searchMax;
            }
            set {
                _searchMax = value;
                OnPropertyChanged(nameof(SearchMax));
            }
        }
        #endregion
        #region commands
        public ICommand OpenPlayList {
            get {
                return new RelayCommand<ObservableCollection<MusicFile>>((e) => {
                    CurrentPlayList = MusicFiles;
                    State = "Плейлист";
                    CenterState = CenterState.Playlist;
                    ClearVisible = Visibility.Visible;
                    Check(MusicFiles, PlayingMusic);
                });
            }
        }
        public ICommand OpenDownloadedList {
            get {
                return new RelayCommand<ObservableCollection<MusicFile>>((e) => {
                    CurrentPlayList = DownloadedFiles;
                    //Update(); 
                    State = "Загруженное";
                    CenterState = CenterState.Downloaded;
                    ClearVisible = Visibility.Visible;
                    Check(DownloadedFiles, PlayingMusic);
                });
            }
        }
        public ICommand OpenSearchList {
            get {
                return new RelayCommand<ObservableCollection<MusicFile>>((e) => {
                    CurrentPlayList = SearchList;
                    State = "Поиск треков";
                    CenterState = CenterState.Search;
                    ClearVisible = Visibility.Visible;
                    Check(SearchList, PlayingMusic);
                });
            }
        }
        public ICommand OpenFavoriteList {
            get {
                return new RelayCommand<ObservableCollection<MusicFile>>((e) => {

                    CurrentPlayList = FavoriteList;
                    State = "Мне нравится";
                    CenterState = CenterState.Favorite;
                    ClearVisible = Visibility.Hidden;
                    Check(FavoriteList, PlayingMusic);
                });
            }
        }
        public ICommand OpenRadioPage {
            get {
                return new RelayCommand<ObservableCollection<MusicFile>>((e) => {
                    CurrentPlayList = RadioList;
                    State = "Радио";
                    CenterState = CenterState.Radio;
                    ClearVisible = Visibility.Hidden;
                });
            }
        }
        public ICommand OpenSettingsPage {
            get {
                return new RelayCommand<MusicFile>((e) => { });
            }
        }
        public ICommand AddFile {
            get {
                return new RelayCommand<MusicFile>((e) => AddingFile());
            }
        }
        public ICommand UpdateList {
            get {
                return new RelayCommand<ObservableCollection<MusicFile>>((e) => Update(DownloadedFiles));
            }
        }
        public ICommand ClearList {
            get {
                return new RelayCommand<ObservableCollection<MusicFile>>((e) => {
                    if (CenterState == CenterState.Playlist)
                        MusicFiles.Clear();
                    if (CenterState == CenterState.Downloaded) {
                        var result = MessageBox.Show("Это действие удалит загруженные файлы с диска.\nВыполнить?","Music Center",MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
                        if (result == MessageBoxResult.OK) {
                            Directory.Delete(AppContext.BaseDirectory + @"Downloaded", true);
                            DownloadedFiles.Clear();
                            MessageBox.Show("Готово!", "Music Center", MessageBoxButton.OK, MessageBoxImage.Information);
                        } else {
                            return;
                        }
                    }
                    if(CenterState == CenterState.Search) {
                        SearchList.Clear();
                    }
                });
            }
        }
        public ICommand PlayFile {
            get {
                return new RelayCommand(item => { Play(SelectedMusic); });
            }
        }
        public RelayCommand PlayFileFromList {
            get {
                return new RelayCommand(item => {
                    PlayClick(SelectedMusic);
                });
            }
        }
        public ICommand ClearBox {
            get {
                return new RelayCommand<string>(item => {
                    SearchText = "";
                });
            }
        }
        public ICommand PlayNext {
            get {
                return new RelayCommand((e) => {
                    if (CurrentPlayList.Count > 0) {
                        _history_index = SelectedMusicIndex;
                        if (SelectedMusicIndex == CurrentPlayList.Count - 1)
                            SelectedMusicIndex = 0;
                        else
                            SelectedMusicIndex++;
                        var file = CurrentPlayList[SelectedMusicIndex];
                        Play(file);

                    }
                });
            }
        }
        public ICommand PlayPrev {
            get {
                return new RelayCommand((e) => {
                    if (_history_index != 0)
                        if (SelectedMusicIndex == CurrentPlayList.Count - 1)
                            SelectedMusicIndex--;
                        else
                            SelectedMusicIndex = CurrentPlayList.Count - 1;
                    else
                        SelectedMusicIndex = _history_index;
                    var file = CurrentPlayList[SelectedMusicIndex];
                    Play(file);
                });
            }
        }
        public ICommand Search {
            get {
                return new RelayCommand<string>(Text => {
                    SearchText = (string)Text;
                    FindMusic(SearchText);
                });
            }
        }
        public ICommand SearchClick {
            get {
                return new RelayCommand(Text => {
                    var box = Text as TextBox;
                    box.Clear();
                });
            }
        }
        public ICommand AddToFavoriteList {
            get {
                return new RelayCommand<MusicFile>(item => {
                    TrackIsFavorite(SelectedMusic);
                    SaveFavoriteList();
                });
            }
        }
        public ICommand Seek {
            get {
                return new RelayCommand(e => {
                    var tick = e;
                    Player.setPosition(Value);
                });
            }
        }
        public ICommand MakeTopMost {
            get {
                return new RelayCommand(e => {
                    if (IsTopMost) {
                        IsTopMost = false;
                    } else {
                        IsTopMost = true;
                    }
                });
            }
        }
        public ICommand Minimize {
            get {
                return new RelayCommand(e => {
                    Window win = new MainWindow();
                    win.WindowState = WindowState.Minimized;
                });
            }
        }
        public ICommand Maximize {
            get {
                return new RelayCommand(e => {

                });
            }
        }
        public ICommand Exit {
            get {
                return new RelayCommand(e => {
                    Process.GetCurrentProcess().Kill();
                });
            }
        }
        public ICommand DownloadMusic {
            get {
                return new RelayCommand((e) => {
                    if (Directory.Exists(AppContext.BaseDirectory + @"Downloaded"))
                        DownloadSong(SelectedMusic);
                    else {
                        Directory.CreateDirectory(AppContext.BaseDirectory + @"Downloaded");
                        DownloadSong(SelectedMusic);
                    }
                });
            }
        }
        #endregion
        #region app_voids
        private void DownloadSong(MusicFile file) {
            file.Download = true;
            WebClient client = new WebClient();
            var file_path = AppContext.BaseDirectory + @"Downloaded\" + Path.GetFileName(file.Source);
            //if (client.IsBusy) return;
            LoadProgress = 0;

            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
            client.DownloadFileAsync(new Uri(file.Source), file_path);
            download_file = file;
        }
        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            LoadProgress = int.Parse(Math.Truncate(percentage).ToString());

        }
        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e) {
            download_file.Download = false;
        }
        private void SaveFavoriteList() {
            var file = JsonConvert.SerializeObject(FavoriteList, Formatting.Indented);
            File.WriteAllText(AppContext.BaseDirectory + "favorite.json", file);
        }
        private void LoadFavoriteList() {
            if (File.Exists(AppContext.BaseDirectory + "favorite.json")) {
                var file = File.ReadAllText(AppContext.BaseDirectory + "favorite.json");
                FavoriteList = JsonConvert.DeserializeObject<ObservableCollection<MusicFile>>(file);
            }
        }
        private void TrackIsFavorite(MusicFile file) {
            if (file.Liked == false) {
                file.Liked = true;
                FavoriteList.Add(file);
            } else {
                file.Liked = false;
                FavoriteList.Remove(file);
            }
        }
        private void AddingFile() {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Все музыкальные форматы (*.mp3, *.flac)|*.mp3;*.flac|MP3 музыка (*.mp3)|*.mp3|FLAC музыка (*.flac)|*.flac";
            ofd.Multiselect = true;
            ofd.RestoreDirectory = true;
            var result = ofd.ShowDialog();
            if (result == true) {
                var files = ofd.FileNames;
                foreach (var item in files) {
                    MusicFiles.Add(new MusicFile(item));
                }
            }
            CurrentPlayList = MusicFiles;
            State = "Плейлист";
            CenterState = CenterState.Playlist;
            ClearVisible = Visibility.Visible;
        }
        private void Update(ObservableCollection<MusicFile> source) {
            source.Clear();
            string extension = ".mp3";
            var temp = new ObservableCollection<MusicFile>();
            if (Directory.Exists(AppContext.BaseDirectory + @"\Downloaded")) {
                DirectoryInfo dir = new DirectoryInfo(AppContext.BaseDirectory + @"\Downloaded");
                FileInfo[] files = dir.GetFiles("*" + extension, SearchOption.AllDirectories);

                foreach (FileInfo file in files) {
                    var item = new MusicFile(file.FullName);
                    var result = source.Contains(item);
                    if (!result)
                        source.Add(item);
                }

                //foreach(var item in temp) {
                //    if(!source.Contains(item))
                //        source.Add(item);
                //    else
                //        source.Remove(item);
                //}
            }
        }
        private async void PlayClick(MusicFile file) {
            if (file == null) return;
            await Player.Stop(); IsPlay = false;
            if (Player.State == NAudio.Wave.PlaybackState.Stopped) {
                if (CenterState == CenterState.Playlist | CenterState == CenterState.Downloaded) {
                    Player.Play(file.FilePath);
                    PlayBtn = "/pause.png";//new BitmapImage(new Uri(AppContext.BaseDirectory + "Images/pause.png"));
                    ThumbPlayText = "Pause";
                    Duration = Player?.getTotalTimeString();
                    Time = Player.getPositionString();
                    Maximum = Player.getTotalTime();
                    IsPlay = true;
                } else if (CenterState == CenterState.Search) {
                    Player.Play(file.URL);
                    PlayBtn = "/pause.png";//new BitmapImage(new Uri(AppContext.BaseDirectory + "Images/pause.png"));
                    ThumbPlayText = "Pause";
                    Duration = Player?.getTotalTimeString();
                    Time = Player.getPositionString();
                    Maximum = Player.getTotalTime();
                    IsPlay = true;
                } else if (CenterState == CenterState.Favorite) {
                    if (file.URL == null)
                        Player.Play(file.FilePath);
                    else
                        Player.Play(file.URL);
                    PlayBtn = "/pause.png";// new BitmapImage(new Uri(AppContext.BaseDirectory + "Images/pause.png"));
                    ThumbPlayText = "Pause";
                    Duration = Player?.getTotalTimeString();
                    Time = Player.getPositionString();
                    Maximum = Player.getTotalTime();
                    IsPlay = true;
                }
            }

            PlayingMusic = file;
            PlayingMusic.IsPlay = true;
            Check(CurrentPlayList, PlayingMusic);
            PlayingFile = $"{PlayingMusic.Title} - {PlayingMusic.Artist}";
        }
        private void Check(ObservableCollection<MusicFile> source, MusicFile target) {
            foreach (var item in source) {
                if (Player.State == NAudio.Wave.PlaybackState.Playing) {
                    if (item != target) {
                        item.IsPlay = false;
                        item.IsPaused = false;
                    }
                }
                if (Player.State == NAudio.Wave.PlaybackState.Paused) {
                    if (item != target) {
                        item.IsPaused = false;
                    }
                }
                if (Player.State == NAudio.Wave.PlaybackState.Stopped) {
                    item.IsPlay = false;
                    item.IsPaused = false;
                }
            }
        }
        private async void Play(MusicFile file) {
            if (CurrentPlayList.Count == 0) return;
            //file = CurrentPlayList[0];
            if (file == null) return;
            //
            if (Player.State == NAudio.Wave.PlaybackState.Playing & file == PlayingMusic) {
                if (file == PlayingMusic) {
                    await Player.Pause();
                    ThumbPlayText = "Play";
                    PlayBtn = "/play.png";// new BitmapImage(new Uri( "/play.png", UriKind.Absolute));
                    IsPlay = false;
                    PlayingMusic.IsPlay = false;
                    PlayingMusic.IsPaused = true;
                } else {
                    await Player.Stop(); IsPlay = false;
                    if (CenterState == CenterState.Playlist | CenterState == CenterState.Downloaded) {
                        Player.Play(file.FilePath);
                        PlayBtn = "/pause.png";// new BitmapImage(new Uri( "/pause.png", UriKind.Absolute));
                        ThumbPlayText = "Pause";
                        Duration = Player?.getTotalTimeString();
                        Time = Player.getPositionString();
                        Maximum = Player.getTotalTime();
                        IsPlay = true;
                    } else if (CenterState == CenterState.Search) {
                        Player.Play(file.URL);
                        PlayBtn = "/pause.png";// new BitmapImage(new Uri( "/pause.png", UriKind.Absolute));
                        ThumbPlayText = "Pause";
                        Duration = Player?.getTotalTimeString();
                        Time = Player.getPositionString();
                        Maximum = Player.getTotalTime();
                        IsPlay = true;
                    } else if (CenterState == CenterState.Favorite) {
                        if (file.URL == null)
                            Player.Play(file.FilePath);
                        else
                            Player.Play(file.URL);
                        PlayBtn = "/pause.png";//new BitmapImage(new Uri( "/pause.png", UriKind.Absolute));
                        ThumbPlayText = "Pause";
                        Duration = Player?.getTotalTimeString();
                        Time = Player.getPositionString();
                        Maximum = Player.getTotalTime();
                        IsPlay = true;
                    }
                    PlayingMusic = file;
                    PlayingMusic.IsPaused = false;
                    PlayingMusic.IsPlay = true;
                    Check(CurrentPlayList, PlayingMusic);
                }
            } else if (Player.State == NAudio.Wave.PlaybackState.Paused & file == PlayingMusic) {
                await Player.Resume();
                ThumbPlayText = "Pause";
                PlayBtn = "/pause.png";//new BitmapImage(new Uri( "/pause.png", UriKind.Absolute));
                IsPlay = true;
                PlayingMusic.IsPlay = true;
                PlayingMusic.IsPaused = false;
            } else if (Player.State == NAudio.Wave.PlaybackState.Stopped | file != PlayingMusic) {
                await Player.Stop();
                file.IsPlay = false;
                file.IsPaused = false;
                if (CenterState == CenterState.Playlist | CenterState == CenterState.Downloaded) {
                    Player.Play(file.FilePath);
                    PlayBtn = "/pause.png";//new BitmapImage(new Uri("/pause.png", UriKind.Absolute));
                    ThumbPlayText = "Pause";
                    Duration = Player?.getTotalTimeString();
                    Time = Player.getPositionString();
                    Maximum = Player.getTotalTime();
                    IsPlay = true;
                } else if (CenterState == CenterState.Search) {
                    Player.Play(file.URL);
                    PlayBtn = "/pause.png";//new BitmapImage(new Uri( "/pause.png", UriKind.Absolute));
                    ThumbPlayText = "Pause";
                    Duration = Player?.getTotalTimeString();
                    Time = Player.getPositionString();
                    Maximum = Player.getTotalTime();
                    IsPlay = true;
                } else if (CenterState == CenterState.Favorite) {
                    if (file.URL == null)
                        Player.Play(file.FilePath);
                    else
                        Player.Play(file.URL);
                    PlayBtn = "/pause.png";//new BitmapImage(new Uri( "/pause.png", UriKind.Absolute));
                    ThumbPlayText = "Pause";
                    Duration = Player?.getTotalTimeString();
                    Time = Player.getPositionString();
                    Maximum = Player.getTotalTime();
                    IsPlay = true;
                }
                PlayingMusic = file;
                PlayingMusic.IsPaused = false;
                PlayingMusic.IsPlay = true;
                Check(CurrentPlayList, PlayingMusic);
            }

        }
        private async void FindMusic(string text) {
            SearchList.Clear();
            await GetPages(text);
            SearchProgress = 0;
            SearchMax = _allpages / 48;
            for (int i = 0; i <= _allpages; i = i + 48) {
                await FindMusic(i, text);
                SearchProgress++;
            }
            //SearchText += $" [{ SearchList.Count}]";
            if (CurrentPlayList != SearchList) {
                CurrentPlayList = SearchList;
                State = "Поиск треков";
                CenterState = CenterState.Search;
                ClearVisible = Visibility.Hidden;
            }
        }
        private async Task FindMusic(int page, string search_text) {
            var config = Configuration.Default.WithDefaultLoader();
            var text = search_text.Replace(" ", "+");
            var address = $"https://ru.hitmotop.com/search/start/{page}?q={text}";
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(address);
            try {
                var cells = document.QuerySelector("ul[class='tracks__list']");
                if (cells != null) {
                    var founds = cells.QuerySelectorAll("li[class='tracks__item track mustoggler']");
                    foreach (var item in founds) {
                        var atitle = item.QuerySelector("div[class='track__info']");
                        var title = atitle.QuerySelector("div[class='track__title']").TextContent.Trim();
                        var aartist = item.QuerySelector("div[class='track__info']");
                        var artist = aartist.QuerySelector("div[class='track__desc']").TextContent;
                        var aload = item.QuerySelector("a[class='track__download-btn']");
                        var link = aload.GetAttribute("href");
                        var image_block = item.QuerySelector("div[class='track__img']");
                        var image_a = image_block.GetAttribute("style");
                        var image_url_small = "https://ru.hitmotop.com" + image_a.Replace("background-image: url('", " ").Replace("');", " ").Trim();
                        var atime = item.QuerySelector("div[class='track__info-r']");
                        var time = atime.QuerySelector("div[class='track__fulltime']").TextContent;
                        var file = new MusicFile(title, artist, "", image_url_small, time, link, link, Location.Internet);
                        SearchList.Add(file);
                    }
                } else {
                    MessageBox.Show("Ничего не найдено");
                }
            } catch {
            }
        }
        private async Task GetPages(string search_text) {
            try {
                var config = Configuration.Default.WithDefaultLoader();
                var text = search_text.Replace(" ", "+");
                var address = $"https://ru.hitmotop.com/search/start/{0}?q={text}";
                var context = BrowsingContext.New(config);
                var document = await context.OpenAsync(address);
                //
                var paginator = document.QuerySelector("section[class='pagination']");
                if (paginator != null) {
                    var pages = paginator.QuerySelectorAll("li");
                    var last_page = pages.Take(pages.Length).LastOrDefault();
                    var count_str = last_page.InnerHtml;
                    int count = 1;
                    int.TryParse(string.Join("", count_str.Where(c => char.IsDigit(c))), out count);
                    _allpages = count;
                } else {
                    _allpages = 0;
                }
            } catch {
                _allpages = 0;
            }
        }
        #endregion
        /*---------------------------------------------------------------------*/
        public MainViewModel() {
            //
            Player = new Player();
            CurrentPlayList = new ObservableCollection<MusicFile>();
            MusicFiles = new ObservableCollection<MusicFile>();
            DownloadedFiles = new ObservableCollection<MusicFile>();
            SearchList = new ObservableCollection<MusicFile>();
            FavoriteList = new ObservableCollection<MusicFile>();
            RadioList = new ObservableCollection<MusicFile>();
            Update(DownloadedFiles);
            LoadFavoriteList();
            //
            CurrentPlayList = MusicFiles;
            State = "Плейлист";
            SearchText = "Поиск трека или исполнителя";
            ClearVisible = Visibility.Visible;
            Volume = 100;
            //
            Timer = new DispatcherTimer(DispatcherPriority.Background);
            Timer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            Timer.Tick += delegate {
                Time = Player.getPositionString();
                //Value = Player.getPosition();
                PageTitle = $"{Player.State}";
                if (Player.State == NAudio.Wave.PlaybackState.Stopped & IsPlay) {
                    if (CurrentPlayList.Count > 0) {
                        if (SelectedMusicIndex == CurrentPlayList.Count - 1)
                            SelectedMusicIndex = 0;
                        else
                            SelectedMusicIndex++;
                        var file = CurrentPlayList[SelectedMusicIndex];
                        Play(file);
                    }
                }
            };
            Timer.Start();
            //
            //Value = Player.getPosition();
        }
    }
}
