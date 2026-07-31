using Avalonia.Media.Imaging;

namespace Paint.Models;

public class RawMaterial
{
    public int MaterialID { get; set; }

    public string MaterialName { get; set; } = "";

    public string Explanation { get; set; } = "";

    public int StockAmount { get; set; }

    public double UnitPrice { get; set; }

    public string Picture { get; set; } = "";

    public Bitmap? MaterialImage { get; set; }
}