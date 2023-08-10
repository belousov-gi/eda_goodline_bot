using System.Text.RegularExpressions;

namespace eda_goodline_bot;

public static class ActionsScenario
{
    public static void RunActionForStep(string userId, Step currentStep, Action action)
    {
        var actionId = action.ActionId;
        
        switch (currentStep.StepId)
        {
            case "availableDishes":
            {
                if (action.ActionId != "В главное меню")
                {
                    var order = OrderManager.Orders.Find(order => order.CustomerId == userId);
                    if (order == null)
                    {
                        order = new Order(userId);
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
        }
    }
}