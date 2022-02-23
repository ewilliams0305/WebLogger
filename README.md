# VirtualControlWeblogger
 Websocket server designed to provide an accessible console application to a Crestron VC4 program instance

 ## Table of Contents
1. [VS Solution](#Visual-Studio-Solution)
2. [Example Program](#WebLogger-Example-Program)
3. [Web Logger](#Create-a-WebLogger)
4. [HTML Example](#html)

## Visual Studio Solution

The included solution includes two projects.  `WebLoggerExample` and `VirtualControl.Debugging`. 

### WebLogger Example Program

 The Weblogger example is a Crestron SDK SimpleSharp program that demonstrates how to instantiate the `WebLogger` class and add console commands with callbacks.

#### Create a WebLogger

Create a new instance of the `WebLoger` class included in the `VirtualControl.Debugging.WebLogger` namespace

``using VirtualControl.Debugging.WebLogger;``

Create a new instance and start the server

`WebLogger logger = new WebLogger(54321, false);
                logger.Start();`