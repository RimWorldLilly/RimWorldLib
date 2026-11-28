using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.SketchGen;
using Verse;

// Token: 0x02000010 RID: 16
public abstract class LayoutWorker
{
	// Token: 0x1700000E RID: 14
	// (get) Token: 0x06000047 RID: 71 RVA: 0x000039BB File Offset: 0x00001BBB
	protected virtual IntVec3 FlushOffset
	{
		get
		{
			return IntVec3.Zero;
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x06000048 RID: 72 RVA: 0x000039C2 File Offset: 0x00001BC2
	public LayoutDef Def
	{
		get
		{
			return this.def;
		}
	}

	// Token: 0x06000049 RID: 73 RVA: 0x000039CA File Offset: 0x00001BCA
	public LayoutWorker(LayoutDef def)
	{
		this.def = def;
	}

	// Token: 0x0600004A RID: 74 RVA: 0x000039DC File Offset: 0x00001BDC
	public LayoutStructureSketch GenerateStructureSketch(StructureGenParams parms)
	{
		LayoutStructureSketch layoutStructureSketch = new LayoutStructureSketch();
		layoutStructureSketch.layoutDef = this.def;
		DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 1);
		defaultInterpolatedStringHandler.AppendLiteral("LayoutStructureSketch_");
		defaultInterpolatedStringHandler.AppendFormatted<int>(Find.UniqueIDsManager.GetNextStructureSketchID());
		layoutStructureSketch.uniqueId = defaultInterpolatedStringHandler.ToStringAndClear();
		parms.sketch = layoutStructureSketch;
		Map mapBeingGenerated = MapGenerator.mapBeingGenerated;
		LayoutStructureSketch sketch;
		using (new RandBlock((mapBeingGenerated != null) ? mapBeingGenerated.NextGenSeed : Rand.Int))
		{
			LayoutSketch layoutSketch = this.GenerateSketch(parms);
			parms.sketch.layoutSketch = layoutSketch;
			parms.sketch.structureLayout = layoutSketch.structureLayout;
			parms.sketch.structureLayout.sketch = parms.sketch;
			this.ResolveRoomDefs(parms.sketch);
			LayoutWorker.MergeAdjacentRooms(parms.sketch.structureLayout);
			LayoutWorker.RemoveBorderDoors(parms.sketch.structureLayout);
			layoutSketch.FlushLayoutToSketch(this.FlushOffset);
			this.PostLayoutFlushedToSketch(parms.sketch);
			this.ResolveRoomSketches(parms.sketch);
			if (this.def.shouldDamage)
			{
				SketchResolveParams parms2 = new SketchResolveParams
				{
					sketch = parms.sketch.layoutSketch,
					destroyChanceExp = new float?(1.5f)
				};
				SketchResolverDefOf.DamageBuildingsLight.Resolve(parms2);
			}
			sketch = parms.sketch;
		}
		return sketch;
	}

	// Token: 0x0600004B RID: 75 RVA: 0x00003B50 File Offset: 0x00001D50
	protected virtual void PostLayoutFlushedToSketch(LayoutStructureSketch parms)
	{
	}

	// Token: 0x0600004C RID: 76
	protected abstract LayoutSketch GenerateSketch(StructureGenParams parms);

	// Token: 0x0600004D RID: 77 RVA: 0x00003B54 File Offset: 0x00001D54
	private static void RemoveBorderDoors(StructureLayout layout)
	{
		for (int i = 0; i < layout.Rooms.Count; i++)
		{
			LayoutRoom layoutRoom = layout.Rooms[i];
			for (int j = i + 1; j < layout.Rooms.Count; j++)
			{
				LayoutRoom layoutRoom2 = layout.Rooms[j];
				if (layoutRoom.connections.Contains(layoutRoom2) && LayoutWorker.CanRemoveBorderDoors(layoutRoom, layoutRoom2) && Rand.Chance(layout.sketch.layoutDef.borderDoorRemoveChance))
				{
					foreach (CellRect cellRect in layoutRoom.rects)
					{
						foreach (IntVec3 intVec in cellRect.EdgeCells)
						{
							if (layoutRoom2.Contains(intVec, 0) && layout.IsDoorAt(intVec))
							{
								bool flag = true;
								for (int k = 0; k < 4; k++)
								{
									IntVec3 position = intVec + GenAdj.CardinalDirections[k];
									if (layout.IsOutside(position))
									{
										flag = false;
										break;
									}
								}
								if (flag)
								{
									layout.Add(intVec, RoomLayoutCellType.Floor);
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x0600004E RID: 78 RVA: 0x00003CC8 File Offset: 0x00001EC8
	private static void MergeAdjacentRooms(StructureLayout layout)
	{
		for (int i = 0; i < layout.Rooms.Count; i++)
		{
			LayoutRoom layoutRoom = layout.Rooms[i];
			if (!LayoutWorker.tmpMergedRooms.Contains(layoutRoom))
			{
				for (int j = i + 1; j < layout.Rooms.Count; j++)
				{
					LayoutRoom layoutRoom2 = layout.Rooms[j];
					if (!LayoutWorker.tmpMergedRooms.Contains(layoutRoom2) && LayoutWorker.CanMergeAdjacentRooms(layoutRoom, layoutRoom2) && layoutRoom.IsAdjacentTo(layoutRoom2, 5) && Rand.Chance(layout.sketch.layoutDef.adjacentRoomMergeChance))
					{
						LayoutWorker.tmpMergedRooms.Add(layoutRoom);
						LayoutWorker.tmpMergedRooms.Add(layoutRoom2);
						layoutRoom.merged.Add(layoutRoom2);
						layoutRoom2.merged.Add(layoutRoom);
						foreach (CellRect cellRect in layoutRoom.rects)
						{
							using (IEnumerator<IntVec3> enumerator2 = cellRect.EdgeCellsNoCorners.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									IntVec3 edge = enumerator2.Current;
									bool flag = true;
									for (int k = 0; k < 4; k++)
									{
										IntVec3 position = edge + GenAdj.CardinalDirections[k];
										if (layout.IsOutside(position))
										{
											flag = false;
											break;
										}
									}
									if (flag && layoutRoom2.Contains(edge, 0) && !layoutRoom2.rects.Any((CellRect r) => r.IsCorner(edge)))
									{
										layout.Add(edge, RoomLayoutCellType.Floor);
									}
								}
							}
						}
					}
				}
			}
		}
		LayoutWorker.tmpMergedRooms.Clear();
	}

	// Token: 0x0600004F RID: 79 RVA: 0x00003EC4 File Offset: 0x000020C4
	private void ResolveRoomDefs(LayoutStructureSketch sketch)
	{
		LayoutWorker.<>c__DisplayClass12_0 CS$<>8__locals1 = new LayoutWorker.<>c__DisplayClass12_0();
		if (this.def.roomDefs.NullOrEmpty<LayoutDef.LayoutRoomWeight>())
		{
			return;
		}
		CS$<>8__locals1.usedDefs = new List<LayoutRoomDef>();
		Dictionary<LayoutRoomDef, int> dictionary = new Dictionary<LayoutRoomDef, int>();
		foreach (LayoutDef.LayoutRoomWeight layoutRoomWeight in this.def.roomDefs)
		{
			if (layoutRoomWeight.countRange.min != 0)
			{
				dictionary.Add(layoutRoomWeight.def, layoutRoomWeight.countRange.min);
			}
		}
		using (IEnumerator<LayoutRoom> enumerator2 = sketch.structureLayout.Rooms.InRandomOrder(null).GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				LayoutWorker.<>c__DisplayClass12_1 CS$<>8__locals2 = new LayoutWorker.<>c__DisplayClass12_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.room = enumerator2.Current;
				if (CS$<>8__locals2.room.requiredDef != null)
				{
					CS$<>8__locals2.room.defs.Add(CS$<>8__locals2.room.requiredDef);
					CS$<>8__locals2.CS$<>8__locals1.usedDefs.Add(CS$<>8__locals2.room.requiredDef);
					if (dictionary.ContainsKey(CS$<>8__locals2.room.requiredDef))
					{
						if (dictionary[CS$<>8__locals2.room.requiredDef] == 1)
						{
							dictionary.Remove(CS$<>8__locals2.room.requiredDef);
						}
						else
						{
							Dictionary<LayoutRoomDef, int> dictionary2 = dictionary;
							LayoutRoomDef key = CS$<>8__locals2.room.requiredDef;
							int num = dictionary2[key];
							dictionary2[key] = num - 1;
						}
					}
				}
				else
				{
					if (dictionary.Any<KeyValuePair<LayoutRoomDef, int>>())
					{
						bool flag = false;
						foreach (LayoutRoomDef layoutRoomDef in dictionary.Keys)
						{
							if (layoutRoomDef.CanResolve(CS$<>8__locals2.room))
							{
								CS$<>8__locals2.room.defs.Add(layoutRoomDef);
								CS$<>8__locals2.CS$<>8__locals1.usedDefs.Add(layoutRoomDef);
								if (dictionary[layoutRoomDef] == 1)
								{
									dictionary.Remove(layoutRoomDef);
								}
								else
								{
									Dictionary<LayoutRoomDef, int> dictionary3 = dictionary;
									LayoutRoomDef key = layoutRoomDef;
									int num = dictionary3[key];
									dictionary3[key] = num - 1;
								}
								flag = true;
								break;
							}
						}
						if (flag)
						{
							continue;
						}
					}
					LayoutDef.LayoutRoomWeight roomDef;
					if ((from d in this.def.roomDefs
					where d.def.CanResolve(CS$<>8__locals2.room) && CS$<>8__locals2.CS$<>8__locals1.usedDefs.Count((LayoutRoomDef ud) => ud == d.def) < d.countRange.max
					select d).TryRandomElementByWeight((LayoutDef.LayoutRoomWeight d) => d.weight, out roomDef))
					{
						CS$<>8__locals2.room.defs.Add(roomDef.def);
						CS$<>8__locals2.CS$<>8__locals1.usedDefs.Add(roomDef.def);
						if (this.def.canHaveMultipleLayoutsInRoom && roomDef.def.canBeInMixedRoom && Rand.Chance(this.def.multipleLayoutRoomChance))
						{
							LayoutDef.LayoutRoomWeight layoutRoomWeight2;
							if ((from d in this.def.roomDefs
							where d != roomDef && d.def.canBeInMixedRoom && d.def.CanResolve(CS$<>8__locals2.room) && CS$<>8__locals2.CS$<>8__locals1.usedDefs.Count((LayoutRoomDef ud) => ud == d.def) < d.countRange.max
							select d).TryRandomElementByWeight((LayoutDef.LayoutRoomWeight d) => d.weight, out layoutRoomWeight2))
							{
								CS$<>8__locals2.room.defs.Add(layoutRoomWeight2.def);
								CS$<>8__locals2.CS$<>8__locals1.usedDefs.Add(layoutRoomWeight2.def);
							}
						}
					}
				}
			}
		}
		if (dictionary.Any<KeyValuePair<LayoutRoomDef, int>>())
		{
			Log.ErrorOnce("Layout failed to spawn all required rooms, rooms failed to place: " + (from x in dictionary.Keys
			select x.defName).ToCommaList(false, false), 114632452);
		}
	}

	// Token: 0x06000050 RID: 80 RVA: 0x00004348 File Offset: 0x00002548
	private void ResolveRoomSketches(LayoutStructureSketch sketch)
	{
		LayoutRoomParams parms = new LayoutRoomParams
		{
			sketch = sketch.layoutSketch
		};
		foreach (LayoutRoom layoutRoom in sketch.structureLayout.Rooms)
		{
			if (!layoutRoom.defs.NullOrEmpty<LayoutRoomDef>())
			{
				parms.room = layoutRoom;
				foreach (LayoutRoomDef layoutRoomDef in layoutRoom.defs)
				{
					layoutRoom.sketch = sketch;
					layoutRoomDef.ResolveSketch(parms);
				}
			}
		}
	}

	// Token: 0x06000051 RID: 81 RVA: 0x00004410 File Offset: 0x00002610
	public virtual void Spawn(LayoutStructureSketch layoutStructureSketch, Map map, IntVec3 pos, float? threatPoints = null, List<Thing> allSpawnedThings = null, bool roofs = true, bool canReuseSketch = false, Faction faction = null)
	{
		if (layoutStructureSketch.spawned && !canReuseSketch)
		{
			Log.ErrorOnce("Attempted to spawn a structure sketch which was previously spawned, this is not guaranteed to work, confirm you intended to do this by setting the canReuseSketch flag to true.", layoutStructureSketch.GetHashCode());
		}
		IntVec3 offset;
		if (layoutStructureSketch.spawned)
		{
			offset = pos - layoutStructureSketch.center;
		}
		else
		{
			offset = pos;
		}
		layoutStructureSketch.structureLayout.container = layoutStructureSketch.structureLayout.container.MovedBy(offset);
		ListerBuildings.TrackingScope scope = null;
		if (DebugSettings.logMismatchedLayoutFactions && faction != null)
		{
			scope = map.listerBuildings.Track(null);
		}
		Thing.allowDestroyNonDestroyable = true;
		Sketch layoutSketch = layoutStructureSketch.layoutSketch;
		Sketch.SpawnPosType posType = Sketch.SpawnPosType.Unchanged;
		Sketch.SpawnMode spawnMode = Sketch.SpawnMode.Normal;
		bool wipeIfCollides = true;
		bool forceTerrainAffordance = true;
		bool clearEdificeWhereFloor = true;
		bool dormant = false;
		Func<SketchEntity, IntVec3, bool> canSpawnThing = null;
		Action<IntVec3, SketchEntity> onFailedToSpawnThing = null;
		LayoutSketch layoutSketch2 = layoutStructureSketch.layoutSketch;
		layoutSketch.Spawn(map, pos, faction, posType, spawnMode, wipeIfCollides, forceTerrainAffordance, clearEdificeWhereFloor, allSpawnedThings, dormant, roofs, canSpawnThing, onFailedToSpawnThing, (layoutSketch2 != null) ? layoutSketch2.DefaultAffordanceTerrain : null);
		Thing.allowDestroyNonDestroyable = false;
		this.FillAllRooms(layoutStructureSketch, map, pos, layoutStructureSketch.layoutDef.clearRoomsEntirely, faction, threatPoints);
		if (DebugSettings.logMismatchedLayoutFactions && faction != null && scope != null)
		{
			scope.Stop();
			IEnumerable<Building> thingsOfType = map.listerThings.GetThingsOfType<Building>();
			Func<Building, bool> <>9__0;
			Func<Building, bool> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = ((Building b) => !scope.Contains(b)));
			}
			foreach (Building building in thingsOfType.Where(predicate))
			{
				if (building.def.CanHaveFaction && building.Faction != faction)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 4);
					defaultInterpolatedStringHandler.AppendLiteral("Building ");
					defaultInterpolatedStringHandler.AppendFormatted<Building>(building);
					defaultInterpolatedStringHandler.AppendLiteral(" at ");
					defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(building.Position);
					defaultInterpolatedStringHandler.AppendLiteral(" has incorrect faction, faction = ");
					Faction faction2 = building.Faction;
					defaultInterpolatedStringHandler.AppendFormatted((faction2 != null) ? faction2.Name : null);
					defaultInterpolatedStringHandler.AppendLiteral(", should be ");
					defaultInterpolatedStringHandler.AppendFormatted(faction.Name);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
		}
		ListerBuildings.TrackingScope scope2 = scope;
		if (scope2 != null)
		{
			scope2.Dispose();
		}
		layoutStructureSketch.spawned = true;
		layoutStructureSketch.center = pos;
	}

	// Token: 0x06000052 RID: 82 RVA: 0x00004640 File Offset: 0x00002840
	private void FillAllRooms(LayoutStructureSketch structureSketch, Map map, IntVec3 center, bool clearRooms, Faction faction, float? threatPoints = null)
	{
		if (structureSketch.structureLayout.Rooms.NullOrEmpty<LayoutRoom>())
		{
			return;
		}
		IntVec3 intVec;
		if (structureSketch.spawned)
		{
			intVec = center - structureSketch.center;
		}
		else
		{
			intVec = center;
		}
		foreach (LayoutRoom layoutRoom in structureSketch.structureLayout.Rooms)
		{
			for (int i = 0; i < layoutRoom.rects.Count; i++)
			{
				layoutRoom.rects[i] = layoutRoom.rects[i].MovedBy(intVec);
			}
		}
		for (int j = 0; j < structureSketch.structureLayout.Rooms.Count; j++)
		{
			LayoutRoom room = structureSketch.structureLayout.Rooms[j];
			using (new RandBlock(HashCode.Combine<int, int>(structureSketch.id, j)))
			{
				this.PreFillRoom(map, room, faction, threatPoints);
			}
		}
		for (int k = 0; k < structureSketch.structureLayout.Rooms.Count; k++)
		{
			LayoutRoom room2 = structureSketch.structureLayout.Rooms[k];
			using (new RandBlock(HashCode.Combine<int, int>(structureSketch.id, k)))
			{
				this.FillRoom(structureSketch, map, clearRooms, threatPoints, room2, intVec, faction);
			}
		}
		for (int l = 0; l < structureSketch.structureLayout.Rooms.Count; l++)
		{
			LayoutRoom room3 = structureSketch.structureLayout.Rooms[l];
			using (new RandBlock(HashCode.Combine<int, int>(structureSketch.id, l)))
			{
				this.PostFillRoom(map, room3, faction, threatPoints);
			}
		}
	}

	// Token: 0x06000053 RID: 83 RVA: 0x0000484C File Offset: 0x00002A4C
	protected virtual void PreFillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
	{
		if (room.defs.NullOrEmpty<LayoutRoomDef>())
		{
			return;
		}
		foreach (LayoutRoomDef layoutRoomDef in room.defs)
		{
			layoutRoomDef.PreResolveContents(map, room, faction, threatPoints);
		}
	}

	// Token: 0x06000054 RID: 84 RVA: 0x000048B0 File Offset: 0x00002AB0
	protected virtual void PostFillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
	{
		if (room.defs.NullOrEmpty<LayoutRoomDef>())
		{
			return;
		}
		foreach (LayoutRoomDef layoutRoomDef in room.defs)
		{
			layoutRoomDef.PostResolveContents(map, room, faction, threatPoints);
		}
	}

	// Token: 0x06000055 RID: 85 RVA: 0x00004914 File Offset: 0x00002B14
	protected virtual void FillRoom(LayoutStructureSketch structureSketch, Map map, bool clearRooms, float? threatPoints, LayoutRoom room, IntVec3 delta, Faction faction)
	{
		if (clearRooms)
		{
			LayoutWorker.ClearThingsInRoom(structureSketch, map, room, delta);
		}
		if (room.defs.NullOrEmpty<LayoutRoomDef>())
		{
			return;
		}
		map.regionAndRoomUpdater.Enabled = true;
		foreach (LayoutRoomDef layoutRoomDef in room.defs)
		{
			layoutRoomDef.ResolveContents(map, room, threatPoints, faction);
		}
	}

	// Token: 0x06000056 RID: 86 RVA: 0x00004998 File Offset: 0x00002B98
	private static void ClearThingsInRoom(LayoutStructureSketch structureSketch, Map map, LayoutRoom room, IntVec3 delta)
	{
		foreach (CellRect cellRect in room.rects)
		{
			foreach (IntVec3 intVec in cellRect.Cells)
			{
				LayoutWorker.<>c__DisplayClass19_0 CS$<>8__locals1;
				structureSketch.layoutSketch.ThingsAt(intVec - delta, out CS$<>8__locals1.single, out CS$<>8__locals1.multiple);
				Building edifice = intVec.GetEdifice(map);
				if (edifice != null && LayoutWorker.<ClearThingsInRoom>g__ShouldClear|19_0(edifice, ref CS$<>8__locals1))
				{
					edifice.Destroy(DestroyMode.Vanish);
				}
				foreach (Thing thing in intVec.GetThingList(map).ToList<Thing>())
				{
					if (LayoutWorker.<ClearThingsInRoom>g__ShouldClear|19_0(thing, ref CS$<>8__locals1))
					{
						thing.Destroy(DestroyMode.Vanish);
					}
				}
			}
		}
	}

	// Token: 0x06000057 RID: 87 RVA: 0x00004ABC File Offset: 0x00002CBC
	protected static IntVec3 FindBestSpawnLocation(List<LayoutRoom> rooms, ThingDef thingDef, Map map, out LayoutRoom roomUsed, out Rot4 rotUsed, HashSet<LayoutRoom> usedRooms = null)
	{
		foreach (LayoutRoom layoutRoom in rooms.InRandomOrder(null))
		{
			if (layoutRoom.requiredDef == null && (usedRooms == null || !usedRooms.Contains(layoutRoom)))
			{
				int num = Rand.Range(0, 3);
				for (int i = 0; i < 4; i++)
				{
					Rot4 rot = new Rot4((i + num) % 4);
					IntVec3 result;
					if (ComplexUtility.TryFindRandomSpawnCell(thingDef, layoutRoom, map, out result, 0, new Rot4?(rot)))
					{
						roomUsed = layoutRoom;
						rotUsed = rot;
						return result;
					}
				}
			}
		}
		roomUsed = null;
		rotUsed = default(Rot4);
		return IntVec3.Invalid;
	}

	// Token: 0x06000058 RID: 88 RVA: 0x00004B74 File Offset: 0x00002D74
	private static bool CanMergeAdjacentRooms(LayoutRoom a, LayoutRoom b)
	{
		using (List<LayoutRoomDef>.Enumerator enumerator = a.defs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.canMergeWithAdjacentRoom)
				{
					return false;
				}
			}
		}
		using (List<LayoutRoomDef>.Enumerator enumerator = b.defs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.canMergeWithAdjacentRoom)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06000059 RID: 89 RVA: 0x00004C14 File Offset: 0x00002E14
	private static bool CanRemoveBorderDoors(LayoutRoom a, LayoutRoom b)
	{
		using (List<LayoutRoomDef>.Enumerator enumerator = a.defs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.canRemoveBorderDoors)
				{
					return false;
				}
			}
		}
		using (List<LayoutRoomDef>.Enumerator enumerator = b.defs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.canRemoveBorderDoors)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x0600005B RID: 91 RVA: 0x00004CC0 File Offset: 0x00002EC0
	[CompilerGenerated]
	internal static bool <ClearThingsInRoom>g__ShouldClear|19_0(Thing thing, ref LayoutWorker.<>c__DisplayClass19_0 A_1)
	{
		if (A_1.single != null && A_1.single.IsSame(thing))
		{
			return false;
		}
		if (A_1.multiple != null)
		{
			using (List<SketchThing>.Enumerator enumerator = A_1.multiple.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsSame(thing))
					{
						return false;
					}
				}
			}
			return true;
		}
		return true;
	}

	// Token: 0x04000019 RID: 25
	private readonly LayoutDef def;

	// Token: 0x0400001A RID: 26
	private static readonly HashSet<LayoutRoom> tmpMergedRooms = new HashSet<LayoutRoom>();
}
