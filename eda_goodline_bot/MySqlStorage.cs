using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;


public class MySqlStorage : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public MySqlStorage()
    {
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql("server=localhost;user=root;password=123456789;database=usersdb;", 
            new MySqlServerVersion(new Version(8, 0, 25)));
    }
}

