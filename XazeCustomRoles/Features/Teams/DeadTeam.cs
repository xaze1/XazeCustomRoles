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

public class DeadTeam : CustomTeamBase
{
    public override string DisplayName => "Dead";
    public override string TeamId => nameof(Team.Dead);
    public override Team BaseGameTeam => Team.Dead;
    public override ICustomFaction Faction => new UnclassifiedFaction();
    
    public override bool IsHostileTo(ICustomTeam other)
    {
        return false;
    }
}