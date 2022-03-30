using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BlazorApp.Data;

public class BikeStoreService
{
    private readonly IConfiguration _config;
    private readonly string _connStr;

    private string query = @"select OrderId = orders.order_id
     , OrderDate = orders.order_date
     , CustomerId = orders.customer_id
     , CustomerName = concat(customer.first_name, ' ', customer.last_name)
     , StoreName = stores.store_name
     , StoreCity = stores.city
     , Staff = concat(staffs.first_name, ' ', staffs.last_name)
from sales.orders orders

inner join sales.customers customer
on orders.customer_id = customer.customer_id

inner join sales.stores stores
on orders.store_id = stores.store_id

inner join sales.staffs staffs
on orders.staff_id = staffs.staff_id

where orders.order_date > DateAdd(DAY, -7, GETDATE())

order by order_id desc
";

    public BikeStoreService(IConfiguration configuration)
    {
        _config = configuration;
        _connStr = configuration.GetConnectionString("BikeStore");

    }

    public async Task<OrderDetails[]> GetOrdersAsync()
    {
        try
        {
            await Console.Out.WriteLineAsync("Fetching orders from last 7 days.");
            await using var connection = new SqlConnection(_connStr);
            await connection.OpenAsync();
            var deets = await connection.QueryAsync<OrderDetails>(query, commandType: CommandType.Text);

            return deets.ToArray();
        }
        catch (Exception e)
        {
            await Console.Error.WriteLineAsync($"Ah plums, something went wrong: {e.Message}");
        }

        return Array.Empty<OrderDetails>();
    }
}