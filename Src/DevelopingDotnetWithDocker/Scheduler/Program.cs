// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Scheduler;


CreateHostBuilder(args).Build().Run();


IHostBuilder CreateHostBuilder(string[] args)
{
    var host = Host.CreateDefaultBuilder(args);

    // See if we are running in Development mode and add the appsettings.Development.json file to our config
    var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

    Console.Out.WriteLine($"Environment: {env}");

    // This will add the configuration to the application. It will allow you to inject the configuration as 
    // IConfiguration configuration in your constructors 
    host.ConfigureAppConfiguration(
        (hostContext, config) =>
        {
            config.SetBasePath(Directory.GetCurrentDirectory());
            config.AddJsonFile("appsettings.json", false, true);
            
            if(!string.IsNullOrEmpty(env))
                config.AddJsonFile($"appsettings.{env}.json", true, true);
            
            config.AddCommandLine(args);
        }
    );
    
    // The AddHostedServer adds the worker service to the application.
    host.ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<SchedulerBackgroundWorker>();
    });
    
    return host;
}
