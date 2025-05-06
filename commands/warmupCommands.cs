using System;
using System.Collections.Generic;
using acolurk_base.classes;
using acolurk_base.patches;
using UnityEngine;

namespace acolurk_base.commands;

public class warmupCommands
{
    public static void LoadWarmupCommands()
    {
        spawnPuck.Register();
        emptyPucks.Register();
        launcherCntrl.Register();
        Plugin.Log.LogInfo("Warmup commands loaded!");

    }

    public static Command spawnPuck = new(new List<string>
        {
            "spawn",
            "s",
            "sp",
            "pucks",
            "gibpuch",
            "puches",
            "puch",
            "opuchki"
        },
        "A command to spawn however many pucks you want to spawn.",
        new List<string>
        {
            "warmup"
        },
        new List<string>
        {
            "numberOfPucks"
        },
        new List<Type>
        {
            typeof(int)
        },
        (args,
            chat,
            clientId) =>
        {
            PuckManager pm = NetworkBehaviourSingleton<PuckManager>.instance;
            PlayerManager plm = NetworkBehaviourSingleton<PlayerManager>.instance;
            
            var player = plm.GetPlayerByClientId(clientId);
            var color = player.Team.Value.ToString()
                .ToLower();
            if (color == "none" || color == "spectator")
                color = "grey";
            var pos = plm.GetPlayerByClientId(clientId)
                .Stick.BladeHandlePosition;
            pos.y += 0.5f;
            var vel = plm.GetPlayerByClientId(clientId)
                .PlayerBody.Rigidbody.velocity;
            var count = 1;
            if (args.Count > 0)
                int.TryParse(args[0],
                    out count);
            if (count > 70)
            {
                chat.Server_SendSystemChatMessage("Please try a smaller number.",
                    clientId);
                return;
            }

            var ticker = 0;
            for (;
                 ticker < count;
                 ticker++)
            {
                pos.y += 0.1f;
                pm.Server_SpawnPuck(pos,
                    new Quaternion(0f,
                        0f,
                        0f,
                        0f),
                    vel);
            }

            chat.Server_SendSystemChatMessage(
                $"<b><color={color}>{player.Username.Value}</b></color> has spawned {ticker} puck{(ticker == 1 ? "" : "s")}.");
        });

    public static Command emptyPucks = new(new List<string>
        {
            "emptypucks",
            "ep",
            "empty",
            "clear",
            "nopuch",
            "badpuch"
        },
        "A command for clearing all currently spawned pucks.",
        new List<string>
        {
            "warmup"
        },
        new List<string>(),
        new List<Type>(),
        (list,
            chat,
            clientId) =>
        {
            PuckManager pm = NetworkBehaviourSingleton<PuckManager>.instance;
            PlayerManager plm = NetworkBehaviourSingleton<PlayerManager>.instance;
            
            var player = plm.GetPlayerByClientId(clientId);
            var color = player.Team.Value.ToString()
                .ToLower();
            if (color == "none" || color == "spectator")
                color = "grey";
            pm.Server_DespawnPucks();
            chat.Server_SendSystemChatMessage(
                $"<b><color={color}>{player.Username.Value}</color></b> has despawned all pucks.");
        });

    public static Command launcherCntrl = new(
        new List<string>
        {
            "launcher",
            "launch",
            "lch"
        },
        "Control your own personal launcher. Can be set to practice shots on goal, practice passes, or just shoot between two points.",
        new List<string>
        {
            "warmup"
        },
        new List<string>
        {
            "modifier",
            "argument"
        },
        new List<Type>(),
        (args,
            chat,
            clientId) =>
        {
            PuckManager pm = NetworkBehaviourSingleton<PuckManager>.instance;
            PlayerManager plm = NetworkBehaviourSingleton<PlayerManager>.instance;
            
            var player = plm.GetPlayerByClientId(clientId);
            var myLauncher = ServerManagerPatch.ServerPL;
            switch (args[0])
            {
                case "toggle":
                    myLauncher.Toggle();
                    chat.Server_SendSystemChatMessage($"Launcher has been toggled by {plm.GetPlayerByClientId(clientId).Username.Value}.");
                    break;
                case "enable":
                    myLauncher.Enable();
                    chat.Server_SendSystemChatMessage($"Launcher has been enabled by {plm.GetPlayerByClientId(clientId).Username.Value}.");
                    break;
                case "disable":
                    myLauncher.Disable();
                    chat.Server_SendSystemChatMessage($"Launcher has been disabled by {plm.GetPlayerByClientId(clientId).Username.Value}.");
                    break;
                case "start":
                    if (args[1] == "stick")
                    {
                        myLauncher.setStart(player.Stick.BladeHandlePosition);
                        chat.Server_SendSystemChatMessage($"Launcher is now launching from your last stick position.", clientId);
                    }
                    else if (args.Count >= 3)
                    {
                        var start = new Vector3();
                        float.TryParse(args[1],
                            out start.x);
                        float.TryParse(args[2],
                            out start.y);
                        float.TryParse(args[3],
                            out start.z);
                        myLauncher.setStart(start);
                        chat.Server_SendSystemChatMessage($"Launcher is now launching from {start}.", clientId);
                    }

                    break;
                case "target":
                    if (args[1] == "stick")
                    {
                        myLauncher.setTarget(player.Stick.BladeHandlePosition);
                        chat.Server_SendSystemChatMessage($"Launcher is now targeting your last stick position.", clientId);
                    }
                    else if (args.Count >= 3)
                    {
                        var start = new Vector3();
                        float.TryParse(args[1],
                            out start.x);
                        float.TryParse(args[2],
                            out start.y);
                        float.TryParse(args[3],
                            out start.z);
                        myLauncher.setTarget(start);
                        chat.Server_SendSystemChatMessage($"Launcher is now targeting {start}.", clientId);
                    }

                    break;
                case "TOF":
                    float newTOF;
                    float.TryParse(args[1],
                        out newTOF);
                    myLauncher.setTOF(newTOF);
                    chat.Server_SendSystemChatMessage($"Launcher is now trying for {newTOF} second time of flight.", clientId);
                    break;
            }
        });
}