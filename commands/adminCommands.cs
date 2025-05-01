using System;
using System.Collections.Generic;
using acolurk_base.classes;
using UnityEngine.Rendering;

namespace acolurk_base.commands;

public class adminCommands
{
    public static void LoadAdminCommands()
    {
        restartAnnouncement.Register();
        setPhase.Register();
        Plugin.Log.LogInfo($"Admin commands loaded!");
    }

    public static Command restartAnnouncement = new Command(new List<string> { "restart" },
        "An admin-only command which announces that a server restart is impending.",
        new List<string>{ "admin" },
        new List<string>(),
        new List<Type>(),
        (args, chat, clientId) =>
        {
            chat.Server_SendSystemChatMessage("<b>This server will be restarting soon.</b> If you do not leave of your own volition,"
            + " the game will appear to freeze -- however, you will still be able to disconnect through the menu.");
        });

    public static Command setPhase = new Command(
        new List<string> {"phase", "setphase", "setp"},
        "Set the current gamephase to a desired state.",
        new List<string> {"admin"},
        new List<string> {"phase"},
        new List<Type> {typeof(GamePhase)},
        (args, chat, clientId) =>
        {
            GameManager gm = NetworkBehaviourSingleton<GameManager>.instance;
            foreach (GamePhase phase in Enum.GetValues(typeof(GamePhase)))
            {
                if (phase.ToString().Equals(args[0])) gm.Server_SetPhase(phase);
            }
            chat.Server_SendSystemChatMessage($"The game state was set to {args[0]}");
        }
        );
}