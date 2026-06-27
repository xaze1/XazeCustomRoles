// // Copyright (c) 2025 xaze_
// //
// // This source code is licensed under the MIT license found in the
// // LICENSE file in the root directory of this source tree.
// //
// // I <3 🦈s :3c

using System.Drawing;
using PlayerRoles;

namespace XazeCustomRoles.Features.Factions;

public class SCPFaction : BaseGameFaction
{
    public override string DisplayName => "SCP";
    public override Color DisplayColor => Color.Red;
    public override string FactionId => nameof(Faction.SCP);
    public override int WinningWeight => 2;
    public override RoundSummary.LeadingTeam WinTeam => RoundSummary.LeadingTeam.Anomalies;

    public override string GetCassieName(int count)
    {
        return "SCP " + (count == 1? "SUBJECT" : "SUBJECTS");
    }
}