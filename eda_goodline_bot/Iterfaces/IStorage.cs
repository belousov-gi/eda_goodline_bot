using System.Text.Json.Nodes;

namespace eda_goodline_bot.Iterfaces;

public interface IStorage
{
    public void SaveOrder();
    public List<JsonArray> GetOrders(DateOnly date);
    
}