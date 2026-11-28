using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001569 RID: 5481
	public static class BeautyUtility
	{
		// Token: 0x06008673 RID: 34419 RVA: 0x00309D5C File Offset: 0x00307F5C
		public static float AverageBeautyPerceptible(IntVec3 root, Map map)
		{
			if (!root.IsValid || !root.InBounds(map))
			{
				return 0f;
			}
			BeautyUtility.tempCountedThings.Clear();
			float num = 0f;
			int num2 = 0;
			using (new ProfilerBlock("FillBeautyRelevantCells"))
			{
				BeautyUtility.FillBeautyRelevantCells(root, map);
			}
			for (int i = 0; i < BeautyUtility.beautyRelevantCells.Count; i++)
			{
				using (new ProfilerBlock("CellBeauty"))
				{
					num += BeautyUtility.CellBeauty(BeautyUtility.beautyRelevantCells[i], map, BeautyUtility.tempCountedThings);
				}
				num2++;
			}
			BeautyUtility.tempCountedThings.Clear();
			if (num2 == 0)
			{
				return 0f;
			}
			return num / (float)num2;
		}

		// Token: 0x06008674 RID: 34420 RVA: 0x00309E38 File Offset: 0x00308038
		public static void FillBeautyRelevantCells(IntVec3 root, Map map)
		{
			BeautyUtility.beautyRelevantCells.Clear();
			Room room = root.GetRoom(map);
			if (room == null)
			{
				return;
			}
			BeautyUtility.visibleRooms.Clear();
			BeautyUtility.visibleRooms.Add(room);
			if (room.IsDoorway)
			{
				foreach (Region region in room.FirstRegion.Neighbors)
				{
					BeautyUtility.visibleRooms.Add(region.Room);
				}
			}
			for (int i = 0; i < BeautyUtility.SampleNumCells_Beauty; i++)
			{
				IntVec3 intVec = root + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map) && !intVec.Fogged(map))
				{
					Room room2 = intVec.GetRoom(map);
					if (!BeautyUtility.visibleRooms.Contains(room2))
					{
						bool flag = false;
						for (int j = 0; j < 8; j++)
						{
							IntVec3 loc = intVec + GenAdj.AdjacentCells[j];
							if (BeautyUtility.visibleRooms.Contains(loc.GetRoom(map)))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							goto IL_10A;
						}
					}
					BeautyUtility.beautyRelevantCells.Add(intVec);
				}
				IL_10A:;
			}
			BeautyUtility.visibleRooms.Clear();
		}

		// Token: 0x06008675 RID: 34421 RVA: 0x00309F78 File Offset: 0x00308178
		public static float CellBeauty(IntVec3 c, Map map, HashSet<Thing> countedThings = null)
		{
			float num = 0f;
			float num2 = 0f;
			bool flag = false;
			Room room = c.GetRoom(map);
			bool flag2 = room == null || room.PsychologicallyOutdoors;
			bool flag3 = map.roofGrid.Roofed(c);
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (BeautyUtility.BeautyRelevant(thing.def.category) && (countedThings == null || !countedThings.Contains(thing)))
				{
					if (countedThings != null)
					{
						countedThings.Add(thing);
					}
					SlotGroup slotGroup = thing.GetSlotGroup();
					if (!thing.def.EverHaulable || slotGroup == null || slotGroup.parent == thing || !slotGroup.parent.IgnoreStoredThingsBeauty)
					{
						float num3 = thing.GetBeauty(flag2);
						if (thing.def.filth != null && flag3)
						{
							num3 *= 0.3f;
						}
						if (thing.def.Fillage == FillCategory.Full)
						{
							flag = true;
							num2 += num3;
						}
						else
						{
							num += num3;
						}
					}
				}
			}
			if (flag)
			{
				return num2;
			}
			TerrainDef terrainDef = map.terrainGrid.TerrainAt(c);
			if (ModsConfig.BiotechActive && !terrainDef.BuildableByPlayer && c.IsPolluted(map))
			{
				num += -1f;
			}
			if (flag2 && terrainDef.StatBaseDefined(StatDefOf.BeautyOutdoors))
			{
				return num + terrainDef.GetStatValueAbstract(StatDefOf.BeautyOutdoors, null);
			}
			return num + terrainDef.GetStatValueAbstract(StatDefOf.Beauty, null);
		}

		// Token: 0x06008676 RID: 34422 RVA: 0x0030A0FD File Offset: 0x003082FD
		public static bool BeautyRelevant(ThingCategory cat)
		{
			return cat == ThingCategory.Building || cat == ThingCategory.Item || cat == ThingCategory.Plant || cat == ThingCategory.Filth;
		}

		// Token: 0x040052CE RID: 21198
		public static List<IntVec3> beautyRelevantCells = new List<IntVec3>();

		// Token: 0x040052CF RID: 21199
		private static HashSet<Room> visibleRooms = new HashSet<Room>();

		// Token: 0x040052D0 RID: 21200
		public static readonly int SampleNumCells_Beauty = GenRadial.NumCellsInRadius(8.9f);

		// Token: 0x040052D1 RID: 21201
		private const float PollutedTerrainBeautyOffset = -1f;

		// Token: 0x040052D2 RID: 21202
		private static HashSet<Thing> tempCountedThings = new HashSet<Thing>();
	}
}
