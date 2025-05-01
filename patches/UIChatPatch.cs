using acolurk_base.helpers;
using HarmonyLib;

namespace acolurk_base.patches;

public class UIChatPatch
{
    [HarmonyPatch(typeof(UIChat), nameof(UIChat.Server_ProcessPlayerChatMessage))]
    public static class UIChatPatchMain
    {
        [HarmonyPrefix]
        public static bool UIChat_Server_ProcessPlayerChatMessage(UIChat __instance, Player player, string message,
            ulong clientId, bool useTeamChat, bool isMuted)
        {
            // catch teamchat/mutes
            if (useTeamChat || isMuted) return true;
           
            // is it a command?
            if (!message.StartsWith('/')) return true;

            string[] args = message.Remove(0, 1).Split(' '); // obtain arguments from command
            return CommandHelper.ParseCommand(args, __instance, clientId);
            
        }
    }
}
