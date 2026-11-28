using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DelaunatorSharp;
using RimWorld;
using RimWorld.BaseGen;
using UnityEngine;
using Verse;

// Token: 0x02000015 RID: 21
public abstract class LayoutWorker_Structure : LayoutWorker
{
	// Token: 0x17000011 RID: 17
	// (get) Token: 0x0600007C RID: 124 RVA: 0x00006354 File Offset: 0x00004554
	public new StructureLayoutDef Def
	{
		get
		{
			return (StructureLayoutDef)base.Def;
		}
	}

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x0600007D RID: 125 RVA: 0x00006361 File Offset: 0x00004561
	protected virtual float RoomToExteriorDoorRatio
	{
		get
		{
			return this.Def.roomToExteriorDoorRatio;
		}
	}

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x0600007E RID: 126 RVA: 0x0000636E File Offset: 0x0000456E
	protected virtual bool CanConnectRoomsExternally
	{
		get
		{
			return this.Def.canConnectRoomsExternally;
		}
	}

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x0600007F RID: 127 RVA: 0x0000637B File Offset: 0x0000457B
	protected virtual TerrainDef Terrain
	{
		get
		{
			return this.Def.terrainDef;
		}
	}

	// Token: 0x06000080 RID: 128 RVA: 0x00006388 File Offset: 0x00004588
	protected virtual ThingDef GetWallDoorStuff(StructureGenParams parms)
	{
		return BaseGenUtility.RandomCheapWallStuff(parms.faction ?? Faction.OfAncients, true);
	}

	// Token: 0x17000015 RID: 21
	// (get) Token: 0x06000081 RID: 129 RVA: 0x0000639F File Offset: 0x0000459F
	protected virtual ThingDef WallStuff
	{
		get
		{
			return this.Def.wallStuffDef;
		}
	}

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x06000082 RID: 130 RVA: 0x000063AC File Offset: 0x000045AC
	protected virtual ThingDef DoorStuff
	{
		get
		{
			return this.Def.doorStuffDef;
		}
	}

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x06000083 RID: 131 RVA: 0x000063B9 File Offset: 0x000045B9
	protected virtual ThingDef WallThing
	{
		get
		{
			return this.Def.wallDef;
		}
	}

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x06000084 RID: 132 RVA: 0x000063C6 File Offset: 0x000045C6
	protected virtual ThingDef DoorThing
	{
		get
		{
			return this.Def.doorDef;
		}
	}

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x06000085 RID: 133 RVA: 0x000063D3 File Offset: 0x000045D3
	protected virtual ThingDef WallLampThing
	{
		get
		{
			return this.Def.wallLampDef;
		}
	}

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x06000086 RID: 134 RVA: 0x000063E0 File Offset: 0x000045E0
	protected virtual TerrainDef DefaultAffordanceTerrain
	{
		get
		{
			return this.Def.defaultAffordanceTerrainDef;
		}
	}

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x06000087 RID: 135 RVA: 0x000063ED File Offset: 0x000045ED
	protected virtual ThingDef ForceExteriorDoor
	{
		get
		{
			return this.Def.exteriorDoorDef;
		}
	}

	// Token: 0x1700001C RID: 28
	// (get) Token: 0x06000088 RID: 136 RVA: 0x000063FA File Offset: 0x000045FA
	protected virtual ThingDef ForceExteriorDoorStuff
	{
		get
		{
			return this.Def.exteriorDoorStuffDef;
		}
	}

	// Token: 0x1700001D RID: 29
	// (get) Token: 0x06000089 RID: 137 RVA: 0x00006407 File Offset: 0x00004607
	protected virtual bool SpawnDoors
	{
		get
		{
			return this.Def.spawnDoors;
		}
	}

	// Token: 0x1700001E RID: 30
	// (get) Token: 0x0600008A RID: 138 RVA: 0x00006414 File Offset: 0x00004614
	protected virtual LayoutRoomDef ImportantRoomDef
	{
		get
		{
			return this.Def.importantRoomDef;
		}
	}

	// Token: 0x1700001F RID: 31
	// (get) Token: 0x0600008B RID: 139 RVA: 0x00006421 File Offset: 0x00004621
	protected virtual TerrainDef SurroundingTerrain
	{
		get
		{
			return this.Def.surroundingTerrainDef;
		}
	}

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x0600008C RID: 140 RVA: 0x0000642E File Offset: 0x0000462E
	protected virtual TerrainDef SurroundingScatterTerrain
	{
		get
		{
			return this.Def.surroundingScatterTerrainDef;
		}
	}

	// Token: 0x0600008D RID: 141 RVA: 0x0000643B File Offset: 0x0000463B
	public LayoutWorker_Structure(LayoutDef def) : base(def)
	{
	}

	// Token: 0x0600008E RID: 142 RVA: 0x00006444 File Offset: 0x00004644
	protected override LayoutSketch GenerateSketch(StructureGenParams parms)
	{
		ThingDef wallDoorStuff = this.GetWallDoorStuff(parms);
		ThingDef thingDef = this.WallThing ?? ThingDefOf.Wall;
		ThingDef thingDef2 = this.DoorThing ?? ThingDefOf.Door;
		LayoutSketch layoutSketch = new LayoutSketch
		{
			floor = (this.Terrain ?? TerrainDefOf.AncientConcrete),
			wall = thingDef,
			door = thingDef2,
			wallStuff = (this.WallStuff ?? ((wallDoorStuff == null || !thingDef.MadeFromStuff) ? null : wallDoorStuff)),
			doorStuff = (this.DoorStuff ?? ((wallDoorStuff == null || !thingDef2.MadeFromStuff) ? null : wallDoorStuff)),
			wallLamp = this.WallLampThing,
			defaultAffordanceTerrain = this.DefaultAffordanceTerrain
		};
		using (new ProfilerBlock("GenerateStructure()"))
		{
			layoutSketch.structureLayout = this.GenerateStructure(parms);
		}
		return layoutSketch;
	}

	// Token: 0x0600008F RID: 143 RVA: 0x00006534 File Offset: 0x00004734
	protected virtual StructureLayout GenerateStructure(StructureGenParams parms)
	{
		CellRect rect = new CellRect(0, 0, parms.size.x, parms.size.z);
		StructureLayout structureLayout = this.GetStructureLayout(parms, rect);
		structureLayout.sketch = parms.sketch;
		using (new ProfilerBlock("Generate Graphs"))
		{
			LayoutWorker_Structure.GenerateGraphs(structureLayout);
		}
		this.PostGraphsGenerated(structureLayout, parms);
		if (this.SpawnDoors)
		{
			this.CreateDoors(structureLayout);
		}
		return structureLayout;
	}

	// Token: 0x06000090 RID: 144 RVA: 0x000065C0 File Offset: 0x000047C0
	protected virtual void PostGraphsGenerated(StructureLayout layout, StructureGenParams parms)
	{
		LayoutRoomDef importantRoomDef = this.ImportantRoomDef;
		if (importantRoomDef == null || !parms.spawnImportantRoom)
		{
			return;
		}
		foreach (LayoutRoom layoutRoom in from r in layout.Rooms.Where(delegate(LayoutRoom r)
		{
			CellRect cellRect;
			return r.requiredDef == null && r.TryGetRectOfSize(7, 7, out cellRect);
		})
		orderby r.Area descending
		select r)
		{
			foreach (ValueTuple<LayoutRoom, CellRect, CellRect> valueTuple in layout.GetLogicalRoomConnections(layoutRoom))
			{
				LayoutRoom item = valueTuple.Item1;
				if (layoutRoom.IsAdjacentTo(item, 2))
				{
					layoutRoom.requiredDef = importantRoomDef;
					layoutRoom.noExteriorDoors = true;
					return;
				}
			}
		}
		LayoutRoom layoutRoom2 = (from r in layout.Rooms
		orderby r.Area descending
		select r).First<LayoutRoom>();
		layoutRoom2.requiredDef = importantRoomDef;
		layoutRoom2.noExteriorDoors = true;
	}

	// Token: 0x06000091 RID: 145
	protected abstract StructureLayout GetStructureLayout(StructureGenParams parms, CellRect rect);

	// Token: 0x06000092 RID: 146 RVA: 0x000066F4 File Offset: 0x000048F4
	public override void Spawn(LayoutStructureSketch layoutStructureSketch, Map map, IntVec3 pos, float? threatPoints = null, List<Thing> allSpawnedThings = null, bool roofs = true, bool canReuseSketch = false, Faction faction = null)
	{
		object obj = this.Def.wallDamageRange != IntRange.Invalid || this.Def.clearDoorFaction || this.Def.ensureOneDoorUnlocked;
		ListerBuildings.TrackingScope trackingScope = null;
		object obj2 = obj;
		if (obj2 != null)
		{
			trackingScope = map.listerBuildings.Track(null);
		}
		base.Spawn(layoutStructureSketch, map, pos, threatPoints, allSpawnedThings, false, canReuseSketch, faction);
		if (obj2 != null)
		{
			trackingScope.Stop();
			bool flag = false;
			foreach (Building building in trackingScope)
			{
				if (building.def.IsWall && this.Def.wallDamageRange != IntRange.Invalid)
				{
					building.HitPoints -= this.Def.wallDamageRange.RandomInRange;
				}
				if (building is Building_Door && this.Def.clearDoorFaction)
				{
					building.SetFaction(null, null);
				}
				if (this.Def.ensureOneDoorUnlocked && !flag)
				{
					Building_HackableDoor building_HackableDoor = building as Building_HackableDoor;
					if (building_HackableDoor != null && this.IsOnExterior(layoutStructureSketch.structureLayout, building_HackableDoor.Position))
					{
						building_HackableDoor.Hackable.HackNow();
						flag = true;
					}
				}
			}
		}
		if (trackingScope != null)
		{
			trackingScope.Dispose();
		}
		this.PlaceSurroundingTerrain(layoutStructureSketch, map, faction);
		this.ScatterPrefabs(map, layoutStructureSketch.structureLayout.container);
	}

	// Token: 0x06000093 RID: 147 RVA: 0x00006860 File Offset: 0x00004A60
	private void ScatterPrefabs(Map map, CellRect structure)
	{
		MapGenUtility.SpawnScatteredGroupPrefabs(map, structure, this.Def.scatteredPrefabs);
	}

	// Token: 0x06000094 RID: 148 RVA: 0x00006874 File Offset: 0x00004A74
	protected override void PostLayoutFlushedToSketch(LayoutStructureSketch parms)
	{
		this.ReplaceExteriorDoors(parms.layoutSketch);
	}

	// Token: 0x06000095 RID: 149 RVA: 0x00006884 File Offset: 0x00004A84
	private static void GenerateGraphs(StructureLayout layout)
	{
		List<Vector2> list = new List<Vector2>();
		foreach (LayoutRoom layoutRoom in layout.Rooms)
		{
			foreach (CellRect cellRect in layoutRoom.rects)
			{
				if (cellRect.Width < 25 && cellRect.Height < 25)
				{
					Vector3 centerVector = cellRect.CenterVector3;
					list.Add(new Vector2(centerVector.x, centerVector.z));
				}
				else
				{
					cellRect.SubdivideToMaxLength(25, LayoutWorker_Structure.TmpSubdivided);
					foreach (CellRect cellRect2 in LayoutWorker_Structure.TmpSubdivided)
					{
						Vector3 centerVector2 = cellRect2.CenterVector3;
						list.Add(new Vector2(centerVector2.x, centerVector2.z));
					}
				}
			}
		}
		if (list.Count < 3)
		{
			return;
		}
		layout.delaunator = new Delaunator(list.ToArray());
		layout.neighbours = new RelativeNeighborhoodGraph(layout.delaunator);
	}

	// Token: 0x06000096 RID: 150 RVA: 0x000069F0 File Offset: 0x00004BF0
	private void CreateDoors(StructureLayout layout)
	{
		foreach (LayoutRoom layoutRoom in layout.Rooms)
		{
			using (IEnumerator<ValueTuple<LayoutRoom, CellRect, CellRect>> enumerator2 = layout.GetLogicalRoomConnections(layoutRoom).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					LayoutWorker_Structure.<>c__DisplayClass43_0 CS$<>8__locals1 = new LayoutWorker_Structure.<>c__DisplayClass43_0();
					ValueTuple<LayoutRoom, CellRect, CellRect> valueTuple = enumerator2.Current;
					LayoutRoom item = valueTuple.Item1;
					CS$<>8__locals1.aRect = valueTuple.Item2;
					CellRect item2 = valueTuple.Item3;
					IntVec3 position;
					if (!layoutRoom.connections.Contains(item) && (LayoutWorker_Structure.TryGetBestDoorCell(layout, CS$<>8__locals1.aRect, item2, out position, new Predicate<IntVec3, IntVec3>(LayoutWorker_Structure.<CreateDoors>g__IsCenterAlignedCell|43_0)) || LayoutWorker_Structure.TryGetBestDoorCell(layout, CS$<>8__locals1.aRect, item2, out position, new Predicate<IntVec3, IntVec3>(CS$<>8__locals1.<CreateDoors>g__NotNearCorner|1)) || LayoutWorker_Structure.TryGetBestDoorCell(layout, CS$<>8__locals1.aRect, item2, out position, null)))
					{
						layout.Add(position, RoomLayoutCellType.Door);
						LayoutWorker_Structure.MarkConnected(layoutRoom, item);
					}
				}
			}
		}
		int num = 0;
		if (this.CanConnectRoomsExternally)
		{
			LayoutWorker_Structure.ConnectViaExteriorDoors(layout, ref num);
		}
		this.PlaceExtraExteriorDoors(layout, ref num);
	}

	// Token: 0x06000097 RID: 151 RVA: 0x00006B34 File Offset: 0x00004D34
	private static void ConnectViaExteriorDoors(StructureLayout layout, ref int exteriorDoors)
	{
		foreach (LayoutRoom layoutRoom in layout.Rooms)
		{
			if (!layoutRoom.noExteriorDoors)
			{
				foreach (ValueTuple<LayoutRoom, CellRect, CellRect> valueTuple in layout.GetLogicalRoomConnections(layoutRoom))
				{
					LayoutRoom item = valueTuple.Item1;
					CellRect item2 = valueTuple.Item2;
					CellRect item3 = valueTuple.Item3;
					if (!layoutRoom.connections.Contains(item) && !item.noExteriorDoors)
					{
						ValueTuple<IntVec3, IntVec3> bestExteriorDoorCells = LayoutWorker_Structure.GetBestExteriorDoorCells(layout, item2, item3);
						IntVec3 item4 = bestExteriorDoorCells.Item1;
						IntVec3 item5 = bestExteriorDoorCells.Item2;
						if (item4.IsValid)
						{
							layout.Add(item4, RoomLayoutCellType.Door);
							foreach (IntVec3 b in GenAdj.CardinalDirectionsAround)
							{
								IntVec3 c = item4 + b;
								if (!layout.container.Contains(c))
								{
									exteriorDoors++;
								}
							}
						}
						if (item5.IsValid)
						{
							layout.Add(item5, RoomLayoutCellType.Door);
							foreach (IntVec3 b2 in GenAdj.CardinalDirectionsAround)
							{
								IntVec3 c2 = item4 + b2;
								if (!layout.container.Contains(c2))
								{
									exteriorDoors++;
								}
							}
						}
						LayoutWorker_Structure.MarkConnected(layoutRoom, item);
					}
				}
			}
		}
	}

	// Token: 0x06000098 RID: 152 RVA: 0x00006CE4 File Offset: 0x00004EE4
	private void PlaceExtraExteriorDoors(StructureLayout layout, ref int exteriorDoors)
	{
		if (this.RoomToExteriorDoorRatio <= 0f)
		{
			return;
		}
		int num = Mathf.Max(Mathf.RoundToInt((float)layout.Rooms.Count * this.RoomToExteriorDoorRatio), 2);
		int num2 = 100;
		while (exteriorDoors < num && num2-- > 0)
		{
			foreach (LayoutRoom layoutRoom in layout.Rooms)
			{
				if (!layoutRoom.noExteriorDoors)
				{
					foreach (CellRect rect in layoutRoom.rects)
					{
						IntVec3 targetCenter = rect.CenterCell + GenAdj.CardinalDirections[Rand.Range(0, 4)] * 50;
						IntVec3 position;
						if (LayoutWorker_Structure.TryGetBestRectExteriorCell(layout, rect, targetCenter, out position, false))
						{
							layout.Add(position, RoomLayoutCellType.Door);
							exteriorDoors++;
							break;
						}
					}
					if (exteriorDoors >= num)
					{
						break;
					}
				}
			}
		}
	}

	// Token: 0x06000099 RID: 153 RVA: 0x00006E10 File Offset: 0x00005010
	private static bool TryGetBestDoorCell(StructureLayout layout, CellRect aRect, CellRect bRect, out IntVec3 cell, Predicate<IntVec3, IntVec3> validator = null)
	{
		float num = float.MaxValue;
		cell = IntVec3.Invalid;
		if (!aRect.ExpandedBy(1).Overlaps(bRect))
		{
			return false;
		}
		IntVec3 centerCell = bRect.CenterCell;
		foreach (IntVec3 intVec in aRect.EdgeCells)
		{
			if ((validator == null || validator(centerCell, intVec)) && !aRect.IsCorner(intVec) && LayoutWorker_Structure.IsGoodForDoor(layout, aRect, bRect, intVec))
			{
				float num2 = centerCell.DistanceTo(intVec);
				if (num2 < num)
				{
					num = num2;
					cell = intVec;
				}
			}
		}
		return cell.IsValid;
	}

	// Token: 0x0600009A RID: 154 RVA: 0x00006ED0 File Offset: 0x000050D0
	private static bool IsGoodForDoor(StructureLayout layout, CellRect aRect, CellRect bRect, IntVec3 pos)
	{
		return LayoutWorker_Structure.IsGoodForHorizontalDoor(layout, aRect, bRect, pos) || LayoutWorker_Structure.IsGoodForVerticalDoor(layout, aRect, bRect, pos);
	}

	// Token: 0x0600009B RID: 155 RVA: 0x00006EE8 File Offset: 0x000050E8
	private static bool IsGoodForHorizontalDoor(StructureLayout layout, CellRect aRect, CellRect bRect, IntVec3 pos)
	{
		return layout.IsGoodForHorizontalDoor(pos) && (bRect.Contains(pos + IntVec3.South) || bRect.Contains(pos + IntVec3.North)) && (aRect.Contains(pos + IntVec3.South) || aRect.Contains(pos + IntVec3.North));
	}

	// Token: 0x0600009C RID: 156 RVA: 0x00006F54 File Offset: 0x00005154
	private static bool IsGoodForVerticalDoor(StructureLayout layout, CellRect aRect, CellRect bRect, IntVec3 pos)
	{
		return layout.IsGoodForVerticalDoor(pos) && (bRect.Contains(pos + IntVec3.East) || bRect.Contains(pos + IntVec3.West)) && (aRect.Contains(pos + IntVec3.East) || aRect.Contains(pos + IntVec3.West));
	}

	// Token: 0x0600009D RID: 157 RVA: 0x00006FC0 File Offset: 0x000051C0
	[return: TupleElementNames(new string[]
	{
		"a",
		"b"
	})]
	private static ValueTuple<IntVec3, IntVec3> GetBestExteriorDoorCells(StructureLayout layout, CellRect aRect, CellRect bRect)
	{
		IntVec3 item;
		LayoutWorker_Structure.TryGetBestRectExteriorCell(layout, aRect, bRect.CenterCell, out item, true);
		IntVec3 item2;
		LayoutWorker_Structure.TryGetBestRectExteriorCell(layout, bRect, aRect.CenterCell, out item2, true);
		return new ValueTuple<IntVec3, IntVec3>(item, item2);
	}

	// Token: 0x0600009E RID: 158 RVA: 0x00006FF8 File Offset: 0x000051F8
	private static bool TryGetBestRectExteriorCell(StructureLayout layout, CellRect rect, IntVec3 targetCenter, out IntVec3 cell, bool desperate)
	{
		foreach (IntVec3 intVec in rect.EdgeCells)
		{
			if (layout.IsGoodForDoor(intVec))
			{
				bool flag = false;
				foreach (IntVec3 b in GenAdj.CardinalDirectionsAround)
				{
					if (!LayoutWorker_Structure.AnyRoomContains(layout, intVec + b) && (desperate || !layout.container.Contains(intVec + b)))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					LayoutWorker_Structure.TmpCells.Add(intVec);
				}
			}
		}
		if (LayoutWorker_Structure.TmpCells.Empty<IntVec3>())
		{
			cell = IntVec3.Invalid;
			return false;
		}
		LayoutWorker_Structure.TmpCells.SortBy((IntVec3 x) => x.DistanceTo(targetCenter));
		cell = LayoutWorker_Structure.TmpCells[0];
		LayoutWorker_Structure.TmpCells.Clear();
		return true;
	}

	// Token: 0x0600009F RID: 159 RVA: 0x00007104 File Offset: 0x00005304
	private static bool AnyRoomContains(StructureLayout layout, IntVec3 cell)
	{
		foreach (LayoutRoom layoutRoom in layout.Rooms)
		{
			foreach (CellRect cellRect in layoutRoom.rects)
			{
				if (cellRect.Contains(cell))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x0000719C File Offset: 0x0000539C
	private static void MarkConnected(LayoutRoom a, LayoutRoom b)
	{
		a.connections.Add(b);
		b.connections.Add(a);
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x000071B8 File Offset: 0x000053B8
	private void ReplaceExteriorDoors(LayoutSketch sketch)
	{
		if (this.ForceExteriorDoor == null)
		{
			return;
		}
		foreach (SketchThing sketchThing in sketch.Things)
		{
			if (sketchThing.def.IsDoor && this.IsOnExterior(sketch.structureLayout, sketchThing.pos))
			{
				sketchThing.def = this.ForceExteriorDoor;
				sketchThing.stuff = this.ForceExteriorDoorStuff;
			}
		}
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x00007248 File Offset: 0x00005448
	private bool IsOnExterior(StructureLayout layout, IntVec3 pos)
	{
		foreach (IntVec3 b in GenAdj.CardinalDirections)
		{
			IntVec3 pos2 = pos + b;
			LayoutRoom layoutRoom;
			if (!layout.TryGetRoom(pos2, out layoutRoom))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x00007288 File Offset: 0x00005488
	private void PlaceSurroundingTerrain(LayoutStructureSketch layoutStructureSketch, Map map, Faction faction)
	{
		if (this.SurroundingTerrain == null)
		{
			return;
		}
		IEnumerable<CellRect> enumerable = layoutStructureSketch.structureLayout.Rooms.SelectMany((LayoutRoom r) => r.rects);
		LayoutWorker_Structure.UsedSurroundingCells.Clear();
		foreach (CellRect cellRect in enumerable)
		{
			int randomInRange = this.Def.surroundingScatterRange.RandomInRange;
			foreach (IntVec3 intVec in cellRect.DifferenceCells(cellRect.ExpandedBy(randomInRange)))
			{
				if (intVec.InBounds(map) && !layoutStructureSketch.AnyRoomContains(intVec, null))
				{
					TerrainDef newTerr = this.SurroundingTerrain;
					if (this.SurroundingScatterTerrain != null)
					{
						float num = Mathf.Clamp01(Mathf.InverseLerp(0f, (float)randomInRange, layoutStructureSketch.ClosestDistTo(intVec)));
						if (Rand.Chance(EasingFunctions.EaseInOutQuad(Mathf.Clamp01(num - 0.2f))))
						{
							newTerr = MapGenUtility.GetNaturalTerrainAt(intVec, map);
						}
						else if (Rand.Chance(EasingFunctions.EaseInOutQuad(Mathf.Clamp01(num))))
						{
							newTerr = this.SurroundingScatterTerrain;
						}
					}
					map.terrainGrid.SetTerrain(intVec, newTerr);
					LayoutWorker_Structure.UsedSurroundingCells.Add(intVec);
				}
			}
		}
		this.SurroundingTerrainPlaced(LayoutWorker_Structure.UsedSurroundingCells, layoutStructureSketch, map, faction);
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x00007430 File Offset: 0x00005630
	protected virtual void SurroundingTerrainPlaced(List<IntVec3> cells, LayoutStructureSketch sketch, Map map, Faction faction)
	{
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x00007458 File Offset: 0x00005658
	[CompilerGenerated]
	internal static bool <CreateDoors>g__IsCenterAlignedCell|43_0(IntVec3 bCenter, IntVec3 pos)
	{
		return pos.x == bCenter.x || pos.z == bCenter.z;
	}

	// Token: 0x04000027 RID: 39
	private static readonly List<CellRect> TmpSubdivided = new List<CellRect>(6);

	// Token: 0x04000028 RID: 40
	private static readonly List<IntVec3> TmpCells = new List<IntVec3>();

	// Token: 0x04000029 RID: 41
	private static readonly List<IntVec3> UsedSurroundingCells = new List<IntVec3>(200);
}
