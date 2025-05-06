using System;
using System.Collections.Generic;
using acolurk_base.classes;
using acolurk_base.helpers;
using UnityEngine;

namespace acolurk_base.commands;

public class privateCommands
{

    public static PlayerManager plm = NetworkBehaviourSingleton<PlayerManager>.instance;
    public static void LoadPrivateCommands()
    {
        help.Register();
        dummy.Register();
        list.Register();
        timeAndDate.Register();
        banan.Register();
        writeStPos.Register();
        Plugin.Log.LogInfo($"Private commands loaded!");
    }

    public static Command help = new Command(new List<string>
        {
            "help",
            "h",
            "man",
            "halp"
        },
        "The help command, which provides a means of accessing internal command descriptions and arguments. Use <i>/list</i> for a list of all commands.",
        new List<string>
        {
            "private"
        },
        new List<string>
        {
            "command"
        },
        new List<Type>
        {
            typeof(string)
        },
        (args,
            chat,
            clientId) =>
        {
            string result = "";
            if (args.Count == 0)
                args.Add("help");
            foreach (string inputCom in args)
            {
                foreach (Command command in Command.commands)
                {
                    if (command.names.Contains(inputCom.ToLower()))
                    {
                        result += $"<b>{command.names[0]}</b><br><i>{command.description}</i><br>";
                        result += $"Use: /{command.names[0]} ";
                        for (int i = 0;
                             i < command.argNames.Count;
                             i++)
                        {
                            result += $"<b>{command.argNames[i]}</b> <i>{command.argTypes[i].Name}</i> ";
                        }

                        result += "<br>Aliases: ";
                        foreach (string alias in command.names)
                            result += $"<i>{alias}</i> ";
                        break;
                    }
                }
            }

            chat.Server_SendSystemChatMessage(result,
                clientId);
        });

    public static Command dummy = new Command(new List<string>
        {
            "dummy"
        },
        "A dummy command for testing purposes.",
        new List<string>
        {
            "dummy",
            "private"
        },
        new List<string>
        {
            "arg1",
            "arg2"
        },
        new List<Type>
        {
            typeof(string),
            typeof(int)
        },
        (args,
            chat,
            clientId) =>
        {
            chat.Server_SendSystemChatMessage("Dummy command received!",
                clientId);
        });

    public static Command list = new Command(
        new List<string>
        {
            "list",
            "listcommands",
            "l",
            "listall"
        },
        "A command to list all currently loaded commands.",
        new List<string>
        {
            "private"
        },
        new List<string>
        {
        },
        new List<Type>
        {
        },
        (args,
            chat,
            clientId) =>
        {
            string result = "";
            foreach (Command command in Command.commands)
                result += $"<i>{command.names[0]}</i> ";
            chat.Server_SendSystemChatMessage(result,
                clientId);
        });

    public static Command timeAndDate = new Command(
        new List<string>
        {
            "time",
            "date",
            "clock"
        },
        "Sends the player the time and date in UTC.",
        new List<string>
        {
            "private"
        },
        new List<string>(),
        new List<Type>(),
        (args,
            chat,
            clientId) =>
        {
            string message = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            chat.Server_SendSystemChatMessage(message,
                clientId);
        });

    public static Command banan = new Command(
        new List<string>
        {
            "banan",
            "banana"
        },
        "Declares someone as a banana.",
        new List<string>
        {
            "private"
        },
        new List<string>
        {
            "player"
        },
        new List<Type>
        {
            typeof(string)
        },
        (args,
            chat,
            clientId) =>
        {
            if (args.Count == 0)
            {
                chat.Server_SendSystemChatMessage("Please specify a player name!",
                    clientId);
                return;
            }

            string player = args[0];
            chat.Server_SendSystemChatMessage($"<b>{player}</b> is a banan!");
        });
    
    public static Command writeStPos = new Command(
        new List<string>
        {
            "stickpos",
            "stp"
        },
        "Sends the caller a private message with their stick position in game coords.",
        new List<string>
        {
            "private"
        },
        new List<string>(),
        new List<Type>(),
        (args,
            chat,
            clientId) =>
        {
            chat.Server_SendSystemChatMessage($"Your stick coords are:<b>{plm.GetPlayerByClientId(clientId).Stick.BladeHandlePosition}");
        });
    
}