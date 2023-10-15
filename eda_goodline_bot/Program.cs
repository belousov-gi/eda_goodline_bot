using eda_goodline_bot.Iterfaces;
using eda_goodline_bot.Scenarios;

namespace eda_goodline_bot
{
    public class Program
    {
        public static void Main()
        {
            ApplicationConfig.LoadConfigFile(@"appsettings.json");
            
            string filePath = "scenario.json";
            string token =  ApplicationConfig.TokenBotTg ?? throw new Exception("Empty TG token");
            
            var scenario = ScenarioBuilder.CreateScenarioFromJson<OrderFood>(filePath);
            
            ISocialNetworkAdapter socialNetworkAdapter = new TelegramAdapter(token, scenario);
            ApiManager.StartApiManager(socialNetworkAdapter);
            
            socialNetworkAdapter.OnMessages += MessageHandler.HandleMessage;
            socialNetworkAdapter.Start();

        }

    }
}

