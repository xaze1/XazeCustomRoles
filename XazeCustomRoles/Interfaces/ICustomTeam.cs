// // Copyright (c) 2025 xaze_
// //
// // This source code is licensed under the MIT license found in the
// // LICENSE file in the root directory of this source tree.
// //
// // I <3 🦈s :3c

using PlayerRoles;

namespace XazeCustomRoles.Interfaces;

public interface ICustomTeam
{
    public string DisplayName { get; }
    public string TeamId { get; }
    public Team BaseGameTeam { get; }
    public ICustomFaction Faction { get; }
    
    public bool IsHostileTo(ICustomTeam other);
}