// // Copyright (c) 2025 xaze_
// //
// // This source code is licensed under the MIT license found in the
// // LICENSE file in the root directory of this source tree.
// //
// // I <3 🦈s :3c

using LabApi.Features.Wrappers;
using PlayerRoles;
using XazeCustomRoles.Interfaces;

namespace XazeCustomRoles.Features.Factions;

public class SCPFaction : ICustomFaction
{
    public string DisplayName => "SCP";
    public string FactionId => nameof(Faction.SCP);
    public Faction BaseGameFaction => Faction.SCP;
}