using System;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace DesafioWarren.Application.Quartz
{
    public static class QuartzExtensions
    {
        private static void AddEarningsJob(this IServiceCollectionQuartzConfigurator serviceCollectionQuartzConfigurator)
        {
            const string jobName = nameof(AccountsEarningJob);

            var cronExpression = CronScheduleBuilder.DailyAtHourAndMinute(00, 00)
                .InTimeZone(TimeZoneInfo.Local);

            var jobKey = new JobKey($"Quartz:{jobName}");

            serviceCollectionQuartzConfigurator.AddJob<AccountsEarningJob>(jobConfigurator =>
                jobConfigurator.WithIdentity(jobKey));

            serviceCollectionQuartzConfigurator.AddTrigger(triggerConfiguration => triggerConfiguration.ForJob(jobKey)
                .WithIdentity($"{jobName}-trigger")
                .WithSchedule(cronExpression));

        }


        public static IServiceCollection AddQuartzJobs(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddQuartz(quartz =>
            {
                quartz.UseMicrosoftDependencyInjectionScopedJobFactory();

                quartz.AddEarningsJob();
            });

            return serviceCollection;
        }
    }
}