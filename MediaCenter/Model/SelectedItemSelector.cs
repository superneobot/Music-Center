using System.Windows;

namespace MediaCenter.Model {
    public class ListViewSelectedItemSelector : DataTemplate {
        public DataSource SelectedMusic { get; set; }
        public Collection SelectedCollection { get; set; }

        public object SelectTemplate(object item) {
            var type = item.GetType();

            switch (type.Name) {
                case "MusicFile":
                    return SelectedMusic;
                case "Collection":
                    return SelectedCollection;
                default:
                    return null;
            }
        }
    }
}
