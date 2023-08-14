namespace eda_goodline_bot;

public class Dish
{
    public string NameDish { get; init; }
    public int PriceDish { get; init; }

    public Dish(string nameDish, int priceDish)
    {
        NameDish = nameDish;
        PriceDish = priceDish;
    }
}