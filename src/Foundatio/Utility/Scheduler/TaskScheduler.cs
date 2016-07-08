using System;
using System.Threading.Tasks;
using Foundatio.Extensions;

namespace Foundatio.Utility {
    public sealed class TaskScheduler : IScheduler {
        public static TaskScheduler Instance { get; } = new TaskScheduler();

        private TaskScheduler() { }

        public IDisposable Schedule(TimeSpan dueTime, Func<IScheduler, IDisposable> action) {
            bool cancel = false;
            var dis = new CompositeDisposable {
                new DisposableAction(() => cancel = true)
            };

            Task.Run(async () => {
                await SystemClock.SleepAsync(SystemClock.Normalize(dueTime)).AnyContext();
                if (!cancel)
                    dis.Add(action(this));
            });

            return dis;
        }
    }
}