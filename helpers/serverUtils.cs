using UnityEngine;

namespace acolurk_base.helpers;

public static class serverUtils
{

    public static void CheckPuckAmount(int limit)
    {
        PuckManager pm = NetworkBehaviourSingleton<PuckManager>.instance;
        if (pm.pucks.Count > limit) pm.Server_DespawnPuck(pm.pucks._items[0]);
    }
}