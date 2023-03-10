using AngleSharp;
using MediaCenter.Model;
using MediaCenter.Views.enums;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Deployment.Application;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace MediaCenter.ViewModel {
    public class MainViewModel : ViewModelBase {
        #region collections
        private ObservableCollection<DataSource> _currentPlayList;
        public ObservableCollection<DataSource> CurrentPlayList {
            get {
                return _currentPlayList;
            }
            set {
                _currentPlayList = value;
                OnPropertyChanged(nameof(CurrentPlayList));
            }
        }
        private ObservableCollection<DataSource> _musicFiles;
        public ObservableCollection<DataSource> MusicFiles {
            get { return _musicFiles; }
            set {
                _musicFiles = value;
                OnPropertyChanged(nameof(MusicFiles));
            }
        }
        private ObservableCollection<DataSource> _downloadedFiles;
        public ObservableCollection<DataSource> DownloadedFiles {
            get { return _downloadedFiles; }
            set {
                _downloadedFiles = value;
                OnPropertyChanged(nameof(DownloadedFiles));
            }
        }
        private ObservableCollection<DataSource> _searchList;
        public ObservableCollection<DataSource> SearchList {
            get { return _searchList; }
            set {
                _searchList = value;
                OnPropertyChanged(nameof(SearchList));
            }
        }
        private ObservableCollection<DataSource> _favoriteList;
        public ObservableCollection<DataSource> FavoriteList {
            get { return _favoriteList; }
            set {
                _favoriteList = value;
                OnPropertyChanged(nameof(FavoriteList));
            }
        }
        public ObservableCollection<DataSource> RadioList;
        private ObservableCollection<DataSource> _collections;
        public ObservableCollection<DataSource> Collections {
            get {
                return _collections;
            }
            set {
                _collections = value;
                OnPropertyChanged(nameof(Collections));
            }
        }
        private ObservableCollection<DataSource> _parsedCollection;
        public ObservableCollection<DataSource> ParsedCollection {
            get {
                return _parsedCollection;
            }
            set {
                _parsedCollection = value;
                OnPropertyChanged(nameof(ParsedCollection));
            }
        }
        private string _parsedCollectionTitle;
        public string ParsedCollectionTitle {
            get {
                return _parsedCollectionTitle;
            }
            set {
                _parsedCollectionTitle = value;
                OnPropertyChanged(nameof(ParsedCollectionTitle));
            }
        }
        private ItemsControl _content;
        public ItemsControl Content {
            get {
                return _content;
            }
            set {
                _content = value;
                OnPropertyChanged(nameof(Content));
            }
        }

        #endregion
        #region program_param
        private PanelColor _panelColor;
        public PanelColor PanelColor {
            get { return _panelColor; }
            set {
                _panelColor = value;
                OnPropertyChanged(nameof(PanelColor));
            }
        }
        private WindowColorStyle _windowColorStyle;
        public WindowColorStyle WindowColorStyle {
            get { return _windowColorStyle; }
            set {
                _windowColorStyle = value;
                OnPropertyChanged(nameof(WindowColorStyle));
            }
        }
        private SavingPath _savingPath;
        public SavingPath SavingPath {
            get { return _savingPath; }
            set {
                _savingPath = value;
                OnPropertyChanged(nameof(SavingPath));
            }
        }
        private string _setColor;
        public string SetColor {
            get {
                if (Properties.Settings.Default.windowcolor == "#00000000") {
                    _setColor = SystemParameters.WindowGlassBrush.ToString();
                    PanelColor = PanelColor.Default;
                } else {
                    _setColor = Properties.Settings.Default.windowcolor;
                    var color = SystemParameters.WindowGlassBrush.ToString();
                    if (_setColor.ToString() == color)
                        PanelColor = PanelColor.Default;
                    if (_setColor.ToString() != color)
                        PanelColor = PanelColor.Set;
                }
                return _setColor;
            }
            set {
                _setColor = value;
                Properties.Settings.Default.windowcolor = _setColor;
                Properties.Settings.Default.Save();
                OnPropertyChanged(nameof(SetColor));
            }
        }

        private string _windowColor;
        public string WindowColor {
            get {
                if (Properties.Settings.Default.windowstyle == "#00000000") {
                    _windowColor = Brushes.Transparent.ToString();
                    WindowColorStyle = WindowColorStyle.Transparent;
                } else {
                    _windowColor = Properties.Settings.Default.windowstyle;
                    var transparent = Brushes.Transparent.ToString();
                    var black = "#111111";
                    var gray = "#222222";
                    if (_windowColor.ToString() == transparent)
                        WindowColorStyle = WindowColorStyle.Transparent;
                    if (_windowColor.ToString() == black)
                        WindowColorStyle = WindowColorStyle.DarkGlass;
                    if (_windowColor.ToString() == gray)
                        WindowColorStyle = WindowColorStyle.MatGlass;
                    if (_windowColor.ToString() != transparent & _windowColor.ToString() != black & _windowColor.ToString() != gray)
                        WindowColorStyle = WindowColorStyle.Set;
                }
                return _windowColor;
            }
            set {
                _windowColor = value;
                Properties.Settings.Default.windowstyle = _windowColor;
                Properties.Settings.Default.Save();
                OnPropertyChanged(nameof(WindowColor));
            }
        }

        private string _savePath;
        public string SavePath {
            get {
                if (Properties.Settings.Default.savepath == "") {
                    _savePath = AppContext.BaseDirectory + @"Downloaded\";
                    SavingPath = SavingPath.Default;
                } else {
                    _savePath = Properties.Settings.Default.savepath;
                    if (_savePath == AppContext.BaseDirectory + @"Downloaded\")
                        SavingPath = SavingPath.Default;
                    if (_savePath == KnownFolders.GetPath(KnownFolder.Music))
                        SavingPath = SavingPath.MyMusic;
                    if (_savePath != KnownFolders.GetPath(KnownFolder.Music) & _savePath != AppContext.BaseDirectory + @"Downloaded\")
                        SavingPath = SavingPath.Set;
                }
                return _savePath;
            }
            set {
                _savePath = value;
                Properties.Settings.Default.savepath = _savePath;
                Properties.Settings.Default.Save();
                OnPropertyChanged(nameof(SavePath));
            }
        }
        private double _opacity;
        public double Opacity {
            get { return _opacity; }
            set {
                _opacity = value;
                OnPropertyChanged(nameof(Opacity));
            }
        }
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
        private Visibility _openPathVisible;
        public Visibility OpenPathVisible {
            get {
                return _openPathVisible;
            }
            set {
                _openPathVisible = value;
                OnPropertyChanged(nameof(OpenPathVisible));
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
        private Visibility _backVisible;
        public Visibility BackVisible {
            get {
                return _backVisible;
            }
            set {
                _backVisible = value;
                OnPropertyChanged(nameof(BackVisible));
            }
        }
        private Visibility _returnVisible;
        public Visibility ReturnVisible {
            get {
                return _returnVisible;
            }
            set {
                _returnVisible = value;
                OnPropertyChanged(nameof(ReturnVisible));
            }
        }
        private Orientation _lvOrientation;
        public Orientation LVOrientation {
            get {
                return _lvOrientation;
            }
            set {
                _lvOrientation = value;
                OnPropertyChanged(nameof(LVOrientation));
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
        private double _strenght;
        public double Strenght {
            get { 
                if(Properties.Settings.Default.strength == 0.0 & WindowColorStyle != WindowColorStyle.Transparent) {
                    _strenght = 0.5;
                } else {
                    _strenght = Properties.Settings.Default.strength;
                }
                return _strenght;
            }
            set {
                _strenght = value;
                Properties.Settings.Default.strength = _strenght;
                Properties.Settings.Default.Save();
                OnPropertyChanged(nameof(Strenght));
            }
        }
        private bool _transparent;
        public bool Transparent {
            get {
                if(Properties.Settings.Default.transparent == false) {
                   _transparent = false;
                    Strenght = 1.0;
                } if(Properties.Settings.Default.transparent == true) {
                    _transparent = true;
                    Strenght = Properties.Settings.Default.strength;
                }
                return _transparent;
            }
            set {
                _transparent = value;
                Properties.Settings.Default.transparent = _transparent;
                Properties.Settings.Default.Save();
                if(_transparent == true) {
                    Strenght = 0.5;
                } else {
                    Strenght = 1.0;
                }
                OnPropertyChanged(nameof(Transparent));
            }
        }
        private CenterState CenterState { get; set; } = CenterState.Playlist;
        public DispatcherTimer Timer;
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
            get { 
                return _searchText; }
            set {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }
        private object _selectedItem;
        public object SelectedType {
            get {
                return _selectedItem;
            }
            set {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedType));
            }
        }
        private DataSource _selectedMusic;
        public DataSource SelectedMusic {
            get {
                return _selectedMusic;
            }
            set {
                _selectedMusic = value;
                OnPropertyChanged(nameof(SelectedMusic));
            }
        }
        private Collection _selectedCollection;
        public Collection SelectedCollection {
            get {
                return _selectedCollection;
            }
            set {
                _selectedCollection = value;
                OnPropertyChanged(nameof(SelectedCollection));
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
        private DataSource _playingMusic;
        public DataSource PlayingMusic {
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
        private string _version;
        public string Version {
            get {
                return _version;
            }
            set {
                _version = value;
                OnPropertyChanged(nameof(Version));
            }
        }
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
                    _playBtn = "/Resources/play.png";
                }
                return _playBtn;
            }
            set {
                _playBtn = value;
                OnPropertyChanged(nameof(PlayBtn));
            }
        }
        private string _playText;
        public string PlayText {
            get {
                return _playText;
            }
            set {
                _playText = value;
                OnPropertyChanged(nameof(PlayText));
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
                if (_volume == 1) {
                    _volume = 100;
                }
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
        private DataSource download_file;
        private bool _sideMenuWidth;
        public bool SideMenuWidth {
            get {
                return _sideMenuWidth;
            }
            set {
                _sideMenuWidth = value;
                OnPropertyChanged(nameof(SideMenuWidth));
            }
        }

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
                return new RelayCommand<ObservableCollection<DataSource>>((e) => {
                    Content = new ItemsControl();
                    Content.ItemsSource = MusicFiles;
                    State = "Плейлист";
                    CenterState = CenterState.Playlist;
                    ClearVisible = Visibility.Visible;
                    OpenPathVisible = Visibility.Collapsed;
                    BackVisible = Visibility.Collapsed;
                    ReturnVisible = Visibility.Collapsed;
                    LVOrientation = Orientation.Vertical;
                    Check(MusicFiles, PlayingMusic);
                });
            }
        }
        public ICommand OpenDownloadedList {
            get {
                return new RelayCommand<ObservableCollection<DataSource>>((e) => {
                    Content = new ItemsControl();
                    Content.ItemsSource = DownloadedFiles;
                    Update(DownloadedFiles);
                    State = "Загруженное";
                    CenterState = CenterState.Downloaded;
                    ClearVisible = Visibility.Visible;
                    OpenPathVisible = Visibility.Visible;
                    BackVisible = Visibility.Collapsed;
                    ReturnVisible = Visibility.Collapsed;
                    LVOrientation = Orientation.Vertical;
                    Check(DownloadedFiles, PlayingMusic);
                });
            }
        }
        public ICommand OpenSearchList {
            get {
                return new RelayCommand<ObservableCollection<DataSource>>((e) => {
                    Content = new ItemsControl();
                    Content.ItemsSource = SearchList;
                    State = "Поиск треков";
                    CenterState = CenterState.Search;
                    ClearVisible = Visibility.Visible;
                    OpenPathVisible = Visibility.Collapsed;
                    BackVisible = Visibility.Collapsed;
                    ReturnVisible = Visibility.Collapsed;
                    LVOrientation = Orientation.Vertical;
                    Check(SearchList, PlayingMusic);
                });
            }
        }
        public ICommand OpenFavoriteList {
            get {
                return new RelayCommand<ObservableCollection<DataSource>>((e) => {
                    Content = new ItemsControl();
                    Content.ItemsSource = FavoriteList;
                    State = "Мне нравится";
                    CenterState = CenterState.Favorite;
                    ClearVisible = Visibility.Collapsed;
                    OpenPathVisible = Visibility.Collapsed;
                    BackVisible = Visibility.Collapsed;
                    ReturnVisible = Visibility.Collapsed;
                    LVOrientation = Orientation.Vertical;
                    Check(FavoriteList, PlayingMusic);
                });
            }
        }
        public ICommand OpenRadioPage {
            get {
                return new RelayCommand<ObservableCollection<DataSource>>((e) => {
                    CurrentPlayList = RadioList;
                    State = "Радио";
                    CenterState = CenterState.Radio;
                    ClearVisible = Visibility.Hidden;
                    OpenPathVisible = Visibility.Hidden;
                });
            }
        }
        public ICommand OpenCollections {
            get {
                return new RelayCommand<ObservableCollection<Collection>>((e) => {
                    if (ParsedCollection.Count > 0) {
                        Content = new ItemsControl();
                        Content.ItemsSource = Collections;
                        State = "Подборки треков";
                        CenterState = CenterState.Collections;
                        ClearVisible = Visibility.Hidden;
                        OpenPathVisible = Visibility.Hidden;
                        BackVisible = Visibility.Collapsed;
                        ReturnVisible = Visibility.Visible;
                        LVOrientation = Orientation.Horizontal;
                    } else {
                        Content = new ItemsControl();
                        GetCollections();
                        Content.ItemsSource = Collections;
                        State = "Подборки треков";
                        CenterState = CenterState.Collections;
                        ClearVisible = Visibility.Collapsed;
                        OpenPathVisible = Visibility.Collapsed;
                        BackVisible = Visibility.Collapsed;
                        ReturnVisible = Visibility.Collapsed;
                        LVOrientation = Orientation.Horizontal;
                    }
                });
            }
        }
        public ICommand OpenSettingsPage {
            get {
                return new RelayCommand<DataSource>((e) => {
                    var settings = new Views.Settings();
                    var vm = Application.Current.Windows[0].DataContext;
                    settings.DataContext = vm;
                    settings.ShowDialog();
                });
            }
        }
        public ICommand AddFile {
            get {
                return new RelayCommand<DataSource>((e) => AddingFile());
            }
        }
        public ICommand UpdateList {
            get {
                return new RelayCommand<ObservableCollection<DataSource>>((e) => Update(DownloadedFiles));
            }
        }
        public ICommand ClearList {
            get {
                return new RelayCommand<ObservableCollection<DataSource>>((e) => {
                    if (CenterState == CenterState.Playlist)
                        MusicFiles.Clear();
                    if (CenterState == CenterState.Downloaded) {
                        var result = MessageBox.Show("Это действие удалит загруженные файлы с диска.\nВыполнить?", "Music Center", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
                        if (result == MessageBoxResult.OK) {
                            if (Directory.Exists(SavePath)) {
                                DirectoryInfo dir = new DirectoryInfo(SavePath);
                                foreach (var file in dir.GetFiles()) {
                                    file.Delete();
                                }
                                DownloadedFiles.Clear();
                                MessageBox.Show("Готово!", "Music Center", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        } else {
                            return;
                        }
                    }
                    if (CenterState == CenterState.Search) {
                        SearchList.Clear();
                    }
                });
            }
        }
        public ICommand PlayFile {
            get {
                return new RelayCommand(item => {
                    if (CenterState == CenterState.Playlist) {
                        CurrentPlayList = MusicFiles;
                        Play(SelectedMusic, MusicFiles);
                        
                    }
                    if (CenterState == CenterState.Downloaded) {
                        Play(SelectedMusic, DownloadedFiles);
                        CurrentPlayList = DownloadedFiles;
                    }
                    if (CenterState == CenterState.Search) {
                        CurrentPlayList = SearchList;
                        Play(SelectedMusic, SearchList);
                        
                    }
                    if (CenterState == CenterState.Favorite) {
                        CurrentPlayList = FavoriteList;
                        Play(SelectedMusic, FavoriteList);
                        
                    }
                    if (CenterState == CenterState.Collections) {
                        Play(SelectedMusic, ParsedCollection);
                        CurrentPlayList = ParsedCollection;
                    }
                });
            }
        }
        public RelayCommand PlayFileFromList {
            get {
                return new RelayCommand(item => {
                    if (CenterState == CenterState.Playlist) {
                        Play(SelectedMusic, MusicFiles);
                        CurrentPlayList = MusicFiles;
                    }
                    if (CenterState == CenterState.Downloaded) {
                        Play(SelectedMusic, DownloadedFiles);
                        CurrentPlayList = DownloadedFiles;
                    }
                    if (CenterState == CenterState.Search) {
                        Play(SelectedMusic, SearchList);
                        CurrentPlayList = SearchList;
                    }
                    if (CenterState == CenterState.Favorite) {
                        Play(SelectedMusic, FavoriteList);
                        CurrentPlayList = FavoriteList;
                    }
                    if (CenterState == CenterState.Collections) {
                        Play(SelectedMusic, ParsedCollection);
                        CurrentPlayList = ParsedCollection;
                    }
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
                        Play(file, CurrentPlayList);

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
                    Play(file, CurrentPlayList);
                });
            }
        }
        public ICommand Search {
            get {
                return new RelayCommand<string>(Text => {
                    SearchText = Text;
                    FindMusic(Text);
                });
            }
        }
        public ICommand SearchClick {
            get {
                return new RelayCommand(obj => {
                    var box = obj as TextBox;
                    box.Clear();
                });
            }
        }
        public ICommand AddToFavoriteList {
            get {
                return new RelayCommand<DataSource>(item => {
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
                    if (!IsTopMost) {
                        IsTopMost = true;
                    } else {
                        IsTopMost = false;
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
                    if (SavePath != string.Empty)
                        DownloadSong(SelectedMusic);
                    else {
                        Directory.CreateDirectory(AppContext.BaseDirectory + @"Downloaded/");
                        DownloadSong(SelectedMusic);
                    }
                });
            }
        }
        public ICommand CompactMode {
            get {
                return new RelayCommand(e => {
                    var compact = new Compact();
                    var vm = Application.Current.Windows[0].DataContext;
                    compact.DataContext = vm;
                    compact.Show();
                    var window = Application.Current.MainWindow;
                    window.Hide();
                });
            }
        }
        public ICommand OpenPath {
            get {
                return new RelayCommand(e => {
                    Process.Start("explorer.exe", SavePath);
                });
            }
        }
        public ICommand Parse {
            get {
                return new RelayCommand(item => {
                    ParsedCollectionTitle = SelectedMusic.Title;
                    ParseCollection(SelectedMusic.FilePath);
                    Content = new ItemsControl();
                    Content.ItemsSource = ParsedCollection;
                    State = ParsedCollectionTitle;
                    CenterState = CenterState.Collections;
                    ClearVisible = Visibility.Collapsed;
                    OpenPathVisible = Visibility.Collapsed;
                    LVOrientation = Orientation.Vertical;
                    Check(ParsedCollection, PlayingMusic);
                });
            }
        }
        public ICommand BackToCollections {
            get {
                return new RelayCommand(e => {
                    //ParsedCollection.Clear();
                    Content = new ItemsControl();
                    //GetCollections();
                    Content.ItemsSource = Collections;
                    State = "Подборки треков";
                    CenterState = CenterState.Collections;
                    ClearVisible = Visibility.Collapsed;
                    OpenPathVisible = Visibility.Collapsed;
                    BackVisible = Visibility.Collapsed;
                    ReturnVisible = Visibility.Visible;
                    LVOrientation = Orientation.Horizontal;
                });
            }
        }
        public ICommand Return {
            get {
                return new RelayCommand(e => {
                    Content = new ItemsControl();
                    Content.ItemsSource = ParsedCollection;
                    State = ParsedCollectionTitle;
                    CenterState = CenterState.Collections;
                    ClearVisible = Visibility.Collapsed;
                    OpenPathVisible = Visibility.Collapsed;
                    BackVisible = Visibility.Visible;
                    ReturnVisible = Visibility.Collapsed;
                    LVOrientation = Orientation.Vertical;
                    Check(ParsedCollection, PlayingMusic);
                });
            }
        }
        public ICommand GetArtist {
            get {
                return new RelayCommand<string>(Text => {
                    var a = Text;
                    MessageBox.Show($"{Text}");
                });
            }
        }
        public ICommand ShrinkMenu {
            get {
                return new RelayCommand(e => {
                    if (SideMenuWidth)
                        SideMenuWidth = false;
                    else
                        SideMenuWidth = true;
                });
            }
        }
        #endregion
        #region app_voids
        private void DownloadSong(DataSource file) {
            file.Download = true;
            WebClient client = new WebClient();
            var file_path = SavePath + Path.GetFileName(file.FilePath);
            //if (client.IsBusy) return;
            LoadProgress = 0;

            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
            client.DownloadFileAsync(new Uri(file.FilePath), file_path);
            download_file = file;
        }
        void client_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e) {
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
            File.WriteAllText(AppContext.BaseDirectory + "favorite.json", file, System.Text.Encoding.UTF8);
            UploadFiles("ftp://localhost/database/", AppContext.BaseDirectory + "favorite.json");
        }

        public void UploadFiles(string address, string file) {
            try {
                using (WebClient webClient = new WebClient()) {
                    webClient.Credentials = new NetworkCredential("ftp", "");
                    var path = address + "favorite.json";
                    webClient.UploadFile(path, WebRequestMethods.Ftp.UploadFile, @file);
                }
            } catch (Exception e) {
                File.WriteAllText(AppContext.BaseDirectory + "favorite.json", file);
            }
        }

        private void LoadFavoriteList() {
            try {
                using (WebClient client = new WebClient()) {
                    var data = client.DownloadString("http://localhost/database/favorite.json");
                    FavoriteList = JsonConvert.DeserializeObject<ObservableCollection<DataSource>>(data);
                }
            } catch {
                if (File.Exists(AppContext.BaseDirectory + "favorite.json")) {
                    var file = File.ReadAllText(AppContext.BaseDirectory + "favorite.json");
                    FavoriteList = JsonConvert.DeserializeObject<ObservableCollection<DataSource>>(file);
                }
            }
        }
        private void TrackIsFavorite(DataSource file) {
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
                    MusicFiles.Add(new DataSource(item));
                }
            }
            CurrentPlayList = MusicFiles;
            State = "Плейлист";
            CenterState = CenterState.Playlist;
            ClearVisible = Visibility.Visible;
        }
        private void Update(ObservableCollection<DataSource> source) {
            //source.Clear();
            string extension = ".mp3";
            var temp = new ObservableCollection<DataSource>();
            if (Directory.Exists(SavePath)) {
                DirectoryInfo dir = new DirectoryInfo(SavePath);
                FileInfo[] files = dir.GetFiles("*" + extension, SearchOption.AllDirectories);

                foreach (FileInfo file in files) {
                    var item = new DataSource(file.FullName);
                    //var result = source.Contains(item);
                    //if (!result)
                    //    source.Add(item);
                    if (!source.Contains(item)) {
                        source.Add(item);
                    }
                }
            }
            Check(source, PlayingMusic);
        }
        public void NextTrack() {
            if (CurrentPlayList.Count > 0) {
                _history_index = SelectedMusicIndex;
                if (SelectedMusicIndex == CurrentPlayList.Count - 1)
                    SelectedMusicIndex = 0;
                else
                    SelectedMusicIndex++;
                var file = CurrentPlayList[SelectedMusicIndex];
                Play(file, CurrentPlayList);

            }
        }
        public void PrevTrack() {
            if (_history_index != 0)
                if (SelectedMusicIndex == CurrentPlayList.Count - 1)
                    SelectedMusicIndex--;
                else
                    SelectedMusicIndex = CurrentPlayList.Count - 1;
            else
                SelectedMusicIndex = _history_index;
            var file = CurrentPlayList[SelectedMusicIndex];
            Play(file, CurrentPlayList);
        }
        public async void PlayPause() {
            if(Player.State == NAudio.Wave.PlaybackState.Playing) {
                await Player.Pause();
                ThumbPlayText = "Играть";
                PlayText = "Играть";
                PlayBtn = "/Resources/play.png";
                IsPlay = false;
                PlayingMusic.IsPlay = false;
                PlayingMusic.IsPaused = true;
            }
            else if(Player.State == NAudio.Wave.PlaybackState.Paused) {
                await Player.Resume();
                ThumbPlayText = "Пауза";
                PlayText = "Пауза";
                PlayBtn = "/Resources/pause.png";
                IsPlay = true;
                PlayingMusic.IsPlay = true;
                PlayingMusic.IsPaused = false;
            }
        }
        public void PlayerStop() {
            Player.Stop();
            ThumbPlayText = "Играть";
            PlayText = "Играть";
            PlayBtn = "/Resources/play.png";
            IsPlay = false;
            PlayingMusic.IsPlay = false;
            PlayingMusic.IsPaused = false;
        }

        private async void PlayClick(DataSource file, ObservableCollection<DataSource> list) {
            if (file == null) return;
            await Player.Stop(); IsPlay = false;
            if (Player.State == NAudio.Wave.PlaybackState.Stopped) {
                if (CenterState == CenterState.Playlist | CenterState == CenterState.Downloaded) {
                    await Player.Play(file.FilePath);
                    PlayBtn = "/Resources/pause.png";
                    ThumbPlayText = "Pause";
                    Duration = Player?.getTotalTimeString();
                    Time = Player.getPositionString();
                    Maximum = Player.getTotalTime();
                    IsPlay = true;
                } else if (CenterState == CenterState.Search) {
                    await Player.Play(file.URL);
                    PlayBtn = "/Resources/pause.png";
                    ThumbPlayText = "Pause";
                    Duration = Player?.getTotalTimeString();
                    Time = Player.getPositionString();
                    Maximum = Player.getTotalTime();
                    IsPlay = true;
                } else if (CenterState == CenterState.Favorite) {
                    if (file.URL == null)
                        await Player.Play(file.FilePath);
                    else
                        await Player.Play(file.URL);
                    PlayBtn = "/Resources/pause.png";
                    ThumbPlayText = "Pause";
                    Duration = Player?.getTotalTimeString();
                    Time = Player.getPositionString();
                    Maximum = Player.getTotalTime();
                    IsPlay = true;
                }
            }

            PlayingMusic = file;
            PlayingMusic.IsPlay = true;
            Check(list, PlayingMusic);
            PlayingFile = $"{PlayingMusic.Title} - {PlayingMusic.Artist}";
        }
        private void Check(ObservableCollection<DataSource> source, DataSource target) {
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
        public async void ShowOpacity() {
            await Task.Factory.StartNew(() => {
                for (double i = 0.0; i < 1.1; i += 0.1) {
                    Opacity = i;
                    Thread.Sleep(5);
                }

            });
        }
        public async void HideOpacity() {
            await Task.Factory.StartNew(() => {
                for (double i = 1.0; i > 0.0; i -= 0.1) {
                    Opacity = i;
                    Thread.Sleep(5);
                }

            });
        }
        private async void Play(DataSource file, ObservableCollection<DataSource> list) {
            if (list.Count == 0) return;
            //file = CurrentPlayList[0];
            if (file == null) return;
            //
            if (Player.State == NAudio.Wave.PlaybackState.Playing & file == PlayingMusic) {
                if (file == PlayingMusic) {
                    await Player.Pause();
                    ThumbPlayText = "Играть";
                    PlayText = "Играть";
                    PlayBtn = "/Resources/play.png";
                    IsPlay = false;
                    PlayingMusic.IsPlay = false;
                    PlayingMusic.IsPaused = true;
                } else {
                    await Player.Stop(); IsPlay = false;
                    //if (CenterState == CenterState.Playlist | CenterState == CenterState.Downloaded) {
                    await Player.Play(file.FilePath);
                    PlayBtn = "/Resources/pause.png";
                    PlayText = "Пауза";
                    ThumbPlayText = "Пауза";
                    Duration = Player?.getTotalTimeString();
                    Time = Player.getPositionString();
                    Maximum = Player.getTotalTime();
                    IsPlay = true;
                    Timer.Start();
                    //} else if (CenterState == CenterState.Search) {
                    //    await Player.Play(file.URL);
                    //    PlayBtn = "/Resources/pause.png";
                    //    PlayText = "Пауза";
                    //    ThumbPlayText = "Пауза";
                    //    Duration = Player?.getTotalTimeString();
                    //    Time = Player.getPositionString();
                    //    Maximum = Player.getTotalTime();
                    //    IsPlay = true;
                    //} else if (CenterState == CenterState.Favorite) {
                    //    if (file.URL == null)
                    //        await Player.Play(file.FilePath);
                    //    else
                    //        await Player.Play(file.URL);
                    //    PlayBtn = "/Resources/pause.png";
                    //    PlayText = "Пауза";
                    //    ThumbPlayText = "Пауза";
                    //    Duration = Player?.getTotalTimeString();
                    //    Time = Player.getPositionString();
                    //    Maximum = Player.getTotalTime();
                    //    IsPlay = true;
                    //}
                    PlayingMusic = file;
                    PlayingMusic.IsPaused = false;
                    PlayingMusic.IsPlay = true;
                    //Check(list, PlayingMusic);
                }
            } else if (Player.State == NAudio.Wave.PlaybackState.Paused & file == PlayingMusic) {
                await Player.Resume();
                ThumbPlayText = "Пауза";
                PlayText = "Пауза";
                PlayBtn = "/Resources/pause.png";
                IsPlay = true;
                PlayingMusic.IsPlay = true;
                PlayingMusic.IsPaused = false;
            } else if (Player.State == NAudio.Wave.PlaybackState.Stopped | file != PlayingMusic) {
                await Player.Stop();
                file.IsPlay = false;
                file.IsPaused = false;
                //if (CenterState == CenterState.Playlist | CenterState == CenterState.Downloaded) {
                await Player.Play(file.FilePath);
                PlayBtn = "/Resources/pause.png";
                PlayText = "Пауза";
                ThumbPlayText = "Пауза";
                Duration = Player?.getTotalTimeString();
                Time = Player.getPositionString();
                Maximum = Player.getTotalTime();
                IsPlay = true;
                Timer.Start();
                //} else if (CenterState == CenterState.Search) {
                //    await Player.Play(file.URL);
                //    PlayBtn = "/Resources/pause.png";
                //    PlayText = "Пауза";
                //    ThumbPlayText = "Пауза";
                //    Duration = Player?.getTotalTimeString();
                //    Time = Player.getPositionString();
                //    Maximum = Player.getTotalTime();
                //    IsPlay = true;
                //} else if (CenterState == CenterState.Favorite) {
                //    if (file.URL == null)
                //        await Player.Play(file.FilePath);
                //    else
                //        await Player.Play(file.URL);
                //    PlayBtn = "/Resources/pause.png";
                //    PlayText = "Пауза";
                //    ThumbPlayText = "Пауза";
                //    Duration = Player?.getTotalTimeString();
                //    Time = Player.getPositionString();
                //    Maximum = Player.getTotalTime();
                //    IsPlay = true;
                //}
                PlayingMusic = file;
                PlayingMusic.IsPaused = false;
                PlayingMusic.IsPlay = true;
                Check(list, PlayingMusic);
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
            Content = new ItemsControl();
            Content.ItemsSource = SearchList;
            State = "Поиск треков";
            CenterState = CenterState.Search;
            ClearVisible = Visibility.Visible;
            OpenPathVisible = Visibility.Collapsed;
            BackVisible = Visibility.Collapsed;
            ReturnVisible = Visibility.Collapsed;
            LVOrientation = Orientation.Vertical;
            Check(SearchList, PlayingMusic);
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
                        var file = new DataSource(title, artist, "", image_url_small, time, link, Location.Internet);
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
        public async void GetCollections() {
            string address = "https://ru.hitmotop.com/top_charts";
            Collections.Clear();
            try {
                var config = Configuration.Default.WithDefaultLoader();
                var context = BrowsingContext.New(config);
                var document = await context.OpenAsync(address);
                //
                var albumlist = document.QuerySelector("ul[class='album-list']");
                var albumitem = albumlist.QuerySelectorAll("li[class='album-item']");
                foreach (var item in albumitem) {
                    var url = "https://ru.hitmotop.com";
                    var link = item.QuerySelector("a").GetAttribute("href");
                    var title = item.QuerySelector("span[class='album-title']").TextContent;
                    var poster = item.QuerySelector("span").GetAttribute("style").Replace("background-image: url('", "").Replace("')", "");

                    Collections.Add(new DataSource(title, url + poster, url + link));
                }
            } catch {

            }
            BackVisible = Visibility.Collapsed;
            if(ParsedCollection.Count >0)
            ReturnVisible = Visibility.Visible;
        }
        private async void ParseCollection(string url) {
            ParsedCollection.Clear();
            var config = Configuration.Default.WithDefaultLoader();
            var address = url;
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
                        var file = new DataSource(title, artist, "", image_url_small, time, link, Location.Internet);
                        ParsedCollection.Add(file);
                    }
                } else {
                    MessageBox.Show("Ничего не найдено");
                }
            } catch {
            }
            BackVisible = Visibility.Visible;
            ReturnVisible = Visibility.Collapsed;
        }
        #endregion
        /*---------------------------------------------------------------------*/
        public MainViewModel() {
            //

            //
            Player = new Player();
            CurrentPlayList = new ObservableCollection<DataSource>();
            MusicFiles = new ObservableCollection<DataSource>();
            DownloadedFiles = new ObservableCollection<DataSource>();
            SearchList = new ObservableCollection<DataSource>();
            FavoriteList = new ObservableCollection<DataSource>();
            RadioList = new ObservableCollection<DataSource>();
            Collections = new ObservableCollection<DataSource>();
            ParsedCollection = new ObservableCollection<DataSource>();
            Update(DownloadedFiles);
            LoadFavoriteList();
            //
            Opacity = 1.0;
            CurrentPlayList = MusicFiles;
            State = "Плейлист";
            //SearchText = "Поиск трека или исполнителя";
            ClearVisible = Visibility.Visible;
            OpenPathVisible = Visibility.Collapsed;
            BackVisible = Visibility.Collapsed;
            ReturnVisible = Visibility.Collapsed;
            LVOrientation = Orientation.Vertical;
            Volume = (long)(Properties.Settings.Default.volume);
            ThumbPlayText = "Играть";
            PlayText = "Играть";
            SideMenuWidth = false;
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
                Version = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            //
            Timer = new DispatcherTimer(DispatcherPriority.Background);
            Timer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            Timer.Tick += delegate {
                if(Player.State == NAudio.Wave.PlaybackState.Playing)
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
                        Play(file, CurrentPlayList);
                    }
                }
            };
            
        }
    }
}
