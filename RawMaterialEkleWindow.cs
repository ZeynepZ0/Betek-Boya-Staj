using System;
using Avalonia.Controls;     //ARAYÜZ BİLEŞENLERİ KULLANMA
using Avalonia.Interactivity; //BUTON TIKLAMA
using Avalonia.Media;         //HATA MESAJININ RENGİNİ DEĞİŞTİRME
using Paint.Data;
using Paint.Models;

namespace Paint.Views;

public partial class RawMaterialEkleWindow : Window
{
    public RawMaterialEkleWindow()
    {
        InitializeComponent();   //RawMaterialEkleWindow.axaml DEKİLER YÜKLENİR.
    }

//===================================================
// KAYDET BUTONU
//===================================================

private void KaydetButton_Click(object? sender, RoutedEventArgs e)
{
    string materialName =
        MaterialNameTextBox.Text?.Trim() ?? "";

    string explanation =
        ExplanationTextBox.Text?.Trim() ?? "";

    string picture =
        PictureTextBox.Text?.Trim() ?? "";

    int stockAmount =
        Convert.ToInt32(
            StockNumericUpDown.Value ?? 0);

    double unitPrice =
        Convert.ToDouble(
            PriceNumericUpDown.Value ?? 0);

    if (string.IsNullOrWhiteSpace(materialName))   //HAMMADDE ADI BOŞ MU KONTROL
    {
        ShowError(
            "Lütfen hammadde adını giriniz.");

        return;
    }

    try                                    //DATABASE'E KAYIT İŞLEMLERİ
    {
        RawMaterial newMaterial = new()
        {
            MaterialName = materialName,
            Explanation = explanation,
            StockAmount = stockAmount,
            UnitPrice = unitPrice,
            Picture = picture
        };

        bool added =
            DatabaseHelper.AddRawMaterial(
                newMaterial);

        if (added)
        {
            Close(true);
        }
        else
        {
            ShowError(
                "Hammadde eklenemedi.");
        }
    }
    catch (Exception ex)         //HATALAR, UYGULAMANIN KAPANMASINI ENGELLER
    {
        ShowError(
            "Kayıt sırasında hata oluştu: " +
            ex.Message);
    }
}

//=======================================
// İPTAL BUTONU
//=======================================

    private void IptalButton_Click(object? sender, RoutedEventArgs e)
    {
        Close(false);
    }
    private void ShowError(string message)
    {
        ResultTextBlock.Text =
            message;

        ResultTextBlock.Foreground =
            new SolidColorBrush(
                Color.Parse("#D64545"));
    }
}