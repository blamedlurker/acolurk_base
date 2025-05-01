using System;
using System.Collections.Generic;
using acolurk_base.classes;
using acolurk_base.helpers;

namespace acolurk_base.commands;

public class privateCommands
{
    public static void LoadPrivateCommands()
    {
        help.Register();
        dummy.Register();
        Plugin.Log.LogInfo($"Private commands loaded!");
    }

    public static Command help = new Command(
        new List<string> { "help", "h", "man", "halp" },
        "The help command, which provides a means of accessing internal command descriptions and arguments.",
        new List<string> { "private" },
        new List<string> { "command" },
        new List<Type> { typeof(string) },
        (args, chat, clientId) =>
        {
            string result = "";
            if (args.Count == 0) args.Add("help");
            foreach (string inputCom in args)
            {

                foreach (Command command in Command.commands)
                {
                    if (command.names.Contains(inputCom.ToLower()))
                    {
                        result += $"<b>{command.names[0]}</b><br><i>{command.description}</i><br>";
                        result += $"Use: /{command.names[0]} ";
                        for (int i = 0; i < command.argNames.Count; i++)
                        {
                            result += $"<b>{command.argNames[i]}</b> <i>{command.argTypes[i].Name}</i> ";
                        }
                        break;
                    }
                }
            }

            chat.Server_SendSystemChatMessage(result, clientId);
        });

    public static Command dummy = new Command(
        new List<string> {"dummy"},
        "A dummy command for testing purposes",
        new List<string> { "dummy", "private" },
        new List<string> {"arg1", "arg2"},
        new List<Type> { typeof(string), typeof(int) },
        (args, chat, clientId) =>
        {
            chat.Server_SendSystemChatMessage("Dummy command received!", clientId);
        });
}