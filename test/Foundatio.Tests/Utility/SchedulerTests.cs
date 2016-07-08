using System;
using System.Diagnostics;
using System.Threading;
using Foundatio.Logging.Xunit;
using Foundatio.Utility;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;

namespace Foundatio.Tests.Utility {
    public class SchedulerTests : TestWithLoggingBase {
        public SchedulerTests(ITestOutputHelper output) : base(output) { }
        
        [Fact]
        public void CanSleep() {
            var sw = Stopwatch.StartNew();
            SystemClock.Sleep(500);
            sw.Stop();

            Assert.InRange(sw.Elapsed.TotalMilliseconds, 500, 1500);

            SystemClock.SleepFunc = d => {};

            sw = Stopwatch.StartNew();
            SystemClock.Sleep(500);
            sw.Stop();

            Assert.InRange(sw.Elapsed.TotalMilliseconds, 0, 50);
        }

        [Fact]
        public async Task CanSleepAsync() {
            var sw = Stopwatch.StartNew();
            await SystemClock.SleepAsync(500);
            sw.Stop();

            Assert.InRange(sw.Elapsed.TotalMilliseconds, 500, 1500);

            SystemClock.SleepAsyncFunc = (d, ct) => Task.CompletedTask;

            sw = Stopwatch.StartNew();
            await SystemClock.SleepAsync(500);
            sw.Stop();

            Assert.InRange(sw.Elapsed.TotalMilliseconds, 0, 50);
        }
        
        [Fact]
        public void CanSchedule() {
            var c = new CountdownEvent(2);
            var sw = Stopwatch.StartNew();
            SystemClock.Scheduler.Schedule(TimeSpan.FromMilliseconds(500), () => {
                sw.Stop();
                Assert.InRange(sw.Elapsed.TotalMilliseconds, 500, 1500);
                c.Signal();
            });

            SystemClock.SleepAsyncFunc = (d, ct) => Task.CompletedTask;

            sw = Stopwatch.StartNew();
            SystemClock.Scheduler.Schedule(TimeSpan.FromMilliseconds(500), () => {
                sw.Stop();
                Assert.InRange(sw.Elapsed.TotalMilliseconds, 0, 50);
                c.Signal();
            });

            Assert.True(c.Wait(2000));
        }
    }
}
