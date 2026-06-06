// // Copyright (c) 2025 xaze_
// //
// // This source code is licensed under the MIT license found in the
// // LICENSE file in the root directory of this source tree.
// //
// // I <3 🦈s :3c

using LabApi.Features.Wrappers;
using PlayerRoles;
using XazeCustomRoles.Interfaces;

namespace XazeCustomRoles.Features.Factions;

public class FoundationEnemyFaction : BaseGameFaction
{
    public override string DisplayName => "Foundation Enemy";
    public override string FactionId => nameof(Faction.FoundationEnemy);
    public override int WinningWeight => 1;
    public override RoundSummary.LeadingTeam WinTeam => RoundSummary.LeadingTeam.ChaosInsurgency;
}