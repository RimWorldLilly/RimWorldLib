using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002BF RID: 703
	public class SectionLayer_Terrain : SectionLayer
	{
		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06001577 RID: 5495 RVA: 0x0007F8B9 File Offset: 0x0007DAB9
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawTerrain;
			}
		}

		// Token: 0x06001578 RID: 5496 RVA: 0x0007F8C0 File Offset: 0x0007DAC0
		public SectionLayer_Terrain(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlagDefOf.Terrain;
		}

		// Token: 0x06001579 RID: 5497 RVA: 0x0007F8DC File Offset: 0x0007DADC
		public virtual Material GetMaterialFor(CellTerrain cellTerrain)
		{
			bool polluted = cellTerrain.polluted && cellTerrain.snowCoverage < 0.4f && cellTerrain.sandCoverage < 0.4f && cellTerrain.def.graphicPolluted != BaseContent.BadGraphic && !WorldComponent_GravshipController.DisableDrawingPollution;
			return base.Map.terrainGrid.GetMaterial(cellTerrain.def, polluted, cellTerrain.color);
		}

		// Token: 0x0600157A RID: 5498 RVA: 0x0007F946 File Offset: 0x0007DB46
		public bool AllowRenderingFor(TerrainDef terrain)
		{
			return DebugViewSettings.drawTerrainWater || !terrain.HasTag("Water");
		}

		// Token: 0x0600157B RID: 5499 RVA: 0x0007F960 File Offset: 0x0007DB60
		public override void Regenerate()
		{
			base.ClearSubMeshes(MeshParts.All);
			TerrainGrid terrainGrid = base.Map.terrainGrid;
			CellRect cellRect = this.section.CellRect;
			CellTerrain[] array = new CellTerrain[8];
			HashSet<CellTerrain> hashSet = new HashSet<CellTerrain>();
			bool[] array2 = new bool[8];
			foreach (IntVec3 intVec in cellRect)
			{
				hashSet.Clear();
				TerrainDef terrainDef = terrainGrid.TerrainAt(intVec);
				CellTerrain cellTerrain = new CellTerrain(terrainGrid.TerrainAt(intVec), intVec.IsPolluted(base.Map), base.Map.snowGrid.GetDepth(intVec), intVec.GetSandDepth(base.Map), terrainGrid.ColorAt(intVec));
				LayerSubMesh subMesh = base.GetSubMesh(terrainDef.dontRender ? MatBases.ShadowMask : this.GetMaterialFor(cellTerrain));
				float y = AltitudeLayer.Terrain.AltitudeFor();
				if (subMesh != null && this.AllowRenderingFor(cellTerrain.def))
				{
					int count = subMesh.verts.Count;
					subMesh.verts.Add(new Vector3((float)intVec.x, y, (float)intVec.z));
					subMesh.verts.Add(new Vector3((float)intVec.x, y, (float)(intVec.z + 1)));
					subMesh.verts.Add(new Vector3((float)(intVec.x + 1), y, (float)(intVec.z + 1)));
					subMesh.verts.Add(new Vector3((float)(intVec.x + 1), y, (float)intVec.z));
					subMesh.colors.Add(SectionLayer_Terrain.ColorWhite);
					subMesh.colors.Add(SectionLayer_Terrain.ColorWhite);
					subMesh.colors.Add(SectionLayer_Terrain.ColorWhite);
					subMesh.colors.Add(SectionLayer_Terrain.ColorWhite);
					subMesh.tris.Add(count);
					subMesh.tris.Add(count + 1);
					subMesh.tris.Add(count + 2);
					subMesh.tris.Add(count);
					subMesh.tris.Add(count + 2);
					subMesh.tris.Add(count + 3);
				}
				for (int i = 0; i < 8; i++)
				{
					IntVec3 c = intVec + GenAdj.AdjacentCellsAroundBottom[i];
					if (!c.InBounds(base.Map))
					{
						array[i] = cellTerrain;
					}
					else
					{
						CellTerrain cellTerrain2 = new CellTerrain(terrainGrid.TerrainAt(c), c.IsPolluted(base.Map), base.Map.snowGrid.GetDepth(c), c.GetSandDepth(base.Map), terrainGrid.ColorAt(c));
						Thing edifice = c.GetEdifice(base.Map);
						if (edifice != null && edifice.def.coversFloor)
						{
							cellTerrain2.def = TerrainDefOf.Underwall;
						}
						array[i] = cellTerrain2;
						if (!cellTerrain2.Equals(cellTerrain) && cellTerrain2.def.edgeType != TerrainDef.TerrainEdgeType.Hard && terrainGrid.FoundationAt(intVec) == null && terrainGrid.FoundationAt(c) == null && cellTerrain2.def.renderPrecedence >= cellTerrain.def.renderPrecedence)
						{
							hashSet.Add(cellTerrain2);
						}
					}
				}
				foreach (CellTerrain cellTerrain3 in hashSet)
				{
					LayerSubMesh subMesh2 = base.GetSubMesh(this.GetMaterialFor(cellTerrain3));
					if (subMesh2 != null && this.AllowRenderingFor(cellTerrain3.def))
					{
						int count = subMesh2.verts.Count;
						subMesh2.verts.Add(new Vector3((float)intVec.x + 0.5f, 0f, (float)intVec.z));
						subMesh2.verts.Add(new Vector3((float)intVec.x, 0f, (float)intVec.z));
						subMesh2.verts.Add(new Vector3((float)intVec.x, 0f, (float)intVec.z + 0.5f));
						subMesh2.verts.Add(new Vector3((float)intVec.x, 0f, (float)(intVec.z + 1)));
						subMesh2.verts.Add(new Vector3((float)intVec.x + 0.5f, 0f, (float)(intVec.z + 1)));
						subMesh2.verts.Add(new Vector3((float)(intVec.x + 1), 0f, (float)(intVec.z + 1)));
						subMesh2.verts.Add(new Vector3((float)(intVec.x + 1), 0f, (float)intVec.z + 0.5f));
						subMesh2.verts.Add(new Vector3((float)(intVec.x + 1), 0f, (float)intVec.z));
						subMesh2.verts.Add(new Vector3((float)intVec.x + 0.5f, 0f, (float)intVec.z + 0.5f));
						for (int j = 0; j < 8; j++)
						{
							array2[j] = false;
						}
						for (int k = 0; k < 8; k++)
						{
							if (k % 2 == 0)
							{
								if (array[k].Equals(cellTerrain3))
								{
									array2[(k - 1 + 8) % 8] = true;
									array2[k] = true;
									array2[(k + 1) % 8] = true;
								}
							}
							else if (array[k].Equals(cellTerrain3))
							{
								array2[k] = true;
							}
						}
						for (int l = 0; l < 8; l++)
						{
							subMesh2.colors.Add(array2[l] ? SectionLayer_Terrain.ColorWhite : SectionLayer_Terrain.ColorClear);
						}
						subMesh2.colors.Add(SectionLayer_Terrain.ColorClear);
						for (int m = 0; m < 8; m++)
						{
							subMesh2.tris.Add(count + m);
							subMesh2.tris.Add(count + (m + 1) % 8);
							subMesh2.tris.Add(count + 8);
						}
					}
				}
			}
			base.FinalizeMesh(MeshParts.All);
		}

		// Token: 0x040012B2 RID: 4786
		private static readonly Color32 ColorWhite = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		// Token: 0x040012B3 RID: 4787
		private static readonly Color32 ColorClear = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);

		// Token: 0x040012B4 RID: 4788
		public const float MaxWeatherBuildupCoverageForVisualPollution = 0.4f;
	}
}
