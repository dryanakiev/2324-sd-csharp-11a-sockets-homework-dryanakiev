using NUnit.Framework;
using System;
using System.Net.Sockets;
using System.Text;
using ChatRoom.Server;
using ChatRoom.Client;

namespace ChatRoom.Tests;

public class ChatRoomClientTest
{
    private ServerSocket server;

    [SetUp]
    public void Setup()
    {
        server = new ServerSocket();
        Thread serverThread = new Thread(new ThreadStart(server.StartServer));
        serverThread.Start();
        Thread.Sleep(1000); // Allow server to start before running tests
    }

    [TearDown]
    public void Teardown()
    {
        server.StartServer();
    }
    
    [Test]
    public void UppercaseConversionTest()
    {
        TcpClient client = new TcpClient("localhost", 8888);
        NetworkStream stream = client.GetStream();

        string message = "hello";
        byte[] data = Encoding.ASCII.GetBytes(message);
        stream.Write(data, 0, data.Length);
        stream.Flush();

        byte[] receivedData = new byte[1024];
        int bytesRead = stream.Read(receivedData, 0, receivedData.Length);
        string receivedMessage = Encoding.ASCII.GetString(receivedData, 0, bytesRead);

        client.Close();

        Assert.AreEqual("HELLO", receivedMessage);
    }
}