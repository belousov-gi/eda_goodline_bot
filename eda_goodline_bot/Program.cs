using eda_goodline_bot.Iterfaces;

namespace eda_goodline_bot
{
    internal class Program
    {
        public static void Main()
        {
            ISocialNetworkAdapter socialNetworkAdapter = new TelegramConnection();
            socialNetworkAdapter.ChooseDish();

            //создается заказ, складывается в БД

            //Формируется общий заказ и отправл в определенное время (через крон отдельынй скрипт, котоырй заберет данные из БД?)
        }
    }
}