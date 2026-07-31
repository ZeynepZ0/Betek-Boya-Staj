//===============================================
// DIŞ CEPHE BOYASI GÜNCELLEME FORMU
//===============================================

using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Paint.Data;
using Paint.Models;

namespace Paint.Views;

public partial class ExteriorPaintGuncelleWindow : Window
{
    private readonly ExteriorPaint selectedPaint;

public ExteriorPaintGuncelleWindow(
    ExteriorPaint paint)
{
    InitializeComponent();

    selectedPaint = paint;

    // Mevcut bilgileri forma yerleştir.
    PaintNameTextBox.Text =
        selectedPaint.ExPaintName;

    ExplanationTextBox.Text =
        selectedPaint.Explanation;

    StockNumericUpDown.Value =
        selectedPaint.StockAmount;

    PriceNumericUpDown.Value =
        Convert.ToDecimal(
            selectedPaint.UnitPrice);

    PictureTextBox.Text =
        selectedPaint.Picture;
    }

// =========================================================
// GÜNCELLE BUTONU
// =========================================================

private void UpdateButton_Click(
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
        ShowError(
            "Lütfen boya adını giriniz.");

        return;
    }

    if (stockAmount < 0)
    {
        ShowError(
            "Stok miktarı sıfırdan küçük olamaz.");

        return;
    }

    if (unitPrice < 0)
    {
        ShowError(
            "Birim fiyat sıfırdan küçük olamaz.");

        return;
    }

    try
    {
        ExteriorPaint updatedPaint = new()
        {
            ExPaintID =
                selectedPaint.ExPaintID,

            ExPaintName =
                paintName,

            Explanation =
                explanation,

            StockAmount =
                stockAmount,

            UnitPrice =
                unitPrice,

            Picture =
                picture
        };

        bool updated =
            DatabaseHelper.UpdateExteriorPaint(
                updatedPaint);

        if (updated)
        {
            Close(true);
        }
        else
        {
            ShowError(
                "Güncellenecek dış cephe boyası bulunamadı.");
        }
    }
    catch (Exception ex)
    {
        ShowError(
            "Güncelleme sırasında hata oluştu: " +
            ex.Message);
    }
}

// =========================================================
// İPTAL BUTONU
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