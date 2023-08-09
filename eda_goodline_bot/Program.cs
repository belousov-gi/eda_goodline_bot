using System.Security.Cryptography.X509Certificates;
using Telegram.Bot.Types.ReplyMarkups;
using eda_goodline_bot.Iterfaces;
using System.Text.Json;

namespace eda_goodline_bot
{
    
    public class Program
    {
        public static void Main()
        {
            string fileName = "scenario.json";
            ISocialNetworkAdapter socialNetworkAdapter = new TelegramAdapter("6075918005:AAHBOlQc-y0PLOHhI4ZZV2LWb_FrEcYaSQ0", fileName);

            // var act1 = new Action("a1", "ACTION 1", "step");
            // List<Action> listActs = new List<Action>();
            // listActs.Add(act1);
            // var step1 = new Step("/start", "start menu", listActs);
            //
            //
            // var act2 = new Action("a2","ACTION 2", "step" );
            // var act3 = new Action("a3", "ACTION 3", "/start");
            // List<Action> listActs2 = new List<Action>();
            // listActs2.Add(act2);
            // listActs2.Add(act3);
            //
            // var step2 = new Step("step", "second menu", listActs2);
            //
            //
            // List<Step> steps = new List<Step>();
            // steps.Add(step1);
            // steps.Add(step2);
            //
            //
            // Scenario sc = new Scenario("purchase", steps);

            // var scJson = JsonSerializer.Serialize(sc);
            //
            // ISocialNetworkAdapter socialNetworkAdapter = new TelegramAdapter("6075918005:AAHBOlQc-y0PLOHhI4ZZV2LWb_FrEcYaSQ0", sc);
            
            socialNetworkAdapter.Start();





            // IStorage storageAdapter = new MySqlStorageConnector(); 
            // storageAdapter.SaveOrder();
        }
        //Отдельный скрипт формирует общий заказ и отправляет в определенное время (через крон отдельынй скрипт, котоырй заберет данные из БД?)
    }
}

