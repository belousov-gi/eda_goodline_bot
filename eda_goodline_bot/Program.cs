using eda_goodline_bot.Iterfaces;
using eda_goodline_bot.Scenarios;

namespace eda_goodline_bot
{
    
    public class Program
    {

        public static void Main()
        {
            string filePath = "scenario.json";
            string token =  Environment.GetEnvironmentVariable("tg_bot_token");
            
            var scenario = ScenarioBuilder.CreateScenarioFromJson<OrderFood>(filePath);
            
            ISocialNetworkAdapter socialNetworkAdapter = new TelegramAdapter(token, scenario);

            socialNetworkAdapter.OnMessages += HandleMessage;
            socialNetworkAdapter.Start();
        }
        
        
        //TODO: Доделать интерфейс для сообщения, мб не через свойства, а через методы
         public static async void HandleMessage(ISocialNetworkAdapter socialNetworkAdapter, IReceivedMessage messages)
        {

            //TODO: логирование ошибок навернуть + 
            await Task.Run(() =>
            {
                foreach (var messageInfo in messages.GeneralMessagesStructure)
                {
                    var userId = messageInfo.message.from.id.ToString();
                    var chatId = messageInfo.message.chat.id;
                    var text = messageInfo.message.text;

                    
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

         

         //Отдельный скрипт формирует общий заказ и отправляет в определенное время (через крон отдельынй скрипт, котоырй заберет данные из БД?)
    }
    
}

