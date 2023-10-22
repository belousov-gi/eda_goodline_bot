using System.Net;
using System.Net.Sockets;
using System.Text;
using ScheduledSendler;

public class Program
{
    public static void Main(string[] args)
    {
        int amountArgs = args.Length;
        Dictionary<string, string> inputArgs = new Dictionary<string, string>();

        for (int i = 0; i < amountArgs; i+=2)
        {
            var inputArg = args[i].Remove(0, 1);
            if (args[i].StartsWith('-') && Enum.IsDefined(typeof(AvailableInputArgs), inputArg))
            {
                inputArgs.Add(inputArg, args[i+1]);
            }
            else
            {
                throw new Exception("Invalid input command");
            }
        }
        
        string? host;
        inputArgs.TryGetValue(AvailableInputArgs.h.ToString(), out host);
        
        try
        {
            string ip = host ?? "0.0.0.0";
            const int port = 9999;
            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );

            var message = "createOrdersInfoForAdmin:{}";
            var date = Encoding.UTF8.GetBytes(message);
        
            tcpSocket.Connect(tcpEndPoint);
            tcpSocket.Send(date); 
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}