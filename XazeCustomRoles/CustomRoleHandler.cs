// Copyright (c) 2025 xaze_
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
// 
// I <3 🦈s :3c

using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;

namespace XazeCustomRoles
{
    public class CustomRoleHandler : CustomEventsHandler
    {
        public static readonly CustomRoleHandler Instance;
        static CustomRoleHandler()
        {
            Instance = new();
        }

        public override void OnPlayerChangingRole(PlayerChangingRoleEventArgs ev)
        {
            if (ev?.Player == null)
            {
                return;
            }

            if (ev.NewRole == ev.OldRole.RoleTypeId)
            {
                return;
            }

            CustomRoleManager.DisableRole(ev.Player);
        }

        public override void OnPlayerDying(PlayerDyingEventArgs ev)
        {
            if (ev?.Player == null)
            {
                return;
            }

            CustomRoleManager.DisableRole(ev.Player);
        }

        public override void OnPlayerLeft(PlayerLeftEventArgs ev)
        {
            if (ev?.Player == null)
            {
                return;
            }
            
            CustomRoleManager.DisableRole(ev.Player);
        }
    }
}
