// Copyright (c) 2025 xaze_
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
// 
// I <3 🦈s :3c

using System.Linq;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using PlayerStatsSystem;
using XazeCustomRoles.Features;
using XazeCustomRoles.Features.Factions;
using XazeCustomRoles.Interfaces;

namespace XazeCustomRoles
{
    public class CustomRoleHandler : CustomEventsHandler
    {
        public static readonly CustomRoleHandler Instance;
        static CustomRoleHandler()
        {
            Instance = new();
        }

        private static bool RoundCanEnd;
        private static ICustomFaction WinningFaction;
        
        public override void OnServerRoundEnding(RoundEndingEventArgs ev)
        {
            ev.IsAllowed = RoundCanEnd;
            if (WinningFaction is null or BaseGameFaction)
                return;

            ev.LeadingTeam = WinningFaction.WinTeam;
        }

        public override void OnServerRoundEndingConditionsCheck(RoundEndingConditionsCheckEventArgs ev)
        {
            ev.CanEnd = RoundCanEnd;
        }

        public override void OnPlayerChangingRole(PlayerChangingRoleEventArgs ev)
        {
            if (ev.OldRole == null || ev.NewRole == ev.OldRole.RoleTypeId)
            {
                return;
            }

            CustomRoleManager.DisableRole(ev.Player);
        }

        public override void OnPlayerChangedRole(PlayerChangedRoleEventArgs ev)
        {
            if (ev.NewRole == null || ev.NewRole.RoleTypeId == ev.OldRole)
            {
                return;
            }
            
            RoundCanEnd = ShouldEndRound(out WinningFaction);
        }

        public override void OnPlayerDying(PlayerDyingEventArgs ev)
        {
            if (ev?.Player == null)
            {
                return;
            }

            if (CustomRoleManager.TryGet(ev.Player, out var manager) &&
                !manager.CurrentRole.OnDying(ev))
                return;

            CustomRoleManager.DisableRole(ev.Player);
        }

        public override void OnPlayerDeath(PlayerDeathEventArgs ev)
        {
            if (ev?.Player == null)
            {
                return;
            }
            
            RoundCanEnd = ShouldEndRound(out WinningFaction);
        }

        public override void OnPlayerSpawned(PlayerSpawnedEventArgs ev)
        {
            if (ev?.Player == null)
            {
                return;
            }

            RoundCanEnd = ShouldEndRound(out WinningFaction);
        }

        public override void OnPlayerLeft(PlayerLeftEventArgs ev)
        {
            if (ev?.Player == null)
            {
                return;
            }
            
            CustomRoleManager.DisableRole(ev.Player);
            RoundCanEnd = ShouldEndRound(out WinningFaction);
        }

        private static bool ShouldEndRound(out ICustomFaction winningFaction)
        {
            var alivePlayers = Player.ReadyList.Where(p => p.IsAlive).ToList();
            var aliveFactions = alivePlayers.Select(p => CustomRoleManager.GetTeam(p).Faction).Distinct(new FactionComparer()).ToList();

            winningFaction = aliveFactions.OrderByDescending(f => f.WinningWeight).FirstOrDefault();
            var outcome =
                !alivePlayers.Any(p1 => alivePlayers.Any(p2 => p1 != p2 && CustomRoleManager.IsEnemy(p1, p2)));
            
            //Logging.Debug("AlivePlayers:", alivePlayers.Count, "| AliveFactions:", aliveFactions.Count, "| RoundCanEnd:", outcome, "| WinningFaction:", winningFaction, "| WinTeam:", winningFaction?.WinTeam?? RoundSummary.LeadingTeam.Draw);
            return outcome;
        }
    }
}
