using System.Windows;
using System.Windows.Controls;

namespace MediaCenter.Model {
    public class ListViewStyleSelector : StyleSelector {
        public Style MusicStyle { get; set; }
        public Style CollectionsStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container) {
            var type = item as DataSource;

            switch (type.Type) {
                case SourceType.Music:
                    return MusicStyle;
                case SourceType.Collection:
                    return CollectionsStyle;
                default:
                    return null;
            }
        }
    }
}
