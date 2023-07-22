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



    public void Start()
    {
        OnMessages += MessageHandler;

        //как часто делаем long pull
        int timeout = 60;
        int updateId = 0;
        
        //TODO: вынести в какой-то пакет констант + таймаут для лонг пулинга
        string serverAddress = $"https://api.telegram.org/bot{token}/getUpdates?offset={0}";
        
        //смотрим на сообщения, которые пришли до включения бота, чтобы определить с какого updateId начать
        TelegramReceivedMessages? messages = CheckNewMessages(serverAddress);
        
        Console.WriteLine($"смотрим кол-во сообщений {messages.result.Length}");
        if (messages.result.Length != 0)
        {
            updateId = messages.result[messages.result.Length - 1].update_id;  
        }
        

        //TODO: offset хранить в файле где-то? + Прием сообщений куда-то в отдельный хендлер надо вынести + использвать allowed_updates (см доку) . 
        //тут логика ожидания сообщений
        while (true)
        {
            Console.WriteLine($"Ищем новые сообщения, update id {updateId}");
            ++updateId;
            serverAddress = $"https://api.telegram.org/bot{token}/getUpdates?offset={updateId}&timeout={timeout}";
            messages = CheckNewMessages(serverAddress);
            
            //вызов эвента, что пришло сообщение
            foreach (var messageInfo in messages.result)
            {
                var text = messageInfo.message.text;
                var idChat = messageInfo.message.chat.id;
                updateId = messageInfo.update_id;
                OnMessages?.Invoke(idChat, text);
                Task.WaitAll();
            }
        }
    }

    private TelegramReceivedMessages CheckNewMessages(string serverAddress)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, serverAddress);
        using var response = telegramClient.Send(request);
        string? responseText = response.Content.ReadAsStringAsync().Result;
        var messages = JsonSerializer.Deserialize<TelegramReceivedMessages>(responseText);
        
        return messages;
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
    
    public async void MessageHandler(int chatId, string text)
    {
        //TODO: логирование ошибок навернуть
        var answer = new Action();
        string jsonString = JsonSerializer.Serialize(answer);
        string serverAddress = $"https://api.telegram.org/bot{token}/sendMessage?chat_id={chatId}&reply_markup={jsonString}&text={"Выбирай"}";
        using var request = new HttpRequestMessage(HttpMethod.Get, serverAddress);
        using var response = await telegramClient.SendAsync(request);
        //TODO: логирование ошибок навернуть
    }
 

    public event ISocialNetworkAdapter.OnMessage? OnMessages;
}
