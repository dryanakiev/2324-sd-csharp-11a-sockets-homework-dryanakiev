namespace ChatRoom.Client;

class Program
{
    static void Main(string[] args)
    {
        ClientSocket clientSocket = new ClientSocket();
        
        clientSocket.StartConnection();

        while (true)
        {
            clientSocket.SendMessage(Console.ReadLine());
        }
    }
}