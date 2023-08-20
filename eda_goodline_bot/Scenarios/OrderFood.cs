using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace eda_goodline_bot.Scenarios;

public class OrderFood : Scenario
{

    public OrderFood(string scenarioId, List<Step> steps) : base(scenarioId, steps)
    {
        AddLogicToScenario(this);
    }
    
      public static void AddLogicToScenario(Scenario scenario)
         {
             foreach (var step in scenario.Steps)
             {
                 // var stepId = step.StepId;
                 switch (step.StepId)
                 {
                     //находим нужный шаг
                     case "currentOrder":
                     {
                         //добавляем логику для шага
                         step.StepLogic = (session) =>
                         {
                             var answer = OrderManager.ShowOrder(session.UserId);
                             session.SocialNetworkAdapter.SendMessage(session.ChatId, answer);
                         };
                         
                         break;
                         
                     }

                     case "availableDishes":
                     {
                         //логики для шага нет
                         
                         //добавляем логику для экшенов, которые на этом шаге есть
                         string actionId;
                         
                         foreach (var action in step.Actions)
                         {
                             if (action.ActionId != "Завершить")
                             {
                                 action.ActionLogic = session =>
                                 {
                                     try
                                     {
                                         actionId = action.ActionId;
                                         var userId = session.UserId;
                                         var order = OrderManager.Orders.Find(order => order.CustomerId == userId);
                                         if (order == null)
                                         {
                                             order = new Order(userId);
                                             OrderManager.Orders.Add(order);
                                         }
                                        
                                         //TODO: мб попробовать заюзать другие кнопки, которые передают значнеие еще. 
                                         // в значениии передавать JSON и все
                                         string patternDishName = @".+(?=\s\/\s*\d*\D*\W\/)";
                                         string patternDishCost = @"\d+(?=.руб)";
                                         string patternDishWeight = @"\d+(?=.гр)";
                                     
                                         var dishName = Regex.Match(actionId, patternDishName).ToString();
                                         var dishCost = int.Parse(Regex.Match(actionId, patternDishCost).ToString());
                                         var dishWeight = int.Parse(Regex.Match(actionId, patternDishWeight).ToString());

                                         var orderedDish = new Dish(actionId, dishName, dishCost, dishWeight);
                                         order.Dishes.Add(orderedDish);
                                         session.SocialNetworkAdapter.SendMessage(session.ChatId, $"{dishName} добавлен в заказ");
                                     }
                                     catch (Exception e)
                                     {
                                         Console.WriteLine(e);
                                         session.SocialNetworkAdapter.SendMessage(session.ChatId, "Не удалось доабвить блюдо");
                                     }
                                 };
                             }
                         }
                         break;
                     }

                     case "deletingPositions":
                     {
                         //Динамически формируем экшены из того, что клиент доабвил в заказ
                         step.StepLogic = session =>
                         {
                             List<Action> actionsForStep = new List<Action>();
                             var order = OrderManager.GetOrderById(session.UserId);
                             string answeredText = "Не добавлено ни одного блюда";

                             if (order != null)
                             {
                                 foreach (var dish in order.Dishes)
                                 {
                                     string actionId = dish.GeneralDishName;
                                     Action action = new Action(actionId);
                                     actionsForStep.Add(action);
                                     answeredText = "Выбери позиции, которые хочешь удалить из заказа";
                                 }
                             }
                             
                             actionsForStep.AddRange(session.CurrentStep.Actions);
                             session.SocialNetworkAdapter.SendMessage(session.ChatId, answeredText, actionsForStep);
                         };

                         foreach (var action in step.Actions)
                         {
                             action.ActionLogic = session =>
                             {
                                 var actionId = action.ActionId;
                                 if (actionId != "В главное меню" && actionId != "Мой заказ")
                                 {
                                     var order = OrderManager.GetOrderById(session.UserId);
                                     var generalDishName = action.ActionId;
                                     var dish = OrderManager.GetDishFromOrder(order, generalDishName);
                                     OrderManager.RemoveDishFromOrder(order, dish);
                                 }
                             };
                         }
                         break;
                     }
                 }
             }
         }
    // private void LogicForSteps(Session session)
    // {
    //     switch (session.CurrentStep.StepId)
    //     {
    //         case "currentOrder":
    //         {
    //             var answer = OrderManager.ShowOrder(session.UserId);
    //             session.SocialNetworkAdapter.SendMessage(session.ChatId, answer);
    //             break;
    //         }
    //     }
    // }
}

