using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001214 RID: 4628
	public static class ComplexUtility
	{
		// Token: 0x060072E3 RID: 29411 RVA: 0x00282924 File Offset: 0x00280B24
		public static bool TryFindRandomSpawnCell(ThingDef def, LayoutRoom room, Map map, out IntVec3 spawnPosition, int gap = 1, Rot4? rot = null)
		{
			ComplexUtility.<>c__DisplayClass0_0 CS$<>8__locals1 = new ComplexUtility.<>c__DisplayClass0_0();
			CS$<>8__locals1.rot = rot;
			CS$<>8__locals1.def = def;
			CS$<>8__locals1.gap = gap;
			CS$<>8__locals1.map = map;
			ThingDef def2 = CS$<>8__locals1.def;
			Map map2 = CS$<>8__locals1.map;
			Func<IntVec3, bool> validator = new Func<IntVec3, bool>(CS$<>8__locals1.<TryFindRandomSpawnCell>g__Validator|0);
			return room.TryGetRandomCellInRoom(def2, map2, out spawnPosition, null, 1, 0, validator, false);
		}

		// Token: 0x060072E4 RID: 29412 RVA: 0x00282984 File Offset: 0x00280B84
		public static string SpawnRoomEnteredTrigger(LayoutRoom room, Map map)
		{
			string text = "RoomEntered" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
			foreach (CellRect rect in room.rects)
			{
				RectTrigger rectTrigger = (RectTrigger)ThingMaker.MakeThing(ThingDefOf.RectTrigger, null);
				rectTrigger.signalTag = text;
				rectTrigger.Rect = rect;
				GenSpawn.Spawn(rectTrigger, rect.CenterCell, map, WipeMode.Vanish);
			}
			return text;
		}

		// Token: 0x060072E5 RID: 29413 RVA: 0x00282A1C File Offset: 0x00280C1C
		public static string SpawnRadialDistanceTrigger(IEnumerable<Thing> things, Map map, int radius)
		{
			string text = "RandomTrigger" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
			foreach (Thing thing in things)
			{
				RadialTrigger radialTrigger = (RadialTrigger)ThingMaker.MakeThing(ThingDefOf.RadialTrigger, null);
				radialTrigger.signalTag = text;
				radialTrigger.maxRadius = radius;
				radialTrigger.lineOfSight = true;
				GenSpawn.Spawn(radialTrigger, thing.Position, map, WipeMode.Vanish);
			}
			return text;
		}
	}
}
