using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

// Token: 0x0200000C RID: 12
public class LayoutWorkerComplex : LayoutWorker_Structure
{
	// Token: 0x0600002D RID: 45 RVA: 0x00002A14 File Offset: 0x00000C14
	public LayoutWorkerComplex(LayoutDef def) : base(def)
	{
	}

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x0600002E RID: 46 RVA: 0x00002A1D File Offset: 0x00000C1D
	public new ComplexLayoutDef Def
	{
		get
		{
			return (ComplexLayoutDef)base.Def;
		}
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00002A2A File Offset: 0x00000C2A
	public virtual Faction GetFixedHostileFactionForThreats()
	{
		return null;
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00002A2D File Offset: 0x00000C2D
	protected virtual void PreSpawnThreats(List<LayoutRoom> rooms, Map map, List<Thing> allSpawnedThings)
	{
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00002A30 File Offset: 0x00000C30
	protected override StructureLayout GetStructureLayout(StructureGenParams parms, CellRect rect)
	{
		LayoutStructureSketch sketch = parms.sketch;
		float areaPrunePercent = this.Def.areaPrunePercent;
		int minRoomHeight = this.Def.minRoomHeight;
		int minRoomWidth = this.Def.minRoomWidth;
		int minRoomHeight2 = minRoomHeight;
		float areaPrunePercent2 = areaPrunePercent;
		bool canRemoveRooms = true;
		bool generateDoors = false;
		IntRange? maxMergeRoomsRange = new IntRange?(LayoutWorkerComplex.MergeRange);
		return RoomLayoutGenerator.GenerateRandomLayout(sketch, rect, minRoomWidth, minRoomHeight2, areaPrunePercent2, canRemoveRooms, generateDoors, this.Def.corridorDef, 2, maxMergeRoomsRange, this.Def.corridorShapes, this.Def.canDisconnectRooms, false);
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00002AA0 File Offset: 0x00000CA0
	public override void Spawn(LayoutStructureSketch layoutStructureSketch, Map map, IntVec3 pos, float? threatPoints = null, List<Thing> allSpawnedThings = null, bool roofs = true, bool canReuseSketch = false, Faction faction = null)
	{
		List<Thing> list = allSpawnedThings ?? new List<Thing>();
		base.Spawn(layoutStructureSketch, map, pos, threatPoints, list, roofs, canReuseSketch, faction);
		List<LayoutRoom> rooms = layoutStructureSketch.structureLayout.Rooms;
		LayoutWorkerComplex.SpawnThings(layoutStructureSketch, map, rooms, list);
		if (threatPoints != null && !this.Def.threats.NullOrEmpty<ComplexThreat>())
		{
			this.PreSpawnThreats(rooms, map, list);
			this.SpawnThreats(layoutStructureSketch, map, pos, threatPoints.Value, list, rooms);
		}
		this.PostSpawnStructure(rooms, map, list);
		LayoutWorkerComplex.tmpSpawnedThreatThings.Clear();
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00002B2C File Offset: 0x00000D2C
	private static void SpawnThings(LayoutStructureSketch layoutStructureSketch, Map map, List<LayoutRoom> rooms, List<Thing> spawnedThings)
	{
		if (layoutStructureSketch.thingsToSpawn.NullOrEmpty<Thing>())
		{
			return;
		}
		HashSet<LayoutRoom> usedRooms = new HashSet<LayoutRoom>();
		for (int i = layoutStructureSketch.thingsToSpawn.Count - 1; i >= 0; i--)
		{
			Thing thing = layoutStructureSketch.thingsToSpawn[i];
			LayoutRoom layoutRoom;
			Rot4 rot;
			IntVec3 loc = LayoutWorker.FindBestSpawnLocation(rooms, thing.def, map, out layoutRoom, out rot, usedRooms);
			if (!loc.IsValid)
			{
				loc = LayoutWorker.FindBestSpawnLocation(rooms, thing.def, map, out layoutRoom, out rot, null);
			}
			if (!loc.IsValid)
			{
				thing.Destroy(DestroyMode.Vanish);
				layoutStructureSketch.thingsToSpawn.RemoveAt(i);
			}
			else
			{
				GenSpawn.Spawn(thing, loc, map, rot, WipeMode.Vanish, false, false);
				spawnedThings.Add(thing);
				layoutStructureSketch.thingsToSpawn.RemoveAt(i);
				if (!layoutStructureSketch.thingDiscoveredMessage.NullOrEmpty())
				{
					string signalTag = "ThingDiscovered" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
					foreach (CellRect rect in layoutRoom.rects)
					{
						RectTrigger rectTrigger = (RectTrigger)ThingMaker.MakeThing(ThingDefOf.RectTrigger, null);
						rectTrigger.signalTag = signalTag;
						rectTrigger.Rect = rect;
						GenSpawn.Spawn(rectTrigger, rect.CenterCell, map, WipeMode.Vanish);
					}
					SignalAction_Message signalAction_Message = (SignalAction_Message)ThingMaker.MakeThing(ThingDefOf.SignalAction_Message, null);
					signalAction_Message.signalTag = signalTag;
					signalAction_Message.message = layoutStructureSketch.thingDiscoveredMessage;
					signalAction_Message.messageType = MessageTypeDefOf.PositiveEvent;
					signalAction_Message.lookTargets = thing;
					GenSpawn.Spawn(signalAction_Message, loc, map, WipeMode.Vanish);
				}
			}
		}
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00002CD0 File Offset: 0x00000ED0
	protected virtual void PostSpawnStructure(List<LayoutRoom> rooms, Map map, List<Thing> allSpawnedThings)
	{
		if (ModsConfig.IdeologyActive)
		{
			this.SpawnRoomRewards(rooms, map, allSpawnedThings);
			LayoutWorkerComplex.SpawnCommsConsole(rooms, map);
		}
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00002CEC File Offset: 0x00000EEC
	private static void SpawnCommsConsole(List<LayoutRoom> rooms, Map map)
	{
		Func<IntVec3, bool> <>9__0;
		foreach (LayoutRoom layoutRoom in rooms.InRandomOrder(null))
		{
			ThingDef commsConsole = ThingDefOf.CommsConsole;
			Map map2 = map;
			Rot4? rot = null;
			int contractedBy = 1;
			int padding = 0;
			Func<IntVec3, bool> validator;
			if ((validator = <>9__0) == null)
			{
				validator = (<>9__0 = ((IntVec3 c) => LayoutWorkerComplex.CanPlaceCommsConsoleAt(c, map)));
			}
			IntVec3 loc;
			if (layoutRoom.TryGetRandomCellInRoom(commsConsole, map2, out loc, rot, contractedBy, padding, validator, false))
			{
				GenSpawn.Spawn(ThingDefOf.AncientCommsConsole, loc, map, WipeMode.Vanish);
				break;
			}
		}
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00002D98 File Offset: 0x00000F98
	private static bool CanPlaceCommsConsoleAt(IntVec3 cell, Map map)
	{
		using (CellRect.Enumerator enumerator = GenAdj.OccupiedRect(cell, Rot4.North, ThingDefOf.AncientCommsConsole.Size).ExpandedBy(1).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.GetEdifice(map) != null)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00002E10 File Offset: 0x00001010
	private void SpawnRoomRewards(List<LayoutRoom> rooms, Map map, List<Thing> allSpawnedThings)
	{
		LayoutWorkerComplex.<>c__DisplayClass14_0 CS$<>8__locals1 = new LayoutWorkerComplex.<>c__DisplayClass14_0();
		CS$<>8__locals1.map = map;
		if (this.Def.roomRewardCrateFactor <= 0f)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < allSpawnedThings.Count; i++)
		{
			if (allSpawnedThings[i] is Building_Crate)
			{
				num++;
			}
		}
		int num2 = Mathf.RoundToInt((float)rooms.Count * this.Def.roomRewardCrateFactor) - num;
		if (num2 <= 0)
		{
			return;
		}
		ThingSetMakerDef thingSetMakerDef = this.Def.rewardThingSetMakerDef ?? ThingSetMakerDefOf.Reward_ItemsStandard;
		foreach (LayoutRoom layoutRoom in rooms.InRandomOrder(null))
		{
			if (layoutRoom.requiredDef == null)
			{
				LayoutRoom layoutRoom2 = layoutRoom;
				ThingDef ancientHermeticCrate = ThingDefOf.AncientHermeticCrate;
				Map map2 = CS$<>8__locals1.map;
				Func<IntVec3, bool> validator = new Func<IntVec3, bool>(CS$<>8__locals1.<SpawnRoomRewards>g__CanSpawnAt|0);
				IntVec3 loc;
				if (layoutRoom2.TryGetRandomCellInRoom(ancientHermeticCrate, map2, out loc, null, 2, 0, validator, false))
				{
					Building_Crate building_Crate = (Building_Crate)GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.AncientHermeticCrate, null), loc, CS$<>8__locals1.map, Rot4.South, WipeMode.Vanish, false, false);
					List<Thing> list = thingSetMakerDef.root.Generate(default(ThingSetMakerParams));
					for (int j = list.Count - 1; j >= 0; j--)
					{
						Thing thing = list[j];
						if (!building_Crate.TryAcceptThing(thing, false))
						{
							thing.Destroy(DestroyMode.Vanish);
						}
					}
					num2--;
				}
				if (num2 <= 0)
				{
					break;
				}
			}
		}
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00002FA0 File Offset: 0x000011A0
	private static List<ComplexThreatDef> GetUsedThreats(LayoutRoom room)
	{
		int roomMergedLowestID = LayoutWorkerComplex.GetRoomMergedLowestID(room);
		return LayoutWorkerComplex.usedThreatsByRoom[roomMergedLowestID];
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00002FBF File Offset: 0x000011BF
	private static bool TryGetRoomUsedThreats(LayoutRoom room, out List<ComplexThreatDef> threats)
	{
		return LayoutWorkerComplex.usedThreatsByRoom.TryGetValue(LayoutWorkerComplex.GetRoomMergedLowestID(room), out threats);
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00002FD4 File Offset: 0x000011D4
	private static void AddUsedThreat(LayoutRoom room, ComplexThreatDef threat)
	{
		int roomMergedLowestID = LayoutWorkerComplex.GetRoomMergedLowestID(room);
		List<ComplexThreatDef> list;
		if (!LayoutWorkerComplex.usedThreatsByRoom.TryGetValue(roomMergedLowestID, out list))
		{
			list = (LayoutWorkerComplex.usedThreatsByRoom[roomMergedLowestID] = new List<ComplexThreatDef>());
		}
		list.Add(threat);
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00003010 File Offset: 0x00001210
	private static int GetRoomMergedLowestID(LayoutRoom room)
	{
		int loadId = room.loadId;
		for (int i = 0; i < room.merged.Count; i++)
		{
			if (room.merged[i].loadId < loadId)
			{
				loadId = room.merged[i].loadId;
			}
		}
		return loadId;
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00003064 File Offset: 0x00001264
	private void SpawnThreats(LayoutStructureSketch structureSketch, Map map, IntVec3 center, float threatPoints, List<Thing> spawnedThings, List<LayoutRoom> rooms)
	{
		ComplexResolveParams threatParams = new ComplexResolveParams
		{
			map = map,
			complexRect = structureSketch.layoutSketch.OccupiedRect.MovedBy(center),
			hostileFaction = this.GetFixedHostileFactionForThreats(),
			allRooms = rooms,
			points = threatPoints
		};
		StringBuilder stringBuilder = null;
		if (DebugViewSettings.logComplexGenPoints)
		{
			stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("----- Logging points for " + this.Def.defName + ". -----");
			StringBuilder stringBuilder2 = stringBuilder;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Total threat points: ");
			defaultInterpolatedStringHandler.AppendFormatted<float>(threatPoints);
			stringBuilder2.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
			StringBuilder stringBuilder3 = stringBuilder;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(12, 1);
			defaultInterpolatedStringHandler2.AppendLiteral("Room count: ");
			defaultInterpolatedStringHandler2.AppendFormatted<int>(rooms.Count);
			stringBuilder3.AppendLine(defaultInterpolatedStringHandler2.ToStringAndClear());
			StringBuilder stringBuilder4 = stringBuilder;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(24, 1);
			defaultInterpolatedStringHandler3.AppendLiteral("Approx points per room: ");
			defaultInterpolatedStringHandler3.AppendFormatted<float>(threatParams.points);
			stringBuilder4.AppendLine(defaultInterpolatedStringHandler3.ToStringAndClear());
			if (threatParams.hostileFaction != null)
			{
				StringBuilder stringBuilder5 = stringBuilder;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(9, 1);
				defaultInterpolatedStringHandler4.AppendLiteral("Faction: ");
				defaultInterpolatedStringHandler4.AppendFormatted<Faction>(threatParams.hostileFaction);
				stringBuilder5.AppendLine(defaultInterpolatedStringHandler4.ToStringAndClear());
			}
		}
		LayoutWorkerComplex.useableThreats.Clear();
		LayoutWorkerComplex.usedThreatsByRoom.Clear();
		LayoutWorkerComplex.useableThreats.AddRange(from t in this.Def.threats
		where Rand.Chance(t.chancePerComplex)
		select t);
		float num = 0f;
		int num2 = 100;
		while (num < threatPoints && num2 > 0)
		{
			num2--;
			LayoutRoom room = rooms.RandomElement<LayoutRoom>();
			threatParams.room = room;
			threatParams.spawnedThings = spawnedThings;
			float b = threatPoints - num;
			threatParams.points = Mathf.Min(LayoutWorkerComplex.ThreatPointsFactorRange.RandomInRange * threatPoints, b);
			ComplexThreat complexThreat;
			if (LayoutWorkerComplex.useableThreats.Where(delegate(ComplexThreat t)
			{
				int num4 = 0;
				Predicate<ComplexThreatDef> <>9__4;
				foreach (KeyValuePair<int, List<ComplexThreatDef>> keyValuePair in LayoutWorkerComplex.usedThreatsByRoom)
				{
					int num5;
					List<ComplexThreatDef> list;
					keyValuePair.Deconstruct(out num5, out list);
					List<ComplexThreatDef> list2 = list;
					int num6 = num4;
					List<ComplexThreatDef> list3 = list2;
					Predicate<ComplexThreatDef> predicate;
					if ((predicate = <>9__4) == null)
					{
						predicate = (<>9__4 = ((ComplexThreatDef td) => td == t.def));
					}
					num4 = num6 + list3.Count(predicate);
				}
				List<ComplexThreatDef> list4;
				return num4 < t.maxPerComplex && (!LayoutWorkerComplex.TryGetRoomUsedThreats(room, out list4) || list4.Count((ComplexThreatDef td) => td == t.def) < t.maxPerRoom) && t.def.Worker.CanResolve(threatParams);
			}).TryRandomElementByWeight((ComplexThreat t) => t.selectionWeight, out complexThreat))
			{
				if (stringBuilder != null)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("-> Resolving threat " + complexThreat.def.defName);
				}
				float num3 = 0f;
				complexThreat.def.Worker.Resolve(threatParams, ref num3, LayoutWorkerComplex.tmpSpawnedThreatThings, stringBuilder);
				num += num3;
				LayoutWorkerComplex.AddUsedThreat(room, complexThreat.def);
			}
		}
		if (stringBuilder != null)
		{
			StringBuilder stringBuilder6 = stringBuilder;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler5 = new DefaultInterpolatedStringHandler(27, 1);
			defaultInterpolatedStringHandler5.AppendLiteral("Total threat points spent: ");
			defaultInterpolatedStringHandler5.AppendFormatted<float>(num);
			stringBuilder6.AppendLine(defaultInterpolatedStringHandler5.ToStringAndClear());
			Log.Message(stringBuilder.ToString());
		}
		LayoutWorkerComplex.useableThreats.Clear();
		LayoutWorkerComplex.usedThreatsByRoom.Clear();
	}

	// Token: 0x0400000C RID: 12
	private static readonly FloatRange ThreatPointsFactorRange = new FloatRange(0.25f, 0.35f);

	// Token: 0x0400000D RID: 13
	private static readonly List<Thing> tmpSpawnedThreatThings = new List<Thing>();

	// Token: 0x0400000E RID: 14
	private static readonly IntRange MergeRange = new IntRange(1, 4);

	// Token: 0x0400000F RID: 15
	private static readonly List<ComplexThreat> useableThreats = new List<ComplexThreat>();

	// Token: 0x04000010 RID: 16
	private static readonly Dictionary<int, List<ComplexThreatDef>> usedThreatsByRoom = new Dictionary<int, List<ComplexThreatDef>>();
}
