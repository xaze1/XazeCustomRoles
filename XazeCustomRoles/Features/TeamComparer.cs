// // Copyright (c) 2025 xaze_
// //
// // This source code is licensed under the MIT license found in the
// // LICENSE file in the root directory of this source tree.
// //
// // I <3 🦈s :3c

using System.Collections.Generic;
using XazeCustomRoles.Interfaces;

namespace XazeCustomRoles.Features;

public class TeamComparer : IEqualityComparer<ICustomTeam>
{
    public bool Equals(ICustomTeam x, ICustomTeam y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;
        
        return x.TeamId == y.TeamId;
    }

    public int GetHashCode(ICustomTeam obj)
    {
        return (obj.TeamId != null ? obj.TeamId.GetHashCode() : 0);
    }
}