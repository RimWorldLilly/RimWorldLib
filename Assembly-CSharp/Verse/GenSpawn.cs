using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020006F7 RID: 1783
	public static class GenSpawn
	{
		// Token: 0x060038AE RID: 14510 RVA: 0x0016B8EB File Offset: 0x00169AEB
		public static Thing Spawn(ThingDef def, IntVec3 loc, Map map, WipeMode wipeMode = WipeMode.Vanish)
		{
			return GenSpawn.Spawn(def, loc, map, Rot4.North, wipeMode);
		}

		// Token: 0x060038AF RID: 14511 RVA: 0x0016B8FB File Offset: 0x00169AFB
		public static Thing Spawn(ThingDef def, IntVec3 loc, Map map, Rot4 rot, WipeMode wipeMode = WipeMode.Vanish)
		{
			if (!def.CanSpawnAt(loc, rot, map))
			{
				return null;
			}
			return GenSpawn.Spawn(ThingMaker.MakeThing(def, null), loc, map, rot, wipeMode, false, false);
		}

		// Token: 0x060038B0 RID: 14512 RVA: 0x0016B91D File Offset: 0x00169B1D
		public static Thing Spawn(Thing newThing, IntVec3 loc, Map map, WipeMode wipeMode = WipeMode.Vanish)
		{
			return GenSpawn.Spawn(newThing, loc, map, Rot4.North, wipeMode, false, false);
		}

		// Token: 0x060038B1 RID: 14513 RVA: 0x0016B930 File Offset: 0x00169B30
		public static bool TrySpawn(ThingDef def, IntVec3 loc, Map map, out Thing thing, WipeMode wipeMode = WipeMode.Vanish, bool canWipeEdifices = true)
		{
			if (!GenSpawn.CanSpawnAt(def, loc, map, null, canWipeEdifices))
			{
				thing = null;
				return false;
			}
			thing = GenSpawn.Spawn(def, loc, map, wipeMode);
			return thing != null;
		}

		// Token: 0x060038B2 RID: 14514 RVA: 0x0016B96A File Offset: 0x00169B6A
		public static bool TrySpawn(ThingDef def, IntVec3 loc, Map map, Rot4 rot, out Thing thing, WipeMode wipeMode = WipeMode.Vanish, bool canWipeEdifices = true)
		{
			if (!GenSpawn.CanSpawnAt(def, loc, map, new Rot4?(rot), canWipeEdifices))
			{
				thing = null;
				return false;
			}
			thing = GenSpawn.Spawn(def, loc, map, rot, wipeMode);
			return thing != null;
		}

		// Token: 0x060038B3 RID: 14515 RVA: 0x0016B998 File Offset: 0x00169B98
		public static void SpawnIrregularLump(ThingDef thing, IntVec3 pos, Map map, IntRange countRange, IntRange distRange, WipeMode wipeMode = WipeMode.Vanish, Predicate<IntVec3> validator = null, List<IntVec3> area = null, List<Thing> spawned = null, ThingDef stuff = null, Faction faction = null)
		{
			int randomInRange = distRange.RandomInRange;
			int randomInRange2 = countRange.RandomInRange;
			int num = 0;
			List<IntVec3> list = GridShapeMaker.IrregularLump(pos, map, randomInRange, validator);
			for (int i = 0; i < list.Count; i++)
			{
				IntVec3 intVec = list[i];
				if (Rand.DynamicChance(num, randomInRange2, list.Count - i))
				{
					num++;
					if (thing.IsFilth)
					{
						FilthMaker.TryMakeFilth(intVec, map, thing, 1, FilthSourceFlags.None, true);
					}
					else if (GenSpawn.CanSpawnAt(thing, intVec, map, null, true))
					{
						Thing thing2 = ThingMaker.MakeThing(thing, stuff);
						if (thing2.def.CanHaveFaction)
						{
							thing2.SetFactionDirect(faction);
						}
						GenSpawn.Spawn(thing2, intVec, map, wipeMode);
						if (spawned != null)
						{
							spawned.Add(thing2);
						}
					}
					if (area != null)
					{
						area.Add(intVec);
					}
				}
			}
		}

		// Token: 0x060038B4 RID: 14516 RVA: 0x0016BA74 File Offset: 0x00169C74
		public static Thing Spawn(Thing newThing, IntVec3 loc, Map map, Rot4 rot, WipeMode wipeMode = WipeMode.Vanish, bool respawningAfterLoad = false, bool forbidLeavings = false)
		{
			if (map == null)
			{
				Log.Error("Tried to spawn " + newThing.ToStringSafe<Thing>() + " in a null map.");
				return null;
			}
			if (!loc.InBounds(map))
			{
				string[] array = new string[5];
				array[0] = "Tried to spawn ";
				array[1] = newThing.ToStringSafe<Thing>();
				array[2] = " out of bounds at ";
				int num = 3;
				IntVec3 intVec = loc;
				array[num] = intVec.ToString();
				array[4] = ".";
				Log.Error(string.Concat(array));
				return null;
			}
			if (newThing.def.randomizeRotationOnSpawn)
			{
				rot = Rot4.Random;
			}
			CellRect occupiedRect = GenAdj.OccupiedRect(loc, rot, newThing.def.Size);
			if (!occupiedRect.InBounds(map))
			{
				string[] array2 = new string[7];
				array2[0] = "Tried to spawn ";
				array2[1] = newThing.ToStringSafe<Thing>();
				array2[2] = " out of bounds at ";
				int num2 = 3;
				IntVec3 intVec = loc;
				array2[num2] = intVec.ToString();
				array2[4] = " (out of bounds because size is ";
				array2[5] = newThing.def.Size.ToString();
				array2[6] = ").";
				Log.Error(string.Concat(array2));
				return null;
			}
			if (newThing.Spawned)
			{
				Log.Error("Tried to spawn " + ((newThing != null) ? newThing.ToString() : null) + " but it's already spawned.");
				return newThing;
			}
			if (wipeMode == WipeMode.Vanish)
			{
				GenSpawn.WipeExistingThings(loc, rot, newThing.def, map, DestroyMode.Vanish);
			}
			else if (wipeMode == WipeMode.FullRefund)
			{
				GenSpawn.WipeAndRefundExistingThings(loc, rot, newThing.def, map, forbidLeavings);
			}
			else if (wipeMode == WipeMode.VanishOrMoveAside)
			{
				GenSpawn.CheckMoveItemsAside(loc, rot, newThing.def, map);
				GenSpawn.WipeExistingThings(loc, rot, newThing.def, map, DestroyMode.Vanish);
			}
			if (newThing.def.category == ThingCategory.Item && Current.ProgramState == ProgramState.Playing && loc.GetItemCount(map) >= loc.GetMaxItemsAllowedInCell(map))
			{
				Predicate<IntVec3> <>9__0;
				foreach (Thing thing in loc.GetThingList(map).ToList<Thing>())
				{
					if (thing != newThing && thing.def.category == ThingCategory.Item)
					{
						thing.DeSpawn(DestroyMode.Vanish);
						Thing thing2 = thing;
						ThingPlaceMode mode = ThingPlaceMode.Near;
						Action<Thing, int> placedAction = null;
						Predicate<IntVec3> extraValidator;
						if ((extraValidator = <>9__0) == null)
						{
							extraValidator = (<>9__0 = ((IntVec3 x) => !occupiedRect.Contains(x)));
						}
						if (!GenPlace.TryPlaceThing(thing2, loc, map, mode, placedAction, extraValidator, null, 1))
						{
							thing.Destroy(DestroyMode.Vanish);
							break;
						}
						break;
					}
				}
			}
			newThing.Rotation = rot;
			newThing.Position = loc;
			if (newThing.holdingOwner != null)
			{
				newThing.holdingOwner.Remove(newThing);
			}
			newThing.SpawnSetup(map, respawningAfterLoad);
			if (newThing.Spawned && newThing.stackCount == 0)
			{
				Log.Error("Spawned thing with 0 stackCount: " + ((newThing != null) ? newThing.ToString() : null));
				newThing.Destroy(DestroyMode.Vanish);
				return null;
			}
			if (newThing.def.passability == Traversability.Impassable)
			{
				foreach (IntVec3 c in occupiedRect)
				{
					foreach (Thing thing3 in c.GetThingList(map).ToList<Thing>())
					{
						if (thing3 != newThing)
						{
							Pawn pawn = thing3 as Pawn;
							if (pawn != null)
							{
								pawn.pather.TryRecoverFromUnwalkablePosition(false);
							}
						}
					}
				}
			}
			return newThing;
		}

		// Token: 0x060038B5 RID: 14517 RVA: 0x0016BDF4 File Offset: 0x00169FF4
		public static void SpawnBuildingAsPossible(Building building, Map map, bool respawningAfterLoad = false)
		{
			bool flag = false;
			if (!building.OccupiedRect().InBounds(map))
			{
				flag = true;
			}
			else
			{
				foreach (IntVec3 c in building.OccupiedRect())
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						if (thingList[i] is Pawn && building.def.passability == Traversability.Impassable)
						{
							flag = true;
							break;
						}
						if ((thingList[i].def.category == ThingCategory.Building || thingList[i].def.category == ThingCategory.Item) && GenSpawn.SpawningWipes(building.def, thingList[i].def, false))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			if (flag)
			{
				GenSpawn.Refund(building, map, CellRect.Empty, false, false);
				return;
			}
			GenSpawn.Spawn(building, building.Position, map, building.Rotation, WipeMode.FullRefund, respawningAfterLoad, false);
		}

		// Token: 0x060038B6 RID: 14518 RVA: 0x0016BF18 File Offset: 0x0016A118
		public static void Refund(Thing thing, Map map, CellRect avoidThisRect, bool forbid = false, bool willReplace = false)
		{
			bool flag = false;
			if (thing.def.Minifiable && !thing.def.IsPlant)
			{
				MinifiedThing minifiedThing = thing.MakeMinified(willReplace ? DestroyMode.WillReplace : DestroyMode.Vanish);
				if (GenPlace.TryPlaceThing(minifiedThing, thing.Position, map, ThingPlaceMode.Near, null, (IntVec3 x) => !avoidThisRect.Contains(x), null, 1))
				{
					flag = true;
					minifiedThing.SetForbidden(forbid, true);
				}
				else
				{
					minifiedThing.GetDirectlyHeldThings().Clear();
					minifiedThing.Destroy(DestroyMode.Vanish);
				}
			}
			if (!flag)
			{
				GenSpawn.leavings.Clear();
				GenLeaving.DoLeavingsFor(thing, map, DestroyMode.Refund, thing.OccupiedRect(), (IntVec3 x) => !avoidThisRect.Contains(x), GenSpawn.leavings);
				thing.Destroy(willReplace ? DestroyMode.WillReplace : DestroyMode.Vanish);
				foreach (Thing t in GenSpawn.leavings)
				{
					t.SetForbidden(forbid, true);
				}
			}
		}

		// Token: 0x060038B7 RID: 14519 RVA: 0x0016C024 File Offset: 0x0016A224
		public static void WipeExistingThings(IntVec3 thingPos, Rot4 thingRot, BuildableDef thingDef, Map map, DestroyMode mode)
		{
			foreach (IntVec3 c in GenAdj.CellsOccupiedBy(thingPos, thingRot, thingDef.Size))
			{
				foreach (Thing thing in map.thingGrid.ThingsAt(c).ToList<Thing>())
				{
					if (GenSpawn.SpawningWipes(thingDef, thing.def, false))
					{
						thing.Destroy(mode);
					}
				}
			}
		}

		// Token: 0x060038B8 RID: 14520 RVA: 0x0016C0D0 File Offset: 0x0016A2D0
		public static void WipeAndRefundExistingThings(IntVec3 thingPos, Rot4 thingRot, BuildableDef thingDef, Map map, bool forbid)
		{
			CellRect occupiedRect = GenAdj.OccupiedRect(thingPos, thingRot, thingDef.Size);
			Predicate<IntVec3> <>9__0;
			foreach (IntVec3 intVec in occupiedRect)
			{
				foreach (Thing thing in intVec.GetThingList(map).ToList<Thing>())
				{
					if (GenSpawn.SpawningWipes(thingDef, thing.def, false))
					{
						if (thing.def.category == ThingCategory.Item)
						{
							thing.DeSpawn(DestroyMode.Vanish);
							Thing thing2 = thing;
							IntVec3 center = intVec;
							ThingPlaceMode mode = ThingPlaceMode.Near;
							Action<Thing, int> placedAction = null;
							Predicate<IntVec3> extraValidator;
							if ((extraValidator = <>9__0) == null)
							{
								extraValidator = (<>9__0 = ((IntVec3 x) => !occupiedRect.Contains(x)));
							}
							if (!GenPlace.TryPlaceThing(thing2, center, map, mode, placedAction, extraValidator, null, 1))
							{
								thing.Destroy(DestroyMode.Vanish);
							}
							else
							{
								thing.SetForbidden(thing.IsForbidden(Faction.OfPlayer) || forbid, false);
							}
						}
						else
						{
							GenSpawn.Refund(thing, map, occupiedRect, forbid, thingDef.IsEdifice());
						}
					}
				}
			}
		}

		// Token: 0x060038B9 RID: 14521 RVA: 0x0016C220 File Offset: 0x0016A420
		public static void CheckMoveItemsAside(IntVec3 thingPos, Rot4 thingRot, ThingDef thingDef, Map map)
		{
			if (thingDef.surfaceType != SurfaceType.None || thingDef.passability == Traversability.Standable)
			{
				return;
			}
			CellRect occupiedRect = GenAdj.OccupiedRect(thingPos, thingRot, thingDef.Size);
			Predicate<IntVec3> <>9__0;
			foreach (IntVec3 intVec in occupiedRect)
			{
				foreach (Thing thing in intVec.GetThingList(map).ToList<Thing>())
				{
					if (thing.def.category == ThingCategory.Item)
					{
						thing.DeSpawn(DestroyMode.Vanish);
						Thing thing2 = thing;
						IntVec3 center = intVec;
						ThingPlaceMode mode = ThingPlaceMode.Near;
						Action<Thing, int> placedAction = null;
						Predicate<IntVec3> extraValidator;
						if ((extraValidator = <>9__0) == null)
						{
							extraValidator = (<>9__0 = ((IntVec3 x) => !occupiedRect.Contains(x)));
						}
						if (!GenPlace.TryPlaceThing(thing2, center, map, mode, placedAction, extraValidator, null, 1))
						{
							thing.Destroy(DestroyMode.Vanish);
						}
					}
				}
			}
		}

		// Token: 0x060038BA RID: 14522 RVA: 0x0016C338 File Offset: 0x0016A538
		public static bool WouldWipeAnythingWith(IntVec3 thingPos, Rot4 thingRot, BuildableDef thingDef, Map map, Predicate<Thing> predicate)
		{
			return GenSpawn.WouldWipeAnythingWith(GenAdj.OccupiedRect(thingPos, thingRot, thingDef.Size), thingDef, map, predicate);
		}

		// Token: 0x060038BB RID: 14523 RVA: 0x0016C350 File Offset: 0x0016A550
		public static bool WouldWipeAnythingWith(CellRect cellRect, BuildableDef thingDef, Map map, Predicate<Thing> predicate)
		{
			foreach (IntVec3 c in cellRect)
			{
				foreach (Thing thing in map.thingGrid.ThingsAt(c).ToList<Thing>())
				{
					if (GenSpawn.SpawningWipes(thingDef, thing.def, false) && predicate(thing))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060038BC RID: 14524 RVA: 0x0016C400 File Offset: 0x0016A600
		public static bool SpawningWipes(BuildableDef newEntDef, BuildableDef oldEntDef, bool ignoreDestroyable = false)
		{
			ThingDef thingDef = newEntDef as ThingDef;
			ThingDef thingDef2 = oldEntDef as ThingDef;
			if (thingDef == null || thingDef2 == null)
			{
				return false;
			}
			if (thingDef.category == ThingCategory.Attachment || thingDef.category == ThingCategory.Mote || thingDef.category == ThingCategory.Filth || thingDef.category == ThingCategory.Projectile)
			{
				return false;
			}
			if (!ignoreDestroyable && !thingDef2.destroyable)
			{
				return false;
			}
			if (thingDef.category == ThingCategory.Plant)
			{
				return false;
			}
			if (thingDef.category == ThingCategory.PsychicEmitter)
			{
				return thingDef2.category == ThingCategory.PsychicEmitter;
			}
			if (thingDef2.category == ThingCategory.Filth && thingDef.passability != Traversability.Standable)
			{
				return true;
			}
			if (thingDef2.category == ThingCategory.Item && thingDef.passability == Traversability.Impassable && thingDef.surfaceType == SurfaceType.None)
			{
				return true;
			}
			if (thingDef.EverTransmitsPower && thingDef2.building != null && thingDef2.building.isPowerConduit)
			{
				return true;
			}
			if (thingDef.IsFrame && GenSpawn.SpawningWipes(thingDef.entityDefToBuild, oldEntDef, false))
			{
				return true;
			}
			BuildableDef buildableDef = GenConstruct.BuiltDefOf(thingDef);
			BuildableDef buildableDef2 = GenConstruct.BuiltDefOf(thingDef2);
			if (buildableDef == null || buildableDef2 == null)
			{
				return false;
			}
			ThingDef thingDef3 = thingDef2.entityDefToBuild as ThingDef;
			ThingDef thingDef4 = thingDef.entityDefToBuild as ThingDef;
			if (thingDef2.IsBlueprint)
			{
				if (thingDef.IsBlueprint)
				{
					if (thingDef4 != null && thingDef4.building != null && thingDef4.building.canPlaceOverWall)
					{
						ThingDef thingDef5 = thingDef2.entityDefToBuild as ThingDef;
						if (thingDef5 != null && thingDef5.building != null && thingDef5.building.isPlaceOverableWall)
						{
							return true;
						}
					}
					if (thingDef4 != null && thingDef4.replaceTags.NotNullAndContainsAnyElement(thingDef2.replaceTags))
					{
						return true;
					}
					if (thingDef2.entityDefToBuild is TerrainDef)
					{
						if (thingDef.entityDefToBuild is ThingDef && ((ThingDef)thingDef.entityDefToBuild).coversFloor)
						{
							return true;
						}
						if (thingDef.entityDefToBuild is TerrainDef)
						{
							return true;
						}
					}
				}
				if (thingDef3 != null)
				{
					BuildingProperties building = thingDef3.building;
					if (((building != null) ? new bool?(building.isPowerConduit) : null).GetValueOrDefault() && thingDef4 != null && thingDef4.EverTransmitsPower)
					{
						return true;
					}
				}
				return false;
			}
			if ((thingDef2.IsFrame || thingDef2.IsBlueprint) && thingDef2.entityDefToBuild is TerrainDef)
			{
				ThingDef thingDef6 = buildableDef as ThingDef;
				if (thingDef6 != null && !thingDef6.CoexistsWithFloors)
				{
					return true;
				}
			}
			if (thingDef2 == ThingDefOf.ActiveDropPod || thingDef == ThingDefOf.ActiveDropPod)
			{
				return false;
			}
			if (thingDef.wipesPlants && thingDef.category == ThingCategory.Building && thingDef2.category == ThingCategory.Plant)
			{
				return true;
			}
			if (thingDef.BlocksPlanting(false) && thingDef2.category == ThingCategory.Plant)
			{
				return true;
			}
			if (thingDef.IsEdifice())
			{
				if (thingDef2.category == ThingCategory.PsychicEmitter)
				{
					return true;
				}
				if (!(buildableDef is TerrainDef) && buildableDef2.IsEdifice())
				{
					return true;
				}
			}
			return (thingDef.blocksAltitudes != null && thingDef.blocksAltitudes.Contains(thingDef2.altitudeLayer)) || (thingDef2.blocksAltitudes != null && thingDef2.blocksAltitudes.Contains(thingDef.altitudeLayer));
		}

		// Token: 0x060038BD RID: 14525 RVA: 0x0016C6D8 File Offset: 0x0016A8D8
		public static bool CanSpawnAt(ThingDef thingDef, IntVec3 c, Map map, Rot4? rot = null, bool canWipeEdifices = true)
		{
			Rot4 value = rot.GetValueOrDefault();
			if (rot == null)
			{
				value = thingDef.defaultPlacingRot;
				rot = new Rot4?(value);
			}
			if (!thingDef.CanSpawnAt(c, rot.Value, map))
			{
				return false;
			}
			foreach (IntVec3 c2 in GenAdj.OccupiedRect(c, rot.Value, thingDef.Size))
			{
				if (!c2.InBounds(map))
				{
					return false;
				}
				if (!c.Walkable(map))
				{
					return false;
				}
				if (!canWipeEdifices && map.edificeGrid[c2] != null)
				{
					return false;
				}
				foreach (Thing thing in c.GetThingList(map))
				{
					if (GenSpawn.SpawningWipes(thingDef, thing.def, true) && !thing.def.destroyable)
					{
						return false;
					}
				}
			}
			return (thingDef.category != ThingCategory.Building || GenConstruct.CanBuildOnTerrain(thingDef, c, map, rot.Value, null, null)) && (!thingDef.HasSingleOrMultipleInteractionCells || GenConstruct.InteractionCellStandable(thingDef, c, rot.Value, map, null)) && GenConstruct.NotBlockingAnyInteractionCells(thingDef, c, rot.Value, map, null);
		}

		// Token: 0x040024ED RID: 9453
		private static readonly List<Thing> leavings = new List<Thing>();
	}
}
