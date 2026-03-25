// Copyright (c) 2025 xaze_
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
// 
// I <3 🦈s :3c

using System.Drawing;
using InventorySystem;
using LabApi.Features.Wrappers;
using PlayerRoles;
using PlayerRoles.FirstPersonControl.Spawnpoints;
using PlayerStatsSystem;
using XazeAPI.API.Enums;
using XazeAPI.API.Extensions;
using XazeCustomRoles.Features.Teams;
using XazeCustomRoles.Interfaces;

namespace XazeCustomRoles
{
    public abstract class CustomRoleBase : IHealthbarRole
    {
        // Team
        public virtual ICustomTeam Team => new OtherAliveTeam();
        public virtual ICustomFaction Faction => Team.Faction;

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
        public virtual void Init(Player owner, PlayerRoleBase roleBase)
        {
            BaseRole = roleBase;
            owner.MaxHealth = MaxHealth;
            owner.Health = MaxHealth;

            if (this is ISpawnBroadcast spawn)
            {
                owner.SendBroadcast(spawn.SpawnBroadcast, spawn.BroadcastDuration, Broadcast.BroadcastFlags.Normal, spawn.ClearPrevious);
            }

            if (this is ICustomNameRole)
            {
                owner.CustomInfo = RoleName;
            }
        }

        public virtual void DisableRole()
        {
            if (this is ICustomNameRole)
            {
                Owner?.CustomInfo = null;
            }
        }
    }
}
