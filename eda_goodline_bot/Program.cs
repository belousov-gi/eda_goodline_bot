using eda_goodline_bot.Iterfaces;
using eda_goodline_bot.Scenarios;

namespace eda_goodline_bot
{
    
    public class Program
    {

        public static void Main()
        {
            // ApplicationConfig.LoadConfigFile(@"C:\Users\frega\RiderProjects\eda_goodline_bot\eda_goodline_bot\appsettings.json");
            //
            // string filePath = "scenario.json";
            // string token =  ApplicationConfig.TokenBotTg ?? throw new Exception("Empty TG token");
            //
            // var scenario = ScenarioBuilder.CreateScenarioFromJson<OrderFood>(filePath);
            //
            // ISocialNetworkAdapter socialNetworkAdapter = new TelegramAdapter(token, scenario);
            //
            // socialNetworkAdapter.OnMessages += MessageHandler.HandleMessage;
            // socialNetworkAdapter.Start();
            
            ApiManager.StartApiManager();

        }

    }
}

