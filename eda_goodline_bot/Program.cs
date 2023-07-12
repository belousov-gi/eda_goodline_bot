using eda_goodline_bot.Iterfaces;

namespace eda_goodline_bot
{
    internal class Program
    {
        public static void Main()
        {
            ISocialNetworkAdapter socialNetworkAdapter = new TelegramConnection();
            socialNetworkAdapter.ChooseDish();

            IStorage storageAdapter = new MySqlStorageConnector();
            storageAdapter.SaveOrder();
            
            
            //Отдельный скрипт формирует общий заказ и отправляет в определенное время (через крон отдельынй скрипт, котоырй заберет данные из БД?)
        }
    }
}