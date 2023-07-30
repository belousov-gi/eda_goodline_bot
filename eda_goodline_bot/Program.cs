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
            // string fileName = "scenario.json";
            // ISocialNetworkAdapter socialNetworkAdapter = new TelegramAdapter("6075918005:AAHBOlQc-y0PLOHhI4ZZV2LWb_FrEcYaSQ0", fileName);
            
            
            
            //TEST SERIALIZATION

            var action = new Action("boom");
            List<Action> actionList = new List<Action>(1);
            actionList.Add(action);
            var step = new Step("start", "стартовое меню", actionList, "start");
            List<Step> stepsList = new List<Step>(1);
            stepsList.Add(step);
            
            Scenario? scenario = new Scenario("default", stepsList);
            string jsonString = JsonSerializer.Serialize(scenario);
            
            Scenario? ss = JsonSerializer.Deserialize<Scenario>(jsonString);
        }
        
        class Person
        {
            public string Name { get;}
            public int age { get; set; }
            public Person(string name, int age)
            {
                Name = name;
                this.age = age;
            }

            // socialNetworkAdapter.Start();




            // IStorage storageAdapter = new MySqlStorageConnector(); 
            // storageAdapter.SaveOrder();



            //Отдельный скрипт формирует общий заказ и отправляет в определенное время (через крон отдельынй скрипт, котоырй заберет данные из БД?)



        }
        
        public class TestClass
        {
            public string Name { get; set; }

            public TestClass(string name)
            {
                Name = name;
            }
        }

        
        
        
        
        
        
        
    }

}