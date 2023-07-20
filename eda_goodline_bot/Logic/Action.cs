namespace eda_goodline_bot;

public class Action
{
    // public string ActionId { get; set; }

    public Action()
    {
        KeyboardButton button = new KeyboardButton("dddd");
        
        List<KeyboardButton> buttonsList = new List<KeyboardButton>();
        buttonsList.Add(button);
        
        ReplyKeyboardMarkup replKeyboard = new ReplyKeyboardMarkup(buttonsList);
      
        
    }
    
    public class ReplyKeyboardMarkup
    {
        public List<List<KeyboardButton>> keyboard { get; init; }

        public ReplyKeyboardMarkup(List<KeyboardButton> btnList)
        {
            List<List<KeyboardButton>> keyboard = new List<List<KeyboardButton>>();
            keyboard.Add(btnList);
            this.keyboard = keyboard;
        }
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

