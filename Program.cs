
//UYGULAMANIN BAŞLANGIÇ KISMI
//PROGRAM ÇALIŞTIRILDIĞINDA İLK BU ÇALIŞIR.
//TASK I AVALONİA'YI BAŞLATMAK

using Avalonia;
using System;

namespace Paint;

class Program
{
    [STAThread]   //AVALONİA VE MASAÜSTÜ UYGULAMALARIN DÜZGÜN ÇALIŞMASINI SAĞLAR
    public static void Main(string[] args)
{

    BuildAvaloniaApp() .StartWithClassicDesktopLifetime(args);  //AVALONİA UYGULAMASINI OLUŞTURDUM
}

    public static AppBuilder BuildAvaloniaApp()  //AVALONİA NIN YAPILANDIRILMASINI OLUŞTURUR.
    {
        return AppBuilder.Configure<App>().UsePlatformDetect().LogToTrace();
    }
}



