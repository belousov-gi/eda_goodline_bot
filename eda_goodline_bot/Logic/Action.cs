using System.Text.Json.Serialization;

namespace eda_goodline_bot;

public class Action
{
    public string ActionId { get; set; }
    public string NavigateToStep { get;  init;}
    public string? ActionAnswer { get;  init;}
    
    //кнопка списком, потому что у нас в 1 строке всегда будет только 1 кнопка. 
    public List<KeyboardButton> button { get; set; }

    public Action(string actionId, string navigateTo, string? actionAnswer)
    {
        ActionId = actionId;
        KeyboardButton button = new KeyboardButton(actionId);
        this.button = new List<KeyboardButton>(1);
        this.button.Add(button);

        NavigateToStep = navigateTo;
        ActionAnswer = actionAnswer;
    }

}

