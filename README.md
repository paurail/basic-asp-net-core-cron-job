# Purpose

Having a lightweight, easy to configure cron service.

# Usage:

Inherit from base `BasicCronHostedService`, provide cron schedule and action

```
public class SampleNightlyService: BasicCronHostedService
    {
        private static string EveryNightAt2AM = "0 0 2 * * *";

        public SampleNightlyService(SomeProcess process):base(process.Execute, EveryNightAt2AM)
        {
        }
    }
```

Bootstrap in Startup

`services.AddHostedService<SampleNightlyService>();`

# Scheduling
Cron format. More at https://github.com/atifaziz/NCrontab
