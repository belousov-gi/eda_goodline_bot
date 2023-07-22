using eda_goodline_bot.Enums;

namespace eda_goodline_bot;

public class GeneralOrder
{
    public List<Dictionary<Enum, int>> ListDishes { get; set; }
    
    public Dictionary<Enum, int> CreateGeneralListDishes()
    {
        // перебираем все заказы и формируем рдин вида:
        // [наименование блюда] - [кол-во]
        
        return new Dictionary<Enum, int>()
        {
            [Dishes.salad] = 1
        };
    }
    
}