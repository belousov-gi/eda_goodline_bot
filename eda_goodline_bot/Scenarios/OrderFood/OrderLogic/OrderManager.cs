using System.Text.RegularExpressions;

namespace eda_goodline_bot;

public static class OrderManager
{
    public static List<Order> Orders = new();

    public static string ShowOrder(string customerId)
    {
        Order? order = Orders.Find(order => order.CustomerId == customerId);
       
        if (order != null && order.Dishes.Count > 0)
        {  
            int orderDishesCount = order.Dishes.Count;
            string[] listOfDishesNames = new string [orderDishesCount];
            Dish currentDish;
            int generalSum = 0;
            
            for (int i = 0; i < orderDishesCount; i++)
            {
                currentDish = order.Dishes[i];
                listOfDishesNames[i] = currentDish.GeneralDishName;
                generalSum += currentDish.PriceDish;
            }

            string answer = "Твой заказ:\n \n";
            answer += string.Join("\n", listOfDishesNames);
            answer += $"\n \n Итого: {generalSum} руб.";
            return answer; 
        }
        return "Твой заказ:\n \n Не добавлено ниодного блюда в заказ";
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

    public static Dish CreateDishFromString(string inputText)
    {
        
        //TODO: мб попробовать заюзать другие кнопки, которые передают значнеие еще. 
        // в значениии передавать JSON и все
        string patternDishName = @".+(?=\s\/\s*\d*\D*\W\/)";
        string patternDishCost = @"\d+(?=.руб)";
        string patternDishWeight = @"\d+(?=.гр)";
                                     
        var dishName = Regex.Match(inputText, patternDishName).ToString();
        var dishCost = int.Parse(Regex.Match(inputText, patternDishCost).ToString());
        var dishWeight = int.Parse(Regex.Match(inputText, patternDishWeight).ToString());

        var createdDish = new Dish(inputText, dishName, dishCost, dishWeight);
        return createdDish;
    }

    public static string AddDishToOrder(string userId, Dish dish)
    {
        var order = Orders.Find(order => order.CustomerId == userId);
        if (order == null)
        {
            order = new Order(userId);
            Orders.Add(order);
        }
        order.Dishes.Add(dish);
        string answer = $"{dish.ShortNameDish} добавлен в заказ";
        
        return answer;
    }
    
    
}