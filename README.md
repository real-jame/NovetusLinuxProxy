# Novetus Web Proxy for Linux

## What is this exactly? Why does it exist?

Novetus works perfectly under Wine, except for the web proxy feature. This project is here to fill in that last bit of functionality for Linux users. In other words, it is an adaptation of the Novetus web proxy as a standalone C# .NET console application for Linux.

## What is the Novetus web proxy?

The web proxy is a component of the excellent [Novetus project](https://github.com/Novetus/Novetus_src), a multi-version Roblox client launcher. It handles client requests to roblox.com, mainly for downloading assets, but also for saving data, awarding badges, and viewing webpages such as opening Studio, the Help window, or after taking a screenshot. Of course, those API endpoints used are defunct now. The proxy works to reroute these requests behind the scenes to supported locations, either by [upgrading the request](https://assetdelivery.roblox.com/docs/), serving local copies of the static pages, or recreating the service locally (for badges). *Saving data (Data Persistence) is currently unimplemented on Novetus, so same goes for here.*

## Building

To build, use the [.NET 7.0 CLI](https://dotnet.microsoft.com/en-us/download/dotnet/7.0). Clone the repository and open a new terminal in /src. Then, run the command `dotnet build` to build.

## Usage

The web proxy must be configured manually on your system. Luckily, we only need to set it under Wine's settings, not your entire system. The best option is to run Novetus in a dedicated Wine prefix, which will allow other programs on Wine to connect to the internet normally. Create the prefix and open the HTTP proxy settings with this command (change "yourusername" to your user name): 

`WINEPREFIX="/home/yourusername/.novetus" WINEARCH=win32 wine rundll32.exe shell32.dll,Control_RunDLL inetcpl.cpl`

Next, click on the `Connections` tab. In the `Proxy server` section, check the `Use a proxy server` checkbox, set `Address` to `localhost`, and `Port` to `61710`. Then click `Apply` and `Ok` to save your changes and close the window.

Now, you may open Novetus, with a command like this (replacing "yourusername" with your user name and /path/to/novetus-windows/ with the path to your copy of Novetus):

`WINEPREFIX="/home/yourusername/.novetus" WINEARCH=win32 wine /path/to/novetus-windows/NovetusBootstrapper.exe`

Use this command when you want to start Novetus. Alongside it, start the proxy server too with this command (in a separate terminal):

`/path/to/NovetusLinuxProxy`

The proxy server will start.

### If the proxy server errors on startup

If this happens, the Novetus proxy server may be using the port. Launch Novetus with the console, and input the console command `proxy disable` to permanently turn off Novetus's own proxy. Then, try running the Linux proxy server again.