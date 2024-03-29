﻿using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.DSP;
using CSCore.SoundIn;
using CSCore.Streams;
using MediaCenter.SpectrumAnalyzer.Enums;
using MediaCenter.SpectrumAnalyzer.Factories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace MediaCenter.SpectrumAnalyzer.Models {
    public class AnalyzerViewModel : ViewModelBase {
        /// <summary>
        ///     captures the default devices audio stream, performs an fft and stores the result in <see cref="FrequencyBins" />
        /// </summary>
        /// <param name="bins">number of frequency bins in <see cref="FrequencyBins" /></param>
        /// <param name="rate">number of refreshes per second</param>
        /// <param name="normal">normalized values in <see cref="FrequencyBins" /></param>
        public AnalyzerViewModel(int bins = 50, int rate = 50, int normal = 255) {
            Bins = bins;
            Rate = rate;
            Normal = normal;
            DetectBeats = true;

            Initialize();
        }

        #region Fields

        private const FftSize FftSize = CSCore.DSP.FftSize.Fft4096;
        private const int ScaleFactorLinear = 9;
        private const int ScaleFactorSqrt = 2;
        private const double MinDbValue = -90;
        private const double MaxDbValue = 0;
        private const double DbScale = MaxDbValue - MinDbValue;

        private readonly Timer _updateSpectrumTimer = new Timer();
        private MMDeviceEnumerator _deviceEnumerator;
        private WasapiLoopbackCapture _soundIn;
        private SpectrumProvider _spectrumProvider;
        private float[] _spectrumData;
        private IWaveSource _source;
        private Queue<float[]> _history;
        private int _minimumFrequencyIndex;
        private int _maximumFrequencyIndex;
        private int[] _spectrumLinearScaleIndexMax;
        private int[] _spectrumLogarithmicScaleIndexMax;

        private int _rate;
        private int _bins;
        private int _normal;
        private bool _detectBeats;
        private int _minFrequency = 20;
        private int _maxFrequency = 3000; // 20000?
        private ScalingStrategy _scalingStrategy = ScalingStrategy.Sqrt;
        private bool _logarithmicX = true;
        private bool _average;
        private MMDevice _currentAudioDevice;

        #endregion Fields

        #region Properties

        #region Input

        #region Frequency Analysis

        public int Bins {
            get => _bins;
            set {
                _bins = value;
                FrequencyBins = new ObservableCollection<FrequencyBin>(AnalyzerFactory.CreateMany(Bins));
                RaisePropertyChanged();
            }
        }

        public int Rate {
            get => _rate;
            set {
                _rate = value;
                _updateSpectrumTimer.Interval = 1000.0 / value;
                RaisePropertyChanged();
            }
        }

        public int Normal {
            get => _normal;
            set {
                _normal = value;
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

        public ScalingStrategy ScalingStrategy {
            get => _scalingStrategy;
            set {
                _scalingStrategy = value;
                RaisePropertyChanged();
            }
        }

        public bool LogarithmicX {
            get => _logarithmicX;
            set {
                _logarithmicX = value;
                RaisePropertyChanged();
            }
        }

        public bool Average {
            get => _average;
            set {
                _average = value;
                RaisePropertyChanged();
            }
        }

        #endregion Frequency Analysis

        #region Beat Detection

        public bool DetectBeats {
            get => _detectBeats;
            set {
                _detectBeats = value;
                if (value)
                    _updateSpectrumTimer.Elapsed += DetectObserverBand;
                else
                    _updateSpectrumTimer.Elapsed -= DetectObserverBand;
            }
        }

        public double BeatSensibility { get; set; }

        #endregion Beat Detection

        #endregion Input

        #region Output

        #region Frequency Analysis

        public ObservableCollection<FrequencyBin> FrequencyBins { get; private set; } = new ObservableCollection<FrequencyBin>();

        public MMDevice CurrentAudioDevice {
            get => _currentAudioDevice;
            set {
                _currentAudioDevice = value;
                RaisePropertyChanged();
            }
        }

        public AudioEndpointVolume AudioEndpointVolume { get; set; }

        #endregion Frequency Analysis

        #region Beat Detection

        public ObservableCollection<FrequencyObserver> FrequencyObservers { get; } =
            new ObservableCollection<FrequencyObserver>
            {
                // TODO load from file + editable
                new FrequencyObserver {Title = "Treble", MinFrequency = 5200, MaxFrequency = 20000},
                new FrequencyObserver {Title = "Mid", MinFrequency = 400, MaxFrequency = 5200},
                new FrequencyObserver {Title = "Bass", MinFrequency = 20, MaxFrequency = 400},
                new FrequencyObserver {Title = "Kick", MinFrequency = 108-30, MaxFrequency = 108+30, PitchColor = Brushes.White}
            };

        #endregion Beat Detection

        #endregion Output

        #region Private

        private static int SpectrumDataSize => (int)FftSize / 2 - 1;

        #endregion

        #endregion

        #region Private Methods

        private void Initialize() {
            Stop();
            InitializeCapture();
            _deviceEnumerator = new MMDeviceEnumerator();

            _deviceEnumerator.DefaultDeviceChanged += OnDefaultDeviceChanged;
            _updateSpectrumTimer.Elapsed += UpdateSpectrum;

            _updateSpectrumTimer.Start();
        }

        public void Begin() {
            Initialize();
        }
        public void End() {
            Stop();
        }

        private void OnDefaultDeviceChanged(object sender, DefaultDeviceChangedEventArgs e) {
            if (e.Role == Role.Multimedia) InitializeCapture();
        }

        private void InitializeCapture() {
            Stop();

            Application.Current.Dispatcher.Invoke(() => {
                FrequencyBins.Clear();
                foreach (var frequencyBin in AnalyzerFactory.CreateMany(Bins)) FrequencyBins.Add(frequencyBin);
            });

            _spectrumData = new float[(int)FftSize];
            _history = new Queue<float[]>(_rate);

            _soundIn = new WasapiLoopbackCapture();
            _soundIn.Initialize();
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => {
                CurrentAudioDevice = _soundIn.Device;
            }));
            //AudioEndpointVolume = AudioEndpointVolume.FromDevice(CurrentAudioDevice);

            var soundInSource = new SoundInSource(_soundIn);
            _spectrumProvider = new SpectrumProvider(soundInSource.WaveFormat.Channels,
                soundInSource.WaveFormat.SampleRate, FftSize);
            UpdateFrequencyMapping();

            var notificationSource = new SingleBlockNotificationStream(soundInSource.ToSampleSource());
            notificationSource.SingleBlockRead += (s, a) => _spectrumProvider.Add(a.Left, a.Right);
            ForceSingleBlockCall(soundInSource, notificationSource);

            _soundIn.Start();
        }

        internal void Stop() {
            //_updateSpectrumTimer.Stop();

            if (_soundIn != null) {
                _soundIn.Stop();
                //_soundIn.Dispose();
                _soundIn = null;
            }

            if (_source != null) {
                _source.Dispose();
                _source = null;
            }
        }

        // based on the https://github.com/filoe/cscore visualization example
        private void ForceSingleBlockCall(SoundInSource soundInSource, ISampleSource notificationSource) {
            _source = notificationSource.ToWaveSource(16);
            var buffer = new byte[_source.WaveFormat.BytesPerSecond / 2];
            soundInSource.DataAvailable += (s, aEvent) => {
                while (_source.Read(buffer, 0, buffer.Length) > 0) { }
            };
        }

        #endregion

        #region Frequencies

        // based on the https://github.com/filoe/cscore visualization example
        private void UpdateFrequencyMapping() {
            _minimumFrequencyIndex = Math.Min(_spectrumProvider.GetFftBandIndex(_minFrequency), SpectrumDataSize);
            _maximumFrequencyIndex = Math.Min(_spectrumProvider.GetFftBandIndex(_maxFrequency) + 1, SpectrumDataSize);

            var indexCount = _maximumFrequencyIndex - _minimumFrequencyIndex;
            var linearIndexBucketSize = Math.Round(indexCount / (double)_bins, 3);

            _spectrumLinearScaleIndexMax = _spectrumLinearScaleIndexMax.CheckBuffer(_bins, true);
            _spectrumLogarithmicScaleIndexMax = _spectrumLogarithmicScaleIndexMax.CheckBuffer(_bins, true);

            var maxLog = Math.Log(_bins, _bins);
            for (var i = 1; i <= _bins; i++) {
                var map = i - 1;
                var logIndex =
                    (int)((maxLog - Math.Log(_bins + 1 - i, _bins + 1)) * indexCount) +
                    _minimumFrequencyIndex;

                _spectrumLinearScaleIndexMax[map] = _minimumFrequencyIndex + (int)(i * linearIndexBucketSize);
                _spectrumLogarithmicScaleIndexMax[map] = logIndex;

                if (FrequencyBins is null) continue; // apply band to bin:
                var relatedBin = FrequencyBins[map];

                relatedBin.MinFrequency = map > 0
                    ? _spectrumProvider.GetFrequency(_logarithmicX
                        ? _spectrumLogarithmicScaleIndexMax[map - 1]
                        : _spectrumLinearScaleIndexMax[map - 1]) + 1
                    : MinFrequency;
                relatedBin.MaxFrequency = map < _bins - 1
                    ? _spectrumProvider.GetFrequency(_logarithmicX
                        ? _spectrumLogarithmicScaleIndexMax[map]
                        : _spectrumLinearScaleIndexMax[map])
                    : MaxFrequency;
            }

            if (_bins > 0)
                _spectrumLinearScaleIndexMax[_spectrumLinearScaleIndexMax.Length - 1] =
                    _spectrumLogarithmicScaleIndexMax[_spectrumLogarithmicScaleIndexMax.Length - 1] = _maximumFrequencyIndex;
        }

        // based on the https://github.com/filoe/cscore visualization example
        private void UpdateSpectrum(object sender, EventArgs e) {
            if (!_spectrumProvider.IsNewDataAvailable) {
                Application.Current?.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => {
                    foreach (var frequencyBin in FrequencyBins) frequencyBin.Value = 0;
                }));
                return;
            }

            _spectrumProvider.GetFftData(_spectrumData);

            double value0 = 0, value = 0;
            double lastValue = 0;
            double actualMaxValue = _normal;
            var spectrumPointIndex = 0;

            var frequencyBins = new double[Bins];

            for (var i = _minimumFrequencyIndex; i <= _maximumFrequencyIndex; i++) {
                switch (ScalingStrategy) {
                    case ScalingStrategy.Decibel:
                        value0 = (20 * Math.Log10(_spectrumData[i]) - MinDbValue) / DbScale * actualMaxValue;
                        break;
                    case ScalingStrategy.Linear:
                        value0 = _spectrumData[i] * ScaleFactorLinear * actualMaxValue;
                        break;
                    case ScalingStrategy.Sqrt:
                        value0 = Math.Sqrt(_spectrumData[i]) * ScaleFactorSqrt * actualMaxValue;
                        break;
                }

                var recalc = true;
                value = Math.Max(0, Math.Max(value0, value));

                while (spectrumPointIndex <= _spectrumLinearScaleIndexMax.Length - 1 &&
                       i == (_logarithmicX
                           ? _spectrumLogarithmicScaleIndexMax[spectrumPointIndex]
                           : _spectrumLinearScaleIndexMax[spectrumPointIndex])) {
                    if (!recalc)
                        value = lastValue;

                    if (value > Normal)
                        value = Normal;

                    if (_average && spectrumPointIndex > 0)
                        value = (lastValue + value) / 2.0;

                    frequencyBins[spectrumPointIndex] = value;
                    lastValue = value;
                    value = 0.0;
                    spectrumPointIndex++;
                    recalc = false;
                }
            }

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => {
                var index = 0;
                foreach (var frequencyBin in FrequencyBins) frequencyBin.Value = frequencyBins[index++];
            }));
        }

        #endregion

        #region Observers

        private void UpdateHistory() {
            if (_history.Count >= _rate) _history.Dequeue();
            _history.Enqueue(_spectrumData.ToArray());
        }

        private float[] CalculateAverages() {
            var avg = new float[SpectrumDataSize];
            if (_history.Count < _rate) return avg;
            try {
                for (var frequencyIndex = 0; frequencyIndex < SpectrumDataSize; frequencyIndex++) {

                    avg[frequencyIndex] = _history.Sum(spectrum => {
                        return spectrum[frequencyIndex];
                    }) / _rate;
                }
            } catch { }
            return avg;
        }

        private float GetFrequencyPool(IReadOnlyList<float> spectrum, int from, int to) {
            var avgFromTo = 0f;
            var minFreqIndex = Math.Min(_spectrumProvider.GetFftBandIndex(from), SpectrumDataSize);
            var maxFreqIndex = Math.Min(_spectrumProvider.GetFftBandIndex(to) + 1, SpectrumDataSize);
            if (minFreqIndex > maxFreqIndex) return avgFromTo;

            for (var frequencyIndex = minFreqIndex; frequencyIndex < maxFreqIndex; frequencyIndex++)
                avgFromTo += spectrum[frequencyIndex];
            return avgFromTo / (maxFreqIndex - minFreqIndex);
        }

        private void DetectObserverBand(object sender, EventArgs e) {
            var historyAverage = CalculateAverages();
            foreach (var fo in FrequencyObservers) {
                var cur = GetFrequencyPool(_spectrumData, fo.MinFrequency, fo.MaxFrequency);
                if (_history.Count < _rate) continue;

                Application.Current?.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => {
                    fo.AdjustAverage(cur);
                    var avg = GetFrequencyPool(historyAverage, fo.MinFrequency, fo.MaxFrequency);
                    fo.BeatDetected = cur > fo.AverageEnergyThreshold && cur > avg * fo.AverageFactor;
                }));
            }
            UpdateHistory();
        }

        #endregion
    }
}