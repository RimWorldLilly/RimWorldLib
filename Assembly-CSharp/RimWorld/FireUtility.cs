using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B19 RID: 6937
	public static class FireUtility
	{
		// Token: 0x0600B172 RID: 45426 RVA: 0x003E87EA File Offset: 0x003E69EA
		public static bool CanEverAttachFire(this Thing t)
		{
			return !t.Destroyed && t.FlammableNow && t.def.category == ThingCategory.Pawn && t.TryGetComp<CompAttachBase>() != null;
		}

		// Token: 0x0600B173 RID: 45427 RVA: 0x003E881C File Offset: 0x003E6A1C
		public static float ChanceToStartFireIn(IntVec3 c, Map map, SimpleCurve flammabilityChanceCurve = null)
		{
			List<Thing> thingList = c.GetThingList(map);
			float num = c.TerrainFlammability(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (thing is Fire)
				{
					return 0f;
				}
				if (thing.def.category != ThingCategory.Pawn && thingList[i].FlammableNow)
				{
					num = Mathf.Max(num, thing.GetStatValue(StatDefOf.Flammability, true, -1));
				}
			}
			if (flammabilityChanceCurve != null)
			{
				num = flammabilityChanceCurve.Evaluate(num);
			}
			if (num > 0f)
			{
				Building edifice = c.GetEdifice(map);
				if (edifice != null && edifice.def.passability == Traversability.Impassable && edifice.OccupiedRect().ContractedBy(1).Contains(c))
				{
					return 0f;
				}
				List<Thing> thingList2 = c.GetThingList(map);
				for (int j = 0; j < thingList2.Count; j++)
				{
					if (thingList2[j].def.category == ThingCategory.Filth && !thingList2[j].def.filth.allowsFire)
					{
						return 0f;
					}
				}
			}
			return num;
		}

		// Token: 0x0600B174 RID: 45428 RVA: 0x003E893C File Offset: 0x003E6B3C
		public static bool TryStartFireIn(IntVec3 c, Map map, float fireSize, Thing instigator, SimpleCurve flammabilityChanceCurve = null)
		{
			if (FireUtility.ChanceToStartFireIn(c, map, flammabilityChanceCurve) <= 0f)
			{
				return false;
			}
			Fire fire = (Fire)ThingMaker.MakeThing(ThingDefOf.Fire, null);
			fire.fireSize = fireSize;
			fire.instigator = instigator;
			GenSpawn.Spawn(fire, c, map, Rot4.North, WipeMode.Vanish, false, false);
			return true;
		}

		// Token: 0x0600B175 RID: 45429 RVA: 0x003E898A File Offset: 0x003E6B8A
		public static float ChanceToAttachFireFromEvent(Thing t)
		{
			return FireUtility.ChanceToAttachFireCumulative(t, 60f);
		}

		// Token: 0x0600B176 RID: 45430 RVA: 0x003E8998 File Offset: 0x003E6B98
		public static float ChanceToAttachFireCumulative(Thing t, float freqInTicks)
		{
			if (!t.CanEverAttachFire())
			{
				return 0f;
			}
			if (t.HasAttachment(ThingDefOf.Fire))
			{
				return 0f;
			}
			float num = FireUtility.ChanceToCatchFirePerSecondForPawnFromFlammability.Evaluate(t.GetStatValue(StatDefOf.Flammability, true, -1));
			return 1f - Mathf.Pow(1f - num, freqInTicks / 60f);
		}

		// Token: 0x0600B177 RID: 45431 RVA: 0x003E89F8 File Offset: 0x003E6BF8
		public static void TryAttachFire(this Thing t, float fireSize, Thing instigator)
		{
			if (!t.CanEverAttachFire())
			{
				return;
			}
			if (t.HasAttachment(ThingDefOf.Fire))
			{
				return;
			}
			Fire fire = (Fire)ThingMaker.MakeThing(ThingDefOf.Fire, null);
			fire.fireSize = fireSize;
			fire.instigator = instigator;
			fire.AttachTo(t);
			GenSpawn.Spawn(fire, t.Position, t.Map, Rot4.North, WipeMode.Vanish, false, false);
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				pawn.jobs.StopAll(false, true);
				pawn.records.Increment(RecordDefOf.TimesOnFire);
			}
		}

		// Token: 0x0600B178 RID: 45432 RVA: 0x003E8A82 File Offset: 0x003E6C82
		public static bool IsBurning(this TargetInfo t)
		{
			if (t.HasThing)
			{
				return t.Thing.IsBurning();
			}
			return t.Cell.ContainsStaticFire(t.Map);
		}

		// Token: 0x0600B179 RID: 45433 RVA: 0x003E8AB0 File Offset: 0x003E6CB0
		public static bool IsBurning(this Thing t)
		{
			if (t.Destroyed || !t.Spawned)
			{
				return false;
			}
			if (!(t.def.size == IntVec2.One))
			{
				using (CellRect.Enumerator enumerator = t.OccupiedRect().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.ContainsStaticFire(t.Map))
						{
							return true;
						}
					}
				}
				return false;
			}
			if (t is Pawn)
			{
				return t.HasAttachment(ThingDefOf.Fire);
			}
			return t.Position.ContainsStaticFire(t.Map);
		}

		// Token: 0x0600B17A RID: 45434 RVA: 0x003E8B64 File Offset: 0x003E6D64
		public static bool ContainsStaticFire(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Fire fire = list[i] as Fire;
				if (fire != null && fire.parent == null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B17B RID: 45435 RVA: 0x003E8BAC File Offset: 0x003E6DAC
		public static int NumFiresAt(IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].IsBurning())
				{
					num++;
				}
				if (list[i].def.CompDefForAssignableFrom<CompFireOverlayBase>() != null)
				{
					CompGlower compGlower = list[i].TryGetComp<CompGlower>();
					if (compGlower != null && compGlower.Glows)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x0600B17C RID: 45436 RVA: 0x003E8C20 File Offset: 0x003E6E20
		public static bool ContainsTrap(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice is Building_Trap;
		}

		// Token: 0x0600B17D RID: 45437 RVA: 0x003E8C43 File Offset: 0x003E6E43
		public static bool Flammable(this TerrainDef terrain)
		{
			return terrain.GetStatValueAbstract(StatDefOf.Flammability, null) > 0.01f;
		}

		// Token: 0x0600B17E RID: 45438 RVA: 0x003E8C58 File Offset: 0x003E6E58
		public static bool TerrainFlammableNow(this IntVec3 c, Map map)
		{
			TerrainDef terrain = c.GetTerrain(map);
			TerrainDef terrainDef = map.terrainGrid.FoundationAt(c);
			if (!terrain.Flammable() && (terrainDef == null || !terrainDef.Flammable()))
			{
				return false;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].FireBulwark)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600B17F RID: 45439 RVA: 0x003E8CB8 File Offset: 0x003E6EB8
		public static float TerrainFlammability(this IntVec3 c, Map map)
		{
			if (!c.TerrainFlammableNow(map))
			{
				return 0f;
			}
			BuildableDef terrain = c.GetTerrain(map);
			TerrainDef terrainDef = map.terrainGrid.FoundationAt(c);
			float num = terrain.GetStatValueAbstract(StatDefOf.Flammability, null);
			if (terrainDef != null)
			{
				num = Mathf.Max(num, terrainDef.GetStatValueAbstract(StatDefOf.Flammability, null));
			}
			return num;
		}

		// Token: 0x0600B180 RID: 45440 RVA: 0x003E8D0C File Offset: 0x003E6F0C
		public static List<Fire> GetFiresNearCell(this IntVec3 cell, Map map)
		{
			FireUtility.fireList.Clear();
			Room room = RegionAndRoomQuery.RoomAt(cell, map, RegionType.Set_All);
			if (room == null || room.Dereferenced || room.Fogged || room.IsHuge || room.TouchesMapEdge)
			{
				Region region = cell.GetRegion(map, RegionType.Set_Passable);
				if (region == null)
				{
					List<Thing> list = map.thingGrid.ThingsListAt(cell);
					for (int i = 0; i < list.Count; i++)
					{
						Fire fire = list[i] as Fire;
						if (fire != null && fire.parent == null)
						{
							FireUtility.fireList.Add(fire);
						}
					}
				}
				else
				{
					region.ListerThings.GetThingsOfType<Fire>(FireUtility.fireList);
				}
			}
			else
			{
				List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
				for (int j = 0; j < containedAndAdjacentThings.Count; j++)
				{
					Fire fire2 = containedAndAdjacentThings[j] as Fire;
					if (fire2 != null)
					{
						FireUtility.fireList.Add(fire2);
					}
				}
			}
			FireUtility.fireList.Shuffle<Fire>();
			FireUtility.fireList.Swap(0, FireUtility.fireList.FindIndex(0, (Fire f) => f.Position == cell));
			return FireUtility.fireList;
		}

		// Token: 0x0600B181 RID: 45441 RVA: 0x003E8E48 File Offset: 0x003E7048
		public static float GetEffectiveVacuumForFire(IntVec3 c, Map map)
		{
			float num = 0f;
			Building edifice = c.GetEdifice(map);
			if (edifice != null && map.Biome.inVacuum && edifice.def.passability == Traversability.Impassable && edifice.def.Fillage == FillCategory.Full)
			{
				if (c.GetRoof(map) == null)
				{
					num = 1f;
				}
				else
				{
					for (int i = 0; i < 4; i++)
					{
						if ((c + GenAdj.CardinalDirections[i]).GetRoof(map) == null)
						{
							num = 1f;
							break;
						}
						num = Mathf.Max(num, c.GetVacuum(map));
					}
				}
			}
			else
			{
				num = c.GetVacuum(map);
			}
			return num;
		}

		// Token: 0x0400668F RID: 26255
		private static readonly SimpleCurve ChanceToCatchFirePerSecondForPawnFromFlammability = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(0.1f, 0.07f),
				true
			},
			{
				new CurvePoint(0.3f, 1f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			}
		};

		// Token: 0x04006690 RID: 26256
		private static readonly List<Fire> fireList = new List<Fire>();
	}
}
