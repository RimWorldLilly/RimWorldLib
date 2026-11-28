using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200128D RID: 4749
	public class SectionLayer_SubstructureProps : SectionLayer
	{
		// Token: 0x170013EA RID: 5098
		// (get) Token: 0x060075C0 RID: 30144 RVA: 0x00295FEC File Offset: 0x002941EC
		public override bool Visible
		{
			get
			{
				return ModsConfig.OdysseyActive && DebugViewSettings.drawTerrain;
			}
		}

		// Token: 0x060075C1 RID: 30145 RVA: 0x00295FFC File Offset: 0x002941FC
		public SectionLayer_SubstructureProps(Section section) : base(section)
		{
			this.relevantChangeTypes = (MapMeshFlagDefOf.Terrain | MapMeshFlagDefOf.Buildings);
		}

		// Token: 0x060075C2 RID: 30146 RVA: 0x00296020 File Offset: 0x00294220
		public override void Regenerate()
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.ClearSubMeshes(MeshParts.All);
			Map map = base.Map;
			TerrainGrid terrainGrid = map.terrainGrid;
			CellRect cellRect = this.section.CellRect;
			float altitude = AltitudeLayer.TerrainScatter.AltitudeFor();
			LayerSubMesh subMesh = base.GetSubMesh(SectionLayer_SubstructureProps.Bottom.Material);
			foreach (IntVec3 intVec in cellRect)
			{
				SectionLayer_SubstructureProps.EdgeDirections edgeDirections;
				SectionLayer_SubstructureProps.CornerDirections cornerDirections;
				if (this.ShouldDrawPropsOn(intVec, terrainGrid, out edgeDirections, out cornerDirections))
				{
					this.DrawEdges(intVec, edgeDirections, altitude);
					this.DrawCorners(intVec, cornerDirections, edgeDirections, altitude);
					SectionLayer_GravshipHull.CornerType cornerType;
					Color color;
					SectionLayer_GravshipHull.ShouldDrawCornerPiece(intVec + IntVec3.South, map, terrainGrid, out cornerType, out color);
					bool flag = cornerType - SectionLayer_GravshipHull.CornerType.Corner_NW <= 1 || cornerType - SectionLayer_GravshipHull.CornerType.Diagonal_NW <= 1;
					bool flag2 = flag;
					if (edgeDirections.HasFlag(SectionLayer_SubstructureProps.EdgeDirections.South) && !flag2)
					{
						this.AddQuad(subMesh, intVec + IntVec3.South, altitude, Rot4.North, SectionLayer_GravshipMask.IsValidSubstructure(intVec));
					}
				}
			}
			base.FinalizeMesh(MeshParts.All);
		}

		// Token: 0x060075C3 RID: 30147 RVA: 0x00296150 File Offset: 0x00294350
		private void DrawEdges(IntVec3 c, SectionLayer_SubstructureProps.EdgeDirections edgeDirs, float altitude)
		{
			ValueTuple<CachedMaterial, Rot4>[] array;
			if (SectionLayer_SubstructureProps.EdgeMats.TryGetValue(edgeDirs, out array))
			{
				foreach (ValueTuple<CachedMaterial, Rot4> valueTuple in array)
				{
					CachedMaterial item = valueTuple.Item1;
					Rot4 item2 = valueTuple.Item2;
					this.AddQuad(base.GetSubMesh(item.Material), c, altitude, item2, false);
				}
			}
		}

		// Token: 0x060075C4 RID: 30148 RVA: 0x002961A4 File Offset: 0x002943A4
		private void DrawCorners(IntVec3 c, SectionLayer_SubstructureProps.CornerDirections cornerDirections, SectionLayer_SubstructureProps.EdgeDirections edgeDirs, float altitude)
		{
			if (cornerDirections.HasFlag(SectionLayer_SubstructureProps.CornerDirections.NorthWest) && !edgeDirs.HasFlag(SectionLayer_SubstructureProps.EdgeDirections.North) && !edgeDirs.HasFlag(SectionLayer_SubstructureProps.EdgeDirections.West))
			{
				this.AddQuad(base.GetSubMesh(SectionLayer_SubstructureProps.CornerInner.Material), c, altitude, Rot4.South, false);
			}
			if (cornerDirections.HasFlag(SectionLayer_SubstructureProps.CornerDirections.NorthEast) && !edgeDirs.HasFlag(SectionLayer_SubstructureProps.EdgeDirections.North) && !edgeDirs.HasFlag(SectionLayer_SubstructureProps.EdgeDirections.East))
			{
				this.AddQuad(base.GetSubMesh(SectionLayer_SubstructureProps.CornerInner.Material), c, altitude, Rot4.West, false);
			}
			if (cornerDirections.HasFlag(SectionLayer_SubstructureProps.CornerDirections.SouthEast) && !edgeDirs.HasFlag(SectionLayer_SubstructureProps.EdgeDirections.South) && !edgeDirs.HasFlag(SectionLayer_SubstructureProps.EdgeDirections.East))
			{
				this.AddQuad(base.GetSubMesh(SectionLayer_SubstructureProps.CornerInner.Material), c, altitude, Rot4.North, false);
			}
			if (cornerDirections.HasFlag(SectionLayer_SubstructureProps.CornerDirections.SouthWest) && !edgeDirs.HasFlag(SectionLayer_SubstructureProps.EdgeDirections.South) && !edgeDirs.HasFlag(SectionLayer_SubstructureProps.EdgeDirections.West))
			{
				this.AddQuad(base.GetSubMesh(SectionLayer_SubstructureProps.CornerInner.Material), c, altitude, Rot4.East, false);
			}
		}

		// Token: 0x060075C5 RID: 30149 RVA: 0x00296314 File Offset: 0x00294514
		private void AddQuad(LayerSubMesh sm, IntVec3 c, float altitude, Rot4 rotation, bool addGravshipMask)
		{
			int count = sm.verts.Count;
			int num = Mathf.Abs(4 - rotation.AsInt);
			for (int i = 0; i < 4; i++)
			{
				sm.verts.Add(new Vector3((float)c.x + SectionLayer_SubstructureProps.UVs[i].x, altitude, (float)c.z + SectionLayer_SubstructureProps.UVs[i].y));
				sm.uvs.Add(SectionLayer_SubstructureProps.UVs[(num + i) % 4]);
			}
			sm.tris.Add(count);
			sm.tris.Add(count + 1);
			sm.tris.Add(count + 2);
			sm.tris.Add(count);
			sm.tris.Add(count + 2);
			sm.tris.Add(count + 3);
			if (addGravshipMask)
			{
				Texture2D srcTex = sm.material.mainTexture as Texture2D;
				Color color = sm.material.color;
				Material material = MaterialPool.MatFrom(srcTex, ShaderDatabase.GravshipMaskMasked, color);
				this.AddQuad(base.GetSubMesh(material), c, altitude, rotation, false);
			}
		}

		// Token: 0x060075C6 RID: 30150 RVA: 0x00296438 File Offset: 0x00294638
		private bool ShouldDrawPropsOn(IntVec3 c, TerrainGrid terrGrid, out SectionLayer_SubstructureProps.EdgeDirections edgeEdgeDirections, out SectionLayer_SubstructureProps.CornerDirections cornerDirections)
		{
			edgeEdgeDirections = SectionLayer_SubstructureProps.EdgeDirections.None;
			cornerDirections = SectionLayer_SubstructureProps.CornerDirections.None;
			TerrainDef terrainDef = terrGrid.FoundationAt(c);
			if (terrainDef == null || !terrainDef.IsSubstructure)
			{
				return false;
			}
			for (int i = 0; i < GenAdj.CardinalDirections.Length; i++)
			{
				IntVec3 c2 = c + GenAdj.CardinalDirections[i];
				if (!c2.InBounds(base.Map))
				{
					edgeEdgeDirections |= (SectionLayer_SubstructureProps.EdgeDirections)(1 << i);
				}
				else
				{
					TerrainDef terrainDef2 = terrGrid.FoundationAt(c2);
					if (terrainDef2 == null || !terrainDef2.IsSubstructure)
					{
						edgeEdgeDirections |= (SectionLayer_SubstructureProps.EdgeDirections)(1 << i);
					}
				}
			}
			for (int j = 0; j < GenAdj.DiagonalDirections.Length; j++)
			{
				IntVec3 c3 = c + GenAdj.DiagonalDirections[j];
				if (!c3.InBounds(base.Map))
				{
					cornerDirections |= (SectionLayer_SubstructureProps.CornerDirections)(1 << j);
				}
				else
				{
					TerrainDef terrainDef3 = terrGrid.FoundationAt(c3);
					if (terrainDef3 == null || !terrainDef3.IsSubstructure)
					{
						cornerDirections |= (SectionLayer_SubstructureProps.CornerDirections)(1 << j);
					}
				}
			}
			return edgeEdgeDirections != SectionLayer_SubstructureProps.EdgeDirections.None || cornerDirections > SectionLayer_SubstructureProps.CornerDirections.None;
		}

		// Token: 0x040049DE RID: 18910
		private static readonly CachedMaterial OShape = new CachedMaterial("Terrain/Surfaces/Substructure/SubstructureBorder_OShape", ShaderDatabase.Transparent);

		// Token: 0x040049DF RID: 18911
		private static readonly CachedMaterial UShape = new CachedMaterial("Terrain/Surfaces/Substructure/SubstructureBorder_UShape", ShaderDatabase.Transparent);

		// Token: 0x040049E0 RID: 18912
		private static readonly CachedMaterial CornerInner = new CachedMaterial("Terrain/Surfaces/Substructure/SubstructureBorder_CornerInner", ShaderDatabase.Transparent);

		// Token: 0x040049E1 RID: 18913
		private static readonly CachedMaterial CornerOuter = new CachedMaterial("Terrain/Surfaces/Substructure/SubstructureBorder_CornerOuter", ShaderDatabase.Transparent);

		// Token: 0x040049E2 RID: 18914
		private static readonly CachedMaterial Flat = new CachedMaterial("Terrain/Surfaces/Substructure/SubstructureBorder_Flat", ShaderDatabase.Transparent);

		// Token: 0x040049E3 RID: 18915
		private static readonly CachedMaterial Bottom = new CachedMaterial("Terrain/Surfaces/Substructure/SubstructureProps_Bottom", ShaderDatabase.Transparent);

		// Token: 0x040049E4 RID: 18916
		private static readonly Vector2[] UVs = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f)
		};

		// Token: 0x040049E5 RID: 18917
		private static readonly Dictionary<SectionLayer_SubstructureProps.EdgeDirections, ValueTuple<CachedMaterial, Rot4>[]> EdgeMats = new Dictionary<SectionLayer_SubstructureProps.EdgeDirections, ValueTuple<CachedMaterial, Rot4>[]>
		{
			{
				SectionLayer_SubstructureProps.EdgeDirections.North,
				new ValueTuple<CachedMaterial, Rot4>[]
				{
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.Flat, Rot4.South)
				}
			},
			{
				SectionLayer_SubstructureProps.EdgeDirections.East,
				new ValueTuple<CachedMaterial, Rot4>[]
				{
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.Flat, Rot4.West)
				}
			},
			{
				SectionLayer_SubstructureProps.EdgeDirections.South,
				new ValueTuple<CachedMaterial, Rot4>[]
				{
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.Flat, Rot4.North)
				}
			},
			{
				SectionLayer_SubstructureProps.EdgeDirections.West,
				new ValueTuple<CachedMaterial, Rot4>[]
				{
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.Flat, Rot4.East)
				}
			},
			{
				SectionLayer_SubstructureProps.EdgeDirections.North | SectionLayer_SubstructureProps.EdgeDirections.East,
				new ValueTuple<CachedMaterial, Rot4>[]
				{
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.CornerOuter, Rot4.West)
				}
			},
			{
				SectionLayer_SubstructureProps.EdgeDirections.East | SectionLayer_SubstructureProps.EdgeDirections.South,
				new ValueTuple<CachedMaterial, Rot4>[]
				{
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.CornerOuter, Rot4.North)
				}
			},
			{
				SectionLayer_SubstructureProps.EdgeDirections.South | SectionLayer_SubstructureProps.EdgeDirections.West,
				new ValueTuple<CachedMaterial, Rot4>[]
				{
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.CornerOuter, Rot4.East)
				}
			},
			{
				SectionLayer_SubstructureProps.EdgeDirections.North | SectionLayer_SubstructureProps.EdgeDirections.West,
				new ValueTuple<CachedMaterial, Rot4>[]
				{
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.CornerOuter, Rot4.South)
				}
			},
			{
				SectionLayer_SubstructureProps.EdgeDirections.North | SectionLayer_SubstructureProps.EdgeDirections.South,
				new ValueTuple<CachedMaterial, Rot4>[]
				{
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.Flat, Rot4.South),
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.Flat, Rot4.North)
				}
			},
			{
				SectionLayer_SubstructureProps.EdgeDirections.East | SectionLayer_SubstructureProps.EdgeDirections.West,
				new ValueTuple<CachedMaterial, Rot4>[]
				{
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.Flat, Rot4.West),
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.Flat, Rot4.East)
				}
			},
			{
				SectionLayer_SubstructureProps.EdgeDirections.North | SectionLayer_SubstructureProps.EdgeDirections.East | SectionLayer_SubstructureProps.EdgeDirections.South,
				new ValueTuple<CachedMaterial, Rot4>[]
				{
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.UShape, Rot4.West)
				}
			},
			{
				SectionLayer_SubstructureProps.EdgeDirections.East | SectionLayer_SubstructureProps.EdgeDirections.South | SectionLayer_SubstructureProps.EdgeDirections.West,
				new ValueTuple<CachedMaterial, Rot4>[]
				{
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.UShape, Rot4.North)
				}
			},
			{
				SectionLayer_SubstructureProps.EdgeDirections.North | SectionLayer_SubstructureProps.EdgeDirections.South | SectionLayer_SubstructureProps.EdgeDirections.West,
				new ValueTuple<CachedMaterial, Rot4>[]
				{
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.UShape, Rot4.East)
				}
			},
			{
				SectionLayer_SubstructureProps.EdgeDirections.North | SectionLayer_SubstructureProps.EdgeDirections.East | SectionLayer_SubstructureProps.EdgeDirections.West,
				new ValueTuple<CachedMaterial, Rot4>[]
				{
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.UShape, Rot4.South)
				}
			},
			{
				SectionLayer_SubstructureProps.EdgeDirections.North | SectionLayer_SubstructureProps.EdgeDirections.East | SectionLayer_SubstructureProps.EdgeDirections.South | SectionLayer_SubstructureProps.EdgeDirections.West,
				new ValueTuple<CachedMaterial, Rot4>[]
				{
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.OShape, Rot4.North)
				}
			}
		};

		// Token: 0x02003181 RID: 12673
		[Flags]
		private enum EdgeDirections
		{
			// Token: 0x0400C82C RID: 51244
			None = 0,
			// Token: 0x0400C82D RID: 51245
			North = 1,
			// Token: 0x0400C82E RID: 51246
			East = 2,
			// Token: 0x0400C82F RID: 51247
			South = 4,
			// Token: 0x0400C830 RID: 51248
			West = 8
		}

		// Token: 0x02003182 RID: 12674
		[Flags]
		private enum CornerDirections
		{
			// Token: 0x0400C832 RID: 51250
			None = 0,
			// Token: 0x0400C833 RID: 51251
			SouthWest = 1,
			// Token: 0x0400C834 RID: 51252
			NorthWest = 2,
			// Token: 0x0400C835 RID: 51253
			NorthEast = 4,
			// Token: 0x0400C836 RID: 51254
			SouthEast = 8
		}
	}
}
