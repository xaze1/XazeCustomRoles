// Copyright (c) 2025 xaze_
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
// 
// I <3 🦈s :3c

using CustomPlayerEffects;
using HarmonyLib;
using PlayerRoles;
using PlayerStatsSystem;
using XazeCustomRoles.Features;

namespace XazeCustomRoles.Patches;

[HarmonyPatchCategory(Loader.CustomRolesPatchGroup)]
[HarmonyPatch(typeof(AttackerDamageHandler), nameof(AttackerDamageHandler.ProcessDamage))]
public class AttackerHandlerPatch
{
    public static bool Prefix(AttackerDamageHandler __instance, ReferenceHub ply)
    {
        if (__instance.DisableSpawnProtect(__instance.Attacker.Hub, ply))
        {
            __instance.Damage = 0.0f;
        }
        else
        {
            foreach (var allEffect in ply.playerEffectsController.AllEffects)
            {
                if (allEffect is not IFriendlyFireModifier friendlyFireModifier || !allEffect.IsEnabled ||
                    !friendlyFireModifier.AllowFriendlyFire(__instance.Damage, __instance, __instance.Hitbox)) continue;
                
                __instance.ForceFullFriendlyFire = true;
                break;
            }
            if ((int) ply.networkIdentity.netId == (int) __instance.Attacker.NetId || __instance.ForceFullFriendlyFire)
            {
                if (__instance is { AllowSelfDamage: false, ForceFullFriendlyFire: false })
                {
                    __instance.Damage = 0.0f;
                    return false;
                }
                __instance.IsSuicide = true;
            }
            else if (!CustomRoleManager.IsEnemy(FootprintPatch.SavedRoles.GetValueSafe(__instance.Attacker), ply))
            {
                __instance.Damage *= AttackerDamageHandler._ffMultiplier;
                __instance.IsFriendlyFire = true;
            }
            
            foreach (StatusEffectBase allEffect in ply.playerEffectsController.AllEffects)
            {
                if (allEffect is IDamageModifierEffect { DamageModifierActive: true } damageModifierEffect)
                    __instance.Damage *= damageModifierEffect.GetDamageModifier(__instance.Damage, __instance, __instance.Hitbox);
            }
        }

        return false;
    }
}