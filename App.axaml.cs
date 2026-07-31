using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Paint.Views;

namespace Paint;

public partial class App : Application      //UYGULAMA İLK AÇILIRKEN ÇALIŞIR
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);      //APP.AXAML DOSYASINI OKUR.
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) //UYGULAMA MASAÜSTÜNDE Mİ
                                                                                   //ÇALIŞIYOR
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();  //UYGULAMA DİREKT GİRİŞ EKRANIYLA AÇILIYOR.
    }
}