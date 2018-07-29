using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace blqw.Progress
{
    public class SimplyProgressive : Progressive
    {
        private readonly Func<IProgress<ProgressValueChangedEventArgs>, CancellationToken, object> _func;
        private readonly Action<IProgress<ProgressValueChangedEventArgs>, CancellationToken> _action;

        public SimplyProgressive(Action<IProgress<ProgressValueChangedEventArgs>, CancellationToken> action) =>
            _action = action ?? throw new ArgumentNullException(nameof(action));

        public SimplyProgressive(Func<IProgress<ProgressValueChangedEventArgs>, CancellationToken, object> func) =>
            _func = func ?? throw new ArgumentNullException(nameof(func));

        public override Task Start(CancellationToken cancellation)
        {
            var source = new CancellationTokenSource();
            if (Interlocked.CompareExchange(ref _tokenSource, source, null) != null)
            {
                return _startTask;
            }
            var token = source.Token;
            return _startTask = Task.Run(() => StartTask(token), token).ContinueWith(t =>
            {
                _startTask = null;
                source = null;
            });
        }

        CancellationTokenSource _tokenSource;
        Task _startTask;
        private void StartTask(CancellationToken token)
        {
            try
            {
                var reproter = CreateReporter();
                var temp = _func;
                if (temp != null)
                {
                    var result = temp(reproter, token);
                    OnProgressCompleted(Total, Total, result);
                }
                else
                {
                    _action?.Invoke(reproter, token);
                    OnProgressCompleted(Total, Total, null);
                }
            }
            catch (Exception ex)
            {
                OnProgressError(Total, Total, ex);
            }
        }

        public override void Suspend() => _tokenSource?.Cancel();
    }
}
