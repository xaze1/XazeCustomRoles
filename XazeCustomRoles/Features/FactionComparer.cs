// // Copyright (c) 2025 xaze_
// //
// // This source code is licensed under the MIT license found in the
// // LICENSE file in the root directory of this source tree.
// //
// // I <3 🦈s :3c

using System.Collections.Generic;
using XazeCustomRoles.Interfaces;

namespace XazeCustomRoles.Features;

public class FactionComparer : IEqualityComparer<ICustomFaction>
{
    public bool Equals(ICustomFaction x, ICustomFaction y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;
        return x.FactionId == y.FactionId;
    }

    public int GetHashCode(ICustomFaction obj)
    {
        return (obj.FactionId != null ? obj.FactionId.GetHashCode() : 0);
    }
}