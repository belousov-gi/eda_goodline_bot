namespace eda_goodline_bot;

public class Dish
{
    public string NameDish { get; init; }
    public int CostDish { get; init; }

    public Dish(string nameDish, int costDish)
    {
        NameDish = nameDish;
        CostDish = costDish;
    }
}