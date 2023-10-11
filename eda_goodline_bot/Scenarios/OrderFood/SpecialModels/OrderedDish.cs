using System.ComponentModel.DataAnnotations.Schema;

namespace eda_goodline_bot;

public class OrderedDish
{
    public int Id { get; init; }
    
    [Column("user_id")]
    public int CustomerId { get; init; }
    [Column("dish_id")]
    public int DishId { get; init; }

    [Column("order_day")]
    public DateTime DateOfOrder { get; init; }
    
    public OrderedDish(int customerId, int dishId)
    {
        CustomerId = customerId;
        DishId = dishId;
        
        //TODO: сделать каст часовых поясов, если использовать бота по РФ 
        DateOfOrder = DateTime.Today;
    }
}