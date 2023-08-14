namespace eda_goodline_bot;

public static class OrderManager
{
    public static List<Order> Orders = new List<Order>();

    public static string ShowOrder(string customerId)
    {
        var order = Orders.Find(order => order.CustomerId == customerId);

        var orderDishesCount = order.Dishes.Count;
        string[] listOfDishes = new string [orderDishesCount];
        Dish currentDish;
        
        for (int i = 0; i < orderDishesCount; i++)
        {
            currentDish = order.Dishes[i];
            listOfDishes[i] = currentDish.NameDish + " / " + currentDish.PriceDish + " руб.";
        }
        string answer = string.Join("\n", listOfDishes);
        return answer;
    }
}