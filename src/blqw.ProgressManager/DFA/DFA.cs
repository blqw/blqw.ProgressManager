using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace blqw.Function
{
    /// <summary>
    /// 状态自动机
    /// </summary>
    public class DFA<T>
        where T : struct, IComparable, IConvertible, IFormattable
    {

        static DFA()
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception("T 必须是 枚举类型");
            }
        }

        protected virtual IEnumerable<(T pre, T next)> StateTransferDefinition() => throw new NotImplementedException();

        /// <summary>
        /// 自动机顺序定义
        /// </summary>
        private readonly IList<(long curr, long next)> _definition;

        /// <summary>
        /// 初始化 确定有限状态自动机
        /// </summary>
        /// <param name="state">初始状态</param>
        protected DFA(T state) : this() => _state = state.ToInt64(null);

        public DFA(T state, Action<Action<T, T[]>> init)
        {
            if (init == null)
            {
                throw new ArgumentNullException(nameof(init));
            }

            _state = state.ToInt64(null);
            var definition = new List<(T pre, T next)>();
            init((pre, nexts) =>
            {
                foreach (var next in nexts)
                {
                    definition.Add((pre, next));
                }
            });
            _definition = definition.Select(x => (x.pre.ToInt64(null), x.next.ToInt64(null))).ToList().AsReadOnly();
        }

        protected DFA() => _definition = StateTransferDefinition().Select(x => (x.pre.ToInt64(null), x.next.ToInt64(null))).ToList().AsReadOnly();

        private static T Cast(long value) => (T)Enum.ToObject(typeof(T), value);

        private long _state;

        /// <summary>
        /// 当前状态
        /// </summary>
        public T State => Cast(Interlocked.Read(ref _state));

        /// <summary>
        /// 设置当前状态, 并返回之前的状态, 如果当前状态已经是指定状态, 返回true
        /// </summary>
        /// <param name="state"></param>
        /// <param name="pre">之前的状态</param>
        /// <returns></returns>
        public bool Set(T state, out T pre) => Change(state, out pre) || Equals(State, state);

        /// <summary>
        /// 设置当前状态，如果当前状态已经是指定状态, 返回true
        /// </summary>
        public bool Set(T state) => Set(state, out _);

        /// <summary>
        /// 改变当前状态, 并返回之前的状态, 只有当状态发生变化时, 才返回true
        /// </summary>
        /// <param name="state"></param>
        /// <param name="pre">之前的状态</param>
        /// <returns></returns>
        public bool Change(T state, out T pre)
        {
            var curr = _state;
            var next = state.ToInt64(null);
            if (_definition.Any(x => x.curr == curr && x.next == next))
            {
                var original = Interlocked.CompareExchange(ref _state, next, curr);
                if (original == curr)
                {
                    pre = Cast(curr);
                    OnStateChanged(pre, state);
                    return true;
                }
                curr = original;
            }
            pre = Cast(curr);
            OnStateChangeFailed(pre, state);
            return false;
        }

        /// <summary>
        /// 改变当前状态, 并返回之前的状态, 只有当状态发生变化时, 才返回true
        /// </summary>
        public bool Change(T state) => Change(state, out _);

        public static bool operator ==(DFA<T> dfa, T other)
        {
            var state = other.ToInt64(null);
            return Interlocked.CompareExchange(ref dfa._state, state, state) == state;
        }

        public static bool operator !=(DFA<T> dfa, T other)
        {
            var state = other.ToInt64(null);
            return Interlocked.CompareExchange(ref dfa._state, state, state) != state;
        }

        EventHandler<StateChangedEventArgs<T>> _stateChanged;

        /// <summary>
        /// 状态变化事件
        /// </summary>
        public event EventHandler<StateChangedEventArgs<T>> StateChanged
        {
            add
            {
                _stateChanged -= value;
                _stateChanged += value;
            }
            remove => _stateChanged -= value;
        }

        /// <summary>
        /// 触发状态变化事件
        /// </summary>
        /// <param name="oldState"></param>
        /// <param name="newState"></param>
        protected void OnStateChanged(T oldState, T newState) =>
            _stateChanged?.Invoke(this, new StateChangedEventArgs<T>(oldState, newState));



        EventHandler<StateChangeFailedEventArgs<T>> _stateChangeFailed;

        /// <summary>
        /// 状态变化事件
        /// </summary>
        public event EventHandler<StateChangeFailedEventArgs<T>> StateChangeFailed
        {
            add
            {
                _stateChangeFailed -= value;
                _stateChangeFailed += value;
            }
            remove => _stateChangeFailed -= value;
        }

        /// <summary>
        /// 触发状态变化事件
        /// </summary>
        /// <param name="oldState"></param>
        /// <param name="newState"></param>
        protected void OnStateChangeFailed(T currentState, T expectState) =>
            _stateChangeFailed?.Invoke(this, new StateChangeFailedEventArgs<T>(currentState, expectState));
    }
}
