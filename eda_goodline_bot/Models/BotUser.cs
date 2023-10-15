using System.ComponentModel.DataAnnotations.Schema;

namespace eda_goodline_bot;

public class BotUser
{
    public int Id { get; set; }
    
    [Column("nick_name_tg")]
    public string? NickNameTg { get; set; }
    
    [Column("user_id_tg")]
    public int UserIdTg { get; set; }
    
    [Column("chat_id_tg")]
    public int ChatIdTg { get; set; }
    public void LoadTelegramInfo(string nickNameTg, int userId, int chatId)
    {
        NickNameTg = nickNameTg;
        UserIdTg = userId;
        ChatIdTg = chatId;
    }
}