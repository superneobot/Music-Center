using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using MediaCenter.ViewModel;
using BlurryControls.Internals;
using BlurryControls.Controls;
using System.Diagnostics;

namespace MediaCenter {
    /// <summary>
    /// Логика взаимодействия для Compact.xaml
    /// </summary>
    public partial class Compact {
        public string poster_path;
        public Compact() {
            InitializeComponent();
        }

        #region Window styles
        [Flags]
        public enum ExtendedWindowStyles {
            // ...
            WS_EX_TOOLWINDOW = 0x00000080,
            // ...
        }

        public enum GetWindowLongFields {
            // ...
            GWL_EXSTYLE = (-20),
            // ...
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong) {
            int error = 0;
            IntPtr result = IntPtr.Zero;
            // Win32 SetWindowLong doesn't clear error on success
            SetLastError(0);

            if (IntPtr.Size == 4) {
                // use SetWindowLong
                Int32 tempResult = IntSetWindowLong(hWnd, nIndex, IntPtrToInt32(dwNewLong));
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(tempResult);
            } else {
                // use SetWindowLongPtr
                result = IntSetWindowLongPtr(hWnd, nIndex, dwNewLong);
                error = Marshal.GetLastWin32Error();
            }

            if ((result == IntPtr.Zero) && (error != 0)) {
                throw new System.ComponentModel.Win32Exception(error);
            }

            return result;
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr IntSetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        private static extern Int32 IntSetWindowLong(IntPtr hWnd, int nIndex, Int32 dwNewLong);

        private static int IntPtrToInt32(IntPtr intPtr) {
            return unchecked((int)intPtr.ToInt64());
        }

        [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
        public static extern void SetLastError(int dwErrorCode);
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width - 50;
            this.Top = desktopWorkingArea.Top + 50;

            WindowInteropHelper wndHelper = new WindowInteropHelper(this);

            int exStyle = (int)GetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE);

            exStyle |= (int)ExtendedWindowStyles.WS_EX_TOOLWINDOW;
            SetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE, (IntPtr)exStyle);
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                this.DragMove();
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e) {
            close.Visibility = Visibility.Visible;
            back.Visibility = Visibility.Visible;
            back_close_buttons.Visibility = Visibility.Visible;
            controls.Visibility = Visibility.Visible;
            MainViewModel vm = (MainViewModel)DataContext;
            vm.ShowOpacity();
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e) {
            close.Visibility = Visibility.Hidden;
            back.Visibility = Visibility.Hidden;
            back_close_buttons.Visibility = Visibility.Hidden;
            controls.Visibility = Visibility.Hidden;
            MainViewModel vm = (MainViewModel)DataContext;
            vm.HideOpacity();
        }

        private void next_Executed(object sender, ExecutedRoutedEventArgs e) {
            MainViewModel vm = (MainViewModel)DataContext;
            vm.NextTrack();
        }

        private void playpause_Executed(object sender, ExecutedRoutedEventArgs e) {
            MainViewModel vm = (MainViewModel)DataContext;
            vm.PlayPause();
        }

        private void prev_Executed(object sender, ExecutedRoutedEventArgs e) {
            MainViewModel vm = (MainViewModel)DataContext;
            vm.PrevTrack();
        }

        private void stop_Executed(object sender, ExecutedRoutedEventArgs e) {
            MainViewModel vm = (MainViewModel)DataContext;
            vm.PlayerStop();
            vm.Timer.Stop();
            vm.Time = null;
        }


        private void back_Click(object sender, RoutedEventArgs e) {
            this.Close();
            var window = Application.Current.MainWindow;
            window.Show();
        }

        private void close_Click(object sender, RoutedEventArgs e) {
            Process.GetCurrentProcess().Kill();
        }
    }
}
