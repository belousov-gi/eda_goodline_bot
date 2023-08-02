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
           
            // IStorage storageAdapter = new MySqlStorageConnector(); 
            // storageAdapter.SaveOrder();
        }
        //Отдельный скрипт формирует общий заказ и отправляет в определенное время (через крон отдельынй скрипт, котоырй заберет данные из БД?)
    }
}

