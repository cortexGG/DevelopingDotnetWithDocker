using System.Data;
using System.Data.Common;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Data.SqlClient;

namespace Scheduler
{
    public class SchedulerBackgroundWorker : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly TimeSpan _waitTime = TimeSpan.FromSeconds(10);
        private readonly string _connStr;

        public SchedulerBackgroundWorker(IConfiguration configuration)
        {
            _configuration = configuration;
            _connStr = configuration.GetConnectionString("BikeStore");

            // Output some diagnostics
            var builder = new DbConnectionStringBuilder { ConnectionString = _connStr };
            var server = builder["Server"] as string;
            var database = builder["Initial Catalog"] as string;                    
            
            Console.Out.WriteLine("Scheduler has been initialised.");
            Console.Out.WriteLine($"DB Connection: Server={server}, Database={database}");
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Console.Out.WriteLineAsync("Processing a new order.");
                    await using var connection = new SqlConnection(_connStr);
                    await connection.OpenAsync(stoppingToken);

                    Random rnd = new Random();

                    await connection.ExecuteAsync(
                        "insert into sales.orders (customer_id, order_status, order_date, required_date, shipped_date, store_id, staff_id)" +
                        $"values ({rnd.Next(1,2890)}, 2, '{DateTime.Today.ToString("yyyy-MM-dd")}', '{DateTime.Today.ToString("yyyy-MM-dd")}', null, {rnd.Next(1,6)}, {rnd.Next(1,10)});",
                        commandType: CommandType.Text);
                    
                    await Task.Delay(_waitTime, stoppingToken);
                }
                catch (Exception e)
                {
                    // await Console.Error.WriteLineAsync($"Ah plums, something went wrong: {e.Message}");
                }
            }
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await Console.Out.WriteLineAsync("Service starting");
            await base.StartAsync(cancellationToken);
        }
    }
}