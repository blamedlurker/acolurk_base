using acolurk_base.classes;
using HarmonyLib;
using MS.Internal.Xml.XPath;

namespace acolurk_base.patches;

public class ServerManagerPatch
{
    public static PreciseLauncher ServerPL;
    [HarmonyPatch(typeof(ServerManager), nameof(ServerManager.Start))]
    public static class StartPatchMain
    {
        [HarmonyPostfix]
        public static void Postfix(ServerManager __instance)
        {
            ServerPL = new PreciseLauncher();
            Plugin.Log.LogDebug("ServerPL Made");
        }
    }
}