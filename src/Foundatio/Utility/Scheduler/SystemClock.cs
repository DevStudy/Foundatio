using System;
using System.Threading;
using System.Threading.Tasks;

namespace Foundatio.Utility {
    public static class SystemClock {
        public static Action<int> SleepFunc = Thread.Sleep;
        public static Func<int, CancellationToken, Task> SleepAsyncFunc = Task.Delay;
        public static Func<DateTime> UtcNowFunc = () => DateTime.UtcNow;

        public static DateTime UtcNow => UtcNowFunc();

        public static TimeSpan Normalize(TimeSpan timeSpan) {
            return timeSpan >= TimeSpan.Zero ? timeSpan : TimeSpan.Zero;
        }

        public static void Sleep(TimeSpan time) {
            SleepFunc((int)time.TotalMilliseconds);
        }

        public static void Sleep(int time) {
            SleepFunc(time);
        }

        public static Task SleepAsync(TimeSpan time, CancellationToken cancellationToken = default(CancellationToken)) {
            return SleepAsyncFunc((int)time.TotalMilliseconds, cancellationToken);
        }

        public static Task SleepAsync(int milliseconds, CancellationToken cancellationToken = default(CancellationToken)) {
            return SleepAsync(TimeSpan.FromMilliseconds(milliseconds), cancellationToken);
        }

        public static IScheduler Scheduler { get; set; } = TaskScheduler.Instance;

        public static void Reset() {
            SleepFunc = Thread.Sleep;
            SleepAsyncFunc = Task.Delay;
            UtcNowFunc = () => DateTime.UtcNow;
            Scheduler = TaskScheduler.Instance;
        }
    }
}