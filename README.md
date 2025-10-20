
Eger proje masaüstünde ise

Path                                          
----                                          
C:\Users\<Kullanıcı İsmini Yaz>\Desktop\OrderManagementSystem

# Migration oluştur
dotnet ef migrations add InitialCreate --project OrderManagementSystem.Infrastructure --startup-project OrderManagementSystem.API

# Database güncelle
dotnet ef database update --project OrderManagementSystem.Infrastructure --startup-project OrderManagementSystem.API

# Projeyi çalıştır
dotnet run --project OrderManagementSystem.API

# Swagger 
https://localhost:7202/index.html

# Frontend kısmı
https://github.com/huseyinseber/OrderManagementFrontend

#  Herhangi bir üründen alan müşterilerin isimleri ve adres listesi

SELECT DISTINCT 
    c.CustomerName,
    ca.Country,
    ca.City,
    ca.Town,
    ca.Address,
    ca.Email,
    ca.Phone
FROM Customers c
INNER JOIN Orders o ON c.CustomerId = o.CustomerId
INNER JOIN OrderDetails od ON o.OrderId = od.OrderId
INNER JOIN CustomerAddresses ca ON c.CustomerId = ca.CustomerId
WHERE od.IsActive = 1 
    AND o.IsActive = 1 
    AND c.IsActive = 1 
    AND ca.IsActive = 1
ORDER BY c.CustomerName;

# Siparişteki ürün miktarı 1'den büyük olan müşteriler ve sipariş detayları

SELECT 
    c.CustomerName,
    o.OrderNo,
    o.OrderDate,
    s.StockName,
    od.Amount,
    s.Unit,
    s.Price,
    (od.Amount * s.Price) as TotalProductPrice
FROM Customers c
INNER JOIN Orders o ON c.CustomerId = o.CustomerId
INNER JOIN OrderDetails od ON o.OrderId = od.OrderId
INNER JOIN Stocks s ON od.StockId = s.StockId
WHERE od.Amount > 1 
    AND od.IsActive = 1 
    AND o.IsActive = 1 
    AND c.IsActive = 1 
    AND s.IsActive = 1
ORDER BY od.Amount DESC, c.CustomerName;

# Fatura adresi ve teslimat adresi aynı olmayan müşteri listesi

   SELECT 
    c.CustomerName,
    o.OrderNo,
    o.OrderDate,
    da.Address as DeliveryAddress,
    ia.Address as InvoiceAddress
FROM Orders o
INNER JOIN Customers c ON o.CustomerId = c.CustomerId
INNER JOIN CustomerAddresses da ON o.DeliveryAddressId = da.AddressId
INNER JOIN CustomerAddresses ia ON o.InvoiceAddressId = ia.AddressId
WHERE o.DeliveryAddressId != o.InvoiceAddressId
    AND o.IsActive = 1 
    AND c.IsActive = 1 
    AND da.IsActive = 1 
    AND ia.IsActive = 1
ORDER BY c.CustomerName, o.OrderDate;

# Müşteri Adı TLS olan müşterinin siparişlerinin listesi

SELECT 
    c.CustomerName,
    o.OrderId,
    o.OrderNo,
    o.OrderDate,
    o.TotalPrice,
    o.Tax,
    s.StockName,
    od.Amount,
    s.Price,
    (od.Amount * s.Price) as LineTotal
FROM Customers c
INNER JOIN Orders o ON c.CustomerId = o.CustomerId
INNER JOIN OrderDetails od ON o.OrderId = od.OrderId
INNER JOIN Stocks s ON od.StockId = s.StockId
WHERE c.CustomerName = 'TLS Lojistik'
    AND c.IsActive = 1 
    AND o.IsActive = 1 
    AND od.IsActive = 1 
    AND s.IsActive = 1
ORDER BY o.OrderDate DESC, o.OrderNo;

# İstanbul şehrine ait kaç sipariş var?

SELECT 
    COUNT(DISTINCT o.OrderId) as OrderCount
FROM Orders o
INNER JOIN CustomerAddresses ca ON o.DeliveryAddressId = ca.AddressId
WHERE ca.City = 'İstanbul'
    AND o.IsActive = 1 
    AND ca.IsActive = 1;



