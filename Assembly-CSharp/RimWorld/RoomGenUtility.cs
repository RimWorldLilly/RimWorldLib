using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001262 RID: 4706
	public static class RoomGenUtility
	{
		// Token: 0x06007486 RID: 29830 RVA: 0x0028C43C File Offset: 0x0028A63C
		public static void FillWithPadding(ThingDef thing, FloatRange countPer100Cells, LayoutRoom room, Map map, Rot4? fixedRot = null, Func<IntVec3, Rot4, CellRect, bool> validator = null, List<Thing> spawned = null, int contractedBy = 1, ThingDef stuff = null, bool alignWithRect = false, bool snapToGrid = false, Func<IntVec3, Rot4, Map, Thing> spawnAction = null, Faction faction = null)
		{
			float num = (float)room.rects.Sum((CellRect r) => r.Area) / 100f;
			int count = Mathf.Max(Mathf.RoundToInt(countPer100Cells.RandomInRange * num), 1);
			RoomGenUtility.FillWithPadding(thing, count, room, map, fixedRot, validator, spawned, contractedBy, stuff, alignWithRect, snapToGrid, spawnAction, faction);
		}

		// Token: 0x06007487 RID: 29831 RVA: 0x0028C4AC File Offset: 0x0028A6AC
		public static void FillWithPadding(ThingDef thing, int count, LayoutRoom room, Map map, Rot4? fixedRot = null, Func<IntVec3, Rot4, CellRect, bool> validator = null, List<Thing> spawned = null, int contractedBy = 1, ThingDef stuff = null, bool alignWithRect = false, bool snapToGrid = false, Func<IntVec3, Rot4, Map, Thing> spawnAction = null, Faction faction = null)
		{
			IntVec2 size = thing.Size;
			int num = 0;
			foreach (IntVec3 intVec in room.Cells.InRandomOrder(null))
			{
				if (num >= count)
				{
					break;
				}
				if (room.Contains(intVec, contractedBy))
				{
					foreach (Rot4 rot in Rot4.AllRotations.InRandomOrder(null))
					{
						if (num >= count)
						{
							break;
						}
						if ((fixedRot == null || !(rot != fixedRot.Value)) && (thing.rotatable || !(rot != Rot4.North)))
						{
							CellRect cellRect;
							room.TryGetRectContainingCell(intVec, out cellRect);
							if (alignWithRect)
							{
								Rot4 b = (cellRect.Width >= cellRect.Height) ? Rot4.East : Rot4.North;
								if (rot != b)
								{
									continue;
								}
							}
							if (snapToGrid)
							{
								bool flag = intVec.x - cellRect.minX != 0;
								int num2 = intVec.z - cellRect.minZ;
								if ((flag ? 1 : 0) % (size.x + 1) != 0 || num2 % (size.z + 1) != 0)
								{
									continue;
								}
							}
							CellRect arg = intVec.RectAbout(size, rot);
							bool flag2 = true;
							if (validator == null || validator(intVec, rot, arg))
							{
								foreach (IntVec3 cell in arg.Cells)
								{
									if (!RoomGenUtility.IsFillValidCell(thing, cell, room, map, contractedBy, default(CellRect)))
									{
										flag2 = false;
										break;
									}
								}
								if (flag2)
								{
									Thing thing2;
									if (spawnAction != null)
									{
										thing2 = spawnAction(intVec, rot, map);
										if (thing2.def.Size.x > size.x || thing2.def.Size.z > size.z)
										{
											DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(126, 4);
											defaultInterpolatedStringHandler.AppendLiteral("Spawned thing ");
											defaultInterpolatedStringHandler.AppendFormatted(thing2.def.defName);
											defaultInterpolatedStringHandler.AppendLiteral(" with size ");
											defaultInterpolatedStringHandler.AppendFormatted<IntVec2>(thing2.def.Size);
											defaultInterpolatedStringHandler.AppendLiteral(" during room edge filling with larger size as root def ");
											defaultInterpolatedStringHandler.AppendFormatted(thing.defName);
											defaultInterpolatedStringHandler.AppendLiteral(" with size ");
											defaultInterpolatedStringHandler.AppendFormatted<IntVec2>(thing.Size);
											defaultInterpolatedStringHandler.AppendLiteral(". Size must be the same or smaller.");
											Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), thing2.def.GetHashCode() ^ 2945741);
										}
									}
									else
									{
										thing2 = ThingMaker.MakeThing(thing, stuff);
										thing2.SetFactionDirect(faction);
										GenSpawn.Spawn(thing2, intVec, map, rot, WipeMode.Vanish, false, false);
									}
									num++;
									if (spawned != null)
									{
										spawned.Add(thing2);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06007488 RID: 29832 RVA: 0x0028C7E8 File Offset: 0x0028A9E8
		public static void FillPrefabs(LayoutPrefabParms parms, int count, LayoutRoom room, Map map, Func<IntVec3, Rot4, bool> validator = null, int contractedBy = 1, List<Thing> spawned = null, bool alignWithRect = false, bool snapToGrid = false, Faction faction = null, Func<PrefabThingData, Tuple<ThingDef, ThingDef>> overrideSpawnData = null)
		{
			IntVec2 size = parms.def.size;
			int num = 0;
			foreach (IntVec3 intVec in room.Cells.InRandomOrder(null))
			{
				if (num >= count)
				{
					break;
				}
				if (room.Contains(intVec, contractedBy))
				{
					foreach (Rot4 rot in Rot4.AllRotations.InRandomOrder(null))
					{
						if (num >= count)
						{
							break;
						}
						if (parms.def.rotations.HasFlag(rot))
						{
							CellRect cellRect;
							if (alignWithRect && room.TryGetRectContainingCell(intVec, out cellRect))
							{
								Rot4 b = (cellRect.Width >= cellRect.Height) ? Rot4.East : Rot4.North;
								if (rot != b)
								{
									continue;
								}
							}
							if ((!snapToGrid || (intVec.x % (size.x + 1) == 0 && intVec.z % (size.z + 1) == 0)) && (validator == null || validator(intVec, rot)))
							{
								bool flag = true;
								CellRect ignore = default(CellRect);
								if (!parms.ensureNoBlocks)
								{
									ignore = map.BoundsRect(0);
								}
								foreach (ValueTuple<PrefabThingData, IntVec3, Rot4> valueTuple in PrefabUtility.GetThings(parms.def, intVec, rot))
								{
									PrefabThingData item = valueTuple.Item1;
									IntVec3 item2 = valueTuple.Item2;
									Rot4 item3 = valueTuple.Item3;
									if (item.def.passability != Traversability.Standable)
									{
										foreach (IntVec3 cell in item2.RectAbout(item.def.Size, item3))
										{
											if (!RoomGenUtility.IsFillValidCell(item.def, cell, room, map, contractedBy, ignore))
											{
												flag = false;
												break;
											}
										}
									}
								}
								if (flag)
								{
									PrefabUtility.SpawnPrefab(parms.def, map, intVec, rot, faction, spawned, overrideSpawnData, null, false);
									num++;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06007489 RID: 29833 RVA: 0x0028CA90 File Offset: 0x0028AC90
		public static void GenerateRows(ThingDef thingDef, LayoutRoom room, Map map, ThingDef stuff = null)
		{
			CellRect rect = room.Boundary.ContractedBy(2);
			if (stuff == null)
			{
				stuff = GenStuff.DefaultStuffFor(thingDef);
			}
			bool @bool = Rand.Bool;
			if (rect.Height < rect.Width)
			{
				bool flag = false;
				for (int i = 0; i < rect.Width; i++)
				{
					if (!flag)
					{
						for (int j = 0; j < rect.Height; j++)
						{
							IntVec3 cell = rect.Min + new IntVec3(i, 0, j);
							Rot4 rot = @bool ? Rot4.West : Rot4.East;
							RoomGenUtility.CheckAndSpawnShelf(thingDef, map, stuff, cell, rot, rect);
						}
					}
					flag = !flag;
				}
				return;
			}
			bool flag2 = false;
			for (int k = 0; k < rect.Height; k++)
			{
				if (!flag2)
				{
					for (int l = 0; l < rect.Width; l++)
					{
						IntVec3 cell2 = rect.Min + new IntVec3(l, 0, k);
						Rot4 rot2 = @bool ? Rot4.South : Rot4.North;
						RoomGenUtility.CheckAndSpawnShelf(thingDef, map, stuff, cell2, rot2, rect);
					}
				}
				flag2 = !flag2;
			}
		}

		// Token: 0x0600748A RID: 29834 RVA: 0x0028CBAC File Offset: 0x0028ADAC
		private static void CheckAndSpawnShelf(ThingDef thingDef, Map map, ThingDef stuff, IntVec3 cell, Rot4 rot, CellRect rect)
		{
			if (!GenAdj.OccupiedRect(cell, rot, thingDef.size).FullyContainedWithin(rect))
			{
				return;
			}
			if (GenAdj.OccupiedRect(cell, rot, thingDef.size).Any((IntVec3 c) => c.GetEdifice(map) != null))
			{
				return;
			}
			if (GenAdj.OccupiedRect(cell, rot, thingDef.size).All((IntVec3 c) => c.GetFirstThing(map, thingDef) == null))
			{
				GenSpawn.Spawn(ThingMaker.MakeThing(thingDef, stuff), cell, map, rot, WipeMode.Vanish, false, false);
			}
		}

		// Token: 0x0600748B RID: 29835 RVA: 0x0028CC64 File Offset: 0x0028AE64
		private static bool IsFillValidCell(ThingDef placing, IntVec3 cell, LayoutRoom room, Map map, int contractedBy, CellRect ignore = default(CellRect))
		{
			if (!room.Contains(cell, contractedBy))
			{
				return false;
			}
			if (cell.GetEdifice(map) != null)
			{
				return false;
			}
			if (placing.passability != Traversability.Standable)
			{
				for (int i = 0; i < 8; i++)
				{
					IntVec3 c = cell + GenAdj.AdjacentCellsAround[i];
					Building edifice = c.GetEdifice(map);
					if (edifice != null && edifice.def.passability != Traversability.Standable && (ignore == default(CellRect) || !ignore.Contains(c)))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600748C RID: 29836 RVA: 0x0028CCE8 File Offset: 0x0028AEE8
		public static void SpawnInCorners(ThingDef thing, LayoutRoom room, Map map, ThingDef stuff = null, float spawnChance = 1f, Rot4 rot = default(Rot4))
		{
			foreach (CellRect rhs in room.rects)
			{
				foreach (IntVec3 intVec in rhs.ContractedBy(1).Corners)
				{
					bool flag = true;
					foreach (CellRect lhs in room.rects)
					{
						if (lhs != rhs && lhs.Contains(intVec))
						{
							flag = false;
							break;
						}
					}
					int num = 0;
					for (int i = 0; i < 4; i++)
					{
						if ((intVec + GenAdj.CardinalDirections[i]).GetEdifice(map) != null)
						{
							num++;
						}
					}
					if (num >= 2 && flag && Rand.Chance(spawnChance))
					{
						GenSpawn.Spawn(ThingMaker.MakeThing(thing, stuff), intVec, map, rot, WipeMode.Vanish, false, false);
					}
				}
			}
		}

		// Token: 0x0600748D RID: 29837 RVA: 0x0028CE38 File Offset: 0x0028B038
		public static void FillAroundEdges(ThingDef thing, FloatRange groupsPerTenEdgeCells, IntRange groupCountRange, LayoutRoom room, Map map, Func<IntVec3, Rot4, CellRect, bool> validator = null, List<Thing> spawned = null, int contractedBy = 1, int padding = 0, ThingDef stuff = null, bool avoidDoors = true, RotationDirection rotationDirectionOffset = RotationDirection.Opposite, Func<IntVec3, Rot4, Map, Thing> spawnAction = null, Faction faction = null)
		{
			float num = (float)room.rects.Sum((CellRect r) => r.ContractedBy(1).EdgeCellsCount) / 10f;
			int count = Mathf.Max(Mathf.RoundToInt(groupsPerTenEdgeCells.RandomInRange * num), 1);
			RoomGenUtility.FillAroundEdges(thing, count, groupCountRange, room, map, validator, spawned, contractedBy, padding, stuff, avoidDoors, rotationDirectionOffset, spawnAction, faction);
		}

		// Token: 0x0600748E RID: 29838 RVA: 0x0028CEAC File Offset: 0x0028B0AC
		public static void FillAroundEdges(ThingDef thing, int count, IntRange groupCountRange, LayoutRoom room, Map map, Func<IntVec3, Rot4, CellRect, bool> validator = null, List<Thing> spawned = null, int contractedBy = 1, int padding = 0, ThingDef stuff = null, bool avoidDoors = true, RotationDirection rotationDirectionOffset = RotationDirection.Opposite, Func<IntVec3, Rot4, Map, Thing> spawnAction = null, Faction faction = null)
		{
			IntVec2 size = thing.Size;
			Dictionary<IntVec3, ValueTuple<Rot4, CellRect>> edgeSpawnOptions = RoomGenUtility.GetEdgeSpawnOptions(room, map, contractedBy);
			if (edgeSpawnOptions.Count == 0)
			{
				return;
			}
			if (edgeSpawnOptions.Count == 0)
			{
				return;
			}
			int num = 0;
			int num2 = 0;
			while (num2 < count && num < count)
			{
				KeyValuePair<IntVec3, ValueTuple<Rot4, CellRect>> keyValuePair = edgeSpawnOptions.RandomElement<KeyValuePair<IntVec3, ValueTuple<Rot4, CellRect>>>();
				edgeSpawnOptions.Remove(keyValuePair.Key);
				IntVec3 key = keyValuePair.Key;
				ValueTuple<Rot4, CellRect> value = keyValuePair.Value;
				Rot4 item = value.Item1;
				CellRect item2 = value.Item2;
				Rot4 rot = item.Rotated(rotationDirectionOffset);
				int randomInRange = groupCountRange.RandomInRange;
				int num3 = 0;
				bool flag = rotationDirectionOffset == RotationDirection.Clockwise || rotationDirectionOffset == RotationDirection.Counterclockwise;
				int num4 = 0;
				while (num4 < randomInRange && num < count)
				{
					int num5 = num3;
					if (num4 % 2 == 1)
					{
						num5 = -num5;
					}
					else
					{
						num3 += (flag ? size.z : size.x) + padding;
					}
					IntVec3 intVec = key + item.RighthandCell * num5;
					CellRect arg = intVec.RectAbout(size, rot);
					bool flag2 = false;
					foreach (CellRect cellRect in room.rects)
					{
						if (arg.FullyContainedWithin(cellRect.ContractedBy(1)))
						{
							flag2 = true;
							break;
						}
					}
					if (flag2)
					{
						arg = arg.ExpandedBy(padding);
						flag2 = true;
						if (validator == null || validator(key, rot, arg))
						{
							foreach (IntVec3 intVec2 in arg.Cells)
							{
								if (room.Contains(intVec2, 1))
								{
									if (avoidDoors && RoomGenUtility.IsDoorAdjacentTo(intVec2, map, true))
									{
										flag2 = false;
										break;
									}
									if (!RoomGenUtility.IsFillValidCell(thing, intVec2, room, map, contractedBy, item2))
									{
										flag2 = false;
										break;
									}
								}
							}
							if (!flag2)
							{
								break;
							}
							Thing thing2;
							if (spawnAction != null)
							{
								thing2 = spawnAction(intVec, rot, map);
								if (thing2.def.Size != size)
								{
									DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(121, 4);
									defaultInterpolatedStringHandler.AppendLiteral("Spawned thing ");
									defaultInterpolatedStringHandler.AppendFormatted(thing2.def.defName);
									defaultInterpolatedStringHandler.AppendLiteral(" with size ");
									defaultInterpolatedStringHandler.AppendFormatted<IntVec2>(thing2.def.Size);
									defaultInterpolatedStringHandler.AppendLiteral(" during room edge filling which did not have same size as root def ");
									defaultInterpolatedStringHandler.AppendFormatted(thing.defName);
									defaultInterpolatedStringHandler.AppendLiteral(" with size ");
									defaultInterpolatedStringHandler.AppendFormatted<IntVec2>(thing.Size);
									defaultInterpolatedStringHandler.AppendLiteral(". Size must match.");
									Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), thing2.def.GetHashCode() ^ 2945741);
								}
							}
							else
							{
								thing2 = ThingMaker.MakeThing(thing, stuff);
								thing2.SetFaction(faction, null);
								GenSpawn.Spawn(thing2, intVec, map, rot, WipeMode.Vanish, false, false);
							}
							num++;
							foreach (IntVec3 key2 in arg.Cells)
							{
								edgeSpawnOptions.Remove(key2);
							}
							if (spawned != null)
							{
								spawned.Add(thing2);
							}
						}
					}
					num4++;
				}
				num2++;
			}
		}

		// Token: 0x0600748F RID: 29839 RVA: 0x0028D208 File Offset: 0x0028B408
		public static void FillPrefabsAroundEdges(LayoutPrefabParms parms, int count, LayoutRoom room, Map map, Func<IntVec3, Rot4, bool> validator = null, List<Thing> spawned = null, int contractedBy = 1, bool avoidDoors = true, Faction faction = null, Func<PrefabThingData, Tuple<ThingDef, ThingDef>> overrideSpawnData = null)
		{
			Dictionary<IntVec3, ValueTuple<Rot4, CellRect>> edgeSpawnOptions = RoomGenUtility.GetEdgeSpawnOptions(room, map, contractedBy);
			if (edgeSpawnOptions.Count == 0)
			{
				return;
			}
			int num = 0;
			int num2 = 0;
			while (num2 < count && num < count)
			{
				if (edgeSpawnOptions.Count == 0)
				{
					return;
				}
				IntVec3 intVec;
				ValueTuple<Rot4, CellRect> valueTuple;
				edgeSpawnOptions.RandomElement<KeyValuePair<IntVec3, ValueTuple<Rot4, CellRect>>>().Deconstruct(out intVec, out valueTuple);
				ValueTuple<Rot4, CellRect> valueTuple2 = valueTuple;
				IntVec3 intVec2 = intVec;
				Rot4 item = valueTuple2.Item1;
				CellRect item2 = valueTuple2.Item2;
				edgeSpawnOptions.Remove(intVec2);
				Rot4 rot = item.Rotated(parms.rotOffset);
				if (parms.def.rotations.HasFlag(rot))
				{
					CellRect cellRect = intVec2.RectAbout(parms.def.size, rot);
					IntVec3 offset = -item.FacingCell;
					int num3 = room.Boundary.GetSideLength(item.Rotated(RotationDirection.Clockwise));
					while (!cellRect.GetEdgeCells(item).Contains(intVec2) && --num3 > 0)
					{
						cellRect = cellRect.MovedBy(offset);
					}
					bool flag = false;
					foreach (CellRect cellRect2 in room.rects)
					{
						if (cellRect.FullyContainedWithin(cellRect2.ContractedBy(1)))
						{
							flag = true;
							break;
						}
					}
					if (flag && (validator == null || validator(cellRect.CenterCell, rot)))
					{
						foreach (ValueTuple<PrefabThingData, IntVec3, Rot4> valueTuple3 in PrefabUtility.GetThings(parms.def, cellRect.CenterCell, rot))
						{
							PrefabThingData item3 = valueTuple3.Item1;
							IntVec3 item4 = valueTuple3.Item2;
							Rot4 item5 = valueTuple3.Item3;
							if (item3.def.passability != Traversability.Standable)
							{
								foreach (IntVec3 cell in item4.RectAbout(item3.def.Size, item5))
								{
									if (avoidDoors && RoomGenUtility.IsDoorAdjacentTo(cell, map, true))
									{
										flag = false;
										break;
									}
									if (!RoomGenUtility.IsFillValidCell(item3.def, cell, room, map, contractedBy, item2))
									{
										flag = false;
										break;
									}
								}
							}
						}
						if (flag)
						{
							PrefabUtility.SpawnPrefab(parms.def, map, cellRect.CenterCell, rot, faction, spawned, overrideSpawnData, null, false);
							num++;
							foreach (IntVec3 key in cellRect.Cells)
							{
								edgeSpawnOptions.Remove(key);
							}
						}
					}
				}
				num2++;
			}
		}

		// Token: 0x06007490 RID: 29840 RVA: 0x0028D4E8 File Offset: 0x0028B6E8
		[return: TupleElementNames(new string[]
		{
			"rot",
			"wall"
		})]
		private static Dictionary<IntVec3, ValueTuple<Rot4, CellRect>> GetEdgeSpawnOptions(LayoutRoom room, Map map, int contractedBy)
		{
			Dictionary<IntVec3, ValueTuple<Rot4, CellRect>> dictionary = new Dictionary<IntVec3, ValueTuple<Rot4, CellRect>>();
			foreach (CellRect rhs in room.rects)
			{
				CellRect cellRect = rhs.ContractedBy(contractedBy);
				for (int i = 0; i < 4; i++)
				{
					Rot4 rot = new Rot4(i);
					CellRect edgeRect = rhs.GetEdgeRect(rot);
					foreach (IntVec3 intVec in cellRect.GetEdgeCells(rot))
					{
						bool flag = true;
						if (!dictionary.ContainsKey(intVec) && (intVec + rot.FacingCell).GetEdifice(map) != null)
						{
							foreach (CellRect lhs in room.rects)
							{
								if (!(lhs == rhs) && lhs.Contains(intVec))
								{
									flag = false;
									break;
								}
							}
							if (flag)
							{
								dictionary[intVec] = new ValueTuple<Rot4, CellRect>(rot, edgeRect);
							}
						}
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06007491 RID: 29841 RVA: 0x0028D66C File Offset: 0x0028B86C
		public static bool IsDoorAdjacentTo(IntVec3 cell, Map map, bool cardinalOnly = true)
		{
			return RoomGenUtility.IsAdjacentTo(cell, (IntVec3 p) => p.GetDoor(map) != null, cardinalOnly);
		}

		// Token: 0x06007492 RID: 29842 RVA: 0x0028D69C File Offset: 0x0028B89C
		public static bool IsAdjacentTo(IntVec3 cell, Predicate<IntVec3> predicate, bool cardinalOnly = false)
		{
			IntVec3[] array = cardinalOnly ? GenAdj.CardinalDirections : GenAdj.AdjacentCellsAround;
			for (int i = 0; i < array.Length; i++)
			{
				if (predicate(cell + array[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007493 RID: 29843 RVA: 0x0028D6E0 File Offset: 0x0028B8E0
		public static Building_AncientCryptosleepCasket SpawnCryptoCasket(IntVec3 cell, Map map, Rot4 rot, int groupID, PodContentsType type, ThingSetMakerDef thingSetMakerDef)
		{
			Building_AncientCryptosleepCasket building_AncientCryptosleepCasket = (Building_AncientCryptosleepCasket)ThingMaker.MakeThing(ThingDefOf.AncientCryptosleepCasket, null);
			building_AncientCryptosleepCasket.groupID = groupID;
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.podContentsType = new PodContentsType?(type);
			List<Thing> list = thingSetMakerDef.root.Generate(parms);
			for (int i = 0; i < list.Count; i++)
			{
				if (!building_AncientCryptosleepCasket.TryAcceptThing(list[i], false))
				{
					Pawn pawn = list[i] as Pawn;
					if (pawn != null)
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
					}
					else
					{
						list[i].Destroy(DestroyMode.Vanish);
					}
				}
			}
			GenSpawn.Spawn(building_AncientCryptosleepCasket, cell, map, rot, WipeMode.Vanish, false, false);
			return building_AncientCryptosleepCasket;
		}

		// Token: 0x06007494 RID: 29844 RVA: 0x0028D786 File Offset: 0x0028B986
		public static SignalAction_OpenCasket SpawnOpenCryptoCasketSignal(Building_AncientCryptosleepCasket pod, Map map, string signalTag)
		{
			SignalAction_OpenCasket signalAction_OpenCasket = (SignalAction_OpenCasket)ThingMaker.MakeThing(ThingDefOf.SignalAction_OpenCasket, null);
			signalAction_OpenCasket.signalTag = signalTag;
			signalAction_OpenCasket.caskets.Add(pod);
			GenSpawn.Spawn(signalAction_OpenCasket, pod.Position, map, WipeMode.Vanish);
			return signalAction_OpenCasket;
		}

		// Token: 0x06007495 RID: 29845 RVA: 0x0028D7BA File Offset: 0x0028B9BA
		public static void SpawnDormantThreatCluster(LayoutRoom room, Map map, float threatPoints, string signalTag = null)
		{
			if (Rand.Chance(0.5f) && Faction.OfInsects != null)
			{
				RoomGenUtility.SpawnDormantInsectCluster(room, map, threatPoints, signalTag);
				return;
			}
			if (Faction.OfMechanoids != null)
			{
				RoomGenUtility.SpawnDormantMechCluster(room, map, threatPoints, signalTag);
			}
		}

		// Token: 0x06007496 RID: 29846 RVA: 0x0028D7E9 File Offset: 0x0028B9E9
		public static void SpawnDormantInsectCluster(LayoutRoom room, Map map, float threatPoints, string signalTag = null)
		{
			RoomGenUtility.SpawnDormantThreatCluster(room, map, Faction.OfInsects, (PawnKindDef kind) => kind.RaceProps.Insect, threatPoints, signalTag);
		}

		// Token: 0x06007497 RID: 29847 RVA: 0x0028D818 File Offset: 0x0028BA18
		public static void SpawnDormantMechCluster(LayoutRoom room, Map map, float threatPoints, string signalTag = null)
		{
			RoomGenUtility.SpawnDormantThreatCluster(room, map, Faction.OfMechanoids, new Func<PawnKindDef, bool>(MechClusterGenerator.MechKindSuitableForCluster), threatPoints, signalTag);
		}

		// Token: 0x06007498 RID: 29848 RVA: 0x0028D834 File Offset: 0x0028BA34
		public static void SpawnDormantThreatCluster(LayoutRoom room, Map map, Faction faction, Func<PawnKindDef, bool> selector, float threatPoints, string signalTag = null)
		{
			RoomGenUtility.<>c__DisplayClass18_0 CS$<>8__locals1 = new RoomGenUtility.<>c__DisplayClass18_0();
			CS$<>8__locals1.map = map;
			if (faction == null)
			{
				Log.Warning("Tried to spawn dormant threat cluster with null faction.");
				return;
			}
			int num = room.rects.Max((CellRect r) => Mathf.Max(r.Width, r.Height));
			LordJob_StructureThreatCluster lordJob = new LordJob_StructureThreatCluster(faction, room.rects[0].CenterCell, Mathf.Min(12f, (float)(num * 2)), true, true);
			Lord lord = LordMaker.MakeNewLord(faction, lordJob, CS$<>8__locals1.map, null);
			foreach (PawnKindDef kindDef in PawnUtility.GetCombatPawnKindsForPoints(selector, threatPoints, (PawnKindDef pk) => 1f / pk.combatPower))
			{
				Pawn pawn = PawnGenerator.GeneratePawn(kindDef, faction, new PlanetTile?(CS$<>8__locals1.map.Tile));
				IntVec3 loc;
				if (room.TryGetRandomCellInRoom(out loc, 1, new Func<IntVec3, bool>(CS$<>8__locals1.<SpawnDormantThreatCluster>g__Validator|2)))
				{
					GenSpawn.Spawn(pawn, loc, CS$<>8__locals1.map, WipeMode.Vanish);
					lord.AddPawn(pawn);
				}
			}
			if (signalTag != null)
			{
				SignalAction_DormancyWakeUp signalAction_DormancyWakeUp = (SignalAction_DormancyWakeUp)ThingMaker.MakeThing(ThingDefOf.SignalAction_DormancyWakeUp, null);
				signalAction_DormancyWakeUp.signalTag = signalTag;
				signalAction_DormancyWakeUp.lord = lord;
				GenSpawn.Spawn(signalAction_DormancyWakeUp, CS$<>8__locals1.map.Center, CS$<>8__locals1.map, WipeMode.Vanish);
			}
		}

		// Token: 0x06007499 RID: 29849 RVA: 0x0028D9A4 File Offset: 0x0028BBA4
		public static void SpawnWallAttatchments(ThingDef thingDef, Map map, LayoutRoom room, float chance = 1f, float thingsPerSide10Cells = 1f, Predicate<IntVec3, Rot4> validator = null, ThingDef stuff = null, Faction faction = null)
		{
			RoomGenUtility.SpawnWallAttatchments(thingDef, map, room.rects, chance, thingsPerSide10Cells, validator, stuff, faction);
		}

		// Token: 0x0600749A RID: 29850 RVA: 0x0028D9BC File Offset: 0x0028BBBC
		public static void SpawnWallAttatchments(ThingDef thingDef, Map map, List<CellRect> rects, float chance = 1f, float thingsPerSide10Cells = 1f, Predicate<IntVec3, Rot4> validator = null, ThingDef stuff = null, Faction faction = null)
		{
			foreach (IntVec3 cell in RoomGenUtility.IdealWallAttatchmentPositions(thingDef, map, rects, thingsPerSide10Cells, validator))
			{
				if (Rand.Chance(chance))
				{
					Thing thing;
					RoomGenUtility.TrySpawnWallAttatchment(thingDef, cell, map, out thing, true, validator, stuff, faction);
				}
			}
		}

		// Token: 0x0600749B RID: 29851 RVA: 0x0028DA30 File Offset: 0x0028BC30
		public static void SpawnWallAttatchments(ThingDef thingDef, Map map, LayoutRoom room, IntRange count, Predicate<IntVec3, Rot4> validator = null, ThingDef stuff = null, Faction faction = null)
		{
			RoomGenUtility.SpawnWallAttatchments(thingDef, map, room.rects, count, validator, stuff, faction);
		}

		// Token: 0x0600749C RID: 29852 RVA: 0x0028DA48 File Offset: 0x0028BC48
		public static void SpawnWallAttatchments(ThingDef thingDef, Map map, List<CellRect> rects, IntRange count, Predicate<IntVec3, Rot4> validator = null, ThingDef stuff = null, Faction faction = null)
		{
			List<IntVec3> list = RoomGenUtility.IdealWallAttatchmentPositions(thingDef, map, rects, 1f, validator);
			int randomInRange = count.RandomInRange;
			int num = 0;
			int num2 = 0;
			while (num2 < randomInRange && list.Any<IntVec3>())
			{
				IntVec3 intVec = list.RandomElement<IntVec3>();
				list.Remove(intVec);
				Thing thing;
				if (RoomGenUtility.TrySpawnWallAttatchment(thingDef, intVec, map, out thing, true, validator, stuff, faction))
				{
					num++;
				}
				num2++;
			}
			int num3 = num;
			while (num3 < count.TrueMin && num < count.TrueMin)
			{
				foreach (CellRect cellRect in rects)
				{
					foreach (IntVec3 cell in cellRect.ContractedBy(1).EdgeCells.InRandomOrder(null))
					{
						Thing thing;
						if (RoomGenUtility.TrySpawnWallAttatchment(thingDef, cell, map, out thing, true, validator, stuff, faction))
						{
							num++;
						}
						if (num >= count.TrueMin)
						{
							break;
						}
					}
					if (num >= count.TrueMin)
					{
						break;
					}
				}
				num3++;
			}
		}

		// Token: 0x0600749D RID: 29853 RVA: 0x0028DB90 File Offset: 0x0028BD90
		private static List<IntVec3> IdealWallAttatchmentPositions(ThingDef thingDef, Map map, List<CellRect> rects, float thingsPerSide10Cells = 1f, Predicate<IntVec3, Rot4> validator = null)
		{
			List<IntVec3> list = new List<IntVec3>();
			foreach (CellRect cellRect in rects)
			{
				CellRect cellRect2 = cellRect.ContractedBy(1);
				for (int i = 0; i < 2; i++)
				{
					Rot4 rot = (i == 0) ? Rot4.North : Rot4.East;
					float num = (float)cellRect2.GetSideLength(rot) / 2f;
					int b = Mathf.RoundToInt(num / 3f);
					int num2 = Mathf.Min(Mathf.RoundToInt(num / 10f * thingsPerSide10Cells), b);
					if (num2 > 0)
					{
						int num3 = Mathf.Max(3, Mathf.RoundToInt(num / (float)(num2 + 1)));
						int num4 = num3;
						for (int j = 0; j < num2; j++)
						{
							for (int k = 0; k < 2; k++)
							{
								int num5 = num4;
								if (cellRect2.Width % 2 == 0 && k == 0 && i == 0)
								{
									num5--;
								}
								else if (cellRect2.Height % 2 == 0 && k == 0 && i == 1)
								{
									num5--;
								}
								for (int l = 0; l < 2; l++)
								{
									Rot4 rot2 = (l == 0) ? rot : rot.Opposite;
									IntVec3 centerCellOnEdge = cellRect2.GetCenterCellOnEdge(rot2, (k == 0) ? num5 : (-num5));
									Rot4 rot3;
									if (cellRect2.Contains(centerCellOnEdge) && !list.Contains(centerCellOnEdge) && RoomGenUtility.CanPlaceWallAttachment(thingDef, centerCellOnEdge, map, out rot3, true, validator))
									{
										list.Add(centerCellOnEdge);
									}
								}
							}
							num4 += num3;
						}
					}
				}
			}
			return list;
		}

		// Token: 0x0600749E RID: 29854 RVA: 0x0028DD4C File Offset: 0x0028BF4C
		public static bool TrySpawnWallAttatchment(ThingDef thingDef, IntVec3 cell, Map map, out Thing spawned, bool avoidEdifices = true, Predicate<IntVec3, Rot4> validator = null, ThingDef stuff = null, Faction faction = null)
		{
			Rot4 rot;
			if (RoomGenUtility.CanPlaceWallAttachment(thingDef, cell, map, out rot, avoidEdifices, validator))
			{
				spawned = ThingMaker.MakeThing(thingDef, stuff);
				spawned.SetFactionDirect(faction);
				GenSpawn.Spawn(spawned, cell, map, rot, WipeMode.Vanish, false, false);
				return true;
			}
			spawned = null;
			return false;
		}

		// Token: 0x0600749F RID: 29855 RVA: 0x0028DD90 File Offset: 0x0028BF90
		public static bool CanPlaceWallAttachment(ThingDef thingDef, IntVec3 cell, Map map, out Rot4 rotation, bool avoidEdifices = true, Predicate<IntVec3, Rot4> validator = null)
		{
			rotation = default(Rot4);
			if (avoidEdifices && cell.GetEdifice(map) != null)
			{
				return false;
			}
			foreach (Rot4 rot in Rot4.AllRotations)
			{
				if (RoomGenUtility.CanPlace(thingDef, cell, map, rot) && (validator == null || validator(cell, rot)))
				{
					rotation = rot;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060074A0 RID: 29856 RVA: 0x0028DE14 File Offset: 0x0028C014
		public static bool CanPlace(ThingDef thingDef, IntVec3 cell, Map map, Rot4 rot)
		{
			if (thingDef.IsEdifice() && cell.GetEdifice(map) != null)
			{
				return false;
			}
			using (List<PlaceWorker>.Enumerator enumerator = thingDef.PlaceWorkers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.AllowsPlacing(thingDef, cell, rot, map, null, null))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060074A1 RID: 29857 RVA: 0x0028DE8C File Offset: 0x0028C08C
		public static void SpawnCorpses(LayoutRoom room, Map map, IntRange countRange, PawnKindDef kind, DamageDef damageType, ThingDef killerThing = null, Tool toolUsed = null, FloatRange? ageDaysRange = null, IntRange? bloodFilthRange = null, int? fixedAge = null, bool forceNoGear = false)
		{
			FloatRange floatRange = ageDaysRange ?? new FloatRange(3f, 600f);
			int randomInRange = countRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				int num = Mathf.RoundToInt(floatRange.RandomInRange * 60000f);
				IntVec3 intVec;
				if (room.TryGetRandomCellInRoom(out intVec, 1, null))
				{
					IntVec3 cell = intVec;
					int deadTicks = num;
					int? num2 = fixedAge;
					RoomGenUtility.SpawnCorpse(cell, kind, deadTicks, map, damageType, (num2 != null) ? new float?((float)num2.GetValueOrDefault()) : null, forceNoGear, killerThing, toolUsed, bloodFilthRange, null);
				}
			}
		}

		// Token: 0x060074A2 RID: 29858 RVA: 0x0028DF30 File Offset: 0x0028C130
		public static Corpse SpawnCorpse(IntVec3 cell, PawnKindDef kind, int deadTicks, Map map, DamageDef damageType, float? fixedAge = null, bool forceNoGear = false, ThingDef killerThing = null, Tool toolUsed = null, IntRange? bloodFilthRange = null, BodyPartDef idealPart = null)
		{
			IntRange countRange = bloodFilthRange ?? new IntRange(1, 5);
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind, null, PawnGenerationContext.NonPlayer, new PlanetTile?(map.Tile), false, true, true, true, false, 1f, false, true, false, false, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, fixedAge, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, forceNoGear));
			if (!pawn.Dead)
			{
				HealthUtility.SimulateKilled(pawn, damageType, killerThing, toolUsed, idealPart);
			}
			pawn.Corpse.Age = deadTicks;
			pawn.relations.hidePawnRelations = true;
			pawn.Corpse.GetComp<CompRottable>().RotProgress += (float)pawn.Corpse.Age;
			Corpse corpse = (Corpse)GenSpawn.Spawn(pawn.Corpse, cell, map, WipeMode.Vanish);
			if (pawn.RaceProps.BloodDef != null)
			{
				GenSpawn.SpawnIrregularLump(pawn.RaceProps.BloodDef, cell, map, countRange, new IntRange(2, 4), WipeMode.Vanish, null, null, null, null, null);
			}
			corpse.TrySetForbidden(true);
			return corpse;
		}

		// Token: 0x060074A3 RID: 29859 RVA: 0x0028E087 File Offset: 0x0028C287
		public static bool IsGoodForDoor(IntVec3 p, Map map)
		{
			return RoomGenUtility.IsGoodForHorizontalDoor(p, map) || RoomGenUtility.IsGoodForVerticalDoor(p, map);
		}

		// Token: 0x060074A4 RID: 29860 RVA: 0x0028E09C File Offset: 0x0028C29C
		public static bool IsGoodForHorizontalDoor(IntVec3 p, Map map)
		{
			return RoomGenUtility.IsWallAt(p + IntVec3.West, map) && RoomGenUtility.IsWallAt(p + IntVec3.East, map) && !RoomGenUtility.IsWallAt(p + IntVec3.North, map) && !RoomGenUtility.IsWallAt(p + IntVec3.South, map);
		}

		// Token: 0x060074A5 RID: 29861 RVA: 0x0028E0F8 File Offset: 0x0028C2F8
		public static bool IsGoodForVerticalDoor(IntVec3 p, Map map)
		{
			return RoomGenUtility.IsWallAt(p + IntVec3.North, map) && RoomGenUtility.IsWallAt(p + IntVec3.South, map) && !RoomGenUtility.IsWallAt(p + IntVec3.East, map) && !RoomGenUtility.IsWallAt(p + IntVec3.West, map);
		}

		// Token: 0x060074A6 RID: 29862 RVA: 0x0028E154 File Offset: 0x0028C354
		public static bool IsWallAt(IntVec3 position, Map map)
		{
			Building edifice = position.GetEdifice(map);
			return edifice != null && edifice.def.IsWall;
		}

		// Token: 0x060074A7 RID: 29863 RVA: 0x0028E16D File Offset: 0x0028C36D
		[Obsolete]
		public static Queue<CellRect> SubdividedIntoRooms(Map map, CellRect rect, int innerWallsContract = 3, int minRoomSize = 5, int wallMoveRange = 1, int maxRooms = 2, ThingDef wall = null, ThingDef wallStuff = null)
		{
			return RoomGenUtility.SubdividedIntoRooms_NewTemp(map, rect.ContractedBy(innerWallsContract), minRoomSize, wallMoveRange, maxRooms, wall, wallStuff);
		}

		// Token: 0x060074A8 RID: 29864 RVA: 0x0028E188 File Offset: 0x0028C388
		public static Queue<CellRect> SubdividedIntoRooms_NewTemp(Map map, CellRect rect, int minRoomSize = 5, int wallMoveRange = 1, int maxRooms = 2, ThingDef wall = null, ThingDef wallStuff = null)
		{
			int a = Mathf.FloorToInt(((float)rect.Width - 2f) / (float)minRoomSize);
			int a2 = Mathf.FloorToInt(((float)rect.Height - 2f) / (float)minRoomSize);
			int num = Rand.Range(0, Mathf.Min(a, maxRooms));
			int num2 = Rand.Range(0, Mathf.Min(a2, maxRooms));
			Queue<CellRect> queue = new Queue<CellRect>();
			queue.Enqueue(rect);
			int num3 = -1;
			int num4 = -1;
			for (int i = 0; i < num; i++)
			{
				ValueTuple<CellRect, CellRect> valueTuple = queue.Dequeue().SplitHorizontal(0);
				CellRect item = valueTuple.Item1;
				CellRect item2 = valueTuple.Item2;
				int num5 = Rand.RangeInclusive(-wallMoveRange, wallMoveRange);
				num3 = item.maxX + num5;
				item.maxX += num5 - 1;
				item2.minX += num5 + 1;
				queue.Enqueue(item);
				queue.Enqueue(item2);
			}
			for (int j = 0; j < num2; j++)
			{
				int count = queue.Count;
				for (int k = 0; k < count; k++)
				{
					ValueTuple<CellRect, CellRect> valueTuple2 = queue.Dequeue().SplitVertical(0);
					CellRect item3 = valueTuple2.Item1;
					CellRect item4 = valueTuple2.Item2;
					int num6 = Rand.RangeInclusive(-wallMoveRange, wallMoveRange);
					num4 = item3.maxZ + num6;
					item3.maxZ += num6 - 1;
					item4.minZ += num6 + 1;
					queue.Enqueue(item3);
					queue.Enqueue(item4);
				}
			}
			if (num3 >= 0)
			{
				for (int l = rect.minZ; l <= rect.maxZ; l++)
				{
					ThingDef thingDef = wall ?? ThingDefOf.Wall;
					GenSpawn.Spawn(ThingMaker.MakeThing(thingDef, thingDef.MadeFromStuff ? (wallStuff ?? ThingDefOf.Steel) : null), new IntVec3(num3, 0, l), map, WipeMode.Vanish);
				}
			}
			if (num4 >= 0)
			{
				for (int m = rect.minX; m <= rect.maxX; m++)
				{
					ThingDef thingDef2 = wall ?? ThingDefOf.Wall;
					GenSpawn.Spawn(ThingMaker.MakeThing(thingDef2, thingDef2.MadeFromStuff ? (wallStuff ?? ThingDefOf.Steel) : null), new IntVec3(m, 0, num4), map, WipeMode.Vanish);
				}
			}
			return queue;
		}

		// Token: 0x060074A9 RID: 29865 RVA: 0x0028E3B2 File Offset: 0x0028C5B2
		public static Building_Crate SpawnHermeticCrate(IntVec3 cell, Map map, ThingSetMakerDef setMaker, bool addRewards = true, string signal = null)
		{
			return RoomGenUtility.SpawnCrate(ThingDefOf.AncientHermeticCrate, cell, map, Rot4.South, setMaker, addRewards, signal);
		}

		// Token: 0x060074AA RID: 29866 RVA: 0x0028E3C9 File Offset: 0x0028C5C9
		public static Building_Crate SpawnHermeticCrate(IntVec3 cell, Map map, Rot4 rot, ThingSetMakerDef setMaker, bool addRewards = true, string signal = null)
		{
			return RoomGenUtility.SpawnCrate(ThingDefOf.AncientHermeticCrate, cell, map, rot, setMaker, addRewards, signal);
		}

		// Token: 0x060074AB RID: 29867 RVA: 0x0028E3DD File Offset: 0x0028C5DD
		public static Building_Crate SpawnCrate(ThingDef crateDef, IntVec3 cell, Map map, ThingSetMakerDef rewardMaker, bool addRewards = true, string signal = null)
		{
			return RoomGenUtility.SpawnCrate(crateDef, cell, map, Rot4.South, rewardMaker, addRewards, signal);
		}

		// Token: 0x060074AC RID: 29868 RVA: 0x0028E3F4 File Offset: 0x0028C5F4
		public static Building_Crate SpawnCrate(ThingDef crateDef, IntVec3 cell, Map map, Rot4 rot, ThingSetMakerDef rewardMaker, bool addRewards = true, string signal = null)
		{
			Building_Crate building_Crate = (Building_Crate)GenSpawn.Spawn(ThingMaker.MakeThing(crateDef, null), cell, map, rot, WipeMode.Vanish, false, false);
			if (addRewards)
			{
				List<Thing> list = rewardMaker.root.Generate(default(ThingSetMakerParams));
				for (int i = list.Count - 1; i >= 0; i--)
				{
					Thing thing = list[i];
					if (!building_Crate.TryAcceptThing(thing, false))
					{
						thing.Destroy(DestroyMode.Vanish);
					}
				}
				if (!string.IsNullOrEmpty(signal))
				{
					building_Crate.openedSignal = signal;
				}
			}
			else
			{
				building_Crate.Open();
			}
			return building_Crate;
		}

		// Token: 0x060074AD RID: 29869 RVA: 0x0028E47C File Offset: 0x0028C67C
		public static void SpawnHermeticCrateInRoom(LayoutRoom room, Map map, ThingSetMakerDef rewardMaker, bool addRewards = true, string signal = null, Rot4? rot = null)
		{
			RoomGenUtility.SpawnCratesInRoom(ThingDefOf.AncientHermeticCrate, room, map, rewardMaker, IntRange.One, addRewards, signal, rot);
		}

		// Token: 0x060074AE RID: 29870 RVA: 0x0028E495 File Offset: 0x0028C695
		public static void SpawnHermeticCratesInRoom(LayoutRoom room, Map map, ThingSetMakerDef rewardMaker, IntRange countRange, bool addRewards = true, string signal = null, Rot4? rot = null)
		{
			RoomGenUtility.SpawnCratesInRoom(ThingDefOf.AncientHermeticCrate, room, map, rewardMaker, countRange, addRewards, signal, rot);
		}

		// Token: 0x060074AF RID: 29871 RVA: 0x0028E4AB File Offset: 0x0028C6AB
		public static void SpawnCrateInRoom(ThingDef crateDef, LayoutRoom room, Map map, ThingSetMakerDef rewardMaker, bool addRewards = true, string signal = null, Rot4? rot = null)
		{
			RoomGenUtility.SpawnCratesInRoom(crateDef, room, map, rewardMaker, IntRange.One, addRewards, signal, rot);
		}

		// Token: 0x060074B0 RID: 29872 RVA: 0x0028E4C4 File Offset: 0x0028C6C4
		public static void SpawnCratesInRoom(ThingDef crateDef, LayoutRoom room, Map map, ThingSetMakerDef rewardMaker, IntRange countRange, bool addRewards = true, string signal = null, Rot4? rot = null)
		{
			RoomGenUtility.<>c__DisplayClass42_0 CS$<>8__locals1 = new RoomGenUtility.<>c__DisplayClass42_0();
			CS$<>8__locals1.crateDef = crateDef;
			CS$<>8__locals1.map = map;
			int num = countRange.RandomInRange;
			CS$<>8__locals1.rotation = (rot ?? Rot4.South);
			IntVec3 cell;
			while (num > 0 && room.TryGetRandomCellInRoom(out cell, 1, new Func<IntVec3, bool>(CS$<>8__locals1.<SpawnCratesInRoom>g__Validator|0)))
			{
				RoomGenUtility.SpawnCrate(CS$<>8__locals1.crateDef, cell, CS$<>8__locals1.map, CS$<>8__locals1.rotation, rewardMaker, addRewards, signal);
				num--;
			}
		}

		// Token: 0x060074B1 RID: 29873 RVA: 0x0028E54C File Offset: 0x0028C74C
		public static bool IsValidCrateCell(ThingDef crate, IntVec3 pos, Map map, Rot4 rot)
		{
			CellRect cellRect = pos.RectAbout(crate.Size, rot);
			using (IEnumerator<IntVec3> enumerator = cellRect.Cells.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetEdifice(map) != null)
					{
						return false;
					}
				}
			}
			foreach (IntVec3 root in cellRect.EdgeCells)
			{
				foreach (IntVec3 c in GenAdjFast.AdjacentCells8Way(root))
				{
					if (!cellRect.Contains(c) && c.GetEdifice(map) != null)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060074B2 RID: 29874 RVA: 0x0028E63C File Offset: 0x0028C83C
		public static bool IsClearAndNotAdjacentToDoor(ThingDef def, IntVec3 pos, Map map, Rot4 rot)
		{
			return RoomGenUtility.IsClearAndNotAdjacentToDoor(pos.RectAbout(def.Size, rot), map);
		}

		// Token: 0x060074B3 RID: 29875 RVA: 0x0028E654 File Offset: 0x0028C854
		public static bool IsClearAndNotAdjacentToDoor(CellRect rect, Map map)
		{
			using (IEnumerator<IntVec3> enumerator = rect.Cells.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetEdifice(map) != null)
					{
						return false;
					}
				}
			}
			foreach (IntVec3 root in rect.EdgeCells)
			{
				foreach (IntVec3 c in GenAdjFast.AdjacentCellsCardinal(root))
				{
					if (!rect.Contains(c) && c.GetDoor(map) != null)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060074B4 RID: 29876 RVA: 0x0028E734 File Offset: 0x0028C934
		public static void SpawnDoorBarricades(ThingDef def, LayoutRoom room, Map map, float chancePerDoor = 0.75f, ThingDef stuff = null, int steps = 2, int offset = 2)
		{
			RoomGenUtility.<>c__DisplayClass46_0 CS$<>8__locals1;
			CS$<>8__locals1.def = def;
			CS$<>8__locals1.stuff = stuff;
			CS$<>8__locals1.map = map;
			List<ValueTuple<IntVec3, Rot4>> list = new List<ValueTuple<IntVec3, Rot4>>();
			foreach (CellRect cellRect in room.rects)
			{
				foreach (IntVec3 intVec in cellRect.EdgeCells)
				{
					if (intVec.GetDoor(CS$<>8__locals1.map) != null)
					{
						list.Add(new ValueTuple<IntVec3, Rot4>(intVec, cellRect.GetEdgeCellRot(intVec)));
					}
				}
			}
			foreach (ValueTuple<IntVec3, Rot4> valueTuple in list)
			{
				IntVec3 item = valueTuple.Item1;
				Rot4 item2 = valueTuple.Item2;
				if (Rand.Chance(chancePerDoor))
				{
					IntVec3 intVec2 = item + item2.Opposite.FacingCell * offset;
					RoomGenUtility.<SpawnDoorBarricades>g__SpawnBarricade|46_0(intVec2, ref CS$<>8__locals1);
					for (int i = 1; i < steps; i++)
					{
						Rot4 rot = item2.Rotated(RotationDirection.Clockwise);
						RoomGenUtility.<SpawnDoorBarricades>g__SpawnBarricade|46_0(intVec2 + rot.FacingCell * i, ref CS$<>8__locals1);
						RoomGenUtility.<SpawnDoorBarricades>g__SpawnBarricade|46_0(intVec2 + rot.FacingCell * -i, ref CS$<>8__locals1);
					}
				}
			}
		}

		// Token: 0x060074B5 RID: 29877 RVA: 0x0028E8D0 File Offset: 0x0028CAD0
		public static void TryPlaceInRandomCorner(Map map, LayoutRoom room, ThingDef thingDef, Faction faction, ThingDef stuff = null)
		{
			RoomGenUtility.<>c__DisplayClass47_0 CS$<>8__locals1 = new RoomGenUtility.<>c__DisplayClass47_0();
			CS$<>8__locals1.map = map;
			List<IntVec3> list = room.rects.SelectMany((CellRect r) => r.ContractedBy(1).Corners).Where(new Func<IntVec3, bool>(CS$<>8__locals1.<TryPlaceInRandomCorner>g__Predicate|0)).ToList<IntVec3>();
			if (list.Count == 0)
			{
				return;
			}
			IntVec3 loc = list.RandomElement<IntVec3>();
			Thing thing = ThingMaker.MakeThing(thingDef, stuff);
			thing.SetFactionDirect(faction);
			GenSpawn.Spawn(thing, loc, CS$<>8__locals1.map, WipeMode.Vanish);
		}

		// Token: 0x060074B6 RID: 29878 RVA: 0x0028E958 File Offset: 0x0028CB58
		[CompilerGenerated]
		internal static void <SpawnDoorBarricades>g__SpawnBarricade|46_0(IntVec3 pos, ref RoomGenUtility.<>c__DisplayClass46_0 A_1)
		{
			GenSpawn.Spawn(ThingMaker.MakeThing(A_1.def, A_1.stuff), pos, A_1.map, WipeMode.Vanish);
		}
	}
}
