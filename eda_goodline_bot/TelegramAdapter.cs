using eda_goodline_bot.Iterfaces;
using System.Net.Http;
using Telegram.Bot.Types.ReplyMarkups;
using System.Text.Json;
using System.Text.Json.Nodes;
using eda_goodline_bot.Models;


namespace eda_goodline_bot;

public class TelegramAdapter : ISocialNetworkAdapter
{
    private HttpClient _httpClient;

    private HttpClient telegramClient
    {
        get => _httpClient;
        set
        {
            _httpClient = value;
            _httpClient.Timeout = TimeSpan.FromSeconds(60);
        }
    } 

    // TODO: УДАЛИТь и из конструктора тоже!!
    private readonly string token;
    

    public string ChooseDish()
    {
        bool swtch = true;
        string responseText = null;
        TelegramReceivedMessage receivedMessage;
        int updateId = 806157099; // сделать механизм обнуления при запуске


        //TODO: offset хранить в файле где-то? + Прием сообщений куда-то в отдельный хендлер надо вынести + использвать allowed_updates (см доку) . 
        //тут логика запроса сообщений
        while (swtch)
        {
            var serverAddress = $"https://api.telegram.org/bot{token}/getUpdates?offset={updateId}&timeout=60";
            using var request = new HttpRequestMessage(HttpMethod.Get, serverAddress);
            
            using var response = telegramClient.SendAsync(request);
            responseText = response.Result.Content.ReadAsStringAsync().Result;
        
            receivedMessage = JsonSerializer.Deserialize<TelegramReceivedMessage>(responseText);
            Console.WriteLine(receivedMessage.result[0].message.text);
            Console.WriteLine(receivedMessage.result[0].update_id);
            updateId = receivedMessage.result[0].update_id;
            ++updateId;



            // var replyMarkup = new ReplyKeyboardMarkup(new[]
            // {
            //     new[]
            //     {
            //         new KeyboardButton("Кнопка 1"),
            //         new KeyboardButton("Кнопка 2"),
            //     },
            //     new[]
            //     {
            //         new KeyboardButton("Кнопка 3"),
            //         new KeyboardButton("Кнопка 4"),
            //     }
            // });
            //
            // telegramClient.SendAsync();

        }


        return responseText;

    }

    public void SendGeneralOrder()
    {
        throw new NotImplementedException();
    }

    public TelegramAdapter(string token)
    {
        telegramClient = new HttpClient();
        
        this.token = token;
    }

    public void TestRequest()
    {
        var content = _httpClient.GetStringAsync($"https://api.telegram.org/bot{token}/getMe");
        Console.WriteLine(content.Result);
    }
    
    
    
}
