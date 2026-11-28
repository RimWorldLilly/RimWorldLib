using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020005A5 RID: 1445
	public static class ThingUtility
	{
		// Token: 0x06002B62 RID: 11106 RVA: 0x00107BA3 File Offset: 0x00105DA3
		public static bool DestroyedOrNull(this Thing t)
		{
			return t == null || t.Destroyed;
		}

		// Token: 0x06002B63 RID: 11107 RVA: 0x00107BB0 File Offset: 0x00105DB0
		public static void DestroyOrPassToWorld(this Thing t, DestroyMode mode = DestroyMode.Vanish)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				if (!Find.WorldPawns.Contains(pawn))
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					return;
				}
			}
			else
			{
				t.Destroy(mode);
			}
		}

		// Token: 0x06002B64 RID: 11108 RVA: 0x00107BE8 File Offset: 0x00105DE8
		public static int TryAbsorbStackNumToTake(Thing thing, Thing other, bool respectStackLimit)
		{
			int result;
			if (respectStackLimit)
			{
				result = Mathf.Min(other.stackCount, thing.def.stackLimit - thing.stackCount);
			}
			else
			{
				result = other.stackCount;
			}
			return result;
		}

		// Token: 0x06002B65 RID: 11109 RVA: 0x00107C20 File Offset: 0x00105E20
		public static int RoundedResourceStackCount(int stackCount)
		{
			if (stackCount > 200)
			{
				return GenMath.RoundTo(stackCount, 10);
			}
			if (stackCount > 100)
			{
				return GenMath.RoundTo(stackCount, 5);
			}
			return stackCount;
		}

		// Token: 0x06002B66 RID: 11110 RVA: 0x00107C41 File Offset: 0x00105E41
		public static IntVec3 InteractionCell(IntVec3 interactionOffset, IntVec3 thingCenter, Rot4 rot)
		{
			return interactionOffset.RotatedBy(rot) + thingCenter;
		}

		// Token: 0x06002B67 RID: 11111 RVA: 0x00107C50 File Offset: 0x00105E50
		public static List<IntVec3> InteractionCellsWhenAt(ThingDef def, IntVec3 center, Rot4 rot, Map map, bool allowFallbackCell = false)
		{
			ThingUtility.InteractionCellsWhenAt(ThingUtility.tmpInteractionCells, def, center, rot, map, allowFallbackCell);
			return ThingUtility.tmpInteractionCells;
		}

		// Token: 0x06002B68 RID: 11112 RVA: 0x00107C68 File Offset: 0x00105E68
		public static void InteractionCellsWhenAt(List<IntVec3> listToPopulate, ThingDef def, IntVec3 center, Rot4 rot, Map map, bool allowFallbackCell = false)
		{
			listToPopulate.Clear();
			if (!def.multipleInteractionCellOffsets.NullOrEmpty<IntVec3>())
			{
				using (List<IntVec3>.Enumerator enumerator = def.multipleInteractionCellOffsets.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IntVec3 interactionOffset = enumerator.Current;
						listToPopulate.Add(ThingUtility.InteractionCell(interactionOffset, center, rot));
					}
					return;
				}
			}
			if (def.hasInteractionCell || allowFallbackCell)
			{
				listToPopulate.Add(ThingUtility.InteractionCellWhenAt(def, center, rot, map));
			}
		}

		// Token: 0x06002B69 RID: 11113 RVA: 0x00107CF0 File Offset: 0x00105EF0
		public static IntVec3 InteractionCellWhenAt(ThingDef def, IntVec3 center, Rot4 rot, Map map)
		{
			if (def.hasInteractionCell)
			{
				return ThingUtility.InteractionCell(def.interactionCellOffset, center, rot);
			}
			if (def.Size.x == 1 && def.Size.z == 1)
			{
				for (int i = 0; i < 8; i++)
				{
					IntVec3 intVec = center + GenAdj.AdjacentCells[i];
					if (intVec.Standable(map) && intVec.GetDoor(map) == null && ReachabilityImmediate.CanReachImmediate(intVec, center, map, PathEndMode.Touch, null))
					{
						return intVec;
					}
				}
				for (int j = 0; j < 8; j++)
				{
					IntVec3 intVec2 = center + GenAdj.AdjacentCells[j];
					if (intVec2.Standable(map) && ReachabilityImmediate.CanReachImmediate(intVec2, center, map, PathEndMode.Touch, null))
					{
						return intVec2;
					}
				}
				for (int k = 0; k < 8; k++)
				{
					IntVec3 intVec3 = center + GenAdj.AdjacentCells[k];
					if (intVec3.Walkable(map) && ReachabilityImmediate.CanReachImmediate(intVec3, center, map, PathEndMode.Touch, null))
					{
						return intVec3;
					}
				}
				return center;
			}
			List<IntVec3> list = GenAdjFast.AdjacentCells8Way(center, rot, def.size);
			CellRect rect = GenAdj.OccupiedRect(center, rot, def.size);
			for (int l = 0; l < list.Count; l++)
			{
				if (list[l].Standable(map) && list[l].GetDoor(map) == null && ReachabilityImmediate.CanReachImmediate(list[l], rect, map, PathEndMode.Touch, null))
				{
					return list[l];
				}
			}
			for (int m = 0; m < list.Count; m++)
			{
				if (list[m].Standable(map) && ReachabilityImmediate.CanReachImmediate(list[m], rect, map, PathEndMode.Touch, null))
				{
					return list[m];
				}
			}
			for (int n = 0; n < list.Count; n++)
			{
				if (list[n].Walkable(map) && ReachabilityImmediate.CanReachImmediate(list[n], rect, map, PathEndMode.Touch, null))
				{
					return list[n];
				}
			}
			return center;
		}

		// Token: 0x06002B6A RID: 11114 RVA: 0x00107EF9 File Offset: 0x001060F9
		public static DamageDef PrimaryMeleeWeaponDamageType(ThingDef thing)
		{
			return ThingUtility.PrimaryMeleeWeaponDamageType(thing.tools);
		}

		// Token: 0x06002B6B RID: 11115 RVA: 0x00107F08 File Offset: 0x00106108
		public static DamageDef PrimaryMeleeWeaponDamageType(List<Tool> tools)
		{
			if (tools.NullOrEmpty<Tool>())
			{
				return null;
			}
			ManeuverDef maneuverDef = tools.MaxBy((Tool tool) => tool.power).Maneuvers.FirstOrDefault<ManeuverDef>();
			if (maneuverDef == null)
			{
				return null;
			}
			return maneuverDef.verb.meleeDamageDef;
		}

		// Token: 0x06002B6C RID: 11116 RVA: 0x00107F60 File Offset: 0x00106160
		public static Blueprint_Build CheckAutoRebuildOnDestroyed(Thing thing, DestroyMode mode, Map map, BuildableDef buildingDef)
		{
			if (Find.PlaySettings.autoRebuild && mode == DestroyMode.KillFinalize && thing.Faction == Faction.OfPlayer && buildingDef.blueprintDef != null && buildingDef.IsResearchFinished && map.areaManager.Home[thing.Position] && GenConstruct.CanPlaceBlueprintAt(buildingDef, thing.Position, thing.Rotation, map, false, null, null, thing.Stuff, false, false, false).Accepted)
			{
				return GenConstruct.PlaceBlueprintForBuild(buildingDef, thing.Position, map, thing.Rotation, Faction.OfPlayer, thing.Stuff, thing.StyleSourcePrecept, thing.StyleDef, true);
			}
			return null;
		}

		// Token: 0x06002B6D RID: 11117 RVA: 0x00108010 File Offset: 0x00106210
		public static void CheckAutoRebuildTerrainOnDestroyed(TerrainDef terrainDef, IntVec3 pos, Map map)
		{
			if (Find.PlaySettings.autoRebuild && terrainDef.autoRebuildable && terrainDef.blueprintDef != null && terrainDef.IsResearchFinished && map.areaManager.Home[pos] && GenConstruct.CanPlaceBlueprintAt(terrainDef, pos, Rot4.South, map, false, null, null, null, false, false, false).Accepted)
			{
				GenConstruct.PlaceBlueprintForBuild(terrainDef, pos, map, Rot4.South, Faction.OfPlayer, null, null, null, true);
			}
		}

		// Token: 0x06002B6E RID: 11118 RVA: 0x0010808C File Offset: 0x0010628C
		public static Pawn FindPawn(List<Thing> things)
		{
			for (int i = 0; i < things.Count; i++)
			{
				Pawn pawn = things[i] as Pawn;
				if (pawn != null)
				{
					return pawn;
				}
				Corpse corpse = things[i] as Corpse;
				if (corpse != null)
				{
					return corpse.InnerPawn;
				}
			}
			return null;
		}

		// Token: 0x06002B6F RID: 11119 RVA: 0x001080D4 File Offset: 0x001062D4
		public static TerrainAffordanceDef GetTerrainAffordanceNeed(this BuildableDef def, ThingDef stuffDef = null)
		{
			TerrainAffordanceDef terrainAffordanceNeeded = def.terrainAffordanceNeeded;
			if (stuffDef != null && def.useStuffTerrainAffordance && stuffDef.terrainAffordanceNeeded != null)
			{
				terrainAffordanceNeeded = stuffDef.terrainAffordanceNeeded;
			}
			return terrainAffordanceNeeded;
		}

		// Token: 0x06002B70 RID: 11120 RVA: 0x00108103 File Offset: 0x00106303
		public static bool HasThingCategory(this Thing thing, ThingCategoryDef thingCategory)
		{
			return !thing.def.thingCategories.NullOrEmpty<ThingCategoryDef>() && thing.def.thingCategories.Contains(thingCategory);
		}

		// Token: 0x06002B71 RID: 11121 RVA: 0x0010812C File Offset: 0x0010632C
		public static int GetStackCountFromThingList(IEnumerable<Thing> things)
		{
			int num = 0;
			foreach (Thing thing in things)
			{
				num += thing.stackCount;
			}
			return num;
		}

		// Token: 0x06002B72 RID: 11122 RVA: 0x0010817C File Offset: 0x0010637C
		public static void FindAllOfType<T>(ThingRequest request, List<T> list) where T : Thing
		{
			list.Clear();
			List<Thing> list2 = Find.CurrentMap.listerThings.ThingsMatching(request);
			for (int i = 0; i < list2.Count; i++)
			{
				T t = list2[i] as T;
				if (t != null)
				{
					list.Add(t);
				}
			}
			List<Pawn> freeColonists = Find.CurrentMap.mapPawns.FreeColonists;
			for (int j = 0; j < freeColonists.Count; j++)
			{
				T t2 = freeColonists[j].carryTracker.CarriedThing as T;
				if (t2 != null)
				{
					list.Add(t2);
				}
				freeColonists[j].inventory.innerContainer.GetThingsOfType<T>(list);
			}
		}

		// Token: 0x06002B73 RID: 11123 RVA: 0x00108240 File Offset: 0x00106440
		public static void FindAllOfType<T>(List<T> list) where T : Thing
		{
			list.Clear();
			Find.CurrentMap.listerThings.GetThingsOfType<T>(list);
			List<Pawn> freeColonists = Find.CurrentMap.mapPawns.FreeColonists;
			for (int i = 0; i < freeColonists.Count; i++)
			{
				T t = freeColonists[i].carryTracker.CarriedThing as T;
				if (t != null)
				{
					list.Add(t);
				}
				freeColonists[i].inventory.innerContainer.GetThingsOfType<T>(list);
			}
		}

		// Token: 0x04001D82 RID: 7554
		private static List<IntVec3> tmpInteractionCells = new List<IntVec3>();
	}
}
