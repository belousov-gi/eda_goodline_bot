using System.Text.RegularExpressions;
using eda_goodline_bot.Iterfaces;

namespace eda_goodline_bot;

public static class ActionsScenario
{
    public static void RunActionForStep(ISocialNetworkAdapter socialNetworkAdapter, string userId, int chatId, Step currentStep, Action action)
    {
        var actionId = action.ActionId;
        
        switch (currentStep.StepId)
        {
            case "availableDishes":
            {
                if (action.NavigateToStep != "/start")
                {
                    var order = OrderManager.Orders.Find(order => order.CustomerId == userId);
                    if (order == null)
                    {
                        order = new Order(userId);
                        OrderManager.Orders.Add(order);
                    }

                    string patternDishName = @".+(?=\s\/\s*\d*\D*\W\/)";
                    string patternDishCost = @"\d+(?=.руб)";

                    var dishName = Regex.Match(actionId, patternDishName).ToString();
                    var dishCost = int.Parse(Regex.Match(actionId, patternDishCost).ToString());

                    var orderedDish = new Dish(dishName, dishCost);
                    order.Dishes.Add(orderedDish);
                }
                break; 
            }
            
            case "/start":
                if (action.NavigateToStep == "currentOrder")
                {
                    var answer = OrderManager.ShowOrder(userId);
                    socialNetworkAdapter.SendMessage(chatId, answer);
                }
                break;
        }
    }
}