
//UYGULAMANIN VERİTABANIYLA HABERLEŞMESİNE YARDIM OLAN YARDIMCI SINIF.
//UYGULAMADAKİ TÜM SQL İŞLEMLERİ BURDSA YAPILIR.
//CRUD İŞLEMLERİ BURADA GERÇEKLEŞTİRİYORUM.

using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Paint.Models;

namespace Paint.Data;

public static class DatabaseHelper
{
    private const string DatabasePath =
        "/Users/zeynepzorba/BetekBoyaManagement";

//BU FONKSİYON SQLİTE VERİTABANINA BAĞLANTI OLUŞTURUR.
    public static SqliteConnection GetConnection()
    {
        Console.WriteLine(
            "Bağlanılan database: " + DatabasePath);

        return new SqliteConnection(
            $"Data Source={DatabasePath}");
    }

    // =========================================================
    // DIŞ CEPHE BOYALARINI DATABASE'TEN GETİR
    // =========================================================

    public static List<ExteriorPaint> GetExteriorPaints()
    {
        List<ExteriorPaint> paints = new();

        using SqliteConnection connection =
            GetConnection();

        connection.Open();

        const string sql =
            "SELECT ExPaintID, ExPaintName, Explanation, " +
            "StockAmount, UnitPrice, Picture " +
            "FROM ExteriorPaint " +
            "ORDER BY ExPaintID;";

        using SqliteCommand command =
            new SqliteCommand(sql, connection);

        using SqliteDataReader reader =
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

            paints.Add(paint);
        }

        return paints;
    }

    // =========================================================
    // DIŞ CEPHE BOYASI EKLE
    // =========================================================

    public static bool AddExteriorPaint(
        ExteriorPaint paint)
    {
        using SqliteConnection connection =
            GetConnection();

        connection.Open();

        const string sql =
            "INSERT INTO ExteriorPaint " +
            "(ExPaintName, Explanation, StockAmount, UnitPrice, Picture) " +
            "VALUES ($name, $explanation, $stock, $price, $picture);";

        using SqliteCommand command =
            new SqliteCommand(sql, connection);

        command.Parameters.AddWithValue(
            "$name",
            paint.ExPaintName);

        command.Parameters.AddWithValue(
            "$explanation",
            paint.Explanation);

        command.Parameters.AddWithValue(
            "$stock",
            paint.StockAmount);

        command.Parameters.AddWithValue(
            "$price",
            paint.UnitPrice);

        command.Parameters.AddWithValue(
            "$picture",
            paint.Picture);

        int affectedRows =
            command.ExecuteNonQuery();

        return affectedRows > 0;
    }

    // =========================================================
    // DIŞ CEPHE BOYASI GÜNCELLE
    // =========================================================

    public static bool UpdateExteriorPaint(
        ExteriorPaint paint)
    {
        using SqliteConnection connection =
            GetConnection();

        connection.Open();

        const string sql =
            "UPDATE ExteriorPaint " +
            "SET ExPaintName = $name, " +
            "Explanation = $explanation, " +
            "StockAmount = $stock, " +
            "UnitPrice = $price, " +
            "Picture = $picture " +
            "WHERE ExPaintID = $id;";

        using SqliteCommand command =
            new SqliteCommand(sql, connection);

        command.Parameters.AddWithValue(
            "$name",
            paint.ExPaintName);

        command.Parameters.AddWithValue(
            "$explanation",
            paint.Explanation);

        command.Parameters.AddWithValue(
            "$stock",
            paint.StockAmount);

        command.Parameters.AddWithValue(
            "$price",
            paint.UnitPrice);

        command.Parameters.AddWithValue(
            "$picture",
            paint.Picture);

        command.Parameters.AddWithValue(
            "$id",
            paint.ExPaintID);

        int affectedRows =
            command.ExecuteNonQuery();

        return affectedRows > 0;
    }

    // =========================================================
    // DIŞ CEPHE BOYASI SİL
    // =========================================================

    public static bool DeleteExteriorPaint(
        int paintId)
    {
        using SqliteConnection connection =
            GetConnection();

        connection.Open();

        const string sql =
            "DELETE FROM ExteriorPaint " +
            "WHERE ExPaintID = $id;";

        using SqliteCommand command =
            new SqliteCommand(sql, connection);

        command.Parameters.AddWithValue(
            "$id",
            paintId);

        int affectedRows =
            command.ExecuteNonQuery();

        return affectedRows > 0;
    }


// =========================================================
// HAMMADDELERİ DATABASE'TEN GETİR
// =========================================================

public static List<RawMaterial> GetRawMaterials()
{
    List<RawMaterial> materials = new();

    using SqliteConnection connection =
        GetConnection();

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

    using SqliteCommand command =
        new SqliteCommand(sql, connection);

    using SqliteDataReader reader =
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

        materials.Add(material);
    }

    return materials;
}

// =========================================================
// HAMMADDE EKLE
// =========================================================

public static bool AddRawMaterial(
    RawMaterial material)
{
    using SqliteConnection connection =
        GetConnection();

    connection.Open();

    const string sql =
        @"INSERT INTO RawMaterial
        (
            MaterialName,
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

    using SqliteCommand command =
        new SqliteCommand(sql, connection);

    command.Parameters.AddWithValue(
        "$name",
        material.MaterialName);

    command.Parameters.AddWithValue(
        "$explanation",
        material.Explanation);

    command.Parameters.AddWithValue(
        "$stock",
        material.StockAmount);

    command.Parameters.AddWithValue(
        "$price",
        material.UnitPrice);

    command.Parameters.AddWithValue(
        "$picture",
        material.Picture);

    int affectedRows =
        command.ExecuteNonQuery();

    return affectedRows > 0;
}

// =========================================================
// HAMMADDE GÜNCELLE
// =========================================================

public static bool UpdateRawMaterial(
    RawMaterial material)
{
    using SqliteConnection connection =
        GetConnection();

    connection.Open();

    const string sql =
        @"UPDATE RawMaterial
          SET
              MaterialName = $name,
              Explanation = $explanation,
              StockAmount = $stock,
              UnitPrice = $price,
              Picture = $picture
          WHERE MaterialID = $id;";

    using SqliteCommand command =
        new SqliteCommand(sql, connection);

    command.Parameters.AddWithValue(
        "$name",
        material.MaterialName);

    command.Parameters.AddWithValue(
        "$explanation",
        material.Explanation);

    command.Parameters.AddWithValue(
        "$stock",
        material.StockAmount);

    command.Parameters.AddWithValue(
        "$price",
        material.UnitPrice);

    command.Parameters.AddWithValue(
        "$picture",
        material.Picture);

    command.Parameters.AddWithValue(
        "$id",
        material.MaterialID);

    int affectedRows =
        command.ExecuteNonQuery();

    return affectedRows > 0;
}

// =========================================================
// HAMMADDE SİL
// =========================================================

public static bool DeleteRawMaterial(
    int materialId)
{
    using SqliteConnection connection =
        GetConnection();

    connection.Open();

    const string sql =
        @"DELETE FROM RawMaterial
          WHERE MaterialID = $id;";

    using SqliteCommand command =
        new SqliteCommand(sql, connection);

    command.Parameters.AddWithValue(
        "$id",
        materialId);

    int affectedRows =
        command.ExecuteNonQuery();

    return affectedRows > 0;
}

// =========================================================
// YENİ SİPARİŞ OLUŞTUR
// =========================================================

public static bool CreateOrder(
    string customerName,
    List<OrderItem> orderItems)
{
    if (orderItems.Count == 0)   //SİPARİŞ BOŞ MU KONTROL EDİLİR.
    {
        return false;
    }

    using SqliteConnection connection = GetConnection();

    connection.Open();

    using SqliteTransaction transaction =    //TRANSACTİON BAŞLATILIR.(ADIMLARDAN BİRİ BİLE YANLIŞSA
        connection.BeginTransaction();       //GERİ ALINIR.

    try
    {   
        const string orderSql =         //ORDERS TABLOSUNA SİPARİŞ EKLENİR.
            @"INSERT INTO Orders
            (
                CustomerName,
                Status
            )
            VALUES
            (
                $customerName,
                'Bekliyor'
            );

            SELECT last_insert_rowid();";

        using SqliteCommand orderCommand =
            new SqliteCommand(
                orderSql,
                connection,
                transaction);

        orderCommand.Parameters.AddWithValue(
            "$customerName",
            customerName);

        long orderId =
            (long)(orderCommand.ExecuteScalar() ?? 0L);

        const string itemSql =           //HER ÜRÜN ORDERITEMS TABLOSUNA EKLENİR.
            @"INSERT INTO OrderItems
            (
                OrderID,
                ProductType,
                ProductID,
                ProductName,
                Quantity,
                UnitPrice
            )
            VALUES
            (
                $orderId,
                $productType,
                $productId,
                $productName,
                $quantity,
                $unitPrice
            );";

        foreach (OrderItem item in orderItems)
        {
            using SqliteCommand itemCommand =
                new SqliteCommand(
                    itemSql,
                    connection,
                    transaction);

            itemCommand.Parameters.AddWithValue(
                "$orderId",
                orderId);

            itemCommand.Parameters.AddWithValue(
                "$productType",
                item.ProductType);

            itemCommand.Parameters.AddWithValue(
                "$productId",
                item.ProductID);

            itemCommand.Parameters.AddWithValue(
                "$productName",
                item.ProductName);

            itemCommand.Parameters.AddWithValue(
                "$quantity",
                item.Quantity);

            itemCommand.Parameters.AddWithValue(
                "$unitPrice",
                item.UnitPrice);

            itemCommand.ExecuteNonQuery();
        }

        transaction.Commit();    //DATABASE'DEKİ TÜM DEĞİŞİKLİKLERİ KALICI OLARAK KAYDEDER.

        return true;
    }
    catch
    {
        transaction.Rollback();
        throw;
    }
}

//====================================================
// KULLANICININ 'BEKLİYOR' DEDİĞİ SİPARİŞLERİ GETİRİR.
// ===================================================

public static List<Order> GetPendingOrders()
{
    List<Order> orders = new();

    using SqliteConnection connection =
        GetConnection();

    connection.Open();

    const string orderSql =
        @"SELECT
            OrderID,
            CustomerName,
            Status
          FROM Orders
          WHERE Status = 'Bekliyor'
          ORDER BY OrderID DESC;";

    using SqliteCommand orderCommand =
        new SqliteCommand(
            orderSql,
            connection);

    using SqliteDataReader orderReader =
        orderCommand.ExecuteReader();

    while (orderReader.Read())
    {
        Order order = new()
        {
            OrderID = orderReader.GetInt32(0),

            CustomerName = orderReader.IsDBNull(1)
                ? ""
                : orderReader.GetString(1),

            Status = orderReader.IsDBNull(2)
                ? ""
                : orderReader.GetString(2)
        };

        orders.Add(order);
    }

    orderReader.Close();

    foreach (Order order in orders)
    {
        const string itemSql =
            @"SELECT
                OrderItemID,
                OrderID,
                ProductType,
                ProductID,
                ProductName,
                Quantity,
                UnitPrice
              FROM OrderItems
              WHERE OrderID = $orderId
              ORDER BY OrderItemID;";

        using SqliteCommand itemCommand =
            new SqliteCommand(
                itemSql,
                connection);

        itemCommand.Parameters.AddWithValue(
            "$orderId",
            order.OrderID);

        using SqliteDataReader itemReader =
            itemCommand.ExecuteReader();

        while (itemReader.Read())
        {
            OrderItem item = new()
            {
                OrderItemID = itemReader.GetInt32(0),
                OrderID = itemReader.GetInt32(1),
                ProductType = itemReader.GetString(2),
                ProductID = itemReader.GetInt32(3),
                ProductName = itemReader.GetString(4),
                Quantity = itemReader.GetInt32(5),
                UnitPrice = itemReader.GetDouble(6)
            };

            order.Items.Add(item);

            order.TotalPrice +=
                item.Quantity * item.UnitPrice;
        }
    }

    return orders;
}

//===================================================
// SİPARİŞ BEKLİYOR MU, ONAYLANDI MI KONTROL EDİYOR.
// ==================================================

public static bool ApproveOrder(
    int orderId)
{
    using SqliteConnection connection =
        GetConnection();

    connection.Open();

    using SqliteTransaction transaction =
        connection.BeginTransaction();

try
{
const string statusSql =
    @"SELECT Status
        FROM Orders
        WHERE OrderID = $orderId;";

using SqliteCommand statusCommand =
    new SqliteCommand(
        statusSql,
        connection,
        transaction);

statusCommand.Parameters.AddWithValue(
    "$orderId",
    orderId);

string status =
    Convert.ToString(
        statusCommand.ExecuteScalar()) ?? "";

if (status != "Bekliyor")
{
    throw new InvalidOperationException(
        "Bu sipariş daha önce işleme alınmış.");
}

const string itemsSql =
    @"SELECT
        ProductType,
        ProductID,
        ProductName,
        Quantity
        FROM OrderItems
        WHERE OrderID = $orderId;";

using SqliteCommand itemsCommand =
    new SqliteCommand(
        itemsSql,
        connection,
        transaction);

itemsCommand.Parameters.AddWithValue(
    "$orderId",
    orderId);

List<OrderItem> items = new();

using (SqliteDataReader reader =
        itemsCommand.ExecuteReader())
{
    while (reader.Read())
    {
    items.Add(
        new OrderItem
        {
            ProductType =
                reader.GetString(0),

            ProductID =
                reader.GetInt32(1),

            ProductName =
                reader.GetString(2),

            Quantity =
                reader.GetInt32(3)
        });
}
}

foreach (OrderItem item in items)
{
    string stockSql =
        item.ProductType switch
{
    "Interior" =>
        @"UPDATE InteriorPaint
            SET StockAmount =
                StockAmount - $quantity
            WHERE InPaintID = $productId
            AND StockAmount >= $quantity;",

    "Exterior" =>
        @"UPDATE ExteriorPaint
            SET StockAmount =
                StockAmount - $quantity
            WHERE ExPaintID = $productId
            AND StockAmount >= $quantity;",

    "RawMaterial" =>
        @"UPDATE RawMaterial
            SET StockAmount =
                StockAmount - $quantity
            WHERE MaterialID = $productId
            AND StockAmount >= $quantity;",

    _ => throw new InvalidOperationException(
        "Geçersiz ürün türü.")
};

    using SqliteCommand stockCommand =
        new SqliteCommand(
            stockSql,
            connection,
            transaction);

    stockCommand.Parameters.AddWithValue(
        "$quantity",
        item.Quantity);

    stockCommand.Parameters.AddWithValue(
        "$productId",
        item.ProductID);

    int affectedRows =
        stockCommand.ExecuteNonQuery();

    if (affectedRows == 0)
    {
        throw new InvalidOperationException(
            $"{item.ProductName} için yeterli stok bulunamadı.");
    }
}

const string approveSql =
    @"UPDATE Orders
        SET Status = 'Onaylandı'
        WHERE OrderID = $orderId
        AND Status = 'Bekliyor';";

using SqliteCommand approveCommand =
    new SqliteCommand(
        approveSql,
        connection,
        transaction);

approveCommand.Parameters.AddWithValue(
    "$orderId",
    orderId);

int updatedRows =
    approveCommand.ExecuteNonQuery();

if (updatedRows == 0)
{
    throw new InvalidOperationException(
        "Sipariş onaylanamadı.");
}

transaction.Commit();

return true;
}
catch
{
transaction.Rollback();
throw;
}
}


//==============================================
// YÖNETİCİ GİRİŞİ İÇİN KONTROL
//==============================================

public static bool CheckManagerLogin(
    string name,
    string surname,
    string password)
{
    using var connection = GetConnection();

    connection.Open();

    const string sql = @"
        SELECT Password
        FROM Manager
        WHERE Name=@Name
        AND Surname=@Surname";

    using var command = connection.CreateCommand();

    command.CommandText = sql;

    command.Parameters.AddWithValue("@Name", name);
    command.Parameters.AddWithValue("@Surname", surname);

    var result = command.ExecuteScalar();

    if (result == null)
        return false;

    string hashedPassword = result.ToString()!;

    return BCrypt.Net.BCrypt.Verify(        //BCRYPT İLE HASHLEME YAPTIM.
        password,
        hashedPassword);
}

//==============================================
// KULLANICI GİRİŞİ İÇİN KONTROL
//==============================================
public static bool CheckUserLogin(
    string name,
    string surname,
    string password)
{
    using var connection = GetConnection();

    connection.Open();

    const string sql = @"
        SELECT UsersID, Password
        FROM Users
        WHERE Name = @Name
        AND Surname = @Surname";

    using var command = connection.CreateCommand();

    command.CommandText = sql;

    command.Parameters.AddWithValue("@Name", name);
    command.Parameters.AddWithValue("@Surname", surname);

    using var reader = command.ExecuteReader();

    if (!reader.Read())
        return false;

    int userId = reader.GetInt32(0);
    string hashedPassword = reader.GetString(1);

    if (BCrypt.Net.BCrypt.Verify(password, hashedPassword))
    {
        CurrentUser.UserID = userId;
        CurrentUser.Name = name;
        CurrentUser.Surname = surname;

        return true;
    }

    return false;
}


//==============================================
// YENİ KULLANICI KAYDI OLUŞTURUR.
//==============================================
public static bool RegisterUser(
    string name,
    string surname,
    string password)
{
    using var connection = GetConnection();

    connection.Open();

    string sql = @"
    INSERT INTO Users
    (
        Name,
        Surname,
        Password
    )
    VALUES
    (
        @Name,
        @Surname,
        @Password
    );";

    using var command = connection.CreateCommand();

    command.CommandText = sql;

    command.Parameters.AddWithValue("@Name", name);
    command.Parameters.AddWithValue("@Surname", surname);
    command.Parameters.AddWithValue("@Password", password);

    command.ExecuteNonQuery();

    return true;
}

}