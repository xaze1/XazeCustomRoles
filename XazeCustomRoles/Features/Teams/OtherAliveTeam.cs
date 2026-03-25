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

public class OtherAliveTeam : CustomTeamBase
{
    public override string DisplayName => "Other Alive";
    public override string TeamId => nameof(Team.OtherAlive);
    public override Team BaseGameTeam => Team.OtherAlive;
    public override ICustomFaction Faction => new UnclassifiedFaction();
}