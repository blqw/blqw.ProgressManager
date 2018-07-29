using blqw.Progress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = new ProgressManager();

            manager.Add(new SimplyProgressive(100, (e, token) =>
            {
                var total = 100;
                var value = 0;
                var r = new Random();
                while (value < 100)
                {
                    Thread.Sleep(r.Next(200, 800));
                    value += r.Next(1, 30);
                    e.Report(new ProgressValueChangedEventArgs(total, Math.Min(total, value), null));
                }
                e.Report(new ProgressValueChangedEventArgs(total, total, null));
                Console.WriteLine("A完成");
            })).Start(CancellationToken.None);

            manager.Add(new SimplyProgressive(100, (e, token) =>
            {
                var total = 100;
                var value = 0;
                var r = new Random();
                while (value < 100)
                {
                    Thread.Sleep(r.Next(200, 800));
                    value += r.Next(1, 30);
                    e.Report(new ProgressValueChangedEventArgs(total, Math.Min(total, value), null));
                }
                e.Report(new ProgressValueChangedEventArgs(total, total, null));
                Console.WriteLine("B完成");
            })).Start(CancellationToken.None);
            manager.Add(new SimplyProgressive(100, (e, token) =>
            {
                var total = 100;
                var value = 0;
                var r = new Random();
                while (value < 100)
                {
                    Thread.Sleep(r.Next(200, 800));
                    value += r.Next(1, 30);
                    e.Report(new ProgressValueChangedEventArgs(total, Math.Min(total, value), null));
                }
                e.Report(new ProgressValueChangedEventArgs(total, total, null));
                Console.WriteLine("C完成");
            })).Start(CancellationToken.None);
            manager.Add(new SimplyProgressive(100, (e, token) =>
            {
                var total = 100;
                var value = 0;
                var r = new Random();
                while (value < 100)
                {
                    Thread.Sleep(r.Next(200, 800));
                    value += r.Next(1, 30);
                    e.Report(new ProgressValueChangedEventArgs(total, Math.Min(total, value), null));
                }
                e.Report(new ProgressValueChangedEventArgs(total, total, null));
                Console.WriteLine("D完成");
            })).Start(CancellationToken.None);

            manager.ProgressValueChanged += Manager_ProgressValueChanged;

            manager.Start(CancellationToken.None).Wait();
            Console.WriteLine("完成");
            Console.ReadLine();
        }

        private static void Manager_ProgressValueChanged(object sender, ProgressValueChangedEventArgs e)
        {
            Console.Title = "处理中 " + e.Percentage + " %...";
        }
    }
}
