using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002B0 RID: 688
	public class SectionLayer_Darkness : SectionLayer
	{
		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06001516 RID: 5398 RVA: 0x0007B491 File Offset: 0x00079691
		public override bool Visible
		{
			get
			{
				return base.Map.gameConditionManager.DarknessVisible;
			}
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x0007B4A4 File Offset: 0x000796A4
		public SectionLayer_Darkness(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlagDefOf.GroundGlow;
			this.propertyBlock = new MaterialPropertyBlock();
			this.cachedLight = new float[section.CellRect.ExpandedBy(1).Area];
			for (int i = 0; i < 9; i++)
			{
				this.vertLight[i] = new List<float>(4);
			}
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x0007B520 File Offset: 0x00079720
		public override void DrawLayer()
		{
			if (!this.Visible)
			{
				return;
			}
			float a = Mathf.Clamp01(1f - Mathf.Max(base.Map.gameConditionManager.MapBrightness, base.Map.skyManager.CurSkyGlow));
			this.propertyBlock.SetColor(ShaderPropertyIDs.ColorTwo, new Color(1f, 1f, 1f, a));
			this.propertyBlock.SetFloat(ShaderPropertyIDs.AgeSecsPausable, RealTime.UnpausedRealTime);
			int count = this.subMeshes.Count;
			for (int i = 0; i < count; i++)
			{
				LayerSubMesh layerSubMesh = this.subMeshes[i];
				if (layerSubMesh.finalized && !layerSubMesh.disabled)
				{
					Graphics.DrawMesh(layerSubMesh.mesh, Vector3.zero, Quaternion.identity, layerSubMesh.material, 0, null, 0, this.propertyBlock);
				}
			}
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x0007B5FC File Offset: 0x000797FC
		public override void Regenerate()
		{
			if (!ModsConfig.AnomalyActive || !this.Visible)
			{
				return;
			}
			LayerSubMesh subMesh = base.GetSubMesh(MatBases.Darkness);
			if (subMesh.mesh.vertexCount == 0)
			{
				SectionLayerGeometryMaker_Solid.MakeBaseGeometry(this.section, subMesh, AltitudeLayer.Darkness);
			}
			subMesh.Clear(MeshParts.Colors);
			CellRect cellRect = this.section.CellRect;
			int num = cellRect.Width + 2;
			int num2 = base.Map.Size.z - 1;
			int num3 = base.Map.Size.x - 1;
			for (int i = cellRect.minX - 1; i <= cellRect.maxX + 1; i++)
			{
				for (int j = cellRect.minZ - 1; j <= cellRect.maxZ + 1; j++)
				{
					int num4 = i - cellRect.minX + 1;
					int num5 = j - cellRect.minZ + 1;
					this.cachedLight[num5 * num + num4] = this.LightAt(i, j);
				}
			}
			for (int k = cellRect.minX; k <= cellRect.maxX; k++)
			{
				for (int l = cellRect.minZ; l <= cellRect.maxZ; l++)
				{
					int num6 = k - cellRect.minX + 1;
					int num7 = l - cellRect.minZ + 1;
					float item = this.cachedLight[num7 * num + num6];
					for (int m = 0; m < 9; m++)
					{
						this.vertLight[m].Clear();
						this.vertLight[m].Add(item);
					}
					if (l < num2)
					{
						item = this.cachedLight[(num7 + 1) * num + num6];
						this.vertLight[2].Add(item);
						this.vertLight[3].Add(item);
						this.vertLight[4].Add(item);
					}
					if (l > 0)
					{
						item = this.cachedLight[(num7 - 1) * num + num6];
						this.vertLight[6].Add(item);
						this.vertLight[7].Add(item);
						this.vertLight[0].Add(item);
					}
					if (k < num3)
					{
						item = this.cachedLight[num7 * num + (num6 + 1)];
						this.vertLight[4].Add(item);
						this.vertLight[5].Add(item);
						this.vertLight[6].Add(item);
					}
					if (k > 0)
					{
						item = this.cachedLight[num7 * num + (num6 - 1)];
						this.vertLight[0].Add(item);
						this.vertLight[1].Add(item);
						this.vertLight[2].Add(item);
					}
					if (l > 0 && k > 0)
					{
						this.vertLight[0].Add(this.cachedLight[(num7 - 1) * num + (num6 - 1)]);
					}
					if (l < num2 && k > 0)
					{
						this.vertLight[2].Add(this.cachedLight[(num7 + 1) * num + (num6 - 1)]);
					}
					if (l < num2 && k < num3)
					{
						this.vertLight[4].Add(this.cachedLight[(num7 + 1) * num + (num6 + 1)]);
					}
					if (l > 0 && k < num3)
					{
						this.vertLight[6].Add(this.cachedLight[(num7 - 1) * num + (num6 + 1)]);
					}
					for (int n = 0; n < 9; n++)
					{
						float num8 = 0f;
						if (this.vertLight[n].Count > 0)
						{
							for (int num9 = 0; num9 < this.vertLight[n].Count; num9++)
							{
								num8 += this.vertLight[n][num9];
							}
							num8 /= (float)this.vertLight[n].Count;
							num8 = 1f - num8;
						}
						subMesh.colors.Add(new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(Mathf.Min(num8, 1f) * 255f)));
					}
				}
			}
			subMesh.FinalizeMesh(MeshParts.Colors);
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x0007B9FC File Offset: 0x00079BFC
		private float LightAt(int x, int z)
		{
			IntVec3 intVec = new IntVec3(x, 0, z);
			if (!intVec.InBounds(base.Map) || base.Map.fogGrid.IsFogged(intVec))
			{
				return 0f;
			}
			if (this.LightBlockingEdificeAt(intVec))
			{
				float num = base.Map.glowGrid.GroundGlowAt(intVec, false, true);
				for (int i = 0; i < 8; i++)
				{
					IntVec3 c = intVec + GenAdj.AdjacentCells[i];
					if (c.InBounds(base.Map) && !base.Map.fogGrid.IsFogged(c) && !this.LightBlockingEdificeAt(c))
					{
						num = Mathf.Max(num, base.Map.glowGrid.GroundGlowAt(c, false, true));
					}
				}
				return num;
			}
			return base.Map.glowGrid.GroundGlowAt(intVec, false, true);
		}

		// Token: 0x0600151B RID: 5403 RVA: 0x0007BAD0 File Offset: 0x00079CD0
		private bool LightBlockingEdificeAt(IntVec3 c)
		{
			Building building = base.Map.edificeGrid[c];
			return building != null && building.def.blockLight;
		}

		// Token: 0x04001284 RID: 4740
		private List<float>[] vertLight = new List<float>[9];

		// Token: 0x04001285 RID: 4741
		private MaterialPropertyBlock propertyBlock;

		// Token: 0x04001286 RID: 4742
		private float[] cachedLight;

		// Token: 0x04001287 RID: 4743
		private const float MaxDarkness = 1f;
	}
}
