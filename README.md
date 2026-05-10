# Remote Command Networking

## Overview

This project is a simple client-server networking application written in C#. The server accepts connections from clients and responds to basic text commands. The goal of this project was to learn how networking communication works using TCP sockets.

## Features

The server currently supports these commands:

* `ping` → returns Pong!
* `echo <message>` → sends the same message back
* `uptime` → shows how long the server has been running

## Technologies Used

* C#
* .NET
* TCP sockets

## How to Run

### Start the Server

1. Open the solution in Visual Studio.
2. Run the `RemoteCommandServer` project.
3. The server will start listening for client connections.

### Start the Client

1. Run the `RemoteCommandClient` project.
2. Type commands into the console.
3. Responses from the server will be displayed.

## Video Demonstration

YouTube Video Link:



## What I Learned

While working on this project, I learned how client-server communication works using TCP networking. I also learned how to send and receive messages between programs and how to process commands on the server side. One of the more difficult parts was debugging communication issues between the client and server, but testing each part separately made it easier to fix problems.

## Future Improvements

Some future improvements could include:

* More commands
* Better error handling
* A graphical user interface
* Support for multiple clients at the same time
