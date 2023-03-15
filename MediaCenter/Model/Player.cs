using NAudio.Wave;
using System.Threading.Tasks;

namespace MediaCenter.Model {
    public class Player {
        private IWavePlayer _waveout { get; set; }
        private AudioFileReader _reader { get; set; }
        public NAudio.Wave.PlaybackState State { get; set; }
        public Player(long volume) {
             _waveout = new WaveOut();
            _waveout.Volume = volume / 100;
        }
        public Task Play(string file) {
            try {
                if (_waveout == null | _reader == null) {
                    _waveout = new WaveOut();
                    //var format = _waveout.OutputWaveFormat;

                    _waveout.PlaybackStopped += (s, a) => {
                        {
                            _waveout.Dispose();
                            _reader.Dispose();
                            _waveout = null;
                            _reader = null;
                            State = PlaybackState.Stopped;
                        }
                    };
                }
                if (_reader == null | _reader != null) {

                    _reader = new AudioFileReader(file);
                    _waveout.Init(_reader);

                }
                _waveout.Play();
                State = PlaybackState.Playing;
            } catch { }
            //catch (Exception e) { MessageBox.Show("Скорее всего этот трек не доступен!", "Media Center", MessageBoxButton.OK, MessageBoxImage.Information); };
            return Task.CompletedTask;
        }
        public void PlayNext(string file) {
            if (_waveout != null & _reader != null) {
                _waveout.Stop();
                _waveout = new NAudio.Wave.WaveOut();
                _reader = new NAudio.Wave.AudioFileReader(file);
                _waveout.PlaybackStopped += (s, a) => {
                    {
                        _waveout.Dispose(); _reader.Dispose(); _waveout = null; _reader = null; State = PlaybackState.Stopped;
                    }
                };
                _waveout.Init(_reader);
            }
            _waveout?.Play();
            State = PlaybackState.Playing;
        }
        public Task Stop() {
            _waveout?.Stop();
            _waveout?.Dispose();
            _reader?.Dispose();
            _waveout = null;
            _reader = null;
            State = PlaybackState.Stopped;
            return Task.CompletedTask;
        }
        public Task Pause() {
            _waveout?.Pause();
            State = PlaybackState.Paused;
            return Task.CompletedTask;
        }
        public Task Resume() {
            _waveout.Play();
            State = PlaybackState.Playing;
            return Task.CompletedTask;
        }
        public string getPositionString() {
            var time = "00:00";
            if (_reader != null)
                time = _reader.CurrentTime.ToString(@"mm\:ss");
            return time;
        }
        public string getTotalTimeString() {
            var time = "00:00";
            if (_reader != null)
                time = _reader?.TotalTime.ToString(@"mm\:ss");
            return time;
        }
        public float getVolume() {
            return _waveout.Volume;
        }
        public void setVolume(float volume) {
            if (_waveout != null)
                _waveout.Volume = volume;
        }
        public long getTotalTime() {
            long time = 10;
            if (_reader != null)
                time = _reader.Length;
            return time;
        }
        public long getPosition() {
            long pos = 0;
            if (_reader != null)
                pos = _reader.Position;
            return pos;
        }
        public Task setPosition(long position) {
            if (_reader != null)
                _reader.Position = position;
            return Task.CompletedTask;
        }
    }
}
