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
using NorthwoodLib.Pools;
using PlayerRoles;
using PlayerStatsSystem;

namespace XazeCustomRoles.Patches;

[HarmonyPatchCategory(CustomRoleManager.CustomRolesPatchGroup)]
[HarmonyPatch(typeof(AttackerDamageHandler), nameof(AttackerDamageHandler.ProcessDamage))]
public class AttackerHandlerPatch
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

        // Locate the call to HitboxIdentity.IsEnemy(RoleTypeId, RoleTypeId)
        int index = newInstructions.FindIndex(i =>
            i.opcode == OpCodes.Call &&
            i.operand is MethodInfo mi &&
            mi.Name == "IsEnemy" &&
            mi.DeclaringType?.Name == "HitboxIdentity"
        );

        if (index != -1)
        {
            // Remove original IsEnemy check and its operands
            // Backtrack to ldarg.0 (4 instructions before IsEnemy)
            int start = index - 4;

            newInstructions.RemoveRange(start, 6); // ldarg.0, call Attacker, ldfld Role, ldarg.1, call GetRoleId, call IsEnemy

            // Also remove the brtrue.s (branch if IsEnemy was true)
            if (newInstructions[start].opcode == OpCodes.Brtrue_S)
                newInstructions.RemoveAt(start);

            // Inject custom check instead
            newInstructions.InsertRange(start, new[]
            {
                new CodeInstruction(OpCodes.Ldarg_0), // __instance
                new CodeInstruction(OpCodes.Ldarg_1), // ply
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(AttackerHandlerPatch), nameof(CustomIsEnemyCheck))),
                new CodeInstruction(OpCodes.Brtrue_S, newInstructions[start + 1].operand), // mimic original jump if check is true
            });
        }

        foreach (var code in newInstructions)
            yield return code;

        ListPool<CodeInstruction>.Shared.Return(newInstructions);
    }

    public static void CustomIsEnemyCheck(AttackerDamageHandler __instance, ReferenceHub ply)
    {
        if (__instance is ExplosionDamageHandler or Scp018DamageHandler &&
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
    }

}