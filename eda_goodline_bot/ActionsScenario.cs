using System.Text.RegularExpressions;
using eda_goodline_bot.Iterfaces;

namespace eda_goodline_bot;

public static class ActionsScenario
{
    public static void RunActionForStep(ISocialNetworkAdapter socialNetworkAdapter, string userId, int chatId, Step step, Action? action)
    {
        string? nextStep;
        string? actionId;
        
        if (action != null)
        {
            actionId = action.ActionId;
            nextStep = action.NavigateToStep;
        }
        else
        {
            actionId = null;
            nextStep = null;
        }
        
        
        switch (step.StepId)
        {
            case "availableDishes":
            {
                if (nextStep != "/start" && actionId != null)
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
                if (nextStep == "currentOrder")
                {
                    var answer = OrderManager.ShowOrder(userId);
                    socialNetworkAdapter.SendMessage(chatId, answer);
                }
                break;

            // case "currentOrder":
            // {
            //     if (nextStep == "deletingPositions")
            //     {
            //         var order = OrderManager.Orders.Find(order => order.CustomerId == userId);
            //         var dishesInOrder = order.Dishes;
            //         var answeredMenu = new List<Action>(dishesInOrder.Count + 1);
            //         
            //         if (order == null)
            //         {
            //             var answer = "В вашем заказе пусто";
            //             socialNetworkAdapter.SendMessage(chatId, answer);
            //         }
            //         else
            //         {
            //             Action dishMenuPoint;
            //             string dishNameForMenu;
            //             
            //             foreach (var dish in dishesInOrder)
            //             {
            //                 dishNameForMenu = dish.NameDish + " / " + dish.PriceDish;
            //                 dishMenuPoint = new Action(dishNameForMenu, null, null);
            //                 answeredMenu.Add(dishMenuPoint);
            //             }
            //             
            //             
            //             
            //         }
            //     }
            // }


        }
    }
}