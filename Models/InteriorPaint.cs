using Avalonia.Media.Imaging;

namespace Paint.Models;

public class InteriorPaint
{
    public int InPaintID { get; set; }

    public string InPaintName { get; set; } = "";

    public string Explanation { get; set; } = "";

    public int StockAmount { get; set; }

    public double UnitPrice { get; set; }

    public string Picture { get; set; } = "";

    public Bitmap? PaintImage { get; set; }
}