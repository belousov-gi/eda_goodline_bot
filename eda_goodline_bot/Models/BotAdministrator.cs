using System.ComponentModel.DataAnnotations.Schema;

namespace eda_goodline_bot;

public class BotAdministrator
{
    public int Id { get; set; }
    
    [Column("name")]
    public string? Name { get; set; }
    
    [Column("telegram_account")]
    public string? NickNameTg { get; set; }
    
    [Column("chat_id_tg")]
    public int ChatIdTg { get; set; }
    
}