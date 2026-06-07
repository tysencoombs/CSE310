using System;
using System.Net.Sockets;
using System.Text;

namespace NetworkCommandClient;

public class Program
{
    private const string Host = "127.0.0.1";
    private const int Port = 5555;

    public static void Main()
    {
        Console.WriteLine("Network Command Client");
        Console.WriteLine($"Connecting to {Host}:{Port}...");

        try
        {
            using TcpClient client = new TcpClient();
            client.Connect(Host, Port);

            using NetworkStream stream = client.GetStream();

            Console.WriteLine("Connected successfully.");
            Console.WriteLine("Try: ping, echo Hello World, time, uptime, help, quit");
            Console.WriteLine();

            while (true)
            {
                Console.Write("Command> ");
                string? command = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(command))
                {
                    continue;
                }

                byte[] requestBytes = Encoding.UTF8.GetBytes(command.Trim() + "\n");
                stream.Write(requestBytes, 0, requestBytes.Length);

                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);

                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                Console.WriteLine("Server: " + response);

                if (command.Equals("quit", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Client error: " + ex.Message);
            Console.WriteLine("Make sure the server is running first.");
        }

        Console.WriteLine("Client closed.");
    }
}