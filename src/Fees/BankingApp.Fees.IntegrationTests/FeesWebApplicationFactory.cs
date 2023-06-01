﻿namespace BankingApp.Fees.IntegrationTests;

public class FeesWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
            var profitFeeDescriptor = services.FirstOrDefault(descriptor =>
                descriptor.ImplementationType == typeof(ProfitFeeBackgroundService));
            var overdraftFeeDescriptor = services.FirstOrDefault(descriptor =>
                descriptor.ImplementationType == typeof(OverdraftFeeBackgroundService));

            if (profitFeeDescriptor is not null)
            {
                services.Remove(profitFeeDescriptor);
            }

            if (overdraftFeeDescriptor is not null)
            {
                services.Remove(overdraftFeeDescriptor);
            }
        });
    }
}