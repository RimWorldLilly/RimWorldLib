using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200128E RID: 4750
	public class SectionLayer_TerrainEdges : SectionLayer
	{
		// Token: 0x170013EB RID: 5099
		// (get) Token: 0x060075C8 RID: 30152 RVA: 0x0029686C File Offset: 0x00294A6C
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawTerrain;
			}
		}

		// Token: 0x060075C9 RID: 30153 RVA: 0x00296873 File Offset: 0x00294A73
		public SectionLayer_TerrainEdges(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlagDefOf.Terrain;
		}

		// Token: 0x060075CA RID: 30154 RVA: 0x0029688C File Offset: 0x00294A8C
		public override void Regenerate()
		{
			if (base.Map.Tile.Valid && !base.Map.Tile.LayerDef.isSpace)
			{
				return;
			}
			base.ClearSubMeshes(MeshParts.All);
			TerrainGrid terrainGrid = base.Map.terrainGrid;
			CellRect cellRect = this.section.CellRect;
			float altitude = AltitudeLayer.TerrainScatter.AltitudeFor();
			float altitude2 = AltitudeLayer.TerrainEdges.AltitudeFor();
			foreach (IntVec3 intVec in cellRect)
			{
				SectionLayer_TerrainEdges.EdgeDirections edgeDirections;
				SectionLayer_TerrainEdges.CornerDirections corners;
				if (this.ShouldDrawRockEdges(intVec, terrainGrid, out edgeDirections, out corners))
				{
					TerrainDef terrain = terrainGrid.BaseTerrainAt(intVec);
					this.DrawEdges(terrain, intVec, edgeDirections, altitude);
					this.DrawCorners(terrain, intVec, edgeDirections, corners, altitude);
					if (this.ShouldDrawPassthrough(intVec, terrainGrid, out edgeDirections, out corners))
					{
						this.DrawLoop(intVec + IntVec3.South, terrainGrid, edgeDirections, corners, altitude2);
					}
				}
				else if (this.ShouldDrawLoop(intVec, terrainGrid, out edgeDirections, out corners))
				{
					this.DrawLoop(intVec, terrainGrid, edgeDirections, corners, altitude2);
				}
			}
			base.FinalizeMesh(MeshParts.All);
		}

		// Token: 0x060075CB RID: 30155 RVA: 0x002969C0 File Offset: 0x00294BC0
		private void DrawEdges(TerrainDef terrain, IntVec3 c, SectionLayer_TerrainEdges.EdgeDirections edgeDirs, float altitude)
		{
			ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[] array;
			if (SectionLayer_TerrainEdges.EdgeMats.TryGetValue(edgeDirs, out array))
			{
				foreach (ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4> valueTuple in array)
				{
					SectionLayer_TerrainEdges.EdgeType item = valueTuple.Item1;
					Rot4 item2 = valueTuple.Item2;
					this.AddQuad(terrain, item, c, altitude, item2, 0);
				}
			}
		}

		// Token: 0x060075CC RID: 30156 RVA: 0x00296A0C File Offset: 0x00294C0C
		private void DrawCorners(TerrainDef terrain, IntVec3 c, SectionLayer_TerrainEdges.EdgeDirections edges, SectionLayer_TerrainEdges.CornerDirections corners, float altitude)
		{
			if (corners.HasFlag(SectionLayer_TerrainEdges.CornerDirections.NorthWest) && !edges.HasFlag(SectionLayer_TerrainEdges.EdgeDirections.North) && !edges.HasFlag(SectionLayer_TerrainEdges.EdgeDirections.West))
			{
				this.AddQuad(terrain, SectionLayer_TerrainEdges.EdgeType.CornerInner, c, altitude, Rot4.East, 0);
			}
			if (corners.HasFlag(SectionLayer_TerrainEdges.CornerDirections.NorthEast) && !edges.HasFlag(SectionLayer_TerrainEdges.EdgeDirections.North) && !edges.HasFlag(SectionLayer_TerrainEdges.EdgeDirections.East))
			{
				this.AddQuad(terrain, SectionLayer_TerrainEdges.EdgeType.CornerInner, c, altitude, Rot4.South, 0);
			}
			if (corners.HasFlag(SectionLayer_TerrainEdges.CornerDirections.SouthEast) && !edges.HasFlag(SectionLayer_TerrainEdges.EdgeDirections.South) && !edges.HasFlag(SectionLayer_TerrainEdges.EdgeDirections.East))
			{
				this.AddQuad(terrain, SectionLayer_TerrainEdges.EdgeType.CornerInner, c, altitude, Rot4.West, 0);
			}
			if (corners.HasFlag(SectionLayer_TerrainEdges.CornerDirections.SouthWest) && !edges.HasFlag(SectionLayer_TerrainEdges.EdgeDirections.South) && !edges.HasFlag(SectionLayer_TerrainEdges.EdgeDirections.West))
			{
				this.AddQuad(terrain, SectionLayer_TerrainEdges.EdgeType.CornerInner, c, altitude, Rot4.North, 0);
			}
		}

		// Token: 0x060075CD RID: 30157 RVA: 0x00296B48 File Offset: 0x00294D48
		private void DrawLoop(IntVec3 c, TerrainGrid grid, SectionLayer_TerrainEdges.EdgeDirections edges, SectionLayer_TerrainEdges.CornerDirections corners, float altitude)
		{
			if (!edges.HasFlag(SectionLayer_TerrainEdges.EdgeDirections.North))
			{
				return;
			}
			TerrainDef terrain = grid.BaseTerrainAt(c + IntVec3.North);
			float num = (float)c.z / (float)base.Map.Size.z;
			altitude += 0.03658537f - num * 0.03658537f;
			if (!corners.HasFlag(SectionLayer_TerrainEdges.CornerDirections.NorthWest) && !corners.HasFlag(SectionLayer_TerrainEdges.CornerDirections.NorthEast))
			{
				this.AddQuad(terrain, SectionLayer_TerrainEdges.EdgeType.LoopSingle, c + IntVec3.South, altitude, Rot4.North, 0);
			}
			if (corners.HasFlag(SectionLayer_TerrainEdges.CornerDirections.NorthWest) && corners.HasFlag(SectionLayer_TerrainEdges.CornerDirections.NorthEast))
			{
				this.AddQuad(terrain, SectionLayer_TerrainEdges.EdgeType.Loop, c + IntVec3.South, altitude, Rot4.North, c.GetHashCode());
			}
			if (!corners.HasFlag(SectionLayer_TerrainEdges.CornerDirections.NorthWest) && corners.HasFlag(SectionLayer_TerrainEdges.CornerDirections.NorthEast))
			{
				this.AddQuad(terrain, SectionLayer_TerrainEdges.EdgeType.LoopLeft, c + IntVec3.South, altitude, Rot4.North, 0);
			}
			if (!corners.HasFlag(SectionLayer_TerrainEdges.CornerDirections.NorthEast) && corners.HasFlag(SectionLayer_TerrainEdges.CornerDirections.NorthWest))
			{
				this.AddQuad(terrain, SectionLayer_TerrainEdges.EdgeType.LoopRight, c + IntVec3.South, altitude, Rot4.North, 0);
			}
		}

		// Token: 0x060075CE RID: 30158 RVA: 0x00296CC0 File Offset: 0x00294EC0
		private void AddQuad(TerrainDef terrain, SectionLayer_TerrainEdges.EdgeType edgeType, IntVec3 c, float altitude, Rot4 rotation, int listIndexOffset = 0)
		{
			Material material = terrain.spaceEdgeGraphicData.GetMaterial(terrain, edgeType, listIndexOffset);
			LayerSubMesh subMesh = base.GetSubMesh(material);
			int count = subMesh.verts.Count;
			float num = Mathf.Max((float)material.mainTexture.width / (float)material.mainTexture.height, 1f);
			float num2 = Mathf.Max((float)material.mainTexture.height / (float)material.mainTexture.width, 1f);
			int num3 = Mathf.Abs(4 - rotation.AsInt);
			for (int i = 0; i < 4; i++)
			{
				subMesh.verts.Add(new Vector3((float)c.x + SectionLayer_TerrainEdges.UVs[i].x * num, altitude, (float)c.z + SectionLayer_TerrainEdges.UVs[i].y * num2));
				subMesh.uvs.Add(SectionLayer_TerrainEdges.UVs[(num3 + i) % 4]);
			}
			subMesh.tris.Add(count);
			subMesh.tris.Add(count + 1);
			subMesh.tris.Add(count + 2);
			subMesh.tris.Add(count);
			subMesh.tris.Add(count + 2);
			subMesh.tris.Add(count + 3);
		}

		// Token: 0x060075CF RID: 30159 RVA: 0x00296E14 File Offset: 0x00295014
		private bool ShouldDrawRockEdges(IntVec3 c, TerrainGrid grid, out SectionLayer_TerrainEdges.EdgeDirections edges, out SectionLayer_TerrainEdges.CornerDirections corners)
		{
			edges = SectionLayer_TerrainEdges.EdgeDirections.None;
			corners = SectionLayer_TerrainEdges.CornerDirections.None;
			TerrainDef terrainDef = grid.BaseTerrainAt(c);
			if (terrainDef == null || terrainDef.spaceEdgeGraphicData == null)
			{
				return false;
			}
			for (int i = 0; i < GenAdj.CardinalDirections.Length; i++)
			{
				IntVec3 c2 = c + GenAdj.CardinalDirections[i];
				if (!c2.InBounds(base.Map))
				{
					if (!base.Map.DrawMapClippers)
					{
						edges |= (SectionLayer_TerrainEdges.EdgeDirections)(1 << i);
					}
				}
				else
				{
					TerrainDef terrainDef2 = grid.TerrainAt(c2);
					if (terrainDef2 == null || terrainDef2.dontRender)
					{
						edges |= (SectionLayer_TerrainEdges.EdgeDirections)(1 << i);
					}
				}
			}
			for (int j = 0; j < GenAdj.DiagonalDirections.Length; j++)
			{
				IntVec3 c3 = c + GenAdj.DiagonalDirections[j];
				if (!c3.InBounds(base.Map))
				{
					if (!base.Map.DrawMapClippers)
					{
						edges |= (SectionLayer_TerrainEdges.EdgeDirections)(1 << j);
					}
				}
				else
				{
					TerrainDef terrainDef3 = grid.TerrainAt(c3);
					if (terrainDef3 == null || terrainDef3.dontRender)
					{
						corners |= (SectionLayer_TerrainEdges.CornerDirections)(1 << j);
					}
				}
			}
			return edges != SectionLayer_TerrainEdges.EdgeDirections.None || corners > SectionLayer_TerrainEdges.CornerDirections.None;
		}

		// Token: 0x060075D0 RID: 30160 RVA: 0x00296F2C File Offset: 0x0029512C
		private bool ShouldDrawPassthrough(IntVec3 c, TerrainGrid grid, out SectionLayer_TerrainEdges.EdgeDirections edges, out SectionLayer_TerrainEdges.CornerDirections corners)
		{
			edges = SectionLayer_TerrainEdges.EdgeDirections.None;
			corners = SectionLayer_TerrainEdges.CornerDirections.None;
			IntVec3 c2 = c + IntVec3.North;
			if (!c2.InBounds(base.Map))
			{
				return false;
			}
			TerrainDef terrainDef = grid.BaseTerrainAt(c2);
			if (terrainDef == null || terrainDef.spaceEdgeGraphicData == null)
			{
				return false;
			}
			IntVec3 c3 = c + IntVec3.West;
			IntVec3 c4 = c + IntVec3.East;
			if (!c3.InBounds(base.Map) || !c4.InBounds(base.Map))
			{
				return false;
			}
			TerrainDef terrainDef2 = grid.TerrainAt(c3);
			TerrainDef terrainDef3 = grid.TerrainAt(c4);
			if ((terrainDef3 != null && terrainDef3.dontRender) || (terrainDef2 != null && terrainDef2.dontRender))
			{
				return false;
			}
			corners = (SectionLayer_TerrainEdges.CornerDirections.NorthWest | SectionLayer_TerrainEdges.CornerDirections.NorthEast);
			edges = SectionLayer_TerrainEdges.EdgeDirections.North;
			return true;
		}

		// Token: 0x060075D1 RID: 30161 RVA: 0x00296FE0 File Offset: 0x002951E0
		private bool ShouldDrawLoop(IntVec3 c, TerrainGrid grid, out SectionLayer_TerrainEdges.EdgeDirections edges, out SectionLayer_TerrainEdges.CornerDirections corners)
		{
			edges = SectionLayer_TerrainEdges.EdgeDirections.None;
			corners = SectionLayer_TerrainEdges.CornerDirections.None;
			IntVec3 c2 = c + IntVec3.North;
			TerrainDef terrainDef = grid.TerrainAt(c);
			if ((terrainDef != null && !terrainDef.dontRender) || !c2.InBounds(base.Map))
			{
				return false;
			}
			TerrainDef terrainDef2 = grid.BaseTerrainAt(c2);
			if (terrainDef2 == null || terrainDef2.spaceEdgeGraphicData == null)
			{
				return false;
			}
			for (int i = 0; i < GenAdj.CardinalDirections.Length; i++)
			{
				IntVec3 c3 = c + GenAdj.CardinalDirections[i];
				if (!c3.InBounds(base.Map))
				{
					if (!base.Map.DrawMapClippers)
					{
						edges |= (SectionLayer_TerrainEdges.EdgeDirections)(1 << i);
					}
				}
				else
				{
					TerrainDef terrainDef3 = grid.TerrainAt(c3);
					if (terrainDef3 != null && !terrainDef3.dontRender)
					{
						edges |= (SectionLayer_TerrainEdges.EdgeDirections)(1 << i);
					}
				}
			}
			for (int j = 0; j < GenAdj.DiagonalDirections.Length; j++)
			{
				IntVec3 c4 = c + GenAdj.DiagonalDirections[j];
				if (!c4.InBounds(base.Map))
				{
					if (!base.Map.DrawMapClippers)
					{
						edges |= (SectionLayer_TerrainEdges.EdgeDirections)(1 << j);
					}
				}
				else
				{
					TerrainDef terrainDef4 = grid.TerrainAt(c4);
					if (terrainDef4 != null && !terrainDef4.dontRender)
					{
						corners |= (SectionLayer_TerrainEdges.CornerDirections)(1 << j);
					}
				}
			}
			return edges.HasFlag(SectionLayer_TerrainEdges.EdgeDirections.North);
		}

		// Token: 0x040049E6 RID: 18918
		private static readonly Vector2[] UVs = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f)
		};

		// Token: 0x040049E7 RID: 18919
		private static readonly Dictionary<SectionLayer_TerrainEdges.EdgeDirections, ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]> EdgeMats = new Dictionary<SectionLayer_TerrainEdges.EdgeDirections, ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]>
		{
			{
				SectionLayer_TerrainEdges.EdgeDirections.North,
				new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]
				{
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.Flat, Rot4.South)
				}
			},
			{
				SectionLayer_TerrainEdges.EdgeDirections.East,
				new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]
				{
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.Flat, Rot4.West)
				}
			},
			{
				SectionLayer_TerrainEdges.EdgeDirections.South,
				new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]
				{
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.Flat, Rot4.North)
				}
			},
			{
				SectionLayer_TerrainEdges.EdgeDirections.West,
				new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]
				{
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.Flat, Rot4.East)
				}
			},
			{
				SectionLayer_TerrainEdges.EdgeDirections.North | SectionLayer_TerrainEdges.EdgeDirections.East,
				new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]
				{
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.CornerOuter, Rot4.West)
				}
			},
			{
				SectionLayer_TerrainEdges.EdgeDirections.East | SectionLayer_TerrainEdges.EdgeDirections.South,
				new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]
				{
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.CornerOuter, Rot4.North)
				}
			},
			{
				SectionLayer_TerrainEdges.EdgeDirections.South | SectionLayer_TerrainEdges.EdgeDirections.West,
				new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]
				{
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.CornerOuter, Rot4.East)
				}
			},
			{
				SectionLayer_TerrainEdges.EdgeDirections.North | SectionLayer_TerrainEdges.EdgeDirections.West,
				new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]
				{
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.CornerOuter, Rot4.South)
				}
			},
			{
				SectionLayer_TerrainEdges.EdgeDirections.North | SectionLayer_TerrainEdges.EdgeDirections.South,
				new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]
				{
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.Flat, Rot4.South),
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.Flat, Rot4.North)
				}
			},
			{
				SectionLayer_TerrainEdges.EdgeDirections.East | SectionLayer_TerrainEdges.EdgeDirections.West,
				new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]
				{
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.Flat, Rot4.West),
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.Flat, Rot4.East)
				}
			},
			{
				SectionLayer_TerrainEdges.EdgeDirections.North | SectionLayer_TerrainEdges.EdgeDirections.East | SectionLayer_TerrainEdges.EdgeDirections.South,
				new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]
				{
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.UShape, Rot4.West)
				}
			},
			{
				SectionLayer_TerrainEdges.EdgeDirections.East | SectionLayer_TerrainEdges.EdgeDirections.South | SectionLayer_TerrainEdges.EdgeDirections.West,
				new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]
				{
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.UShape, Rot4.North)
				}
			},
			{
				SectionLayer_TerrainEdges.EdgeDirections.North | SectionLayer_TerrainEdges.EdgeDirections.South | SectionLayer_TerrainEdges.EdgeDirections.West,
				new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]
				{
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.UShape, Rot4.East)
				}
			},
			{
				SectionLayer_TerrainEdges.EdgeDirections.North | SectionLayer_TerrainEdges.EdgeDirections.East | SectionLayer_TerrainEdges.EdgeDirections.West,
				new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]
				{
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.UShape, Rot4.South)
				}
			},
			{
				SectionLayer_TerrainEdges.EdgeDirections.North | SectionLayer_TerrainEdges.EdgeDirections.East | SectionLayer_TerrainEdges.EdgeDirections.South | SectionLayer_TerrainEdges.EdgeDirections.West,
				new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]
				{
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.OShape, Rot4.North)
				}
			}
		};

		// Token: 0x02003183 RID: 12675
		[Flags]
		private enum EdgeDirections
		{
			// Token: 0x0400C838 RID: 51256
			None = 0,
			// Token: 0x0400C839 RID: 51257
			North = 1,
			// Token: 0x0400C83A RID: 51258
			East = 2,
			// Token: 0x0400C83B RID: 51259
			South = 4,
			// Token: 0x0400C83C RID: 51260
			West = 8
		}

		// Token: 0x02003184 RID: 12676
		[Flags]
		private enum CornerDirections
		{
			// Token: 0x0400C83E RID: 51262
			None = 0,
			// Token: 0x0400C83F RID: 51263
			SouthWest = 1,
			// Token: 0x0400C840 RID: 51264
			NorthWest = 2,
			// Token: 0x0400C841 RID: 51265
			NorthEast = 4,
			// Token: 0x0400C842 RID: 51266
			SouthEast = 8
		}

		// Token: 0x02003185 RID: 12677
		public enum EdgeType
		{
			// Token: 0x0400C844 RID: 51268
			OShape,
			// Token: 0x0400C845 RID: 51269
			UShape,
			// Token: 0x0400C846 RID: 51270
			CornerInner,
			// Token: 0x0400C847 RID: 51271
			CornerOuter,
			// Token: 0x0400C848 RID: 51272
			Flat,
			// Token: 0x0400C849 RID: 51273
			LoopLeft,
			// Token: 0x0400C84A RID: 51274
			LoopRight,
			// Token: 0x0400C84B RID: 51275
			LoopSingle,
			// Token: 0x0400C84C RID: 51276
			Loop
		}
	}
}
