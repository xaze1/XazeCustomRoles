// // Copyright (c) 2025 xaze_
// //
// // This source code is licensed under the MIT license found in the
// // LICENSE file in the root directory of this source tree.
// //
// // I <3 🦈s :3c

using PlayerRoles;
using XazeCustomRoles.Features.Factions;
using XazeCustomRoles.Features.Teams;
using XazeCustomRoles.Interfaces;

namespace XazeCustomRoles.Extensions;

public static class TeamAndFactionExtensions
    {
        public static ICustomTeam ToCustomTeam(this Team team)
        {
            return team switch
            {
                Team.SCPs => new SCPsTeam(),
                Team.FoundationForces => new FoundationForcesTeam(),
                Team.ChaosInsurgency => new ChaosInsurgencyTeam(),
                Team.Scientists => new ScientistsTeam(),
                Team.ClassD => new ClassDTeam(),
                Team.Dead => new DeadTeam(),
                Team.Flamingos => new FlamingosTeam(),
                _ => new OtherAliveTeam()
            };
        }

        public static Team ToTeam(this ICustomTeam team)
        {
            return team switch
            {
                SCPsTeam => Team.SCPs,
                FoundationForcesTeam => Team.FoundationForces,
                ChaosInsurgencyTeam => Team.ChaosInsurgency,
                ScientistsTeam => Team.Scientists,
                ClassDTeam => Team.ClassD,
                DeadTeam => Team.Dead,
                FlamingosTeam => Team.Flamingos,
                _ => Team.OtherAlive
            };
        }

        public static ICustomFaction ToCustomFaction(this Faction faction)
        {
            return faction switch
            {
                Faction.SCP => new SCPFaction(),
                Faction.FoundationStaff => new FoundationStaffFaction(),
                Faction.FoundationEnemy => new FoundationEnemyFaction(),
                Faction.Flamingos => new FlamingosFaction(),
                _ => new UnclassifiedFaction()
            };
        }

        public static Faction ToFaction(this ICustomFaction faction)
        {
            return faction switch
            {
                SCPFaction => Faction.SCP,
                FoundationStaffFaction => Faction.FoundationStaff,
                FoundationEnemyFaction => Faction.FoundationEnemy,
                FlamingosFaction => Faction.Flamingos,
                _ => Faction.Unclassified
            };
        }
    }