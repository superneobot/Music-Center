﻿using System.Diagnostics;
using System.Windows.Media;

namespace MediaCenter.SpectrumAnalyzer.Models {
    [DebuggerDisplay("{MinFrequency} - {MaxFrequency}Hz")]
    public class FrequencyBin : ViewModelBase {
        #region Fields

        private double _value;
        private int _minFrequency;
        private int _maxFrequency;
        private SolidColorBrush _idleColor = Brushes.DimGray;
        private SolidColorBrush _pitchColor = Brushes.DodgerBlue;

        #endregion Fields

        #region Properties

        public FrequencyBin(int value = 0) {
            Value = value;
        }

        public double Value {
            get => _value;
            set {
                _value = value;
                RaisePropertyChanged();
            }
        }

        public int MinFrequency {
            get => _minFrequency;
            set {
                _minFrequency = value;
                RaisePropertyChanged();
            }
        }

        public int MaxFrequency {
            get => _maxFrequency;
            set {
                _maxFrequency = value;
                RaisePropertyChanged();
            }
        }

        public SolidColorBrush IdleColor {
            get => _idleColor;
            set {
                _idleColor = value;
                RaisePropertyChanged();
            }
        }

        public SolidColorBrush PitchColor {
            get => _pitchColor;
            set {
                _pitchColor = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        public override string ToString() => $"{MinFrequency} - {MaxFrequency} Hz";

        #endregion Methods
    }
}