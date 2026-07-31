using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Paint.Data;
using Paint.Models;
using System.Linq;
using System.Globalization;
using Microsoft.Data.Sqlite;

namespace Paint.Views;

public partial class UserPanelWindow : Window
{
    public ObservableCollection<InteriorPaint> InteriorPaints { get; } = new();

    public ObservableCollection<ExteriorPaint> ExteriorPaints { get; } = new();

    public ObservableCollection<RawMaterial> RawMaterials { get; } = new();

    public ObservableCollection<FavoriteItem> FavoriteItems { get; } = new();

    private readonly List<CartItem> cartItems = new();

    public UserPanelWindow()
    {
        InitializeComponent();

        DataContext = this;

        LoadInteriorPaints();

        ShowInteriorPanel();

        SetSelectedButton(InteriorMenuButton);

        DataContext = this;
    }

  // =========================================================
// İÇ CEPHE BOYALARINI GETİR
// =========================================================

private void LoadInteriorPaints()
{
    try
    {
        InteriorPaints.Clear();

        using var connection =
            DatabaseHelper.GetConnection();

        connection.Open();

        const string sql =
            @"SELECT
                InPaintID,
                InPaintName,
                Explanation,
                StockAmount,
                UnitPrice,
                Picture
              FROM InteriorPaint
              ORDER BY InPaintID;";

        using var command =
            new Microsoft.Data.Sqlite.SqliteCommand(
                sql,
                connection);

        using var reader =
            command.ExecuteReader();

        while (reader.Read())
        {
            InteriorPaint paint = new()
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

            // DATABASE'DEKİ FOTOĞRAF YOLUNU RESME ÇEVİR
            if (!string.IsNullOrWhiteSpace(paint.Picture))
            {
                try
                {
                    paint.PaintImage =
                        new Avalonia.Media.Imaging.Bitmap(
                            Avalonia.Platform.AssetLoader.Open(
                                new Uri(paint.Picture)));
                }
                catch (Exception imageEx)
                {
                    paint.PaintImage = null;

                    Console.WriteLine(
                        "Resim yüklenemedi: " +
                        paint.Picture +
                        " - " +
                        imageEx.Message);
                }
            }

            InteriorPaints.Add(paint);
        }
    }
   catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
    throw;
}
}



   // =========================================================
// DIŞ CEPHE BOYALARINI GETİR
// =========================================================

private void LoadExteriorPaints()
{
    try
    {
        ExteriorPaints.Clear();

        using var connection =
            DatabaseHelper.GetConnection();

        connection.Open();

        const string sql =
            @"SELECT
                ExPaintID,
                ExPaintName,
                Explanation,
                StockAmount,
                UnitPrice,
                Picture
              FROM ExteriorPaint
              ORDER BY ExPaintID;";

        using var command =
            new Microsoft.Data.Sqlite.SqliteCommand(
                sql,
                connection);

        using var reader =
            command.ExecuteReader();

        while (reader.Read())
        {
            ExteriorPaint paint = new()
            {
                ExPaintID = reader.IsDBNull(0)
                    ? 0
                    : reader.GetInt32(0),

                ExPaintName = reader.IsDBNull(1)
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

            // FOTOĞRAFI YÜKLE
            if (!string.IsNullOrWhiteSpace(paint.Picture))
            {
                try
                {
                    paint.PaintImage =
                        new Avalonia.Media.Imaging.Bitmap(
                            Avalonia.Platform.AssetLoader.Open(
                                new Uri(paint.Picture)));
                }
                catch (Exception imageEx)
                {
                    paint.PaintImage = null;

                    Console.WriteLine(
                        "Dış cephe resmi yüklenemedi: " +
                        paint.Picture +
                        " - " +
                        imageEx.Message);
                }
            }

            ExteriorPaints.Add(paint);
        }
    }
    catch (Exception ex)
    {
        ExteriorResultTextBlock.Text =
            "Dış cephe boyaları yüklenemedi: " +
            ex.Message;

        ExteriorResultTextBlock.Foreground =
            new SolidColorBrush(
                Color.Parse("#D64545"));
    }
}

   // =========================================================
// HAMMADDELERİ GETİR
// =========================================================

private void LoadRawMaterials()
{
    try
    {
        RawMaterials.Clear();

        using var connection =
            DatabaseHelper.GetConnection();

        connection.Open();

        const string sql =
            @"SELECT
                MaterialID,
                MaterialName,
                Explanation,
                StockAmount,
                UnitPrice,
                Picture
              FROM RawMaterial
              ORDER BY MaterialID;";

        using var command =
            new Microsoft.Data.Sqlite.SqliteCommand(
                sql,
                connection);

        using var reader =
            command.ExecuteReader();

        while (reader.Read())
        {
            RawMaterial material = new()
            {
                MaterialID = reader.IsDBNull(0)
                    ? 0
                    : reader.GetInt32(0),

                MaterialName = reader.IsDBNull(1)
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

            // FOTOĞRAFI YÜKLE
            if (!string.IsNullOrWhiteSpace(material.Picture))
            {
                try
                {
                    material.MaterialImage =
                        new Avalonia.Media.Imaging.Bitmap(
                            Avalonia.Platform.AssetLoader.Open(
                                new Uri(material.Picture)));
                }
                catch (Exception imageEx)
                {
                    material.MaterialImage = null;

                    Console.WriteLine(
                        "Hammadde resmi yüklenemedi: " +
                        material.Picture +
                        " - " +
                        imageEx.Message);
                }
            }

            RawMaterials.Add(material);
        }
    }
    catch (Exception ex)
{
    Console.WriteLine(
        "Hammaddeler yüklenemedi: " +
        ex.Message);
}
}
    // =========================================================
    // İÇ CEPHE BOYASINI SEPETE EKLE
    // =========================================================

    private void AddInteriorToCart_Click(
        object? sender,
        RoutedEventArgs e)
    {
        if (sender is not Button button ||
            button.DataContext is not InteriorPaint paint)
        {
            return;
        }

        AddToCart(
            "Interior",
            paint.InPaintID,
            paint.InPaintName,
            paint.UnitPrice,
            paint.StockAmount,
            InteriorResultTextBlock);
    }

    // =========================================================
    // DIŞ CEPHE BOYASINI SEPETE EKLE
    // =========================================================

    private void AddExteriorToCart_Click(
        object? sender,
        RoutedEventArgs e)
    {
        if (sender is not Button button ||
            button.DataContext is not ExteriorPaint paint)
        {
            return;
        }

        AddToCart(
            "Exterior",
            paint.ExPaintID,
            paint.ExPaintName,
            paint.UnitPrice,
            paint.StockAmount,
            ExteriorResultTextBlock);
    }

    private void AddExteriorToFavorites_Click(
    object? sender,
    RoutedEventArgs e)
{


    if (sender is not Button button ||
        button.DataContext is not ExteriorPaint paint)
    {
        return;
    }

      Console.WriteLine("Exterior butonuna basıldı");
    Console.WriteLine($"ExPaintID = {paint.ExPaintID}");
    Console.WriteLine($"CurrentUser = {CurrentUser.UserID}");

    using var connection = DatabaseHelper.GetConnection();

connection.Open();

const string checkSql = @"
SELECT COUNT(*)
FROM Favorites
WHERE UsersID = @UsersID
AND ProductType = @ProductType
AND ProductID = @ProductID;";

using var checkCommand = connection.CreateCommand();

checkCommand.CommandText = checkSql;

checkCommand.Parameters.AddWithValue("@UsersID",CurrentUser.UserID);
checkCommand.Parameters.AddWithValue("@ProductType", "Exterior");
checkCommand.Parameters.AddWithValue("@ProductID", paint.ExPaintID);

long count = (long)checkCommand.ExecuteScalar()!;

if (count > 0)
{
    ExteriorResultTextBlock.Text =
        "Bu ürün zaten favorilerinizde.";

    return;
}

const string sql = @"
INSERT INTO Favorites
(
    UsersID,
    ProductType,
    ProductID
)
VALUES
(
    @UsersID,
    @ProductType,
    @ProductID
);";

using var command = connection.CreateCommand();

command.CommandText = sql;

command.Parameters.AddWithValue("@UsersID", CurrentUser.UserID);
command.Parameters.AddWithValue("@ProductType", "Exterior");
command.Parameters.AddWithValue("@ProductID", paint.ExPaintID);

int affectedRows = command.ExecuteNonQuery();
Console.WriteLine($"Etkilenen satır: {affectedRows}");

    FavoriteItems.Add(new FavoriteItem
    {
        ProductType = "Exterior",
        ProductID = paint.ExPaintID,
        ProductName = paint.ExPaintName,
        Explanation = paint.Explanation,
        UnitPrice = paint.UnitPrice,
        Picture = paint.Picture
    });

    ExteriorResultTextBlock.Text =
        $"{paint.ExPaintName} favorilere eklendi.";
}

    // =========================================================
    // HAMMADDEYİ SEPETE EKLE
    // =========================================================

    private void AddMaterialToCart_Click(
        object? sender,
        RoutedEventArgs e)
    {
        if (sender is not Button button ||
            button.DataContext is not RawMaterial material)
        {
            return;
        }

        AddToCart(
            "RawMaterial",
            material.MaterialID,
            material.MaterialName,
            material.UnitPrice,
            material.StockAmount,
            MaterialResultTextBlock);
    }

     // =========================================================
    // HAMMADDEYİ FAVROİLERE EKLE
    // =========================================================

   private void AddMaterialToFavorites_Click(
    object? sender,
    RoutedEventArgs e)
{
    if (sender is not Button button ||
        button.DataContext is not RawMaterial material)
    {
        return;
    }

    using var connection = DatabaseHelper.GetConnection();
    connection.Open();

    // Daha önce eklenmiş mi kontrol et
    using var checkCommand = connection.CreateCommand();

    checkCommand.CommandText = @"
SELECT COUNT(*)
FROM Favorites
WHERE UsersID=@UsersID
AND ProductType=@ProductType
AND ProductID=@ProductID;";

    checkCommand.Parameters.AddWithValue("@UsersID", CurrentUser.UserID);
    checkCommand.Parameters.AddWithValue("@ProductType", "Material");
    checkCommand.Parameters.AddWithValue("@ProductID", material.MaterialID);

    long count = (long)checkCommand.ExecuteScalar()!;

    if (count > 0)
    {
        MaterialResultTextBlock.Text =
            "Bu ürün zaten favorilerinizde.";

        return;
    }

    // Veritabanına ekle
    using var insertCommand = connection.CreateCommand();

    insertCommand.CommandText = @"
INSERT INTO Favorites
(
    UsersID,
    ProductType,
    ProductID
)
VALUES
(
    @UsersID,
    @ProductType,
    @ProductID
);";

    insertCommand.Parameters.AddWithValue("@UsersID", CurrentUser.UserID);
    insertCommand.Parameters.AddWithValue("@ProductType", "Material");
    insertCommand.Parameters.AddWithValue("@ProductID", material.MaterialID);

    insertCommand.ExecuteNonQuery();

    // Ekranı güncelle
    LoadFavorites();

    MaterialResultTextBlock.Text =
        $"{material.MaterialName} favorilere eklendi.";
}



// =========================================================
    // FAVORİLER EKLE
    // =========================================================


   private void AddInteriorToFavorites_Click(
    object? sender,
    RoutedEventArgs e)
{
    if (sender is not Button button ||
        button.DataContext is not InteriorPaint paint)
    {
        return;
    }

    bool exists = FavoriteItems.Any(x =>
        x.ProductType == "Interior" &&
        x.ProductID == paint.InPaintID);

    if (exists)
    {
        InteriorResultTextBlock.Text =
            "Bu ürün zaten favorilerinizde.";

        return;
    }

using var connection = DatabaseHelper.GetConnection();

connection.Open();

const string sql = @"
INSERT INTO Favorites
(
    UsersID,
    ProductType,
    ProductID
)
VALUES
(
    @UsersID,
    @ProductType,
    @ProductID
);";

using var command = connection.CreateCommand();

command.CommandText = sql;

command.Parameters.AddWithValue("@UsersID", CurrentUser.UserID);
command.Parameters.AddWithValue("@ProductType", "Interior");
command.Parameters.AddWithValue("@ProductID", paint.InPaintID);

int affectedRows = command.ExecuteNonQuery();
Console.WriteLine($"Etkilenen satır: {affectedRows}");


    FavoriteItems.Add(new FavoriteItem
    {
        ProductType = "Interior",
        ProductID = paint.InPaintID,
        ProductName = paint.InPaintName,
        Explanation = paint.Explanation,
        UnitPrice = paint.UnitPrice,
        Picture = paint.Picture
    });

    InteriorResultTextBlock.Text =
        $"{paint.InPaintName} favorilere eklendi.";
}

    // =========================================================
    // SEPETE ÜRÜN EKLE
    // =========================================================

    private void AddToCart(
        string productType,
        int productId,
        string productName,
        double unitPrice,
        int availableStock,
        TextBlock resultTextBlock)
    {
        if (availableStock <= 0)
        {
            resultTextBlock.Text =
                $"{productName} stokta bulunmuyor.";

            resultTextBlock.Foreground =
                new SolidColorBrush(
                    Color.Parse("#D64545"));

            return;
        }

        CartItem? existingItem =
            cartItems.Find(
                item =>
                    item.ProductType == productType &&
                    item.ProductID == productId);

        if (existingItem != null)
        {
            if (existingItem.Quantity >= availableStock)
            {
                resultTextBlock.Text =
                    $"{productName} için yeterli stok bulunmuyor.";

                resultTextBlock.Foreground =
                    new SolidColorBrush(
                        Color.Parse("#D64545"));

                return;
            }

            existingItem.Quantity++;
        }
        else
        {
            cartItems.Add(
                new CartItem
                {
                    ProductType = productType,

                    ProductID = productId,

                    ProductName = productName,

                    Quantity = 1,

                    UnitPrice = unitPrice,

                    AvailableStock = availableStock
                });
        }

        resultTextBlock.Text =
            $"{productName} sepete eklendi.";

        resultTextBlock.Foreground =
            new SolidColorBrush(
                Color.Parse("#29A847"));

        UpdateCart();
    }

    // =========================================================
    // SEPETİ EKRANDA GÖSTER
    // =========================================================

    private void UpdateCart()
    {
        if (cartItems.Count == 0)
        {
            CartItemsTextBlock.Text =
                "Sepetiniz şu anda boş.";

            CartItemsTextBlock.Foreground =
                new SolidColorBrush(
                    Color.Parse("#10233F"));

            TotalPriceTextBlock.Text =
                "0,00 TL";

            return;
        }

        List<string> cartLines = new();

        double totalPrice = 0;

        foreach (CartItem item in cartItems)
        {
            cartLines.Add(
                $"{item.ProductName}" +
                Environment.NewLine +
                $"Miktar: {item.Quantity}" +
                Environment.NewLine +
                $"Birim fiyat: {item.UnitPrice:0.00} TL" +
                Environment.NewLine +
                $"Toplam: {item.TotalPrice:0.00} TL");

            totalPrice += item.TotalPrice;
        }

        CartItemsTextBlock.Text =
            string.Join(
                Environment.NewLine +
                Environment.NewLine,
                cartLines);

        CartItemsTextBlock.Foreground =
            new SolidColorBrush(
                Color.Parse("#10233F"));

        TotalPriceTextBlock.Text =
            $"{totalPrice:0.00} TL";
    }

    // =========================================================
    // SATIN AL
    // =========================================================

    private void ConfirmPurchaseButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        if (cartItems.Count == 0)
        {
            ShowCartError(
                "Satın almak için önce sepete ürün ekleyiniz.");

            return;
        }

        try
        {
            List<OrderItem> orderItems = new();

            foreach (CartItem cartItem in cartItems)
            {
                orderItems.Add(
                    new OrderItem
                    {
                        ProductType =
                            cartItem.ProductType,

                        ProductID =
                            cartItem.ProductID,

                        ProductName =
                            cartItem.ProductName,

                        Quantity =
                            cartItem.Quantity,

                        UnitPrice =
                            cartItem.UnitPrice
                    });
            }

//Burda kullanıcının adını ve soyadını görmek istyiroum database de 

            bool orderCreated =
                DatabaseHelper.CreateOrder(
                    $"{CurrentUser.Name} {CurrentUser.Surname}",
                    orderItems);

            if (!orderCreated)
            {
                ShowCartError(
                    "Satın alma işlemi oluşturulamadı.");

                return;
            }

            /*
             * Burada stok miktarı azaltılmıyor.
             * Stok yalnızca yönetici siparişi
             * onayladığında azaltılacak.
             */

            cartItems.Clear();

            CartItemsTextBlock.Text =
                "Satın alma işleminiz oluşturuldu. " +
                "Yönetici onayı bekleniyor.";

            CartItemsTextBlock.Foreground =
                new SolidColorBrush(
                    Color.Parse("#29A847"));

            TotalPriceTextBlock.Text =
                "0,00 TL";
        }
        catch (Exception ex)
        {
            ShowCartError(
                "Satın alma işlemi sırasında hata oluştu: " +
                ex.Message);
        }
    }

    // =========================================================
    // SEPET HATA MESAJI
    // =========================================================

    private void ShowCartError(
        string message)
    {
        CartItemsTextBlock.Text =
            message;

        CartItemsTextBlock.Foreground =
            new SolidColorBrush(
                Color.Parse("#D64545"));
    }

// =========================================================
// MENÜLER
// =========================================================

private void InteriorMenuButton_Click(
    object? sender,
    RoutedEventArgs e)
{
    LoadInteriorPaints();
    ShowInteriorPanel();
}

private void ExteriorMenuButton_Click(
    object? sender,
    RoutedEventArgs e)
{
    LoadExteriorPaints();
    ShowExteriorPanel();
}

private void MaterialsMenuButton_Click(
    object? sender,
    RoutedEventArgs e)
{
    LoadRawMaterials();
    ShowMaterialsPanel();
}

private void FavoritesMenuButton_Click(
    object? sender,
    RoutedEventArgs e)
{
    ShowFavoritesPanel();
}

private void CartMenuButton_Click(
    object? sender,
    RoutedEventArgs e)
{
    ShowCartPanel();
}


    // =========================================================
    // PANELLER
    // =========================================================

    // =========================================================
// İÇ CEPHE PANELİNİ GÖSTER
// =========================================================

private void ShowInteriorPanel()
{
    InteriorPanel.IsVisible = true;
    ExteriorPanel.IsVisible = false;
    MaterialsPanel.IsVisible = false;
    FavoritesPanel.IsVisible = false;
    CartPanel.IsVisible = false;

    SetSelectedButton(InteriorMenuButton);
}



// =========================================================
// DIŞ CEPHE PANELİNİ GÖSTER
// =========================================================

private void ShowExteriorPanel()
{
    InteriorPanel.IsVisible = false;
    ExteriorPanel.IsVisible = true;
    MaterialsPanel.IsVisible = false;
    FavoritesPanel.IsVisible = false;
    CartPanel.IsVisible = false;

    SetSelectedButton(ExteriorMenuButton);
}

// =========================================================
// HAMMADDE PANELİNİ GÖSTER
// =========================================================

private void ShowMaterialsPanel()
{
    InteriorPanel.IsVisible = false;
    ExteriorPanel.IsVisible = false;
    MaterialsPanel.IsVisible = true;
    FavoritesPanel.IsVisible = false;
    CartPanel.IsVisible = false;

    SetSelectedButton(MaterialsMenuButton);
}

// =========================================================
// FAVORİLER PANELİNİ GÖSTER
// =========================================================

private void ShowFavoritesPanel()
{
    LoadFavorites();

    InteriorPanel.IsVisible = false;
    ExteriorPanel.IsVisible = false;
    MaterialsPanel.IsVisible = false;
    FavoritesPanel.IsVisible = true;
    CartPanel.IsVisible = false;

    SetSelectedButton(FavoritesMenuButton);
}

private void LoadFavorites()
{
    FavoriteItems.Clear();

    using var connection = DatabaseHelper.GetConnection();
    connection.Open();

    const string sql = @"
SELECT
    F.ProductType,
    F.ProductID,

    CASE
        WHEN F.ProductType='Interior' THEN I.InPaintName
        WHEN F.ProductType='Exterior' THEN E.ExPaintName
        WHEN F.ProductType='Material' THEN M.MaterialName
    END AS ProductName,

    CASE
        WHEN F.ProductType='Interior' THEN I.Explanation
        WHEN F.ProductType='Exterior' THEN E.Explanation
        WHEN F.ProductType='Material' THEN M.Explanation
    END AS Explanation,

    CASE
        WHEN F.ProductType='Interior' THEN I.UnitPrice
        WHEN F.ProductType='Exterior' THEN E.UnitPrice
        WHEN F.ProductType='Material' THEN M.UnitPrice
    END AS UnitPrice,

    CASE
        WHEN F.ProductType='Interior' THEN I.Picture
        WHEN F.ProductType='Exterior' THEN E.Picture
        WHEN F.ProductType='Material' THEN M.Picture
    END AS Picture

    FROM Favorites F

    LEFT JOIN InteriorPaint I
    ON F.ProductID = I.InPaintID
    AND F.ProductType='Interior'

    LEFT JOIN ExteriorPaint E
    ON F.ProductID = E.ExPaintID
    AND F.ProductType='Exterior'

    LEFT JOIN RawMaterial M
    ON F.ProductID = M.MaterialID
    AND F.ProductType='Material'

    WHERE F.UsersID=@UsersID;";
    
    using var command = connection.CreateCommand();
    command.CommandText = sql;
    command.Parameters.AddWithValue("@UsersID", CurrentUser.UserID);

    using var reader = command.ExecuteReader();

    while (reader.Read())
    {
        FavoriteItem item = new()
        {
            ProductType = reader.GetString(0),
            ProductID = reader.GetInt32(1),
            ProductName = reader.GetString(2),
            Explanation = reader.GetString(3),
            UnitPrice = reader.GetDouble(4),
            Picture = reader.GetString(5)
        };

        if (!string.IsNullOrWhiteSpace(item.Picture))
        {
            item.FavoriteImage =
                new Avalonia.Media.Imaging.Bitmap(
                    Avalonia.Platform.AssetLoader.Open(
                        new Uri(item.Picture)));
        }

        FavoriteItems.Add(item);
    }
}

// =========================================================
// SEPET PANELİNİ GÖSTER
// =========================================================

private void ShowCartPanel()
{
    InteriorPanel.IsVisible = false;
    ExteriorPanel.IsVisible = false;
    MaterialsPanel.IsVisible = false;
    FavoritesPanel.IsVisible = false;
    CartPanel.IsVisible = true;

    SetSelectedButton(CartMenuButton);

    UpdateCart();
}
    // =========================================================
    // ÇIKIŞ
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
// ===========================================================
// YILDIZLA
// ===========================================================

private void Star_Click(object? sender, RoutedEventArgs e)
{
    if (sender is not Button button)
        return;

    if (button.DataContext is not FavoriteItem favorite)
        return;

    int star = int.Parse(button.Tag!.ToString()!);

    using var connection = DatabaseHelper.GetConnection();
    connection.Open();

    // Aynı yıldıza tekrar basıldıysa puanı kaldır
    if (favorite.Star == star)
    {
        favorite.Star = 0;

        var delete = connection.CreateCommand();
        delete.CommandText = @"
DELETE FROM Ratings
WHERE UsersID=@UsersID
AND ProductType=@ProductType
AND ProductID=@ProductID;";

        delete.Parameters.AddWithValue("@UsersID", CurrentUser.UserID);
        delete.Parameters.AddWithValue("@ProductType", favorite.ProductType);
        delete.Parameters.AddWithValue("@ProductID", favorite.ProductID);

        delete.ExecuteNonQuery();
    }
    else
    {
        favorite.Star = star;

        var delete = connection.CreateCommand();
        delete.CommandText = @"
DELETE FROM Ratings
WHERE UsersID=@UsersID
AND ProductType=@ProductType
AND ProductID=@ProductID;";

        delete.Parameters.AddWithValue("@UsersID", CurrentUser.UserID);
        delete.Parameters.AddWithValue("@ProductType", favorite.ProductType);
        delete.Parameters.AddWithValue("@ProductID", favorite.ProductID);

        delete.ExecuteNonQuery();

        var insert = connection.CreateCommand();
        insert.CommandText = @"
INSERT INTO Ratings
(
    UsersID,
    ProductType,
    ProductID,
    Star
)
VALUES
(
    @UsersID,
    @ProductType,
    @ProductID,
    @Star
);";

        insert.Parameters.AddWithValue("@UsersID", CurrentUser.UserID);
        insert.Parameters.AddWithValue("@ProductType", favorite.ProductType);
        insert.Parameters.AddWithValue("@ProductID", favorite.ProductID);
        insert.Parameters.AddWithValue("@Star", star);

        insert.ExecuteNonQuery();
    }

    LoadFavorites();
    if (sender is Button clickedButton)
{
    var panel = clickedButton.Parent as StackPanel;

    if (panel != null)
    {
        foreach (var child in panel.Children)
        {
            if (child is Button starButton && starButton.Tag != null)
            {
                int value = int.Parse(starButton.Tag.ToString()!);

                if (favorite.Star >= value)
                    starButton.Foreground = Brushes.Gold;
                else
                    starButton.Foreground = Brushes.Gray;
            }
        }
    }
}
}


// ===========================================================
// FAVORİLERDEN ÇIKAR
// ===========================================================

private void RemoveFavorite_Click(
    object? sender,
    RoutedEventArgs e)
{
    if (sender is not Button button ||
        button.DataContext is not FavoriteItem favorite)
    {
        return;
    }

    using var connection = DatabaseHelper.GetConnection();
    connection.Open();

    const string sql = @"
    DELETE FROM Favorites
    WHERE UsersID = @UsersID
    AND ProductType = @ProductType
    AND ProductID = @ProductID;";

    using var command = connection.CreateCommand();

    command.CommandText = sql;

    command.Parameters.AddWithValue("@UsersID", CurrentUser.UserID);
    command.Parameters.AddWithValue("@ProductType", favorite.ProductType);
    command.Parameters.AddWithValue("@ProductID", favorite.ProductID);

    command.ExecuteNonQuery();

    FavoriteItems.Remove(favorite);
}


    // =========================================================
    // SEÇİLİ MENÜ BUTONU
    // =========================================================

    private void SetSelectedButton(
        Button selectedButton)
    {
        Button[] buttons =
        {
            InteriorMenuButton,
            ExteriorMenuButton,
            MaterialsMenuButton,
            FavoritesMenuButton,
            CartMenuButton
        };

        foreach (Button button in buttons)
        {
            button.Background =
                new SolidColorBrush(
                    Color.Parse("#10233F"));

            button.BorderBrush =
                new SolidColorBrush(
                    Color.Parse("#2E9BFF"));

            button.Foreground =
                Brushes.White;
        }

        selectedButton.Background =
            new SolidColorBrush(
                Color.Parse("#1677FF"));
    }

    // =========================================================
    // SEPET ÜRÜNÜ
    // =========================================================

    private sealed class CartItem
    {
        public string ProductType { get; set; } = "";

        public int ProductID { get; set; }

        public string ProductName { get; set; } = "";

        public int Quantity { get; set; }

        public double UnitPrice { get; set; }

        public int AvailableStock { get; set; }

        public double TotalPrice
        {
            get
            {
                return Quantity * UnitPrice;
            }
        }
    }
}