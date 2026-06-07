# Overview

For this project, I wanted to learn more about network communication and how software applications exchange information over a network. I created a client-server networking application in C# that allows a client program to connect to a server and send text commands. The server processes each command and sends a response back to the client.

To use the software, start the server application first. The server begins listening for incoming TCP connections on port 5555. Next, start the client application. The client connects to the server and allows the user to enter commands such as `ping`, `echo`, `time`, and `uptime`. The server reads the request, creates the correct response, and sends it back to the client. The response is then displayed in the client console.

The purpose of writing this software was to better understand TCP networking, socket programming, client-server architecture, and request-response communication. Building this project helped me learn how two separate programs can communicate with each other using the networking stack.

[Software Demo Video](https://www.youtube.com/watch?v=p24UiC-uLzU)

# Network Communication

This project uses a client-server architecture. The server is responsible for listening for incoming connections and responding to client requests. The client is responsible for connecting to the server, sending commands, and displaying the responses that come back.

The program uses TCP communication. TCP was used because it provides reliable, ordered communication between the client and server. The server listens on port `5555`, and the client connects to `127.0.0.1` on port `5555` by default.

Messages are sent between the client and server as UTF-8 plain text strings. The client sends one command at a time, and the server responds with a plain text response. Example commands include:

- `ping` - server responds with `pong`
- `echo Hello World` - server responds with `Hello World`
- `time` - server responds with the current server time
- `uptime` - server responds with how long the server has been running
- `help` - server displays the available commands
- `quit` - client ends the session

# Development Environment

The software was developed using the following tools:

- Visual Studio Code
- .NET 8
- GitHub

The programming language used was C#. The project uses standard .NET libraries, including:

- `System.Net`
- `System.Net.Sockets`
- `System.Text`
- `System.Threading.Tasks`
- `System.Diagnostics`

These libraries were used to create TCP connections, listen for clients, send and receive messages, process commands, and track server uptime.

# Useful Websites

* [Microsoft - System.Net.Sockets Namespace](https://learn.microsoft.com/en-us/dotnet/api/system.net.sockets)
* [Microsoft - TcpListener Class](https://learn.microsoft.com/en-us/dotnet/api/system.net.sockets.tcplistener)
* [Microsoft - TcpClient Class](https://learn.microsoft.com/en-us/dotnet/api/system.net.sockets.tcpclient)
* [Microsoft - C# Documentation](https://learn.microsoft.com/en-us/dotnet/csharp/)

# Future Work

* Add a graphical user interface instead of using only the console.
* Add stronger error handling for more unusual disconnect situations.
* Add file-based commands where the server can read information from a local file.
* Add user authentication before allowing commands to be processed.
* Improve the message format.
