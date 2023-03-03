using MediaCenter.ViewModel;
using System;
using System.IO;
using System.Windows;

namespace MediaCenter {
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application {

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            MainWindow window = new MainWindow();
            var model = new MainViewModel();
            window.DataContext = model;
            window.Show();
            //
            var path = AppContext.BaseDirectory + @"\\Downloaded";
            if (Directory.Exists(path)) {
                return;
            } else {
                Directory.CreateDirectory(path);
            }
        }

    }
}
