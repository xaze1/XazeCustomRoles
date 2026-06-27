// // Copyright (c) 2025 xaze_
// //
// // This source code is licensed under the MIT license found in the
// // LICENSE file in the root directory of this source tree.
// //
// // I <3 🦈s :3c

using System.Drawing;
using PlayerRoles;

namespace XazeCustomRoles.Features.Factions;

public class FlamingosFaction : BaseGameFaction
{
    public override string DisplayName => "Flamingos";
    public override Color DisplayColor => Color.DeepPink;
    public override string FactionId => nameof(Faction.Flamingos);
    public override int WinningWeight => 2;
    public override RoundSummary.LeadingTeam WinTeam => RoundSummary.LeadingTeam.Flamingos;

    public override string GetCassieName(int count)
    {
        return count == 1? "FLAMINGO" : "FLAMINGOS";
    }
}