// // Copyright (c) 2025 xaze_
// //
// // This source code is licensed under the MIT license found in the
// // LICENSE file in the root directory of this source tree.
// //
// // I <3 🦈s :3c

using HarmonyLib;
using Interactables.Interobjects.DoorUtils;
using XazeCustomRoles.Features;

namespace XazeCustomRoles.Patches;

[HarmonyPatchCategory(Loader.CustomRolesPatchGroup)]
[HarmonyPatch(
    typeof(DoorPermissionsPolicy), 
    nameof(DoorPermissionsPolicy.CheckPermissions), 
    [typeof(ReferenceHub), typeof(IDoorPermissionRequester), typeof(PermissionUsed)],
    [ArgumentType.Normal,ArgumentType.Normal, ArgumentType.Out]
    )]
public class DoorPermissionProviderPatch
{
    public static bool Prefix(DoorPermissionsPolicy __instance, ReferenceHub hub, IDoorPermissionRequester requester, ref PermissionUsed callback, ref bool __result)
    {
        callback = null;
        if (__instance.RequiredPermissions == DoorPermissionFlags.None || hub.serverRoles.BypassMode)
        {
            __result = true;
            return false;
        }

        if (hub.roleManager.CurrentRole is IDoorPermissionProvider currentRole)
        {
            __result = __instance.CheckPermissions(currentRole, requester, out callback);
            return false;
        }
        
        if (CustomRoleManager.TryGet(hub, out var manager) && manager.CurrentRole is IDoorPermissionProvider customRole)
        {
            __result = __instance.CheckPermissions(customRole, requester, out callback);
            return false;
        }
        
        var curInstance = hub.inventory.CurInstance;
        __result = curInstance != null && curInstance is IDoorPermissionProvider provider && __instance.CheckPermissions(provider, requester, out callback);
        return false;
    }
}