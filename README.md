
Eger proje masaüstünde ise

Path                                          
----                                          
C:\Users\<Kullanıcı İsmini Yaz>\Desktop\OrderManagementSystem


# Migrations oluşturma
dotnet ef migrations add InitialCreate --project OrderManagementSystem.Infrastructure --startup-project OrderManagementSystem.API

# Yeniden oluştur
dotnet ef database update --project OrderManagementSystem.Infrastructure --startup-project OrderManagementSystem.API

Projeyi Çalıştır

dotnet run --project OrderManagementSystem.API
