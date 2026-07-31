//======================
//GİRİŞ SAYFASI
//======================

using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Paint.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

// ================================
// YÖNETİCİ GİRİŞİ BUTONU
// ================================

private void ShowButton_Click 
( object? sender,  RoutedEventArgs e)
{
    AdminWindow adminWindow = new AdminWindow();

    adminWindow.Show();

    Close();
}


// ================================
// KULLANICI GİRİŞİ BUTONU
// ================================

private void UserLoginButton_Click 
( object? sender,  RoutedEventArgs e)
{
    UserWindow userWindow = new UserWindow();

    userWindow.Show();

    Close();
}
}