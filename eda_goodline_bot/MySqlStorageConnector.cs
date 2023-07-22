using System.Text.Json.Nodes;
using eda_goodline_bot.Iterfaces;

namespace eda_goodline_bot;

public class MySqlStorageConnector : IStorage
{
    public void SaveOrder()
    {
        throw new NotImplementedException();
    }

    public List<JsonArray> GetOrders(DateOnly date)
    {
        throw new NotImplementedException();
    }
}