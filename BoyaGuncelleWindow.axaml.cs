//=========================================
//İÇ CEPHE BOYASI GÜNCELLEME FORMU
//=========================================

using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Data.Sqlite;
using Paint.Data;
using Paint.Models;

namespace Paint.Views;

public partial class BoyaGuncelleWindow : Window
{
    private readonly int _paintId;  //GÜNCELLENECEK BOYANIN ID'SİNİ TAŞIR

    public BoyaGuncelleWindow(InteriorPaint paint)
    {
        InitializeComponent();

        _paintId = paint.InPaintID;

        BoyaAdiTextBox.Text = paint.InPaintName;
        AciklamaTextBox.Text = paint.Explanation;
        StokNumericUpDown.Value = paint.StockAmount;
        FiyatNumericUpDown.Value =
            Convert.ToDecimal(paint.UnitPrice);
    }


   // =========================================================
    // İÇ CEPHE BOYASI GÜNCELLE BUTONU 
    // =========================================================
    private void GuncelleButton_Click(object? sender,RoutedEventArgs e)
    {
        string boyaAdi =
            BoyaAdiTextBox.Text?.Trim() ?? "";

        string aciklama =
            AciklamaTextBox.Text?.Trim() ?? "";

        int stok =
            Convert.ToInt32(
                StokNumericUpDown.Value ?? 0);

        double fiyat =
            Convert.ToDouble(
                FiyatNumericUpDown.Value ?? 0);

        if (string.IsNullOrWhiteSpace(boyaAdi))   //BOŞ ALAN KONTROLÜ
        {
            ResultTextBlock.Text =
                "Lütfen boya adını giriniz.";
            return;
        }

        if (string.IsNullOrWhiteSpace(aciklama))
        {
            ResultTextBlock.Text =
                "Lütfen açıklama giriniz.";
            return;
        }

        try
        {
            using SqliteConnection connection =
                DatabaseHelper.GetConnection();

            connection.Open();

            const string sql =
                "UPDATE InteriorPaint " +
                "SET InPaintName = $name, " +
                "Explanation = $explanation, " +
                "StockAmount = $stock, " +
                "UnitPrice = $price " +
                "WHERE InPaintID = $id;";

            using SqliteCommand command =new SqliteCommand(sql, connection);

            command.Parameters.AddWithValue(
                "$name",
                boyaAdi);

            command.Parameters.AddWithValue(
                "$explanation",
                aciklama);

            command.Parameters.AddWithValue(
                "$stock",
                stok);

            command.Parameters.AddWithValue(
                "$price",
                fiyat);

            command.Parameters.AddWithValue(
                "$id",
                _paintId);

            int affectedRows =
                command.ExecuteNonQuery();

            if (affectedRows > 0)
            {
                Close(true);
            }
            else
            {
                ResultTextBlock.Text =
                    "Boya güncellenemedi.";
            }
        }
        catch (Exception ex)
        {
            ResultTextBlock.Text =
                "Güncelleme hatası: " +
                ex.Message;
        }
    }

   // ======================================
    // İÇ CEPHE BOYASI GÜNCELLEME İPTAL BUTONU 
    // ======================================
    private void IptalButton_Click(object? sender,RoutedEventArgs e)
    {
        Close(false);
    }
}