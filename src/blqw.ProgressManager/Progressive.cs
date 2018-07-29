using blqw.Function;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace blqw.Progress
{
    public abstract class Progressive : ProgressHandler, IProgressive
    {
        public Progressive()
        {
            StateDFA = new DFA<ProgressState>(ProgressState.Initial, def =>
            {
                def(ProgressState.Initial, new[] { ProgressState.Ready, ProgressState.Progressing, ProgressState.Error });
                def(ProgressState.Ready, new[] { ProgressState.Progressing, ProgressState.Suspend, ProgressState.Error });
                def(ProgressState.Progressing, new[] { ProgressState.Suspend, ProgressState.Completed, ProgressState.Error });
                def(ProgressState.Suspend, new[] { ProgressState.Ready, ProgressState.Progressing, ProgressState.Error });
            });
            StateDFA.StateChanged += DFAStateChanged;
        }

        private void DFAStateChanged(object sender, StateChangedEventArgs<ProgressState> e) =>
            OnStateChanged(e.OldState, e.NewState);

        protected virtual void OnStateChanged(ProgressState oldState, ProgressState newState) =>
            _progressStateChanged?.Invoke(this, new ProgressStateChangedEventArgs(oldState, newState));

        protected internal DFA<ProgressState> StateDFA { get; }

        public ProgressState State => StateDFA.State;

        public abstract Task Start(CancellationToken cancellation);

        public abstract void Suspend();

        private EventHandler<ProgressStateChangedEventArgs> _progressStateChanged;

        public event EventHandler<ProgressStateChangedEventArgs> ProgressStateChanged
        {
            add
            {
                _progressStateChanged -= value;
                _progressStateChanged += value;
            }
            remove { _progressStateChanged -= value; }
        }

        protected override void OnProgressCompleted(double total, double value, object result)
        {
            if (StateDFA.Change(ProgressState.Completed))
            {
                base.OnProgressCompleted(total, value, result);
            }
        }
    }
}
