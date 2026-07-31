using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Paint.Data;

namespace Paint.Views;

public partial class RegisterWindow : Window
{
    public RegisterWindow()
    {
        InitializeComponent();
    }

//=========================================
// KAYIT OL BUTONU
//=========================================

private void RegisterButton_Click(object? sender, RoutedEventArgs e)
{
    string name = NameTextBox.Text?.Trim() ?? "";
    string surname = SurnameTextBox.Text?.Trim() ?? "";
    string password = PasswordTextBox.Text ?? "";
    string passwordAgain = PasswordAgainTextBox.Text ?? "";

    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);       //HASHLEME YAPTIM

    if (string.IsNullOrWhiteSpace(name) ||
        string.IsNullOrWhiteSpace(surname) ||
        string.IsNullOrWhiteSpace(password) ||
        string.IsNullOrWhiteSpace(passwordAgain))
    {
        ResultTextBlock.Text = "Lütfen tüm alanları doldurunuz.";
        ResultTextBlock.Foreground =
            new SolidColorBrush(Colors.Red);
        return;
    }

    if (password != passwordAgain)
    {
        ResultTextBlock.Text = "Şifreler eşleşmiyor.";
        ResultTextBlock.Foreground =
            new SolidColorBrush(Colors.Red);
        return;
    }

    DatabaseHelper.RegisterUser(
    name,
    surname,
    hashedPassword);

    ResultTextBlock.Text = "Kayıt başarılı.";
    ResultTextBlock.Foreground =
        new SolidColorBrush(Colors.Green);
}
//=======================================
// GERİ BUTONU
//=======================================

    private void BackButton_Click( object? sender, RoutedEventArgs e)
    {
        new UserWindow().Show();
        Close();
    }
}