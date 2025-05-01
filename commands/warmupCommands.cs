using System;
using System.Collections.Generic;
using acolurk_base.classes;
using Unity.Collections;
using UnityEngine;

namespace acolurk_base.commands;

public class warmupCommands
{
    public static void LoadWarmupCommands()
    {
        spawnPuck.Register();
        emptyPucks.Register();
        launcher.Register();
        Plugin.Log.LogInfo("Warmup commands loaded!");
    }

    public static Command spawnPuck = new Command(new List<string>
        {
            "spawn",
            "s",
            "sp",
            "pucks",
            "gibpuch",
            "puches",
            "puch",
            "opuchki"
        }, "A command to spawn however many pucks you want to spawn.", new List<string> { "warmup" },
        new List<string> { "numberOfPucks" }, new List<Type> { typeof(int) }, (args, chat, clientId) =>
        {
            PuckManager pm = NetworkBehaviourSingleton<PuckManager>.instance;
            PlayerManager plm = NetworkBehaviourSingleton<PlayerManager>.instance;
            Player player = plm.GetPlayerByClientId(clientId);
            string color = player.Team.Value.ToString().ToLower();
            if (color == "none" || color == "spectator") color = "grey";
            Vector3 pos = plm.GetPlayerByClientId(clientId).Stick.BladeHandlePosition;
            pos.y += 0.5f;
            Vector3 vel = plm.GetPlayerByClientId(clientId).PlayerBody.Rigidbody.velocity;
            int count = 1;
            if (args.Count > 0) int.TryParse(args[0], out count);
            if (count > 70)
            {
                chat.Server_SendSystemChatMessage("Please try a smaller number.", clientId);
                return;
            }

            int ticker = 0;
            for (; ticker < count; ticker++)
            {
                pos.y += 0.1f;
                pm.Server_SpawnPuck(pos, new Quaternion(0f, 0f, 0f, 0f), vel, false);
            }

            chat.Server_SendSystemChatMessage(
                $"<b><color={color}>{player.Username.Value}</b></color> has spawned {ticker} puck{(ticker == 1 ? "" : "s")}.");
        });

    public static Command emptyPucks = new Command(new List<string>
        {
            "emptypucks",
            "ep",
            "empty",
            "clear",
            "nopuch",
            "badpuch"
        }, "A command for clearing all currently spawned pucks.", new List<string> { "warmup" }, new List<string>(),
        new List<Type>(), (list, chat, clientId) =>
        {
            PuckManager pm = NetworkBehaviourSingleton<PuckManager>.instance;
            PlayerManager plm = NetworkBehaviourSingleton<PlayerManager>.instance;
            Player player = plm.GetPlayerByClientId(clientId);
            string color = player.Team.Value.ToString().ToLower();
            if (color == "none" || color == "spectator") color = "grey";
            pm.Server_DespawnPucks();
            chat.Server_SendSystemChatMessage(
                $"<b><color={color}>{player.Username.Value}</color></b> has despawned all pucks.");
        });

    public static Command launcher = new Command(new List<string> { "launcher", "lnch" },
        "A command for controlling a custom puck launcher.", new List<string> { "launcher", "private" },
        new List<string> { "launcherCommand", "args" }, new List<Type> { typeof(string), typeof(string[]) },
        (args, chat, clientId) => { throw new NotImplementedException(); });
    
    /*
    public static Command n = new Command(
        new List<string> {},
        "",
        new List<string> {},
        new List<string> {},
        new List<Type> {},
        (args, chat, clientId) => { });
        */
}