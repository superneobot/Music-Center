using MediaCenter.ViewModel;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MediaCenter {
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            if (!InstanceCheck()) {
                Process.GetCurrentProcess().Kill();
            } else {
                MainWindow window = new MainWindow();
                var model = new MainViewModel();
                model.WindowColor = MediaCenter.Properties.Settings.Default.windowstyle;
                window.DataContext = model;
                window.Show();
                //
                var path = Path.Combine(Environment.CurrentDirectory, @"Downloaded");
                var database = Path.Combine(Environment.CurrentDirectory, @"Database");
                if (Directory.Exists(path)) {
                    //return;
                } else {
                    Directory.CreateDirectory(path);
                }
                if (Directory.Exists(database)) {
                    //DirectoryInfo dir = new DirectoryInfo(database);
                    //foreach (var file in dir.GetDirectories()) {
                    //    file.Delete(true);
                    //}
                    //Directory.CreateDirectory(database);
                } else {
                    Directory.CreateDirectory(database);
                }
                //


            }
        }


        static Mutex InstanceCheckMutex;
        static bool InstanceCheck() {
            bool isNew;
            var mutex = new Mutex(true, "Music Center", out isNew);
            if (isNew)
                InstanceCheckMutex = mutex;
            else
                mutex.Dispose(); // отпустить mutex сразу
            return isNew;
        }

        private DependencyObject findParentTreeItem(DependencyObject CurrentControl, Type ParentType) {
            bool notfound = true;
            while (notfound) {
                DependencyObject parent = VisualTreeHelper.GetParent(CurrentControl);
                string ParentTypeName = ParentType.Name;
                //Compare current type name with what we want
                if (parent == null) {
                    System.Diagnostics.Debugger.Break();
                    notfound = false;
                    continue;
                }
                if (parent.GetType().Name == ParentTypeName) {
                    return parent;
                }
                //we haven't found it so walk up the tree.
                CurrentControl = parent;
            }
            return null;
        }

        private void MakeItemSelected(object sender, RoutedEventArgs e) {
            Button button = sender as Button;
            DependencyObject tvi = findParentTreeItem(button, typeof(ListViewItem));
            if (tvi != null) {
                ListViewItem lbi = tvi as ListViewItem;
                lbi.IsSelected = true;
            }
        }
    }
}
