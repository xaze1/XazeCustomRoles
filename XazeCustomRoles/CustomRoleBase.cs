using System.Drawing;
using InventorySystem;
using LabApi.Features.Wrappers;
using PlayerRoles;
using PlayerRoles.FirstPersonControl.Spawnpoints;
using PlayerStatsSystem;
using XazeAPI.API.Enums;
using XazeAPI.API.Extensions;
using XazeCustomRoles.Interfaces;

namespace XazeCustomRoles
{
    public abstract class CustomRoleBase : IHealthbarRole
    {
        // Team
        public virtual CustomTeam Team => CustomTeam.OtherAlive;
        public virtual CustomFaction Faction => Team.GetFaction();

        // Role
        public string RoleName
        {
            get
            {
                if (this is not ICustomNameRole customNameRole)
                {
                    return RoleTranslations.GetRoleName(Role);
                }

                return customNameRole.CustomRoleName;
            }
        }

        public abstract float MaxHealth { get; }
        public abstract RoleTypeId Role { get; }
        public abstract Color RoleColor { get; }
        public PlayerRoleBase BaseRole;

        // Starting Variables
        public abstract InventoryRoleInfo StartingInventory { get; }
        public abstract ISpawnpointHandler Spawnpoint { get; }

        // Player
        public ReferenceHub LastOwner => BaseRole._lastOwner;
        public Player? Owner => Player.Get(LastOwner);
        public PlayerStats TargetStats => LastOwner.playerStats;


        // Methods
        public virtual void Init(Player Owner, PlayerRoleBase roleBase)
        {
            BaseRole = roleBase;
            Owner.MaxHealth = MaxHealth;
            Owner.Health = MaxHealth;

            if (this is ISpawnBroadcast spawn)
            {
                Owner.SendBroadcast(spawn.SpawnBroadcast, spawn.BroadcastDuration, Broadcast.BroadcastFlags.Normal, spawn.ClearPrevious);
            }

            if (this is ICustomNameRole)
            {
                Owner.CustomInfo = RoleName;
            }
        }

        public virtual void DisableRole()
        {
            if (this is ICustomNameRole)
            {
                Owner.CustomInfo = null;
            }
        }
    }
}
