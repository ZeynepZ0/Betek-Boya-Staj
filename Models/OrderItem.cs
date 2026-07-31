namespace Paint.Models;

public class OrderItem
{
    public int OrderItemID { get; set; }

    public int OrderID { get; set; }

    public string ProductType { get; set; } = "";

    public int ProductID { get; set; }

    public string ProductName { get; set; } = "";

    public int Quantity { get; set; }

    public double UnitPrice { get; set; }

    public double TotalPrice
    {
        get
        {
            return Quantity * UnitPrice;
        }
    }
}