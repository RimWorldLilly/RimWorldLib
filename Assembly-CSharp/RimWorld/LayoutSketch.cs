using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001220 RID: 4640
	public class LayoutSketch : Sketch
	{
		// Token: 0x17001393 RID: 5011
		// (get) Token: 0x06007326 RID: 29478 RVA: 0x00284A0B File Offset: 0x00282C0B
		protected virtual ThingDef WallThing
		{
			get
			{
				return this.wall ?? ThingDefOf.Wall;
			}
		}

		// Token: 0x17001394 RID: 5012
		// (get) Token: 0x06007327 RID: 29479 RVA: 0x00284A1C File Offset: 0x00282C1C
		protected virtual ThingDef DoorThing
		{
			get
			{
				return this.door ?? ThingDefOf.Door;
			}
		}

		// Token: 0x17001395 RID: 5013
		// (get) Token: 0x06007328 RID: 29480 RVA: 0x00284A2D File Offset: 0x00282C2D
		public virtual ThingDef WallLampThing
		{
			get
			{
				return this.wallLamp;
			}
		}

		// Token: 0x17001396 RID: 5014
		// (get) Token: 0x06007329 RID: 29481 RVA: 0x00284A35 File Offset: 0x00282C35
		protected virtual TerrainDef FloorTerrain
		{
			get
			{
				return this.floor ?? TerrainDefOf.PavedTile;
			}
		}

		// Token: 0x17001397 RID: 5015
		// (get) Token: 0x0600732A RID: 29482 RVA: 0x00284A46 File Offset: 0x00282C46
		public virtual TerrainDef DefaultAffordanceTerrain
		{
			get
			{
				return this.defaultAffordanceTerrain;
			}
		}

		// Token: 0x0600732B RID: 29483 RVA: 0x00284A4E File Offset: 0x00282C4E
		protected virtual ThingDef GetWallStuff(int roomId)
		{
			return this.wallStuff;
		}

		// Token: 0x0600732C RID: 29484 RVA: 0x00284A56 File Offset: 0x00282C56
		protected virtual ThingDef GetDoorStuff(int roomId)
		{
			return this.doorStuff;
		}

		// Token: 0x0600732D RID: 29485 RVA: 0x00284A5E File Offset: 0x00282C5E
		public void FlushLayoutToSketch()
		{
			this.FlushLayoutToSketch(IntVec3.Zero);
		}

		// Token: 0x0600732E RID: 29486 RVA: 0x00284A6C File Offset: 0x00282C6C
		public void FlushLayoutToSketch(IntVec3 at)
		{
			this.structureLayout.container = this.structureLayout.container.MovedBy(at);
			foreach (LayoutRoom layoutRoom in this.structureLayout.Rooms)
			{
				for (int i = 0; i < layoutRoom.rects.Count; i++)
				{
					layoutRoom.rects[i] = layoutRoom.rects[i].MovedBy(at);
				}
			}
			for (int j = this.structureLayout.container.minX; j <= this.structureLayout.container.maxX; j++)
			{
				for (int k = this.structureLayout.container.minZ; k <= this.structureLayout.container.maxZ; k++)
				{
					IntVec3 intVec = new IntVec3(j, 0, k);
					int roomIdAt = this.structureLayout.GetRoomIdAt(intVec);
					if (this.structureLayout.IsWallAt(intVec))
					{
						base.AddThing(this.WallThing, at + intVec, Rot4.North, this.GetWallStuff(roomIdAt), 1, null, null, true, 2f);
					}
					if (this.structureLayout.IsFloorAt(intVec) || this.structureLayout.IsDoorAt(intVec) || this.structureLayout.IsWallAt(intVec))
					{
						base.AddTerrain(this.FloorTerrain, at + intVec, true);
					}
					if (this.structureLayout.IsDoorAt(intVec))
					{
						base.AddThing(this.DoorThing, at + intVec, Rot4.North, this.GetDoorStuff(roomIdAt), 1, null, null, true, 2f);
					}
				}
			}
		}

		// Token: 0x040048D1 RID: 18641
		public StructureLayout structureLayout;

		// Token: 0x040048D2 RID: 18642
		public ThingDef wall;

		// Token: 0x040048D3 RID: 18643
		public ThingDef wallStuff;

		// Token: 0x040048D4 RID: 18644
		public ThingDef door;

		// Token: 0x040048D5 RID: 18645
		public ThingDef doorStuff;

		// Token: 0x040048D6 RID: 18646
		public TerrainDef floor;

		// Token: 0x040048D7 RID: 18647
		public TerrainDef defaultAffordanceTerrain;

		// Token: 0x040048D8 RID: 18648
		public ThingDef wallLamp;
	}
}
