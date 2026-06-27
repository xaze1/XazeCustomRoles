// // Copyright (c) 2025 xaze_
// //
// // This source code is licensed under the MIT license found in the
// // LICENSE file in the root directory of this source tree.
// //
// // I <3 🦈s :3c

using HarmonyLib;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.Voice;
using XazeCustomRoles.Features;
using XazeCustomRoles.Interfaces;

namespace XazeCustomRoles.Patches;

[HarmonyPatchCategory(Loader.CustomRolesPatchGroup)]
[HarmonyPatch(typeof(FpcStandardRoleBase), nameof(FpcStandardRoleBase.VoiceModule), MethodType.Getter)]
public class VoiceModulePatch
{
    public static bool Prefix(FpcStandardRoleBase __instance, ref VoiceModuleBase __result)
    {
        if (!__instance.TryGetOwner(out var hub) || !CustomRoleManager.TryGet(hub, out var manager) || manager.CurrentRole is not ICustomVoiceModule cvm || !cvm.VoiceModuleType.IsSubclassOf(typeof(VoiceModuleBase)))
            return true;
        
        __result = __instance.gameObject.GetComponent<VoiceModuleBase>();
        return false;
    }
}