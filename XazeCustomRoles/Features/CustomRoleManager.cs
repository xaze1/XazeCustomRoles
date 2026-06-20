// Copyright (c) 2025 xaze_
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
// 
// I <3 🦈s :3c

using System;
using System.Collections.Generic;
using Interactables.Interobjects.DoorUtils;
using InventorySystem;
using LabApi.Features.Wrappers;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.PlayableScps;
using PlayerRoles.PlayableScps.Scp049;
using PlayerRoles.Voice;
using UnityEngine;
using XazeAPI.API.AudioCore.FakePlayers;
using XazeAPI.API.Helpers;
using XazeCustomRoles.Extensions;
using XazeCustomRoles.Features.Teams;
using XazeCustomRoles.Interfaces;
using Object = UnityEngine.Object;

namespace XazeCustomRoles.Features
{
    public class CustomRoleManager : MonoBehaviour
    {
        public static readonly Dictionary<uint, CustomRoleManager> ActiveManagers = new();

        public bool _hubSet;
        public ReferenceHub _hub;
        public CustomRoleBase _curRole;
        public bool _anySet;

        public CustomRoleBase? CurrentRole
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
                plr.ClearItems();
                Inventory inventory = plr.Inventory;
                foreach (var keyValuePair in roleInstance.StartingInventory.Ammo)
                {
                    inventory.ServerSetAmmo(keyValuePair.Key, keyValuePair.Value);
                }

                foreach (ItemType item in roleInstance.StartingInventory.Items)
                {
                    plr.GiveFirearmWithAttachments(item);
                }
                roleInstance.GiveCustomItems();
            }

            if (!spawnFlags.HasFlag(RoleSpawnFlags.UseSpawnpoint) || roleInstance.Spawnpoint == null ||
                !roleInstance.Spawnpoint.TryGetSpawnpoint(out Vector3 spawnPos, out float spawnRot)) return;

            if (roleInstance is ICustomVoiceModule cvm && plr.RoleBase is FpcStandardRoleBase fpc && cvm.VoiceModuleType.IsSubclassOf(typeof(VoiceModuleBase)))
            {
                fpc.gameObject.AddComponent(cvm.VoiceModuleType);
            }
            
            plr.Position = spawnPos;
            plr.LookRotation = new Vector2(0, spawnRot);
        }

        public static void SetRole<T>(ReferenceHub hub, RoleChangeReason reason, RoleSpawnFlags spawnFlags) where T : CustomRoleBase => SetRole<T>(Player.Get(hub), reason, spawnFlags);

        public static void DisableRole(Player plr)
        {
            if (!TryGet(plr, out var manager))
            {
                return;
            }

            if (!manager._anySet)
            {
                return;
            }
            
            if (manager.CurrentRole is ICustomVoiceModule cvm && plr.RoleBase is FpcStandardRoleBase fpc && cvm.VoiceModuleType.IsSubclassOf(typeof(VoiceModuleBase)))
            {
                Destroy(fpc.gameObject.GetComponent(cvm.VoiceModuleType));
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

        public static ICustomTeam GetTeam(ReferenceHub player)
        {
            if (player == null || FakeManager.ActiveFakes.Contains(player))
            {
                return new DeadTeam();
            }

            if (ActiveManagers.TryGetValue(player.netId, out var manager) && manager._anySet)
            {
                return manager.CurrentRole?.Team?? new DeadTeam();
            }

            return player.GetTeam().ToCustomTeam();
        }
        
        public static ICustomTeam GetTeam(Player player) => GetTeam(player.ReferenceHub);

        public static bool IsEnemy(Player attacker, Player target) => IsEnemy(attacker.ReferenceHub, target.ReferenceHub);
        public static bool IsEnemy(Player attacker, ICustomTeam target) => IsEnemy(attacker.ReferenceHub, target);
        public static bool IsEnemy(ICustomTeam attacker, Player target) => IsEnemy(attacker, target.ReferenceHub);
        
        public static bool IsEnemy(ReferenceHub attacker, ReferenceHub target)
        {
            return IsEnemy(GetTeam(attacker), GetTeam(target));
        }
        
        public static bool IsEnemy(ReferenceHub attacker, ICustomTeam target)
        {
            return IsEnemy(GetTeam(attacker), target);
        }
        
        public static bool IsEnemy(ICustomTeam attacker, ReferenceHub target)
        {
            return IsEnemy(attacker, GetTeam(target));
        }

        public static bool IsEnemy(ICustomTeam attacker, ICustomTeam target)
        {
            if (attacker is DeadTeam || target is DeadTeam)
            {
                return false;
            }
            
            return attacker.IsHostileTo(target) || target.IsHostileTo(attacker);
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
