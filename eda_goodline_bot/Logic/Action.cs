namespace eda_goodline_bot;

public class Action
{
    // public string ActionId { get; set; }
    public List<List<KeyboardButton>> keyboard { get; init; }

    public Action()
    {
        KeyboardButton button = new KeyboardButton("999");
     
        //TODO:LIST - надо задавать сразу его емкость.
        List<KeyboardButton> buttonsList = new List<KeyboardButton>();
        buttonsList.Add(button);
        
        List<List<KeyboardButton>> keyboard = new List<List<KeyboardButton>>();
        keyboard.Add(buttonsList);
        this.keyboard = keyboard;
    }
    
  
    public class KeyboardButton
    {
        public string text { get; init; }

        public KeyboardButton(string nameButon)
        {
            text = nameButon;
        }
    }

}

