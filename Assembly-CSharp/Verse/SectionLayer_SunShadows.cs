using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002BD RID: 701
	internal class SectionLayer_SunShadows : SectionLayer_Dynamic
	{
		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x0600156B RID: 5483 RVA: 0x0007F044 File Offset: 0x0007D244
		public override bool Visible
		{
			get
			{
				if (DebugViewSettings.drawShadows)
				{
					Map map = base.Map;
					bool? flag;
					if (map == null)
					{
						flag = null;
					}
					else
					{
						BiomeDef biome = map.Biome;
						flag = ((biome != null) ? new bool?(biome.disableShadows) : null);
					}
					bool? flag2 = flag;
					return !flag2.GetValueOrDefault();
				}
				return false;
			}
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x0007F098 File Offset: 0x0007D298
		public SectionLayer_SunShadows(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlagDefOf.Buildings;
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x0007F0B4 File Offset: 0x0007D2B4
		public override bool ShouldDrawDynamic(CellRect view)
		{
			return this.section.CellRect.Overlaps(SectionLayer_SunShadows.GetSunShadowsViewRect(this.section.map, view));
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x0007F0E5 File Offset: 0x0007D2E5
		public override CellRect GetBoundaryRect()
		{
			return new CellRect(0, 0, this.section.map.Size.x, this.section.map.Size.z);
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x0007F118 File Offset: 0x0007D318
		public override void DrawLayer()
		{
			this.RefreshSubMeshBounds();
			base.DrawLayer();
		}

		// Token: 0x06001570 RID: 5488 RVA: 0x0007F128 File Offset: 0x0007D328
		public override void Regenerate()
		{
			if (!MatBases.SunShadow.shader.isSupported)
			{
				return;
			}
			Building[] innerArray = base.Map.edificeGrid.InnerArray;
			float y = AltitudeLayer.Shadows.AltitudeFor();
			CellRect cellRect = new CellRect(this.section.botLeft.x, this.section.botLeft.z, 17, 17);
			cellRect.ClipInsideMap(base.Map);
			LayerSubMesh subMesh = base.GetSubMesh(MatBases.SunShadow);
			subMesh.Clear(MeshParts.All);
			subMesh.verts.Capacity = cellRect.Area * 2;
			subMesh.tris.Capacity = cellRect.Area * 4;
			subMesh.colors.Capacity = cellRect.Area * 2;
			CellIndices cellIndices = base.Map.cellIndices;
			for (int i = cellRect.minX; i <= cellRect.maxX; i++)
			{
				for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
				{
					Building building = innerArray[cellIndices.CellToIndex(i, j)];
					if (building != null && building.def.staticSunShadowHeight > 0f)
					{
						float staticSunShadowHeight = building.def.staticSunShadowHeight;
						Color32 item = new Color32(0, 0, 0, (byte)(255f * staticSunShadowHeight));
						int count = subMesh.verts.Count;
						subMesh.verts.Add(new Vector3((float)i, y, (float)j));
						subMesh.verts.Add(new Vector3((float)i, y, (float)(j + 1)));
						subMesh.verts.Add(new Vector3((float)(i + 1), y, (float)(j + 1)));
						subMesh.verts.Add(new Vector3((float)(i + 1), y, (float)j));
						subMesh.colors.Add(SectionLayer_SunShadows.LowVertexColor);
						subMesh.colors.Add(SectionLayer_SunShadows.LowVertexColor);
						subMesh.colors.Add(SectionLayer_SunShadows.LowVertexColor);
						subMesh.colors.Add(SectionLayer_SunShadows.LowVertexColor);
						int count2 = subMesh.verts.Count;
						subMesh.tris.Add(count2 - 4);
						subMesh.tris.Add(count2 - 3);
						subMesh.tris.Add(count2 - 2);
						subMesh.tris.Add(count2 - 4);
						subMesh.tris.Add(count2 - 2);
						subMesh.tris.Add(count2 - 1);
						if (i > 0)
						{
							building = innerArray[cellIndices.CellToIndex(i - 1, j)];
							if (building == null || building.def.staticSunShadowHeight < staticSunShadowHeight)
							{
								int count3 = subMesh.verts.Count;
								subMesh.verts.Add(new Vector3((float)i, y, (float)j));
								subMesh.verts.Add(new Vector3((float)i, y, (float)(j + 1)));
								subMesh.colors.Add(item);
								subMesh.colors.Add(item);
								subMesh.tris.Add(count + 1);
								subMesh.tris.Add(count);
								subMesh.tris.Add(count3);
								subMesh.tris.Add(count3);
								subMesh.tris.Add(count3 + 1);
								subMesh.tris.Add(count + 1);
							}
						}
						if (i < base.Map.Size.x - 1)
						{
							building = innerArray[cellIndices.CellToIndex(i + 1, j)];
							if (building == null || building.def.staticSunShadowHeight < staticSunShadowHeight)
							{
								int count4 = subMesh.verts.Count;
								subMesh.verts.Add(new Vector3((float)(i + 1), y, (float)(j + 1)));
								subMesh.verts.Add(new Vector3((float)(i + 1), y, (float)j));
								subMesh.colors.Add(item);
								subMesh.colors.Add(item);
								subMesh.tris.Add(count + 2);
								subMesh.tris.Add(count4);
								subMesh.tris.Add(count4 + 1);
								subMesh.tris.Add(count4 + 1);
								subMesh.tris.Add(count + 3);
								subMesh.tris.Add(count + 2);
							}
						}
						if (j > 0)
						{
							building = innerArray[cellIndices.CellToIndex(i, j - 1)];
							if (building == null || building.def.staticSunShadowHeight < staticSunShadowHeight)
							{
								int count5 = subMesh.verts.Count;
								subMesh.verts.Add(new Vector3((float)i, y, (float)j));
								subMesh.verts.Add(new Vector3((float)(i + 1), y, (float)j));
								subMesh.colors.Add(item);
								subMesh.colors.Add(item);
								subMesh.tris.Add(count);
								subMesh.tris.Add(count + 3);
								subMesh.tris.Add(count5);
								subMesh.tris.Add(count + 3);
								subMesh.tris.Add(count5 + 1);
								subMesh.tris.Add(count5);
							}
						}
					}
				}
			}
			if (subMesh.verts.Count > 0)
			{
				subMesh.FinalizeMesh(MeshParts.Verts | MeshParts.Tris | MeshParts.Colors);
				float num = Mathf.Max(15f, 15f);
				Vector3 size = subMesh.mesh.bounds.size;
				size.x += 2f * num + 2f;
				size.z += 2f * num + 2f;
				subMesh.mesh.bounds = new Bounds(Vector3.zero, new Vector3(1000f, 1000f, 1000f));
			}
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x0007F6E0 File Offset: 0x0007D8E0
		public static CellRect GetSunShadowsViewRect(Map map, CellRect rect)
		{
			if (SectionLayer_SunShadows.frame == RealTime.frameCount)
			{
				return SectionLayer_SunShadows.cachedRect;
			}
			GenCelestial.LightInfo lightSourceInfo = GenCelestial.GetLightSourceInfo(map, GenCelestial.LightType.Shadow);
			if (lightSourceInfo.vector.x < 0f)
			{
				rect.maxX -= Mathf.FloorToInt(lightSourceInfo.vector.x);
			}
			else
			{
				rect.minX -= Mathf.CeilToInt(lightSourceInfo.vector.x);
			}
			if (lightSourceInfo.vector.y < 0f)
			{
				rect.maxZ -= Mathf.FloorToInt(lightSourceInfo.vector.y);
			}
			else
			{
				rect.minZ -= Mathf.CeilToInt(lightSourceInfo.vector.y);
			}
			SectionLayer_SunShadows.frame = RealTime.frameCount;
			return SectionLayer_SunShadows.cachedRect = rect.ClipInsideMap(map);
		}

		// Token: 0x040012AA RID: 4778
		private static readonly Color32 LowVertexColor = new Color32(0, 0, 0, 0);

		// Token: 0x040012AB RID: 4779
		private static int frame;

		// Token: 0x040012AC RID: 4780
		private static CellRect cachedRect;
	}
}
