// // Copyright (c) 2025 xaze_
// //
// // This source code is licensed under the MIT license found in the
// // LICENSE file in the root directory of this source tree.
// //
// // I <3 🦈s :3c

using PlayerRoles;
using XazeCustomRoles.Interfaces;

namespace XazeCustomRoles.Features;

public abstract class CustomTeamBase : ICustomTeam
{
    public abstract string DisplayName { get; }
    public abstract string TeamId { get; }
    public abstract Team BaseGameTeam { get; }
    public abstract ICustomFaction Faction { get; }
    
    public virtual bool IsHostileTo(ICustomTeam other)
    {
        return Faction.FactionId != other.Faction.FactionId;
    }
}