using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChatRoom.Server;

public class ServerSocket
{
    static TcpListener listener;
    static List<TcpClient> clients = new List<TcpClient>();

    public void StartServer()
    {
        try
        {
            // Start listening for client requests on port 8888
            listener = new TcpListener(IPAddress.Any, 8888);
            listener.Start();
            
            Console.WriteLine("Server started...");

            // Start a new thread to handle incoming client connections
            Thread clientThread = new Thread(new ThreadStart(ListenForClients));
            
            clientThread.Start();
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private static void ListenForClients()
    {
        while (true)
        {
            // Accept a pending client connection
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Client connected...");

            // Add client to the list of connected clients
            clients.Add(client);

            // Start a new thread to handle client communication
            Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientCommunication));
            clientThread.Start(client);
        }
    }

    private static void HandleClientCommunication(object clientObject)
    {
        TcpClient client = (TcpClient)clientObject;
        
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];

        while (true)
        {
            try
            {
                // Read incoming data from the client
                var bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0)
                    break;

                // Convert bytes to string and display
                string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Client: " + dataReceived);
                

                // Broadcast the received message to all connected clients
                BroadcastMessage(dataReceived);
            }
            catch
            {
                break;
            }
        }

        // Close client connection and remove from list
        client.Close();
        clients.Remove(client);
    }

    private static void BroadcastMessage(string message)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(message);
        foreach (TcpClient client in clients)
        {
            NetworkStream stream = client.GetStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
        }
    }

    private static void StopServer()
    {
        listener.Stop();
    }
}