using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E2A RID: 7722
	public static class AutoHomeAreaMaker
	{
		// Token: 0x0600C8A6 RID: 51366 RVA: 0x00499BCE File Offset: 0x00497DCE
		private static bool ShouldAdd()
		{
			return Find.PlaySettings.autoHomeArea && Current.ProgramState == ProgramState.Playing;
		}

		// Token: 0x0600C8A7 RID: 51367 RVA: 0x00499BE6 File Offset: 0x00497DE6
		public static void Notify_BuildingSpawned(Thing b)
		{
			if (!AutoHomeAreaMaker.ShouldAdd() || !b.def.building.expandHomeArea || b.Faction != Faction.OfPlayer)
			{
				return;
			}
			AutoHomeAreaMaker.MarkHomeAroundThing(b);
		}

		// Token: 0x0600C8A8 RID: 51368 RVA: 0x00499C15 File Offset: 0x00497E15
		public static void Notify_BuildingClaimed(Thing b)
		{
			if (!AutoHomeAreaMaker.ShouldAdd() || !b.def.building.expandHomeArea || b.Faction != Faction.OfPlayer)
			{
				return;
			}
			AutoHomeAreaMaker.MarkHomeAroundThing(b);
		}

		// Token: 0x0600C8A9 RID: 51369 RVA: 0x00499C44 File Offset: 0x00497E44
		public static void MarkHomeAroundThing(Thing t)
		{
			if (!AutoHomeAreaMaker.ShouldAdd())
			{
				return;
			}
			CellRect cellRect = new CellRect(t.Position.x - t.RotatedSize.x / 2 - 4, t.Position.z - t.RotatedSize.z / 2 - 4, t.RotatedSize.x + 8, t.RotatedSize.z + 8);
			cellRect.ClipInsideMap(t.Map);
			foreach (IntVec3 c in cellRect)
			{
				t.Map.areaManager.Home[c] = true;
			}
		}

		// Token: 0x0600C8AA RID: 51370 RVA: 0x00499D10 File Offset: 0x00497F10
		public static void Notify_ZoneCellAdded(IntVec3 c, Zone zone)
		{
			if (!AutoHomeAreaMaker.ShouldAdd())
			{
				return;
			}
			foreach (IntVec3 c2 in CellRect.CenteredOn(c, 4).ClipInsideMap(zone.Map))
			{
				zone.Map.areaManager.Home[c2] = true;
			}
		}

		// Token: 0x04007253 RID: 29267
		private const int BorderWidth = 4;
	}
}
