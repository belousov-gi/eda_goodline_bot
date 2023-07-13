using eda_goodline_bot.Iterfaces;
using System.Net.Http;


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


        //TODO: offset хранить в файле где-то? + Прием сообщений куда-то в отдельный хендлер надо вынести. 
        while (swtch)
        {
            var serverAddress = $"https://api.telegram.org/bot{token}/getUpdates?offset=806157094&timeout=60";
            using var request = new HttpRequestMessage(HttpMethod.Get, serverAddress);
            
            using var response = telegramClient.SendAsync(request);
            responseText = response.Result.Content.ReadAsStringAsync().Result;
            Console.WriteLine(responseText);

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