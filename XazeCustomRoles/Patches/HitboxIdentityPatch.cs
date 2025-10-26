// Copyright (c) 2025 xaze_
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
// 
// I <3 🦈s :3c

using HarmonyLib;
using XazeAPI.API.AudioCore.FakePlayers;

namespace XazeCustomRoles.Patches
{
    [HarmonyPatchCategory(Loader.CustomRolesPatchGroup)]
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
