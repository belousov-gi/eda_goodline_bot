using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace eda_goodline_bot;

static public class ApiManager
{
    public static void StartApiManager()
    {
        const string ip = "127.0.0.1";
        const int port = 9999;
        var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        
        var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );
        
        tcpSocket.Bind(tcpEndPoint);
        tcpSocket.Listen(2);

        while (true)
        {
            var listener = tcpSocket.Accept();
            var buffer = new byte[100];
            var size = 0;
            var data = new StringBuilder();

            do
            {
                size = listener.Receive(buffer);
                data.Append(Encoding.UTF8.GetString(buffer, 0, size));
            }
            while (listener.Available > 0);

            string pattern = @"(\w+):([a-zA-Z0-9 а-яА-Я]+)";
            
            Regex regex = new Regex(pattern);
            var method = Regex.Match(data.ToString(), pattern).Groups[1].ToString();
            var additionalData = Regex.Match(data.ToString(), pattern).Groups[2].ToString();
            
           //routing 
            switch (method)
            {
                case "sendMessage":
                    //TODO: сделать подключение к БД к списку администраторов + прокинуть сюда объект ТГ + дернуть отправку
                    
            }

            listener.Shutdown(SocketShutdown.Both);
            listener.Close();
        }
    } 
    
}