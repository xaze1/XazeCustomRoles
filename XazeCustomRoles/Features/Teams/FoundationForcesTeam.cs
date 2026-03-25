// // Copyright (c) 2025 xaze_
// //
// // This source code is licensed under the MIT license found in the
// // LICENSE file in the root directory of this source tree.
// //
// // I <3 🦈s :3c

using PlayerRoles;
using XazeCustomRoles.Features.Factions;
using XazeCustomRoles.Interfaces;

namespace XazeCustomRoles.Features.Teams;

public class FoundationForcesTeam : CustomTeamBase
{
    public override string DisplayName => "Foundation Forces";
    public override string TeamId => nameof(Team.FoundationForces);
    public override Team BaseGameTeam => Team.FoundationForces;
    public override ICustomFaction Faction => new FoundationStaffFaction();
}