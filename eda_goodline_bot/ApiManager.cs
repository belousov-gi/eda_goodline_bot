using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using eda_goodline_bot.Iterfaces;
using eda_goodline_bot.Models;
using Microsoft.EntityFrameworkCore;

namespace eda_goodline_bot;

static public class ApiManager
{
    public async static void StartApiManager(ISocialNetworkAdapter socialNetworkAdapter)
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
                    var additionalDataJson = Regex.Match(data.ToString(), pattern).Groups[2].ToString().Trim();
                    var additionalData = JsonSerializer.Deserialize<AdditionalDataApiModel>(additionalDataJson);
                   
                    //routing 
                    switch (method)
                    { 
                        case "sendMessageToAdministrators":
                            using (MySqlStorage db = new MySqlStorage())
                            {
                                var administrators = db.administrators.ToList();
                                
                                if (additionalData?.Text != null)
                                {
                                    foreach (var admin in administrators)
                                    {
                                        var chatIdAdmin = db.users.Where(user => user.NickNameTg == admin.NickNameTg).Select(user => user.ChatIdTg).First();
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

                            break;
                    }

                    listener.Shutdown(SocketShutdown.Both);
                    listener.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                
            }
        });

    } 
    
}