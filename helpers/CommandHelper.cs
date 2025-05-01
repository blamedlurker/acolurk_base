using System;
using System.Collections.Generic;
using System.Linq;
using acolurk_base.classes;
using acolurk_base.commands;
using HarmonyLib;
using Microsoft.Extensions.Logging;

namespace acolurk_base.helpers;

public static class CommandHelper
{
    public static List<Command> commands = Command.commands;
    public static bool ParseCommand(string[] args, UIChat chat, ulong clientId)
    {
        // string logMessage = "ParseCommand was called with the following arguments:\n";
        // foreach (string arg in args) logMessage += $"{arg}\n";
        // Plugin.Log.LogInfo(logMessage);
        string logMessage3 = "The following commands are registered:\n";
        foreach (var command in commands) logMessage3 += command.names[0] + "\n";
        Plugin.Log.LogInfo(logMessage3);
        
        List<string> argsList = args.ToList();
        foreach (Command command in commands)
        {
            if (command.names.Contains(argsList[0].ToLower()))
            {
                argsList.RemoveAt(0);
                command.CallCommand(argsList, chat, clientId);
                string logMessage2 = $"{command.names[0]} was called with the following arguments:\n";
                foreach (string arg in argsList) logMessage2 += $"{arg}\n";
                Plugin.Log.LogInfo(logMessage2);
                return false;
            }
        }
        return true;
    }
}