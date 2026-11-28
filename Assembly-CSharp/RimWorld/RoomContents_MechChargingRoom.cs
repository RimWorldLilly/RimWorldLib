using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200123B RID: 4667
	public class RoomContents_MechChargingRoom : RoomContentsWorker
	{
		// Token: 0x060073C9 RID: 29641 RVA: 0x00288BA8 File Offset: 0x00286DA8
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			RoomContents_MechChargingRoom.SpawnMechRechargers(map, room);
		}

		// Token: 0x060073CA RID: 29642 RVA: 0x00288BBC File Offset: 0x00286DBC
		private static void SpawnMechRechargers(Map map, LayoutRoom room)
		{
			float num = (float)room.rects.Sum((CellRect r) => r.ContractedBy(1).EdgeCellsCount) / 10f;
			int count = Mathf.Max(Mathf.RoundToInt(RoomContents_MechChargingRoom.StandardRechargersPer10EdgeCells.RandomInRange * num), 1);
			RoomGenUtility.FillAroundEdges(ThingDefOf.AncientStandardRecharger, count, RoomContents_MechChargingRoom.StandardRechargersGroupSize, room, map, null, null, 1, 0, null, true, RotationDirection.Opposite, null, null);
			count = Mathf.Max(Mathf.RoundToInt(RoomContents_MechChargingRoom.BasicRechargersPer10EdgeCells.RandomInRange * num), 1);
			RoomGenUtility.FillAroundEdges(ThingDefOf.AncientBasicRecharger, count, RoomContents_MechChargingRoom.BasicRechargersGroupSize, room, map, null, null, 1, 0, null, true, RotationDirection.Opposite, null, null);
		}

		// Token: 0x040048FF RID: 18687
		private static readonly FloatRange StandardRechargersPer10EdgeCells = new FloatRange(0.2f, 1f);

		// Token: 0x04004900 RID: 18688
		private static readonly IntRange StandardRechargersGroupSize = new IntRange(1, 1);

		// Token: 0x04004901 RID: 18689
		private static readonly FloatRange BasicRechargersPer10EdgeCells = new FloatRange(0.5f, 1.5f);

		// Token: 0x04004902 RID: 18690
		private static readonly IntRange BasicRechargersGroupSize = new IntRange(1, 2);
	}
}
