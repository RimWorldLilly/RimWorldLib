using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200126A RID: 4714
	public class RoomPart_Breached : RoomPartWorker
	{
		// Token: 0x170013D3 RID: 5075
		// (get) Token: 0x060074D6 RID: 29910 RVA: 0x00290324 File Offset: 0x0028E524
		public override bool FillOnPost
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060074D7 RID: 29911 RVA: 0x00290327 File Offset: 0x0028E527
		public RoomPart_Breached(RoomPartDef def) : base(def)
		{
		}

		// Token: 0x060074D8 RID: 29912 RVA: 0x00290330 File Offset: 0x0028E530
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints)
		{
			if (!RoomPart_Breached.CanRemoveWalls(room))
			{
				return;
			}
			foreach (CellRect cellRect in room.rects)
			{
				int num = Rand.Range(0, 4);
				for (int i = 0; i < 4; i++)
				{
					Rot4 rot = new Rot4(i + num);
					CellRect edgeRect = cellRect.GetEdgeRect(rot);
					bool flag = true;
					foreach (IntVec3 a in edgeRect.Cells)
					{
						for (int j = 0; j < 4; j++)
						{
							IntVec3 intVec = a + GenAdj.CardinalDirections[j];
							LayoutRoom room2;
							if (!room.Contains(intVec, 0) && room.sketch.structureLayout.TryGetRoom(intVec, out room2) && !RoomPart_Breached.CanRemoveWalls(room2))
							{
								flag = false;
								break;
							}
						}
					}
					if (flag)
					{
						this.DestroyWall(map, edgeRect, rot);
						return;
					}
				}
			}
		}

		// Token: 0x060074D9 RID: 29913 RVA: 0x00290460 File Offset: 0x0028E660
		private static bool CanRemoveWalls(LayoutRoom room)
		{
			if (room.requiredDef != null)
			{
				return false;
			}
			foreach (LayoutRoomDef layoutRoomDef in room.defs)
			{
				if (layoutRoomDef.canRemoveBorderDoors || layoutRoomDef.canRemoveBorderWalls)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060074DA RID: 29914 RVA: 0x002904D0 File Offset: 0x0028E6D0
		private void DestroyWall(Map map, CellRect edge, Rot4 rot)
		{
			int num = edge.GetSideLength(rot) / 2 - 2;
			if (num <= 2)
			{
				return;
			}
			int i = Mathf.Min(RoomPart_Breached.CellsToDestroy.RandomInRange, num);
			int i2 = Mathf.Min(RoomPart_Breached.CellsToDestroy.RandomInRange, num);
			IntVec3 first = edge.CenterCell - rot.RighthandCell * i;
			IntVec3 second = edge.CenterCell + rot.RighthandCell * i2;
			CellRect cellRect = CellRect.FromLimits(first, second);
			foreach (IntVec3 c in cellRect.Cells)
			{
				Building edifice = c.GetEdifice(map);
				if (edifice != null)
				{
					if (cellRect.IsCorner(c))
					{
						edifice.HitPoints = (int)((float)edifice.MaxHitPoints * RoomPart_Breached.DamageRangePercent.RandomInRange);
					}
					else
					{
						edifice.Destroy(DestroyMode.Vanish);
					}
				}
			}
			int randomInRange = RoomPart_Breached.RadialDamageRange.RandomInRange;
			CellRect cellRect2 = cellRect.CenterCell.RectAbout(randomInRange, randomInRange);
			foreach (IntVec3 c2 in cellRect2.Cells)
			{
				if (!cellRect2.IsCorner(c2))
				{
					Building edifice2 = c2.GetEdifice(map);
					if (edifice2 != null)
					{
						edifice2.HitPoints = (int)((float)edifice2.MaxHitPoints * RoomPart_Breached.DamageRangePercent.RandomInRange);
					}
				}
			}
			GenSpawn.SpawnIrregularLump(ThingDefOf.Filth_RubbleBuilding, cellRect.CenterCell, map, RoomPart_Breached.FilthGroupRange, RoomPart_Breached.FilthDistanceRange, WipeMode.Vanish, null, null, null, null, null);
			GenSpawn.SpawnIrregularLump(ThingDefOf.Filth_RubbleBuilding, cellRect.RandomCell, map, RoomPart_Breached.FilthGroupRange, RoomPart_Breached.FilthDistanceRange, WipeMode.Vanish, null, null, null, null, null);
			FilthMaker.TryMakeFilth(cellRect.CenterCell, map, ThingDefOf.Filth_BlastMark, 1, FilthSourceFlags.None, true);
		}

		// Token: 0x04004953 RID: 18771
		private static readonly IntRange CellsToDestroy = new IntRange(2, 4);

		// Token: 0x04004954 RID: 18772
		private static readonly IntRange RadialDamageRange = new IntRange(2, 4);

		// Token: 0x04004955 RID: 18773
		private static readonly FloatRange DamageRangePercent = new FloatRange(0.4f, 0.8f);

		// Token: 0x04004956 RID: 18774
		private static readonly IntRange FilthGroupRange = new IntRange(3, 6);

		// Token: 0x04004957 RID: 18775
		private static readonly IntRange FilthDistanceRange = new IntRange(3, 6);
	}
}
