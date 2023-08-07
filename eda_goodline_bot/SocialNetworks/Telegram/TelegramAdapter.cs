using System.Net;
using eda_goodline_bot.Iterfaces;
using System.Net.Http;
using System.Reflection;
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
    public string ServerAddress { get; set; }
    public Scenario LoadedScenario { get; init; }
    
    public delegate void OnMessage(TelegramReceivedMessages messages);
    public event OnMessage OnMessages;
    

    public TelegramAdapter(string token, string fileName)
    {
        telegramClient = new HttpClient();
        this.token = token;
        ServerAddress = $"https://api.telegram.org/bot{token}";
        LoadedScenario = CreateScenarioFromJson(fileName);

    }
    
    public TelegramAdapter(string token, Scenario sc)
    {
        telegramClient = new HttpClient();
        this.token = token;
        ServerAddress = $"https://api.telegram.org/bot{token}";
        LoadedScenario = sc;

    }


    public Scenario CreateScenarioFromJson(string fileName)
    {
        Console.WriteLine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        string jsonString = File.ReadAllText(fileName);
        // LoadedScenario = 
        try
        {
            Scenario scenario = JsonSerializer.Deserialize<Scenario>(jsonString);
            return scenario;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
           
    }


    public void Start()
    {
        
        OnMessages += HandleMessage;
        // OnMessages += MessageHandler.HandleMessage;

        //TODO: вынести в какой-то пакет констант + таймаут для лонг пулинга

        //смотрим на сообщения, которые пришли до включения бота, чтобы определить с какого updateId начать
        var updateId = FindLastUpdateId(ServerAddress);
        
        //для отладки
        // var updateId = 0;

        ReceiveNewMessages(ServerAddress, updateId);
        
        //TODO: Прием сообщений куда-то в отдельный хендлер надо вынести + использвать allowed_updates (см доку) . 

        
    }

    private void ReceiveNewMessages(string serverAddress, int updateId)
    {
        int resultLenght;
        TelegramReceivedMessages messages;
        
        while (true)
        {
            Console.WriteLine($"Ищем новые сообщения, update id {updateId} тред {Thread.GetCurrentProcessorId()}");

            messages = CheckNewMessages(serverAddress, updateId);
            resultLenght = messages.result.Length;
            
            if (resultLenght != 0)
            {
                Console.WriteLine($"Номер треда при триггере события {Thread.GetCurrentProcessorId()}");
                OnMessages?.Invoke(messages);
                updateId = messages.result[resultLenght - 1].update_id;  
            }
            
            ++updateId;
        }
    }


    private TelegramReceivedMessages CheckNewMessages(string serverAddress, int updateId)
    {
        const int timeout = 60;
        string requestStr = serverAddress + $"/getUpdates?timeout={timeout}&offset={updateId}";
        using var request = new HttpRequestMessage(HttpMethod.Get, requestStr);
        using var response = telegramClient.Send(request);
        string responseText = response.Content.ReadAsStringAsync().Result;
        var messages = JsonSerializer.Deserialize<TelegramReceivedMessages>(responseText);
        return messages;
    }
    
    private int FindLastUpdateId(string serverAddress)
    {
        int updateId = 0;
        TelegramReceivedMessages? messages = CheckNewMessages(serverAddress, updateId);
        
        Console.WriteLine($"смотрим кол-во пропущенных сообщений {messages.result.Length}");
        int resultLenght = messages.result.Length;
        
        if (resultLenght != 0)
        {
            updateId = messages.result[resultLenght - 1].update_id;  
        }
        return updateId;
    }

    private void SendMessageToTgApi(string requestStr)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, requestStr);

        // Console.WriteLine($"Номер треда внутри хендлера ДО отправки запроса {Thread.GetCurrentProcessorId()} текст {text}");

        using var response = telegramClient.Send(request);
                
        if (response.StatusCode != HttpStatusCode.OK)
        {
            //TODO:залогировать ошибку отправки
        }
    }

    public void SendGeneralOrder()
    {
        throw new NotImplementedException();
    }

    public void SendMessage(int chatId, string answerText, List<Action> actionsList)
    {
        string answerMenu = "";
        string answerMenuJSON = "";

        
        foreach (var action in actionsList)
        {
            answerMenu = JsonSerializer.Serialize(action.button) + ",";
            answerMenuJSON += answerMenu;
        }
        // string answerMenuJSON = JsonSerializer.Serialize(answerMenu);
        answerMenuJSON = answerMenuJSON.Remove(answerMenuJSON.Length - 1);
        string jsonAnswer = "{\"keyboard\":[" + answerMenuJSON+ "]}";

        var requestStr =
            ServerAddress + $"/sendMessage?chat_id={chatId}&text={$"{answerText}&reply_markup={jsonAnswer}"}";
        SendMessageToTgApi(requestStr);
    }
    
    public void SendMessage(int chatId, string answerText)
    {
        var requestStr = ServerAddress + $"/sendMessage?chat_id={chatId}&text={$"{answerText}"}";
        SendMessageToTgApi(requestStr);
    }



    public async void HandleMessage(TelegramReceivedMessages messages)
    {

        //TODO: логирование ошибок навернуть + 
        await Task.Run(() =>
        {
            foreach (var messageInfo in messages.result)
            {
                var userId = messageInfo.message.from.id.ToString();
                var chatId = messageInfo.message.chat.id;
                var text = messageInfo.message.text;
                // var messageType
                
                string? answerText = null;
                List<Action>? answerMenu = null;

                var userSession =  SessionManager.SessionsList.Find(session => session.UserId == userId);

                if (userSession == null)
                {
                    userSession = SessionManager.CreateSession(userId, LoadedScenario);
                    
                    //по дейфолту берем первый шаг из списка
                    
                    var currentStep =  userSession.CurrentStep;
                    answerText = currentStep.StepDesc;
                    answerMenu = currentStep.Actions;
                    SendMessage(chatId, answerText, answerMenu);
                }
                else
                {
                    Step? nextStep;
                    
                    //для системных комманд бота
                    if (text[0] == '/')
                    {
                        nextStep = userSession.CurrentScenario.Steps.Find(step => step.StepId == text);
                        answerText = nextStep.StepDesc;
                        answerMenu = nextStep.Actions;
                        userSession.CurrentStep = nextStep;
                    }
                    else
                    {
                        var currentStep = userSession.CurrentStep;
                        var action = currentStep.Actions.Find(action => action.ActionId == text);
                        string? answerAction = action.ActionAnswer;

                        if (action.NavigateToStep != currentStep.StepId)
                        {
                            nextStep = userSession.CurrentScenario.Steps.Find(step => step.StepId == action.NavigateToStep);
                            if (nextStep != null)
                            {
                                answerText = nextStep.StepDesc;
                                answerMenu = nextStep.Actions;
                                userSession.CurrentStep = nextStep;
                            }
                            else
                            {
                                answerText = "Не существует шага навигации";
                            }
                        } 
                        
                        if (answerAction != null) { SendMessage(chatId, answerAction); }
                    }
                    
                    if (answerText != null)
                    {
                        SendMessage(chatId, answerText, answerMenu);
                    }
                    
                }
            }
        });
    }
    
    // public static void ActivateStep(Session userSession,string inputText, out string answerText, out string answerMenu)
    // {
    //     //только при заходе на шаг
    //     answerText = CurrentStep.StepDesc;
    //     answerMenu = CurrentStep.Actions.ToString();
    //       
    //     
    //     //TODO:спорное решение что это должно быть здесь, возможно стоит вынести
    //     //в отправку (ведь сообщение может и не отправиться из-за сбоя)
    //
    //     CurrentStep = CurrentScenario.Steps.Find();
    //     // answerText = CurrentStep.Actions.Find(action => action.ActionId == inputText)
    // }
    
    

}
