// // Copyright (c) 2025 xaze_
// //
// // This source code is licensed under the MIT license found in the
// // LICENSE file in the root directory of this source tree.
// //
// // I <3 🦈s :3c

using HarmonyLib;
using LabApi.Features.Wrappers;
using PlayerRoles;
using PlayerRoles.PlayableScps.HumanTracker;
using XazeCustomRoles.Features;
using XazeCustomRoles.Features.Teams;

namespace XazeCustomRoles.Patches;

[HarmonyPatchCategory(Loader.CustomRolesPatchGroup)]
[HarmonyPatch(typeof(LastHumanTracker), nameof(LastHumanTracker.TryGetLastTarget))]
public static class LastHumanPatch
{
    public static bool Prefix(ref ReferenceHub lastTarget, bool __result)
    {
        lastTarget = null;
        int humans = 0;
        int scps = 0;
        foreach (var plr in Player.ReadyList)
        {
            var team = CustomRoleManager.GetTeam(plr);
            if (team is SCPsTeam)
                scps++;
            else if (plr.IsAlive && (plr.Role != RoleTypeId.Tutorial || CustomRoleManager.IsCustomRole(plr)))
            {
                lastTarget = plr.ReferenceHub;
                humans++;
            }
        }
        return humans == 1 && scps > 0;
    }
}