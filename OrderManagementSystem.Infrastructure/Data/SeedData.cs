using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderManagementSystem.Domain.Entities;

namespace OrderManagementSystem.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Database'in oluştuğundan emin ol
            await context.Database.EnsureCreatedAsync();

            // Eğer veri yoksa seed data ekle
            if (!context.Customers.Any())
            {
                Console.WriteLine("Seed data başlıyor...");

                // Önce temel verileri ekle ve kaydet (ID'ler oluşsun)
                await SeedCustomers(context);
                await context.SaveChangesAsync();

                await SeedStocks(context);
                await context.SaveChangesAsync();

                // Sonra ilişkili verileri ekle
                await SeedCustomerAddresses(context);
                await context.SaveChangesAsync();

                await SeedOrders(context);
                await context.SaveChangesAsync();

                await SeedOrderDetails(context);
                await context.SaveChangesAsync();

                // Toplam fiyatları güncelle
                await UpdateOrderTotals(context);

                Console.WriteLine($"Seed data başarıyla eklendi:");
                Console.WriteLine($"- {context.Customers.Count()} müşteri");
                Console.WriteLine($"- {context.CustomerAddresses.Count()} adres");
                Console.WriteLine($"- {context.Stocks.Count()} stok");
                Console.WriteLine($"- {context.Orders.Count()} sipariş");
                Console.WriteLine($"- {context.OrderDetails.Count()} sipariş detayı");
            }
        }

        private static async Task SeedCustomers(ApplicationDbContext context)
        {
            if (context.Customers.Any()) return;

            var customers = new[]
            {
                new Customer { CustomerName = "TLS Logistics", IsActive = true },
                new Customer { CustomerName = "ABC Teknoloji", IsActive = true },
                new Customer { CustomerName = "XYZ Ltd. Şti.", IsActive = true },
                new Customer { CustomerName = "Global Trade", IsActive = true },
                new Customer { CustomerName = "Mega Şirket", IsActive = true },
                new Customer { CustomerName = "Anadolu Ticaret", IsActive = true },
                new Customer { CustomerName = "Ege İhracat", IsActive = true },
                new Customer { CustomerName = "Akdeniz Ltd.", IsActive = true },
                new Customer { CustomerName = "Karadeniz A.Ş.", IsActive = true },
                new Customer { CustomerName = "Marmara Holding", IsActive = true },
                new Customer { CustomerName = "İç Anadolu Ticaret", IsActive = true },
                new Customer { CustomerName = "Doğu Firması", IsActive = true },
                new Customer { CustomerName = "Batı Şirket", IsActive = true },
                new Customer { CustomerName = "Kuzey Ltd.", IsActive = true },
                new Customer { CustomerName = "Güney A.Ş.", IsActive = true },
                new Customer { CustomerName = "Merkez Ticaret", IsActive = true },
                new Customer { CustomerName = "Şehir Market", IsActive = true },
                new Customer { CustomerName = "Teknoloji Dünyası", IsActive = true },
                new Customer { CustomerName = "İnşaat Malzeme", IsActive = true },
                new Customer { CustomerName = "Ofis Çözüm", IsActive = true }
            };

            await context.Customers.AddRangeAsync(customers);
            Console.WriteLine($"{customers.Length} müşteri eklendi.");
        }

        private static async Task SeedStocks(ApplicationDbContext context)
        {
            if (context.Stocks.Any()) return;

            var stocks = new[]
            {
                new Stock { StockName = "Dizüstü Bilgisayar", Unit = "Adet", Price = 15000.00m, Barcode = "1234567890123", IsActive = true },
                new Stock { StockName = "Gaming Mouse", Unit = "Adet", Price = 450.50m, Barcode = "1234567890124", IsActive = true },
                new Stock { StockName = "Mekanik Klavye", Unit = "Adet", Price = 850.75m, Barcode = "1234567890125", IsActive = true },
                new Stock { StockName = "24\" IPS Monitör", Unit = "Adet", Price = 3200.00m, Barcode = "1234567890126", IsActive = true },
                new Stock { StockName = "Web Kamera 1080p", Unit = "Adet", Price = 800.00m, Barcode = "1234567890127", IsActive = true },
                new Stock { StockName = "Laptop Çantası", Unit = "Adet", Price = 350.25m, Barcode = "1234567890128", IsActive = true },
                new Stock { StockName = "USB-C Hub", Unit = "Adet", Price = 280.00m, Barcode = "1234567890129", IsActive = true },
                new Stock { StockName = "SSD 500GB", Unit = "Adet", Price = 1200.00m, Barcode = "1234567890130", IsActive = true },
                new Stock { StockName = "DDR4 16GB RAM", Unit = "Adet", Price = 900.00m, Barcode = "1234567890131", IsActive = true },
                new Stock { StockName = "Wireless Kulaklık", Unit = "Adet", Price = 650.00m, Barcode = "1234567890132", IsActive = true }
            };

            await context.Stocks.AddRangeAsync(stocks);
            Console.WriteLine($"{stocks.Length} stok eklendi.");
        }

        private static async Task SeedCustomerAddresses(ApplicationDbContext context)
        {
            if (context.CustomerAddresses.Any()) return;

            var customers = await context.Customers.ToListAsync();
            var addresses = new List<CustomerAddress>();
            var cities = new[] { "İstanbul", "Ankara", "İzmir", "Bursa", "Antalya", "Adana", "Konya", "Gaziantep" };
            var random = new Random();

            foreach (var customer in customers)
            {
                var city = cities[random.Next(cities.Length)];

                // Teslimat adresi
                addresses.Add(new CustomerAddress
                {
                    CustomerId = customer.CustomerId,
                    AddressType = "Delivery",
                    Country = "Türkiye",
                    City = city,
                    Town = GetDistrictByCity(city),
                    Address = $"{GetStreetName()} No:{random.Next(1, 200)}",
                    Email = $"{customer.CustomerName.ToLower().Replace(" ", "").Replace(".", "").Replace("ş", "s").Replace("ı", "i")}@gmail.com",
                    Phone = $"+90555{random.Next(100, 999)}{random.Next(1000, 9999)}",
                    PostalCode = $"{random.Next(34000, 34999)}",
                    IsActive = true
                });

                // Fatura adresi - %40 ihtimalle farklı şehir
                var invoiceCity = random.Next(100) < 40 ? cities[random.Next(cities.Length)] : city;

                addresses.Add(new CustomerAddress
                {
                    CustomerId = customer.CustomerId,
                    AddressType = "Invoice",
                    Country = "Türkiye",
                    City = invoiceCity,
                    Town = GetDistrictByCity(invoiceCity),
                    Address = $"{GetStreetName()} No:{random.Next(1, 200)}",
                    Email = $"fatura@{customer.CustomerName.ToLower().Replace(" ", "").Replace(".", "").Replace("ş", "s").Replace("ı", "i")}.com",
                    Phone = $"+90555{random.Next(100, 999)}{random.Next(1000, 9999)}",
                    PostalCode = $"{random.Next(34000, 34999)}",
                    IsActive = true
                });
            }

            await context.CustomerAddresses.AddRangeAsync(addresses);
            Console.WriteLine($"{addresses.Count} adres eklendi.");
        }

        private static async Task SeedOrders(ApplicationDbContext context)
        {
            if (context.Orders.Any()) return;

            var customers = await context.Customers.ToListAsync();
            var addresses = await context.CustomerAddresses.ToListAsync();
            var orders = new List<Order>();
            var random = new Random();

            // Her müşteri için 1-3 sipariş oluştur
            foreach (var customer in customers)
            {
                var customerAddresses = addresses.Where(a => a.CustomerId == customer.CustomerId).ToList();
                if (!customerAddresses.Any()) continue;

                var deliveryAddress = customerAddresses.First(a => a.AddressType == "Delivery");
                var invoiceAddress = customerAddresses.First(a => a.AddressType == "Invoice");

                var orderCount = random.Next(1, 4); // 1-3 sipariş

                for (int i = 0; i < orderCount; i++)
                {
                    var order = new Order
                    {
                        CustomerId = customer.CustomerId,
                        OrderDate = DateTime.Now.AddDays(-random.Next(1, 365)),
                        OrderNo = $"ORD{DateTime.Now:yyyyMMdd}-{customer.CustomerId:000}-{i + 1:00}",
                        TotalPrice = 0,
                        Tax = 0.18m,
                        DeliveryAddressId = deliveryAddress.AddressId,
                        InvoiceAddressId = invoiceAddress.AddressId,
                        IsActive = true
                    };

                    orders.Add(order);
                }
            }

            await context.Orders.AddRangeAsync(orders);
            Console.WriteLine($"{orders.Count} sipariş eklendi.");
        }

        private static async Task SeedOrderDetails(ApplicationDbContext context)
        {
            if (context.OrderDetails.Any()) return;

            var orders = await context.Orders.ToListAsync();
            var stocks = await context.Stocks.ToListAsync();
            var orderDetails = new List<OrderDetail>();
            var random = new Random();

            foreach (var order in orders)
            {
                // Her sipariş için 1-4 arası ürün ekle
                var detailCount = random.Next(1, 5);
                var usedStocks = new HashSet<int>();

                for (int i = 0; i < detailCount; i++)
                {
                    Stock stock;
                    int attempt = 0;
                    do
                    {
                        stock = stocks[random.Next(stocks.Count)];
                        attempt++;
                        if (attempt > 10) break; // Sonsuz döngüyü önle
                    } while (usedStocks.Contains(stock.StockId));

                    usedStocks.Add(stock.StockId);

                    var amount = random.Next(1, 6); // 1-5 arası miktar

                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.OrderId,
                        StockId = stock.StockId,
                        Amount = amount,
                        IsActive = true
                    };

                    orderDetails.Add(orderDetail);
                }
            }

            await context.OrderDetails.AddRangeAsync(orderDetails);
            Console.WriteLine($"{orderDetails.Count} sipariş detayı eklendi.");
        }

        private static async Task UpdateOrderTotals(ApplicationDbContext context)
        {
            var orders = await context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Stock)
                .Where(o => o.TotalPrice == 0)
                .ToListAsync();

            foreach (var order in orders)
            {
                decimal subtotal = order.OrderDetails.Sum(od => od.Amount * od.Stock.Price);
                order.TotalPrice = subtotal * (1 + order.Tax);
            }

            await context.SaveChangesAsync();
            Console.WriteLine("Sipariş toplamları güncellendi.");
        }

        // Yardımcı metodlar
        private static string GetDistrictByCity(string city)
        {
            return city switch
            {
                "İstanbul" => "Kadıköy",
                "Ankara" => "Çankaya",
                "İzmir" => "Karşıyaka",
                "Bursa" => "Nilüfer",
                "Antalya" => "Muratpaşa",
                "Adana" => "Seyhan",
                "Konya" => "Selçuklu",
                "Gaziantep" => "Şahinbey",
                _ => "Merkez"
            };
        }

        private static string GetStreetName()
        {
            var streets = new[]
            {
                "Atatürk", "Cumhuriyet", "İstiklal", "Fatih", "Yavuz", "Kanuni", "Barbaros",
                "Mimar Sinan", "Şehitler", "Zafer", "Gazi", "Osman Gazi"
            };

            var random = new Random();
            return streets[random.Next(streets.Length)];
        }
    }
}