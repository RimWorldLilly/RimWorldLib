using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022F2 RID: 8946
	public class Gravship : WorldObject, IIncidentTarget, ILoadReferenceable
	{
		// Token: 0x17002680 RID: 9856
		// (get) Token: 0x0600DCF7 RID: 56567 RVA: 0x0052406F File Offset: 0x0052226F
		private Vector3 Start
		{
			get
			{
				return Find.WorldGrid.GetTileCenter(this.initialTile);
			}
		}

		// Token: 0x17002681 RID: 9857
		// (get) Token: 0x0600DCF8 RID: 56568 RVA: 0x00524081 File Offset: 0x00522281
		private Vector3 End
		{
			get
			{
				return Find.WorldGrid.GetTileCenter(this.destinationTile);
			}
		}

		// Token: 0x17002682 RID: 9858
		// (get) Token: 0x0600DCF9 RID: 56569 RVA: 0x00524093 File Offset: 0x00522293
		public override Vector3 DrawPos
		{
			get
			{
				return Vector3.Slerp(this.Start, this.End, this.traveledPct);
			}
		}

		// Token: 0x17002683 RID: 9859
		// (get) Token: 0x0600DCFA RID: 56570 RVA: 0x005240AC File Offset: 0x005222AC
		public IEnumerable<Pawn> Pawns
		{
			get
			{
				return this.pawns.Keys;
			}
		}

		// Token: 0x17002684 RID: 9860
		// (get) Token: 0x0600DCFB RID: 56571 RVA: 0x005240B9 File Offset: 0x005222B9
		public IEnumerable<Thing> Things
		{
			get
			{
				return this.things.Keys;
			}
		}

		// Token: 0x17002685 RID: 9861
		// (get) Token: 0x0600DCFC RID: 56572 RVA: 0x005240C6 File Offset: 0x005222C6
		public override string Label
		{
			get
			{
				if (!this.engine.nameHidden)
				{
					return this.engine.RenamableLabel;
				}
				return base.Label;
			}
		}

		// Token: 0x17002686 RID: 9862
		// (get) Token: 0x0600DCFD RID: 56573 RVA: 0x005240E8 File Offset: 0x005222E8
		public override Material Material
		{
			get
			{
				Material result;
				if ((result = this.cachedMat) == null)
				{
					result = (this.cachedMat = MaterialPool.MatFrom(this.def.texture, ShaderDatabase.WorldOverlayTransparentLit, base.Faction.Color, 3550));
				}
				return result;
			}
		}

		// Token: 0x17002687 RID: 9863
		// (get) Token: 0x0600DCFE RID: 56574 RVA: 0x0052412D File Offset: 0x0052232D
		public override Color ExpandingIconColor
		{
			get
			{
				Faction faction = base.Faction;
				if (faction == null)
				{
					return Color.white;
				}
				return faction.Color;
			}
		}

		// Token: 0x17002688 RID: 9864
		// (get) Token: 0x0600DCFF RID: 56575 RVA: 0x00524144 File Offset: 0x00522344
		// (set) Token: 0x0600DD00 RID: 56576 RVA: 0x0052414C File Offset: 0x0052234C
		public Rot4 Rotation
		{
			get
			{
				return this.rotation;
			}
			set
			{
				this.rotation = value;
			}
		}

		// Token: 0x17002689 RID: 9865
		// (get) Token: 0x0600DD01 RID: 56577 RVA: 0x00524158 File Offset: 0x00522358
		private float TraveledPctStepPerTick
		{
			get
			{
				Vector3 start = this.Start;
				Vector3 end = this.End;
				if (start == end)
				{
					return 1f;
				}
				float num = GenMath.SphericalDistance(start.normalized, end.normalized);
				if (num == 0f)
				{
					return 1f;
				}
				return 0.00025f / num;
			}
		}

		// Token: 0x0600DD02 RID: 56578 RVA: 0x005241AC File Offset: 0x005223AC
		private Gravship()
		{
			this.things = new Dictionary<Thing, PositionData>();
			this.pawns = new Dictionary<Pawn, PositionData>();
			this.powerOn = new Dictionary<Thing, bool>();
			this.connectParents = new Dictionary<Thing, Thing>();
			this.roofs = new Dictionary<IntVec3, RoofDef>();
			this.foundations = new Dictionary<IntVec3, TerrainDef>();
			this.terrains = new Dictionary<IntVec3, TerrainDef>();
			this.terrainColors = new Dictionary<IntVec3, ColorDef>();
			this.terrainDesignations = new Dictionary<IntVec3, Designation>();
			this.gases = new Dictionary<IntVec3, uint>();
			this.areas = new MoveableAreas();
			this.bounds = CellRect.Empty;
			this.occupiedRects = new List<CellRect>();
			this.roomTemperatures = new List<RoomTemperatureVacuum>();
			this.thrusters = new Dictionary<Thing, PositionData>();
			this.exteriorDoors = new Dictionary<Thing, PositionData>();
			this.gravFieldExtenderPositions = new List<IntVec3>();
		}

		// Token: 0x0600DD03 RID: 56579 RVA: 0x005242F4 File Offset: 0x005224F4
		public Gravship(Building_GravEngine engine) : this()
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.engine = engine;
			Map map = engine.Map;
			this.originalPosition = engine.Position;
			engine.ForceSubstructureDirty();
			HashSet<IntVec3> validSubstructure = engine.ValidSubstructure;
			this.storyState = new StoryState(this);
			map.storyState.CopyTo(this.storyState);
			this.autoSlaughterConfigs = map.autoSlaughterManager.configs;
			this.TransferZones(map, this.originalPosition, validSubstructure);
			HashSet<IntVec3> occupiedCells;
			this.CopyCellContents(map, this.originalPosition, validSubstructure, out occupiedCells);
			this.CopyAreas(map, this.originalPosition, validSubstructure);
			this.CopyStorageGroups(map);
			this.UpdateBoundingBoxes(occupiedCells);
			this.DetermineLaunchDirection();
			this.CheckAffectedGoodwill();
			this.engineToCenter = CellRect.FromCellList(engine.ValidSubstructure).CenterVector3 - this.originalPosition.ToVector3();
		}

		// Token: 0x0600DD04 RID: 56580 RVA: 0x005243D4 File Offset: 0x005225D4
		public override void ExposeData()
		{
			base.ExposeData();
			Dictionary<IntVec3, RoofDef> dictionary = this.roofs;
			if (dictionary != null)
			{
				dictionary.RemoveAll((KeyValuePair<IntVec3, RoofDef> x) => x.Value == null);
			}
			Dictionary<IntVec3, TerrainDef> dictionary2 = this.foundations;
			if (dictionary2 != null)
			{
				dictionary2.RemoveAll((KeyValuePair<IntVec3, TerrainDef> x) => x.Value == null);
			}
			Dictionary<IntVec3, TerrainDef> dictionary3 = this.terrains;
			if (dictionary3 != null)
			{
				dictionary3.RemoveAll((KeyValuePair<IntVec3, TerrainDef> x) => x.Value == null);
			}
			Dictionary<IntVec3, ColorDef> dictionary4 = this.terrainColors;
			if (dictionary4 != null)
			{
				dictionary4.RemoveAll((KeyValuePair<IntVec3, ColorDef> x) => x.Value == null);
			}
			Dictionary<IntVec3, Designation> dictionary5 = this.terrainDesignations;
			if (dictionary5 != null)
			{
				dictionary5.RemoveAll((KeyValuePair<IntVec3, Designation> x) => x.Value == null);
			}
			Scribe_References.Look<Building_GravEngine>(ref this.engine, "engine", false);
			Scribe_Collections.Look<Thing, PositionData>(ref this.things, "things", LookMode.Deep, LookMode.Deep);
			Scribe_Collections.Look<Pawn, PositionData>(ref this.pawns, "pawns", LookMode.Deep, LookMode.Deep);
			Scribe_Collections.Look<Thing, bool>(ref this.powerOn, "powerOn", LookMode.Reference, LookMode.Value, ref this.tmpPoweredThings, ref this.tmpPoweredThingsOn, true, false, false);
			Scribe_Collections.Look<Thing, Thing>(ref this.connectParents, "connectParents", LookMode.Reference, LookMode.Reference, ref this.tmpConnectedThings, ref this.tmpConnectedParents, true, false, false);
			Scribe_Collections.Look<IntVec3, RoofDef>(ref this.roofs, "roofs", LookMode.Value, LookMode.Def);
			Scribe_Collections.Look<IntVec3, TerrainDef>(ref this.foundations, "foundations", LookMode.Value, LookMode.Def);
			Scribe_Collections.Look<IntVec3, TerrainDef>(ref this.terrains, "terrains", LookMode.Value, LookMode.Def);
			Scribe_Collections.Look<IntVec3, ColorDef>(ref this.terrainColors, "terrainColors", LookMode.Value, LookMode.Def);
			Scribe_Collections.Look<IntVec3, uint>(ref this.gases, "gases", LookMode.Value, LookMode.Value);
			Scribe_Collections.Look<IntVec3, Designation>(ref this.terrainDesignations, "terrainDesignations", LookMode.Value, LookMode.Deep);
			Scribe_Deep.Look<MoveableAreas>(ref this.areas, "areas", Array.Empty<object>());
			Scribe_Values.Look<Vector3>(ref this.engineToCenter, "engineToCenter", default(Vector3), false);
			Scribe_Values.Look<Rot4>(ref this.rotation, "rotation", default(Rot4), false);
			Scribe_Values.Look<CellRect>(ref this.bounds, "bounds", default(CellRect), false);
			Scribe_Collections.Look<CellRect>(ref this.occupiedRects, "occupiedRects", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<RoomTemperatureVacuum>(ref this.roomTemperatures, "roomTemperatures", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Thing, PositionData>(ref this.thrusters, "thrusters", LookMode.Reference, LookMode.Deep, ref this.tmpThrusters, ref this.tmpThrusterPositionData, true, false, false);
			Scribe_Collections.Look<Thing, PositionData>(ref this.exteriorDoors, "exteriorDoors", LookMode.Reference, LookMode.Deep, ref this.tmpExteriorDoors, ref this.tmpExteriorDoorPositionData, true, false, false);
			Scribe_Values.Look<PlanetTile>(ref this.destinationTile, "destinationTile", default(PlanetTile), false);
			Scribe_Values.Look<PlanetTile>(ref this.initialTile, "initialTile", default(PlanetTile), false);
			Scribe_Values.Look<float>(ref this.traveledPct, "traveledPct", 0f, false);
			Scribe_Values.Look<bool>(ref this.arrived, "arrived", false, false);
			Scribe_Deep.Look<StoryState>(ref this.storyState, "storyState", new object[]
			{
				this
			});
			Scribe_Collections.Look<AutoSlaughterConfig>(ref this.autoSlaughterConfigs, "autoSlaughterConfigs", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<IntVec3>(ref this.launchDirection, "launchDirection", default(IntVec3), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (base.Spawned)
				{
					LongEventHandler.ExecuteWhenFinished(delegate
					{
						CameraJumper.TryJump(this, CameraJumper.MovementMode.Pan);
					});
				}
				Current.Game.Gravship = this;
				if (this.connectParents == null)
				{
					this.connectParents = new Dictionary<Thing, Thing>();
				}
			}
		}

		// Token: 0x0600DD05 RID: 56581 RVA: 0x00524776 File Offset: 0x00522976
		public bool ContainsThing(Thing thing)
		{
			return this.things.ContainsKey(thing);
		}

		// Token: 0x0600DD06 RID: 56582 RVA: 0x00524784 File Offset: 0x00522984
		public bool ContainsPawn(Pawn pawn)
		{
			return this.pawns.ContainsKey(pawn);
		}

		// Token: 0x0600DD07 RID: 56583 RVA: 0x00524794 File Offset: 0x00522994
		private void DetermineLaunchDirection()
		{
			if (this.launchDirection == IntVec3.Zero)
			{
				this.launchDirection = this.pilotConsole.Rotation.AsIntVec3;
			}
		}

		// Token: 0x0600DD08 RID: 56584 RVA: 0x005247CC File Offset: 0x005229CC
		private void TransferZones(Map oldMap, IntVec3 origin, HashSet<IntVec3> engineFloors)
		{
			for (int i = oldMap.zoneManager.AllZones.Count - 1; i >= 0; i--)
			{
				Zone zone = oldMap.zoneManager.AllZones[i];
				Zone_Stockpile zone_Stockpile = zone as Zone_Stockpile;
				if (zone_Stockpile != null)
				{
					MoveableStockpile moveableStockpile = null;
					foreach (IntVec3 intVec in zone_Stockpile.Cells)
					{
						if (engineFloors.Contains(intVec))
						{
							if (moveableStockpile == null)
							{
								moveableStockpile = new MoveableStockpile(this, zone_Stockpile);
							}
							moveableStockpile.Add(intVec - origin);
						}
					}
					if (moveableStockpile != null)
					{
						this.areas.stockpiles.Add(moveableStockpile);
						zone.Delete();
					}
				}
				else
				{
					Zone_Growing zone_Growing = zone as Zone_Growing;
					if (zone_Growing != null)
					{
						MoveableGrowZone moveableGrowZone = null;
						foreach (IntVec3 intVec2 in zone_Growing.Cells)
						{
							if (engineFloors.Contains(intVec2))
							{
								if (moveableGrowZone == null)
								{
									moveableGrowZone = new MoveableGrowZone(this, zone_Growing);
								}
								moveableGrowZone.Add(intVec2 - origin);
							}
						}
						if (moveableGrowZone != null)
						{
							this.areas.growZones.Add(moveableGrowZone);
							zone.Delete();
						}
					}
				}
			}
		}

		// Token: 0x0600DD09 RID: 56585 RVA: 0x00524930 File Offset: 0x00522B30
		private void CopyAreas(Map oldMap, IntVec3 origin, HashSet<IntVec3> engineFloors)
		{
			using (List<Area>.Enumerator enumerator = oldMap.areaManager.AllAreas.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Area area = enumerator.Current;
					if (area is Area_Home)
					{
						this.areas.homeArea = new MoveableArea_Allowed(this, area);
						foreach (IntVec3 intVec in area.ActiveCells)
						{
							if (engineFloors.Contains(intVec))
							{
								this.areas.homeArea.Add(intVec - origin);
							}
						}
						this.areas.homeArea.assignedPawns.AddRange(this.Pawns.Where(delegate(Pawn x)
						{
							Pawn_PlayerSettings playerSettings4 = x.playerSettings;
							return ((playerSettings4 != null) ? playerSettings4.AreaRestrictionInPawnCurrentMap : null) == area;
						}));
						foreach (Pawn pawn in oldMap.mapPawns.AllPawnsUnspawned)
						{
							if (pawn.SpawnedParentOrMe != null && this.things.ContainsKey(pawn.SpawnedParentOrMe))
							{
								Pawn_PlayerSettings playerSettings = pawn.playerSettings;
								if (((playerSettings != null) ? playerSettings.AreaRestrictionInPawnCurrentMap : null) == area)
								{
									this.areas.homeArea.assignedPawns.Add(pawn);
								}
							}
						}
						using (List<Pawn>.Enumerator enumerator3 = this.areas.homeArea.assignedPawns.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								Pawn pawn2 = enumerator3.Current;
								Pawn_PlayerSettings playerSettings2 = pawn2.playerSettings;
								if (playerSettings2 != null)
								{
									playerSettings2.Notify_MapRemoved(pawn2.MapHeld);
								}
							}
							continue;
						}
					}
					Area_Allowed area_Allowed = area as Area_Allowed;
					if (area_Allowed != null)
					{
						MoveableArea_Allowed moveableArea_Allowed = new MoveableArea_Allowed(this, area_Allowed);
						foreach (IntVec3 intVec2 in area.ActiveCells)
						{
							if (engineFloors.Contains(intVec2))
							{
								moveableArea_Allowed.Add(intVec2 - origin);
							}
						}
						moveableArea_Allowed.assignedPawns.AddRange(this.Pawns.Where(delegate(Pawn x)
						{
							Pawn_PlayerSettings playerSettings4 = x.playerSettings;
							return ((playerSettings4 != null) ? playerSettings4.AreaRestrictionInPawnCurrentMap : null) == area;
						}));
						this.areas.allowedAreas.Add(moveableArea_Allowed);
						using (List<Pawn>.Enumerator enumerator3 = moveableArea_Allowed.assignedPawns.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								Pawn pawn3 = enumerator3.Current;
								Pawn_PlayerSettings playerSettings3 = pawn3.playerSettings;
								if (playerSettings3 != null)
								{
									playerSettings3.Notify_MapRemoved(pawn3.Map);
								}
							}
							continue;
						}
					}
					MoveableArea moveableArea = null;
					foreach (IntVec3 intVec3 in area.ActiveCells)
					{
						if (engineFloors.Contains(intVec3))
						{
							if (moveableArea == null)
							{
								moveableArea = new MoveableArea(this, area.Label, area.RenamableLabel, area.Color, area.ID);
							}
							moveableArea.Add(intVec3 - origin);
						}
					}
					if (moveableArea != null)
					{
						if (area is Area_NoRoof)
						{
							this.areas.noRoofArea = moveableArea;
						}
						else if (area is Area_BuildRoof)
						{
							this.areas.buildRoofArea = moveableArea;
						}
						else if (area is Area_SnowOrSandClear)
						{
							this.areas.snowClearArea = moveableArea;
						}
						else if (ModsConfig.BiotechActive && area is Area_PollutionClear)
						{
							this.areas.pollutionClearArea = moveableArea;
						}
					}
				}
			}
		}

		// Token: 0x0600DD0A RID: 56586 RVA: 0x00524D90 File Offset: 0x00522F90
		private void CopyStorageGroups(Map oldMap)
		{
			for (int i = oldMap.storageGroups.StorageGroupsForReading.Count - 1; i >= 0; i--)
			{
				StorageGroup storageGroup = oldMap.storageGroups.StorageGroupsForReading[i];
				MoveableStorageGroup moveableStorageGroup = null;
				foreach (IStorageGroupMember storageGroupMember in storageGroup.members)
				{
					Thing thing = storageGroupMember as Thing;
					if (thing != null && this.things.ContainsKey(thing))
					{
						if (moveableStorageGroup == null)
						{
							moveableStorageGroup = new MoveableStorageGroup(storageGroup);
						}
						moveableStorageGroup.members.Add(storageGroupMember);
					}
				}
				if (moveableStorageGroup != null)
				{
					if (moveableStorageGroup.members.Any<IStorageGroupMember>())
					{
						this.areas.storageGroups.Add(moveableStorageGroup);
					}
					foreach (IStorageGroupMember member in moveableStorageGroup.members)
					{
						member.SetStorageGroup(null, true);
					}
				}
			}
		}

		// Token: 0x0600DD0B RID: 56587 RVA: 0x00524EAC File Offset: 0x005230AC
		private void CopyCellContents(Map oldMap, IntVec3 origin, HashSet<IntVec3> engineFloors, out HashSet<IntVec3> outOccupiedCells)
		{
			HashSet<Room> hashSet = new HashSet<Room>();
			outOccupiedCells = new HashSet<IntVec3>();
			foreach (CompGravshipFacility compGravshipFacility in this.engine.GravshipComponents)
			{
				this.AddThing(compGravshipFacility.parent, compGravshipFacility.parent.Position - origin);
			}
			this.AddThing(this.engine, this.engine.Position - origin);
			foreach (IntVec3 intVec in engineFloors)
			{
				IntVec3 intVec2 = intVec - origin;
				List<Thing> list = oldMap.thingGrid.ThingsListAt(intVec);
				for (int i = list.Count - 1; i >= 0; i--)
				{
					Thing thing = list[i];
					if (Gravship.ShouldBringOnGravship(thing, intVec))
					{
						Building building = thing as Building;
						if (building == null || !building.def.building.isAttachment)
						{
							foreach (Thing thing2 in GenConstruct.GetAttachedBuildings(thing))
							{
								this.AddThing(thing2, intVec2 + thing2.Rotation.Opposite.FacingCell);
							}
							this.AddThing(thing, intVec2);
						}
					}
				}
				outOccupiedCells.Add(intVec2);
				this.foundations[intVec2] = oldMap.terrainGrid.FoundationAt(intVec);
				this.terrains[intVec2] = oldMap.terrainGrid.TerrainAt(intVec);
				this.terrainColors[intVec2] = oldMap.terrainGrid.ColorAt(intVec);
				this.roofs[intVec2] = oldMap.roofGrid.RoofAt(intVec);
				foreach (Designation designation in oldMap.designationManager.AllDesignationsAt(intVec))
				{
					if (designation.def == DesignationDefOf.RemoveFloor || designation.def == DesignationDefOf.PaintFloor || designation.def == DesignationDefOf.RemovePaintFloor)
					{
						this.terrainDesignations[intVec2] = designation;
						oldMap.designationManager.RemoveDesignation(designation);
					}
				}
				this.gases[intVec2] = oldMap.gasGrid.GetDirect(intVec);
				Room room = intVec.GetRoom(oldMap);
				if (room != null && hashSet.Add(room) && !room.UsesOutdoorTemperature)
				{
					this.roomTemperatures.Add(new RoomTemperatureVacuum
					{
						roomCell = intVec2,
						temperature = room.Temperature,
						vacuum = room.Vacuum
					});
				}
			}
		}

		// Token: 0x0600DD0C RID: 56588 RVA: 0x005251FC File Offset: 0x005233FC
		private static bool ShouldBringOnGravship(Thing thing, IntVec3 cell)
		{
			return !(thing is Mote) && !(thing is Skyfaller) && thing.def.bringAlongOnGravship && !(cell != thing.Position) && (thing.def.category != ThingCategory.Ethereal || thing is Blueprint || thing is Frame);
		}

		// Token: 0x0600DD0D RID: 56589 RVA: 0x00525260 File Offset: 0x00523460
		private void AddThing(Thing thing, IntVec3 offset)
		{
			if (thing is Mote || thing is Skyfaller || !thing.def.bringAlongOnGravship)
			{
				return;
			}
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				if (this.pawns.ContainsKey(pawn))
				{
					return;
				}
				this.pawns.Add(pawn, new PositionData(offset, pawn.Rotation, pawn.Drafted));
				Thing thing2;
				if (pawn.carryTracker.CarriedThing != null && pawn.carryTracker.TryDropCarriedThing(pawn.Position, ThingPlaceMode.Near, out thing2, null) && !thing2.Destroyed)
				{
					this.AddThing(thing2, offset);
				}
			}
			else
			{
				if (this.things.ContainsKey(thing))
				{
					return;
				}
				if (!this.engine.OnValidSubstructure(thing))
				{
					return;
				}
				this.things.Add(thing, new PositionData(offset, thing.Rotation, false));
			}
			CompPowerTrader compPowerTrader;
			if (thing.TryGetComp(out compPowerTrader))
			{
				this.powerOn.Add(thing, compPowerTrader.PowerOn);
			}
			CompPower compPower;
			if (thing.TryGetComp(out compPower))
			{
				CompPower connectParent = compPower.connectParent;
				ThingWithComps thingWithComps = (connectParent != null) ? connectParent.parent : null;
				if (thingWithComps != null && this.engine.OnValidSubstructure(thingWithComps))
				{
					this.connectParents.Add(thing, thingWithComps);
				}
			}
			CompGravshipThruster compGravshipThruster;
			if (thing.TryGetComp(out compGravshipThruster) && compGravshipThruster.CanBeActive)
			{
				this.thrusters.Add(thing, new PositionData(offset, thing.Rotation, false));
				this.launchDirection += thing.Rotation.AsIntVec3 * compGravshipThruster.Props.directionInfluence;
			}
			Building_Door building_Door = thing as Building_Door;
			if (building_Door != null)
			{
				foreach (IntVec3 cell in GenAdj.CellsAdjacentCardinal(building_Door))
				{
					if (!this.engine.ValidSubstructureAt(cell))
					{
						this.exteriorDoors.Add(thing, new PositionData(offset, thing.Rotation, false));
						break;
					}
				}
			}
			if (thing.def == ThingDefOf.GravFieldExtender)
			{
				this.gravFieldExtenderPositions.Add(offset);
			}
			if (thing.def == ThingDefOf.PilotConsole)
			{
				this.pilotConsole = (Building)thing;
			}
		}

		// Token: 0x0600DD0E RID: 56590 RVA: 0x00525490 File Offset: 0x00523690
		private void UpdateBoundingBoxes(HashSet<IntVec3> occupiedCells)
		{
			HashSet<IntVec3> hashSet = new HashSet<IntVec3>();
			this.bounds = CellRect.FromCellList(occupiedCells);
			for (int i = this.bounds.minX; i <= this.bounds.maxX; i++)
			{
				for (int j = this.bounds.minZ; j <= this.bounds.maxZ; j++)
				{
					IntVec3 item = new IntVec3(i, 0, j);
					if (occupiedCells.Contains(item) && !hashSet.Contains(item))
					{
						int num = 1;
						int num2 = 1;
						int num3 = i + 1;
						while (num3 <= this.bounds.maxX && occupiedCells.Contains(new IntVec3(num3, 0, j)))
						{
							num++;
							num3++;
						}
						bool flag = true;
						while (j + num2 <= this.bounds.maxZ && flag)
						{
							for (int k = 0; k < num; k++)
							{
								if (!occupiedCells.Contains(new IntVec3(i + k, 0, j + num2)) || hashSet.Contains(new IntVec3(i + 1, 0, j + num2)))
								{
									flag = false;
									break;
								}
							}
							if (flag)
							{
								num2++;
							}
						}
						for (int l = 0; l < num; l++)
						{
							for (int m = 0; m < num2; m++)
							{
								hashSet.Add(new IntVec3(i + l, 0, j + m));
							}
						}
						this.occupiedRects.Add(new CellRect(i, j, num, num2));
					}
				}
			}
		}

		// Token: 0x0600DD0F RID: 56591 RVA: 0x0052560C File Offset: 0x0052380C
		private void CheckAffectedGoodwill()
		{
			Gravship.tmpFactionMembersKidnappedCount.Clear();
			foreach (KeyValuePair<Pawn, PositionData> keyValuePair in this.pawns)
			{
				Pawn pawn;
				PositionData positionData;
				keyValuePair.Deconstruct(out pawn, out positionData);
				Pawn pawn2 = pawn;
				if (GravshipUtility.ConsideredKidnapped(this.engine, pawn2))
				{
					Gravship.tmpFactionMembersKidnappedCount.TryAdd(pawn2.Faction, 0);
					Dictionary<Faction, int> dictionary = Gravship.tmpFactionMembersKidnappedCount;
					Faction faction = pawn2.Faction;
					int num = dictionary[faction];
					dictionary[faction] = num + 1;
				}
			}
			foreach (KeyValuePair<Faction, int> keyValuePair2 in Gravship.tmpFactionMembersKidnappedCount)
			{
				Faction faction;
				int num;
				keyValuePair2.Deconstruct(out faction, out num);
				Faction faction2 = faction;
				int num2 = num;
				faction2.TryAffectGoodwillWith(Faction.OfPlayer, -10 * num2, true, true, HistoryEventDefOf.PawnKidnappedOnGravShip, null);
			}
		}

		// Token: 0x0600DD10 RID: 56592 RVA: 0x00525720 File Offset: 0x00523920
		public override void PostAdd()
		{
			base.PostAdd();
			this.initialTile = base.Tile;
		}

		// Token: 0x0600DD11 RID: 56593 RVA: 0x00525734 File Offset: 0x00523934
		protected override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			this.traveledPct += this.TraveledPctStepPerTick * (float)delta;
			if (this.traveledPct >= 1f)
			{
				this.traveledPct = 1f;
				MapParent mapParent = Find.WorldObjects.MapParentAt(this.destinationTile);
				if (((mapParent != null) ? mapParent.Map : null) != null)
				{
					GravshipUtility.ArriveExistingMap(this);
					return;
				}
				LongEventHandler.QueueLongEvent(delegate()
				{
					GravshipUtility.ArriveNewMap(this);
				}, "GeneratingMap", false, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap), true, false, null);
			}
		}

		// Token: 0x1700268A RID: 9866
		// (get) Token: 0x0600DD12 RID: 56594 RVA: 0x005257C1 File Offset: 0x005239C1
		public int ConstantRandSeed
		{
			get
			{
				return HashCode.Combine<int, int>(this.ID, 18948292);
			}
		}

		// Token: 0x1700268B RID: 9867
		// (get) Token: 0x0600DD13 RID: 56595 RVA: 0x005257D3 File Offset: 0x005239D3
		public StoryState StoryState
		{
			get
			{
				return this.storyState;
			}
		}

		// Token: 0x1700268C RID: 9868
		// (get) Token: 0x0600DD14 RID: 56596 RVA: 0x005257DB File Offset: 0x005239DB
		public GameConditionManager GameConditionManager
		{
			get
			{
				Log.ErrorOnce("Attempted to retrieve condition manager directly from gravship", 13291050);
				return null;
			}
		}

		// Token: 0x1700268D RID: 9869
		// (get) Token: 0x0600DD15 RID: 56597 RVA: 0x005257F0 File Offset: 0x005239F0
		public float PlayerWealthForStoryteller
		{
			get
			{
				float num = 0f;
				foreach (KeyValuePair<Pawn, PositionData> keyValuePair in this.pawns)
				{
					Pawn pawn;
					PositionData positionData;
					keyValuePair.Deconstruct(out pawn, out positionData);
					Pawn pawn2 = pawn;
					num += WealthWatcher.GetEquipmentApparelAndInventoryWealth(pawn2);
					if (pawn2.Faction == Faction.OfPlayer)
					{
						float num2 = pawn2.MarketValue;
						if (pawn2.IsSlave)
						{
							num2 *= 0.75f;
						}
						num += num2;
					}
				}
				return num * 0.7f;
			}
		}

		// Token: 0x1700268E RID: 9870
		// (get) Token: 0x0600DD16 RID: 56598 RVA: 0x0052588C File Offset: 0x00523A8C
		public IEnumerable<Pawn> PlayerPawnsForStoryteller
		{
			get
			{
				return from x in this.Pawns
				where x.Faction == Faction.OfPlayer
				select x;
			}
		}

		// Token: 0x1700268F RID: 9871
		// (get) Token: 0x0600DD17 RID: 56599 RVA: 0x005258B8 File Offset: 0x00523AB8
		public FloatRange IncidentPointsRandomFactorRange
		{
			get
			{
				return StorytellerUtility.GravshipPointsRandomFactorRange;
			}
		}

		// Token: 0x17002690 RID: 9872
		// (get) Token: 0x0600DD18 RID: 56600 RVA: 0x005258BF File Offset: 0x00523ABF
		public CellRect Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		// Token: 0x17002691 RID: 9873
		// (get) Token: 0x0600DD19 RID: 56601 RVA: 0x005258C7 File Offset: 0x00523AC7
		public Building_GravEngine Engine
		{
			get
			{
				return this.engine;
			}
		}

		// Token: 0x17002692 RID: 9874
		// (get) Token: 0x0600DD1A RID: 56602 RVA: 0x005258CF File Offset: 0x00523ACF
		public Building PilotConsole
		{
			get
			{
				return this.pilotConsole;
			}
		}

		// Token: 0x17002693 RID: 9875
		// (get) Token: 0x0600DD1B RID: 56603 RVA: 0x005258D7 File Offset: 0x00523AD7
		public Dictionary<Thing, bool> PoweredOn
		{
			get
			{
				return this.powerOn;
			}
		}

		// Token: 0x17002694 RID: 9876
		// (get) Token: 0x0600DD1C RID: 56604 RVA: 0x005258DF File Offset: 0x00523ADF
		public Dictionary<Thing, Thing> ConnectParents
		{
			get
			{
				return this.connectParents;
			}
		}

		// Token: 0x17002695 RID: 9877
		// (get) Token: 0x0600DD1D RID: 56605 RVA: 0x005258E7 File Offset: 0x00523AE7
		public IEnumerable<IntVec3> GravFieldExtenderPositions
		{
			get
			{
				return this.GetRotatedValues(this.gravFieldExtenderPositions);
			}
		}

		// Token: 0x17002696 RID: 9878
		// (get) Token: 0x0600DD1E RID: 56606 RVA: 0x005258F5 File Offset: 0x00523AF5
		public Dictionary<Thing, PositionData.Data> ThingPlacements
		{
			get
			{
				return this.GetPlacementValues<Thing>(this.things, ref this.tmpThingPlacements, ref this.tmpThingsRot);
			}
		}

		// Token: 0x17002697 RID: 9879
		// (get) Token: 0x0600DD1F RID: 56607 RVA: 0x0052590F File Offset: 0x00523B0F
		public Dictionary<Pawn, PositionData.Data> PawnPlacements
		{
			get
			{
				return this.GetPlacementValues<Pawn>(this.pawns, ref this.tmpPawns, ref this.tmpPawnsRot);
			}
		}

		// Token: 0x17002698 RID: 9880
		// (get) Token: 0x0600DD20 RID: 56608 RVA: 0x00525929 File Offset: 0x00523B29
		public Dictionary<Thing, PositionData>.KeyCollection Thrusters
		{
			get
			{
				return this.thrusters.Keys;
			}
		}

		// Token: 0x17002699 RID: 9881
		// (get) Token: 0x0600DD21 RID: 56609 RVA: 0x00525936 File Offset: 0x00523B36
		public Dictionary<Thing, PositionData.Data> ThrusterPlacements
		{
			get
			{
				return this.GetPlacementValues<Thing>(this.thrusters, ref this.tmpThrusterPlacements, ref this.tmpThrusterRot);
			}
		}

		// Token: 0x1700269A RID: 9882
		// (get) Token: 0x0600DD22 RID: 56610 RVA: 0x00525950 File Offset: 0x00523B50
		public Dictionary<Thing, PositionData.Data> ExteriorDoorPlacements
		{
			get
			{
				return this.GetPlacementValues<Thing>(this.exteriorDoors, ref this.tmpExteriorDoorPlacements, ref this.tmpExteriorDoorRot);
			}
		}

		// Token: 0x1700269B RID: 9883
		// (get) Token: 0x0600DD23 RID: 56611 RVA: 0x0052596A File Offset: 0x00523B6A
		public Dictionary<IntVec3, TerrainDef> Foundations
		{
			get
			{
				return this.GetRotatedValues<TerrainDef>(this.foundations, ref this.tmpFoundations, ref this.tmpFoundationRot);
			}
		}

		// Token: 0x1700269C RID: 9884
		// (get) Token: 0x0600DD24 RID: 56612 RVA: 0x00525984 File Offset: 0x00523B84
		public Dictionary<IntVec3, TerrainDef> Terrains
		{
			get
			{
				return this.GetRotatedValues<TerrainDef>(this.terrains, ref this.tmpTerrains, ref this.tmpTerrainRot);
			}
		}

		// Token: 0x1700269D RID: 9885
		// (get) Token: 0x0600DD25 RID: 56613 RVA: 0x0052599E File Offset: 0x00523B9E
		public IEnumerable<ValueTuple<IntVec3, RoofDef>> Roofs
		{
			get
			{
				return this.GetRotatedValues<RoofDef>(this.roofs);
			}
		}

		// Token: 0x1700269E RID: 9886
		// (get) Token: 0x0600DD26 RID: 56614 RVA: 0x005259AC File Offset: 0x00523BAC
		public IEnumerable<ValueTuple<IntVec3, ColorDef>> TerrainColors
		{
			get
			{
				return this.GetRotatedValues<ColorDef>(this.terrainColors);
			}
		}

		// Token: 0x1700269F RID: 9887
		// (get) Token: 0x0600DD27 RID: 56615 RVA: 0x005259BA File Offset: 0x00523BBA
		public IEnumerable<ValueTuple<IntVec3, Designation>> TerrainDesignations
		{
			get
			{
				return this.GetRotatedValues<Designation>(this.terrainDesignations);
			}
		}

		// Token: 0x170026A0 RID: 9888
		// (get) Token: 0x0600DD28 RID: 56616 RVA: 0x005259C8 File Offset: 0x00523BC8
		public IEnumerable<ValueTuple<IntVec3, uint>> Gases
		{
			get
			{
				return this.GetRotatedValues<uint>(this.gases);
			}
		}

		// Token: 0x170026A1 RID: 9889
		// (get) Token: 0x0600DD29 RID: 56617 RVA: 0x005259D6 File Offset: 0x00523BD6
		public IEnumerable<CellRect> OccupiedRects
		{
			get
			{
				Gravship.<get_OccupiedRects>d__138 <get_OccupiedRects>d__ = new Gravship.<get_OccupiedRects>d__138(-2);
				<get_OccupiedRects>d__.<>4__this = this;
				return <get_OccupiedRects>d__;
			}
		}

		// Token: 0x170026A2 RID: 9890
		// (get) Token: 0x0600DD2A RID: 56618 RVA: 0x005259E6 File Offset: 0x00523BE6
		public IEnumerable<RoomTemperatureVacuum.Data> RoomTemperatures
		{
			get
			{
				Gravship.<get_RoomTemperatures>d__140 <get_RoomTemperatures>d__ = new Gravship.<get_RoomTemperatures>d__140(-2);
				<get_RoomTemperatures>d__.<>4__this = this;
				return <get_RoomTemperatures>d__;
			}
		}

		// Token: 0x0600DD2B RID: 56619 RVA: 0x005259F8 File Offset: 0x00523BF8
		private Dictionary<T, PositionData.Data> GetPlacementValues<T>(Dictionary<T, PositionData> dictionary, ref Dictionary<T, PositionData.Data> working, ref Rot4 prevRotation) where T : Thing
		{
			if (working != null && prevRotation == this.rotation)
			{
				return working;
			}
			if (working == null)
			{
				working = new Dictionary<T, PositionData.Data>(dictionary.Count);
			}
			working.Clear();
			foreach (KeyValuePair<T, PositionData> keyValuePair in dictionary)
			{
				T t;
				PositionData positionData;
				keyValuePair.Deconstruct(out t, out positionData);
				T t2 = t;
				PositionData positionData2 = positionData;
				IntVec3 adjustedLocalPosition = PrefabUtility.GetAdjustedLocalPosition(positionData2.position, this.rotation);
				IntVec2 size = t2.def.size;
				bool flag = true;
				if (!t2.def.rotatable && size.x == size.z)
				{
					GenAdj.AdjustForRotation(ref adjustedLocalPosition, ref size, t2.def.defaultPlacingRot, this.rotation);
					flag = false;
				}
				else if (!t2.def.rotatable && t2.def.category != ThingCategory.Building)
				{
					flag = false;
				}
				Rot4 rot = (!flag) ? t2.def.defaultPlacingRot : this.rotation.Rotated(positionData2.relativeRotation);
				working[t2] = new PositionData.Data
				{
					local = adjustedLocalPosition,
					rotation = rot,
					drafted = positionData2.drafted
				};
			}
			prevRotation = this.rotation;
			return working;
		}

		// Token: 0x0600DD2C RID: 56620 RVA: 0x00525B94 File Offset: 0x00523D94
		private Dictionary<IntVec3, T> GetRotatedValues<T>(Dictionary<IntVec3, T> dictionary, ref Dictionary<IntVec3, T> working, ref Rot4 prevRotation)
		{
			if (working != null && prevRotation == this.rotation)
			{
				return working;
			}
			if (working == null)
			{
				working = new Dictionary<IntVec3, T>(dictionary.Count);
			}
			working.Clear();
			foreach (KeyValuePair<IntVec3, T> keyValuePair in dictionary)
			{
				IntVec3 intVec;
				T t;
				keyValuePair.Deconstruct(out intVec, out t);
				IntVec3 local = intVec;
				T value = t;
				working[PrefabUtility.GetAdjustedLocalPosition(local, this.rotation)] = value;
			}
			prevRotation = this.rotation;
			return working;
		}

		// Token: 0x0600DD2D RID: 56621 RVA: 0x00525C40 File Offset: 0x00523E40
		private IEnumerable<ValueTuple<IntVec3, T>> GetRotatedValues<T>(Dictionary<IntVec3, T> dictionary)
		{
			Gravship.<GetRotatedValues>d__143<T> <GetRotatedValues>d__ = new Gravship.<GetRotatedValues>d__143<T>(-2);
			<GetRotatedValues>d__.<>4__this = this;
			<GetRotatedValues>d__.<>3__dictionary = dictionary;
			return <GetRotatedValues>d__;
		}

		// Token: 0x0600DD2E RID: 56622 RVA: 0x00525C57 File Offset: 0x00523E57
		private IEnumerable<IntVec3> GetRotatedValues(List<IntVec3> list)
		{
			Gravship.<GetRotatedValues>d__144 <GetRotatedValues>d__ = new Gravship.<GetRotatedValues>d__144(-2);
			<GetRotatedValues>d__.<>4__this = this;
			<GetRotatedValues>d__.<>3__list = list;
			return <GetRotatedValues>d__;
		}

		// Token: 0x04008D88 RID: 36232
		private Rot4 rotation;

		// Token: 0x04008D89 RID: 36233
		private CellRect bounds;

		// Token: 0x04008D8A RID: 36234
		private Building_GravEngine engine;

		// Token: 0x04008D8B RID: 36235
		private Building pilotConsole;

		// Token: 0x04008D8C RID: 36236
		private Dictionary<Thing, PositionData> things;

		// Token: 0x04008D8D RID: 36237
		private Dictionary<Pawn, PositionData> pawns;

		// Token: 0x04008D8E RID: 36238
		private Dictionary<Thing, bool> powerOn;

		// Token: 0x04008D8F RID: 36239
		private Dictionary<Thing, Thing> connectParents;

		// Token: 0x04008D90 RID: 36240
		private Dictionary<IntVec3, RoofDef> roofs;

		// Token: 0x04008D91 RID: 36241
		private Dictionary<IntVec3, TerrainDef> foundations;

		// Token: 0x04008D92 RID: 36242
		private Dictionary<IntVec3, TerrainDef> terrains;

		// Token: 0x04008D93 RID: 36243
		private Dictionary<IntVec3, ColorDef> terrainColors;

		// Token: 0x04008D94 RID: 36244
		private Dictionary<IntVec3, Designation> terrainDesignations;

		// Token: 0x04008D95 RID: 36245
		private Dictionary<IntVec3, uint> gases;

		// Token: 0x04008D96 RID: 36246
		private List<CellRect> occupiedRects;

		// Token: 0x04008D97 RID: 36247
		private List<RoomTemperatureVacuum> roomTemperatures;

		// Token: 0x04008D98 RID: 36248
		private List<IntVec3> gravFieldExtenderPositions;

		// Token: 0x04008D99 RID: 36249
		private StoryState storyState;

		// Token: 0x04008D9A RID: 36250
		public MoveableAreas areas;

		// Token: 0x04008D9B RID: 36251
		public List<AutoSlaughterConfig> autoSlaughterConfigs;

		// Token: 0x04008D9C RID: 36252
		private Dictionary<Thing, PositionData> thrusters;

		// Token: 0x04008D9D RID: 36253
		private Dictionary<Thing, PositionData> exteriorDoors;

		// Token: 0x04008D9E RID: 36254
		[Unsaved(false)]
		public IntVec3 originalPosition;

		// Token: 0x04008D9F RID: 36255
		public Vector3 engineToCenter;

		// Token: 0x04008DA0 RID: 36256
		public IntVec3 launchDirection;

		// Token: 0x04008DA1 RID: 36257
		[Unsaved(false)]
		public Capture capture;

		// Token: 0x04008DA2 RID: 36258
		[Unsaved(false)]
		public List<LayerSubMesh> bakedIndoorMasks = new List<LayerSubMesh>();

		// Token: 0x04008DA3 RID: 36259
		public PlanetTile destinationTile = PlanetTile.Invalid;

		// Token: 0x04008DA4 RID: 36260
		private PlanetTile initialTile = PlanetTile.Invalid;

		// Token: 0x04008DA5 RID: 36261
		private float traveledPct;

		// Token: 0x04008DA6 RID: 36262
		private bool arrived;

		// Token: 0x04008DA7 RID: 36263
		private Material cachedMat;

		// Token: 0x04008DA8 RID: 36264
		private const float TravelSpeed = 0.00025f;

		// Token: 0x04008DA9 RID: 36265
		private List<Thing> tmpPoweredThings = new List<Thing>();

		// Token: 0x04008DAA RID: 36266
		private List<bool> tmpPoweredThingsOn = new List<bool>();

		// Token: 0x04008DAB RID: 36267
		private List<Thing> tmpConnectedThings = new List<Thing>();

		// Token: 0x04008DAC RID: 36268
		private List<Thing> tmpConnectedParents = new List<Thing>();

		// Token: 0x04008DAD RID: 36269
		private List<Thing> tmpThrusters = new List<Thing>();

		// Token: 0x04008DAE RID: 36270
		private List<PositionData> tmpThrusterPositionData = new List<PositionData>();

		// Token: 0x04008DAF RID: 36271
		private List<Thing> tmpExteriorDoors = new List<Thing>();

		// Token: 0x04008DB0 RID: 36272
		private List<PositionData> tmpExteriorDoorPositionData = new List<PositionData>();

		// Token: 0x04008DB1 RID: 36273
		private static readonly Dictionary<Faction, int> tmpFactionMembersKidnappedCount = new Dictionary<Faction, int>();

		// Token: 0x04008DB2 RID: 36274
		private Dictionary<Thing, PositionData.Data> tmpThingPlacements;

		// Token: 0x04008DB3 RID: 36275
		private Rot4 tmpThingsRot;

		// Token: 0x04008DB4 RID: 36276
		private Dictionary<Pawn, PositionData.Data> tmpPawns;

		// Token: 0x04008DB5 RID: 36277
		private Rot4 tmpPawnsRot;

		// Token: 0x04008DB6 RID: 36278
		private Dictionary<Thing, PositionData.Data> tmpThrusterPlacements;

		// Token: 0x04008DB7 RID: 36279
		private Rot4 tmpThrusterRot;

		// Token: 0x04008DB8 RID: 36280
		private Dictionary<Thing, PositionData.Data> tmpExteriorDoorPlacements;

		// Token: 0x04008DB9 RID: 36281
		private Rot4 tmpExteriorDoorRot;

		// Token: 0x04008DBA RID: 36282
		private Dictionary<IntVec3, TerrainDef> tmpFoundations;

		// Token: 0x04008DBB RID: 36283
		private Rot4 tmpFoundationRot;

		// Token: 0x04008DBC RID: 36284
		private Dictionary<IntVec3, TerrainDef> tmpTerrains;

		// Token: 0x04008DBD RID: 36285
		private Rot4 tmpTerrainRot;
	}
}
