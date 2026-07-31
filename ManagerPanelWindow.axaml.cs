using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;          //HATA MESAJININ RENGİNİ DEĞİŞTİRMEK İÇİN
using Microsoft.Data.Sqlite;
using Paint.Data;
using Paint.Models;
using Avalonia.Platform;
using Avalonia.Media.Imaging;
using Paint.Views;

namespace Paint.Views;

public partial class ManagerPanelWindow : Window
{   
    //LİSTELER OLUŞTURULUYOR
    // İç cephe boyaları
    public ObservableCollection<InteriorPaint> Paints { get; } = new();

    // Dış cephe boyaları
    public ObservableCollection<ExteriorPaint> ExteriorPaints { get; } = new();

    public ObservableCollection<RawMaterial> RawMaterials { get; } = new();

    public ObservableCollection<Order> PendingOrders { get; } = new();

    private readonly List<string> cartItems = new();
    private decimal totalPrice = 0;

    public ManagerPanelWindow()
    {
        InitializeComponent();  //TASARIM YÜKLENİR

        DataContext = this;     //ARAYÜZ BU SINIFA BAĞLANIR.

        LoadPaints();          //İÇ CEPHE BOYALARI ARAYÜZDEN ÇEKİLİR

        ShowPaintsPanel();
    }

// =========================================================
// İÇ CEPHE BOYALARINI DATABASE'DEN GETİR
// =========================================================

private void LoadPaints()
{
    try
    {
        Paints.Clear();

        using SqliteConnection connection =    //SQLİTE BAĞLANTISI AÇILIR.
            DatabaseHelper.GetConnection();

        connection.Open();

        const string sql =
            "SELECT InPaintID, InPaintName, Explanation, " +
            "StockAmount, UnitPrice, Picture " +
            "FROM InteriorPaint " +
            "ORDER BY InPaintID;";

        using SqliteCommand command =
            new SqliteCommand(sql, connection);

        using SqliteDataReader reader =
            command.ExecuteReader();

    while (reader.Read())            //HER KAYIT İNTERİOR PAİNT NESNESİNE DÖNÜŞTÜRÜLÜR.
{
    var paint = new InteriorPaint
    {
        InPaintID = reader.IsDBNull(0)
            ? 0
            : reader.GetInt32(0),

        InPaintName = reader.IsDBNull(1)
            ? ""
            : reader.GetString(1),

        Explanation = reader.IsDBNull(2)
            ? ""
            : reader.GetString(2),

        StockAmount = reader.IsDBNull(3)
            ? 0
            : reader.GetInt32(3),

        UnitPrice = reader.IsDBNull(4)
            ? 0
            : reader.GetDouble(4),

        Picture = reader.IsDBNull(5)
            ? ""
            : reader.GetString(5)
    };

    // RESİM YÜKLENİYOR

    if (!string.IsNullOrWhiteSpace(paint.Picture))
    {
        try
        {
            paint.PaintImage = new Bitmap(
                AssetLoader.Open(new Uri(paint.Picture)));
        }
        catch
        {
            paint.PaintImage = null;
        }
    }

//LİSTEYE EKLENİYOR

    Paints.Add(paint);
}
        PaintResultTextBlock.Text =
            Paints.Count == 0
                ? "Database içinde boya bulunamadı."
                : $"{Paints.Count} boya yüklendi.";

        PaintResultTextBlock.Foreground =
            new SolidColorBrush(Color.Parse("#10233F"));
    }
    catch (Exception ex)
    {
        PaintResultTextBlock.Text =
            "Boyalar yüklenemedi: " + ex.Message;

        PaintResultTextBlock.Foreground =
            new SolidColorBrush(Color.Parse("#D64545"));

        Console.WriteLine(ex);
    }
    }

    // =========================================================
    // DIŞ CEPHE BOYALARINI DATABASE'DEN GETİR
    // =========================================================

   private void LoadExteriorPaints()
{
try
{
    ExteriorPaints.Clear();

    List<ExteriorPaint> exteriorPaintList =
        DatabaseHelper.GetExteriorPaints();   //SQL SORGUSU YERİNE DATABASEHELPER VERİLERİ ÇEKER

    foreach (ExteriorPaint paint in exteriorPaintList)
    {
        // FOTOĞRAFI ASSETS KLASÖRÜNDEN YÜKLEME

    if (!string.IsNullOrWhiteSpace(paint.Picture))
    {
        try
        {
            paint.PaintImage = new Bitmap(
                AssetLoader.Open(
                    new Uri(paint.Picture)));
        }
        catch (Exception imageEx)
        {
            paint.PaintImage = null;

            Console.WriteLine(
                "Dış cephe fotoğrafı yüklenemedi: "
                + paint.Picture);

            Console.WriteLine(imageEx.Message);
        }
    }

            ExteriorPaints.Add(paint);
        }

        ExteriorPaintResultTextBlock.Text =
            ExteriorPaints.Count == 0
                ? "Database içinde dış cephe boyası bulunamadı."
                : $"{ExteriorPaints.Count} dış cephe boyası yüklendi.";

        ExteriorPaintResultTextBlock.Foreground =
            new SolidColorBrush(
                Color.Parse("#10233F"));
    }
    catch (Exception ex)
    {
        ExteriorPaintResultTextBlock.Text =
            "Dış cephe boyaları yüklenemedi: " +
            ex.Message;

        ExteriorPaintResultTextBlock.Foreground =
            new SolidColorBrush(
                Color.Parse("#D64545"));

        Console.WriteLine(ex);
    }
}

// =========================================================
// HAMMADDELERİ DATABASE'DEN GETİR
// =========================================================

private void LoadRawMaterials()
{
    try
    {
        RawMaterials.Clear();

        List<RawMaterial> materialList =
            DatabaseHelper.GetRawMaterials();

        foreach (RawMaterial material in materialList)
        {
            // HAMMADDE FOTOĞRAFINI YÜKLE
            if (!string.IsNullOrWhiteSpace(material.Picture))
            {
                try
                {
                    material.MaterialImage = new Bitmap(
                        AssetLoader.Open(
                            new Uri(material.Picture)));
                }
                catch (Exception imageEx)
                {
                    material.MaterialImage = null;

                    Console.WriteLine(
                        "Hammadde fotoğrafı yüklenemedi: "
                        + material.Picture);

                    Console.WriteLine(imageEx.Message);
                }
            }

            RawMaterials.Add(material);
        }

        MaterialResultTextBlock.Text =
            RawMaterials.Count == 0
                ? "Database içinde hammadde bulunamadı."
                : $"{RawMaterials.Count} hammadde yüklendi.";

        MaterialResultTextBlock.Foreground =
            new SolidColorBrush(
                Color.Parse("#10233F"));
    }
    catch (Exception ex)
    {
        MaterialResultTextBlock.Text =
            "Hammaddeler yüklenemedi: " +
            ex.Message;

        MaterialResultTextBlock.Foreground =
            new SolidColorBrush(
                Color.Parse("#D64545"));

        Console.WriteLine(ex);
    }
}


//================================================================================================
// İÇ CEPHE BOYASI EKLE BUTONU
//================================================================================================

    private async void BoyaEkleButton_Click(
    object? sender,
    RoutedEventArgs e)
{
    var boyaEkleWindow = new BoyaEkleWindow();

    bool? boyaEklendi =
        await boyaEkleWindow.ShowDialog<bool?>(this);

    if (boyaEklendi == true)
    {
        LoadPaints();

        PaintResultTextBlock.Text =
            "Yeni iç cephe boyası başarıyla eklendi.";

        PaintResultTextBlock.Foreground =
            new SolidColorBrush(
                Color.Parse("#29A847"));
    }
}

// =========================================================
// İÇ CEPHE BOYASI GÜNCELLE
// =========================================================

private async void UpdatePaint_Click(
    object? sender,
    RoutedEventArgs e)
{
    if (sender is not Button button)
    {
        return;
    }

    if (button.DataContext is not InteriorPaint selectedPaint)
    {
        PaintResultTextBlock.Text =
            "Güncellenecek boya bilgisi bulunamadı.";

        PaintResultTextBlock.Foreground =
            new SolidColorBrush(Color.Parse("#D64545"));

        return;
    }

    var boyaGuncelleWindow =
        new BoyaGuncelleWindow(selectedPaint);

    bool? guncellendi =
        await boyaGuncelleWindow.ShowDialog<bool?>(this);

    if (guncellendi == true)
    {
        LoadPaints();

        PaintResultTextBlock.Text =
            "Boya başarıyla güncellendi.";

        PaintResultTextBlock.Foreground =
            new SolidColorBrush(Color.Parse("#29A847"));
    }
}

// ================================================
// İÇ CEPHE BOYASI SİL
// ================================================

private void DeletePaint_Click(
    object? sender,
    RoutedEventArgs e)
{
    if (sender is not Button button)
    {
        return;
    }

    if (button.DataContext is not InteriorPaint selectedPaint)
    {
        PaintResultTextBlock.Text =
            "Silinecek boya bilgisi bulunamadı.";

        PaintResultTextBlock.Foreground =
            new SolidColorBrush(Color.Parse("#D64545"));

        return;
    }

    try
    {
        using SqliteConnection connection =
            DatabaseHelper.GetConnection();

        connection.Open();

        const string sql =
            "DELETE FROM InteriorPaint " +
            "WHERE InPaintID = $id;";

        using SqliteCommand command =
            new SqliteCommand(sql, connection);

        command.Parameters.AddWithValue(
            "$id",
            selectedPaint.InPaintID);

        int affectedRows =
            command.ExecuteNonQuery();

        if (affectedRows > 0)
        {
            Paints.Remove(selectedPaint);

            PaintResultTextBlock.Text =
                $"{selectedPaint.InPaintName} silindi.";

            PaintResultTextBlock.Foreground =
                new SolidColorBrush(Color.Parse("#29A847"));
        }
        else
        {
            PaintResultTextBlock.Text =
                "Silinecek boya bulunamadı.";

            PaintResultTextBlock.Foreground =
                new SolidColorBrush(Color.Parse("#D64545"));
        }
    }
    catch (Exception ex)
    {
        PaintResultTextBlock.Text =
            "Boya silinirken hata oluştu: " +
            ex.Message;

        PaintResultTextBlock.Foreground =
            new SolidColorBrush(Color.Parse("#D64545"));
    }
    }

    
// =================================================================================================
// DIŞ CEPHE BOYASI EKLEME BUTONU
// =================================================================================================

private async void ExteriorPaintEkleButton_Click(
    object? sender,
    RoutedEventArgs e)
{
    var exteriorPaintEkleWindow =
        new ExteriorPaintEkleWindow();

    bool? boyaEklendi =
        await exteriorPaintEkleWindow
            .ShowDialog<bool?>(this);

    if (boyaEklendi == true)
    {
        LoadExteriorPaints();

        ExteriorPaintResultTextBlock.Text =
            "Yeni dış cephe boyası başarıyla eklendi.";

        ExteriorPaintResultTextBlock.Foreground =
            new SolidColorBrush(
                Color.Parse("#29A847"));
    }
}

// =========================================================
// DIŞ CEPHE BOYASI GÜNCELLE
// =========================================================

private async void UpdateExteriorPaint_Click(
    object? sender,
    RoutedEventArgs e)
{
    if (sender is not Button button)
    {
        return;
    }

    if (button.DataContext is not ExteriorPaint selectedPaint)
    {
        ExteriorPaintResultTextBlock.Text =
            "Güncellenecek dış cephe boyası bulunamadı.";

        ExteriorPaintResultTextBlock.Foreground =
            new SolidColorBrush(
                Color.Parse("#D64545"));

        return;
    }

    var updateWindow =
        new ExteriorPaintGuncelleWindow(
            selectedPaint);

    bool? updated =
        await updateWindow.ShowDialog<bool?>(this);

    if (updated == true)
    {
        LoadExteriorPaints();

        ExteriorPaintResultTextBlock.Text =
            "Dış cephe boyası başarıyla güncellendi.";

        ExteriorPaintResultTextBlock.Foreground =
            new SolidColorBrush(
                Color.Parse("#29A847"));
    }
}
// =========================================================
// DIŞ CEPHE BOYASI SİL
// =========================================================

private void DeleteExteriorPaint_Click(
    object? sender,
    RoutedEventArgs e)
{
    if (sender is not Button button)
    {
        return;
    }

    if (button.DataContext is not ExteriorPaint selectedPaint)
    {
        ExteriorPaintResultTextBlock.Text =
            "Silinecek dış cephe boyası bulunamadı.";

        ExteriorPaintResultTextBlock.Foreground =
            new SolidColorBrush(Color.Parse("#D64545"));

        return;
    }

    try
    {
        bool deleted =
            DatabaseHelper.DeleteExteriorPaint(
                selectedPaint.ExPaintID);

        if (deleted)
        {
            ExteriorPaints.Remove(selectedPaint);

            ExteriorPaintResultTextBlock.Text =
                $"{selectedPaint.ExPaintName} silindi.";

            ExteriorPaintResultTextBlock.Foreground =
                new SolidColorBrush(Color.Parse("#29A847"));
        }
        else
        {
            ExteriorPaintResultTextBlock.Text =
                "Silinecek dış cephe boyası bulunamadı.";

            ExteriorPaintResultTextBlock.Foreground =
                new SolidColorBrush(Color.Parse("#D64545"));
        }
    }
    catch (Exception ex)
    {
        ExteriorPaintResultTextBlock.Text =
            "Dış cephe boyası silinirken hata oluştu: " +
            ex.Message;

        ExteriorPaintResultTextBlock.Foreground =
            new SolidColorBrush(Color.Parse("#D64545"));
        }
    }

// ===================================================================================================
// HAMMADDE EKLEME BUTONU
// ===================================================================================================

private async void RawMaterialEkleButton_Click(
    object? sender,
    RoutedEventArgs e)
{
    var materialEkleWindow =
        new RawMaterialEkleWindow();

    bool? added =
        await materialEkleWindow
            .ShowDialog<bool?>(this);

    if (added == true)
    {
        LoadRawMaterials();

        MaterialResultTextBlock.Text =
            "Yeni hammadde başarıyla eklendi.";

        MaterialResultTextBlock.Foreground =
            new SolidColorBrush(
                Color.Parse("#29A847"));
    }
}

// =========================================================
// HAMMADDE GÜNCELLE
// =========================================================

private async void UpdateRawMaterial_Click(
    object? sender,
    RoutedEventArgs e)
{
    if (sender is not Button button)
    {
        return;
    }

    if (button.DataContext is not RawMaterial selectedMaterial)
    {
        MaterialResultTextBlock.Text =
            "Güncellenecek hammadde bulunamadı.";

        return;
    }

    var updateWindow =
        new RawMaterialGuncelleWindow(
            selectedMaterial);

    bool? updated =
        await updateWindow
            .ShowDialog<bool?>(this);

    if (updated == true)
    {
        LoadRawMaterials();

        MaterialResultTextBlock.Text =
            "Hammadde başarıyla güncellendi.";

        MaterialResultTextBlock.Foreground =
            new SolidColorBrush(
                Color.Parse("#29A847"));
    }
}

// =========================================================
// HAMMADDE SİL
// =========================================================

private void DeleteRawMaterial_Click(
    object? sender,
    RoutedEventArgs e)
{
    if (sender is not Button button)
    {
        return;
    }

    if (button.DataContext is not RawMaterial selectedMaterial)
    {
        MaterialResultTextBlock.Text =
            "Silinecek hammadde bulunamadı.";

        return;
    }

    try
    {
        bool deleted =
            DatabaseHelper.DeleteRawMaterial(
                selectedMaterial.MaterialID);

        if (deleted)
        {
            RawMaterials.Remove(
                selectedMaterial);

            MaterialResultTextBlock.Text =
                $"{selectedMaterial.MaterialName} silindi.";

            MaterialResultTextBlock.Foreground =
                new SolidColorBrush(
                    Color.Parse("#29A847"));
        }
        else
        {
            MaterialResultTextBlock.Text =
                "Silinecek hammadde database içinde bulunamadı.";
        }
    }
    catch (Exception ex)
    {
        MaterialResultTextBlock.Text =
            "Hammadde silinirken hata oluştu: " +
            ex.Message;

        MaterialResultTextBlock.Foreground =
            new SolidColorBrush(
                Color.Parse("#D64545"));
    }
}


    // =========================================================
    // MENÜ BUTONLARI
    // =========================================================

     //iç cephe boyaları
    private void PaintsMenuButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        LoadPaints();
        ShowPaintsPanel();
    }
    //dış cephe boyaları
    private void Paints2MenuButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        LoadExteriorPaints();
        ShowExteriorPaintsPanel();
    }
   //hammaddeler 
    private void MaterialsMenuButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        LoadRawMaterials();
        ShowMaterialsPanel();
    }
   //siparişler
    private void CartMenuButton_Click(object? sender,RoutedEventArgs e)
{
    LoadPendingOrders();

    ShowOrdersPanel();

    SetSelectedButton(CartMenuButton);
}

    //PANEL GÖSTERME METOTLARI

    // =========================================================
    // İÇ CEPHE PANELİNİ GÖSTER
    // =========================================================

    private void ShowPaintsPanel()
    {
        PaintsPanel.IsVisible = true;
        ExteriorPaintsPanel.IsVisible = false;
        MaterialsPanel.IsVisible = false;
        CartPanel.IsVisible = false;

        SetSelectedButton(PaintsMenuButton);
        SetNormalButton(Paints2MenuButton);
        SetNormalButton(MaterialsMenuButton);
        SetNormalButton(CartMenuButton);
    }

    // =========================================================
    // DIŞ CEPHE PANELİNİ GÖSTER
    // =========================================================

    private void ShowExteriorPaintsPanel()
    {
        PaintsPanel.IsVisible = false;
        ExteriorPaintsPanel.IsVisible = true;
        MaterialsPanel.IsVisible = false;
        CartPanel.IsVisible = false;

        SetNormalButton(PaintsMenuButton);
        SetSelectedButton(Paints2MenuButton);
        SetNormalButton(MaterialsMenuButton);
        SetNormalButton(CartMenuButton);
    }

    // =========================================================
    // HAMMADDE PANELİNİ GÖSTER
    // =========================================================

    private void ShowMaterialsPanel()
    {
        PaintsPanel.IsVisible = false;
        ExteriorPaintsPanel.IsVisible = false;
        MaterialsPanel.IsVisible = true;
        CartPanel.IsVisible = false;

        SetNormalButton(PaintsMenuButton);
        SetNormalButton(Paints2MenuButton);
        SetSelectedButton(MaterialsMenuButton);
        SetNormalButton(CartMenuButton);
    }

//=============================================================
// BEKLEYEN SİPARİŞLER
//=============================================================
private void LoadPendingOrders()
{
    PendingOrders.Clear();

    using var connection = DatabaseHelper.GetConnection();
    connection.Open();

    const string sql = @"
SELECT
    OrderID,
    CustomerName,
    Status
FROM Orders
WHERE Status='Bekliyor'
ORDER BY OrderID;";

    using var command = connection.CreateCommand();
    command.CommandText = sql;

    using var reader = command.ExecuteReader();

    while (reader.Read())
    {
        PendingOrders.Add(new Order
        {
            OrderID = reader.GetInt32(0),
            CustomerName = reader.GetString(1),
            Status = reader.GetString(2)
        });
    }
}


private void ShowOrdersPanel()
{
    PaintsPanel.IsVisible = false;
    ExteriorPaintsPanel.IsVisible = false;
    MaterialsPanel.IsVisible = false;
    CartPanel.IsVisible = true;

    SetNormalButton(PaintsMenuButton);
    SetNormalButton(Paints2MenuButton);
    SetNormalButton(MaterialsMenuButton);
    SetSelectedButton(CartMenuButton);
}

// =========================================================
// SEPET PANELİNİ GÖSTER
// =========================================================


    private static void SetSelectedButton(Button button)
    {
        button.Background =
            new SolidColorBrush(Color.Parse("#1677FF"));
    }

    private static void SetNormalButton(Button button)
    {
        button.Background = Brushes.Transparent;
    }

// =========================================================
// SİPARİŞİ ONAYLA
// =========================================================

private void ApproveOrderButton_Click(
    object? sender,
    RoutedEventArgs e)
{
    if (sender is not Button button || button.DataContext is not Order selectedOrder)
    {
        return;
    }

    try
    {
        DatabaseHelper.ApproveOrder( selectedOrder.OrderID);

        PendingOrders.Remove( selectedOrder);

        OrdersResultTextBlock.Text =
            $"Sipariş #{selectedOrder.OrderID} onaylandı. Stoklar güncellendi.";

        OrdersResultTextBlock.Foreground = new SolidColorBrush( Color.Parse("#29A847"));

        LoadPaints();
        LoadExteriorPaints();
        LoadRawMaterials();
    }
    catch (Exception ex)
    {
        OrdersResultTextBlock.Text =
            "Sipariş onaylanamadı: " +
            ex.Message;

        OrdersResultTextBlock.Foreground =new SolidColorBrush(Color.Parse("#D64545"));
    }
}


    // =========================================================
    // ÇIKIŞ BUTONU
    // =========================================================

    private void LogoutButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        MainWindow mainWindow =
            new MainWindow();

        mainWindow.Show();

        Close();
    }
}