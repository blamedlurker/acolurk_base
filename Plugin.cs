using System.Collections.Generic;
using System.Reflection;
using acolurk_base.classes;
using acolurk_base.commands;
using acolurk_base.helpers;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace acolurk_base;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    internal static new ManualLogSource Log;

    public override void Load()
    {
        // Plugin startup logic
        Log = base.Log;
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is patched!");

        privateCommands.LoadPrivateCommands();
        warmupCommands.LoadWarmupCommands();
        vanillaCommands.LoadVanillaCommands();
        adminCommands.LoadAdminCommands();
        string logCmnds = "The following commands are loaded: ";
        foreach (Command command in Command.commands)
        {
            logCmnds += command.names[0] + ", ";
        }

        logCmnds = logCmnds.TrimEnd(' ').TrimEnd(',');
        Plugin.Log.LogInfo(logCmnds);
        
    }
}