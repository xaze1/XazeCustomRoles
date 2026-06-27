// // Copyright (c) 2025 xaze_
// //
// // This source code is licensed under the MIT license found in the
// // LICENSE file in the root directory of this source tree.
// //
// // I <3 🦈s :3c

using System.Drawing;
using PlayerRoles;
using XazeCustomRoles.Features.Factions;
using XazeCustomRoles.Interfaces;

namespace XazeCustomRoles.Features.Teams;

public class ClassDTeam : CustomTeamBase
{
    public override string DisplayName => "Class-D";
    public override Color DisplayColor => Color.Orange;
    public override string TeamId => nameof(Team.ClassD);
    public override Team BaseGameTeam => Team.ClassD;
    public override ICustomFaction Faction => new FoundationEnemyFaction();

    public override string GetCassieName(int count)
    {
        return "CLASS D PERSONNEL";
    }
}