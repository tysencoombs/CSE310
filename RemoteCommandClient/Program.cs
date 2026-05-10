using System;
using System.Net.Sockets;
using System.Text;

namespace RemoteCommandClient;

/// <summary>
/// A simple TCP client for the CSE 310 Networking module.
/// The client connects to the server, sends commands typed by the user,
/// receives responses, and displays those responses in the console.
/// </summary>
public class Program
{
    private const string DefaultServerAddress = "127.0.0.1";
    private const int DefaultPort = 5000;
    private const int BufferSize = 4096;

    public static void Main(string[] args)
    {
        string serverAddress = args.Length > 0 ? args[0] : DefaultServerAddress;
        int port = args.Length > 1 && int.TryParse(args[1], out int customPort)
            ? customPort
            : DefaultPort;

        Console.WriteLine("Remote Command Client");
        Console.WriteLine($"Connecting to {serverAddress}:{port}...");

        try
        {
            using TcpClient client = new TcpClient(serverAddress, port);
            using NetworkStream stream = client.GetStream();

            Console.WriteLine(ReceiveMessage(stream));
            Console.WriteLine("Try commands like: PING, ECHO hello, UPTIME, TIME, HELP, QUIT\n");

            RunCommandLoop(stream);
        }
        catch (SocketException)
        {
            Console.WriteLine("Could not connect to the server.");
            Console.WriteLine("Make sure the server program is running first.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    /// <summary>
    /// Continues asking the user for commands until they choose to quit.
    /// </summary>
    private static void RunCommandLoop(NetworkStream stream)
    {
        bool keepRunning = true;

        while (keepRunning)
        {
            Console.Write("Command> ");
            string? command = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(command))
            {
                Console.WriteLine("Please enter a command or type HELP.");
                continue;
            }

            SendMessage(stream, command);
            string response = ReceiveMessage(stream);
            Console.WriteLine(response);

            string normalizedCommand = command.Trim().ToUpperInvariant();
            if (normalizedCommand == "QUIT" || normalizedCommand == "EXIT")
            {
                keepRunning = false;
            }
        }
    }

    /// <summary>
    /// Sends a UTF-8 command to the server.
    /// </summary>
    private static void SendMessage(NetworkStream stream, string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        stream.Write(data, 0, data.Length);
    }

    /// <summary>
    /// Receives a UTF-8 response from the server.
    /// </summary>
    private static string ReceiveMessage(NetworkStream stream)
    {
        byte[] buffer = new byte[BufferSize];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        return Encoding.UTF8.GetString(buffer, 0, bytesRead);
    }
}
