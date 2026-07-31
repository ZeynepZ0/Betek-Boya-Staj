using System.Collections.Generic;

namespace Paint.Models;

public class Order
{
    public int OrderID { get; set; }

    public string CustomerName { get; set; } = "";

    public string Status { get; set; } = "";

    public List<OrderItem> Items { get; set; } = new();

    public double TotalPrice { get; set; }
}