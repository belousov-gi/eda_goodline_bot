﻿using System.Net;
using System.Net.Sockets;
using System.Text;

public class Program
{
    public static void Main()
    {
        try
        {
            string? host = Environment.GetEnvironmentVariable("IP_API_CONTAINER");
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