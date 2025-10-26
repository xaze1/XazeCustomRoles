// Copyright (c) 2025 xaze_
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
// 
// I <3 🦈s :3c

using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Mirror;
using NorthwoodLib.Pools;
using PlayerRoles;
using PlayerStatsSystem;

namespace XazeCustomRoles.Patches;

[HarmonyPatchCategory(Loader.CustomRolesPatchGroup)]
[HarmonyPatch(typeof(AttackerDamageHandler), nameof(AttackerDamageHandler.ProcessDamage))]
public class AttackerHandlerPatch
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        // Rent a list from the shared pool, initialized with old instructions
        var code = ListPool<CodeInstruction>.Shared.Rent(instructions);
        var newInstructions = ListPool<CodeInstruction>.Shared.Rent();

        var customMethod = AccessTools.Method(typeof(AttackerHandlerPatch), nameof(AttackerHandlerPatch.CustomIsEnemyCheck));

        int startIndex = -1;
        int endIndex = -1;

        // Find start and end of the block to replace
        for (int i = 0; i < code.Count - 1; i++)
        {
            if (startIndex == -1 &&
                code[i].opcode == OpCodes.Ldarg_1 && // loading ply
                code[i + 1].Calls(AccessTools.PropertyGetter(typeof(NetworkIdentity), nameof(NetworkIdentity.netId))))
            {
                startIndex = i;
            }

            if (startIndex != -1 &&
                code[i].Calls(AccessTools.Method(typeof(StandardDamageHandler), nameof(StandardDamageHandler.ProcessDamage))))
            {
                endIndex = i - 1;
                break;
            }
        }

        if (startIndex == -1 || endIndex == -1)
        {
            UnityEngine.Debug.LogError("[Harmony] Could not locate conditional block in AttackerDamageHandler.ProcessDamage.");

            foreach (var ci in code)
                yield return ci;

            ListPool<CodeInstruction>.Shared.Return(code);
            ListPool<CodeInstruction>.Shared.Return(newInstructions);
            yield break;
        }

        // Copy instructions before replaced block
        for (int i = 0; i < startIndex; i++)
            newInstructions.Add(code[i]);

        // Insert call to our custom method:
        // if (!CustomDamagePatch.CustomDamageHandlerPatch(this, ply)) return;

        var continueLabel = new Label();

        newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0)); // this
        newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_1)); // ply
        newInstructions.Add(new CodeInstruction(OpCodes.Call, customMethod));
        newInstructions.Add(new CodeInstruction(OpCodes.Brtrue_S, continueLabel));
        newInstructions.Add(new CodeInstruction(OpCodes.Ret));

        // Mark label at instruction after replaced block
        code[endIndex + 1].labels.Add(continueLabel);

        // Copy remaining instructions
        for (int i = endIndex + 1; i < code.Count; i++)
            newInstructions.Add(code[i]);

        // Yield all instructions
        foreach (var ci in newInstructions)
            yield return ci;

        // Return lists to pool
        ListPool<CodeInstruction>.Shared.Return(code);
        ListPool<CodeInstruction>.Shared.Return(newInstructions);
    }

    public static bool CustomIsEnemyCheck(AttackerDamageHandler __instance, ReferenceHub ply)
    {
        if (ply.networkIdentity.netId == __instance.Attacker.NetId || __instance.ForceFullFriendlyFire)
        {
            if (!__instance.AllowSelfDamage && !__instance.ForceFullFriendlyFire)
            {
                __instance.Damage = 0f;
                return false; // skip processing
            }

            __instance.IsSuicide = true;
        }
        else if (__instance is ExplosionDamageHandler or Scp018DamageHandler &&
                 !HitboxIdentity.IsEnemy(__instance.Attacker.Role, ply.GetRoleId()))
        {
            __instance.Damage *= AttackerDamageHandler._ffMultiplier;
            __instance.IsFriendlyFire = true;
        }
        else if (!HitboxIdentity.IsEnemy(__instance.Attacker.Hub, ply))
        {
            __instance.Damage *= AttackerDamageHandler._ffMultiplier;
            __instance.IsFriendlyFire = true;
        }

        return true;
    }
}