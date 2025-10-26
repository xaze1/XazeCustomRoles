// Copyright (c) 2025 xaze_
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
// 
// I <3 🦈s :3c

using System;
using System.Collections.Generic;
using HarmonyLib;
using InventorySystem;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using MEC;
using PlayerRoles;
using UnityEngine;
using XazeAPI.API;
using XazeAPI.API.AudioCore.FakePlayers;
using XazeAPI.API.Enums;
using XazeAPI.API.Events;
using XazeAPI.API.Extensions;
using XazeAPI.API.Helpers;

namespace XazeCustomRoles
{
    public class CustomRoleManager : MonoBehaviour
    {
        public static readonly Dictionary<uint, CustomRoleManager> ActiveManagers = new();

        public bool _hubSet;
        public ReferenceHub _hub;
        public CustomRoleBase _curRole;
        public bool _anySet;

        public CustomRoleBase CurrentRole
        {
            get
            {
                if (!_anySet)
                {
                    return null;
                }

                return _curRole;
            }
            set
            {
                _curRole = value;
                _anySet = true;
            }
        }

        public ReferenceHub Hub
        {
            get
            {
                if (!_hubSet && ReferenceHub.TryGetHub(gameObject, out _hub))
                {
                    _hubSet = true;
                }
                return _hub;
            }
        }

        public static void SetRole<T>(Player plr, RoleChangeReason reason, RoleSpawnFlags spawnFlags) where T : CustomRoleBase
        {
            if (!TryGet(plr.ReferenceHub, out var manager))
            {
                return;
            }

            if (manager._anySet)
            {
                manager.CurrentRole.DisableRole();
                manager._anySet = false;
                manager._curRole = null;
            }

            T roleInstance = Activator.CreateInstance<T>();
            plr.SetRole(roleInstance.Role, reason, RoleSpawnFlags.None);
            manager.CurrentRole = roleInstance;
            roleInstance.Init(plr, plr.RoleBase);

            if (spawnFlags.HasFlag(RoleSpawnFlags.AssignInventory))
            {
                Inventory inventory = plr.Inventory;
                foreach (KeyValuePair<ItemType, ushort> keyValuePair in roleInstance.StartingInventory.Ammo)
                {
                    inventory.ServerAddAmmo(keyValuePair.Key, keyValuePair.Value);
                }

                foreach (ItemType item in roleInstance.StartingInventory.Items)
                {
                    plr.GiveFirearmWithAttachments(item);
                }
            }

            if (spawnFlags.HasFlag(RoleSpawnFlags.UseSpawnpoint) && roleInstance.Spawnpoint != null && roleInstance.Spawnpoint.TryGetSpawnpoint(out Vector3 spawnPos, out float spawnRot))
            {
                plr.Position = spawnPos;
                plr.LookRotation = new Vector2(0, spawnRot);
            }
        }

        public static void SetRole<T>(ReferenceHub hub, RoleChangeReason reason, RoleSpawnFlags spawnFlags) where T : CustomRoleBase => SetRole<T>(Player.Get(hub), reason, spawnFlags);

        public static void DisableRole(Player plr)
        {
            if (!TryGet(plr?.ReferenceHub, out var manager))
            {
                return;
            }

            if (!manager._anySet)
            {
                return;
            }

            manager.CurrentRole.DisableRole();
            manager._anySet = false;
            manager._curRole = null;
        }
        
        public static void DisableRole(ReferenceHub hub)
        {
            if (!TryGet(hub, out var manager))
            {
                return;
            }

            if (!manager._anySet)
            {
                return;
            }

            manager.CurrentRole.DisableRole();
            manager._anySet = false;
            manager._curRole = null;
        }

        public static CustomTeam GetTeam(ReferenceHub player)
        {
            if (player == null || AudioManager.ActiveFakes.Contains(player))
            {
                return CustomTeam.Dead;
            }

            if (ActiveManagers.TryGetValue(player.netId, out var manager) && manager._anySet)
            {
                return manager.CurrentRole.Team;
            }

            return player.GetTeam().ToCustomTeam();
        }

        public static bool IsEnemy(ReferenceHub attacker, ReferenceHub target)
        {
            return IsEnemy(GetTeam(attacker), GetTeam(target));
        }

        public static bool IsEnemy(CustomTeam attacker, CustomTeam target)
        {
            if (attacker.HasFlag(CustomTeam.Dead) || target.HasFlag(CustomTeam.Dead))
            {
                return false;
            }

            if (attacker.HasFlag(CustomTeam.SCPs) && target.HasFlag(CustomTeam.SCPs))
            {
                return false;
            }

            if (attacker.HasFlag(CustomTeam.Personnel) ^ target.HasFlag(CustomTeam.Personnel))
            {
                return true;
            }

            CustomFaction aFaction = attacker.GetFaction();
            CustomFaction tFaction = target.GetFaction();

            if (aFaction.HasFlag(CustomFaction.Daybreak) ^ tFaction.HasFlag(CustomFaction.Daybreak))
            {
                return true;
            }

            if (aFaction.HasFlag(CustomFaction.NullEvent) ^ tFaction.HasFlag(CustomFaction.NullEvent))
            {
                return true;
            }

            return aFaction != tFaction;
        }

        private void Awake()
        {
            ActiveManagers[Hub.netId] = this;
            _anySet = false;
            _curRole = null;
        }

        private void OnDestroy()
        {
            if (_anySet)
            {
                CurrentRole.DisableRole();
                _curRole = null;
                _anySet = false;
            }

            ActiveManagers.Remove(Hub.netId);
        }

        public static bool IsCustomRole(Player plr) => IsCustomRole(plr.ReferenceHub);
        public static bool IsCustomRole(ReferenceHub hub)
        {
            return ActiveManagers.TryGetValue(hub.netId, out var manager) && manager._anySet;
        }

        public static CustomRoleManager Get(Player plr) => Get(plr.ReferenceHub);
        public static CustomRoleManager Get(ReferenceHub hub)
        {
            return ActiveManagers[hub.netId];
        }

        public static bool TryGet(Player plr, out CustomRoleManager manager) => TryGet(plr.ReferenceHub, out manager);
        public static bool TryGet(ReferenceHub hub, out CustomRoleManager manager)
        {
            return ActiveManagers.TryGetValue(hub.netId, out manager);
        }
    }
}
