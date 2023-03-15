using MediaCenter.SpectrumAnalyzer.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MediaCenter.SpectrumAnalyzer.Models {
    public class ViewModelBase : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}