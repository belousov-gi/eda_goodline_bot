using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Telegram.Bot.Types.ReplyMarkups;
using eda_goodline_bot.Iterfaces;
using System.Text.Json;
using System.Text.RegularExpressions;
using eda_goodline_bot.Models;
using eda_goodline_bot.Scenarios;

namespace eda_goodline_bot
{
    
    public class Program
    {

        public static void Main()
        {
            string fileName = "scenario.json";
            var scenario = CreateScenarioFromJson<OrderFood>(fileName);

            ISocialNetworkAdapter socialNetworkAdapter = new TelegramAdapter("6075918005:AAHBOlQc-y0PLOHhI4ZZV2LWb_FrEcYaSQ0", scenario);

            socialNetworkAdapter.OnMessages += HandleMessage;

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
        
        
        //TODO: сообщения телеги заменить на общий интерфейс сообщений 
         public static async void HandleMessage(ISocialNetworkAdapter socialNetworkAdapter, TelegramReceivedMessages messages)
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
                    List<Action> answerMenu;

                    var userSession =  SessionManager.SessionsList.Find(session => session.UserId == userId && session.SocialNetworkAdapter == socialNetworkAdapter);
                    
                    if (userSession == null)
                    {
                        userSession = SessionManager.CreateSession(socialNetworkAdapter, userId, chatId, socialNetworkAdapter.LoadedScenario);
                        
                        //по дейфолту берем шаг с названием /start
                        
                        var currentStep =  userSession.CurrentStep;
                        answerText = currentStep.StepDesc;
                        answerMenu = currentStep.Actions;
                        socialNetworkAdapter.SendMessage(chatId, answerText, answerMenu);
                        
                        
                    }
                    else
                    {
                        Step? nextStep;
                        
                        //для системных комманд бота
                        if (text[0] == '/')
                        {
                            nextStep = userSession.CurrentScenario.Steps.Find(step => step.StepId == text);
                            if (nextStep != null)
                            {
                                answerText = nextStep.StepDesc;
                                answerMenu = nextStep.Actions;
                                userSession.CurrentStep = nextStep;
                                socialNetworkAdapter.SendMessage(chatId, answerText, answerMenu);
                            }
                        }
                        else
                        {
                            //команда не системная
                            
                            var currentStep = userSession.CurrentStep;
                            
                            //TODO:здесь крашит динамичческие экшены (блюда котоыре добавлены в заказ тут не отображаются. ПОЧИНИТЬ)
                            var action = currentStep.Actions.Find(action => action.ActionId == text);
                            
                            //нашли такой экшен в текущем шаге, выполняем его
                            if (action != null)
                            {
                                //действия над экшеном для данного шага. 
                                action.ActionLogic?.Invoke(userSession);
                                userSession.CurrentStep.LastAction = action;
                                
                                //смотрим меняется ли шаг после выполнение экшена
                                if (action.NavigateToStep != null)
                                {
                                    nextStep = userSession.CurrentScenario.Steps.Find(step => step.StepId == action.NavigateToStep);
                                    if (nextStep != null)
                                    {
                                        answerText = nextStep.StepDesc;
                                        answerMenu = nextStep.Actions;
                                        userSession.CurrentStep = nextStep;
                                        
                                        //при смене шага очищаем историю последнего выполненного шага для него
                                        userSession.CurrentStep.LastAction = null;
                                        
                                        if (answerText != null)
                                        {
                                            socialNetworkAdapter.SendMessage(chatId, answerText, answerMenu);
                                        }

                                        //TODO:  убрать навигацию в функции

                                    }
                                    else
                                    {
                                        throw new Exception("Не существует такого шага навигации");
                                    }
                                }
                            }
                            
                            
                        }
                        
                        //выполняем логику шага
                        userSession.CurrentStep.StepLogic?.Invoke(userSession);

                    }
                }
            });
        }
         
         //TODO: Заменить generic на фабрику?
         public static T CreateScenarioFromJson<T>(string fileName) where T: Scenario
         {
             Console.WriteLine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
             string jsonString = File.ReadAllText(fileName);
             // LoadedScenario = 
             try
             {
                 T? scenario = JsonSerializer.Deserialize<T>(jsonString);

                 if (scenario == null)
                 {
                     throw new Exception("Scenario is null!");
                 }
                 return scenario;
             }
             catch (Exception e)
             {
                 Console.WriteLine(e);
                 throw;
             }
           
         }

         
        //Отдельный скрипт формирует общий заказ и отправляет в определенное время (через крон отдельынй скрипт, котоырй заберет данные из БД?)
    }
    
}

