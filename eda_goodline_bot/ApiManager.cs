using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using eda_goodline_bot.Models;

namespace eda_goodline_bot;

static public class ApiManager
{
    public async static void StartApiManager()
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

                    Regex regex = new Regex(pattern);
                    var method = Regex.Match(data.ToString(), pattern).Groups[1].ToString();
                    var additionalDataJson = Regex.Match(data.ToString(), pattern).Groups[2].ToString().Trim();
                    var additionalData = JsonSerializer.Deserialize<AdditionalDataApiModel>(additionalDataJson);
                   
                    //routing 
                    // switch (method)
                    // { 
                    //     case "sendMessage":
                    //         using (MySqlStorage db = new MySqlStorage())
                    //         {
                    //             
                    //         }
                    //         //TODO: сделать подключение к БД к списку администраторов + прокинуть сюда объект ТГ + дернуть отправку
                    //         
                    // }

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