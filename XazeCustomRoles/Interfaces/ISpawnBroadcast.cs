// Copyright (c) 2025 xaze_
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
// 
// I <3 🦈s :3c

namespace XazeCustomRoles.Interfaces
{
    public interface ISpawnBroadcast
    {
        string SpawnBroadcast { get; }

        ushort BroadcastDuration { get; }

        bool ClearPrevious { get; }
    }
}
