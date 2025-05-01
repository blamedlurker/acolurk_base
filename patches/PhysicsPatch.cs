using acolurk_base.helpers;
using HarmonyLib;

namespace acolurk_base.patches;

public class PhysicsPatch
{
    [HarmonyPatch(typeof(PhysicsManager), nameof(PhysicsManager.Update))]
    public static class PhysicsManagerPatch
    {
        [HarmonyPostfix]
        public static void Postfix(PhysicsManager __instance)
        {
            serverUtils.CheckPuckAmount(50);
        }
    }
}