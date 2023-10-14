using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using eda_goodline_bot.Iterfaces;
using eda_goodline_bot.Models;
using Microsoft.EntityFrameworkCore;

namespace eda_goodline_bot;

public static class ApiManager
{
    public static async void StartApiManager(ISocialNetworkAdapter socialNetworkAdapter)
    {
        await Task.Run(() =>
        {
            const string ip = "127.0.0.1";
            const int port = 9999;
            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            tcpSocket.Bind(tcpEndPoint);
            tcpSocket.Listen(2);

            while (true)
            {
                Console.WriteLine("Запустили апи");
                var listener = tcpSocket.Accept();
                var buffer = new byte[100];
                var size = 0;
                var data = new StringBuilder();
                
                try
                {
                    do
                    {
                        size = listener.Receive(buffer);
                        data.Append(Encoding.UTF8.GetString(buffer, 0, size));
                    } while (listener.Available > 0);

                    string pattern = @"(\w+):([\S\s]+)";
                    
                    var method = Regex.Match(data.ToString(), pattern).Groups[1].ToString();
                    if (method == "") {throw new Exception("Invalid inpud data");}

                    var additionalDataJson = Regex.Match(data.ToString(), pattern).Groups[2].ToString().Trim();
                    var additionalData = JsonSerializer.Deserialize<AdditionalDataApiModel>(additionalDataJson);
                   
                    //routing 
                    switch (method)
                    {
                        case "createOrdersInfoForAdmin":
                        {
                            var orderInfo = CreateOrdersInfoForAdmin();
                            SendMessageToAdministrators(orderInfo, socialNetworkAdapter);
                            break;
                        }
                    }
                    
                    listener.Shutdown(SocketShutdown.Both);
                    listener.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    var exceptionInfo = Encoding.UTF8.GetBytes(e.Message);
                    listener.Send(exceptionInfo);
                    listener.Shutdown(SocketShutdown.Both);
                    listener.Close();
                }
            }
        });

    }

    private static AdditionalDataApiModel CreateOrdersInfoForAdmin()
    {
        string ordersInfo = "";
        int priceForOrder = 0;
        
        using (MySqlStorage db = new MySqlStorage())
        {

            var todaysCustomersId =
                db.ordered_dishes.Where(dish => dish.DateOfOrder == DateTime.Today)
                                .Join(db.users.Select(user => new
                                                {
                                                    userNick = user.NickNameTg,
                                                    userId = user.UserIdTg
                                                }).Distinct(),
                        dish => dish.CustomerId,
                        user => user.userId,
                                (dish, user) => new
                                {
                                    userId = dish.CustomerId,
                                    userNickname = user.userNick
                                }).Distinct().ToList();
                    
            
            foreach (var customer in todaysCustomersId)
            {
                var orderedDishes = db.ordered_dishes
                    .Where(dish => dish.CustomerId == customer.userId)
                    .Join(db.dish_catalog,
                        dish => dish.DishId,
                        dishCatalog => dishCatalog.Id,
                        (dish, dishCatalog) => new
                        {
                            dishId = dish.DishId,
                            dishName = dishCatalog.GeneralDishName,
                            dishPrice = dishCatalog.PriceDish
                        }).ToList();
                                                                  

                ordersInfo += "@" + customer.userNickname + "\n";
                
                foreach (var dish in orderedDishes)
                {
                    ordersInfo += dish.dishName + "\n";
                    priceForOrder += dish.dishPrice;
                }
                ordersInfo += $"Итого: {priceForOrder} руб" + "\n" + "------";
            }
            AdditionalDataApiModel info = new AdditionalDataApiModel();
            info.Text = ordersInfo;
            return info;
        } 
    }

    private static void SendMessageToAdministrators(AdditionalDataApiModel additionalData, ISocialNetworkAdapter socialNetworkAdapter)
    {
        using (MySqlStorage db = new MySqlStorage())
        {
            var administrators = db.administrators.ToList();

            if (additionalData?.Text != null)
            {
                foreach (var admin in administrators)
                {
                    var chatIdAdmin = db.users.Where(user => user.NickNameTg == admin.NickNameTg)
                        .Select(user => user.ChatIdTg).First();
                                        
                    if (!Equals(admin.ChatIdTg, chatIdAdmin))
                    {
                        db.administrators.Where(adm => adm.NickNameTg == admin.NickNameTg)
                            .ExecuteUpdate(s =>
                                s.SetProperty(adm => adm.ChatIdTg, x => chatIdAdmin));

                    }
                    socialNetworkAdapter.SendMessage(admin.ChatIdTg, additionalData.Text);
                }
            }
        }
    }
    
}