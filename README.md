# VirtualControlWeblogger
 Websocket server designed to provide an accessible console application to a Crestron VC4 program instance

 ## Table of Contents
1. [VS Solution](#Visual-Studio-Solution)
2. [Example Program](#WebLogger-Example-Program)
3. [Web Logger](#Create-a-WebLogger)
4. [Console Commands](#Register-Console-Commands)
4. [Embedded HTML](#Embedded-HTML)

## Visual Studio Solution

The included solution includes two projects.  `WebLoggerExample` and `VirtualControl.Debugging`. 

### WebLogger Example Program

 The Weblogger example is a Crestron SDK SimpleSharp program that demonstrates how to instantiate the `WebLogger` class and add console commands with callbacks.

#### Create a WebLogger

Create a new instance of the `WebLoger` class included in the `VirtualControl.Debugging.WebLogger` namespace.  Creating a new instace will:

1. Create a Websocket Server at the specified port
2. Copy all embedded resource HTML files to the VC4 server
3. Create a few default console commands

```csharp
using VirtualControl.Debugging.WebLogger;
```

Create a new instance and start the server

```csharp
WebLogger logger = new WebLogger(54321, false);
logger.Start();
```

#### Register Console Commands

Custom console commands be created at any point in the life cycle of the `WebLogger`.  
To add a custom command create a new instance of the `ConsoleCommand` class.  Each console command will be added to a dictionary using the `Command` property as the key.  Each console command should have a pointer to callback method.  When a string matching the `Command` name is received from the `WebLogger` server, the callback will be invoked. 

###### Default Constructor

Using the default constuctor and setting all properties with the object initialization syntax

```csharp
new ConsoleCommand()
{
    Command = "EXAMPLE",
    Description = "Simple example of console command",
    Help = "Parameter: NA",
    CommandAction = (cmd, args) =>
    {
        logger.WriteLine($"{cmd} Haha");
    }
};
```

Using the optional constuctor to set all properties at instantiation
```csharp
new ConsoleCommand("EXAMPLE", "Simple example of console command", "Parameter: NA", Handler);
```

Console command callback signature
```csharp
/// <summary>
/// Console command callback
/// </summary>
/// <param name="command">Command that was issued</param>
/// <param name="arguments">Argument passed into the command seperated by spaces</param>
public delegate void ConsoleCommandCallback(string command, List<string> arguments);
```

Putting it all together the example program illustrates a lambda expression with two custom commands.  The command `List<string> arguments` are created by content passed from the console seperated by spaces.  Each argument will be passed to the delegate as a seperate string.

```csharp
new ConsoleCommand()
{
    Command = "EXAMPLE",
    Description = "Simple example of console command",
    Help = "Parameter: NA",
    CommandAction = (cmd, args) =>
    {
        logger.WriteLine($"{cmd} Haha");
    }
};

```

## Embedded HTML

Located in the `VirtualControl.Debugging` project is a folder titled `WebLogger.HTML`.  All HTML source files have been added to the project and configured as an embedded resource.  These files will be automatically extracted and written to the HTML directory of the VC4 instance.  **Be aware, the program will check if the files are already created and ONLY write them if they are not found.  This means the HTML files will need to be deleted off the server if changees to the HTML are made**

After loading the code to your VC4, create a new room and associate it with the program.  Once the room has been started browse to `/opt/crestron/virtualcontrol/RunningPrograms/[ROOM ID]/html/logger` to review the files on the server.  Files will be served up using the index/html file found at http://server/VirtualControl/Rooms/EXAMPLE/Html/logger/index.html



