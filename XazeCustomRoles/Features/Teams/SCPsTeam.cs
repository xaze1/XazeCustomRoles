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

public class SCPsTeam : CustomTeamBase
{
    public override string DisplayName => "SCPs";
    public override string TeamId => nameof(Team.SCPs);
    public override Team BaseGameTeam => Team.SCPs;
    public override ICustomFaction Faction => new SCPFaction();

    public override bool IsHostileTo(ICustomTeam other)
    {
        if (other is SCPsTeam)
            return false;
        
        return base.IsHostileTo(other);
    }
}