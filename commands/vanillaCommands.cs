using System;
using System.Collections.Generic;
using acolurk_base.classes;

namespace acolurk_base.commands;

public class vanillaCommands
{
    public static void LoadVanillaCommands()
    {
        voteStart.Register();
        voteWarmup.Register();
        voteKick.Register();
        Plugin.Log.LogInfo("Vanilla commands loaded!");
    }
    
    public static Command voteStart = new Command(
        new List<string> {"votestart", "vs", "startgame", "start"},
        "A vanilla command to vote to start a new game.",
        new List<string> {"vanilla"},
        new List<string> {},
        new List<Type> {},
        (args, chat, clientId) =>
        {
            PlayerManager plm = NetworkBehaviourSingleton<PlayerManager>.instance;
            Player player = plm.GetPlayerByClientId(clientId);
            VoteManager vm = NetworkBehaviourSingleton<VoteManager>.instance;
            if (vm.Server_IsVoteStarted(VoteType.Start)) vm.Server_SubmitVote(VoteType.Start, player);
            else
            {
                vm.Server_CreateVote(VoteType.Start, plm.players.Count / 2, player);
            }
        });
    
    public static Command voteWarmup = new Command(
        new List<string> {"votewarmup", "vw", "warmup"},
        "A vanilla command to vote to enter warmup.",
        new List<string> {"vanilla"},
        new List<string> {},
        new List<Type> {},
        (args, chat, clientId) =>
        {
            PlayerManager plm = NetworkBehaviourSingleton<PlayerManager>.instance;
            Player player = plm.GetPlayerByClientId(clientId);
            VoteManager vm = NetworkBehaviourSingleton<VoteManager>.instance;
            if (vm.Server_IsVoteStarted(VoteType.Warmup)) vm.Server_SubmitVote(VoteType.Warmup, player);
            else
            {
                vm.Server_CreateVote(VoteType.Warmup, plm.players.Count / 2, player);
            }
        });
    
    public static Command voteKick = new Command(
        new List<string> {"votekick", "vk"},
        "A vanilla command to vote to kick a player from the server.",
        new List<string> {"vanilla"},
        new List<string> {"PlayerID"},
        new List<Type> {typeof(string)},
        (args, chat, clientId) =>
        {
            throw new Exception("This should not be called");
        });
    
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