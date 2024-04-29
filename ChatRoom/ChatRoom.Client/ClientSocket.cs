using System;
using System.Net.Sockets;
using System.Text;

namespace ChatRoom.Client;

public class ClientSocket
{
    private TcpClient _client;

    public void StartConnection()
    {
        try
        {
            _client = new TcpClient("localhost", 8888);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("Connected to server...");
    }

    public void SendMessage(string message)
    {
        NetworkStream stream = _client.GetStream();
        
        Console.Write("Enter your message: ");
        
        // Start reading user input and send it to the server
        try
        {
            message = Console.ReadLine();
            byte[] data = Encoding.ASCII.GetBytes(message);
        
            stream.Write(data, 0, data.Length);
            
            stream.Flush();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}