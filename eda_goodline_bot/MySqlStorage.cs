using eda_goodline_bot;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;


public class MySqlStorage : DbContext
{
    public DbSet<Dish> dish_catalog { get; set; } = null!;
    public DbSet<OrderedDish> ordered_dishes { get; set; } = null!;
    
    public DbSet<BotUser> users { get; set; } = null!;
    public DbSet<BotAdministrator> administrators { get; set; } = null!;
    
    public MySqlStorage()
    {
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(ApplicationConfig.DbStringConnection, 
            new MySqlServerVersion(new Version(8, 0, 32)));
    }
}

