using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NCrontab;

namespace AspNetCore.CronJob
{
    public class BasicCronHostedService: IHostedService
    {
        private readonly CrontabSchedule _crontabSchedule;
        private DateTime _nextRun;
        private readonly Func<Task> _action;

        public BasicCronHostedService(Func<Task> action, string schedule)
        {
            _action = action;
            _crontabSchedule = CrontabSchedule.Parse(schedule, new CrontabSchedule.ParseOptions{IncludingSeconds = true});
            _nextRun = _crontabSchedule.GetNextOccurrence(DateTime.Now);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(UntilNextExecution(), cancellationToken);

                    await _action();

                    _nextRun = _crontabSchedule.GetNextOccurrence(DateTime.Now);
                }
            }, cancellationToken);

            return Task.CompletedTask;
        }

        private TimeSpan UntilNextExecution() => _nextRun.Subtract(DateTime.Now);

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}