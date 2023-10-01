using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace eda_goodline_bot;

public static class OrderManager
{
    public static string ShowOrder(int customerId)
    {
            var order = GetOrderById(customerId);
    
            if (order.Any())
            {  
                var orderDishesCount = order.Count();
                string[] listOfDishesNames = new string [orderDishesCount];
                int generalSum = 0;
                
                for (int i = 0; i < orderDishesCount; i++)
                {
                    var currentDish = order[i];
                    listOfDishesNames[i] = currentDish.GeneralName;
                    generalSum += currentDish.PriceDish;
                }
    
                string answer = "Твой заказ:\n \n";
                answer += string.Join("\n", listOfDishesNames);
                answer += $"\n \n Итого: {generalSum} руб.";
                return answer; 
            }
            return "Твой заказ:\n \n Не добавлено ниодного блюда в заказ";
    }


 
    public static List<OrderedDishInfo> GetOrderById(int customerId)
    {
        using (MySqlStorage db = new MySqlStorage())
        {
            var order = db.ordered_dishes.Where(p => p.CustomerId == customerId)
                .Join(db.dish_catalog,
                    p => p.DishId,
                    c => c.Id,
                    (p, c) => new OrderedDishInfo()
                    {
                        GeneralName = c.GeneralDishName,
                        PriceDish = c.PriceDish,
                        DishId = p.DishId,
                        CustomerId = p.CustomerId
                    }).ToList();
            
            var qqq = db.ordered_dishes.Where(p => p.CustomerId == customerId)
                .Join(db.dish_catalog,
                    p => p.DishId,
                    c => c.Id,
                    (p, c) => new OrderedDishInfo()
                    {
                        GeneralName = c.GeneralDishName,
                        PriceDish = c.PriceDish,
                        DishId = p.DishId,
                        CustomerId = p.CustomerId
                    }).Select(p => p.GeneralName).ToList();;
            
            // var result = from order in db.ordered_dishes
            //     join dish in db.dish_catalog on order.DishId equals dish.Id
            //     select new
            //     { 
            //         Name = phone.Name, 
            //         Company = company.Name, 
            //         Price = phone.Price, 
            //         Country = country.Name 
            //     };
            return order;
        }
    }
    
    public static int GetDishIdFromCatalog(string generalDishName)
    {
        using (MySqlStorage db = new MySqlStorage())
        {
            var DishId = db.dish_catalog.Where(p => p.GeneralDishName == generalDishName).Select(p => new
            {
                DishId = p.Id
            });
            
            return DishId.First().DishId;
        }
    }
    
    public static void RemoveDishFromOrder(int userId, int dishId)
    {
        using (MySqlStorage db = new MySqlStorage())
        {
            var date = DateTime.Today.Date.ToString("yyyy-MM-dd");
            // db.ordered_dishes.Where(p => p.DishId == dishId && p.CustomerId == userId && p.DateOfOrder == DateTime.Today);
            var rr = db.ordered_dishes.FromSql(
                @$"DELETE
                   FROM ordered_dishes 
                   WHERE ordered_dishes.id = (SELECT tmp.id 
                                              FROM 
                                                    (SELECT id FROM ordered_dishes 
                                                     WHERE (ordered_dishes.user_id = {userId})
                                                     AND  (ordered_dishes.dish_id = {dishId})
                                                     AND (ordered_dishes.order_day = {date}) 
                                                     LIMIT 1) as tmp);");
        }
    }

    public static void TruncateDishesCatalogAndOrders()
    {
        using (MySqlStorage db = new MySqlStorage())
        {
            db.dish_catalog.ExecuteDelete();
            db.ordered_dishes.ExecuteDelete();
        }
    } 
    public static int AddDishToDb(string inputText)
    {
        
        //TODO: мб попробовать заюзать другие кнопки, которые передают значнеие еще. 
        // в значениии передавать JSON и все
        string patternDishName = @".+(?=\s\/\s*\d*\D*\W\/)";
        string patternDishCost = @"\d+(?=.руб)";
        string patternDishWeight = @"\d+(?=.гр)";
                                     
        var dishName = Regex.Match(inputText, patternDishName).ToString();
        var dishCost = int.Parse(Regex.Match(inputText, patternDishCost).ToString());
        var dishWeight = int.Parse(Regex.Match(inputText, patternDishWeight).ToString());

        
        // TODO: создаем объект только чтобы сложить в БД. МБ проще напрямую инсертнуть?
        var createdDish = new Dish(inputText, dishName, dishCost, dishWeight);
        
        
        using (MySqlStorage db = new MySqlStorage())
        {
            db.dish_catalog.Add(createdDish);
            db.SaveChanges();
        }
        return createdDish.Id;
    }

    public static string AddDishToOrder(int userId, int dishId)
    {
        var createdOrder = new OrderedDish(userId, dishId);
        
        using (MySqlStorage db = new MySqlStorage())
        {
            db.ordered_dishes.Add(createdOrder);
            db.SaveChanges();
            var shortNameDish = db.dish_catalog.Where(p => p.Id == dishId).Select(p => new
            {
                shortName = p.ShortNameDish
            });
            
            string answer = $"{shortNameDish.First().shortName} добавлен в заказ";
            return answer;
        }

    }

}