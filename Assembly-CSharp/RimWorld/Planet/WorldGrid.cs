using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002355 RID: 9045
	public class WorldGrid : IExposable, IDisposable
	{
		// Token: 0x1400001E RID: 30
		// (add) Token: 0x0600DFF6 RID: 57334 RVA: 0x005353A8 File Offset: 0x005335A8
		// (remove) Token: 0x0600DFF7 RID: 57335 RVA: 0x005353E0 File Offset: 0x005335E0
		public event Action<PlanetLayer> OnPlanetLayerAdded;

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x0600DFF8 RID: 57336 RVA: 0x00535418 File Offset: 0x00533618
		// (remove) Token: 0x0600DFF9 RID: 57337 RVA: 0x00535450 File Offset: 0x00533650
		public event Action<PlanetLayer> OnPlanetLayerRemoved;

		// Token: 0x17002755 RID: 10069
		// (get) Token: 0x0600DFFA RID: 57338 RVA: 0x00535485 File Offset: 0x00533685
		public SurfaceLayer Surface
		{
			get
			{
				return this.surface;
			}
		}

		// Token: 0x17002756 RID: 10070
		// (get) Token: 0x0600DFFB RID: 57339 RVA: 0x0053548D File Offset: 0x0053368D
		public PlanetLayer Orbit
		{
			get
			{
				return this.orbit;
			}
		}

		// Token: 0x17002757 RID: 10071
		// (get) Token: 0x0600DFFC RID: 57340 RVA: 0x00535495 File Offset: 0x00533695
		public IReadOnlyList<WorldDrawLayerBase> GlobalLayers
		{
			get
			{
				return this.globalLayers;
			}
		}

		// Token: 0x17002758 RID: 10072
		// (get) Token: 0x0600DFFD RID: 57341 RVA: 0x0053549D File Offset: 0x0053369D
		public float SurfaceViewAngle
		{
			get
			{
				return this.surfaceViewAngle;
			}
		}

		// Token: 0x17002759 RID: 10073
		// (get) Token: 0x0600DFFE RID: 57342 RVA: 0x005354A5 File Offset: 0x005336A5
		public Vector3 SurfaceViewCenter
		{
			get
			{
				return this.surfaceViewCenter;
			}
		}

		// Token: 0x1700275A RID: 10074
		// (get) Token: 0x0600DFFF RID: 57343 RVA: 0x005354AD File Offset: 0x005336AD
		public int TilesCount
		{
			get
			{
				return this.surface.TilesCount;
			}
		}

		// Token: 0x1700275B RID: 10075
		// (get) Token: 0x0600E000 RID: 57344 RVA: 0x005354BA File Offset: 0x005336BA
		public IEnumerable<SurfaceTile> Tiles
		{
			get
			{
				return this.surface.Tiles.Cast<SurfaceTile>();
			}
		}

		// Token: 0x1700275C RID: 10076
		// (get) Token: 0x0600E001 RID: 57345 RVA: 0x005354CC File Offset: 0x005336CC
		public IReadOnlyDictionary<int, PlanetLayer> PlanetLayers
		{
			get
			{
				return this.planetLayers;
			}
		}

		// Token: 0x1700275D RID: 10077
		// (get) Token: 0x0600E002 RID: 57346 RVA: 0x005354D4 File Offset: 0x005336D4
		public NativeArray<Vector3> UnsafeVerts
		{
			get
			{
				return this.surface.UnsafeVerts;
			}
		}

		// Token: 0x1700275E RID: 10078
		// (get) Token: 0x0600E003 RID: 57347 RVA: 0x005354E1 File Offset: 0x005336E1
		public NativeArray<int> UnsafeTileIDToVerts_offsets
		{
			get
			{
				return this.surface.UnsafeTileIDToVerts_offsets;
			}
		}

		// Token: 0x1700275F RID: 10079
		// (get) Token: 0x0600E004 RID: 57348 RVA: 0x005354EE File Offset: 0x005336EE
		public NativeArray<int> UnsafeTileIDToNeighbors_offsets
		{
			get
			{
				return this.surface.UnsafeTileIDToNeighbors_offsets;
			}
		}

		// Token: 0x17002760 RID: 10080
		// (get) Token: 0x0600E005 RID: 57349 RVA: 0x005354FB File Offset: 0x005336FB
		public NativeArray<PlanetTile> UnsafeTileIDToNeighbors_values
		{
			get
			{
				return this.surface.UnsafeTileIDToNeighbors_values;
			}
		}

		// Token: 0x17002761 RID: 10081
		// (get) Token: 0x0600E006 RID: 57350 RVA: 0x00535508 File Offset: 0x00533708
		public Vector3 NorthPolePos
		{
			get
			{
				return this.surface.NorthPolePos;
			}
		}

		// Token: 0x17002762 RID: 10082
		public SurfaceTile this[int tileID]
		{
			get
			{
				return this.surface[tileID];
			}
		}

		// Token: 0x17002763 RID: 10083
		public Tile this[PlanetTile tile]
		{
			get
			{
				return tile.Layer.Tiles[tile.tileId];
			}
		}

		// Token: 0x17002764 RID: 10084
		// (get) Token: 0x0600E009 RID: 57353 RVA: 0x0053553C File Offset: 0x0053373C
		public bool HasWorldData
		{
			get
			{
				return this.surface.HasWorldData;
			}
		}

		// Token: 0x17002765 RID: 10085
		// (get) Token: 0x0600E00A RID: 57354 RVA: 0x00535549 File Offset: 0x00533749
		public float AverageTileSize
		{
			get
			{
				return this.surface.AverageTileSize;
			}
		}

		// Token: 0x0600E00B RID: 57355 RVA: 0x00535556 File Offset: 0x00533756
		public void GenerateFreshWorld()
		{
			this.CalculateViewCenterAndAngle();
			this.CreateRequiredLayers();
		}

		// Token: 0x0600E00C RID: 57356 RVA: 0x00535564 File Offset: 0x00533764
		private void InitializeGlobalLayers()
		{
			this.globalLayers.Clear();
			foreach (GlobalWorldDrawLayerDef globalWorldDrawLayerDef in DefDatabase<GlobalWorldDrawLayerDef>.AllDefs)
			{
				this.globalLayers.Add((WorldDrawLayerBase)Activator.CreateInstance(globalWorldDrawLayerDef.worldDrawLayer));
			}
		}

		// Token: 0x0600E00D RID: 57357 RVA: 0x005355D0 File Offset: 0x005337D0
		private void CreateRequiredLayers()
		{
			this.InitializeGlobalLayers();
			Dictionary<PlanetLayer, List<LayerConnection>> dictionary = new Dictionary<PlanetLayer, List<LayerConnection>>();
			foreach (ScenPart scenPart in Find.Scenario.AllParts)
			{
				ScenPart_PlanetLayer scenPart_PlanetLayer = scenPart as ScenPart_PlanetLayer;
				if (scenPart_PlanetLayer != null)
				{
					bool flag = false;
					foreach (PlanetLayer planetLayer in this.planetLayers.Values)
					{
						if (!(planetLayer.ScenarioTag != scenPart_PlanetLayer.tag))
						{
							dictionary[planetLayer] = scenPart_PlanetLayer.connections;
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						PlanetLayer planetLayer2;
						if (scenPart_PlanetLayer == Find.Scenario.surfaceLayer)
						{
							PlanetLayerDef layer = scenPart_PlanetLayer.layer;
							PlanetLayerSettings settings = scenPart_PlanetLayer.Settings;
							Vector3? overrideViewCenter = new Vector3?(this.surfaceViewCenter);
							float? overrideViewAngle = new float?(this.surfaceViewAngle);
							planetLayer2 = (this.surface = (SurfaceLayer)this.RegisterPlanetLayer(layer, settings, null, null, overrideViewCenter, overrideViewAngle));
							this.surface.MarkRootSurface();
						}
						else
						{
							planetLayer2 = this.RegisterPlanetLayer(scenPart_PlanetLayer.layer, scenPart_PlanetLayer.Settings, null, null, null, null);
						}
						if (ModsConfig.OdysseyActive && scenPart_PlanetLayer.layer == PlanetLayerDefOf.Orbit)
						{
							this.orbit = planetLayer2;
						}
						planetLayer2.ScenarioTag = scenPart_PlanetLayer.tag;
						dictionary[planetLayer2] = scenPart_PlanetLayer.connections;
					}
				}
			}
			foreach (KeyValuePair<PlanetLayer, List<LayerConnection>> keyValuePair in dictionary)
			{
				PlanetLayer planetLayer3;
				List<LayerConnection> list;
				keyValuePair.Deconstruct(out planetLayer3, out list);
				PlanetLayer planetLayer4 = planetLayer3;
				foreach (LayerConnection layerConnection in list)
				{
					PlanetLayer planetLayer5;
					if (this.TryGetLayerByTag(layerConnection.tag, out planetLayer5) && !planetLayer4.HasConnectionFromTo(planetLayer5))
					{
						planetLayer4.AddConnection(planetLayer5, layerConnection.fuelCost);
						if (layerConnection.zoomMode == LayerConnection.ZoomMode.ZoomIn)
						{
							planetLayer4.zoomInToLayer = planetLayer5;
						}
						else if (layerConnection.zoomMode == LayerConnection.ZoomMode.ZoomOut)
						{
							planetLayer4.zoomOutToLayer = planetLayer5;
						}
					}
				}
			}
		}

		// Token: 0x0600E00E RID: 57358 RVA: 0x00535894 File Offset: 0x00533A94
		public bool TryGetLayerByTag(string tag, out PlanetLayer layer)
		{
			for (int i = 0; i < this.planetLayers.Count; i++)
			{
				if (this.planetLayers[i].ScenarioTag == tag)
				{
					layer = this.planetLayers[i];
					return true;
				}
			}
			layer = null;
			return false;
		}

		// Token: 0x0600E00F RID: 57359 RVA: 0x005358E4 File Offset: 0x00533AE4
		public Vector2 LongLatOf(PlanetTile tile)
		{
			if (!tile.Valid)
			{
				if (Find.AnyPlayerHomeMap == null)
				{
					return Vector2.zero;
				}
				tile = Find.AnyPlayerHomeMap.Tile;
			}
			return tile.Layer.LongLatOf(tile);
		}

		// Token: 0x0600E010 RID: 57360 RVA: 0x00535918 File Offset: 0x00533B18
		public PlanetLayer RegisterPlanetLayer(PlanetLayerDef layer, PlanetLayerSettings settings, float? overrideRadius = null, Vector3? overrideOrigin = null, Vector3? overrideViewCenter = null, float? overrideViewAngle = null)
		{
			float radius = overrideRadius ?? settings.radius;
			Vector3 value = overrideOrigin ?? settings.origin;
			Vector3 value2 = overrideViewCenter ?? this.surfaceViewCenter;
			float viewAngle = overrideViewAngle ?? (settings.useSurfaceViewAngle ? this.surfaceViewAngle : settings.viewAngle);
			return this.RegisterPlanetLayer(layer, new Vector3?(value), radius, viewAngle, settings.extraCameraAltitude, settings.subdivisions, settings.backgroundWorldCameraOffset, settings.backgroundWorldCameraParallaxDistancePer100Cells, new Vector3?(value2));
		}

		// Token: 0x0600E011 RID: 57361 RVA: 0x005359D8 File Offset: 0x00533BD8
		public PlanetLayer RegisterPlanetLayer(PlanetLayerDef def, Vector3? origin = null, float radius = 100f, float viewAngle = 180f, float extraCameraAltitude = 0f, int subdivisions = 10, float backgroundWorldCameraOffset = 0f, float backgroundWorldCameraParallaxDistancePer100Cells = 0f, Vector3? overrideViewCenter = null)
		{
			int num = this.nextLayerId;
			this.nextLayerId = num + 1;
			int num2 = num;
			Vector3 vector = origin ?? Vector3.zero;
			Vector3 vector2 = overrideViewCenter ?? this.surfaceViewCenter;
			PlanetLayer planetLayer = this.planetLayers[num2] = (PlanetLayer)Activator.CreateInstance(def.layerType, new object[]
			{
				num2,
				def,
				radius,
				vector,
				vector2,
				viewAngle,
				subdivisions,
				extraCameraAltitude,
				backgroundWorldCameraOffset,
				backgroundWorldCameraParallaxDistancePer100Cells
			});
			planetLayer.InitializeLayer();
			Action<PlanetLayer> onPlanetLayerAdded = this.OnPlanetLayerAdded;
			if (onPlanetLayerAdded != null)
			{
				onPlanetLayerAdded(planetLayer);
			}
			return planetLayer;
		}

		// Token: 0x0600E012 RID: 57362 RVA: 0x00535AD0 File Offset: 0x00533CD0
		public void RemovePlanetLayer(PlanetLayer layer)
		{
			if (!this.planetLayers.Remove(layer.LayerID))
			{
				return;
			}
			Action<PlanetLayer> onPlanetLayerRemoved = this.OnPlanetLayerRemoved;
			if (onPlanetLayerRemoved != null)
			{
				onPlanetLayerRemoved(layer);
			}
			foreach (KeyValuePair<int, PlanetLayer> keyValuePair in this.PlanetLayers)
			{
				int num;
				PlanetLayer planetLayer;
				keyValuePair.Deconstruct(out num, out planetLayer);
				PlanetLayer planetLayer2 = planetLayer;
				if (planetLayer2 != layer)
				{
					planetLayer2.RemoveConnection(layer);
				}
			}
			foreach (WorldObject worldObject in Find.WorldObjects.AllWorldObjectsOnLayer(layer))
			{
				worldObject.Destroy();
			}
			layer.Dispose();
		}

		// Token: 0x0600E013 RID: 57363 RVA: 0x00535BA0 File Offset: 0x00533DA0
		public bool TryGetFirstLayerOfDef(PlanetLayerDef def, out PlanetLayer layer)
		{
			layer = this.FirstLayerOfDef(def);
			return layer != null;
		}

		// Token: 0x0600E014 RID: 57364 RVA: 0x00535BB0 File Offset: 0x00533DB0
		public bool TryGetFirstAdjacentLayerOfDef(PlanetTile origin, PlanetLayerDef def, out PlanetLayer layer)
		{
			if (origin.LayerDef == def)
			{
				layer = origin.Layer;
				return true;
			}
			for (int i = 0; i < this.planetLayers.Count; i++)
			{
				if (this.planetLayers[i].Def == def && this.planetLayers[i].DirectConnectionTo(origin.Layer))
				{
					layer = this.planetLayers[i];
					return true;
				}
			}
			layer = null;
			return false;
		}

		// Token: 0x0600E015 RID: 57365 RVA: 0x00535C2C File Offset: 0x00533E2C
		public PlanetLayer FirstLayerOfDef(PlanetLayerDef def)
		{
			if (def == null)
			{
				return this.surface;
			}
			for (int i = 0; i < this.planetLayers.Count; i++)
			{
				if (this.planetLayers[i].Def == def)
				{
					return this.planetLayers[i];
				}
			}
			return null;
		}

		// Token: 0x0600E016 RID: 57366 RVA: 0x00535C7B File Offset: 0x00533E7B
		public Vector3 GetTileCenter(PlanetTile tile)
		{
			return tile.Layer.GetTileCenter(tile);
		}

		// Token: 0x0600E017 RID: 57367 RVA: 0x00535C8A File Offset: 0x00533E8A
		public bool InBounds(PlanetTile tile)
		{
			return tile.Layer.InBounds(tile);
		}

		// Token: 0x0600E018 RID: 57368 RVA: 0x00535C99 File Offset: 0x00533E99
		public float GetHeadingFromTo(Vector3 from, Vector3 to)
		{
			return this.GetHeadingFromTo(this.surface, from, to);
		}

		// Token: 0x0600E019 RID: 57369 RVA: 0x00535CA9 File Offset: 0x00533EA9
		public float GetHeadingFromTo(PlanetLayer layer, Vector3 from, Vector3 to)
		{
			return layer.GetHeadingFromTo(from, to);
		}

		// Token: 0x0600E01A RID: 57370 RVA: 0x00535CB3 File Offset: 0x00533EB3
		public float GetHeadingFromTo(PlanetTile fromTile, PlanetTile toTile)
		{
			return fromTile.Layer.GetHeadingFromTo(fromTile, toTile);
		}

		// Token: 0x0600E01B RID: 57371 RVA: 0x00535CC3 File Offset: 0x00533EC3
		public Direction8Way GetDirection8WayFromTo(PlanetTile fromTile, PlanetTile toTile)
		{
			return fromTile.Layer.GetDirection8WayFromTo(fromTile, toTile);
		}

		// Token: 0x0600E01C RID: 57372 RVA: 0x00535CD3 File Offset: 0x00533ED3
		public Rot4 GetRotFromTo(PlanetTile fromTile, PlanetTile toTile)
		{
			return fromTile.Layer.GetRotFromTo(fromTile, toTile);
		}

		// Token: 0x0600E01D RID: 57373 RVA: 0x00535CE3 File Offset: 0x00533EE3
		public void GetTileVertices(PlanetTile tile, List<Vector3> outVerts)
		{
			tile.Layer.GetTileVertices(tile, outVerts);
		}

		// Token: 0x0600E01E RID: 57374 RVA: 0x00535CF3 File Offset: 0x00533EF3
		public void GetTileVerticesIndices(PlanetTile tile, List<int> outVertsIndices)
		{
			tile.Layer.GetTileVerticesIndices(tile, outVertsIndices);
		}

		// Token: 0x0600E01F RID: 57375 RVA: 0x00535D03 File Offset: 0x00533F03
		public void GetTileNeighbors(PlanetTile tile, List<PlanetTile> outNeighbors)
		{
			tile.Layer.GetTileNeighbors(tile, outNeighbors);
		}

		// Token: 0x0600E020 RID: 57376 RVA: 0x00535D13 File Offset: 0x00533F13
		public int GetTileNeighborCount(PlanetTile tile)
		{
			return tile.Layer.GetTileNeighborCount(tile);
		}

		// Token: 0x0600E021 RID: 57377 RVA: 0x00535D22 File Offset: 0x00533F22
		public int GetMaxTileNeighborCountEver(PlanetTile tile)
		{
			return tile.Layer.GetMaxTileNeighborCountEver(tile);
		}

		// Token: 0x0600E022 RID: 57378 RVA: 0x00535D31 File Offset: 0x00533F31
		public bool IsNeighbor(PlanetTile tileA, PlanetTile tileB)
		{
			return tileA.Layer.IsNeighbor(tileA, tileB);
		}

		// Token: 0x0600E023 RID: 57379 RVA: 0x00535D41 File Offset: 0x00533F41
		public bool IsNeighborOrSame(PlanetTile tileA, PlanetTile tileB)
		{
			return tileA.Layer.IsNeighborOrSame(tileA, tileB);
		}

		// Token: 0x0600E024 RID: 57380 RVA: 0x00535D51 File Offset: 0x00533F51
		public int GetNeighborId(PlanetTile tileA, PlanetTile tileB)
		{
			return tileA.Layer.GetNeighborId(tileA, tileB);
		}

		// Token: 0x0600E025 RID: 57381 RVA: 0x00535D61 File Offset: 0x00533F61
		public PlanetTile GetTileNeighbor(PlanetTile tile, int adjacentId)
		{
			return tile.Layer.GetTileNeighbor(tile, adjacentId);
		}

		// Token: 0x0600E026 RID: 57382 RVA: 0x00535D71 File Offset: 0x00533F71
		public float TileRadiusToAngle(float radius)
		{
			return this.TileRadiusToAngle(this.surface, radius);
		}

		// Token: 0x0600E027 RID: 57383 RVA: 0x00535D80 File Offset: 0x00533F80
		public float TileRadiusToAngle(PlanetLayer layer, float radius)
		{
			return layer.TileRadiusToAngle(radius);
		}

		// Token: 0x0600E028 RID: 57384 RVA: 0x00535D89 File Offset: 0x00533F89
		public float DistOnSurfaceToAngle(float dist)
		{
			return this.DistOnSurfaceToAngle(this.surface, dist);
		}

		// Token: 0x0600E029 RID: 57385 RVA: 0x00535D98 File Offset: 0x00533F98
		public float DistOnSurfaceToAngle(PlanetLayer layer, float dist)
		{
			return layer.DistOnSurfaceToAngle(dist);
		}

		// Token: 0x0600E02A RID: 57386 RVA: 0x00535DA1 File Offset: 0x00533FA1
		public float DistanceFromEquatorNormalized(PlanetTile tile)
		{
			return tile.Layer.DistanceFromEquatorNormalized(tile);
		}

		// Token: 0x0600E02B RID: 57387 RVA: 0x00535DB0 File Offset: 0x00533FB0
		public float ApproxDistanceInTiles(float sphericalDistance)
		{
			return this.ApproxDistanceInTiles(this.surface, sphericalDistance);
		}

		// Token: 0x0600E02C RID: 57388 RVA: 0x00535DBF File Offset: 0x00533FBF
		public float ApproxDistanceInTiles(PlanetLayer layer, float sphericalDistance)
		{
			return layer.ApproxDistanceInTiles(sphericalDistance);
		}

		// Token: 0x0600E02D RID: 57389 RVA: 0x00535DC8 File Offset: 0x00533FC8
		public float ApproxDistanceInTiles(PlanetTile tileA, PlanetTile tileB)
		{
			return tileA.Layer.ApproxDistanceInTiles(tileA, tileB);
		}

		// Token: 0x0600E02E RID: 57390 RVA: 0x00535DD8 File Offset: 0x00533FD8
		public void OverlayRoad(PlanetTile fromTile, PlanetTile toTile, RoadDef roadDef)
		{
			if (roadDef == null)
			{
				Log.ErrorOnce("Attempted to remove road with overlayRoad; not supported", 90292249);
				return;
			}
			RoadDef roadDef2 = this.GetRoadDef(fromTile, toTile, false);
			if (roadDef2 == roadDef)
			{
				return;
			}
			object obj = this[fromTile];
			Tile obj2 = this[toTile];
			SurfaceTile surfaceTile;
			SurfaceTile surfaceTile2;
			if (obj.Isnt(out surfaceTile) || obj2.Isnt(out surfaceTile2))
			{
				return;
			}
			if (roadDef2 != null)
			{
				if (roadDef2.priority >= roadDef.priority)
				{
					return;
				}
				surfaceTile.potentialRoads.RemoveAll((SurfaceTile.RoadLink rl) => rl.neighbor == toTile);
				surfaceTile2.potentialRoads.RemoveAll((SurfaceTile.RoadLink rl) => rl.neighbor == fromTile);
			}
			if (surfaceTile.potentialRoads == null)
			{
				surfaceTile.potentialRoads = new List<SurfaceTile.RoadLink>();
			}
			if (surfaceTile2.potentialRoads == null)
			{
				surfaceTile2.potentialRoads = new List<SurfaceTile.RoadLink>();
			}
			surfaceTile.potentialRoads.Add(new SurfaceTile.RoadLink
			{
				neighbor = toTile,
				road = roadDef
			});
			surfaceTile2.potentialRoads.Add(new SurfaceTile.RoadLink
			{
				neighbor = fromTile,
				road = roadDef
			});
		}

		// Token: 0x0600E02F RID: 57391 RVA: 0x00535F14 File Offset: 0x00534114
		public RoadDef GetRoadDef(PlanetTile fromTile, PlanetTile toTile, bool visibleOnly = true)
		{
			if (!this.IsNeighbor(fromTile, toTile))
			{
				Log.ErrorOnce("Tried to find road information between non-neighboring tiles", 12390444);
				return null;
			}
			SurfaceTile surfaceTile;
			if (this[fromTile].Isnt(out surfaceTile))
			{
				return null;
			}
			List<SurfaceTile.RoadLink> list = visibleOnly ? surfaceTile.Roads : surfaceTile.potentialRoads;
			if (list == null)
			{
				return null;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].neighbor == toTile)
				{
					return list[i].road;
				}
			}
			return null;
		}

		// Token: 0x0600E030 RID: 57392 RVA: 0x00535F98 File Offset: 0x00534198
		public void OverlayRiver(PlanetTile fromTile, PlanetTile toTile, RiverDef riverDef)
		{
			if (riverDef == null)
			{
				Log.ErrorOnce("Attempted to remove river with overlayRiver; not supported", 90292250);
				return;
			}
			RiverDef riverDef2 = this.GetRiverDef(fromTile, toTile, false);
			if (riverDef2 == riverDef)
			{
				return;
			}
			object obj = this[fromTile];
			Tile obj2 = this[toTile];
			SurfaceTile surfaceTile;
			SurfaceTile surfaceTile2;
			if (obj.Isnt(out surfaceTile) || obj2.Isnt(out surfaceTile2))
			{
				return;
			}
			if (riverDef2 != null)
			{
				if (riverDef2.degradeThreshold >= riverDef.degradeThreshold)
				{
					return;
				}
				surfaceTile.potentialRivers.RemoveAll((SurfaceTile.RiverLink rl) => rl.neighbor == toTile);
				surfaceTile2.potentialRivers.RemoveAll((SurfaceTile.RiverLink rl) => rl.neighbor == fromTile);
			}
			SurfaceTile surfaceTile3 = surfaceTile;
			if (surfaceTile3.potentialRivers == null)
			{
				surfaceTile3.potentialRivers = new List<SurfaceTile.RiverLink>();
			}
			surfaceTile3 = surfaceTile2;
			if (surfaceTile3.potentialRivers == null)
			{
				surfaceTile3.potentialRivers = new List<SurfaceTile.RiverLink>();
			}
			surfaceTile.potentialRivers.Add(new SurfaceTile.RiverLink
			{
				neighbor = toTile,
				river = riverDef
			});
			surfaceTile2.potentialRivers.Add(new SurfaceTile.RiverLink
			{
				neighbor = fromTile,
				river = riverDef
			});
			surfaceTile2.riverDist = Mathf.Max(surfaceTile2.riverDist, surfaceTile.riverDist + 1);
		}

		// Token: 0x0600E031 RID: 57393 RVA: 0x005360F8 File Offset: 0x005342F8
		public RiverDef GetRiverDef(PlanetTile fromTile, PlanetTile toTile, bool visibleOnly = true)
		{
			if (!this.IsNeighbor(fromTile, toTile))
			{
				Log.ErrorOnce("Tried to find river information between non-neighboring tiles", 12390444);
				return null;
			}
			SurfaceTile surfaceTile = (SurfaceTile)this[fromTile];
			List<SurfaceTile.RiverLink> list = visibleOnly ? surfaceTile.Rivers : surfaceTile.potentialRivers;
			if (list == null)
			{
				return null;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].neighbor == toTile)
				{
					return list[i].river;
				}
			}
			return null;
		}

		// Token: 0x0600E032 RID: 57394 RVA: 0x00536178 File Offset: 0x00534378
		public float GetRoadMovementDifficultyMultiplier(PlanetTile fromTile, PlanetTile toTile, StringBuilder explanation = null)
		{
			List<SurfaceTile.RoadLink> roads = ((SurfaceTile)this[fromTile]).Roads;
			if (roads == null)
			{
				return 1f;
			}
			if (!toTile.Valid)
			{
				toTile = this.FindMostReasonableAdjacentTileForDisplayedPathCost(fromTile);
			}
			for (int i = 0; i < roads.Count; i++)
			{
				if (roads[i].neighbor == toTile)
				{
					float movementCostMultiplier = roads[i].road.movementCostMultiplier;
					if (explanation != null)
					{
						if (explanation.Length > 0)
						{
							explanation.AppendLine();
						}
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
						defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(roads[i].road.LabelCap);
						defaultInterpolatedStringHandler.AppendLiteral(": ");
						defaultInterpolatedStringHandler.AppendFormatted(movementCostMultiplier.ToStringPercent());
						explanation.Append(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					return movementCostMultiplier;
				}
			}
			return 1f;
		}

		// Token: 0x0600E033 RID: 57395 RVA: 0x00536254 File Offset: 0x00534454
		public PlanetTile FindMostReasonableAdjacentTileForDisplayedPathCost(PlanetTile fromTile)
		{
			SurfaceTile surfaceTile = (SurfaceTile)this[fromTile];
			float num = 1f;
			PlanetTile planetTile = PlanetTile.Invalid;
			List<SurfaceTile.RoadLink> roads = surfaceTile.Roads;
			if (roads != null)
			{
				for (int i = 0; i < roads.Count; i++)
				{
					float movementCostMultiplier = roads[i].road.movementCostMultiplier;
					if (movementCostMultiplier < num && !Find.World.Impassable(roads[i].neighbor))
					{
						num = movementCostMultiplier;
						planetTile = roads[i].neighbor;
					}
				}
			}
			if (planetTile != PlanetTile.Invalid)
			{
				return planetTile;
			}
			this.tmpNeighbors.Clear();
			this.GetTileNeighbors(fromTile, this.tmpNeighbors);
			for (int j = 0; j < this.tmpNeighbors.Count; j++)
			{
				if (!Find.World.Impassable(this.tmpNeighbors[j]))
				{
					return this.tmpNeighbors[j];
				}
			}
			return fromTile;
		}

		// Token: 0x0600E034 RID: 57396 RVA: 0x0053633C File Offset: 0x0053453C
		public int TraversalDistanceBetween(PlanetTile start, PlanetTile end, bool passImpassable = true, int maxDist = 2147483647, bool canTraverseLayers = false)
		{
			if (start == end)
			{
				return 0;
			}
			if (!start.Valid || !end.Valid)
			{
				return int.MaxValue;
			}
			PlanetTile planetTile = start;
			if (((this.cachedTraversalDistanceForStart == start && this.cachedTraversalDistanceForEnd == end && this.cachedLayer == end.Layer) & passImpassable) && maxDist == 2147483647)
			{
				return this.cachedTraversalDistance;
			}
			if (start.Layer != end.Layer)
			{
				if (!canTraverseLayers)
				{
					return int.MaxValue;
				}
				this.cachedLayer = end.Layer;
				start = end.Layer.GetClosestTile_NewTemp(start, false);
			}
			if (!passImpassable && !Find.WorldReachability.CanReach(start, end))
			{
				return int.MaxValue;
			}
			int finalDist = int.MaxValue;
			int maxTilesToProcess = (maxDist == int.MaxValue) ? int.MaxValue : this.TilesNumWithinTraversalDistance(maxDist + 1);
			start.Layer.Filler.FloodFill(start, (PlanetTile x) => passImpassable || !Find.World.Impassable(x), delegate(PlanetTile tile, int dist)
			{
				if (tile == end)
				{
					finalDist = dist;
					return true;
				}
				return false;
			}, maxTilesToProcess, null);
			if (passImpassable && maxDist == 2147483647)
			{
				this.cachedTraversalDistance = finalDist;
				this.cachedTraversalDistanceForStart = planetTile;
				this.cachedTraversalDistanceForEnd = end;
			}
			return finalDist;
		}

		// Token: 0x0600E035 RID: 57397 RVA: 0x005364C7 File Offset: 0x005346C7
		public int TilesNumWithinTraversalDistance(int traversalDist)
		{
			if (traversalDist < 0)
			{
				return 0;
			}
			return 3 * traversalDist * (traversalDist + 1) + 1;
		}

		// Token: 0x0600E036 RID: 57398 RVA: 0x005364D8 File Offset: 0x005346D8
		public bool IsOnEdge(PlanetTile tileID)
		{
			return this.InBounds(tileID) && this.GetTileNeighborCount(tileID) < this.GetMaxTileNeighborCountEver(tileID);
		}

		// Token: 0x0600E037 RID: 57399 RVA: 0x005364F8 File Offset: 0x005346F8
		private void CalculateViewCenterAndAngle()
		{
			this.surfaceViewAngle = Find.World.PlanetCoverage * 180f;
			this.surfaceViewCenter = Vector3.back;
			float angle = 45f;
			if (this.surfaceViewAngle > 45f)
			{
				angle = Mathf.Max(90f - this.surfaceViewAngle, 0f);
			}
			this.surfaceViewCenter = Quaternion.AngleAxis(angle, Vector3.right) * this.surfaceViewCenter;
		}

		// Token: 0x0600E038 RID: 57400 RVA: 0x0053656C File Offset: 0x0053476C
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.nextLayerId, "nextLayerId", 0, false);
			Scribe_Collections.Look<int, PlanetLayer>(ref this.planetLayers, "layers", LookMode.Value, LookMode.Deep);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.EnsureNextLayerIDValid();
				this.CalculateViewCenterAndAngle();
				if (this.planetLayers.NullOrEmpty<int, PlanetLayer>())
				{
					this.planetLayers = new Dictionary<int, PlanetLayer>();
					this.CreateRequiredLayers();
					this.surface.ExposeBody();
					this.StandardizeTileData();
				}
				else
				{
					foreach (KeyValuePair<int, PlanetLayer> keyValuePair in this.planetLayers)
					{
						int num;
						PlanetLayer planetLayer;
						keyValuePair.Deconstruct(out num, out planetLayer);
						int item = num;
						PlanetLayer planetLayer2 = planetLayer;
						if (planetLayer2.Def == null)
						{
							WorldGrid.tmpLayerIds.Add(item);
						}
						else
						{
							if (planetLayer2.IsRootSurface)
							{
								this.surface = (SurfaceLayer)planetLayer2;
							}
							if (ModsConfig.OdysseyActive && planetLayer2.Def == PlanetLayerDefOf.Orbit)
							{
								this.orbit = planetLayer2;
							}
						}
					}
					if (this.surface == null)
					{
						foreach (KeyValuePair<int, PlanetLayer> keyValuePair in this.planetLayers)
						{
							int num;
							PlanetLayer planetLayer;
							keyValuePair.Deconstruct(out num, out planetLayer);
							PlanetLayer planetLayer3 = planetLayer;
							if (planetLayer3.Def == PlanetLayerDefOf.Surface)
							{
								this.surface = (SurfaceLayer)planetLayer3;
								this.surface.MarkRootSurface();
								break;
							}
						}
					}
					foreach (int key in WorldGrid.tmpLayerIds)
					{
						this.planetLayers.Remove(key);
					}
					WorldGrid.tmpLayerIds.Clear();
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.CreateRequiredLayers();
				this.StandardizeTileData();
			}
		}

		// Token: 0x0600E039 RID: 57401 RVA: 0x00536760 File Offset: 0x00534960
		private void EnsureNextLayerIDValid()
		{
			if (this.planetLayers.NullOrEmpty<int, PlanetLayer>())
			{
				return;
			}
			foreach (KeyValuePair<int, PlanetLayer> keyValuePair in this.planetLayers)
			{
				int num;
				PlanetLayer planetLayer;
				keyValuePair.Deconstruct(out num, out planetLayer);
				int num2 = num;
				if (num2 >= this.nextLayerId)
				{
					this.nextLayerId = num2 + 1;
				}
			}
		}

		// Token: 0x0600E03A RID: 57402 RVA: 0x005367DC File Offset: 0x005349DC
		public void StandardizeTileData()
		{
			foreach (KeyValuePair<int, PlanetLayer> keyValuePair in this.planetLayers)
			{
				int num;
				PlanetLayer planetLayer;
				keyValuePair.Deconstruct(out num, out planetLayer);
				PlanetLayer planetLayer2 = planetLayer;
				if (planetLayer2.Tiles.Empty<Tile>())
				{
					planetLayer2.Standardize();
				}
			}
		}

		// Token: 0x0600E03B RID: 57403 RVA: 0x0053684C File Offset: 0x00534A4C
		public IEnumerable<Gizmo> GetGizmos()
		{
			WorldGrid.<GetGizmos>d__103 <GetGizmos>d__ = new WorldGrid.<GetGizmos>d__103(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600E03C RID: 57404 RVA: 0x0053685C File Offset: 0x00534A5C
		public void Dispose()
		{
			foreach (KeyValuePair<int, PlanetLayer> keyValuePair in this.planetLayers)
			{
				int num;
				PlanetLayer planetLayer;
				keyValuePair.Deconstruct(out num, out planetLayer);
				planetLayer.Dispose();
			}
		}

		// Token: 0x04008FC9 RID: 36809
		private Dictionary<int, PlanetLayer> planetLayers = new Dictionary<int, PlanetLayer>();

		// Token: 0x04008FCA RID: 36810
		private Vector3 surfaceViewCenter;

		// Token: 0x04008FCB RID: 36811
		private float surfaceViewAngle;

		// Token: 0x04008FCC RID: 36812
		private int nextLayerId;

		// Token: 0x04008FCD RID: 36813
		private SurfaceLayer surface;

		// Token: 0x04008FCE RID: 36814
		private PlanetLayer orbit;

		// Token: 0x04008FCF RID: 36815
		private readonly List<WorldDrawLayerBase> globalLayers = new List<WorldDrawLayerBase>();

		// Token: 0x04008FD0 RID: 36816
		private readonly List<PlanetTile> tmpNeighbors = new List<PlanetTile>();

		// Token: 0x04008FD1 RID: 36817
		public const int SubdivisionsCount = 10;

		// Token: 0x04008FD2 RID: 36818
		public const float PlanetRadius = 100f;

		// Token: 0x04008FD5 RID: 36821
		private int cachedTraversalDistance = -1;

		// Token: 0x04008FD6 RID: 36822
		private PlanetTile cachedTraversalDistanceForStart = PlanetTile.Invalid;

		// Token: 0x04008FD7 RID: 36823
		private PlanetTile cachedTraversalDistanceForEnd = PlanetTile.Invalid;

		// Token: 0x04008FD8 RID: 36824
		private PlanetLayer cachedLayer;

		// Token: 0x04008FD9 RID: 36825
		private static readonly List<int> tmpLayerIds = new List<int>();
	}
}
