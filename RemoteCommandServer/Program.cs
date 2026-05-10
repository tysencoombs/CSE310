using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace RemoteCommandServer;

/// <summary>
/// A simple TCP server for the CSE 310 Networking module.
/// The server listens for one client at a time, receives text commands,
/// processes the commands, and sends a text response back to the client.
/// </summary>
public class Program
{
    private const int Port = 5000;
    private const int BufferSize = 4096;
    private static readonly DateTime ServerStartTime = DateTime.Now;

    public static void Main()
    {
        TcpListener server = new TcpListener(IPAddress.Any, Port);
        server.Start();

        Console.WriteLine("Remote Command Server started.");
        Console.WriteLine($"Listening on port {Port}...");
        Console.WriteLine("Press Ctrl+C to stop the server.\n");

        // Keep accepting clients until the server program is closed.
        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine($"Client connected: {client.Client.RemoteEndPoint}");

            try
            {
                HandleClient(client);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Client error: {ex.Message}");
            }
            finally
            {
                client.Close();
                Console.WriteLine("Client disconnected.\n");
            }
        }
    }

    /// <summary>
    /// Reads commands from the client and sends responses until the client exits.
    /// </summary>
    private static void HandleClient(TcpClient client)
    {
        using NetworkStream stream = client.GetStream();
        SendMessage(stream, "Connected to Remote Command Server. Type HELP for commands.");

        bool keepRunning = true;
        while (keepRunning)
        {
            string request = ReceiveMessage(stream);

            if (string.IsNullOrWhiteSpace(request))
            {
                SendMessage(stream, "ERROR: Empty command received.");
                continue;
            }

            Console.WriteLine($"Received: {request}");
            string response = ProcessCommand(request, out keepRunning);
            SendMessage(stream, response);
            Console.WriteLine($"Sent: {response}");
        }
    }

    /// <summary>
    /// Converts a command string into the correct server response.
    /// This method demonstrates multiple different request types.
    /// </summary>
    private static string ProcessCommand(string request, out bool keepRunning)
    {
        keepRunning = true;
        string trimmedRequest = request.Trim();
        string[] parts = trimmedRequest.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        string command = parts[0].ToUpperInvariant();
        string argument = parts.Length > 1 ? parts[1] : string.Empty;

        switch (command)
        {
            case "PING":
                return "PONG - server is reachable.";

            case "ECHO":
                return string.IsNullOrWhiteSpace(argument)
                    ? "ERROR: Use ECHO followed by a message."
                    : $"ECHO: {argument}";

            case "UPTIME":
                TimeSpan uptime = DateTime.Now - ServerStartTime;
                return $"Server uptime: {uptime.Days} days, {uptime.Hours} hours, {uptime.Minutes} minutes, {uptime.Seconds} seconds.";

            case "TIME":
                return $"Server time: {DateTime.Now:F}";

            case "HELP":
                return BuildHelpMessage();

            case "QUIT":
            case "EXIT":
                keepRunning = false;
                return "Goodbye. Connection closing.";

            default:
                return $"ERROR: Unknown command '{command}'. Type HELP for valid commands.";
        }
    }

    /// <summary>
    /// Creates the help response shown to the client.
    /// </summary>
    private static string BuildHelpMessage()
    {
        return "Available commands: " +
               "PING, " +
               "ECHO <message>, " +
               "UPTIME, " +
               "TIME, " +
               "HELP, " +
               "QUIT";
    }

    /// <summary>
    /// Receives a UTF-8 message from the network stream.
    /// </summary>
    private static string ReceiveMessage(NetworkStream stream)
    {
        byte[] buffer = new byte[BufferSize];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        return Encoding.UTF8.GetString(buffer, 0, bytesRead);
    }

    /// <summary>
    /// Sends a UTF-8 message through the network stream.
    /// </summary>
    private static void SendMessage(NetworkStream stream, string message)
    {
        byte[] responseBytes = Encoding.UTF8.GetBytes(message);
        stream.Write(responseBytes, 0, responseBytes.Length);
    }
}
