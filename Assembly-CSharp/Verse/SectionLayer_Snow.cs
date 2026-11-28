using System;
using System.Collections.Generic;
using RimWorld;
using Unity.Collections;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002BC RID: 700
	[StaticConstructorOnStartup]
	public class SectionLayer_Snow : SectionLayer
	{
		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06001566 RID: 5478 RVA: 0x0007EBAE File Offset: 0x0007CDAE
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawSnow;
			}
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x0007EBB5 File Offset: 0x0007CDB5
		public SectionLayer_Snow(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlagDefOf.Snow;
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x0007EBDC File Offset: 0x0007CDDC
		private bool Filled(int index)
		{
			Building building = base.Map.edificeGrid[index];
			return building != null && building.def.Fillage == FillCategory.Full;
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x0007EC10 File Offset: 0x0007CE10
		public override void Regenerate()
		{
			LayerSubMesh subMesh = base.GetSubMesh(MatBases.Snow);
			if (ModsConfig.BiotechActive)
			{
				subMesh.material.SetTexture(ShaderPropertyIDs.PollutedTex, SectionLayer_Snow.PollutedSnowTex.Texture);
			}
			if (subMesh.mesh.vertexCount == 0)
			{
				SectionLayerGeometryMaker_Solid.MakeBaseGeometry(this.section, subMesh, AltitudeLayer.Terrain);
			}
			SectionLayer_Snow.opacityListTmp.Clear();
			subMesh.Clear(MeshParts.Colors);
			NativeArray<float> depthGrid_Unsafe = base.Map.snowGrid.DepthGrid_Unsafe;
			CellRect cellRect = this.section.CellRect;
			bool flag = false;
			CellIndices cellIndices = base.Map.cellIndices;
			for (int i = cellRect.minX; i <= cellRect.maxX; i++)
			{
				for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
				{
					SectionLayer_Snow.opacityListTmp.Clear();
					float num = depthGrid_Unsafe[cellIndices.CellToIndex(i, j)];
					for (int k = 0; k < 9; k++)
					{
						IntVec3 c = new IntVec3(i, 0, j) + GenAdj.AdjacentCellsAndInsideForUV[k];
						this.adjValuesTmp[k] = (c.InBounds(base.Map) ? depthGrid_Unsafe[cellIndices.CellToIndex(c)] : num);
					}
					for (int l = 0; l < 9; l++)
					{
						float num2 = 0f;
						for (int m = 0; m < SectionLayer_Snow.vertexWeights[l].Count; m++)
						{
							num2 += this.adjValuesTmp[SectionLayer_Snow.vertexWeights[l][m]];
						}
						num2 /= (float)SectionLayer_Snow.vertexWeights[l].Count;
						if (num2 > 0.01f)
						{
							flag = true;
						}
						SectionLayer_Snow.opacityListTmp.Add(num2);
					}
					for (int n = 0; n < 9; n++)
					{
						this.adjValuesTmp[n] = (base.Map.pollutionGrid.IsPolluted(new IntVec3(i, 0, j) + GenAdj.AdjacentCellsAndInsideForUV[n]) ? 1f : 0f);
					}
					for (int num3 = 0; num3 < 9; num3++)
					{
						float num4 = 0f;
						for (int num5 = 0; num5 < SectionLayer_Snow.vertexWeights[num3].Count; num5++)
						{
							num4 += this.adjValuesTmp[SectionLayer_Snow.vertexWeights[num3][num5]];
						}
						num4 /= (float)SectionLayer_Snow.vertexWeights[num3].Count;
						float num6 = SectionLayer_Snow.opacityListTmp[num3];
						subMesh.colors.Add(new Color32(Convert.ToByte(num4 * 255f), byte.MaxValue, byte.MaxValue, Convert.ToByte(num6 * 255f)));
					}
				}
			}
			if (flag)
			{
				subMesh.disabled = false;
				subMesh.FinalizeMesh(MeshParts.Colors);
				return;
			}
			subMesh.disabled = true;
		}

		// Token: 0x040012A6 RID: 4774
		private float[] adjValuesTmp = new float[9];

		// Token: 0x040012A7 RID: 4775
		private static readonly List<float> opacityListTmp = new List<float>();

		// Token: 0x040012A8 RID: 4776
		private static readonly CachedTexture PollutedSnowTex = new CachedTexture("Other/SnowPolluted");

		// Token: 0x040012A9 RID: 4777
		public static readonly List<List<int>> vertexWeights = new List<List<int>>
		{
			new List<int>
			{
				0,
				1,
				2,
				8
			},
			new List<int>
			{
				2,
				8
			},
			new List<int>
			{
				2,
				3,
				4,
				8
			},
			new List<int>
			{
				4,
				8
			},
			new List<int>
			{
				4,
				5,
				6,
				8
			},
			new List<int>
			{
				6,
				8
			},
			new List<int>
			{
				6,
				7,
				0,
				8
			},
			new List<int>
			{
				0,
				8
			},
			new List<int>
			{
				8
			}
		};
	}
}
