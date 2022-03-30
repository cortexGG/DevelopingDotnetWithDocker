namespace BlazorApp.Data;

public class OrderDetails
{
    public int OrderId {get; set; }
    public DateTime OrderDate {get; set; }
    public int CustomerId {get; set; }
    public string CustomerName {get; set; }
    public string StoreName {get; set; }
    public string StoreCity {get; set; }
    public string Staff {get; set; }    
}