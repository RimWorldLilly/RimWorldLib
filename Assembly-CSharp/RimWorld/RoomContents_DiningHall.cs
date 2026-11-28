using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001231 RID: 4657
	public class RoomContents_DiningHall : RoomContentsWorker
	{
		// Token: 0x170013A0 RID: 5024
		// (get) Token: 0x06007396 RID: 29590 RVA: 0x00287AEE File Offset: 0x00285CEE
		protected virtual ThingDef TableDef
		{
			get
			{
				return ThingDefOf.Table2x4c;
			}
		}

		// Token: 0x170013A1 RID: 5025
		// (get) Token: 0x06007397 RID: 29591 RVA: 0x00287AF5 File Offset: 0x00285CF5
		protected virtual ThingDef TableStuffDef
		{
			get
			{
				return ThingDefOf.Steel;
			}
		}

		// Token: 0x170013A2 RID: 5026
		// (get) Token: 0x06007398 RID: 29592 RVA: 0x00287AFC File Offset: 0x00285CFC
		protected virtual ThingDef ChairDef
		{
			get
			{
				return ThingDefOf.DiningChair;
			}
		}

		// Token: 0x170013A3 RID: 5027
		// (get) Token: 0x06007399 RID: 29593 RVA: 0x00287B03 File Offset: 0x00285D03
		protected virtual ThingDef ChairStuffDef
		{
			get
			{
				return ThingDefOf.Steel;
			}
		}

		// Token: 0x0600739A RID: 29594 RVA: 0x00287B0C File Offset: 0x00285D0C
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			Rot4 value = Rot4.North;
			CellRect boundary = room.Boundary;
			if (boundary.Height > boundary.Width)
			{
				value = Rot4.East;
			}
			List<Thing> spawned = new List<Thing>();
			RoomGenUtility.FillWithPadding(this.TableDef, this.GetCount(room), room, map, new Rot4?(value), null, spawned, 1, this.TableStuffDef, false, false, null, null);
			this.SpawnChairs(map, spawned);
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x0600739B RID: 29595 RVA: 0x00287B7C File Offset: 0x00285D7C
		private void SpawnChairs(Map map, List<Thing> spawned)
		{
			foreach (Thing thing in spawned)
			{
				CellRect cellRect = thing.OccupiedRect().ExpandedBy(1);
				float randomInRange = RoomContents_DiningHall.ChairsPerTableRange.RandomInRange;
				int num = 0;
				foreach (IntVec3 intVec in cellRect.EdgeCellsNoCorners.InRandomOrder(null))
				{
					if ((float)num < randomInRange)
					{
						Rot4 rot = Rot4.FromAngleFlat((thing.Position - intVec).AngleFlat);
						if (GenSpawn.CanSpawnAt(this.ChairDef, intVec, map, new Rot4?(rot), true))
						{
							GenSpawn.Spawn(ThingMaker.MakeThing(this.ChairDef, this.ChairStuffDef), intVec, map, rot, WipeMode.Vanish, false, false);
							num++;
						}
					}
				}
			}
		}

		// Token: 0x0600739C RID: 29596 RVA: 0x00287C90 File Offset: 0x00285E90
		private int GetCount(LayoutRoom room)
		{
			float num = (float)room.Area / 100f;
			LayoutRoomDef roomDef = base.RoomDef;
			return Mathf.Max(Mathf.RoundToInt(((roomDef.itemsPer100CellsRange != null) ? roomDef.itemsPer100CellsRange.GetValueOrDefault().RandomInRange : 1f) * num), 1);
		}

		// Token: 0x040048F1 RID: 18673
		private static readonly FloatRange ChairsPerTableRange = new FloatRange(-1f, 2f);
	}
}
