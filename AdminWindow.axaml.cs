//YÖNETİCİ GİRİŞ EKRANI

using System;           //C# TEMEL ÖZELLİKLERİ 
using Avalonia.Controls;   //BUTON, TEXTBOX LARIN KULLANILMASI
using Avalonia.Interactivity; //BUTONA TIKLAMA 
using Paint.Data;            //DATABASE HELPER SINIFINA ERİŞMEK 

namespace Paint.Views;
// Database' e bağladım
public partial class AdminWindow : Window
{
    public AdminWindow()   //CONSTRUCTOR FORM AÇILDIĞINDA İLK ÇALIŞAN 
    {
        InitializeComponent(); //AXAML DEKİ HER ŞEYİ YÜKLER
    }

//=========================================================
// YÖNETİCİ GİRİŞ YAP BUTONU
//=========================================================

    private void AdminLoginButton_Click(object? sender, RoutedEventArgs e)
{
    string name = AdminNameTextBox.Text?.Trim() ?? "";      //TRİM BAŞTAKİ VE SONDAKİ BOŞLUKLARI SİLER
    string surname = AdminSurnameTextBox.Text?.Trim() ?? "";
    string password = AdminPasswordTextBox.Text ?? "";

    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname) || 
     string.IsNullOrWhiteSpace(password))
    {
        AdminResultTextBlock.Text = "Lütfen bütün alanları doldurunuz.";
        return;
    }

    bool login = DatabaseHelper.CheckManagerLogin(   //DATABASE'DEKİ YÖNETİCİYİ BULUR.
    name,
    surname,
    password);

    if (login)
    {
        ManagerPanelWindow managerPanel = new ManagerPanelWindow();  //YENİ FORM
        managerPanel.Show();
        Close();
    }
    else
    {
        AdminResultTextBlock.Text = "Ad, soyad veya şifre hatalı!";
    }
}

//===========================================
// YÖNETİCİ GİRİŞİNDE GERİ BUTONU
//===========================================

private void GeriButton_Click(object? sender, RoutedEventArgs e)
{
    MainWindow mainWindow = new MainWindow();

    mainWindow.Show();

    Close();
}

}