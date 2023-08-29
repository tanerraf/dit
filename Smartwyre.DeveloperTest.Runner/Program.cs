using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Runner;
using Smartwyre.DeveloperTest.Services;
using System;

using IHost host = CreateHostBuilder(args).Build();
using var scope = host.Services.CreateScope();

var services = scope.ServiceProvider;

try
{
    services.GetRequiredService<App>().Run(args);
}
catch (Exception e)
{
    Console.Error.WriteLine(e.Message);
}

IHostBuilder CreateHostBuilder(string[] strings)
{
    return Host.CreateDefaultBuilder()
        .ConfigureServices((_, services) =>
        {
            services.AddSingleton<IRebateDataStore, RebateDataStore>();
            services.AddSingleton<IProductDataStore, ProductDataStore>();
            services.AddSingleton<IRebateService, RebateService>();
            services.AddSingleton<App>();
        });
}