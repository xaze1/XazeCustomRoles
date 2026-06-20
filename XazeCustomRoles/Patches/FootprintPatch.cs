// // Copyright (c) 2025 xaze_
// //
// // This source code is licensed under the MIT license found in the
// // LICENSE file in the root directory of this source tree.
// //
// // I <3 🦈s :3c

using System.Collections.Generic;
using Footprinting;
using HarmonyLib;
using XazeAPI.API;
using XazeCustomRoles.Features;
using XazeCustomRoles.Interfaces;

namespace XazeCustomRoles.Patches;

[HarmonyPatchCategory(Loader.CustomRolesPatchGroup)]
[HarmonyPatch(typeof(Footprint), MethodType.Constructor, [typeof(ReferenceHub)])]
public class FootprintPatch
{
    public static Dictionary<Footprint, ICustomTeam> SavedRoles { get; } = new();
    
    public static void Postfix(Footprint __instance, ReferenceHub hub)
    {
        //Logging.Debug("FootprintPatch.Prefix was called by", hub.nicknameSync.MyNick);
        if (__instance._serial == 0)
            return;
        
        SavedRoles.Add(__instance, CustomRoleManager.GetTeam(hub));
    }
}