using System.Windows;
using System.Windows.Controls;

namespace MediaCenter.Model {
    public class ListViewSelector : DataTemplateSelector {
        public DataTemplate MusicData { get; set; }
        public DataTemplate CollectionsData { get; set; }
        public DataTemplate RadioData { get; set; }
        public DataTemplate UsersChartData { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container) {
            var type = item as DataSource;
            switch (type.Type) {
                case SourceType.Music:
                    return MusicData;
                case SourceType.Collection:
                    return CollectionsData;
                case SourceType.Radio:
                    return RadioData;
                case SourceType.Users:
                    return UsersChartData;
                default:
                    return null;
            }
        }
    }
}
