using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001229 RID: 4649
	public class RoomContentsWorker
	{
		// Token: 0x1700139D RID: 5021
		// (get) Token: 0x0600735A RID: 29530 RVA: 0x00286120 File Offset: 0x00284320
		// (set) Token: 0x0600735B RID: 29531 RVA: 0x00286128 File Offset: 0x00284328
		public LayoutRoomDef RoomDef { get; private set; }

		// Token: 0x1700139E RID: 5022
		// (get) Token: 0x0600735C RID: 29532 RVA: 0x00286131 File Offset: 0x00284331
		// (set) Token: 0x0600735D RID: 29533 RVA: 0x00286139 File Offset: 0x00284339
		protected virtual string ThreatSignal { get; set; }

		// Token: 0x0600735E RID: 29534 RVA: 0x00286142 File Offset: 0x00284342
		public void Initialize(LayoutRoomDef roomDef)
		{
			this.RoomDef = roomDef;
		}

		// Token: 0x0600735F RID: 29535 RVA: 0x0028614B File Offset: 0x0028434B
		private bool GenerateRoof(LayoutRoom room)
		{
			return !this.RoomDef.noRoof && !room.sketch.layoutDef.noRoof;
		}

		// Token: 0x06007360 RID: 29536 RVA: 0x0028616F File Offset: 0x0028436F
		private IEnumerable<LayoutScatterParms> ScatterParms(LayoutRoom room)
		{
			RoomContentsWorker.<ScatterParms>d__11 <ScatterParms>d__ = new RoomContentsWorker.<ScatterParms>d__11(-2);
			<ScatterParms>d__.<>3__room = room;
			return <ScatterParms>d__;
		}

		// Token: 0x06007361 RID: 29537 RVA: 0x0028617F File Offset: 0x0028437F
		private IEnumerable<LayoutScatterTerrainParms> ScatterTerrainParms(LayoutRoom room)
		{
			RoomContentsWorker.<ScatterTerrainParms>d__12 <ScatterTerrainParms>d__ = new RoomContentsWorker.<ScatterTerrainParms>d__12(-2);
			<ScatterTerrainParms>d__.<>3__room = room;
			return <ScatterTerrainParms>d__;
		}

		// Token: 0x06007362 RID: 29538 RVA: 0x0028618F File Offset: 0x0028438F
		private IEnumerable<LayoutFillEdgesParms> FillEdgeParms(LayoutRoom room)
		{
			RoomContentsWorker.<FillEdgeParms>d__13 <FillEdgeParms>d__ = new RoomContentsWorker.<FillEdgeParms>d__13(-2);
			<FillEdgeParms>d__.<>3__room = room;
			return <FillEdgeParms>d__;
		}

		// Token: 0x06007363 RID: 29539 RVA: 0x0028619F File Offset: 0x0028439F
		private IEnumerable<LayoutFillInteriorParms> FillInteriorParms(LayoutRoom room)
		{
			RoomContentsWorker.<FillInteriorParms>d__14 <FillInteriorParms>d__ = new RoomContentsWorker.<FillInteriorParms>d__14(-2);
			<FillInteriorParms>d__.<>3__room = room;
			return <FillInteriorParms>d__;
		}

		// Token: 0x06007364 RID: 29540 RVA: 0x002861AF File Offset: 0x002843AF
		private IEnumerable<LayoutWallAttatchmentParms> WallAttachmentParms(LayoutRoom room)
		{
			RoomContentsWorker.<WallAttachmentParms>d__15 <WallAttachmentParms>d__ = new RoomContentsWorker.<WallAttachmentParms>d__15(-2);
			<WallAttachmentParms>d__.<>3__room = room;
			return <WallAttachmentParms>d__;
		}

		// Token: 0x06007365 RID: 29541 RVA: 0x002861BF File Offset: 0x002843BF
		private IEnumerable<LayoutPrefabParms> PrefabParms(LayoutRoom room)
		{
			RoomContentsWorker.<PrefabParms>d__16 <PrefabParms>d__ = new RoomContentsWorker.<PrefabParms>d__16(-2);
			<PrefabParms>d__.<>3__room = room;
			return <PrefabParms>d__;
		}

		// Token: 0x06007366 RID: 29542 RVA: 0x002861CF File Offset: 0x002843CF
		private IEnumerable<LayoutPartParms> PartParms(LayoutRoom room)
		{
			RoomContentsWorker.<PartParms>d__17 <PartParms>d__ = new RoomContentsWorker.<PartParms>d__17(-2);
			<PartParms>d__.<>3__room = room;
			return <PartParms>d__;
		}

		// Token: 0x06007367 RID: 29543 RVA: 0x002861DF File Offset: 0x002843DF
		public virtual void PreFillRooms(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
		}

		// Token: 0x06007368 RID: 29544 RVA: 0x002861E1 File Offset: 0x002843E1
		public virtual void PostFillRooms(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			this.TrySpawnParts(map, room, faction, threatPoints, true);
		}

		// Token: 0x06007369 RID: 29545 RVA: 0x002861F0 File Offset: 0x002843F0
		public virtual void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			if (this.RoomDef == null)
			{
				return;
			}
			this.TrySetRoof(map, room);
			this.RemoveWalls(map, room);
			this.TryScatterTerrain(map, room);
			this.TrySpawnParts(map, room, faction, threatPoints, false);
			this.TryPlacePrefabs(map, room, faction);
			this.TryFillContents(map, room, faction);
			this.TryScatterContents(map, room, faction);
			this.TryPlaceWallAttachments(map, room, faction);
		}

		// Token: 0x0600736A RID: 29546 RVA: 0x00286250 File Offset: 0x00284450
		private void TrySetRoof(Map map, LayoutRoom room)
		{
			if (!this.GenerateRoof(room))
			{
				return;
			}
			foreach (CellRect cellRect in room.rects)
			{
				foreach (IntVec3 cell in cellRect)
				{
					this.SpawnRoof(map, cell);
				}
			}
		}

		// Token: 0x0600736B RID: 29547 RVA: 0x002862E8 File Offset: 0x002844E8
		protected void SpawnRoof(Map map, IntVec3 cell)
		{
			RoofDef roof = cell.GetRoof(map);
			if (roof == null || !roof.isNatural)
			{
				map.roofGrid.SetRoof(cell, this.RoomDef.roofDef ?? RoofDefOf.RoofConstructed);
			}
		}

		// Token: 0x0600736C RID: 29548 RVA: 0x00286328 File Offset: 0x00284528
		private void RemoveWalls(Map map, LayoutRoom room)
		{
			if (!RoomContentsWorker.CanRemoveWalls(room))
			{
				return;
			}
			foreach (CellRect cellRect in room.rects)
			{
				foreach (IntVec3 intVec in cellRect.EdgeCells)
				{
					if (this.CanRemoveWall(intVec, map, room))
					{
						bool flag = true;
						for (int i = 0; i < 4; i++)
						{
							if ((intVec + GenAdj.CardinalDirections[i]).GetDoor(map) != null)
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							foreach (LayoutRoom layoutRoom in room.sketch.structureLayout.Rooms)
							{
								if (!RoomContentsWorker.CanRemoveWalls(layoutRoom) && layoutRoom.Contains(intVec, 0))
								{
									flag = false;
									break;
								}
							}
							if (flag)
							{
								Building building = map.edificeGrid[intVec];
								if (building != null && building.def.IsWall)
								{
									building.Destroy(DestroyMode.Vanish);
									map.roofGrid.SetRoof(intVec, null);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600736D RID: 29549 RVA: 0x002864C4 File Offset: 0x002846C4
		private void TryScatterTerrain(Map map, LayoutRoom room)
		{
			float num = (float)room.Area / 100f;
			foreach (LayoutScatterTerrainParms layoutScatterTerrainParms in this.ScatterTerrainParms(room))
			{
				int num2;
				if (layoutScatterTerrainParms.groupCount != IntRange.Invalid)
				{
					num2 = layoutScatterTerrainParms.groupCount.RandomInRange;
				}
				else
				{
					num2 = Mathf.RoundToInt(layoutScatterTerrainParms.groupsPerHundredCells.RandomInRange * num);
				}
				num2 = Mathf.Max(num2, layoutScatterTerrainParms.minGroups);
				for (int i = 0; i < num2; i++)
				{
					this.SpawnIrregularTerrainLump(layoutScatterTerrainParms, map, room, null);
				}
			}
		}

		// Token: 0x0600736E RID: 29550 RVA: 0x00286574 File Offset: 0x00284774
		private void TrySpawnParts(Map map, LayoutRoom room, Faction faction, float? threatPoints, bool post)
		{
			float num;
			if (threatPoints != null)
			{
				SimpleCurve threatPointsScaleCurve = this.RoomDef.threatPointsScaleCurve;
				num = ((threatPointsScaleCurve != null) ? threatPointsScaleCurve.Evaluate(threatPoints.Value) : threatPoints.Value);
			}
			else
			{
				num = 300f;
			}
			foreach (LayoutPartParms layoutPartParms in this.PartParms(room))
			{
				int num2 = 1;
				if (layoutPartParms.def.Worker.FillOnPost == post)
				{
					if (layoutPartParms.countRange != IntRange.Invalid)
					{
						num2 = layoutPartParms.countRange.RandomInRange;
					}
					else if (!Rand.Chance(layoutPartParms.chance))
					{
						continue;
					}
					float threatPoints2 = num;
					if (layoutPartParms.threatPointsRange != IntRange.Invalid)
					{
						threatPoints2 = (float)layoutPartParms.threatPointsRange.RandomInRange;
					}
					for (int i = 0; i < num2; i++)
					{
						layoutPartParms.def.Worker.FillRoom(map, room, faction, threatPoints2);
					}
				}
			}
		}

		// Token: 0x0600736F RID: 29551 RVA: 0x00286684 File Offset: 0x00284884
		private void TryPlacePrefabs(Map map, LayoutRoom room, Faction faction)
		{
			RoomContentsWorker.<>c__DisplayClass26_0 CS$<>8__locals1 = new RoomContentsWorker.<>c__DisplayClass26_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.room = room;
			CS$<>8__locals1.map = map;
			LayoutSketch layoutSketch = CS$<>8__locals1.room.sketch.layoutSketch;
			float num = (float)CS$<>8__locals1.room.Area / 100f;
			float num2 = (float)CS$<>8__locals1.room.rects.Sum((CellRect r) => r.ContractedBy(1).EdgeCellsCount) / 10f;
			CS$<>8__locals1.wallTuple = new Tuple<ThingDef, ThingDef>(layoutSketch.wall, layoutSketch.wallStuff);
			CS$<>8__locals1.doorTuple = new Tuple<ThingDef, ThingDef>(layoutSketch.door, layoutSketch.doorStuff);
			Dictionary<LayoutPrefabParms, int> dictionary = new Dictionary<LayoutPrefabParms, int>();
			List<LayoutPrefabParms> list = new List<LayoutPrefabParms>();
			foreach (LayoutPrefabParms layoutPrefabParms in this.PrefabParms(CS$<>8__locals1.room))
			{
				list.Add(layoutPrefabParms);
				int num3;
				if (layoutPrefabParms.countRange != IntRange.Invalid)
				{
					num3 = layoutPrefabParms.countRange.RandomInRange;
				}
				else if (layoutPrefabParms.def.edgeOnly)
				{
					num3 = Mathf.RoundToInt(layoutPrefabParms.countPerTenEdgeCells.RandomInRange * num2);
				}
				else
				{
					num3 = Mathf.RoundToInt(layoutPrefabParms.countPerHundredCells.RandomInRange * num);
				}
				num3 = Mathf.Min(Mathf.Max(num3, layoutPrefabParms.minMaxRange.TrueMin), layoutPrefabParms.minMaxRange.TrueMax);
				dictionary[layoutPrefabParms] = num3;
			}
			int num4 = 0;
			while (dictionary.Any<KeyValuePair<LayoutPrefabParms, int>>())
			{
				RoomContentsWorker.<>c__DisplayClass26_1 CS$<>8__locals2 = new RoomContentsWorker.<>c__DisplayClass26_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.parms = list[num4];
				num4 = (num4 + 1) % list.Count;
				if (dictionary.ContainsKey(CS$<>8__locals2.parms))
				{
					Dictionary<LayoutPrefabParms, int> dictionary2 = dictionary;
					LayoutPrefabParms parms = CS$<>8__locals2.parms;
					int num5 = dictionary2[parms] - 1;
					dictionary2[parms] = num5;
					if (num5 <= 0)
					{
						dictionary.Remove(CS$<>8__locals2.parms);
					}
					if (CS$<>8__locals2.parms.def.edgeOnly)
					{
						RoomGenUtility.FillPrefabsAroundEdges(CS$<>8__locals2.parms, 1, CS$<>8__locals2.CS$<>8__locals1.room, CS$<>8__locals2.CS$<>8__locals1.map, new Func<IntVec3, Rot4, bool>(CS$<>8__locals2.<TryPlacePrefabs>g__Validator|1), null, 1, true, faction, new Func<PrefabThingData, Tuple<ThingDef, ThingDef>>(CS$<>8__locals2.CS$<>8__locals1.<TryPlacePrefabs>g__OverrideSpawnData|2));
					}
					else
					{
						RoomGenUtility.FillPrefabs(CS$<>8__locals2.parms, 1, CS$<>8__locals2.CS$<>8__locals1.room, CS$<>8__locals2.CS$<>8__locals1.map, new Func<IntVec3, Rot4, bool>(CS$<>8__locals2.<TryPlacePrefabs>g__Validator|1), 1, null, false, false, faction, new Func<PrefabThingData, Tuple<ThingDef, ThingDef>>(CS$<>8__locals2.CS$<>8__locals1.<TryPlacePrefabs>g__OverrideSpawnData|2));
					}
				}
			}
		}

		// Token: 0x06007370 RID: 29552 RVA: 0x00286958 File Offset: 0x00284B58
		private void TryPlaceWallAttachments(Map map, LayoutRoom room, Faction faction)
		{
			RoomContentsWorker.<>c__DisplayClass27_0 CS$<>8__locals1 = new RoomContentsWorker.<>c__DisplayClass27_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.room = room;
			CS$<>8__locals1.map = map;
			using (IEnumerator<LayoutWallAttatchmentParms> enumerator = this.WallAttachmentParms(CS$<>8__locals1.room).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RoomContentsWorker.<>c__DisplayClass27_1 CS$<>8__locals2 = new RoomContentsWorker.<>c__DisplayClass27_1();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					CS$<>8__locals2.parms = enumerator.Current;
					if (CS$<>8__locals2.parms.countRange != IntRange.Invalid)
					{
						RoomGenUtility.SpawnWallAttatchments(CS$<>8__locals2.parms.def, CS$<>8__locals2.CS$<>8__locals1.map, CS$<>8__locals2.CS$<>8__locals1.room, CS$<>8__locals2.parms.countRange, new Predicate<IntVec3, Rot4>(CS$<>8__locals2.<TryPlaceWallAttachments>g__Validator|0), CS$<>8__locals2.parms.stuff, faction);
					}
					else
					{
						RoomGenUtility.SpawnWallAttatchments(CS$<>8__locals2.parms.def, CS$<>8__locals2.CS$<>8__locals1.map, CS$<>8__locals2.CS$<>8__locals1.room, CS$<>8__locals2.parms.spawnChancePerPosition, CS$<>8__locals2.parms.thingsPer10EdgeCells.RandomInRange, new Predicate<IntVec3, Rot4>(CS$<>8__locals2.<TryPlaceWallAttachments>g__Validator|0), CS$<>8__locals2.parms.stuff, faction);
					}
				}
			}
		}

		// Token: 0x06007371 RID: 29553 RVA: 0x00286A94 File Offset: 0x00284C94
		private void TryFillContents(Map map, LayoutRoom room, Faction faction)
		{
			RoomContentsWorker.<>c__DisplayClass28_0 CS$<>8__locals1 = new RoomContentsWorker.<>c__DisplayClass28_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.room = room;
			CS$<>8__locals1.map = map;
			using (IEnumerator<LayoutFillEdgesParms> enumerator = this.FillEdgeParms(CS$<>8__locals1.room).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RoomContentsWorker.<>c__DisplayClass28_1 CS$<>8__locals2 = new RoomContentsWorker.<>c__DisplayClass28_1();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					CS$<>8__locals2.parms = enumerator.Current;
					if (CS$<>8__locals2.parms.countRange != IntRange.Invalid)
					{
						ThingDef def = CS$<>8__locals2.parms.def;
						int randomInRange = CS$<>8__locals2.parms.countRange.RandomInRange;
						IntRange groupCountRange = CS$<>8__locals2.parms.groupCountRange;
						LayoutRoom room2 = CS$<>8__locals2.CS$<>8__locals1.room;
						Map map2 = CS$<>8__locals2.CS$<>8__locals1.map;
						Func<IntVec3, Rot4, CellRect, bool> validator = new Func<IntVec3, Rot4, CellRect, bool>(CS$<>8__locals2.<TryFillContents>g__Validator|0);
						List<Thing> spawned = null;
						ThingDef stuff = CS$<>8__locals2.parms.stuff;
						int padding = CS$<>8__locals2.parms.padding;
						RoomGenUtility.FillAroundEdges(def, randomInRange, groupCountRange, room2, map2, validator, spawned, CS$<>8__locals2.parms.contractedBy, padding, stuff, true, CS$<>8__locals2.parms.rotOffset, null, faction);
					}
					else
					{
						ThingDef def2 = CS$<>8__locals2.parms.def;
						FloatRange groupsPerTenEdgeCells = CS$<>8__locals2.parms.groupsPerTenEdgeCells;
						IntRange groupCountRange2 = CS$<>8__locals2.parms.groupCountRange;
						LayoutRoom room3 = CS$<>8__locals2.CS$<>8__locals1.room;
						Map map3 = CS$<>8__locals2.CS$<>8__locals1.map;
						Func<IntVec3, Rot4, CellRect, bool> validator2 = new Func<IntVec3, Rot4, CellRect, bool>(CS$<>8__locals2.<TryFillContents>g__Validator|0);
						List<Thing> spawned2 = null;
						ThingDef stuff = CS$<>8__locals2.parms.stuff;
						int padding = CS$<>8__locals2.parms.padding;
						RoomGenUtility.FillAroundEdges(def2, groupsPerTenEdgeCells, groupCountRange2, room3, map3, validator2, spawned2, CS$<>8__locals2.parms.contractedBy, padding, stuff, true, CS$<>8__locals2.parms.rotOffset, null, faction);
					}
				}
			}
			using (IEnumerator<LayoutFillInteriorParms> enumerator2 = this.FillInteriorParms(CS$<>8__locals1.room).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					RoomContentsWorker.<>c__DisplayClass28_2 CS$<>8__locals3 = new RoomContentsWorker.<>c__DisplayClass28_2();
					CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals1;
					CS$<>8__locals3.parms = enumerator2.Current;
					if (CS$<>8__locals3.parms.countRange != IntRange.Invalid)
					{
						ThingDef def3 = CS$<>8__locals3.parms.def;
						int randomInRange2 = CS$<>8__locals3.parms.countRange.RandomInRange;
						LayoutRoom room4 = CS$<>8__locals3.CS$<>8__locals2.room;
						Map map4 = CS$<>8__locals3.CS$<>8__locals2.map;
						Rot4? fixedRot = CS$<>8__locals3.parms.fixedRot;
						Func<IntVec3, Rot4, CellRect, bool> validator3 = new Func<IntVec3, Rot4, CellRect, bool>(CS$<>8__locals3.<TryFillContents>g__Validator|1);
						List<Thing> spawned3 = null;
						ThingDef stuff = CS$<>8__locals3.parms.stuff;
						RoomGenUtility.FillWithPadding(def3, randomInRange2, room4, map4, fixedRot, validator3, spawned3, CS$<>8__locals3.parms.contractedBy, stuff, CS$<>8__locals3.parms.alignWithRect, CS$<>8__locals3.parms.snapToGrid, null, faction);
					}
					else
					{
						ThingDef def4 = CS$<>8__locals3.parms.def;
						FloatRange thingsPerHundredCells = CS$<>8__locals3.parms.thingsPerHundredCells;
						LayoutRoom room5 = CS$<>8__locals3.CS$<>8__locals2.room;
						Map map5 = CS$<>8__locals3.CS$<>8__locals2.map;
						Rot4? fixedRot2 = CS$<>8__locals3.parms.fixedRot;
						Func<IntVec3, Rot4, CellRect, bool> validator4 = new Func<IntVec3, Rot4, CellRect, bool>(CS$<>8__locals3.<TryFillContents>g__Validator|1);
						List<Thing> spawned4 = null;
						ThingDef stuff = CS$<>8__locals3.parms.stuff;
						RoomGenUtility.FillWithPadding(def4, thingsPerHundredCells, room5, map5, fixedRot2, validator4, spawned4, CS$<>8__locals3.parms.contractedBy, stuff, CS$<>8__locals3.parms.alignWithRect, CS$<>8__locals3.parms.snapToGrid, null, faction);
					}
				}
			}
		}

		// Token: 0x06007372 RID: 29554 RVA: 0x00286DD0 File Offset: 0x00284FD0
		private void TryScatterContents(Map map, LayoutRoom room, Faction faction)
		{
			float num = (float)room.Area / 100f;
			foreach (LayoutScatterParms layoutScatterParms in this.ScatterParms(room))
			{
				int num2;
				if (layoutScatterParms.groupCount != IntRange.Invalid)
				{
					num2 = layoutScatterParms.groupCount.RandomInRange;
				}
				else
				{
					num2 = Mathf.RoundToInt(layoutScatterParms.groupsPerHundredCells.RandomInRange * num);
				}
				num2 = Mathf.Max(num2, layoutScatterParms.minGroups);
				for (int i = 0; i < num2; i++)
				{
					this.SpawnIrregularLump(layoutScatterParms, map, room, faction, null, null);
				}
			}
		}

		// Token: 0x06007373 RID: 29555 RVA: 0x00286E84 File Offset: 0x00285084
		private void SpawnIrregularLump(LayoutScatterParms parms, Map map, LayoutRoom room, Faction faction, List<IntVec3> area = null, List<Thing> spawned = null)
		{
			IntVec3 pos;
			if (!room.TryGetRandomCellInRoom(map, out pos, 2, 0, (IntVec3 c) => this.IsValidScatterCell(parms, c, room, map), false))
			{
				return;
			}
			GenSpawn.SpawnIrregularLump(parms.def, pos, map, parms.itemsPerGroup, parms.groupDistRange, WipeMode.Vanish, (IntVec3 c) => this.IsValidScatterCell(parms, c, room, map), area, spawned, parms.stuff, faction);
		}

		// Token: 0x06007374 RID: 29556 RVA: 0x00286F28 File Offset: 0x00285128
		private void SpawnIrregularTerrainLump(LayoutScatterTerrainParms parms, Map map, LayoutRoom room, List<IntVec3> area = null)
		{
			RoomContentsWorker.<>c__DisplayClass31_0 CS$<>8__locals1 = new RoomContentsWorker.<>c__DisplayClass31_0();
			CS$<>8__locals1.room = room;
			IntVec3 center;
			if (!CS$<>8__locals1.room.TryGetRandomCellInRoom(map, out center, 2, 0, new Func<IntVec3, bool>(CS$<>8__locals1.<SpawnIrregularTerrainLump>g__Validator|0), false))
			{
				return;
			}
			int randomInRange = parms.groupDistRange.RandomInRange;
			int randomInRange2 = parms.itemsPerGroup.RandomInRange;
			int num = 0;
			List<IntVec3> list = GridShapeMaker.IrregularLump(center, map, randomInRange, new Predicate<IntVec3>(CS$<>8__locals1.<SpawnIrregularTerrainLump>g__Validator|0));
			for (int i = 0; i < list.Count; i++)
			{
				IntVec3 intVec = list[i];
				if (Rand.DynamicChance(num, randomInRange2, list.Count - i))
				{
					num++;
					map.terrainGrid.SetTerrain(intVec, parms.def);
					if (area != null)
					{
						area.Add(intVec);
					}
				}
			}
		}

		// Token: 0x06007375 RID: 29557 RVA: 0x00286FF0 File Offset: 0x002851F0
		protected virtual bool IsValidCellBase(ThingDef thing, ThingDef stuff, IntVec3 cell, LayoutRoom room, Map map)
		{
			TerrainAffordanceDef terrainAffordanceNeed = thing.GetTerrainAffordanceNeed(stuff);
			return room.Contains(cell, 1) && !cell.IsBuildingInteractionCell(map) && (terrainAffordanceNeed == null || cell.GetAffordances(map).Contains(terrainAffordanceNeed)) && (thing.passability != Traversability.Impassable || cell.GetFirstPawn(map) == null) && cell.GetFirstBuilding(map) == null && (thing.IsFilth || !RoomGenUtility.IsDoorAdjacentTo(cell, map, true)) && (!cell.GetTerrain(map).exposesToVacuum || cell.GetRoof(map) != null);
		}

		// Token: 0x06007376 RID: 29558 RVA: 0x0028707C File Offset: 0x0028527C
		protected virtual bool IsValidScatterCell(LayoutScatterParms parms, IntVec3 cell, LayoutRoom room, Map map)
		{
			if (parms.def.passability == Traversability.Impassable && !RoomGenUtility.IsClearAndNotAdjacentToDoor(parms.def, cell, map, Rot4.North))
			{
				return false;
			}
			foreach (Thing thing in cell.GetThingList(map))
			{
				if (parms.def.IsFilth && thing.def.IsFilth)
				{
					return false;
				}
				if (!parms.def.IsFilth && !thing.def.IsFilth)
				{
					return false;
				}
			}
			return this.IsValidCellBase(parms.def, parms.stuff, cell, room, map);
		}

		// Token: 0x06007377 RID: 29559 RVA: 0x00287144 File Offset: 0x00285344
		protected virtual bool IsValidFillEdgeCell(LayoutFillEdgesParms parms, IntVec3 cell, Rot4 rot, LayoutRoom room, Map map)
		{
			CellRect cellRect = cell.RectAbout(parms.def.Size, rot);
			foreach (IntVec3 cell2 in cellRect)
			{
				if (!this.IsValidCellBase(parms.def, parms.stuff, cell2, room, map))
				{
					return false;
				}
			}
			if (parms.def.Fillage != FillCategory.Full)
			{
				return true;
			}
			foreach (IntVec3 intVec in cellRect.ExpandedBy(1).EdgeCells)
			{
				if (room.Contains(intVec, 2) && !intVec.Standable(map))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06007378 RID: 29560 RVA: 0x00287234 File Offset: 0x00285434
		protected virtual bool IsValidPrefabRect(LayoutPrefabParms parms, IntVec3 cell, Rot4 rot, LayoutRoom room, Map map)
		{
			CellRect cellRect = cell.RectAbout(parms.def.size, rot);
			RoomContentsWorker.tmpOccupiedEdges.Clear();
			foreach (ValueTuple<PrefabThingData, IntVec3, Rot4> valueTuple in PrefabUtility.GetThings(parms.def, cell, rot))
			{
				PrefabThingData item = valueTuple.Item1;
				IntVec3 item2 = valueTuple.Item2;
				Rot4 item3 = valueTuple.Item3;
				foreach (IntVec3 intVec in item2.RectAbout(item.def.Size, item3).Cells)
				{
					if (!this.IsValidCellBase(item.def, item.stuff, intVec, room, map))
					{
						return false;
					}
					if (cellRect.EdgeCells.Contains(intVec) && !intVec.Standable(map))
					{
						for (int i = 0; i < 8; i++)
						{
							IntVec3 intVec2 = intVec + GenAdj.AdjacentCellsAround[i];
							if (!cellRect.Contains(intVec2) && !intVec2.Standable(map) && !cellRect.ExpandedBy(1).IsCorner(intVec2) && !RoomContentsWorker.tmpOccupiedEdges.Contains(intVec2))
							{
								RoomContentsWorker.tmpOccupiedEdges.Add(intVec2);
							}
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06007379 RID: 29561 RVA: 0x002873D4 File Offset: 0x002855D4
		protected virtual bool IsValidFillInteriorCell(LayoutFillInteriorParms parms, IntVec3 cell, Rot4 rot, LayoutRoom room, Map map)
		{
			CellRect cellRect = cell.RectAbout(parms.def.Size, rot);
			foreach (IntVec3 cell2 in cellRect)
			{
				if (!this.IsValidCellBase(parms.def, parms.stuff, cell2, room, map))
				{
					return false;
				}
			}
			if (parms.def.Fillage != FillCategory.Full)
			{
				return true;
			}
			foreach (IntVec3 intVec in cellRect.ExpandedBy(1).EdgeCells)
			{
				if (room.Contains(intVec, 2) && !intVec.Standable(map))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600737A RID: 29562 RVA: 0x002874C4 File Offset: 0x002856C4
		protected virtual bool IsValidWallAttachmentCell(LayoutWallAttatchmentParms parms, IntVec3 cell, Rot4 rot, LayoutRoom room, Map map)
		{
			Building edifice = (cell + rot.FacingCell).GetEdifice(map);
			return ((edifice != null) ? edifice.def : null) == room.sketch.layoutSketch.wall;
		}

		// Token: 0x0600737B RID: 29563 RVA: 0x002874FC File Offset: 0x002856FC
		protected virtual bool CanRemoveWall(IntVec3 cell, Map map, LayoutRoom room)
		{
			return true;
		}

		// Token: 0x0600737C RID: 29564 RVA: 0x00287500 File Offset: 0x00285700
		private static bool CanRemoveWalls(LayoutRoom room)
		{
			using (List<LayoutRoomDef>.Enumerator enumerator = room.defs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.canRemoveBorderWalls)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x040048E5 RID: 18661
		private const int DefaultThreatPoints = 300;

		// Token: 0x040048E8 RID: 18664
		private static readonly List<IntVec3> tmpOccupiedEdges = new List<IntVec3>();
	}
}
