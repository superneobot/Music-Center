using MediaCenter.ViewModel;
using MediaCenter.Views.enums;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace MediaCenter.Views {
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings {
        MainViewModel model;

        public Settings() {
            InitializeComponent();
            model = (MainViewModel)DataContext;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            var path = model.SavePath;
            Process.Start("explorer.exe", path);
        }

        private void ChooseFolder() {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            var result = fbd.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK) {
                var path = fbd.SelectedPath;
                System.Windows.MessageBox.Show($"{path}");
                model.SavePath = path;
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e) {
            model = (MainViewModel)DataContext;
            //
            if (model.PanelColor == PanelColor.Default) {
                check_color_default.IsChecked = true;
                check_color_variable.IsChecked = false;
            } else if (model.PanelColor == PanelColor.Set) {
                check_color_default.IsChecked = false;
                check_color_variable.IsChecked = true;
            }
            //
            if (model.WindowColorStyle == WindowColorStyle.Transparent) {
                check_style_transparent.IsChecked = true;
                check_style_darkglass.IsChecked = false;
                check_style_matglass.IsChecked = false;
                check_style_set.IsChecked = false;
            } else if (model.WindowColorStyle == WindowColorStyle.DarkGlass) {
                check_style_transparent.IsChecked = false;
                check_style_darkglass.IsChecked = true;
                check_style_matglass.IsChecked = false;
                check_style_set.IsChecked = false;
            } else if (model.WindowColorStyle == WindowColorStyle.MatGlass) {
                check_style_transparent.IsChecked = false;
                check_style_darkglass.IsChecked = false;
                check_style_matglass.IsChecked = true;
                check_style_set.IsChecked = false;
            } else if (model.WindowColorStyle == WindowColorStyle.Set) {
                check_style_transparent.IsChecked = false;
                check_style_darkglass.IsChecked = false;
                check_style_matglass.IsChecked = false;
                check_style_set.IsChecked = true;
            }
            //
            if (model.SavingPath == SavingPath.Default) {
                check_savepath_default.IsChecked = true;
                check_savepath_mymusic.IsChecked = false;
                check_savepath_set.IsChecked = false;
            } else if (model.SavingPath == SavingPath.MyMusic) {
                check_savepath_default.IsChecked = false;
                check_savepath_mymusic.IsChecked = true;
                check_savepath_set.IsChecked = false;
            } else if (model.SavingPath == SavingPath.Set) {
                check_savepath_default.IsChecked = false;
                check_savepath_mymusic.IsChecked = false;
                check_savepath_set.IsChecked = true;
            }

            //transparent.IsChecked = model.Transparent;
            //model.Transparent = Properties.Settings.Default.transparent;
        }

        private void close_btn_Click(object sender, RoutedEventArgs e) {

            this.Close();
        }

        private void check_color_default_Click(object sender, RoutedEventArgs e) {
            model.PanelColor = PanelColor.Default;
            check_color_default.IsChecked = true;
            check_color_variable.IsChecked = false;
            model.SetColor = SystemParameters.WindowGlassBrush.ToString();
        }

        private void check_color_variable_Click(object sender, RoutedEventArgs e) {
            ColorDialog color = new ColorDialog();
            var result = color.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK) {
                var alpha = color.Color.A;
                var red = color.Color.R;
                var green = color.Color.G;
                var blue = color.Color.B;
                model.SetColor = new System.Windows.Media.SolidColorBrush(Color.FromArgb(alpha, red, green, blue)).ToString();

                model.PanelColor = PanelColor.Set;
                check_color_default.IsChecked = false;
                check_color_variable.IsChecked = true;
            } else { check_color_variable.IsChecked = false; }
        }

        private void check_style_transparent_Click(object sender, RoutedEventArgs e) {
            model.WindowColorStyle = WindowColorStyle.Transparent;
            check_style_transparent.IsChecked = true;
            check_style_darkglass.IsChecked = false;
            check_style_matglass.IsChecked = false;
            check_style_set.IsChecked = false;
            //if (model.Transparent == true)
            model.Strenght = 0;
            //model.Transparent = true;
            model.WindowColor = Brushes.Transparent.ToString();
        }

        private void check_style_darkglass_Click(object sender, RoutedEventArgs e) {
            model.WindowColorStyle = WindowColorStyle.DarkGlass;
            check_style_transparent.IsChecked = false;
            check_style_darkglass.IsChecked = true;
            check_style_matglass.IsChecked = false;
            check_style_set.IsChecked = false;
            //if(model.Transparent == true)
            model.Strenght = 0.7;
            //if (model.Transparent == false)
            //    model.Strenght = 1.0;
            model.WindowColor = new System.Windows.Media.SolidColorBrush(Color.FromArgb(0, 17, 17, 17)).ToString().Remove(0, 3).Insert(0, "#");
        }

        private void check_style_matglass_Click(object sender, RoutedEventArgs e) {
            model.WindowColorStyle = WindowColorStyle.MatGlass;
            check_style_transparent.IsChecked = false;
            check_style_darkglass.IsChecked = false;
            check_style_matglass.IsChecked = true;
            check_style_set.IsChecked = false;
            //if (model.Transparent == true)
            model.Strenght = 0.9;
            //if (model.Transparent == false)
            //    model.Strenght = 1.0;
            model.WindowColor = new System.Windows.Media.SolidColorBrush(Color.FromArgb(0, 34, 34, 34)).ToString().Remove(0, 3).Insert(0, "#");
        }

        private void check_style_set_Click(object sender, RoutedEventArgs e) {

            ColorDialog color = new ColorDialog();
            var result = color.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK) {
                var alpha = color.Color.A;
                var red = color.Color.R;
                var green = color.Color.G;
                var blue = color.Color.B;
                model.WindowColor = new System.Windows.Media.SolidColorBrush(Color.FromArgb(alpha, red, green, blue)).ToString();
                //if (model.Transparent == true)
                model.Strenght = 0.3;
                //if (model.Transparent == false)
                //    model.Strenght = 1.0;
                model.WindowColorStyle = WindowColorStyle.Set;
                check_style_transparent.IsChecked = false;
                check_style_darkglass.IsChecked = false;
                check_style_matglass.IsChecked = false;
                check_style_set.IsChecked = true;

            } else { check_style_set.IsChecked = false; }
        }

        private void check_savepath_default_Click(object sender, RoutedEventArgs e) {
            model.SavingPath = SavingPath.Default;
            check_savepath_default.IsChecked = true;
            check_savepath_mymusic.IsChecked = false;
            check_savepath_set.IsChecked = false;
            //
            model.SavePath = AppContext.BaseDirectory + @"Downloaded\";
        }

        private void check_savepath_mymusic_Click(object sender, RoutedEventArgs e) {
            model.SavingPath = SavingPath.MyMusic;
            check_savepath_default.IsChecked = false;
            check_savepath_mymusic.IsChecked = true;
            check_savepath_set.IsChecked = false;
            //
            model.SavePath = KnownFolders.GetPath(KnownFolder.Music) + @"\";
        }

        private void check_savepath_set_Click(object sender, RoutedEventArgs e) {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            var result = fbd.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK) {
                var path = fbd.SelectedPath;
                System.Windows.MessageBox.Show($"{path}", "Music Center", MessageBoxButton.OK, MessageBoxImage.Information);
                model.SavePath = path + @"\";
                //
                model.SavingPath = SavingPath.Set;
                check_savepath_default.IsChecked = false;
                check_savepath_mymusic.IsChecked = false;
                check_savepath_set.IsChecked = true;
                //                
            } else { check_savepath_set.IsChecked = false; }
        }

        private void DockPanel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                this.DragMove();
        }

        private void transparent_Click(object sender, RoutedEventArgs e) {
            if (model.Transparent == false) {
                transparent.IsChecked = true;
                model.Transparent = true;
            }
        }
    }
}
