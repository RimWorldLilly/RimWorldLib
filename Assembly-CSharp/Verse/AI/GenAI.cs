using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200090C RID: 2316
	public static class GenAI
	{
		// Token: 0x06004686 RID: 18054 RVA: 0x001BB858 File Offset: 0x001B9A58
		public static bool MachinesLike(Faction machineFaction, Pawn p)
		{
			return (p.Faction != null || !p.NonHumanlikeOrWildMan() || (p.HostFaction == machineFaction && !p.IsPrisoner)) && (!p.IsPrisoner || p.HostFaction != machineFaction) && (p.Faction == null || !p.Faction.HostileTo(machineFaction));
		}

		// Token: 0x06004687 RID: 18055 RVA: 0x001BB8B4 File Offset: 0x001B9AB4
		public static bool CanUseItemForWork(Pawn p, Thing item)
		{
			return !item.IsForbidden(p) && p.CanReserveAndReach(item, PathEndMode.ClosestTouch, p.NormalMaxDanger(), 1, -1, null, false);
		}

		// Token: 0x06004688 RID: 18056 RVA: 0x001BB8E0 File Offset: 0x001B9AE0
		public static bool CanBeCaptured(this Pawn pawn)
		{
			if (!pawn.RaceProps.Humanlike)
			{
				return false;
			}
			LifeStageDef curLifeStage = pawn.ageTracker.CurLifeStage;
			return (curLifeStage == null || !curLifeStage.claimable) && !pawn.IsSubhuman && (!pawn.InMentalState || pawn.Downed) && (pawn.Faction != Faction.OfPlayer || (pawn.Downed && (pawn.guilt.IsGuilty || pawn.IsPrisonerOfColony)));
		}

		// Token: 0x06004689 RID: 18057 RVA: 0x001BB964 File Offset: 0x001B9B64
		public static bool CanBeArrestedBy(this Pawn pawn, Pawn arrester)
		{
			return pawn.RaceProps.Humanlike && !pawn.IsSubhuman && (!pawn.InAggroMentalState || !pawn.HostileTo(arrester)) && !pawn.HostileTo(Faction.OfPlayer) && (!pawn.IsPrisonerOfColony || !pawn.Position.IsInPrisonCell(pawn.MapHeld)) && (!ModsConfig.AnomalyActive || !Find.Anomaly.IsPawnHypnotized(pawn)) && !pawn.DevelopmentalStage.Baby();
		}

		// Token: 0x0600468A RID: 18058 RVA: 0x001BB9F0 File Offset: 0x001B9BF0
		public static bool InDangerousCombat(Pawn pawn)
		{
			Region root = pawn.GetRegion(RegionType.Set_Passable);
			bool found = false;
			Predicate<Thing> <>9__2;
			RegionTraverser.BreadthFirstTraverse(root, (Region r1, Region r2) => r2.Room == root.Room, delegate(Region r)
			{
				List<Thing> list = r.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn);
				Predicate<Thing> predicate;
				if ((predicate = <>9__2) == null)
				{
					predicate = (<>9__2 = delegate(Thing t)
					{
						Pawn pawn2 = t as Pawn;
						if (pawn2 != null && !pawn2.Downed && (float)(pawn.Position - pawn2.Position).LengthHorizontalSquared < 144f && pawn2.HostileTo(pawn.Faction))
						{
							found = true;
							return true;
						}
						return false;
					});
				}
				return list.Any(predicate);
			}, 9, RegionType.Set_Passable);
			return found;
		}

		// Token: 0x0600468B RID: 18059 RVA: 0x001BBA54 File Offset: 0x001B9C54
		public static IntVec3 RandomRaidDest(IntVec3 raidSpawnLoc, Map map)
		{
			List<ThingDef> allBedDefBestToWorst = RestUtility.AllBedDefBestToWorst;
			List<Building> list = new List<Building>(map.mapPawns.FreeColonistsAndPrisonersSpawnedCount);
			for (int i = 0; i < allBedDefBestToWorst.Count; i++)
			{
				foreach (Building building in map.listerBuildings.AllBuildingsColonistOfDef(allBedDefBestToWorst[i]))
				{
					if (((Building_Bed)building).OwnersForReading.Any<Pawn>() && !building.IsClearableFreeBuilding && map.reachability.CanReach(raidSpawnLoc, building, PathEndMode.OnCell, TraverseMode.PassAllDestroyableThings, Danger.Deadly))
					{
						list.Add(building);
					}
				}
			}
			Building building2;
			if (list.TryRandomElement(out building2))
			{
				return building2.Position;
			}
			IEnumerable<Building> source = from b in map.listerBuildings.allBuildingsColonist
			where !b.def.building.ai_combatDangerous && !b.def.building.isInert && !b.def.building.ai_neverTrashThis && !b.IsClearableFreeBuilding
			select b;
			if (source.Any<Building>())
			{
				for (int j = 0; j < 500; j++)
				{
					IntVec3 intVec = source.RandomElement<Building>().RandomAdjacentCell8Way();
					if (intVec.Walkable(map) && map.reachability.CanReach(raidSpawnLoc, intVec, PathEndMode.OnCell, TraverseMode.PassAllDestroyableThings, Danger.Deadly))
					{
						return intVec;
					}
				}
			}
			Pawn pawn;
			if ((from x in map.mapPawns.FreeColonistsSpawned
			where map.reachability.CanReach(raidSpawnLoc, x, PathEndMode.OnCell, TraverseMode.PassAllDestroyableThings, Danger.Deadly)
			select x).TryRandomElement(out pawn))
			{
				return pawn.Position;
			}
			IntVec3 result;
			if (CellFinderLoose.TryGetRandomCellWith((IntVec3 x) => map.reachability.CanReach(raidSpawnLoc, x, PathEndMode.OnCell, TraverseMode.PassAllDestroyableThings, Danger.Deadly), map, 1000, out result))
			{
				return result;
			}
			return map.Center;
		}

		// Token: 0x0600468C RID: 18060 RVA: 0x001BBC4C File Offset: 0x001B9E4C
		public static bool EnemyIsNear(Pawn p, float radius, bool meleeOnly = false)
		{
			Thing thing;
			return GenAI.EnemyIsNear(p, radius, out thing, meleeOnly, false);
		}

		// Token: 0x0600468D RID: 18061 RVA: 0x001BBC64 File Offset: 0x001B9E64
		public static bool EnemyIsNear(Pawn p, float radius, out Thing threat, bool meleeOnly = false, bool requireLos = false)
		{
			threat = null;
			if (!p.Spawned)
			{
				return false;
			}
			bool flag = p.Position.Fogged(p.Map);
			List<IAttackTarget> potentialTargetsFor = p.Map.attackTargetsCache.GetPotentialTargetsFor(p);
			for (int i = 0; i < potentialTargetsFor.Count; i++)
			{
				IAttackTarget attackTarget = potentialTargetsFor[i];
				if (!attackTarget.ThreatDisabled(p) && (flag || !attackTarget.Thing.Position.Fogged(attackTarget.Thing.Map)) && (!requireLos || GenSight.LineOfSightToThing(p.Position, attackTarget.Thing, p.MapHeld, false, null)))
				{
					if (meleeOnly)
					{
						Pawn pawn = attackTarget as Pawn;
						if (pawn != null && pawn.equipment != null)
						{
							CompEquippable primaryEq = pawn.equipment.PrimaryEq;
							if (primaryEq != null && !primaryEq.PrimaryVerb.IsMeleeAttack)
							{
								goto IL_EA;
							}
						}
					}
					if (p.Position.InHorDistOf(((Thing)attackTarget).Position, radius))
					{
						threat = (Thing)attackTarget;
						return true;
					}
				}
				IL_EA:;
			}
			return false;
		}
	}
}
