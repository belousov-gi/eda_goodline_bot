namespace eda_goodline_bot;

public class Dish
{
    public string ShortNameDish { get; init; }
    public int PriceDish { get; init; }
    public int WeightDish { get; init; }
    public string GeneralDishName { get; init; }

    public Dish(string generalDishName, string shortNameDish, int priceDish, int weightDish)
    {
        GeneralDishName = generalDishName;
        ShortNameDish = shortNameDish;
        PriceDish = priceDish;
        WeightDish = weightDish;
    }
}