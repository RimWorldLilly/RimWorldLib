using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200123D RID: 4669
	public class RoomContents_OperatingRoom : RoomContentsWorker
	{
		// Token: 0x170013AA RID: 5034
		// (get) Token: 0x060073DC RID: 29660 RVA: 0x002893C1 File Offset: 0x002875C1
		protected virtual FloatRange ItemsPer100Cells
		{
			get
			{
				return new FloatRange(2f, 4f);
			}
		}

		// Token: 0x060073DD RID: 29661 RVA: 0x002893D4 File Offset: 0x002875D4
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			Rot4 value = Rot4.North;
			CellRect boundary = room.Boundary;
			if (boundary.Height > boundary.Width)
			{
				value = Rot4.East;
			}
			RoomGenUtility.FillWithPadding(ThingDefOf.AncientOperatingTable, this.GetCount(room), room, map, new Rot4?(value), null, null, 1, null, false, false, null, null);
			foreach (IntVec3 c in room.Cells)
			{
				if (Rand.Chance(0.75f))
				{
					map.terrainGrid.SetTerrain(c, TerrainDefOf.AncientTile);
				}
			}
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x060073DE RID: 29662 RVA: 0x00289488 File Offset: 0x00287688
		private int GetCount(LayoutRoom room)
		{
			float num = (float)room.Area / 100f;
			return Mathf.Max(Mathf.RoundToInt(this.ItemsPer100Cells.RandomInRange * num), 1);
		}

		// Token: 0x04004906 RID: 18694
		private const float FloorSteelPercentage = 0.75f;
	}
}
