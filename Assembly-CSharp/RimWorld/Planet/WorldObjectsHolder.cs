using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200236E RID: 9070
	public class WorldObjectsHolder : IExposable
	{
		// Token: 0x1700278E RID: 10126
		// (get) Token: 0x0600E0F5 RID: 57589 RVA: 0x00538D1D File Offset: 0x00536F1D
		public List<WorldObject> AllWorldObjects
		{
			get
			{
				return this.worldObjects;
			}
		}

		// Token: 0x1700278F RID: 10127
		// (get) Token: 0x0600E0F6 RID: 57590 RVA: 0x00538D25 File Offset: 0x00536F25
		public List<Caravan> Caravans
		{
			get
			{
				return this.caravans;
			}
		}

		// Token: 0x17002790 RID: 10128
		// (get) Token: 0x0600E0F7 RID: 57591 RVA: 0x00538D2D File Offset: 0x00536F2D
		public List<Settlement> Settlements
		{
			get
			{
				return this.settlements;
			}
		}

		// Token: 0x17002791 RID: 10129
		// (get) Token: 0x0600E0F8 RID: 57592 RVA: 0x00538D35 File Offset: 0x00536F35
		public List<TravellingTransporters> TravellingTransporters
		{
			get
			{
				return this.travellingTransporters;
			}
		}

		// Token: 0x17002792 RID: 10130
		// (get) Token: 0x0600E0F9 RID: 57593 RVA: 0x00538D3D File Offset: 0x00536F3D
		public List<Settlement> SettlementBases
		{
			get
			{
				return this.settlementBases;
			}
		}

		// Token: 0x17002793 RID: 10131
		// (get) Token: 0x0600E0FA RID: 57594 RVA: 0x00538D45 File Offset: 0x00536F45
		public List<DestroyedSettlement> DestroyedSettlements
		{
			get
			{
				return this.destroyedSettlements;
			}
		}

		// Token: 0x17002794 RID: 10132
		// (get) Token: 0x0600E0FB RID: 57595 RVA: 0x00538D4D File Offset: 0x00536F4D
		public List<RoutePlannerWaypoint> RoutePlannerWaypoints
		{
			get
			{
				return this.routePlannerWaypoints;
			}
		}

		// Token: 0x17002795 RID: 10133
		// (get) Token: 0x0600E0FC RID: 57596 RVA: 0x00538D55 File Offset: 0x00536F55
		public List<MapParent> MapParents
		{
			get
			{
				return this.mapParents;
			}
		}

		// Token: 0x17002796 RID: 10134
		// (get) Token: 0x0600E0FD RID: 57597 RVA: 0x00538D5D File Offset: 0x00536F5D
		public List<Site> Sites
		{
			get
			{
				return this.sites;
			}
		}

		// Token: 0x17002797 RID: 10135
		// (get) Token: 0x0600E0FE RID: 57598 RVA: 0x00538D65 File Offset: 0x00536F65
		public List<PeaceTalks> PeaceTalks
		{
			get
			{
				return this.peaceTalks;
			}
		}

		// Token: 0x17002798 RID: 10136
		// (get) Token: 0x0600E0FF RID: 57599 RVA: 0x00538D6D File Offset: 0x00536F6D
		public int WorldObjectsCount
		{
			get
			{
				return this.worldObjects.Count;
			}
		}

		// Token: 0x17002799 RID: 10137
		// (get) Token: 0x0600E100 RID: 57600 RVA: 0x00538D7A File Offset: 0x00536F7A
		public int CaravansCount
		{
			get
			{
				return this.caravans.Count;
			}
		}

		// Token: 0x1700279A RID: 10138
		// (get) Token: 0x0600E101 RID: 57601 RVA: 0x00538D87 File Offset: 0x00536F87
		public int RoutePlannerWaypointsCount
		{
			get
			{
				return this.routePlannerWaypoints.Count;
			}
		}

		// Token: 0x1700279B RID: 10139
		// (get) Token: 0x0600E102 RID: 57602 RVA: 0x00538D94 File Offset: 0x00536F94
		public int PlayerControlledCaravansCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.caravans.Count; i++)
				{
					if (this.caravans[i].IsPlayerControlled)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x0600E103 RID: 57603 RVA: 0x00538DD1 File Offset: 0x00536FD1
		public List<WorldObject> AllWorldObjectsOnLayer(PlanetLayer layer)
		{
			return this.layerWorldObjects.GetValueOrDefault(layer, WorldObjectsHolder.EmptyWorldObjectList);
		}

		// Token: 0x0600E104 RID: 57604 RVA: 0x00538DE4 File Offset: 0x00536FE4
		public List<WorldObject> AllSettlementsOnLayer(PlanetLayer layer)
		{
			List<WorldObject> list;
			if (!this.layerWorldObjects.TryGetValue(layer, out list))
			{
				return WorldObjectsHolder.EmptyWorldObjectList;
			}
			WorldObjectsHolder.tmpSettlements.Clear();
			foreach (WorldObject worldObject in list)
			{
				if (worldObject is Settlement)
				{
					WorldObjectsHolder.tmpSettlements.Add(worldObject);
				}
			}
			return WorldObjectsHolder.tmpSettlements;
		}

		// Token: 0x0600E105 RID: 57605 RVA: 0x00538E64 File Offset: 0x00537064
		public bool AnyFactionSettlementOnLayer(Faction faction, PlanetLayer layer)
		{
			List<WorldObject> list;
			if (!this.layerWorldObjects.TryGetValue(layer, out list))
			{
				return false;
			}
			foreach (WorldObject worldObject in list)
			{
				Settlement settlement = worldObject as Settlement;
				if (settlement != null && settlement.Faction == faction)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600E106 RID: 57606 RVA: 0x00538ED8 File Offset: 0x005370D8
		public bool AnyFactionSettlementOnRootSurface(Faction faction)
		{
			return this.AnyFactionSettlementOnLayer(faction, Find.WorldGrid.Surface);
		}

		// Token: 0x0600E107 RID: 57607 RVA: 0x00538EEC File Offset: 0x005370EC
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				WorldObjectsHolder.tmpUnsavedWorldObjects.Clear();
				for (int i = this.worldObjects.Count - 1; i >= 0; i--)
				{
					if (!this.worldObjects[i].def.saved)
					{
						WorldObjectsHolder.tmpUnsavedWorldObjects.Add(this.worldObjects[i]);
						this.worldObjects.RemoveAt(i);
					}
				}
			}
			Scribe_Collections.Look<WorldObject>(ref this.worldObjects, "worldObjects", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.worldObjects.AddRange(WorldObjectsHolder.tmpUnsavedWorldObjects);
				WorldObjectsHolder.tmpUnsavedWorldObjects.Clear();
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.worldObjects.RemoveAll((WorldObject wo) => wo == null);
				this.Recache();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.worldObjects.RemoveAll((WorldObject wo) => wo == null || wo.def == null) != 0)
				{
					Log.Error("Some WorldObjects had null def after loading.");
				}
				for (int j = this.worldObjects.Count - 1; j >= 0; j--)
				{
					try
					{
						this.worldObjects[j].SpawnSetup();
					}
					catch (Exception value)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Exception spawning WorldObject: ");
						defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
						Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
						this.worldObjects.RemoveAt(j);
					}
				}
			}
		}

		// Token: 0x0600E108 RID: 57608 RVA: 0x00539084 File Offset: 0x00537284
		public void Add(WorldObject o)
		{
			if (this.worldObjects.Contains(o))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to add world object ");
				defaultInterpolatedStringHandler.AppendFormatted<WorldObject>(o);
				defaultInterpolatedStringHandler.AppendLiteral(" to world, but it's already here.");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			if (!o.Tile.Valid && !(o is PocketMapParent))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(65, 1);
				defaultInterpolatedStringHandler2.AppendLiteral("Tried to add world object ");
				defaultInterpolatedStringHandler2.AppendFormatted<WorldObject>(o);
				defaultInterpolatedStringHandler2.AppendLiteral(" but its tile is not set. Setting to 0.");
				Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
				o.Tile = new PlanetTile(0);
			}
			this.worldObjects.Add(o);
			this.AddToCache(o);
			o.SpawnSetup();
			o.PostAdd();
		}

		// Token: 0x0600E109 RID: 57609 RVA: 0x00539150 File Offset: 0x00537350
		public void Remove(WorldObject o)
		{
			if (!this.worldObjects.Contains(o))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(60, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to remove world object ");
				defaultInterpolatedStringHandler.AppendFormatted<WorldObject>(o);
				defaultInterpolatedStringHandler.AppendLiteral(" from world, but it's not here.");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			this.worldObjects.Remove(o);
			this.RemoveFromCache(o);
			o.PostRemove();
		}

		// Token: 0x0600E10A RID: 57610 RVA: 0x005391BC File Offset: 0x005373BC
		public void WorldObjectsHolderTick()
		{
			WorldObjectsHolder.tmpWorldObjects.Clear();
			WorldObjectsHolder.tmpWorldObjects.AddRange(this.worldObjects);
			for (int i = 0; i < WorldObjectsHolder.tmpWorldObjects.Count; i++)
			{
				WorldObjectsHolder.tmpWorldObjects[i].DoTick();
			}
		}

		// Token: 0x0600E10B RID: 57611 RVA: 0x00539208 File Offset: 0x00537408
		private void AddToCache(WorldObject o)
		{
			this.worldObjectsHashSet.Add(o);
			if (o.Tile.Valid)
			{
				List<WorldObject> list;
				if (!this.layerWorldObjects.TryGetValue(o.Tile.Layer, out list))
				{
					list = (this.layerWorldObjects[o.Tile.Layer] = new List<WorldObject>());
				}
				list.Add(o);
			}
			Caravan caravan = o as Caravan;
			if (caravan != null)
			{
				this.caravans.Add(caravan);
			}
			Settlement settlement = o as Settlement;
			if (settlement != null)
			{
				this.settlements.Add(settlement);
				this.settlementBases.Add(settlement);
			}
			TravellingTransporters travellingTransporters = o as TravellingTransporters;
			if (travellingTransporters != null)
			{
				this.travellingTransporters.Add(travellingTransporters);
			}
			DestroyedSettlement destroyedSettlement = o as DestroyedSettlement;
			if (destroyedSettlement != null)
			{
				this.destroyedSettlements.Add(destroyedSettlement);
			}
			RoutePlannerWaypoint routePlannerWaypoint = o as RoutePlannerWaypoint;
			if (routePlannerWaypoint != null)
			{
				this.routePlannerWaypoints.Add(routePlannerWaypoint);
			}
			MapParent mapParent = o as MapParent;
			if (mapParent != null)
			{
				this.mapParents.Add(mapParent);
			}
			Site site = o as Site;
			if (site != null)
			{
				this.sites.Add(site);
			}
			PeaceTalks peaceTalks = o as PeaceTalks;
			if (peaceTalks != null)
			{
				this.peaceTalks.Add(peaceTalks);
			}
			ExpandableLandmarksUtility.Notify_WorldObjectsChanged();
		}

		// Token: 0x0600E10C RID: 57612 RVA: 0x0053934C File Offset: 0x0053754C
		private void RemoveFromCache(WorldObject o)
		{
			this.worldObjectsHashSet.Remove(o);
			List<WorldObject> list;
			if (o.Tile.Valid && this.layerWorldObjects.TryGetValue(o.Tile.Layer, out list))
			{
				list.Remove(o);
			}
			Caravan caravan = o as Caravan;
			if (caravan != null)
			{
				this.caravans.Remove(caravan);
			}
			Settlement settlement = o as Settlement;
			if (settlement != null)
			{
				this.settlements.Remove(settlement);
				this.settlementBases.Remove(settlement);
			}
			TravellingTransporters travellingTransporters = o as TravellingTransporters;
			if (travellingTransporters != null)
			{
				this.travellingTransporters.Remove(travellingTransporters);
			}
			DestroyedSettlement destroyedSettlement = o as DestroyedSettlement;
			if (destroyedSettlement != null)
			{
				this.destroyedSettlements.Remove(destroyedSettlement);
			}
			RoutePlannerWaypoint routePlannerWaypoint = o as RoutePlannerWaypoint;
			if (routePlannerWaypoint != null)
			{
				this.routePlannerWaypoints.Remove(routePlannerWaypoint);
			}
			MapParent mapParent = o as MapParent;
			if (mapParent != null)
			{
				this.mapParents.Remove(mapParent);
			}
			Site site = o as Site;
			if (site != null)
			{
				this.sites.Remove(site);
			}
			PeaceTalks peaceTalks = o as PeaceTalks;
			if (peaceTalks != null)
			{
				this.peaceTalks.Remove(peaceTalks);
			}
			ExpandableLandmarksUtility.Notify_WorldObjectsChanged();
		}

		// Token: 0x0600E10D RID: 57613 RVA: 0x00539478 File Offset: 0x00537678
		private void Recache()
		{
			this.worldObjectsHashSet.Clear();
			this.layerWorldObjects.Clear();
			this.caravans.Clear();
			this.settlements.Clear();
			this.travellingTransporters.Clear();
			this.settlementBases.Clear();
			this.destroyedSettlements.Clear();
			this.routePlannerWaypoints.Clear();
			this.mapParents.Clear();
			this.sites.Clear();
			this.peaceTalks.Clear();
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				this.AddToCache(this.worldObjects[i]);
			}
		}

		// Token: 0x0600E10E RID: 57614 RVA: 0x00539526 File Offset: 0x00537726
		public bool Contains(WorldObject o)
		{
			return o != null && this.worldObjectsHashSet.Contains(o);
		}

		// Token: 0x0600E10F RID: 57615 RVA: 0x00539539 File Offset: 0x00537739
		public IEnumerable<WorldObject> ObjectsAt(PlanetTile tileID)
		{
			WorldObjectsHolder.<ObjectsAt>d__58 <ObjectsAt>d__ = new WorldObjectsHolder.<ObjectsAt>d__58(-2);
			<ObjectsAt>d__.<>4__this = this;
			<ObjectsAt>d__.<>3__tileID = tileID;
			return <ObjectsAt>d__;
		}

		// Token: 0x0600E110 RID: 57616 RVA: 0x00539550 File Offset: 0x00537750
		public bool AnyWorldObjectOnLayer(PlanetLayer layer)
		{
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				if (this.worldObjects[i].Tile.Layer == layer)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600E111 RID: 57617 RVA: 0x00539594 File Offset: 0x00537794
		public bool AnyWorldObjectAt(PlanetTile tile)
		{
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				if (this.worldObjects[i].Tile == tile)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600E112 RID: 57618 RVA: 0x005395D3 File Offset: 0x005377D3
		public bool AnyWorldObjectAt<T>(PlanetTile tile) where T : WorldObject
		{
			return this.WorldObjectAt<T>(tile) != null;
		}

		// Token: 0x0600E113 RID: 57619 RVA: 0x005395E4 File Offset: 0x005377E4
		public T WorldObjectAt<T>(PlanetTile tile) where T : WorldObject
		{
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				if (this.worldObjects[i].Tile == tile && this.worldObjects[i] is T)
				{
					return this.worldObjects[i] as T;
				}
			}
			return default(T);
		}

		// Token: 0x0600E114 RID: 57620 RVA: 0x00539654 File Offset: 0x00537854
		public bool TryGetWorldObjectAt<T>(PlanetTile tile, out T wo) where T : WorldObject
		{
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				if (this.worldObjects[i].Tile == tile && this.worldObjects[i] is T)
				{
					wo = (this.worldObjects[i] as T);
					return true;
				}
			}
			wo = default(T);
			return false;
		}

		// Token: 0x0600E115 RID: 57621 RVA: 0x005396C9 File Offset: 0x005378C9
		public bool AnyWorldObjectAt(PlanetTile tile, WorldObjectDef def)
		{
			return this.WorldObjectAt(tile, def) != null;
		}

		// Token: 0x0600E116 RID: 57622 RVA: 0x005396D8 File Offset: 0x005378D8
		public WorldObject WorldObjectAt(PlanetTile tile, WorldObjectDef def)
		{
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				if (this.worldObjects[i].Tile == tile && this.worldObjects[i].def == def)
				{
					return this.worldObjects[i];
				}
			}
			return null;
		}

		// Token: 0x0600E117 RID: 57623 RVA: 0x00539736 File Offset: 0x00537936
		public bool AnySettlementAt(PlanetTile tile)
		{
			return this.SettlementAt(tile) != null;
		}

		// Token: 0x0600E118 RID: 57624 RVA: 0x00539744 File Offset: 0x00537944
		public Settlement SettlementAt(PlanetTile tile)
		{
			for (int i = 0; i < this.settlements.Count; i++)
			{
				if (this.settlements[i].Tile == tile)
				{
					return this.settlements[i];
				}
			}
			return null;
		}

		// Token: 0x0600E119 RID: 57625 RVA: 0x0053978E File Offset: 0x0053798E
		public bool AnySettlementBaseAt(PlanetTile tile)
		{
			return this.SettlementBaseAt(tile) != null;
		}

		// Token: 0x0600E11A RID: 57626 RVA: 0x0053979C File Offset: 0x0053799C
		public Settlement SettlementBaseAt(PlanetTile tile)
		{
			for (int i = 0; i < this.settlementBases.Count; i++)
			{
				if (this.settlementBases[i].Tile == tile)
				{
					return this.settlementBases[i];
				}
			}
			return null;
		}

		// Token: 0x0600E11B RID: 57627 RVA: 0x005397E6 File Offset: 0x005379E6
		public bool AnySiteAt(PlanetTile tile)
		{
			return this.SiteAt(tile) != null;
		}

		// Token: 0x0600E11C RID: 57628 RVA: 0x005397F4 File Offset: 0x005379F4
		public Site SiteAt(PlanetTile tile)
		{
			for (int i = 0; i < this.sites.Count; i++)
			{
				if (this.sites[i].Tile == tile)
				{
					return this.sites[i];
				}
			}
			return null;
		}

		// Token: 0x0600E11D RID: 57629 RVA: 0x0053983E File Offset: 0x00537A3E
		public bool AnyDestroyedSettlementAt(PlanetTile tile)
		{
			return this.DestroyedSettlementAt(tile) != null;
		}

		// Token: 0x0600E11E RID: 57630 RVA: 0x0053984C File Offset: 0x00537A4C
		public DestroyedSettlement DestroyedSettlementAt(PlanetTile tile)
		{
			for (int i = 0; i < this.destroyedSettlements.Count; i++)
			{
				if (this.destroyedSettlements[i].Tile == tile)
				{
					return this.destroyedSettlements[i];
				}
			}
			return null;
		}

		// Token: 0x0600E11F RID: 57631 RVA: 0x00539896 File Offset: 0x00537A96
		public bool AnyMapParentAt(PlanetTile tile)
		{
			return this.MapParentAt(tile) != null;
		}

		// Token: 0x0600E120 RID: 57632 RVA: 0x005398A4 File Offset: 0x00537AA4
		public MapParent MapParentAt(PlanetTile tile)
		{
			for (int i = 0; i < this.mapParents.Count; i++)
			{
				if (this.mapParents[i].Tile == tile)
				{
					return this.mapParents[i];
				}
			}
			return null;
		}

		// Token: 0x0600E121 RID: 57633 RVA: 0x005398EE File Offset: 0x00537AEE
		public bool AnyWorldObjectOfDefAt(WorldObjectDef def, PlanetTile tile)
		{
			return this.WorldObjectOfDefAt(def, tile) != null;
		}

		// Token: 0x0600E122 RID: 57634 RVA: 0x005398FC File Offset: 0x00537AFC
		public WorldObject WorldObjectOfDefAt(WorldObjectDef def, PlanetTile tile)
		{
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				if (this.worldObjects[i].def == def && this.worldObjects[i].Tile == tile)
				{
					return this.worldObjects[i];
				}
			}
			return null;
		}

		// Token: 0x0600E123 RID: 57635 RVA: 0x0053995C File Offset: 0x00537B5C
		public bool AnyGeneratedWorldLocationAt(PlanetTile tile)
		{
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				if (this.worldObjects[i].isGeneratedLocation && this.worldObjects[i].Tile == tile)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600E124 RID: 57636 RVA: 0x005399B0 File Offset: 0x00537BB0
		public Caravan PlayerControlledCaravanAt(PlanetTile tile)
		{
			for (int i = 0; i < this.caravans.Count; i++)
			{
				if (this.caravans[i].Tile == tile && this.caravans[i].IsPlayerControlled)
				{
					return this.caravans[i];
				}
			}
			return null;
		}

		// Token: 0x0600E125 RID: 57637 RVA: 0x00539A10 File Offset: 0x00537C10
		public bool AnySettlementBaseAtOrAdjacent(PlanetTile tile)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			for (int i = 0; i < this.settlementBases.Count; i++)
			{
				Settlement settlement = this.settlementBases[i];
				if (settlement.Tile.Layer == tile.Layer && worldGrid.IsNeighborOrSame(settlement.Tile, tile))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600E126 RID: 57638 RVA: 0x00539A70 File Offset: 0x00537C70
		public bool AnySettlementBaseAtOrAdjacent(PlanetTile tile, out WorldObject wo)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			for (int i = 0; i < this.settlementBases.Count; i++)
			{
				Settlement settlement = this.settlementBases[i];
				if (settlement.Tile.Layer == tile.Layer && worldGrid.IsNeighborOrSame(settlement.Tile, tile))
				{
					wo = settlement;
					return true;
				}
			}
			wo = null;
			return false;
		}

		// Token: 0x0600E127 RID: 57639 RVA: 0x00539AD8 File Offset: 0x00537CD8
		public RoutePlannerWaypoint RoutePlannerWaypointAt(PlanetTile tile)
		{
			for (int i = 0; i < this.routePlannerWaypoints.Count; i++)
			{
				if (this.routePlannerWaypoints[i].Tile == tile)
				{
					return this.routePlannerWaypoints[i];
				}
			}
			return null;
		}

		// Token: 0x0600E128 RID: 57640 RVA: 0x00539B24 File Offset: 0x00537D24
		public void GetPlayerControlledCaravansAt(PlanetTile tile, List<Caravan> outCaravans)
		{
			outCaravans.Clear();
			for (int i = 0; i < this.caravans.Count; i++)
			{
				Caravan caravan = this.caravans[i];
				if (caravan.Tile == tile && caravan.IsPlayerControlled)
				{
					outCaravans.Add(caravan);
				}
			}
		}

		// Token: 0x04009022 RID: 36898
		private List<WorldObject> worldObjects = new List<WorldObject>();

		// Token: 0x04009023 RID: 36899
		private readonly Dictionary<PlanetLayer, List<WorldObject>> layerWorldObjects = new Dictionary<PlanetLayer, List<WorldObject>>();

		// Token: 0x04009024 RID: 36900
		private readonly HashSet<WorldObject> worldObjectsHashSet = new HashSet<WorldObject>();

		// Token: 0x04009025 RID: 36901
		private readonly List<Caravan> caravans = new List<Caravan>();

		// Token: 0x04009026 RID: 36902
		private readonly List<Settlement> settlements = new List<Settlement>();

		// Token: 0x04009027 RID: 36903
		private readonly List<TravellingTransporters> travellingTransporters = new List<TravellingTransporters>();

		// Token: 0x04009028 RID: 36904
		private readonly List<Settlement> settlementBases = new List<Settlement>();

		// Token: 0x04009029 RID: 36905
		private readonly List<DestroyedSettlement> destroyedSettlements = new List<DestroyedSettlement>();

		// Token: 0x0400902A RID: 36906
		private readonly List<RoutePlannerWaypoint> routePlannerWaypoints = new List<RoutePlannerWaypoint>();

		// Token: 0x0400902B RID: 36907
		private readonly List<MapParent> mapParents = new List<MapParent>();

		// Token: 0x0400902C RID: 36908
		private readonly List<Site> sites = new List<Site>();

		// Token: 0x0400902D RID: 36909
		private readonly List<PeaceTalks> peaceTalks = new List<PeaceTalks>();

		// Token: 0x0400902E RID: 36910
		private static readonly List<WorldObject> EmptyWorldObjectList = new List<WorldObject>();

		// Token: 0x0400902F RID: 36911
		private static readonly List<WorldObject> tmpSettlements = new List<WorldObject>();

		// Token: 0x04009030 RID: 36912
		private List<PlanetTile> tmpReservedTiles = new List<PlanetTile>();

		// Token: 0x04009031 RID: 36913
		private List<ILoadReferenceable> tmpTileReservers = new List<ILoadReferenceable>();

		// Token: 0x04009032 RID: 36914
		private static readonly List<WorldObject> tmpUnsavedWorldObjects = new List<WorldObject>();

		// Token: 0x04009033 RID: 36915
		private static readonly List<WorldObject> tmpWorldObjects = new List<WorldObject>();
	}
}
