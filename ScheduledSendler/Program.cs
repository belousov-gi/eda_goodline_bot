﻿using System.Net;
using System.Net.Sockets;
using System.Text;

public class Program
{
    public static void Main()
    {
        while (true)
        {
            const string ip = "127.0.0.1";
            const int port = 9999;
            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );

          
            Console.WriteLine("Innput your message");
            var message = Console.ReadLine();

            var date = Encoding.UTF8.GetBytes(message);
        
            tcpSocket.Connect(tcpEndPoint);
            tcpSocket.Send(date); 
        }
        
    }
}