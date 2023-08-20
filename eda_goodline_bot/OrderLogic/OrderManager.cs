namespace eda_goodline_bot;

public static class OrderManager
{
    public static List<Order> Orders = new();

    public static string ShowOrder(string customerId)
    {
        Order? order = Orders.Find(order => order.CustomerId == customerId);

        if (order != null)
        {  
            var orderDishesCount = order.Dishes.Count;
            string[] listOfDishes = new string [orderDishesCount];
            Dish currentDish;
        
            for (int i = 0; i < orderDishesCount; i++)
            {
                currentDish = order.Dishes[i];
                listOfDishes[i] = currentDish.GeneralDishName;
            }
            string answer = string.Join("\n", listOfDishes);
            return answer; 
        }
        return "Не до,авлено ниодного блюда в заказ";
    }

    public static Order? GetOrderById(string customerId)
    {
        return Orders.Find(order => order.CustomerId == customerId);
    }
    
    public static Dish? GetDishFromOrder(Order order, string generalDishName)
    {
        return order.Dishes.Find(dish => dish.GeneralDishName == generalDishName);
    }
    
    public static void RemoveDishFromOrder(Order order, Dish dish)
    {
        order.Dishes.Remove(dish);
    }
    
    
}