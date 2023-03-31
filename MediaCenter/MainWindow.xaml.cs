using MediaCenter.ViewModel;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using Button = System.Windows.Controls.Button;
using ListViewItem = System.Windows.Controls.ListViewItem;
using MenuItem = System.Windows.Controls.MenuItem;

namespace MediaCenter {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        private DispatcherTimer timer;
        private const uint WM_NCRBUTTONDOWN = 0xa4;
        private const uint HTCAPTION = 0x02;

        bool glass = SystemParameters.IsGlassEnabled;
        bool volume_panel_visible = false;

        MediaCenter.Model.ApplicaionContext db;
        public MainWindow() {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            timer = new DispatcherTimer(DispatcherPriority.DataBind);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
            timer.Start();

            db = new Model.ApplicaionContext();
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e) {
            IntPtr windowhandle = new WindowInteropHelper(this).Handle;
            HwndSource hwndSource = HwndSource.FromHwnd(windowhandle);
            hwndSource.AddHook(new HwndSourceHook(WndProc));
            MainViewModel vm = (MainViewModel)DataContext;
            volume_control.Value = (int)Properties.Settings.Default.volume;
            vm.Volume = (long)Properties.Settings.Default.volume;
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {
            if ((msg == WM_NCRBUTTONDOWN) && (wParam.ToInt32() == HTCAPTION)) {
                ShowContextMenu();
                handled = true;
            }
            return IntPtr.Zero;
        }

        private void ShowContextMenu() {
            var contextMenu = Resources["app_menu"] as System.Windows.Controls.ContextMenu;
            contextMenu.IsOpen = true;
        }

        private void Timer_Tick(object sender, EventArgs e) {
            MainViewModel vm = (MainViewModel)DataContext;
            if (vm != null)
                vm.Value = vm.Player.getPosition();
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

        private void ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {

        }

        private void seeker_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            timer.Stop();
        }

        private void seeker_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            Slider slider = sender as Slider;
            MainViewModel vm = (MainViewModel)DataContext;
            vm.Player.setPosition((long)slider.Value);
            timer.Start();
        }

        private void seeker_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e) {
            timer.Stop();
        }

        private void seeker_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e) {
            timer.Start();
        }

        private void seeker_MouseMove(object sender, System.Windows.Input.MouseEventArgs e) {
            MainViewModel vm = (MainViewModel)DataContext;
            Slider slider = sender as Slider;
            var Faktor = (vm.Player.getTotalTime() / slider.ActualWidth) / 360;
            var time_pos = Faktor * e.GetPosition(seeker).X;
            var time_tip = TimeSpan.FromMilliseconds(time_pos).ToString();
            var index = time_tip.IndexOf('.');
            if (index >= 0) {
                time_tip = time_tip.Substring(0, index);
            }
            vm.TimeTip = time_tip;
        }

        private void MenuItem_Checked(object sender, RoutedEventArgs e) {
            MenuItem item = sender as MenuItem;
            MainViewModel vm = (MainViewModel)DataContext;
            if (vm.IsTopMost == true) {
                item.IsChecked = false;
                vm.IsTopMost = false;
            } else {
                vm.IsTopMost = true;
                item.IsChecked = true;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e) {
            Process.GetCurrentProcess().Kill();
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            try {
                if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                    this.DragMove();
            } catch { }
        }

        private void show_volume_btn_Click(object sender, RoutedEventArgs e) {
            MainViewModel vm = (MainViewModel)DataContext;
            if (!volume_panel_visible) {
                vm.ShowOpacity();
                volume_panel_visible = true;
                volume_panel.Visibility = Visibility.Visible;
            } else {
                vm.HideOpacity();
                volume_panel_visible = false;
                volume_panel.Visibility = Visibility.Hidden;
            }
        }

        private void DockPanel_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e) {
            MainViewModel vm = (MainViewModel)DataContext;
            vm.HideOpacity();
            volume_panel.Visibility = Visibility.Hidden;
        }

        private void volume_control_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            var value = (double)e.NewValue;
            Properties.Settings.Default.volume = value;
            Properties.Settings.Default.Save();
        }

        private void show_volume_btn_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e) {
            MainViewModel vm = (MainViewModel)DataContext;
            vm.ShowOpacity();
            volume_panel.Visibility = Visibility.Visible;
            volume_panel_visible = true;
        }

        private void ScrollToSelected() {
            if (LV.SelectedItem != null && LV.SelectedIndex != -1) {
                LV.ScrollIntoView(LV.SelectedItem);
            }
        }

        private void LV_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ScrollToSelected();
        }


        private void next_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e) {
            MainViewModel vm = (MainViewModel)DataContext;
            vm.NextTrack();
        }

        private void playpause_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e) {
            MainViewModel vm = (MainViewModel)DataContext;
            vm.PlayPause();
        }

        private void prev_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e) {
            MainViewModel vm = (MainViewModel)DataContext;
            vm.PrevTrack();
        }

        private void stop_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e) {
            MainViewModel vm = (MainViewModel)DataContext;
            vm.PlayerStop();
            vm.Timer.Stop();
            vm.Time = null;
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e) {
            //SpectrumAnalyzer.Models.AnalyzerViewModel vis = (SpectrumAnalyzer.Models.AnalyzerViewModel)DataContext;
            //if(poster_check.IsChecked == false) {
            //    vis.End();
            //}
            //if(poster_check.IsChecked == true) {
            //    vis.Begin();
            //}
        }
    }
}
