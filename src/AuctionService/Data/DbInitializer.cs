using AuctionService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data;

public class DbInitializer
{
    public static void InitDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetService<AuctionDbContext>()
            ?? throw new InvalidOperationException("Failed to retrieve AuctionDbContext from the service provider.");

        SeedData(context);
    }

    private static void SeedData(AuctionDbContext context)
    {
        context.Database.Migrate();

        if (context.Auctions.Any())
        {
            Console.WriteLine("Already have data - no seeding required");
            return;
        }

        var auctions = new List<Auction>();
        
        // Define base data for random generation
        var makes = new[] { "BMW", "Mercedes", "Audi", "Toyota", "Honda", "Ford", "Chevrolet", 
            "Volkswagen", "Porsche", "Ferrari", "Lamborghini", "Maserati", "Bentley", 
            "Rolls-Royce", "McLaren", "Bugatti", "Aston Martin", "Jaguar", "Land Rover", "Lexus" };
        var models = new Dictionary<string, string[]>
        {
            { "Ford", new[] { "GT", "Mustang", "Model T" } },
            { "Bugatti", new[] { "Veyron" } },
            { "Mercedes", new[] { "SLK", "AMG GT" } },
            { "BMW", new[] { "X1" } },
            { "Ferrari", new[] { "Spider", "F-430" } },
            { "Audi", new[] { "R8", "TT" } },
            { "Porsche", new[] { "911" } },
            { "Tesla", new[] { "Model S" } },
            { "Lamborghini", new[] { "Aventador" } },
            { "Chevrolet", new[] { "Corvette" } },
            { "Maserati", new[] { "Ghibli" } },
            { "Jaguar", new[] { "F-Type" } },
            { "Bentley", new[] { "Continental GT" } },
            { "Aston Martin", new[] { "DB11" } },
            { "McLaren", new[] { "720S" } }
        };
        var colors = new[] { "Black", "White", "Silver", "Gray", "Red", "Blue", "Green", 
            "Yellow", "Orange", "Purple", "Brown", "Gold", "Bronze", "Champagne", 
            "British Racing Green", "Dark Blue", "Rust" };
        var sellers = new[] { "bob", "alice", "tom" };
        var baseImageUrls = new[] {
            "https://cdn.pixabay.com/photo/2016/04/01/12/16/car-1300629_960_720.png",
            "https://cdn.pixabay.com/photo/2020/07/19/07/36/car-5418586_960_720.jpg",
            "https://cdn.pixabay.com/photo/2019/12/26/20/50/audi-r8-4721217_960_720.jpg",
            "https://cdn.pixabay.com/photo/2017/03/27/14/56/auto-2179220_960_720.jpg",
            "https://cdn.pixabay.com/photo/2015/09/02/12/25/tesla-918785_960_720.jpg",
            "https://cdn.pixabay.com/photo/2016/11/18/12/51/automobile-1834274_1280.jpg",
            "https://cdn.pixabay.com/photo/2018/01/18/18/00/ferrari-3090880_1280.jpg",
            "https://cdn.pixabay.com/photo/2017/03/20/04/57/truck-2158284_1280.jpg",
            "https://cdn.pixabay.com/photo/2014/09/07/22/34/car-438467_1280.jpg",
            "https://cdn.pixabay.com/photo/2016/12/03/18/57/amg-1880381_1280.jpg",
            "https://cdn.pixabay.com/photo/2015/05/28/23/12/auto-788747_1280.jpg",
            "https://cdn.pixabay.com/photo/2015/01/19/13/51/car-604019_1280.jpg",
            "https://cdn.pixabay.com/photo/2016/12/07/21/50/car-1890494_1280.jpg",
        };

        var random = new Random();
        // Generate 100 auctions (including your original 20)
        for (int i = 0; i < 100; i++)
        {
            var make = makes[random.Next(makes.Length)];
            var modelArray = models.ContainsKey(make) ? models[make] : new[] { $"Model {(char)('A' + random.Next(26))}" };
            var model = modelArray[random.Next(modelArray.Length)];
            var year = random.Next(2015, 2024);
            var color = colors[random.Next(colors.Length)];
            var mileage = random.Next(1000, 150000);
            var reservePrice = random.Next(20000, 500000);
            var daysToAdd = random.Next(-10, 60);
            var seller = sellers[random.Next(sellers.Length)];
            var imageUrl = baseImageUrls[random.Next(baseImageUrls.Length)];

            auctions.Add(new()
            {
                Id = Guid.NewGuid(),
                Status = daysToAdd < 0 ? Status.ReserveNotMet : Status.Live,
                ReservePrice = reservePrice,
                Seller = seller,
                AuctionEnd = DateTime.UtcNow.AddDays(daysToAdd),
                Item = new Item
                {
                    Make = make,
                    Model = model,
                    Color = color,
                    Mileage = mileage,
                    Year = year,
                    ImageUrl = imageUrl
                }
            });
        }

        context.AddRange(auctions);
        context.SaveChanges();
    }
}
