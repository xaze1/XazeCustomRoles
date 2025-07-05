using HarmonyLib;
using XazeAPI.API.AudioCore.FakePlayers;

namespace XazeCustomRoles.Patches
{
    [HarmonyPatchCategory(CustomRoleManager.CustomRolesPatchGroup)]
    [HarmonyPatch(typeof(HitboxIdentity), nameof(HitboxIdentity.IsEnemy), [typeof(ReferenceHub), typeof(ReferenceHub)])]
    public class HitboxIdentityPatch
    {
        public static bool Prefix(HitboxIdentity __instance, ref ReferenceHub attacker, ref ReferenceHub victim, ref bool __result)
        {
            if (AudioManager.ActiveFakes.Contains(victim))
            {
                __result = false;
                return false;
            }

            __result = CustomRoleManager.IsEnemy(attacker, victim);
            return false;
        }
    }
}
