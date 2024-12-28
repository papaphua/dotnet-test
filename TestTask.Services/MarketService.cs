using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Data.Entities;

namespace TestTask.Services;

public class MarketService
{
    private readonly TestDbContext _testDbContext;

    public MarketService(TestDbContext testDbContext)
    {
        _testDbContext = testDbContext;
    }

    public async Task BuyAsync(int userId, int itemId)
    {
        var user = await _testDbContext.Users.FirstOrDefaultAsync(n => n.Id == userId);
        if (user == null)
            throw new Exception("User not found");
        var item = await _testDbContext.Items.FirstOrDefaultAsync(n => n.Id == itemId);
        if (item == null)
            throw new Exception("Item not found");

        if (user.Balance < item.Cost)
            if (item == null)
                throw new Exception("Not enough balance");

        user.Balance -= item.Cost;
        await _testDbContext.UserItems.AddAsync(new UserItem
        {
            UserId = userId,
            ItemId = itemId
        });

        await _testDbContext.SaveChangesAsync();
    }

    public async Task<List<PopularItemDto>> GetPopularItemsAsync()
    {
        var query = await _testDbContext.UserItems
            .GroupBy(x => new
            {
                x.UserId,
                x.ItemId,
                x.Item!.Name,
                x.PurchaseDate.Date
            })
            .Select(x => new
            {
                x.Key.Date.Year,
                x.Key.Name,
                TimesBoughtInMostPopularDay = x.Count()
            })
            .GroupBy(x => x.Year)
            .Select(x => x
                .OrderByDescending(c => c.TimesBoughtInMostPopularDay)
                .Take(3))
            .ToListAsync();

        return query
            .SelectMany(x => x)
            .OrderByDescending(x => x.Year)
            .ThenBy(x => x.TimesBoughtInMostPopularDay)
            .Select(x => new PopularItemDto(
                x.Year,
                x.Name,
                x.TimesBoughtInMostPopularDay))
            .ToList();
    }
}