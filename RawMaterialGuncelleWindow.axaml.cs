using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Paint.Data;
using Paint.Models;

namespace Paint.Views;

public partial class RawMaterialGuncelleWindow : Window
{
    private readonly RawMaterial selectedMaterial;

    public RawMaterialGuncelleWindow(RawMaterial material)
    {
        InitializeComponent();

        selectedMaterial = material;

        MaterialNameTextBox.Text =
            selectedMaterial.MaterialName;

        ExplanationTextBox.Text =
            selectedMaterial.Explanation;

        StockNumericUpDown.Value =
            selectedMaterial.StockAmount;

        PriceNumericUpDown.Value =
            Convert.ToDecimal(
                selectedMaterial.UnitPrice);

        PictureTextBox.Text =
            selectedMaterial.Picture;
    }

//===========================================
// GÜNCELLE BUTONU
//==========================================
    private void GuncelleButton_Click(object? sender,RoutedEventArgs e)
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

        if (string.IsNullOrWhiteSpace(materialName))
        {
            ShowError(
                "Lütfen hammadde adını giriniz.");

            return;
        }

    try
    {
        RawMaterial updatedMaterial = new()
        {
            MaterialID =
                selectedMaterial.MaterialID,

            MaterialName =
                materialName,

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
            DatabaseHelper.UpdateRawMaterial(
                updatedMaterial);

        if (updated)
        {
            Close(true);
        }
        else
        {
            ShowError(
                "Güncellenecek hammadde bulunamadı.");
        }
    }
    catch (Exception ex)
    {
        ShowError(
            "Güncelleme sırasında hata oluştu: " +
            ex.Message);
    }
}

//=======================================
// İPTAL BUTONU
//=======================================
    private void IptalButton_Click(
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