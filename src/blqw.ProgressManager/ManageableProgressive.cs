using blqw.Function;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace blqw.Progress
{
    public sealed class ManageableProgressive : IProgressHandler, IProgressive
    {
        internal ManageableProgressive(IProgressManager manager, IProgressive progressive)
        {
            Manager = manager ?? throw new ArgumentNullException(nameof(manager));
            _progressive = progressive ?? throw new ArgumentNullException(nameof(progressive));
            Id = Manager.CreateId();
            _taskCompletion = new TaskCompletionSource<int>();
            if (_progressive is Progressive p)
            {
                StateDFA = p.StateDFA;
                StateDFA.StateChanged += Progressive_ProgressStateChanged;
            }
            else
            {
                StateDFA = new DFA<ProgressState>(ProgressState.Initial, def =>
                {
                    def(ProgressState.Initial, new[] { ProgressState.Ready, ProgressState.Progressing, ProgressState.Error });
                    def(ProgressState.Ready, new[] { ProgressState.Progressing, ProgressState.Suspend, ProgressState.Error });
                    def(ProgressState.Progressing, new[] { ProgressState.Suspend, ProgressState.Completed, ProgressState.Error });
                    def(ProgressState.Suspend, new[] { ProgressState.Ready, ProgressState.Progressing, ProgressState.Error });
                });
                _progressive.ProgressStateChanged += Progressive_ProgressStateChanged;
            }
        }

        private void Progressive_ProgressStateChanged(object sender, StateChangedEventArgs<ProgressState> e)
        {
            switch (e.NewState)
            {
                case ProgressState.Initial:
                    break;
                case ProgressState.Ready:
                    break;
                case ProgressState.Progressing: //开始执行
                    _cancellation = new CancellationTokenSource();
                    _progressive.Start(_cancellation.Token).ContinueWith(x => _taskCompletion.TrySetResult(0));
                    break;
                case ProgressState.Suspend: //暂停执行
                    if (e.OldState == ProgressState.Progressing)
                    {
                        _cancellation.Cancel();
                    }
                    break;
                case ProgressState.Completed:
                    break;
                case ProgressState.Error:
                    break;
                default:
                    break;
            }
        }

        private DFA<ProgressState> StateDFA { get; }

        private readonly IProgressive _progressive;
        private readonly TaskCompletionSource<int> _taskCompletion;

        private CancellationTokenSource _cancellation;
        public IProgressManager Manager { get; }

        public Guid Id { get; }

        internal bool SetState(ProgressState state)
        {
            return StateDFA.Set(state);
        }

        public Task Start(CancellationToken cancellation)
        {
            Manager.Start(Id);
            return _taskCompletion.Task;
        }

        public void Suspend() => Manager.Suspend(Id);


        public event EventHandler<ProgressValueChangedEventArgs> ProgressValueChanged
        {
            add
            {
                _progressive.ProgressValueChanged += value;
            }
            remove
            {
                _progressive.ProgressValueChanged -= value;
            }
        }

        public event EventHandler<ProgressCompletedEventArgs> ProgressCompleted
        {
            add
            {
                _progressive.ProgressCompleted += value;
            }
            remove
            {
                _progressive.ProgressCompleted -= value;
            }
        }

        public double Total => _progressive.Total;

        public double Value => _progressive.Value;

        public double Percentage => _progressive.Percentage;

        public ProgressState State => StateDFA.State;

        public event EventHandler<ProgressStateChangedEventArgs> ProgressStateChanged
        {
            add
            {
                _progressive.ProgressStateChanged += value;
            }

            remove
            {
                _progressive.ProgressStateChanged -= value;
            }
        }
    }
}
