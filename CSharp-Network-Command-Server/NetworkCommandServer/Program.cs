using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NetworkCommandServer;

public class Program
{
    private const int Port = 5555;
    private static readonly Stopwatch Uptime = Stopwatch.StartNew();
    private static int _clientCounter = 0;

    public static void Main()
    {
        TcpListener listener = new TcpListener(IPAddress.Any, Port);
        listener.Start();

        Console.WriteLine("Network Command Server");
        Console.WriteLine($"Listening on TCP port {Port}...");
        Console.WriteLine("Supported commands: ping, echo <message>, time, uptime, help, quit");
        Console.WriteLine();

        while (true)
        {
            Console.WriteLine("Waiting for client connection...");
            TcpClient client = listener.AcceptTcpClient();

            int clientId = Interlocked.Increment(ref _clientCounter);
            Console.WriteLine($"Client {clientId} connected from {client.Client.RemoteEndPoint}");

            Thread clientThread = new Thread(() => HandleClient(client, clientId));
            clientThread.Start();
        }
    }

    private static void HandleClient(TcpClient client, int clientId)
    {
        using TcpClient connectedClient = client;
        using NetworkStream stream = connectedClient.GetStream();

        byte[] buffer = new byte[1024];

        try
        {
            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);

                if (bytesRead == 0)
                {
                    Console.WriteLine($"Client {clientId} disconnected.");
                    break;
                }

                string request = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                Console.WriteLine($"Client {clientId} request: {request}");

                string response = ProcessCommand(request);
                byte[] responseBytes = Encoding.UTF8.GetBytes(response + "\n");
                stream.Write(responseBytes, 0, responseBytes.Length);

                if (request.Equals("quit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"Client {clientId} ended the session.");
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error with client {clientId}: {ex.Message}");
        }
    }

    private static string ProcessCommand(string request)
    {
        if (string.IsNullOrWhiteSpace(request))
        {
            return "ERROR: Empty command.";
        }

        string[] parts = request.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        string command = parts[0].ToLowerInvariant();
        string argument = parts.Length > 1 ? parts[1] : string.Empty;

        return command switch
        {
            "ping" => "pong",
            "echo" => string.IsNullOrWhiteSpace(argument)
                ? "ERROR: echo requires a message. Example: echo Hello World"
                : argument,
            "time" => $"Server time: {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
            "uptime" => $"Server uptime: {FormatUptime(Uptime.Elapsed)}",
            "help" => "Commands: ping, echo <message>, time, uptime, help, quit",
            "quit" => "Goodbye.",
            _ => $"ERROR: Unknown command '{command}'. Type help for available commands."
        };
    }

    private static string FormatUptime(TimeSpan time)
    {
        return $"{time.Days}d {time.Hours}h {time.Minutes}m {time.Seconds}s";
    }
}