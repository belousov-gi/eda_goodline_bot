using eda_goodline_bot.Iterfaces;

namespace eda_goodline_bot.Scenarios;

public class OrderFood : IScenario
{
    public string ScenarioId { get; set; }
    public List<Step> Steps { get; set; }

    public OrderFood(string scenarioId, List<Step> steps)
    {
        ScenarioId = scenarioId;
        Steps = steps;
        OrderManager.TruncateDishesCatalogAndOrders();
        AddLogicToScenario();
    }
    
      public void  AddLogicToScenario()
         {
             foreach (var step in Steps)
             {
                 switch (step.StepId)
                 {
                     //находим нужный шаг
                     case "currentOrder":
                     {
                         //добавляем логику для шага
                         step.StepLogic = ( session) =>
                         {
                             var answer = OrderManager.ShowOrder(session.UserId);
                             var answerMenu = session.CurrentStep.Actions;
                             session.SocialNetworkAdapter.SendMessage(session.ChatId, answer, answerMenu);
                         };
                         break;
                     }
      
                     case "availableDishes":
                     {
                         //логики для шага нет
                         
                         //добавляем логику для экшенов, которые на этом шаге есть

                         foreach (var action in step.Actions)
                         {
                             //добавляем лоигку
                             if (action.ActionId != "Мой заказ")
                             {
                                 //добавляем блюда в БД
                                 var dishId = OrderManager.AddDishToDb(action.ActionId);
                                 action.ExtraData = dishId.ToString();
                                 
                                 action.ActionLogic = session =>
                                 {
                                         var dishId = int.Parse(action.ExtraData);
                                         var userId = session.UserId;
                                         string resultOfAdding;
                                         try
                                         {
                                             if (DateTime.Now.Hour < 10)
                                             {
                                                 resultOfAdding = OrderManager.AddDishToOrder(userId, dishId);  
                                             }
                                             else
                                             {
                                                 resultOfAdding = "Заказать блюдо можно только до 10:00";
                                             }
                                         }
                                         catch (Exception e)
                                         {
                                             Console.WriteLine(e);
                                             resultOfAdding = "Не удалось добавить блюдо";
                                         }
                                         session.SocialNetworkAdapter.SendMessage(session.ChatId, resultOfAdding);
                                 };
                             }
                         }
                         break;
                     }
      
                     case "deletingPositions":
                     {
                         var defaultActonsAmount = step.Actions.Count;
                         
                         //Динамически формируем экшены из того, что клиент доабвил в заказ
                         step.StepLogic = session =>
                         {
                             List<Action> dynamicActionsForStep = new List<Action>();
                             var order = OrderManager.GetOrderById(session.UserId);
                             var answeredText = "Выберeте блюдо для удаления";
                             
                             if (order.Count == 0)
                             {
                                 answeredText = "Не добавлено ни одного блюда";
                             }
                             else
                             {
                                 foreach (var dish in order)
                                 {
                                     string actionId = dish.GeneralName;
                                     Action action = new Action(actionId);
                                     dynamicActionsForStep.Add(action);
                                 }
                             }
      
                             var currentActions = session.CurrentStep.Actions;
                             var currentActionsAmount = currentActions.Count;
                             
                             //Если уже есть добавленные блюда, то очищаем весь список за исключением дефолтовых кнопок
                             if (currentActionsAmount > defaultActonsAmount)
                             {
                                 currentActions.RemoveRange(0, currentActionsAmount - defaultActonsAmount);
                             }
                             
                             //добавляем к динамическим шагам стандартные шаги из JSON
                             dynamicActionsForStep.AddRange(currentActions);
                             session.CurrentStep.Actions = dynamicActionsForStep;
                             
                             //Каждому экшену добавляем логику
                             foreach (var action in dynamicActionsForStep)
                             {
                                 action.ActionLogic = _ =>
                                 {
                                     var actionId = action.ActionId;
                                     if (actionId != "В главное меню" && actionId != "Мой заказ")
                                     {
                                         // var DishId = int.Parse(action.ExtraData);
                                         var dishId = OrderManager.GetDishIdFromCatalog(action.ActionId);
                                         var orderedDish =  order.Find(x => x.DishId == dishId);
                                         
                                         OrderManager.RemoveDishFromOrder(orderedDish.CustomerId, dishId);
                                        
                                         action.ActionAnswer = $"Удалено из заказа: {orderedDish.GeneralName}";
                                     }
                                 };
                             }
      
                             answeredText = session.CurrentStep.LastAction == null
                                 ? answeredText
                                 : session.CurrentStep.LastAction.ActionAnswer; 
                             session.SocialNetworkAdapter.SendMessage(session.ChatId, answeredText, dynamicActionsForStep);
                         };
                         break;
                     }
                 }
             }
         }
}

