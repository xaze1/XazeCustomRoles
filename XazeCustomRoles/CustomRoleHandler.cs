using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;

namespace XazeCustomRoles
{
    public class CustomRoleHandler : CustomEventsHandler
    {
        public static readonly CustomRoleHandler Instance = new();

        public override void OnPlayerChangingRole(PlayerChangingRoleEventArgs ev)
        {
            if (ev.NewRole == ev.OldRole.RoleTypeId)
            {
                return;
            }

            CustomRoleManager.DisableRole(ev.Player);
        }

        public override void OnPlayerDying(PlayerDyingEventArgs ev)
        {
            CustomRoleManager.DisableRole(ev.Player);
        }

        public override void OnPlayerLeft(PlayerLeftEventArgs ev)
        {
            CustomRoleManager.DisableRole(ev.Player);
        }
    }
}
