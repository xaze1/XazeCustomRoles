// // Copyright (c) 2025 xaze_
// //
// // This source code is licensed under the MIT license found in the
// // LICENSE file in the root directory of this source tree.
// //
// // I <3 🦈s :3c

using System;
using HarmonyLib;
using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using MEC;
using XazeAPI.API;
using XazeAPI.API.AudioCore.FakePlayers;
using XazeAPI.API.Events;

namespace XazeCustomRoles;

public class Loader : Plugin
{
    public const string CustomRolesPatchGroup = "XAZE-CustomRoles";

    public override void Enable()
    {
        new CustomRoleManager();
        new CustomRoleHandler();
        
        CustomHandlersManager.RegisterEventsHandler(CustomRoleHandler.Instance);
        try
        {
            Harmony harmonyPatch = new("Xaze-Patches-CustomRoles");
            harmonyPatch.PatchCategory(CustomRolesPatchGroup);

            XazeEvents.PreventHitmarker += OnPreventHitmarker;

            ReferenceHub.OnPlayerAdded += ctx => Timing.CallDelayed(0.1f, () =>
            {
                if (ctx.Mode == CentralAuth.ClientInstanceMode.Host ||
                    ctx.Mode == CentralAuth.ClientInstanceMode.DedicatedServer ||
                    AudioManager.ActiveFakes.Contains(ctx))
                {
                    return;
                }

                ctx.gameObject.AddComponent<CustomRoleManager>();
            });
        }
        catch (Exception ex)
        {
            Logging.Error("[RoleManager] Exception in Static Initializer: " + ex);
        }
    }

    public override void Disable()
    {
    }

    public override string Name => "Xaze-CustomRoles";
    public override string Description => "Custom Roles API for SL";
    public override string Author => "xaze_";
    public override Version Version => new(1, 0, 0);
    public override Version RequiredApiVersion => new(LabApiProperties.CompiledVersion);

    private static void OnPreventHitmarker(PreventHitmarkerEvent args)
    {
        if (!CustomRoleManager.TryGet(args.Target, out var manager) || manager.CurrentRole is not IHitmarkerPreventer preventer ||
            !preventer.TryPreventHitmarker(args.DamageHandler))
        {
            return;
        }

        args.IsAllowed = false;
    }
}