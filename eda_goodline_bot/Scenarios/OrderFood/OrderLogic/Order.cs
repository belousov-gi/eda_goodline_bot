// using System.ComponentModel.DataAnnotations.Schema;
// using System.Runtime.InteropServices.JavaScript;
// using eda_goodline_bot.Enums;
//
// namespace eda_goodline_bot;
//
// public class Order
// {
//     [Column("user_id")]
//     public string CustomerId { get; init; }
//     [Column("dish_id")]
//     public int DishId { get; init; }
//     [Column("order_day")]
//     public DateTime DateOfOrder { get; init; }
//     
//     public Order(string customerId, int dishId)
//     {
//         CustomerId = customerId;
//         DishId = dishId;
//         
//         //TODO: сделать каст часовых поясов, если использовать бота по РФ 
//         DateOfOrder = DateTime.Today;
//     }
// }