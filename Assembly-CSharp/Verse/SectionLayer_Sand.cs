using System;
using System.Collections.Generic;
using RimWorld;
using Unity.Collections;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002BB RID: 699
	[StaticConstructorOnStartup]
	public class SectionLayer_Sand : SectionLayer
	{
		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06001561 RID: 5473 RVA: 0x0007E6FF File Offset: 0x0007C8FF
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawSand && ModsConfig.OdysseyActive;
			}
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x0007E70F File Offset: 0x0007C90F
		public SectionLayer_Sand(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlagDefOf.Sand;
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x0007E738 File Offset: 0x0007C938
		private bool Filled(int index)
		{
			Building building = base.Map.edificeGrid[index];
			return building != null && building.def.Fillage == FillCategory.Full;
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x0007E76C File Offset: 0x0007C96C
		public override void Regenerate()
		{
			LayerSubMesh subMesh = base.GetSubMesh(MatBases.Sand);
			if (ModsConfig.BiotechActive)
			{
				subMesh.material.SetTexture(ShaderPropertyIDs.PollutedTex, SectionLayer_Sand.PollutedSandTex.Texture);
			}
			if (subMesh.mesh.vertexCount == 0)
			{
				SectionLayerGeometryMaker_Solid.MakeBaseGeometry(this.section, subMesh, AltitudeLayer.Terrain);
			}
			SectionLayer_Sand.opacityListTmp.Clear();
			subMesh.Clear(MeshParts.Colors);
			if (!ModsConfig.OdysseyActive)
			{
				subMesh.disabled = true;
				return;
			}
			NativeArray<float> depthGrid_Unsafe = base.Map.sandGrid.DepthGrid_Unsafe;
			CellRect cellRect = this.section.CellRect;
			bool flag = false;
			CellIndices cellIndices = base.Map.cellIndices;
			for (int i = cellRect.minX; i <= cellRect.maxX; i++)
			{
				for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
				{
					SectionLayer_Sand.opacityListTmp.Clear();
					float num = depthGrid_Unsafe[cellIndices.CellToIndex(i, j)];
					for (int k = 0; k < 9; k++)
					{
						IntVec3 c = new IntVec3(i, 0, j) + GenAdj.AdjacentCellsAndInsideForUV[k];
						this.adjValuesTmp[k] = (c.InBounds(base.Map) ? depthGrid_Unsafe[cellIndices.CellToIndex(c)] : num);
					}
					for (int l = 0; l < 9; l++)
					{
						float num2 = 0f;
						for (int m = 0; m < SectionLayer_Sand.vertexWeights[l].Count; m++)
						{
							num2 += this.adjValuesTmp[SectionLayer_Sand.vertexWeights[l][m]];
						}
						num2 /= (float)SectionLayer_Sand.vertexWeights[l].Count;
						if (num2 > 0.01f)
						{
							flag = true;
						}
						SectionLayer_Sand.opacityListTmp.Add(num2);
					}
					for (int n = 0; n < 9; n++)
					{
						this.adjValuesTmp[n] = (base.Map.pollutionGrid.IsPolluted(new IntVec3(i, 0, j) + GenAdj.AdjacentCellsAndInsideForUV[n]) ? 1f : 0f);
					}
					for (int num3 = 0; num3 < 9; num3++)
					{
						float num4 = 0f;
						for (int num5 = 0; num5 < SectionLayer_Sand.vertexWeights[num3].Count; num5++)
						{
							num4 += this.adjValuesTmp[SectionLayer_Sand.vertexWeights[num3][num5]];
						}
						num4 /= (float)SectionLayer_Sand.vertexWeights[num3].Count;
						float num6 = SectionLayer_Sand.opacityListTmp[num3];
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

		// Token: 0x040012A2 RID: 4770
		private float[] adjValuesTmp = new float[9];

		// Token: 0x040012A3 RID: 4771
		private static readonly List<float> opacityListTmp = new List<float>();

		// Token: 0x040012A4 RID: 4772
		private static readonly CachedTexture PollutedSandTex = new CachedTexture("Other/WindsweptSandPolluted");

		// Token: 0x040012A5 RID: 4773
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
