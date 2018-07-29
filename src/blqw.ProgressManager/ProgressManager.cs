using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace blqw.Progress
{
    public class ProgressManager : Progressive, IProgressManager
    {
        ManageableProgressive[] _items = Array.Empty<ManageableProgressive>();

        private TaskCompletionSource<int> _taskCompletion = new TaskCompletionSource<int>();
        public override Task Start(CancellationToken cancellation)
        {
            if (StateDFA.Change(ProgressState.Progressing))
            {
                NextStart();
            }
            return _taskCompletion.Task;
        }

        public override void Suspend()
        {
            if (StateDFA.Change(ProgressState.Suspend))
            {
                foreach (var item in _items)
                {
                    if (item.SetState(ProgressState.Suspend))
                    {
                        item.SetState(ProgressState.Ready);
                    }
                }
            }
        }

        public Guid CreateId()
        {
            while (true)
            {
                var id = Guid.NewGuid();
                if (_items.Any(x => x.Id == id))
                {
                    continue;
                }
                return id;
            }
        }

        public ManageableProgressive Add(IProgressive progressive)
        {
            var mp = new ManageableProgressive(this, progressive);
            mp.ProgressCompleted += Item_ProgressCompleted;
            mp.ProgressValueChanged += Item_ProgressValueChanged;
            lock (this)
            {
                var @new = new ManageableProgressive[Count + 1];
                var i = 0;
                foreach (var item in _items)
                {
                    if (item != null)
                    {
                        @new[i] = item;
                        i++;
                    }
                }
                @new[i] = mp;
                Count = Count + 1;
                _items = @new;
            }
            return mp;
        }

        public IEnumerable<ManageableProgressive> AddRange(IEnumerable<IProgressive> progressives)
        {
            var mp = progressives.Select(x => new ManageableProgressive(this, x)).ToArray();
            lock (this)
            {
                var @new = new ManageableProgressive[Count + mp.Length];
                var i = 0;
                foreach (var item in _items)
                {
                    if (item != null)
                    {
                        @new[i] = item;
                        i++;
                    }
                }
                foreach (var m in mp)
                {
                    m.ProgressCompleted += Item_ProgressCompleted;
                    m.ProgressValueChanged += Item_ProgressValueChanged;
                    @new[i] = m;
                    i++;
                }
                Count = Count + mp.Length;
                _items = @new;
            }
            return mp;
        }

        readonly private Interval _interval = new Interval(200);
        private void Item_ProgressValueChanged(object sender, ProgressValueChangedEventArgs e)
        {
            if (!_interval.Update())
            {
                return;
            }
            var total = 0d;
            var value = 0d;
            foreach (var item in _items)
            {
                if (item.State != ProgressState.Initial && item.State != ProgressState.Suspend)
                {
                    total += item.Total;
                    value += item.Value;
                }
            }
            OnProgressValueChanged(total, value);
        }

        private void Item_ProgressCompleted(object sender, ProgressCompletedEventArgs e)
        {
            Interlocked.Decrement(ref _progressCount);
            OnProgressCompleted(Total, Value, sender);
            NextStart();
        }

        readonly object _locker = new object();
        int _timestamp = 0;
        int _progressCount = 0;
        int _parallelCount = 1;
        private void NextStart()
        {
            if (_progressCount >= _parallelCount)
            {
                return;
            }
            _timestamp = Environment.TickCount;
            if (Monitor.TryEnter(_locker))
            {
                var ts = _timestamp;
                try
                {

                    foreach (var item in _items)
                    {
                        if (item.State == ProgressState.Ready && item.SetState(ProgressState.Progressing))
                        {
                            if (Interlocked.Increment(ref _progressCount) >= _parallelCount)
                            {
                                if (_taskCompletion.Task.IsCompleted)
                                {
                                    _taskCompletion = new TaskCompletionSource<int>();
                                }
                                return;
                            }
                        }
                    }
                    if (_progressCount == 0)
                    {
                        _taskCompletion.TrySetResult(0);
                    }
                }
                finally
                {
                    Monitor.Exit(_locker);
                }
                if (ts != _timestamp)
                {
                    NextStart();
                }
            }
        }

        public ManageableProgressive Remove(Guid id)
        {
            lock (this)
            {
                for (var i = 0; i < _items.Length; i++)
                {
                    var mp = _items[i];
                    if (mp != null && mp.Id == id)
                    {
                        _items[i] = null;
                        Count--;
                        mp.ProgressCompleted -= Item_ProgressCompleted;
                        mp.ProgressValueChanged -= Item_ProgressValueChanged;
                        return mp;
                    }
                }
            }
            return null;
        }

        public ManageableProgressive this[Guid id] => _items.FirstOrDefault(x => x.Id == id);

        public int Count { get; private set; }

        public void Start(Guid id)
        {
            this[id]?.SetState(ProgressState.Ready);
            NextStart();
        }


        public void Suspend(Guid id) =>
            this[id]?.SetState(ProgressState.Suspend);

        public IEnumerator<ManageableProgressive> GetEnumerator()
        {
            foreach (var item in _items)
            {
                if (item != null)
                {
                    yield return item;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
