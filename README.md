
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
