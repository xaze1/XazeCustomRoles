// // Copyright (c) 2025 xaze_
// //
// // This source code is licensed under the MIT license found in the
// // LICENSE file in the root directory of this source tree.
// //
// // I <3 🦈s :3c

using System.Drawing;
using PlayerRoles;

namespace XazeCustomRoles.Features.Factions;

public class FoundationStaffFaction : BaseGameFaction
{
    public override string DisplayName => "Foundation Staff";
    public override Color DisplayColor => Color.CornflowerBlue;
    public override string FactionId => nameof(Faction.FoundationStaff);
    public override int WinningWeight => 1;
    public override RoundSummary.LeadingTeam WinTeam => RoundSummary.LeadingTeam.FacilityForces;

    public override string GetCassieName(int count)
    {
        return "NINETAILEDFOX";
    }
}