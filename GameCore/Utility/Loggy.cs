using System;
using dgameincsharp.GameCore.Enums;
using Godot;

namespace dgameincsharp.GameCore.Utility;

public static class Loggy
{
    public static void Log(LogLevel level, params object[] message)
    {
        var dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var callingMethod = new System.Diagnostics.StackTrace().GetFrame(2).GetMethod();
        var logMessage = $"{dateTime} [{level}] [{callingMethod}] [{callingMethod.Name}]";

        string color = "TEAL";

        switch (level)
        {
            case LogLevel.DEBUG:
                color = "GREEN_YELLOW";
                break;
            case LogLevel.INFO:
                color = "TEAL";
                break;
            case LogLevel.WARNING:
                color = "GOLDENROD";
                break;
            case LogLevel.ERROR:
                color = "MAROON";
                break;
            default:
                break;
        }
        
        GD.PrintRich([$"[color={color}][{logMessage}/color]", ..message]);
    }
    
    public static void Debug(params object[] message)
    {
        Log(LogLevel.DEBUG, message);
    }
    
    public static void Info(params object[] message)
    {
        Log(LogLevel.INFO, message);
    }
    
    public static void Warning(params object[] message)
    {
        Log(LogLevel.WARNING, message);
    }
    
    public static void Error(params object[] message)
    {
        Log(LogLevel.ERROR, message);
    }
}