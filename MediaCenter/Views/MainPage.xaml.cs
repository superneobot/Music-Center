using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MediaCenter {
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page {
        public MainPage() {
            InitializeComponent();
        }

        private void LV_SelectionChanged(object sender, SelectionChangedEventArgs e) {

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
