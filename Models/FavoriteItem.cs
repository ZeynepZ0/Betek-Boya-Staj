using Avalonia.Media.Imaging;

namespace Paint.Models;

public class FavoriteItem
{
    public string ProductType { get; set; } = "";

    public int ProductID { get; set; }

    public string ProductName { get; set; } = "";

    public string Explanation { get; set; } = "";

    public double UnitPrice { get; set; }

    public string Picture { get; set; } = "";

    public Bitmap? FavoriteImage { get; set; }

    public int Star { get; set; }
}