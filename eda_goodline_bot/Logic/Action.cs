using System.Text.Json.Serialization;

namespace eda_goodline_bot;

public class Action
{
    public string ActionId { get; set; }
    
    //кнопка списком, потому что у нас в 1 строке всегда будет только 1 кнопка. 
    public List<KeyboardButton> button { get; set; }

    public Action(string actionId)
    {
        ActionId = actionId;
        KeyboardButton button = new KeyboardButton(actionId);

        this.button = new List<KeyboardButton>(1);
        //TODO:LIST - надо задавать сразу его емкость.

        this.button.Add(button);
    }

}

