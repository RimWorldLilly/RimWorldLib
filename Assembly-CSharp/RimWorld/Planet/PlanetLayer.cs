using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using LudeonTK;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002344 RID: 9028
	[BurstCompile]
	public class PlanetLayer : IExposable, ILoadReferenceable, IDisposable
	{
		// Token: 0x0600DF24 RID: 57124 RVA: 0x00530330 File Offset: 0x0052E530
		public string GetUniqueLoadID()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(12, 1);
			defaultInterpolatedStringHandler.AppendLiteral("PlanetLayer_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.layerId);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x17002728 RID: 10024
		// (get) Token: 0x0600DF25 RID: 57125 RVA: 0x00530367 File Offset: 0x0052E567
		public IReadOnlyDictionary<PlanetLayer, PlanetLayerConnection> Connections
		{
			get
			{
				return this.connections;
			}
		}

		// Token: 0x17002729 RID: 10025
		// (get) Token: 0x0600DF26 RID: 57126 RVA: 0x0053036F File Offset: 0x0052E56F
		public List<WorldDrawLayer> WorldDrawLayers { get; } = new List<WorldDrawLayer>();

		// Token: 0x1700272A RID: 10026
		// (get) Token: 0x0600DF27 RID: 57127 RVA: 0x00530377 File Offset: 0x0052E577
		public FastTileFinder FastTileFinder
		{
			get
			{
				return this.finder;
			}
		}

		// Token: 0x1700272B RID: 10027
		// (get) Token: 0x0600DF28 RID: 57128 RVA: 0x0053037F File Offset: 0x0052E57F
		public WorldPathing Pather
		{
			get
			{
				return this.pather;
			}
		}

		// Token: 0x1700272C RID: 10028
		// (get) Token: 0x0600DF29 RID: 57129 RVA: 0x00530387 File Offset: 0x0052E587
		public WorldFloodFiller Filler
		{
			get
			{
				return this.filler;
			}
		}

		// Token: 0x1700272D RID: 10029
		// (get) Token: 0x0600DF2A RID: 57130 RVA: 0x0053038F File Offset: 0x0052E58F
		public virtual bool Visible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700272E RID: 10030
		// (get) Token: 0x0600DF2B RID: 57131 RVA: 0x00530392 File Offset: 0x0052E592
		public bool IsSelected
		{
			get
			{
				return PlanetLayer.Selected == this;
			}
		}

		// Token: 0x1700272F RID: 10031
		// (get) Token: 0x0600DF2C RID: 57132 RVA: 0x0053039C File Offset: 0x0052E59C
		public int LayerID
		{
			get
			{
				return this.layerId;
			}
		}

		// Token: 0x17002730 RID: 10032
		// (get) Token: 0x0600DF2D RID: 57133 RVA: 0x005303A4 File Offset: 0x0052E5A4
		public PlanetLayerDef Def
		{
			get
			{
				return this.def;
			}
		}

		// Token: 0x17002731 RID: 10033
		// (get) Token: 0x0600DF2E RID: 57134 RVA: 0x005303AC File Offset: 0x0052E5AC
		public List<Tile> Tiles
		{
			get
			{
				return this.tiles;
			}
		}

		// Token: 0x17002732 RID: 10034
		// (get) Token: 0x0600DF2F RID: 57135 RVA: 0x005303B4 File Offset: 0x0052E5B4
		public int TilesCount
		{
			get
			{
				return this.tileIDToNeighbors_offsets.Length;
			}
		}

		// Token: 0x17002733 RID: 10035
		// (get) Token: 0x0600DF30 RID: 57136 RVA: 0x005303C1 File Offset: 0x0052E5C1
		public bool IsRootSurface
		{
			get
			{
				return this.isRootSurface;
			}
		}

		// Token: 0x17002734 RID: 10036
		// (get) Token: 0x0600DF31 RID: 57137 RVA: 0x005303C9 File Offset: 0x0052E5C9
		public NativeArray<Vector3> UnsafeVerts
		{
			get
			{
				return this.verts;
			}
		}

		// Token: 0x17002735 RID: 10037
		// (get) Token: 0x0600DF32 RID: 57138 RVA: 0x005303D1 File Offset: 0x0052E5D1
		public NativeArray<int> UnsafeTileIDToVerts_offsets
		{
			get
			{
				return this.tileIDToVerts_offsets;
			}
		}

		// Token: 0x17002736 RID: 10038
		// (get) Token: 0x0600DF33 RID: 57139 RVA: 0x005303D9 File Offset: 0x0052E5D9
		public NativeArray<int> UnsafeTileIDToNeighbors_offsets
		{
			get
			{
				return this.tileIDToNeighbors_offsets;
			}
		}

		// Token: 0x17002737 RID: 10039
		// (get) Token: 0x0600DF34 RID: 57140 RVA: 0x005303E1 File Offset: 0x0052E5E1
		public NativeArray<PlanetTile> UnsafeTileIDToNeighbors_values
		{
			get
			{
				return this.tileIDToNeighbors_values;
			}
		}

		// Token: 0x17002738 RID: 10040
		// (get) Token: 0x0600DF35 RID: 57141 RVA: 0x005303E9 File Offset: 0x0052E5E9
		public virtual Vector3 Origin
		{
			get
			{
				return this.origin;
			}
		}

		// Token: 0x17002739 RID: 10041
		// (get) Token: 0x0600DF36 RID: 57142 RVA: 0x005303F1 File Offset: 0x0052E5F1
		public Vector3 NorthPolePos
		{
			get
			{
				return new Vector3(0f, this.Radius, 0f);
			}
		}

		// Token: 0x1700273A RID: 10042
		// (get) Token: 0x0600DF37 RID: 57143 RVA: 0x00530408 File Offset: 0x0052E608
		public float AverageTileSize
		{
			get
			{
				return this.averageTileSize;
			}
		}

		// Token: 0x1700273B RID: 10043
		// (get) Token: 0x0600DF38 RID: 57144 RVA: 0x00530410 File Offset: 0x0052E610
		public float Radius
		{
			get
			{
				return this.radius;
			}
		}

		// Token: 0x1700273C RID: 10044
		// (get) Token: 0x0600DF39 RID: 57145 RVA: 0x00530418 File Offset: 0x0052E618
		public float ExtraCameraAltitude
		{
			get
			{
				return this.extraCameraAltitude;
			}
		}

		// Token: 0x1700273D RID: 10045
		// (get) Token: 0x0600DF3A RID: 57146 RVA: 0x00530420 File Offset: 0x0052E620
		public float BackgroundWorldCameraOffset
		{
			get
			{
				return this.backgroundWorldCameraOffset;
			}
		}

		// Token: 0x1700273E RID: 10046
		// (get) Token: 0x0600DF3B RID: 57147 RVA: 0x00530428 File Offset: 0x0052E628
		public float BackgroundWorldCameraParallaxDistancePer100Cells
		{
			get
			{
				return this.backgroundWorldCameraParallaxDistancePer100Cells;
			}
		}

		// Token: 0x1700273F RID: 10047
		// (get) Token: 0x0600DF3C RID: 57148 RVA: 0x00530430 File Offset: 0x0052E630
		public Vector3 ViewCenter
		{
			get
			{
				return this.viewCenter;
			}
		}

		// Token: 0x17002740 RID: 10048
		// (get) Token: 0x0600DF3D RID: 57149 RVA: 0x00530438 File Offset: 0x0052E638
		public float ViewAngle
		{
			get
			{
				return this.viewAngle;
			}
		}

		// Token: 0x17002741 RID: 10049
		public Tile this[int tileID]
		{
			get
			{
				if ((ulong)tileID >= (ulong)((long)this.TilesCount))
				{
					return null;
				}
				return this.tiles[tileID];
			}
		}

		// Token: 0x17002742 RID: 10050
		public Tile this[PlanetTile tile]
		{
			get
			{
				return this[tile.tileId];
			}
		}

		// Token: 0x0600DF40 RID: 57152 RVA: 0x00530469 File Offset: 0x0052E669
		public PlanetTile PlanetTileForID(int tileID)
		{
			return new PlanetTile(tileID, this.layerId);
		}

		// Token: 0x17002743 RID: 10051
		// (get) Token: 0x0600DF41 RID: 57153 RVA: 0x00530477 File Offset: 0x0052E677
		public virtual bool Raycastable
		{
			get
			{
				return this.def.alwaysRaycastable || PlanetLayer.Selected == this;
			}
		}

		// Token: 0x17002744 RID: 10052
		// (get) Token: 0x0600DF42 RID: 57154 RVA: 0x00530490 File Offset: 0x0052E690
		public virtual bool CanReachLayer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002745 RID: 10053
		// (get) Token: 0x0600DF43 RID: 57155 RVA: 0x00530493 File Offset: 0x0052E693
		// (set) Token: 0x0600DF44 RID: 57156 RVA: 0x0053049B File Offset: 0x0052E69B
		public string ScenarioTag
		{
			get
			{
				return this.scenarioTag;
			}
			set
			{
				this.scenarioTag = value;
			}
		}

		// Token: 0x17002746 RID: 10054
		// (get) Token: 0x0600DF45 RID: 57157 RVA: 0x005304A4 File Offset: 0x0052E6A4
		// (set) Token: 0x0600DF46 RID: 57158 RVA: 0x005304B0 File Offset: 0x0052E6B0
		public static PlanetLayer Selected
		{
			get
			{
				return Find.WorldSelector.SelectedLayer;
			}
			set
			{
				Find.WorldSelector.SelectedLayer = value;
			}
		}

		// Token: 0x0600DF47 RID: 57159 RVA: 0x005304BD File Offset: 0x0052E6BD
		public PlanetLayer()
		{
		}

		// Token: 0x0600DF48 RID: 57160 RVA: 0x005304F0 File Offset: 0x0052E6F0
		public PlanetLayer(int layerId, PlanetLayerDef def, float radius, Vector3 origin, Vector3 viewCenter, float viewAngle, int subdivisions, float extraCameraAltitude, float backgroundWorldCameraOffset, float backgroundWorldCameraParallaxDistancePer100Cells)
		{
			this.layerId = layerId;
			this.def = def;
			this.radius = radius;
			this.origin = origin;
			this.viewCenter = viewCenter;
			this.viewAngle = viewAngle;
			this.subdivisions = subdivisions;
			this.extraCameraAltitude = extraCameraAltitude;
			this.backgroundWorldCameraOffset = backgroundWorldCameraOffset;
			this.backgroundWorldCameraParallaxDistancePer100Cells = backgroundWorldCameraParallaxDistancePer100Cells;
		}

		// Token: 0x0600DF49 RID: 57161 RVA: 0x00530578 File Offset: 0x0052E778
		public void MarkRootSurface()
		{
			this.isRootSurface = true;
		}

		// Token: 0x0600DF4A RID: 57162 RVA: 0x00530584 File Offset: 0x0052E784
		public void AddConnection(PlanetLayer target, float cost)
		{
			if (this.connections.ContainsKey(target))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(60, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Attempted to add a layer connection (");
				defaultInterpolatedStringHandler.AppendFormatted<PlanetLayer>(target);
				defaultInterpolatedStringHandler.AppendLiteral(") which already exists.");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			this.connections[target] = new PlanetLayerConnection
			{
				origin = this,
				target = target,
				fuelCost = cost
			};
		}

		// Token: 0x0600DF4B RID: 57163 RVA: 0x005305FC File Offset: 0x0052E7FC
		public void RemoveConnection(PlanetLayer target)
		{
			this.connections.Remove(target);
		}

		// Token: 0x0600DF4C RID: 57164 RVA: 0x0053060B File Offset: 0x0052E80B
		public bool HasConnectionFromTo(PlanetLayer layer)
		{
			return this.connections.ContainsKey(layer);
		}

		// Token: 0x0600DF4D RID: 57165 RVA: 0x00530619 File Offset: 0x0052E819
		public PlanetLayerConnection GetConnectionFromTo(PlanetLayer layer)
		{
			return this.connections[layer];
		}

		// Token: 0x0600DF4E RID: 57166 RVA: 0x00530627 File Offset: 0x0052E827
		public bool TryGetConnectionFromTo(PlanetLayer layer, out PlanetLayerConnection connection)
		{
			return this.connections.TryGetValue(layer, out connection);
		}

		// Token: 0x0600DF4F RID: 57167 RVA: 0x00530636 File Offset: 0x0052E836
		public virtual AcceptanceReport CanSelectLayer()
		{
			return true;
		}

		// Token: 0x0600DF50 RID: 57168 RVA: 0x0053063E File Offset: 0x0052E83E
		public void RunWorldGeneration()
		{
			this.RunWorldGeneration(Find.World.info.seedString, Rand.Int);
		}

		// Token: 0x0600DF51 RID: 57169 RVA: 0x0053065A File Offset: 0x0052E85A
		public void RunWorldGeneration(int seed)
		{
			this.RunWorldGeneration(Find.World.info.seedString, seed);
		}

		// Token: 0x0600DF52 RID: 57170 RVA: 0x00530672 File Offset: 0x0052E872
		public void RunWorldGeneration(string seedString, int seed)
		{
			WorldGenerator.GeneratePlanetLayer(this, seedString, seed);
		}

		// Token: 0x0600DF53 RID: 57171 RVA: 0x0053067C File Offset: 0x0052E87C
		public void InitializeLayer()
		{
			PlanetLayer.tmp_tris.Clear();
			PlanetLayer.tmp_verts.Clear();
			this.Dispose();
			IcosahedronGenerator.GenerateIcosahedron(PlanetLayer.tmp_verts, PlanetLayer.tmp_tris, this.Radius, this.viewCenter, this.viewAngle);
			for (int i = 0; i < this.subdivisions + 1; i++)
			{
				bool lastPass = i == this.subdivisions;
				this.Subdivide(lastPass);
			}
			this.verts = NativeArrayUtility.GetNativeArrayCopy<Vector3>(PlanetLayer.tmp_generatedVerts, Allocator.Persistent, false);
			this.tileIDToVerts_offsets = NativeArrayUtility.GetNativeArrayCopy<int>(PlanetLayer.tmp_generatedVertOffsets, Allocator.Persistent, false);
			this.CalculateTileNeighbors();
			PlanetLayer.ClearAndDeallocateWorkingLists();
			this.averageTileSize = PlanetLayer.CalculateAverageTileSize(this.tileIDToNeighbors_offsets, this.tileIDToNeighbors_values, this.tileIDToVerts_offsets, this.verts);
			this.WorldDrawLayers.Clear();
			foreach (Type type in this.def.worldDrawLayers)
			{
				WorldDrawLayer worldDrawLayer = (WorldDrawLayer)Activator.CreateInstance(type);
				worldDrawLayer.Initialize(this);
				this.WorldDrawLayers.Add(worldDrawLayer);
			}
			this.finder = new FastTileFinder(this);
			this.pather = new WorldPathing(this);
			this.filler = new WorldFloodFiller(this);
		}

		// Token: 0x0600DF54 RID: 57172 RVA: 0x005307CC File Offset: 0x0052E9CC
		private void Subdivide(bool lastPass)
		{
			PackedListOfLists.GenerateVertToTrisPackedList(PlanetLayer.tmp_verts, PlanetLayer.tmp_tris, PlanetLayer.tmp_vertToTris_offsets, PlanetLayer.tmp_vertToTris_values);
			int count = PlanetLayer.tmp_verts.Count;
			int i = 0;
			int count2 = PlanetLayer.tmp_tris.Count;
			while (i < count2)
			{
				TriangleIndices triangleIndices = PlanetLayer.tmp_tris[i];
				Vector3 vector = (PlanetLayer.tmp_verts[triangleIndices.v1] + PlanetLayer.tmp_verts[triangleIndices.v2] + PlanetLayer.tmp_verts[triangleIndices.v3]) / 3f;
				PlanetLayer.tmp_verts.Add(vector.normalized * this.Radius);
				i++;
			}
			PlanetLayer.tmp_generatedTris.Clear();
			if (lastPass)
			{
				PlanetLayer.tmp_vertToTileIDs_offsets.Clear();
				PlanetLayer.tmp_vertToTileIDs_values.Clear();
				PlanetLayer.tmp_tileIDToVerts_offsets.Clear();
				PlanetLayer.tmp_tileIDToVerts_values.Clear();
				int j = 0;
				int count3 = PlanetLayer.tmp_verts.Count;
				while (j < count3)
				{
					PlanetLayer.tmp_vertToTileIDs_offsets.Add(PlanetLayer.tmp_vertToTileIDs_values.Count);
					if (j >= count)
					{
						for (int k = 0; k < 6; k++)
						{
							PlanetLayer.tmp_vertToTileIDs_values.Add(-1);
						}
					}
					j++;
				}
			}
			for (int l = 0; l < count; l++)
			{
				PackedListOfLists.GetList<int>(PlanetLayer.tmp_vertToTris_offsets, PlanetLayer.tmp_vertToTris_values, l, PlanetLayer.tmp_adjacentTris);
				int count4 = PlanetLayer.tmp_adjacentTris.Count;
				if (!lastPass)
				{
					for (int m = 0; m < count4; m++)
					{
						int num = PlanetLayer.tmp_adjacentTris[m];
						int v = count + num;
						int nextOrderedVertex = PlanetLayer.tmp_tris[num].GetNextOrderedVertex(l);
						int num2 = -1;
						for (int n = 0; n < count4; n++)
						{
							if (m != n)
							{
								TriangleIndices triangleIndices2 = PlanetLayer.tmp_tris[PlanetLayer.tmp_adjacentTris[n]];
								if (triangleIndices2.v1 == nextOrderedVertex || triangleIndices2.v2 == nextOrderedVertex || triangleIndices2.v3 == nextOrderedVertex)
								{
									num2 = PlanetLayer.tmp_adjacentTris[n];
									break;
								}
							}
						}
						if (num2 >= 0)
						{
							int v2 = count + num2;
							PlanetLayer.tmp_generatedTris.Add(new TriangleIndices(l, v2, v));
						}
					}
				}
				else if (count4 == 5 || count4 == 6)
				{
					int num3 = 0;
					int nextOrderedVertex2 = PlanetLayer.tmp_tris[PlanetLayer.tmp_adjacentTris[num3]].GetNextOrderedVertex(l);
					int num4 = num3;
					int currentTriangleVertex = nextOrderedVertex2;
					PlanetLayer.tmp_generatedTileVerts.Clear();
					for (int num5 = 0; num5 < count4; num5++)
					{
						int item = count + PlanetLayer.tmp_adjacentTris[num4];
						PlanetLayer.tmp_generatedTileVerts.Add(item);
						int nextAdjacentTriangle = this.GetNextAdjacentTriangle(num4, currentTriangleVertex);
						int nextOrderedVertex3 = PlanetLayer.tmp_tris[PlanetLayer.tmp_adjacentTris[nextAdjacentTriangle]].GetNextOrderedVertex(l);
						num4 = nextAdjacentTriangle;
						currentTriangleVertex = nextOrderedVertex3;
					}
					this.FinalizeGeneratedTile(PlanetLayer.tmp_generatedTileVerts);
				}
			}
			PlanetLayer.tmp_tris.Clear();
			PlanetLayer.tmp_tris.AddRange(PlanetLayer.tmp_generatedTris);
		}

		// Token: 0x0600DF55 RID: 57173 RVA: 0x00530AD8 File Offset: 0x0052ECD8
		private void FinalizeGeneratedTile(List<int> generatedTileVertsList)
		{
			if ((generatedTileVertsList.Count != 5 && generatedTileVertsList.Count != 6) || generatedTileVertsList.Count > 6)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(98, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Planet shape generation internal error: generated a tile with ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(generatedTileVertsList.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" vertices. Only 5 and 6 are allowed.");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			if (this.ShouldDiscardGeneratedTile(generatedTileVertsList))
			{
				return;
			}
			int count = PlanetLayer.tmp_generatedVertOffsets.Count;
			PlanetLayer.tmp_generatedVertOffsets.Add(PlanetLayer.tmp_generatedVerts.Count);
			int i = 0;
			int count2 = generatedTileVertsList.Count;
			while (i < count2)
			{
				int index = generatedTileVertsList[i];
				PlanetLayer.tmp_generatedVerts.Add(PlanetLayer.tmp_verts[index]);
				PlanetLayer.tmp_vertToTileIDs_values[PlanetLayer.tmp_vertToTileIDs_values.IndexOf(-1, PlanetLayer.tmp_vertToTileIDs_offsets[index])] = count;
				i++;
			}
			PackedListOfLists.AddList<int>(PlanetLayer.tmp_tileIDToVerts_offsets, PlanetLayer.tmp_tileIDToVerts_values, generatedTileVertsList);
		}

		// Token: 0x0600DF56 RID: 57174 RVA: 0x00530BCC File Offset: 0x0052EDCC
		private bool ShouldDiscardGeneratedTile(List<int> generatedTileVertsList)
		{
			Vector3 a = Vector3.zero;
			int i = 0;
			int count = generatedTileVertsList.Count;
			while (i < count)
			{
				a += PlanetLayer.tmp_verts[generatedTileVertsList[i]];
				i++;
			}
			return !MeshUtility.VisibleForWorldgen(a / (float)generatedTileVertsList.Count, this.Radius, this.viewCenter, this.viewAngle);
		}

		// Token: 0x0600DF57 RID: 57175 RVA: 0x00530C34 File Offset: 0x0052EE34
		private void CalculateTileNeighbors()
		{
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			List<PlanetTile> list3 = new List<PlanetTile>();
			int i = 0;
			int count = PlanetLayer.tmp_tileIDToVerts_offsets.Count;
			while (i < count)
			{
				PlanetLayer.tmp_neighborsToAdd.Clear();
				PackedListOfLists.GetList<int>(PlanetLayer.tmp_tileIDToVerts_offsets, PlanetLayer.tmp_tileIDToVerts_values, i, PlanetLayer.tmp_neighbourVerts);
				int j = 0;
				int count2 = PlanetLayer.tmp_neighbourVerts.Count;
				while (j < count2)
				{
					PackedListOfLists.GetList<int>(PlanetLayer.tmp_vertToTileIDs_offsets, PlanetLayer.tmp_vertToTileIDs_values, PlanetLayer.tmp_neighbourVerts[j], PlanetLayer.tmp_tileIDs);
					PackedListOfLists.GetList<int>(PlanetLayer.tmp_vertToTileIDs_offsets, PlanetLayer.tmp_vertToTileIDs_values, PlanetLayer.tmp_neighbourVerts[(j + 1) % PlanetLayer.tmp_neighbourVerts.Count], list);
					int k = 0;
					int count3 = PlanetLayer.tmp_tileIDs.Count;
					while (k < count3)
					{
						int num = PlanetLayer.tmp_tileIDs[k];
						if (num != i && num != -1 && list.Contains(num))
						{
							PlanetLayer.tmp_neighborsToAdd.Add(new PlanetTile(num, this.layerId));
						}
						k++;
					}
					j++;
				}
				PackedListOfLists.AddList<PlanetTile>(list2, list3, PlanetLayer.tmp_neighborsToAdd);
				i++;
			}
			this.tileIDToNeighbors_offsets = NativeArrayUtility.GetNativeArrayCopy<int>(list2, Allocator.Persistent, false);
			this.tileIDToNeighbors_values = NativeArrayUtility.GetNativeArrayCopy<PlanetTile>(list3, Allocator.Persistent, false);
		}

		// Token: 0x0600DF58 RID: 57176 RVA: 0x00530D7C File Offset: 0x0052EF7C
		private int GetNextAdjacentTriangle(int currentAdjTriangleIndex, int currentTriangleVertex)
		{
			int i = 0;
			int count = PlanetLayer.tmp_adjacentTris.Count;
			while (i < count)
			{
				if (currentAdjTriangleIndex != i)
				{
					TriangleIndices triangleIndices = PlanetLayer.tmp_tris[PlanetLayer.tmp_adjacentTris[i]];
					if (triangleIndices.v1 == currentTriangleVertex || triangleIndices.v2 == currentTriangleVertex || triangleIndices.v3 == currentTriangleVertex)
					{
						return i;
					}
				}
				i++;
			}
			Log.Error("Planet shape generation internal error: could not find next adjacent triangle.");
			return -1;
		}

		// Token: 0x0600DF59 RID: 57177 RVA: 0x00530DE4 File Offset: 0x0052EFE4
		[BurstCompile(CompileSynchronously = true)]
		private static float CalculateAverageTileSize(in NativeArray<int> tileIDToNeighbors_offsets, in NativeArray<PlanetTile> tileIDToNeighbors_values, in NativeArray<int> tileIDToVerts_offsets, in NativeArray<Vector3> verts)
		{
			NativeArray<int> nativeArray = tileIDToNeighbors_offsets;
			if (nativeArray.Length == 0)
			{
				return 1f;
			}
			nativeArray = tileIDToNeighbors_offsets;
			int length = nativeArray.Length;
			double num = 0.0;
			int num2 = 0;
			for (int i = 0; i < length; i++)
			{
				Vector3 a;
				PlanetLayer.IntGetTileCenter(tileIDToVerts_offsets, verts, i, out a);
				ValueTuple<int, int> listIndexes = PackedListOfLists.GetListIndexes<PlanetTile>(tileIDToNeighbors_offsets, tileIDToNeighbors_values, i);
				int item = listIndexes.Item1;
				int item2 = listIndexes.Item2;
				for (int j = item; j < item2; j++)
				{
					NativeArray<PlanetTile> nativeArray2 = tileIDToNeighbors_values;
					int tileId = nativeArray2[j].tileId;
					Vector3 b;
					PlanetLayer.IntGetTileCenter(tileIDToVerts_offsets, verts, tileId, out b);
					num += (double)Vector3.Distance(a, b);
					num2++;
				}
			}
			return (float)(num / (double)num2);
		}

		// Token: 0x0600DF5A RID: 57178 RVA: 0x00530EAA File Offset: 0x0052F0AA
		public float GetTileSize(PlanetTile tile)
		{
			return PlanetLayer.IntGetTileSize(this.tileIDToNeighbors_offsets, this.tileIDToNeighbors_values, this.tileIDToVerts_offsets, this.verts, tile);
		}

		// Token: 0x0600DF5B RID: 57179 RVA: 0x00530ECC File Offset: 0x0052F0CC
		[BurstCompile]
		private static float IntGetTileSize(in NativeArray<int> tileIDToNeighbors_offsets, in NativeArray<PlanetTile> tileIDToNeighbors_values, in NativeArray<int> tileIDToVerts_offsets, in NativeArray<Vector3> verts, in PlanetTile tile)
		{
			double num = 0.0;
			int num2 = 0;
			Vector3 a;
			PlanetLayer.IntGetTileCenter(tileIDToVerts_offsets, verts, tile, out a);
			int num3 = tile + 1;
			NativeArray<int> nativeArray = tileIDToNeighbors_offsets;
			int num4;
			if (num3 >= nativeArray.Length)
			{
				NativeArray<PlanetTile> nativeArray2 = tileIDToNeighbors_values;
				num4 = nativeArray2.Length;
			}
			else
			{
				nativeArray = tileIDToNeighbors_offsets;
				num4 = nativeArray[tile + 1];
			}
			int num5 = num4;
			nativeArray = tileIDToNeighbors_offsets;
			for (int i = nativeArray[tile]; i < num5; i++)
			{
				NativeArray<PlanetTile> nativeArray2 = tileIDToNeighbors_values;
				int tileId = nativeArray2[i].tileId;
				Vector3 b;
				PlanetLayer.IntGetTileCenter(tileIDToVerts_offsets, verts, tileId, out b);
				num += (double)Vector3.Distance(a, b);
				num2++;
			}
			return (float)(num / (double)num2);
		}

		// Token: 0x0600DF5C RID: 57180 RVA: 0x00530FAC File Offset: 0x0052F1AC
		public Vector3 GetTileCenter(PlanetTile tile)
		{
			if (tile.Layer != this)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(92, 4);
				defaultInterpolatedStringHandler.AppendLiteral("Attempted to get the center of a tile that is not the same planet layer (");
				defaultInterpolatedStringHandler.AppendFormatted(this.def.label);
				defaultInterpolatedStringHandler.AppendLiteral(", ID: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.layerId);
				defaultInterpolatedStringHandler.AppendLiteral("): ");
				defaultInterpolatedStringHandler.AppendFormatted<PlanetTile>(tile);
				defaultInterpolatedStringHandler.AppendLiteral(" (layer: ");
				defaultInterpolatedStringHandler.AppendFormatted(tile.LayerDef.label);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return Vector3.zero;
			}
			return this.GetTileCenter(tile.tileId);
		}

		// Token: 0x0600DF5D RID: 57181 RVA: 0x00531064 File Offset: 0x0052F264
		public Vector3 GetTileCenter(int tileID)
		{
			if (tileID < 0 || tileID >= this.TilesCount)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(69, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Attempted to access a tile with ID ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(tileID);
				defaultInterpolatedStringHandler.AppendLiteral(", but it is out of range (count: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.TilesCount);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return Vector3.zero;
			}
			Vector3 result;
			PlanetLayer.IntGetTileCenter(this.tileIDToVerts_offsets, this.verts, tileID, out result);
			return result;
		}

		// Token: 0x0600DF5E RID: 57182 RVA: 0x005310E8 File Offset: 0x0052F2E8
		[BurstCompile]
		private static void IntGetTileCenter(in NativeArray<int> tileIDToVerts_offsets, in NativeArray<Vector3> verts, int tileID, out Vector3 center)
		{
			if (tileID >= 0)
			{
				NativeArray<int> nativeArray = tileIDToVerts_offsets;
				if (tileID < nativeArray.Length)
				{
					nativeArray = tileIDToVerts_offsets;
					if (!nativeArray.IsCreated)
					{
						center = Vector3.zero;
						return;
					}
					ValueTuple<int, int> listIndexes = PackedListOfLists.GetListIndexes<Vector3>(tileIDToVerts_offsets, verts, tileID);
					int item = listIndexes.Item1;
					int item2 = listIndexes.Item2;
					Vector3 vector = Vector3.zero;
					int num = 0;
					for (int i = item; i < item2; i++)
					{
						Vector3 a = vector;
						NativeArray<Vector3> nativeArray2 = verts;
						vector = a + nativeArray2[i];
						num++;
					}
					center = vector / (float)num;
					return;
				}
			}
			center = Vector3.zero;
		}

		// Token: 0x0600DF5F RID: 57183 RVA: 0x00531195 File Offset: 0x0052F395
		public float GetHeadingFromTo(PlanetTile fromTile, PlanetTile toTile)
		{
			return this.GetHeadingFromTo(fromTile.tileId, toTile.tileId);
		}

		// Token: 0x0600DF60 RID: 57184 RVA: 0x005311AC File Offset: 0x0052F3AC
		public float GetHeadingFromTo(int fromTileID, int toTileID)
		{
			if (fromTileID == toTileID)
			{
				return 0f;
			}
			Vector3 tileCenter = this.GetTileCenter(fromTileID);
			Vector3 tileCenter2 = this.GetTileCenter(toTileID);
			return this.GetHeadingFromTo(tileCenter, tileCenter2);
		}

		// Token: 0x0600DF61 RID: 57185 RVA: 0x005311DC File Offset: 0x0052F3DC
		public float GetHeadingFromTo(Vector3 from, Vector3 to)
		{
			if (from == to)
			{
				return 0f;
			}
			Vector3 northPolePos = this.NorthPolePos;
			Vector3 from2;
			Vector3 rhs;
			WorldRendererUtility.GetTangentialVectorFacing(from, northPolePos, out from2, out rhs);
			Vector3 vector;
			Vector3 vector2;
			WorldRendererUtility.GetTangentialVectorFacing(from, to, out vector, out vector2);
			float num = Vector3.Angle(from2, vector);
			if (Vector3.Dot(vector, rhs) < 0f)
			{
				num = 360f - num;
			}
			return num;
		}

		// Token: 0x0600DF62 RID: 57186 RVA: 0x00531238 File Offset: 0x0052F438
		public bool InBounds(PlanetTile tile)
		{
			return this.InBounds(tile.tileId);
		}

		// Token: 0x0600DF63 RID: 57187 RVA: 0x00531246 File Offset: 0x0052F446
		public bool InBounds(int tileID)
		{
			return (ulong)tileID < (ulong)((long)this.TilesCount);
		}

		// Token: 0x0600DF64 RID: 57188 RVA: 0x00531253 File Offset: 0x0052F453
		public Vector2 LongLatOf(PlanetTile tile)
		{
			return this.LongLatOf(tile.tileId);
		}

		// Token: 0x0600DF65 RID: 57189 RVA: 0x00531264 File Offset: 0x0052F464
		public Vector2 LongLatOf(int tileID)
		{
			Vector3 tileCenter = this.GetTileCenter(tileID);
			float x = Mathf.Atan2(tileCenter.x, -tileCenter.z) * 57.29578f;
			float y = Mathf.Asin(tileCenter.y / this.Radius) * 57.29578f;
			return new Vector2(x, y);
		}

		// Token: 0x0600DF66 RID: 57190 RVA: 0x005312B0 File Offset: 0x0052F4B0
		public Direction8Way GetDirection8WayFromTo(PlanetTile fromTile, PlanetTile toTile)
		{
			return this.GetDirection8WayFromTo(fromTile.tileId, toTile.tileId);
		}

		// Token: 0x0600DF67 RID: 57191 RVA: 0x005312C4 File Offset: 0x0052F4C4
		public Direction8Way GetDirection8WayFromTo(int fromTileID, int toTileID)
		{
			float headingFromTo = this.GetHeadingFromTo(fromTileID, toTileID);
			if (headingFromTo >= 337.5f || headingFromTo < 22.5f)
			{
				return Direction8Way.North;
			}
			if (headingFromTo < 67.5f)
			{
				return Direction8Way.NorthEast;
			}
			if (headingFromTo < 112.5f)
			{
				return Direction8Way.East;
			}
			if (headingFromTo < 157.5f)
			{
				return Direction8Way.SouthEast;
			}
			if (headingFromTo < 202.5f)
			{
				return Direction8Way.South;
			}
			if (headingFromTo < 247.5f)
			{
				return Direction8Way.SouthWest;
			}
			if (headingFromTo < 292.5f)
			{
				return Direction8Way.West;
			}
			return Direction8Way.NorthWest;
		}

		// Token: 0x0600DF68 RID: 57192 RVA: 0x00531329 File Offset: 0x0052F529
		public Rot4 GetRotFromTo(PlanetTile fromTile, PlanetTile toTile)
		{
			return this.GetRotFromTo(fromTile.tileId, toTile.tileId);
		}

		// Token: 0x0600DF69 RID: 57193 RVA: 0x00531340 File Offset: 0x0052F540
		public Rot4 GetRotFromTo(int fromTileID, int toTileID)
		{
			float headingFromTo = this.GetHeadingFromTo(fromTileID, toTileID);
			if (headingFromTo >= 315f || headingFromTo < 45f)
			{
				return Rot4.North;
			}
			if (headingFromTo < 135f)
			{
				return Rot4.East;
			}
			if (headingFromTo < 225f)
			{
				return Rot4.South;
			}
			return Rot4.West;
		}

		// Token: 0x0600DF6A RID: 57194 RVA: 0x0053138D File Offset: 0x0052F58D
		public void GetTileVertices(PlanetTile tile, List<Vector3> outVerts)
		{
			this.GetTileVertices(tile.tileId, outVerts);
		}

		// Token: 0x0600DF6B RID: 57195 RVA: 0x0053139C File Offset: 0x0052F59C
		public void GetTileVertices(int tileID, List<Vector3> outVerts)
		{
			PackedListOfLists.GetList<Vector3>(this.tileIDToVerts_offsets, this.verts, tileID, outVerts);
		}

		// Token: 0x0600DF6C RID: 57196 RVA: 0x005313B1 File Offset: 0x0052F5B1
		public void GetTileVerticesIndices(PlanetTile tile, List<int> outVertsIndices)
		{
			this.GetTileVerticesIndices(tile.tileId, outVertsIndices);
		}

		// Token: 0x0600DF6D RID: 57197 RVA: 0x005313C0 File Offset: 0x0052F5C0
		public void GetTileVerticesIndices(int tileID, List<int> outVertsIndices)
		{
			PackedListOfLists.GetListValuesIndices<Vector3>(this.tileIDToVerts_offsets, this.verts, tileID, outVertsIndices);
		}

		// Token: 0x0600DF6E RID: 57198 RVA: 0x005313D5 File Offset: 0x0052F5D5
		public void GetTileNeighbors(PlanetTile tile, List<PlanetTile> outNeighbors)
		{
			this.GetTileNeighbors(tile.tileId, outNeighbors);
		}

		// Token: 0x0600DF6F RID: 57199 RVA: 0x005313E4 File Offset: 0x0052F5E4
		public void GetTileNeighbors(int tileID, List<PlanetTile> outNeighbors)
		{
			PackedListOfLists.GetList<PlanetTile>(this.tileIDToNeighbors_offsets, this.tileIDToNeighbors_values, tileID, outNeighbors);
		}

		// Token: 0x0600DF70 RID: 57200 RVA: 0x005313F9 File Offset: 0x0052F5F9
		public int GetTileNeighborCount(PlanetTile tile)
		{
			return this.GetTileNeighborCount(tile.tileId);
		}

		// Token: 0x0600DF71 RID: 57201 RVA: 0x00531407 File Offset: 0x0052F607
		public int GetTileNeighborCount(int tileID)
		{
			return PackedListOfLists.GetListCount<PlanetTile>(this.tileIDToNeighbors_offsets, this.tileIDToNeighbors_values, tileID);
		}

		// Token: 0x0600DF72 RID: 57202 RVA: 0x0053141B File Offset: 0x0052F61B
		public int GetMaxTileNeighborCountEver(PlanetTile tile)
		{
			return this.GetMaxTileNeighborCountEver(tile.tileId);
		}

		// Token: 0x0600DF73 RID: 57203 RVA: 0x00531429 File Offset: 0x0052F629
		public int GetMaxTileNeighborCountEver(int tileID)
		{
			return PackedListOfLists.GetListCount<Vector3>(this.tileIDToVerts_offsets, this.verts, tileID);
		}

		// Token: 0x0600DF74 RID: 57204 RVA: 0x0053143D File Offset: 0x0052F63D
		public bool IsNeighbor(PlanetTile tileA, PlanetTile tileB)
		{
			return this.IsNeighbor(tileA.tileId, tileB.tileId);
		}

		// Token: 0x0600DF75 RID: 57205 RVA: 0x00531454 File Offset: 0x0052F654
		public bool IsNeighbor(int tileA, int tileB)
		{
			ValueTuple<int, int> listIndexes = PackedListOfLists.GetListIndexes<PlanetTile>(this.tileIDToNeighbors_offsets, this.tileIDToNeighbors_values, tileA);
			int item = listIndexes.Item1;
			int item2 = listIndexes.Item2;
			for (int i = item; i < item2; i++)
			{
				if (this.tileIDToNeighbors_values[i].tileId == tileB)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600DF76 RID: 57206 RVA: 0x005314A3 File Offset: 0x0052F6A3
		public int GetNeighborId(PlanetTile tileA, PlanetTile tileB)
		{
			return this.GetNeighborId(tileA.tileId, tileB.tileId);
		}

		// Token: 0x0600DF77 RID: 57207 RVA: 0x005314B8 File Offset: 0x0052F6B8
		public int GetNeighborId(int tileA, int tileB)
		{
			ValueTuple<int, int> listIndexes = PackedListOfLists.GetListIndexes<PlanetTile>(this.tileIDToNeighbors_offsets, this.tileIDToNeighbors_values, tileA);
			int item = listIndexes.Item1;
			int item2 = listIndexes.Item2;
			int i = item;
			int num = 0;
			while (i < item2)
			{
				if (this.tileIDToNeighbors_values[i].tileId == tileB)
				{
					return num;
				}
				i++;
				num++;
			}
			return -1;
		}

		// Token: 0x0600DF78 RID: 57208 RVA: 0x0053150D File Offset: 0x0052F70D
		public PlanetTile GetTileNeighbor(PlanetTile tile, int adjacentId)
		{
			return this.GetTileNeighbor(tile.tileId, adjacentId);
		}

		// Token: 0x0600DF79 RID: 57209 RVA: 0x0053151C File Offset: 0x0052F71C
		public PlanetTile GetTileNeighbor(int tileID, int adjacentId)
		{
			ValueTuple<int, int> listIndexes = PackedListOfLists.GetListIndexes<PlanetTile>(this.tileIDToNeighbors_offsets, this.tileIDToNeighbors_values, tileID);
			int item = listIndexes.Item1;
			int item2 = listIndexes.Item2;
			if (item + adjacentId >= item2)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(74, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Attempted to get tile neighbour ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(tileID);
				defaultInterpolatedStringHandler.AppendLiteral(" with invalid adjacent ID: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(adjacentId);
				defaultInterpolatedStringHandler.AppendLiteral(", adjacencies: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(item2 - item);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return this.tileIDToNeighbors_values[item + adjacentId];
		}

		// Token: 0x0600DF7A RID: 57210 RVA: 0x005315B0 File Offset: 0x0052F7B0
		public bool IsNeighborOrSame(PlanetTile tileA, PlanetTile tileB)
		{
			return this.IsNeighborOrSame(tileA.tileId, tileB.tileId);
		}

		// Token: 0x0600DF7B RID: 57211 RVA: 0x005315C4 File Offset: 0x0052F7C4
		public bool IsNeighborOrSame(int tileA, int tileB)
		{
			return tileA == tileB || this.IsNeighbor(tileA, tileB);
		}

		// Token: 0x0600DF7C RID: 57212 RVA: 0x005315D4 File Offset: 0x0052F7D4
		public float TileRadiusToAngle(float radii)
		{
			return this.DistOnSurfaceToAngle(radii * this.averageTileSize);
		}

		// Token: 0x0600DF7D RID: 57213 RVA: 0x005315E4 File Offset: 0x0052F7E4
		public float DistOnSurfaceToAngle(float dist)
		{
			return dist / (6.2831855f * this.Radius) * 360f;
		}

		// Token: 0x0600DF7E RID: 57214 RVA: 0x005315FA File Offset: 0x0052F7FA
		public float ApproxDistanceInTiles(float sphericalDistance)
		{
			return sphericalDistance * this.Radius / this.averageTileSize;
		}

		// Token: 0x0600DF7F RID: 57215 RVA: 0x0053160B File Offset: 0x0052F80B
		public float DistanceFromEquatorNormalized(PlanetTile tile)
		{
			return this.DistanceFromEquatorNormalized(tile.tileId);
		}

		// Token: 0x0600DF80 RID: 57216 RVA: 0x00531619 File Offset: 0x0052F819
		public float DistanceFromEquatorNormalized(int tile)
		{
			return Mathf.Abs(this.GetTileCenter(tile).y / this.Radius);
		}

		// Token: 0x0600DF81 RID: 57217 RVA: 0x00531633 File Offset: 0x0052F833
		public float ApproxDistanceInTiles(PlanetTile tileA, PlanetTile tileB)
		{
			return this.ApproxDistanceInTiles(tileA.tileId, tileB.tileId);
		}

		// Token: 0x0600DF82 RID: 57218 RVA: 0x00531648 File Offset: 0x0052F848
		public float ApproxDistanceInTiles(int tileA, int tileB)
		{
			Vector3 tileCenter = this.GetTileCenter(tileA);
			Vector3 tileCenter2 = this.GetTileCenter(tileB);
			return this.ApproxDistanceInTiles(GenMath.SphericalDistance(tileCenter.normalized, tileCenter2.normalized));
		}

		// Token: 0x0600DF83 RID: 57219 RVA: 0x00531680 File Offset: 0x0052F880
		private static void ClearAndDeallocateWorkingLists()
		{
			PlanetLayer.ClearAndDeallocate<TriangleIndices>(ref PlanetLayer.tmp_tris);
			PlanetLayer.ClearAndDeallocate<Vector3>(ref PlanetLayer.tmp_verts);
			PlanetLayer.ClearAndDeallocate<Vector3>(ref PlanetLayer.tmp_generatedVerts);
			PlanetLayer.ClearAndDeallocate<int>(ref PlanetLayer.tmp_generatedVertOffsets);
			PlanetLayer.ClearAndDeallocate<TriangleIndices>(ref PlanetLayer.tmp_generatedTris);
			PlanetLayer.ClearAndDeallocate<int>(ref PlanetLayer.tmp_generatedTileVerts);
			PlanetLayer.ClearAndDeallocate<int>(ref PlanetLayer.tmp_adjacentTris);
			PlanetLayer.ClearAndDeallocate<int>(ref PlanetLayer.tmp_tileIDs);
			PlanetLayer.ClearAndDeallocate<int>(ref PlanetLayer.tmp_neighbourVerts);
			PlanetLayer.ClearAndDeallocate<PlanetTile>(ref PlanetLayer.tmp_neighborsToAdd);
			PlanetLayer.ClearAndDeallocate<int>(ref PlanetLayer.tmp_vertToTris_offsets);
			PlanetLayer.ClearAndDeallocate<int>(ref PlanetLayer.tmp_vertToTris_values);
			PlanetLayer.ClearAndDeallocate<int>(ref PlanetLayer.tmp_vertToTileIDs_offsets);
			PlanetLayer.ClearAndDeallocate<int>(ref PlanetLayer.tmp_vertToTileIDs_values);
			PlanetLayer.ClearAndDeallocate<int>(ref PlanetLayer.tmp_tileIDToVerts_offsets);
			PlanetLayer.ClearAndDeallocate<int>(ref PlanetLayer.tmp_tileIDToVerts_values);
		}

		// Token: 0x0600DF84 RID: 57220 RVA: 0x0053172D File Offset: 0x0052F92D
		private static void ClearAndDeallocate<T>(ref List<T> list)
		{
			list.Clear();
			list.TrimExcess();
			list = new List<T>();
		}

		// Token: 0x0600DF85 RID: 57221 RVA: 0x00531744 File Offset: 0x0052F944
		public void ExposeData()
		{
			Scribe_Defs.Look<PlanetLayerDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.layerId, "layerId", -1, false);
			Scribe_Values.Look<int>(ref this.subdivisions, "subdivisions", 0, false);
			Scribe_Values.Look<string>(ref this.scenarioTag, "scenarioTag", null, false);
			Scribe_Values.Look<bool>(ref this.isRootSurface, "isRootSurface", false, false);
			Scribe_Values.Look<float>(ref this.backgroundWorldCameraOffset, "backgroundWorldCameraOffset", 0f, false);
			Scribe_Values.Look<float>(ref this.backgroundWorldCameraParallaxDistancePer100Cells, "backgroundWorldCameraParallaxDistance", 0f, false);
			Scribe_Values.Look<float>(ref this.radius, "radius", 0f, false);
			Scribe_Values.Look<float>(ref this.viewAngle, "viewAngle", 0f, false);
			Scribe_Values.Look<float>(ref this.extraCameraAltitude, "extraCameraAltitude", 0f, false);
			Scribe_Values.Look<Vector3>(ref this.viewCenter, "viewCenter", default(Vector3), false);
			Scribe_Values.Look<Vector3>(ref this.origin, "origin", default(Vector3), false);
			Scribe_References.Look<PlanetLayer>(ref this.zoomInToLayer, "zoomInToLayer", false);
			Scribe_References.Look<PlanetLayer>(ref this.zoomOutToLayer, "zoomOutToLayer", false);
			Scribe_Collections.Look<PlanetLayer, PlanetLayerConnection>(ref this.connections, "connections", LookMode.Reference, LookMode.Deep, ref this.tmpLayers, ref this.tmpConnections, true, false, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.InitializeLayer();
			}
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.TilesToRawData();
			}
			this.ExposeBody();
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.RawDataToTiles();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.finder.RegenerateCache();
			}
		}

		// Token: 0x0600DF86 RID: 57222 RVA: 0x005318D1 File Offset: 0x0052FAD1
		public virtual void Standardize()
		{
			if (this.tiles.Any<Tile>())
			{
				this.TilesToRawData();
			}
			this.RawDataToTiles();
			this.finder.RegenerateCache();
		}

		// Token: 0x0600DF87 RID: 57223 RVA: 0x005318F7 File Offset: 0x0052FAF7
		internal virtual void ExposeBody()
		{
			DataExposeUtility.LookByteArray(ref this.tileBiome, "tileBiome");
		}

		// Token: 0x0600DF88 RID: 57224 RVA: 0x00531909 File Offset: 0x0052FB09
		protected virtual void TilesToRawData()
		{
			this.tileBiome = DataSerializeUtility.SerializeUshort(this.TilesCount, (int i) => this.tiles[i].PrimaryBiome.shortHash);
		}

		// Token: 0x0600DF89 RID: 57225 RVA: 0x00531928 File Offset: 0x0052FB28
		protected virtual void RawDataToTiles()
		{
			if (this.tiles.Count != this.TilesCount)
			{
				Func<PlanetTile, Tile> orCreateTileFactoryMethod = this.GetOrCreateTileFactoryMethod();
				this.tiles.Clear();
				for (int j = 0; j < this.TilesCount; j++)
				{
					Tile tile = orCreateTileFactoryMethod(new PlanetTile(j, this));
					this.tiles.Add(tile);
					tile.PrimaryBiome = this.def.DefaultBiome;
					tile.elevation = this.Radius;
				}
			}
			DataSerializeUtility.LoadUshort(this.tileBiome, this.TilesCount, delegate(int i, ushort data)
			{
				this.tiles[i].PrimaryBiome = (DefDatabase<BiomeDef>.GetByShortHash(data) ?? this.def.DefaultBiome);
			});
		}

		// Token: 0x0600DF8A RID: 57226 RVA: 0x005319C0 File Offset: 0x0052FBC0
		public void SetAllLayersDirty()
		{
			foreach (WorldDrawLayer worldDrawLayer in this.WorldDrawLayers)
			{
				worldDrawLayer.SetDirty();
			}
		}

		// Token: 0x0600DF8B RID: 57227 RVA: 0x00531A10 File Offset: 0x0052FC10
		public void SetDirty<T>() where T : WorldDrawLayer
		{
			foreach (WorldDrawLayer worldDrawLayer in this.WorldDrawLayers)
			{
				if (worldDrawLayer is T)
				{
					worldDrawLayer.SetDirty();
				}
			}
		}

		// Token: 0x0600DF8C RID: 57228 RVA: 0x00531A6C File Offset: 0x0052FC6C
		public T GetLayer<T>() where T : WorldDrawLayer
		{
			foreach (WorldDrawLayer worldDrawLayer in this.WorldDrawLayers)
			{
				T t = worldDrawLayer as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x0600DF8D RID: 57229 RVA: 0x00531ADC File Offset: 0x0052FCDC
		public void RegenerateAllLayersNow()
		{
			foreach (WorldDrawLayer worldDrawLayer in this.WorldDrawLayers)
			{
				if (worldDrawLayer.Visible)
				{
					worldDrawLayer.RegenerateNow();
				}
			}
		}

		// Token: 0x0600DF8E RID: 57230 RVA: 0x00531B38 File Offset: 0x0052FD38
		public bool LineIntersects(Vector3 A, Vector3 B, float radiusFactor = 1f)
		{
			Vector3 a = this.Origin;
			Vector3 lhs = a - A;
			Vector3 vector = B - A;
			float sqrMagnitude = vector.sqrMagnitude;
			float num = Vector3.Dot(lhs, vector) / sqrMagnitude;
			if (num < 0f)
			{
				return false;
			}
			if (num > 1f)
			{
				return false;
			}
			Vector3 b = A + vector * num;
			return Vector3.Distance(a, b) < this.Radius * radiusFactor;
		}

		// Token: 0x0600DF8F RID: 57231 RVA: 0x00531BA1 File Offset: 0x0052FDA1
		[Obsolete("Use GetClosestTile_NewTemp")]
		public PlanetTile GetClosestTile(PlanetTile other)
		{
			return this.GetClosestTile_NewTemp(other, false);
		}

		// Token: 0x0600DF90 RID: 57232 RVA: 0x00531BAC File Offset: 0x0052FDAC
		public PlanetTile GetClosestTile_NewTemp(PlanetTile other, bool validSettlement = false)
		{
			if (other.Layer == this)
			{
				return other;
			}
			FastTileFinder.TileQueryParams query = new FastTileFinder.TileQueryParams(other, 0f, float.MaxValue, FastTileFinder.LandmarkMode.Any, true, Hilliness.Undefined, Hilliness.Undefined, true, validSettlement, true);
			PlanetTile result;
			if (!this.FastTileFinder.Closest(query, out result, null, null, default(FastTileFinder.TileQueryParams)))
			{
				return PlanetTile.Invalid;
			}
			return result;
		}

		// Token: 0x0600DF91 RID: 57233 RVA: 0x00531C00 File Offset: 0x0052FE00
		public bool DirectConnectionTo(PlanetLayer other)
		{
			return this.connections.ContainsKey(other);
		}

		// Token: 0x0600DF92 RID: 57234 RVA: 0x00531C0E File Offset: 0x0052FE0E
		public bool HasConnectionPathTo(PlanetLayer other)
		{
			bool result = this.IsConnectedToRecursive(other);
			PlanetLayer.TmpVisited.Clear();
			return result;
		}

		// Token: 0x0600DF93 RID: 57235 RVA: 0x00531C24 File Offset: 0x0052FE24
		private bool IsConnectedToRecursive(PlanetLayer other)
		{
			PlanetLayer.TmpVisited.Add(this);
			if (this.connections.ContainsKey(other))
			{
				return true;
			}
			foreach (KeyValuePair<PlanetLayer, PlanetLayerConnection> keyValuePair in this.connections)
			{
				PlanetLayer planetLayer;
				PlanetLayerConnection planetLayerConnection;
				keyValuePair.Deconstruct(out planetLayer, out planetLayerConnection);
				PlanetLayer planetLayer2 = planetLayer;
				if (!PlanetLayer.TmpVisited.Contains(planetLayer2) && planetLayer2.IsConnectedToRecursive(other))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600DF94 RID: 57236 RVA: 0x00531CB8 File Offset: 0x0052FEB8
		private static void ResetPathVars()
		{
			PlanetLayer.openSet.Clear();
			PlanetLayer.cameFrom.Clear();
			PlanetLayer.gScore.Clear();
			PlanetLayer.toEnqueue.Clear();
		}

		// Token: 0x0600DF95 RID: 57237 RVA: 0x00531CE4 File Offset: 0x0052FEE4
		private Func<PlanetTile, Tile> GetOrCreateTileFactoryMethod()
		{
			if (this.tileCreatorFunc != null)
			{
				return this.tileCreatorFunc;
			}
			ConstructorInfo constructor = this.def.tileType.GetConstructor(new Type[]
			{
				typeof(PlanetTile)
			});
			if (constructor == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(116, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unable to deserialize tiles of type ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(this.def.tileType);
				defaultInterpolatedStringHandler.AppendLiteral(" because it does not have a constructor that takes a single PlanetTile argument.");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			DynamicMethod dynamicMethod = new DynamicMethod("DynamicMethod_CreateTile_" + this.def.tileType.Name, typeof(Tile), new Type[]
			{
				typeof(PlanetTile)
			}, typeof(PlanetLayer), true);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Newobj, constructor);
			ilgenerator.Emit(OpCodes.Ret);
			this.tileCreatorFunc = (Func<PlanetTile, Tile>)dynamicMethod.CreateDelegate(typeof(Func<PlanetTile, Tile>));
			return this.tileCreatorFunc;
		}

		// Token: 0x0600DF96 RID: 57238 RVA: 0x00531DFE File Offset: 0x0052FFFE
		public bool TryGetPath(PlanetLayer goal, List<PlanetLayerConnection> path, out float cost, int max = 2147483647)
		{
			return PlanetLayer.TryGetPath(this, goal, path, out cost, max);
		}

		// Token: 0x0600DF97 RID: 57239 RVA: 0x00531E0C File Offset: 0x0053000C
		public static bool TryGetPath(PlanetLayer start, PlanetLayer goal, List<PlanetLayerConnection> path, out float cost, int max = 2147483647)
		{
			PlanetLayer.ResetPathVars();
			PlanetLayer.gScore.Add(start, 0);
			PlanetLayer.openSet.Enqueue(start, PlanetLayer.gScore[start]);
			while (PlanetLayer.openSet.Count != 0)
			{
				PlanetLayer planetLayer = PlanetLayer.openSet.Dequeue();
				if (planetLayer == goal)
				{
					PlanetLayer.ReconstructPath(path, PlanetLayer.cameFrom, planetLayer);
					cost = path.Sum((PlanetLayerConnection x) => x.fuelCost);
					PlanetLayer.ResetPathVars();
					return true;
				}
				PlanetLayer.toEnqueue.Clear();
				foreach (KeyValuePair<PlanetLayer, PlanetLayerConnection> keyValuePair in planetLayer.connections)
				{
					PlanetLayer planetLayer2;
					PlanetLayerConnection planetLayerConnection;
					keyValuePair.Deconstruct(out planetLayer2, out planetLayerConnection);
					PlanetLayer planetLayer3 = planetLayer2;
					PlanetLayerConnection planetLayerConnection2 = planetLayerConnection;
					if (planetLayer3.CanReachLayer)
					{
						if (planetLayer3 == goal)
						{
							PlanetLayer.cameFrom[planetLayer3] = planetLayer;
							PlanetLayer.ReconstructPath(path, PlanetLayer.cameFrom, planetLayer3);
							cost = path.Sum((PlanetLayerConnection x) => x.fuelCost);
							PlanetLayer.ResetPathVars();
							return true;
						}
						int num = PlanetLayer.gScore[planetLayer] + (int)planetLayerConnection2.fuelCost;
						if (num > max)
						{
							break;
						}
						if (!PlanetLayer.gScore.ContainsKey(planetLayer3) || num < PlanetLayer.gScore[planetLayer3])
						{
							PlanetLayer.cameFrom[planetLayer3] = planetLayer;
							PlanetLayer.gScore[planetLayer3] = num;
							PlanetLayer.openSet.Enqueue(planetLayer3, PlanetLayer.gScore[planetLayer3]);
							PlanetLayer.toEnqueue.Add(planetLayer3);
						}
					}
				}
				foreach (PlanetLayer planetLayer4 in PlanetLayer.toEnqueue)
				{
					PlanetLayer.openSet.Enqueue(planetLayer4, PlanetLayer.gScore[planetLayer4]);
				}
			}
			cost = 0f;
			PlanetLayer.ResetPathVars();
			return false;
		}

		// Token: 0x0600DF98 RID: 57240 RVA: 0x00532030 File Offset: 0x00530230
		private static void ReconstructPath(List<PlanetLayerConnection> path, Dictionary<PlanetLayer, PlanetLayer> from, PlanetLayer current)
		{
			path.Clear();
			path.Add(from[current].GetConnectionFromTo(current));
			while (from.ContainsKey(from[current]))
			{
				current = from[current];
				path.Add(from[current].GetConnectionFromTo(current));
			}
			path.Reverse();
		}

		// Token: 0x0600DF99 RID: 57241 RVA: 0x0053208C File Offset: 0x0053028C
		public void Dispose()
		{
			foreach (WorldDrawLayer worldDrawLayer in this.WorldDrawLayers)
			{
				worldDrawLayer.Dispose();
			}
			FastTileFinder fastTileFinder = this.finder;
			if (fastTileFinder != null)
			{
				fastTileFinder.Dispose();
			}
			WorldPathing worldPathing = this.pather;
			if (worldPathing != null)
			{
				worldPathing.Dispose();
			}
			ref this.verts.EnsureDisposed<Vector3>();
			ref this.tileIDToVerts_offsets.EnsureDisposed<int>();
			ref this.tileIDToNeighbors_offsets.EnsureDisposed<int>();
			ref this.tileIDToNeighbors_values.EnsureDisposed<PlanetTile>();
		}

		// Token: 0x04008F4B RID: 36683
		private PlanetLayerDef def;

		// Token: 0x04008F4C RID: 36684
		private string scenarioTag;

		// Token: 0x04008F4D RID: 36685
		private bool isRootSurface;

		// Token: 0x04008F4E RID: 36686
		private int layerId = -1;

		// Token: 0x04008F4F RID: 36687
		private int subdivisions;

		// Token: 0x04008F50 RID: 36688
		private float backgroundWorldCameraOffset;

		// Token: 0x04008F51 RID: 36689
		private float backgroundWorldCameraParallaxDistancePer100Cells;

		// Token: 0x04008F52 RID: 36690
		private float radius;

		// Token: 0x04008F53 RID: 36691
		private float viewAngle;

		// Token: 0x04008F54 RID: 36692
		private float extraCameraAltitude;

		// Token: 0x04008F55 RID: 36693
		private Vector3 viewCenter;

		// Token: 0x04008F56 RID: 36694
		private Vector3 origin;

		// Token: 0x04008F57 RID: 36695
		public PlanetLayer zoomOutToLayer;

		// Token: 0x04008F58 RID: 36696
		public PlanetLayer zoomInToLayer;

		// Token: 0x04008F59 RID: 36697
		private Dictionary<PlanetLayer, PlanetLayerConnection> connections = new Dictionary<PlanetLayer, PlanetLayerConnection>();

		// Token: 0x04008F5A RID: 36698
		protected readonly List<Tile> tiles = new List<Tile>();

		// Token: 0x04008F5B RID: 36699
		private FastTileFinder finder;

		// Token: 0x04008F5C RID: 36700
		private WorldPathing pather;

		// Token: 0x04008F5D RID: 36701
		private WorldFloodFiller filler;

		// Token: 0x04008F5E RID: 36702
		private Func<PlanetTile, Tile> tileCreatorFunc;

		// Token: 0x04008F5F RID: 36703
		private byte[] tileBiome;

		// Token: 0x04008F60 RID: 36704
		private NativeArray<Vector3> verts;

		// Token: 0x04008F61 RID: 36705
		private NativeArray<int> tileIDToVerts_offsets;

		// Token: 0x04008F62 RID: 36706
		private NativeArray<int> tileIDToNeighbors_offsets;

		// Token: 0x04008F63 RID: 36707
		private NativeArray<PlanetTile> tileIDToNeighbors_values;

		// Token: 0x04008F64 RID: 36708
		private float averageTileSize;

		// Token: 0x04008F65 RID: 36709
		private static List<TriangleIndices> tmp_tris = new List<TriangleIndices>();

		// Token: 0x04008F66 RID: 36710
		private static List<Vector3> tmp_verts = new List<Vector3>();

		// Token: 0x04008F67 RID: 36711
		private static List<Vector3> tmp_generatedVerts = new List<Vector3>();

		// Token: 0x04008F68 RID: 36712
		private static List<int> tmp_generatedVertOffsets = new List<int>();

		// Token: 0x04008F69 RID: 36713
		private static List<TriangleIndices> tmp_generatedTris = new List<TriangleIndices>();

		// Token: 0x04008F6A RID: 36714
		private static List<int> tmp_generatedTileVerts = new List<int>();

		// Token: 0x04008F6B RID: 36715
		private static List<int> tmp_adjacentTris = new List<int>();

		// Token: 0x04008F6C RID: 36716
		private static List<int> tmp_tileIDs = new List<int>();

		// Token: 0x04008F6D RID: 36717
		private static List<int> tmp_neighbourVerts = new List<int>();

		// Token: 0x04008F6E RID: 36718
		private static List<PlanetTile> tmp_neighborsToAdd = new List<PlanetTile>();

		// Token: 0x04008F6F RID: 36719
		private static List<int> tmp_vertToTris_offsets = new List<int>();

		// Token: 0x04008F70 RID: 36720
		private static List<int> tmp_vertToTris_values = new List<int>();

		// Token: 0x04008F71 RID: 36721
		private static List<int> tmp_vertToTileIDs_offsets = new List<int>();

		// Token: 0x04008F72 RID: 36722
		private static List<int> tmp_vertToTileIDs_values = new List<int>();

		// Token: 0x04008F73 RID: 36723
		private static List<int> tmp_tileIDToVerts_offsets = new List<int>();

		// Token: 0x04008F74 RID: 36724
		private static List<int> tmp_tileIDToVerts_values = new List<int>();

		// Token: 0x04008F75 RID: 36725
		private const int MaxTileVertices = 6;

		// Token: 0x04008F77 RID: 36727
		private List<PlanetLayer> tmpLayers;

		// Token: 0x04008F78 RID: 36728
		private List<PlanetLayerConnection> tmpConnections;

		// Token: 0x04008F79 RID: 36729
		private static readonly HashSet<PlanetLayer> TmpVisited = new HashSet<PlanetLayer>();

		// Token: 0x04008F7A RID: 36730
		private static readonly PriorityQueue<PlanetLayer, int> openSet = new PriorityQueue<PlanetLayer, int>();

		// Token: 0x04008F7B RID: 36731
		private static readonly Dictionary<PlanetLayer, PlanetLayer> cameFrom = new Dictionary<PlanetLayer, PlanetLayer>();

		// Token: 0x04008F7C RID: 36732
		private static readonly Dictionary<PlanetLayer, int> gScore = new Dictionary<PlanetLayer, int>();

		// Token: 0x04008F7D RID: 36733
		private static readonly List<PlanetLayer> toEnqueue = new List<PlanetLayer>();
	}
}
