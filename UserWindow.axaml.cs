using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media; //BUTONA TIKLAMA
using Paint.Data;     //RENK İŞLEMLERİ İÇİN BURASI

namespace Paint.Views;   //BENİM PROJEM Paint.Views ALTINDA

public partial class UserWindow : Window   //KULLANICI GİRİŞİ PENCEREM 
{                                          // Partial ile başka bir pencere ile çalışıyorum
    public UserWindow()
    {
        InitializeComponent();   //AXAML DOSYAMI YÜKLÜYOR
    }

  //============================================================
  //KULLANICI GİRİŞ YAP BUTONU
  //============================================================

    private void LoginButton_Click(
    object? sender,
    RoutedEventArgs e)              //EVENT METODU
{                                                         //.Trim()  gereksiz boşlukları kaldırıyor.
    string name = NameTextBox.Text?.Trim() ?? "";              //KULLANICININ YAZDIĞI BİLGİLERİ ALIYORUM
    string surname = SurnameTextBox.Text?.Trim() ?? "";
    string password = PasswordTextBox.Text?.Trim() ?? "";

    if (string.IsNullOrWhiteSpace(name) ||
        string.IsNullOrWhiteSpace(surname) ||
        string.IsNullOrWhiteSpace(password))
    {
        ResultTextBlock.Text = "Lütfen tüm alanları doldurunuz.";

        ResultTextBlock.Foreground =
            new SolidColorBrush(Color.Parse("#D64545"));    //YAZININ RENGİNİ KIRMIZI YAPTIM

        return;    //ALANLAR BOŞSA DEVAM ETMEM
    }

    bool login = DatabaseHelper.CheckUserLogin(
        name,
        surname,
        password);
                             //DATABASE' 3 BİLGİ GÖNDERİYORUM 
    if (login)
    {
        UserPanelWindow userPanelWindow =  //DOĞRUYSA YENİ KULLANICI FORMU OLUŞUYOR
            new UserPanelWindow();

        userPanelWindow.Show();

        Close();                          //MEVCUT GİRİŞ FORMUNU KAPATIYORUM
    }
    else
    {
        ResultTextBlock.Text = "Ad, soyad veya şifre hatalı.";

        ResultTextBlock.Foreground =
            new SolidColorBrush(Color.Parse("#D64545"));
    }
}
// ======================================================
// KAYIT OL BUTONU
// ======================================================


private void RegisterButton_Click(
    object? sender,
    RoutedEventArgs e)
{
    RegisterWindow registerWindow = new RegisterWindow();

    registerWindow.Show();

    Close();
}

// ======================================================
// GERİ GEL BUTONU
// ======================================================

    private void GeriButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        MainWindow mainWindow =
            new MainWindow();

        mainWindow.Show();

        Close();
    }
}