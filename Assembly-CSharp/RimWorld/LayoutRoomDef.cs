using System;
using System.Collections.Generic;
using RimWorld.SketchGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F90 RID: 3984
	public class LayoutRoomDef : Def
	{
		// Token: 0x17001165 RID: 4453
		// (get) Token: 0x060065B7 RID: 26039 RVA: 0x00243374 File Offset: 0x00241574
		public RoomContentsWorker ContentsWorker
		{
			get
			{
				return this.GetWorker(ref this.workerInt);
			}
		}

		// Token: 0x060065B8 RID: 26040 RVA: 0x00243384 File Offset: 0x00241584
		public bool CanResolve(LayoutRoom room)
		{
			int area = room.Area;
			CellRect cellRect;
			return (room.requiredDef == this || !this.dontPlaceRandomly) && ((this.minSingleRectHeight <= 0 && this.minSingleRectWidth <= 0) || room.TryGetRectOfSize(this.minSingleRectWidth, this.minSingleRectHeight, out cellRect)) && this.SatisfiesMinAdjRooms(room) && (area >= this.areaSizeRange.min && area <= this.areaSizeRange.max && room.connections.Count >= this.minConnectedRooms) && (!this.requiresSingleRectRoom || room.rects.Count == 1);
		}

		// Token: 0x060065B9 RID: 26041 RVA: 0x0024342C File Offset: 0x0024162C
		private bool SatisfiesMinAdjRooms(LayoutRoom room)
		{
			if (this.minAdjRooms <= 0)
			{
				return true;
			}
			if (room.connections.Count < this.minAdjRooms)
			{
				return false;
			}
			int num = 0;
			foreach (LayoutRoom layoutRoom in room.connections)
			{
				int num2 = num;
				foreach (CellRect cellRect in layoutRoom.rects)
				{
					foreach (CellRect other in room.rects)
					{
						if (cellRect.OverlapsCardinal(other))
						{
							num++;
							break;
						}
					}
					if (num != num2)
					{
						break;
					}
				}
				if (num >= this.minAdjRooms)
				{
					break;
				}
			}
			return num >= this.minAdjRooms;
		}

		// Token: 0x060065BA RID: 26042 RVA: 0x00243548 File Offset: 0x00241748
		public void ResolveSketch(LayoutRoomParams parms)
		{
			SketchResolveParams parms2 = default(SketchResolveParams);
			foreach (CellRect cellRect in parms.room.rects)
			{
				if (!this.floorTypes.NullOrEmpty<TerrainDef>())
				{
					TerrainDef def = this.floorTypes.RandomElement<TerrainDef>();
					foreach (IntVec3 pos in cellRect)
					{
						parms.sketch.AddTerrain(def, pos, true);
					}
				}
				if (this.edgeTerrain != null)
				{
					foreach (IntVec3 intVec in cellRect.ContractedBy(1).EdgeCells)
					{
						bool flag = true;
						foreach (CellRect lhs in parms.room.rects)
						{
							if (!(lhs == cellRect) && lhs.ContractedBy(1).Contains(intVec))
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							parms.sketch.AddTerrain(this.edgeTerrain, intVec, true);
						}
					}
				}
				parms2.rect = new CellRect?(cellRect);
				parms2.sketch = parms.sketch;
				if (this.sketchResolverDef != null)
				{
					this.sketchResolverDef.Resolve(parms2);
				}
			}
		}

		// Token: 0x060065BB RID: 26043 RVA: 0x00243740 File Offset: 0x00241940
		public void PreResolveContents(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			RoomContentsWorker contentsWorker = this.ContentsWorker;
			if (contentsWorker == null)
			{
				return;
			}
			contentsWorker.PreFillRooms(map, room, faction, threatPoints);
		}

		// Token: 0x060065BC RID: 26044 RVA: 0x00243757 File Offset: 0x00241957
		public void PostResolveContents(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			RoomContentsWorker contentsWorker = this.ContentsWorker;
			if (contentsWorker == null)
			{
				return;
			}
			contentsWorker.PostFillRooms(map, room, faction, threatPoints);
		}

		// Token: 0x060065BD RID: 26045 RVA: 0x0024376E File Offset: 0x0024196E
		public void ResolveContents(Map map, LayoutRoom room, float? threatPoints = null, Faction faction = null)
		{
			RoomContentsWorker contentsWorker = this.ContentsWorker;
			if (contentsWorker == null)
			{
				return;
			}
			contentsWorker.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x060065BE RID: 26046 RVA: 0x00243785 File Offset: 0x00241985
		private RoomContentsWorker GetWorker(ref RoomContentsWorker worker)
		{
			if (this.roomContentsWorkerType == null)
			{
				return null;
			}
			if (worker != null)
			{
				return worker;
			}
			worker = (RoomContentsWorker)Activator.CreateInstance(this.roomContentsWorkerType);
			worker.Initialize(this);
			return worker;
		}

		// Token: 0x04003CB3 RID: 15539
		public SketchResolverDef sketchResolverDef;

		// Token: 0x04003CB4 RID: 15540
		public IntRange areaSizeRange = new IntRange(25, int.MaxValue);

		// Token: 0x04003CB5 RID: 15541
		public bool requiresSingleRectRoom;

		// Token: 0x04003CB6 RID: 15542
		public List<TerrainDef> floorTypes;

		// Token: 0x04003CB7 RID: 15543
		public TerrainDef edgeTerrain;

		// Token: 0x04003CB8 RID: 15544
		public int minSingleRectWidth;

		// Token: 0x04003CB9 RID: 15545
		public int minSingleRectHeight;

		// Token: 0x04003CBA RID: 15546
		public Type roomContentsWorkerType = typeof(RoomContentsWorker);

		// Token: 0x04003CBB RID: 15547
		public bool canBeInMixedRoom;

		// Token: 0x04003CBC RID: 15548
		public bool dontPlaceRandomly;

		// Token: 0x04003CBD RID: 15549
		public bool isValidPlayerSpawnRoom = true;

		// Token: 0x04003CBE RID: 15550
		public bool dontDestroyWallsDoors;

		// Token: 0x04003CBF RID: 15551
		public SimpleCurve threatPointsScaleCurve;

		// Token: 0x04003CC0 RID: 15552
		public int minConnectedRooms;

		// Token: 0x04003CC1 RID: 15553
		public int minAdjRooms;

		// Token: 0x04003CC2 RID: 15554
		public bool spawnJunk = true;

		// Token: 0x04003CC3 RID: 15555
		public bool canMergeWithAdjacentRoom;

		// Token: 0x04003CC4 RID: 15556
		public bool canRemoveBorderDoors;

		// Token: 0x04003CC5 RID: 15557
		public bool canRemoveBorderWalls;

		// Token: 0x04003CC6 RID: 15558
		public RoofDef roofDef;

		// Token: 0x04003CC7 RID: 15559
		public bool noRoof;

		// Token: 0x04003CC8 RID: 15560
		public FloatRange? itemsPer100CellsRange;

		// Token: 0x04003CC9 RID: 15561
		public ThingSetMakerDef thingSetMakerDef;

		// Token: 0x04003CCA RID: 15562
		public List<LayoutScatterParms> scatter = new List<LayoutScatterParms>();

		// Token: 0x04003CCB RID: 15563
		public List<LayoutScatterTerrainParms> scatterTerrain = new List<LayoutScatterTerrainParms>();

		// Token: 0x04003CCC RID: 15564
		public List<LayoutFillEdgesParms> fillEdges = new List<LayoutFillEdgesParms>();

		// Token: 0x04003CCD RID: 15565
		public List<LayoutFillInteriorParms> fillInterior = new List<LayoutFillInteriorParms>();

		// Token: 0x04003CCE RID: 15566
		public List<LayoutWallAttatchmentParms> wallAttachments = new List<LayoutWallAttatchmentParms>();

		// Token: 0x04003CCF RID: 15567
		public List<LayoutPrefabParms> prefabs = new List<LayoutPrefabParms>();

		// Token: 0x04003CD0 RID: 15568
		public List<LayoutPartParms> parts = new List<LayoutPartParms>();

		// Token: 0x04003CD1 RID: 15569
		[Unsaved(false)]
		private RoomContentsWorker workerInt;
	}
}
