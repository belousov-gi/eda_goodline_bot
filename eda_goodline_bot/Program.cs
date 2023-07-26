using System.Security.Cryptography.X509Certificates;
using Telegram.Bot.Types.ReplyMarkups;
using eda_goodline_bot.Iterfaces;

namespace eda_goodline_bot
{
    
    public class Program
    {
        public static void Main()
        {
            ISocialNetworkAdapter socialNetworkAdapter = new TelegramAdapter("6075918005:AAHBOlQc-y0PLOHhI4ZZV2LWb_FrEcYaSQ0");

            
            
            // socialNetworkAdapter.Start();

            OnMessages += MessageHandler;


            // IStorage storageAdapter = new MySqlStorageConnector();
            // storageAdapter.SaveOrder();



            //Отдельный скрипт формирует общий заказ и отправляет в определенное время (через крон отдельынй скрипт, котоырй заберет данные из БД?)



        }
        
        
        public async void MessageHandler(TelegramReceivedMessages messages)
        {

            //TODO: логирование ошибок навернуть + 
            await Task.Run(() =>
            {
                foreach (var messageInfo in messages.result)
                {
                    var userId = messageInfo.message.from.id.ToString();
                    var chatId = messageInfo.message.chat.id;
                    var text = messageInfo.message.text;

                    var userSession =  SessionManager.SessionsList.Find(session => session.UserId == userId);

                    if (userSession == null)
                    {
                        userSession = SessionManager.CreateSession(userId, LoadedScenario);
                        userSession.CurrentScenario.S
                    }



                    //TODO:убрать в отдельную функцию отправки
                    // 
                    string jsonString = null;
                    var requestStr =
                        ServerAddress + $"/sendMessage?chat_id={chatId}&text={$"{text}&reply_markup={jsonString}"}";
                    using var request = new HttpRequestMessage(HttpMethod.Get, requestStr);
                
                    // Console.WriteLine($"Номер треда внутри хендлера ДО отправки запроса {Thread.GetCurrentProcessorId()} текст {text}");

                    using var response = telegramClient.Send(request);
                
                    if ((int)response.StatusCode != 200)
                    {
                        //TODO:залогировать ошибку отправки
                    }

                    // Console.WriteLine(
                    //     $"Номер треда внутри хендлера ПОСЛЕ отправки запроса {Thread.GetCurrentProcessorId()} текст {text}");
                }
            });

        }
        
        
        
    }

}