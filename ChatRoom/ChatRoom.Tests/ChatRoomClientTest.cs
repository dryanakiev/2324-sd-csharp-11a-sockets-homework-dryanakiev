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

    private Thread serverThread;
    
    [SetUp]
    public void Setup()
    {
        server = new ServerSocket();
        serverThread = new Thread(new ThreadStart(server.StartServer));
        
        serverThread.Start();
        
        Thread.Sleep(1000); // Allow server to start before running tests
    }
    
    [TearDown]
    public void Teardown()
    {
        server.StopServer();
    }
    
    
    
    
    // TODO: Change the greeting message from the server on established connection
    // You can change the greeting string of the server and/or the unit test to your liking.
    [Test]
    public void TestGreetingMessage()
    {
        // Arrange
        ClientSocket client = new ClientSocket();
        client.StartConnection();

        // Act

        string receivedMessage = client.ReceiveMessage();

        client.CloseConnection();
        
        // Assert
        Assert.That(receivedMessage, Is.EqualTo("Welcome to the chat server!"));
    }

    
    
    
    
    // TODO: Implement a server reply to the client
    // Make the server reply to a message sent from the client. You can change the string of the server and/or the unit test to your liking.
    [Test]
    public void TestReplyToClient()
    {
        // Arrange
        ClientSocket client = new ClientSocket();
        client.StartConnection();

        // Act
        string message = "Test message";
        client.SendMessage(message);

        string receivedMessage = client.ReceiveMessage();

        client.CloseConnection();

        // Assert
        Assert.That(receivedMessage, Is.EqualTo("Message received: Test message"));
    }
    
    
    
    
    
    
    // TODO: Implement a method in the server that checks an incoming messages from a client is a lowercase string.
    // If the message has lower case letters only, the server should broadcast to the client the same message but in ALLCAPS.
    [Test]
    public void TestUppercaseConversion()
    {
        // Arrange
        ClientSocket client = new ClientSocket();
        client.StartConnection();

        // Act
        string message = "hello";
        client.SendMessage(message);

        string receivedMessage = client.ReceiveMessage();

        client.CloseConnection();

        // Assert
        Assert.That(receivedMessage, Is.EqualTo("HELLO"));
    }
    
    
    
    
    // TODO: Implement a server method to handle multiple client communication
    // In the ListenForClients method you can add a condition to check if there are more than one clients in the clients list. Then you simply need to broadcast the received message back to all clients, which you have defined in the BroadcastMessage method.
    [Test]
    public void TestBroadcastMessage()
    {
        // Arrange
        ClientSocket clientOne = new ClientSocket();
        clientOne.StartConnection();
        
        ClientSocket clientTwo = new ClientSocket();
        clientTwo.StartConnection();

        // Act
        string message = "This is a message from client 1";
        clientOne.SendMessage(message);
        
        // Ignore greeting message. Uncomment to run test
        // clientTwo.ReceiveMessage(); 
        string receivedMessage = clientTwo.ReceiveMessage();

        clientOne.CloseConnection();
        clientTwo.CloseConnection();

        // Assert
        Assert.That(receivedMessage, Is.EqualTo(message));
    }
    
    
    
    
    
    // TODO: Implement a method in the server that checks if an incoming messages is a number.
    // If the message has numbers ONLY, the server should reply to the client the sum of each number in the message. If you wish you can change up the method to do any other arithmetic operation.
    [Test]
    public void TestSimpleCalculationAndNumberIdentification()
    {
        // Arrange
        ClientSocket client = new ClientSocket();
        client.StartConnection();

        // Act
        string message = "12345";
        client.SendMessage(message);

        string receivedMessage = client.ReceiveMessage();

        client.CloseConnection();

        // Assert
        Assert.That(receivedMessage, Is.EqualTo("15"));
    }

    [Test]
    public void Test()
    {
        Assert.Pass();
    }
}