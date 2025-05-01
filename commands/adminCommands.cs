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
}