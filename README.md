
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

# Frontend kısmı

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


