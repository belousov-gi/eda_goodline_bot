namespace eda_goodline_bot;

public static class OrderManager
{
    public static List<Order> Orders = new List<Order>();

    public static string ShowOrder(string customerId)
    {
        var order = Orders.Find(order => order.CustomerId == customerId);
        string answer = string.Join("\n", order.Dishes);
        return answer;
    }
}