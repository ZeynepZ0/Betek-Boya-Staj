using Avalonia.Media.Imaging;

namespace Paint.Models;

public class ExteriorPaint
{
    public int ExPaintID { get; set; }

    public string ExPaintName { get; set; } = "";

    public string Explanation { get; set; } = "";

    public int StockAmount { get; set; }

    public double UnitPrice { get; set; }

    public string Picture { get; set; } = "";

    // EKRANDA GÖSTERECEĞİM FOTOĞRAF
    public Bitmap? PaintImage { get; set; }
}