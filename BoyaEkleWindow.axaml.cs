// ====================================
// İÇ CEPHE BOYASI EKLEME FORMU
//=====================================

using System;
using Avalonia.Controls;      //ARAYÜZ ELEMANLARI
using Avalonia.Interactivity; //BUTON TIKLAMA
using Avalonia.Media;
using Microsoft.Data.Sqlite; //SQLİTE' A BAĞLANMA
using Paint.Data;            //DATABASEHELPER' A ERİŞME
using Paint.Models;          //INTERİOR PAİNT MODELİNİ KULLANMA

namespace Paint.Views;

public partial class BoyaEkleWindow : Window
{
    public BoyaEkleWindow()
    {
        InitializeComponent();  //BOYAEKLEWİNDOW.AXAML DOSYASINI YÜKLER
    }


//=====================================
//BOYA EKLEME KAYDET BUTONU FONKSİYONU
//=====================================

    private void KaydetButton_Click( object? sender,RoutedEventArgs e)
    {
        string boyaAdi =                       //KULLANICININ GİRDİĞİ BİLGİLERİ ALIR
            BoyaAdiTextBox.Text?.Trim() ?? "";

        string aciklama =
            AciklamaTextBox.Text?.Trim() ?? "";

        string picture =
            PictureTextBox.Text?.Trim() ?? "";
                                       
        int stok =                     //NUMERİCTEKİ DEĞERİ TAM SAYIYA ÇEVİRME
            Convert.ToInt32(
                StokNumericUpDown.Value ?? 0);

        double fiyat =
            Convert.ToDouble(
                FiyatNumericUpDown.Value ?? 0);

        if (string.IsNullOrWhiteSpace(boyaAdi))  //BOŞ ALAN KONTROLÜ
        {
            ShowError("Lütfen boya adını giriniz.");
            return;
        }

        if (string.IsNullOrWhiteSpace(aciklama))
        {
            ShowError("Lütfen açıklama giriniz.");
            return;
        }

    try       //DATABASE E KAYIT İŞLEMİ BURDA
    {         // GİRİLEN BİLGİLER MODELS' E AKTARILIR.
        InteriorPaint newPaint = new()
        {
            InPaintName = boyaAdi,
            Explanation = aciklama,
            StockAmount = stok,
            UnitPrice = fiyat,
            Picture = picture
        };

        using SqliteConnection connection = DatabaseHelper.GetConnection();

        connection.Open();

//SQL İNSERT SORGUSU
//İNERİORPAİNT TABLOSUNA YENİ KAYIT

        const string sql =
            @"INSERT INTO InteriorPaint
            (
                InPaintName,
                Explanation,
                StockAmount,
                UnitPrice,
                Picture
            )
            VALUES
            (
                $name,
                $explanation,
                $stock,
                $price,
                $picture
            );";

        using SqliteCommand command =  new SqliteCommand(sql, connection);          //SQL SORGUSUNU ÇALIŞTIRIR.
        

        command.Parameters.AddWithValue(
            "$name",
            newPaint.InPaintName);

        command.Parameters.AddWithValue(
            "$explanation",
            newPaint.Explanation);

        command.Parameters.AddWithValue(
            "$stock",
            newPaint.StockAmount);

        command.Parameters.AddWithValue(
            "$price",
            newPaint.UnitPrice);

        command.Parameters.AddWithValue(
            "$picture",
            newPaint.Picture);

        int affectedRows =
            command.ExecuteNonQuery();  //INSERT SORGUSUNU ÇALIŞTIRIYOR

        if (affectedRows > 0)
        {
            Close(true);
        }
        else
        {
            ShowError("Boya eklenemedi.");
        }
    }
    catch (Exception ex)      //HATA OLUŞURSA
    {
        ShowError(
            "Kayıt sırasında hata oluştu: " +
            ex.Message);
    }
}

//======================================
// İÇ CEPHE BOYASI EKLEME İPTAL BUTONU FONKSİYONU
//======================================
    private void IptalButton_Click(object? sender, RoutedEventArgs e)
    {
        Close(false);
    }

    private void ShowError(string message)
    {
        ResultTextBlock.Text = message;

        ResultTextBlock.Foreground =new SolidColorBrush(
                Color.Parse("#D64545"));
    }
}