using eda_goodline_bot.Enums;

namespace eda_goodline_bot;

public class Order
{
    public string CustomerId { get; init; }
    public List<Dish> Dishes = new();

    public Order(string customerId)
    {
        CustomerId = customerId;
    }
}