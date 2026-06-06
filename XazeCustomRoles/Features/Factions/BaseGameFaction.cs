// // Copyright (c) 2025 xaze_
// //
// // This source code is licensed under the MIT license found in the
// // LICENSE file in the root directory of this source tree.
// //
// // I <3 🦈s :3c

using XazeCustomRoles.Interfaces;

namespace XazeCustomRoles.Features.Factions;

public abstract class BaseGameFaction : ICustomFaction
{
    public abstract string DisplayName { get; }
    public abstract string FactionId { get; }
    public abstract int WinningWeight { get; }
    public abstract RoundSummary.LeadingTeam WinTeam { get; }
}