using eda_goodline_bot.Enums;
using eda_goodline_bot.Iterfaces;
using eda_goodline_bot.Scenarios;

namespace eda_goodline_bot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int amountArgs = args.Length;
            Dictionary<string, string> inputArgs = new Dictionary<string, string>();

            for (int i = 0; i < amountArgs; i+=2)
            {
                var inputArg = args[i].Remove(0, 1);
                if (args[i].StartsWith('-') && Enum.IsDefined(typeof(AvailableInputArgs), inputArg))
                {
                    inputArgs.Add(inputArg, args[i+1]);
                }
                else
                {
                    throw new Exception("Invalid input command");
                }
            }
            
            ApplicationConfig.LoadConfigFile(@"appsettings.json");
            
            string filePath = "scenario.json";
            string token =  ApplicationConfig.TokenBotTg ?? throw new Exception("Empty TG token");
            
            var scenario = ScenarioBuilder.CreateScenarioFromJson<OrderFood>(filePath);
            
            ISocialNetworkAdapter socialNetworkAdapter = new TelegramAdapter(token, scenario);

            string? host;
            inputArgs.TryGetValue(AvailableInputArgs.h.ToString(), out host);
            ApiManager.StartApiManager(socialNetworkAdapter, host);

            socialNetworkAdapter.OnMessages += MessageHandler.HandleMessage;
            socialNetworkAdapter.Start();

        }

    }
}

