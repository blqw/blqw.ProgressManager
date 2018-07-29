using System;
using System.Threading;

namespace blqw.Progress
{
    public class ProgressHandler : IProgressHandler
    {
        private IProgressHandler _handler;
        private double _total;
        private double _value;

        public double Total => _total;

        public double Value => _value;

        public double Percentage => ProgerssHelper.Percentage(_total, _value);

        private EventHandler<ProgressValueChangedEventArgs> _progressValueChanged;

        private EventHandler<ProgressCompletedEventArgs> _progressCompleted;


        public event EventHandler<ProgressValueChangedEventArgs> ProgressValueChanged
        {
            add
            {
                _progressValueChanged -= value;
                _progressValueChanged += value;
            }
            remove { _progressValueChanged -= value; }
        }
        public event EventHandler<ProgressCompletedEventArgs> ProgressCompleted
        {
            add
            {
                _progressCompleted -= value;
                _progressCompleted += value;
            }
            remove { _progressCompleted -= value; }
        }

        private bool SetProgress(double total, double value) =>
            Interlocked.Exchange(ref _total, total) != total | Interlocked.Exchange(ref _value, value) != value;

        protected void OnProgressValueChanged(double total, double value)
        {
            if (SetProgress(total, value))
            {
                _progressValueChanged?.Invoke(this, new ProgressValueChangedEventArgs(total, value, null));
            }
        }

        protected virtual void OnProgressCompleted(double total, double value, object result)
        {
            SetProgress(total, value);
            _progressCompleted?.Invoke(this, new ProgressCompletedEventArgs(total, value, result, null, false));
        }

        protected void OnProgressError(double total, double value, Exception exception)
        {
            if (SetProgress(total, value))
            {
                _progressCompleted?.Invoke(this, new ProgressCompletedEventArgs(total, value, null, exception, exception is OperationCanceledException));
            }
        }

        protected IProgressible CreateReporter() =>
             new Progressible(this);

        class Progressible : IProgressible
        {
            private readonly ProgressHandler _progress;

            public Progressible(ProgressHandler progress) => _progress = progress;
            public void OnCompleted(ProgressCompletedEventArgs e)
            {
                if (_progress.SetProgress(e.Total, e.Value))
                {
                    _progress._progressCompleted?.Invoke(this, e);
                }
            }

            public event EventHandler<ProgressValueChangedEventArgs> ProgressValueChanged
            {
                add
                {
                    _progress._progressValueChanged -= value;
                    _progress._progressValueChanged += value;
                }
                remove { _progress._progressValueChanged -= value; }
            }
            public event EventHandler<ProgressCompletedEventArgs> ProgressCompleted
            {
                add
                {
                    _progress._progressCompleted -= value;
                    _progress._progressCompleted += value;
                }
                remove { _progress._progressCompleted -= value; }
            }

            public double Total => _progress.Total;

            public double Value => _progress.Value;

            public double Percentage => _progress.Percentage;

            public void Report(ProgressValueChangedEventArgs e)
            {
                if (_progress.SetProgress(e.Total, e.Value))
                {
                    _progress._progressValueChanged?.Invoke(this, e);
                }
            }
        }

    }
}
