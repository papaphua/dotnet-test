using TestTask.API.Controllers;
using TestTask.Data.Entities;

namespace TestTask.API.Tests;

public class ReportTests : BaseTest
{
    protected override async Task SetupBase()
    {
        // Add more users
        await Context.DbContext.Users.AddRangeAsync(new List<User>
        {
            new() { Id = 1, Email = "user1@gmail.com" },
            new() { Id = 2, Email = "user2@gmail.com" },
            new() { Id = 3, Email = "user3@gmail.com" },
            new() { Id = 4, Email = "user4@gmail.com" },
            new() { Id = 5, Email = "user5@gmail.com" },
            new() { Id = 6, Email = "user6@gmail.com" },
            new() { Id = 7, Email = "user7@gmail.com" },
            new() { Id = 8, Email = "user8@gmail.com" }
        });

        // Add more items
        await Context.DbContext.Items.AddRangeAsync(new List<Item>
        {
            new() { Id = 1, Name = "Item 1", Cost = 1 },
            new() { Id = 2, Name = "Item 2", Cost = 2 },
            new() { Id = 3, Name = "Item 3", Cost = 3 },
            new() { Id = 4, Name = "Item 4", Cost = 4 },
            new() { Id = 5, Name = "Item 5", Cost = 5 },
            new() { Id = 6, Name = "Item 6", Cost = 6 }
        });

        await Context.DbContext.UserItems.AddRangeAsync(new List<UserItem>
        {
            // User 1 purchases
            new() { UserId = 1, ItemId = 1, PurchaseDate = DateTime.UtcNow.AddDays(-1) }, // Most popular
            new() { UserId = 1, ItemId = 1, PurchaseDate = DateTime.UtcNow.AddDays(-1) }, //
            new() { UserId = 1, ItemId = 1, PurchaseDate = DateTime.UtcNow.AddDays(-1) }, //
            new() { UserId = 1, ItemId = 2, PurchaseDate = DateTime.UtcNow.AddDays(-1) },
            new() { UserId = 1, ItemId = 3, PurchaseDate = DateTime.UtcNow.AddDays(-2) },

            // User 2 purchases
            new() { UserId = 2, ItemId = 1, PurchaseDate = DateTime.UtcNow.AddDays(-1) },
            new() { UserId = 2, ItemId = 2, PurchaseDate = DateTime.UtcNow.AddDays(-2) },
            new() { UserId = 2, ItemId = 4, PurchaseDate = DateTime.UtcNow.AddDays(-3) },

            // User 3 purchases
            new() { UserId = 3, ItemId = 1, PurchaseDate = DateTime.UtcNow.AddDays(-3) },
            new() { UserId = 3, ItemId = 6, PurchaseDate = DateTime.UtcNow.AddDays(-1) },
            new() { UserId = 3, ItemId = 2, PurchaseDate = DateTime.UtcNow.AddDays(-5) },

            // User 4 purchases
            new() { UserId = 4, ItemId = 3, PurchaseDate = DateTime.UtcNow.AddDays(-6) },
            new() { UserId = 4, ItemId = 4, PurchaseDate = DateTime.UtcNow.AddDays(-6) },

            // User 5 purchases
            new() { UserId = 5, ItemId = 5, PurchaseDate = DateTime.UtcNow.AddDays(-7) },
            new() { UserId = 5, ItemId = 6, PurchaseDate = DateTime.UtcNow.AddDays(-8) }, // Most popular
            new() { UserId = 5, ItemId = 6, PurchaseDate = DateTime.UtcNow.AddDays(-8) }, //
            new() { UserId = 5, ItemId = 6, PurchaseDate = DateTime.UtcNow.AddDays(-8) }, //

            // User 6 purchases
            new() { UserId = 6, ItemId = 1, PurchaseDate = DateTime.UtcNow.AddDays(-9) },
            new() { UserId = 6, ItemId = 1, PurchaseDate = DateTime.UtcNow.AddDays(-10) },
            new() { UserId = 6, ItemId = 2, PurchaseDate = DateTime.UtcNow.AddDays(-11) },

            // User 7 purchases
            new() { UserId = 7, ItemId = 2, PurchaseDate = DateTime.UtcNow.AddDays(-12) }, // Most popular
            new() { UserId = 7, ItemId = 2, PurchaseDate = DateTime.UtcNow.AddDays(-12) }, //
            new() { UserId = 7, ItemId = 3, PurchaseDate = DateTime.UtcNow.AddDays(-13) },

            // User 8 purchases
            new() { UserId = 8, ItemId = 4, PurchaseDate = DateTime.UtcNow.AddDays(-14) },
            new() { UserId = 8, ItemId = 2, PurchaseDate = DateTime.UtcNow.AddDays(-12) }
        });

        await Context.DbContext.SaveChangesAsync();
    }

    [Test]
    public async Task GetPopularItemsAsync_ShouldReturnCorrectResults()
    {
        // Act
        var items = await Rait<MarketController>()
            .Call(controller => controller.GetPopularItemsAsync());
        
        // Assert
        Assert.That(items, Is.Not.Null);
        Assert.That(items, Has.Count.EqualTo(3));
        Assert.That(
            items.Select(item => item.ItemName),
            Is.EquivalentTo(new List<string> { "Item 2", "Item 6", "Item 1" }));
    }
    
    [Test]
    public async Task GetPopularItemsAsync_ShouldReturnWrongResults()
    {
        // Act
        var items = await Rait<MarketController>()
            .Call(controller => controller.GetPopularItemsAsync());
        
        // Assert
        Assert.That(items, Is.Not.Null);
        Assert.That(items, Has.Count.EqualTo(3));
        Assert.That(
            items.Select(item => item.ItemName),
            Is.Not.EquivalentTo(new List<string> { "Item 3", "Item 6", "Item 4" }));
    }
}