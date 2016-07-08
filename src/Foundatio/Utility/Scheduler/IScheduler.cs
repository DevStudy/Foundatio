using System;
using System.Threading.Tasks;

namespace Foundatio.Utility {
    public interface IScheduler {
        IDisposable Schedule(TimeSpan dueTime, Func<IScheduler, IDisposable> action);
    }

    public static class SchedulerExtensions {
        public static IDisposable Schedule(this IScheduler scheduler, TimeSpan dueTime, Func<Task> action) {
            return scheduler.Schedule(dueTime, s => new DisposableAction(() => action().GetAwaiter().GetResult()));
        }

        public static IDisposable Schedule(this IScheduler scheduler, TimeSpan dueTime, Action action) {
            return scheduler.Schedule(dueTime, s => new DisposableAction(action));
        }
    }
}