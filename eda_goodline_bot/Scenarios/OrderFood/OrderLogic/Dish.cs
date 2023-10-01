using System.ComponentModel.DataAnnotations.Schema;

namespace eda_goodline_bot;

public class Dish
{
    [Column("short_name")]
    public string ShortNameDish { get; init; }
    [Column("price")]
    public int PriceDish { get; init; }
    [Column("weight")]
    public int WeightDish { get; init; }
    [Column("general_name")]
    public string GeneralDishName { get; init; }
    [Column("dish_id")]
    public int Id { get; set; }

    public Dish(string generalDishName, string shortNameDish, int priceDish, int weightDish)
    {
        GeneralDishName = generalDishName;
        ShortNameDish = shortNameDish;
        PriceDish = priceDish;
        WeightDish = weightDish;
    }
}