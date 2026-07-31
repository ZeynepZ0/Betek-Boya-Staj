using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Paint.Data;
using Paint.Models;

namespace Paint.Views;

public partial class ExteriorPaintEkleWindow : Window
{
    public ExteriorPaintEkleWindow()
    {
        InitializeComponent();
    }

   // ===============================================
    // DIŞ CEPHE BOYASI EKLEME KAYDET BUTONU 
    // ==============================================
    private void SaveButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        string paintName =
            PaintNameTextBox.Text?.Trim() ?? "";

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

        if (string.IsNullOrWhiteSpace(paintName))
        {
            ShowError("Lütfen boya adını giriniz.");
            return;
        }

        if (string.IsNullOrWhiteSpace(explanation))
        {
            ShowError("Lütfen açıklama giriniz.");
            return;
        }

        try
        {
            ExteriorPaint newPaint = new()
            {
                ExPaintName = paintName,
                Explanation = explanation,
                StockAmount = stockAmount,
                UnitPrice = unitPrice,
                Picture = picture
            };

            bool added =
                DatabaseHelper.AddExteriorPaint(
                    newPaint);

            if (added)
            {
                Close(true);
            }
            else
            {
                ShowError(
                    "Dış cephe boyası eklenemedi.");
            }
        }
        catch (Exception ex)
        {
            ShowError(
                "Kayıt sırasında hata oluştu: " +
                ex.Message);
        }
    }
   // =========================================================
    //  DIŞ CEPHE BOYASI EKLEME İPTAL BUTONU 
    // =========================================================
    private void CancelButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        Close(false);
    }

    private void ShowError(
        string message)
    {
        ResultTextBlock.Text =
            message;

        ResultTextBlock.Foreground =
            new SolidColorBrush(
                Color.Parse("#D64545"));
    }
}