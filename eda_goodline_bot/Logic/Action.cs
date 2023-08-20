using System.Text.Json.Serialization;

namespace eda_goodline_bot;

public class Action
{
    public string ActionId { get; set; }
    public string? NavigateToStep { get;  init;}
    public string? ActionAnswer { get;  init;}
    
    //кнопка списком, потому что у нас в 1 строке всегда будет только 1 кнопка. 
    public List<KeyboardButton> button { get; set; }
    public RunActionLogic? ActionLogic { get; set; }
    
    [JsonConstructor] 
    public Action(string actionId, string? actionAnswer, string navigateToStep) : this(actionId)
    {
        ActionId = actionId;
        KeyboardButton button = new KeyboardButton(actionId);
        this.button = new List<KeyboardButton>(1);
        this.button.Add(button);
        
        NavigateToStep = navigateToStep;
        ActionAnswer = actionAnswer;
    }
  
    //TODO: наличие этого конструктора крашит дисерриализацю. Подумать как исправить
    public Action(string actionId)
    {
        ActionId = actionId;
        KeyboardButton button = new KeyboardButton(actionId);
        this.button = new List<KeyboardButton>(1);
        this.button.Add(button);
    }
    
    
    public delegate void RunActionLogic(Session session);
    
    // public Action(string actionId, string? actionAnswer)
    // {
    //     ActionId = actionId;
    //     KeyboardButton button = new KeyboardButton(actionId);
    //     this.button = new List<KeyboardButton>(1);
    //     this.button.Add(button);
    //
    //     NavigateToStep = null;
    //     ActionAnswer = actionAnswer;
    // }
    
    // public Action(string actionId)
    // {
    //     ActionId = actionId;
    //     KeyboardButton button = new KeyboardButton(actionId);
    //     this.button = new List<KeyboardButton>(1);
    //     this.button.Add(button);
    //
    //     NavigateToStep = null;
    //     ActionAnswer = null;
    // }

}

