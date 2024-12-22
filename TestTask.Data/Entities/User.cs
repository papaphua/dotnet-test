using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestTask.Data.Entities;

public class User
{
    public int Id { get; set; }
    [MaxLength(100)] public required string Email { get; set; }
    public decimal Balance { get; set; }
}

public class UserItem
{
    public int Id { get; set; }
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))] public User? User { get; set; }

    public int ItemId { get; set; }
    [ForeignKey(nameof(ItemId))] public Item? Item { get; set; }
}

public class Item
{
    public int Id { get; set; }
    [MaxLength(100)] public required string Name { get; set; }
    public required decimal Cost { get; set; }
}