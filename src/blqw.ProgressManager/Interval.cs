using System;
using System.Collections.Generic;
using System.Text;

namespace blqw.Progress
{
    class Interval
    {
        private readonly int _milliseconds;
        private int _next = 0;
        public Interval(int milliseconds) => _milliseconds = milliseconds;

        public int IntervalMilliseconds { get; }

        public bool Update()
        {
            var curr = Environment.TickCount;
            if (_next > curr)
            {
                return false;
            }
            lock (this)
            {
                if (_next > curr)
                {
                    return false;
                }
                _next = curr + _milliseconds;
                return true;
            }
        }
    }
}
