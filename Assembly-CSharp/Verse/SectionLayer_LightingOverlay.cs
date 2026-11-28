using System;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002B8 RID: 696
	[StaticConstructorOnStartup]
	public class SectionLayer_LightingOverlay : SectionLayer
	{
		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x0600154C RID: 5452 RVA: 0x0007D8BD File Offset: 0x0007BABD
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawLightingOverlay;
			}
		}

		// Token: 0x0600154D RID: 5453 RVA: 0x0007D8C4 File Offset: 0x0007BAC4
		public SectionLayer_LightingOverlay(Section section) : base(section)
		{
			this.relevantChangeTypes = (MapMeshFlagDefOf.Roofs | MapMeshFlagDefOf.GroundGlow);
		}

		// Token: 0x0600154E RID: 5454 RVA: 0x0007D8E8 File Offset: 0x0007BAE8
		public string GlowReportAt(IntVec3 c)
		{
			Color32[] colors = base.GetSubMesh(MatBases.LightOverlay).mesh.colors32;
			int num;
			int num2;
			int num3;
			int num4;
			int num5;
			SectionLayer_LightingOverlay.CalculateVertexIndices(this.sectRect, c.x, c.z, this.firstCenterInd, out num, out num2, out num3, out num4, out num5);
			StringBuilder stringBuilder = new StringBuilder();
			string str = "BL=";
			Color32 color = colors[num];
			stringBuilder.Append(str + color.ToString());
			string str2 = "\nTL=";
			color = colors[num2];
			stringBuilder.Append(str2 + color.ToString());
			string str3 = "\nTR=";
			color = colors[num3];
			stringBuilder.Append(str3 + color.ToString());
			string str4 = "\nBR=";
			color = colors[num4];
			stringBuilder.Append(str4 + color.ToString());
			string str5 = "\nCenter=";
			color = colors[num5];
			stringBuilder.Append(str5 + color.ToString());
			return stringBuilder.ToString();
		}

		// Token: 0x0600154F RID: 5455 RVA: 0x0007DA04 File Offset: 0x0007BC04
		public static LayerSubMesh Bake(Map map, CellRect sectRect, Material mat, Predicate<int> filter)
		{
			LayerSubMesh layerSubMesh = MapDrawLayer.CreateFreeSubMesh(mat ?? MatBases.LightOverlay, map);
			int num = 0;
			SectionLayer_LightingOverlay.GenerateLightingOverlay(map, layerSubMesh, sectRect, ref num, true, filter);
			return layerSubMesh;
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x0007DA34 File Offset: 0x0007BC34
		public override void Regenerate()
		{
			LayerSubMesh subMesh = base.GetSubMesh(MatBases.LightOverlay);
			if (subMesh.verts.Count == 0)
			{
				this.sectRect = new CellRect(this.section.botLeft.x, this.section.botLeft.z, 17, 17);
				this.sectRect.ClipInsideMap(base.Map);
			}
			SectionLayer_LightingOverlay.GenerateLightingOverlay(base.Map, subMesh, this.sectRect, ref this.firstCenterInd, false, null);
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x0007DAB8 File Offset: 0x0007BCB8
		private static void GenerateLightingOverlay(Map map, LayerSubMesh subMesh, CellRect rect, ref int firstCenterInd, bool centered = false, Predicate<int> filter = null)
		{
			rect.ClipInsideMap(map);
			if (subMesh.verts.Count == 0)
			{
				SectionLayer_LightingOverlay.MakeBaseGeometry(map, subMesh, rect, out firstCenterInd, centered);
			}
			Color32[] array = new Color32[subMesh.verts.Count];
			int maxX = rect.maxX;
			int maxZ = rect.maxZ;
			int width = rect.Width;
			int x = map.Size.x;
			Thing[] innerArray = map.edificeGrid.InnerArray;
			Thing[] array2 = innerArray;
			int num = array2.Length;
			RoofGrid roofGrid = map.roofGrid;
			CellIndices cellIndices = map.cellIndices;
			int num2;
			int num3;
			int num4;
			int num5;
			int num6;
			SectionLayer_LightingOverlay.CalculateVertexIndices(rect, firstCenterInd, rect.minX, rect.minZ, out num2, out num3, out num4, out num5, out num6);
			int num7 = cellIndices.CellToIndex(new IntVec3(rect.minX, 0, rect.minZ));
			int[] array3 = new int[4];
			array3[0] = -map.Size.x - 1;
			array3[1] = -map.Size.x;
			array3[2] = -1;
			int[] array4 = array3;
			int[] array5 = new int[4];
			array5[0] = -1;
			array5[1] = -1;
			int[] array6 = array5;
			for (int i = rect.minZ; i <= maxZ + 1; i++)
			{
				int num8 = num7 / x;
				int j = rect.minX;
				while (j <= maxX + 1)
				{
					if (filter != null && !filter(num7))
					{
						array[num2] = new Color32(0, 0, 0, 0);
					}
					else
					{
						ColorInt colorInt = new ColorInt(0, 0, 0, 0);
						int num9 = 0;
						bool flag = false;
						for (int k = 0; k < 4; k++)
						{
							int num10 = num7 + array4[k];
							if (num10 >= 0 && num10 < num && num10 / x == num8 + array6[k])
							{
								Thing thing = array2[num10];
								RoofDef roofDef = roofGrid.RoofAt(num10);
								if (roofDef != null && (roofDef.isThickRoof || thing == null || !thing.def.holdsRoof || thing.def.altitudeLayer == AltitudeLayer.DoorMoveable))
								{
									flag = true;
								}
								if (thing == null || !thing.def.blockLight)
								{
									colorInt += map.glowGrid.VisualGlowAt(num10);
									num9++;
								}
							}
						}
						if (num9 > 0)
						{
							array[num2] = (colorInt / num9).ProjectToColor32();
						}
						else
						{
							array[num2] = new Color32(0, 0, 0, 0);
						}
						if (flag && array[num2].a < 100)
						{
							array[num2].a = 100;
						}
					}
					j++;
					num2++;
					num7++;
				}
				int num11 = maxX + 2 - rect.minX;
				num2 -= num11;
				num7 -= num11;
				num2 += width + 1;
				num7 += map.Size.x;
			}
			int num12;
			int num13;
			SectionLayer_LightingOverlay.CalculateVertexIndices(rect, firstCenterInd, rect.minX, rect.minZ, out num12, out num6, out num5, out num4, out num13);
			int num14 = cellIndices.CellToIndex(rect.minX, rect.minZ);
			for (int l = rect.minZ; l <= maxZ; l++)
			{
				int m = rect.minX;
				while (m <= maxX)
				{
					if (filter != null && !filter(num14))
					{
						array[num13] = new Color32(0, 0, 0, 0);
					}
					else
					{
						ColorInt colorInt2 = default(ColorInt) + array[num12];
						colorInt2 += array[num12 + 1];
						colorInt2 += array[num12 + width + 1];
						colorInt2 += array[num12 + width + 2];
						array[num13] = new Color32((byte)(colorInt2.r / 4), (byte)(colorInt2.g / 4), (byte)(colorInt2.b / 4), (byte)(colorInt2.a / 4));
						if (array[num13].a < 100 && roofGrid.Roofed(num14))
						{
							Thing thing2 = array2[num14];
							if (thing2 == null || !thing2.def.holdsRoof)
							{
								array[num13].a = 100;
							}
						}
					}
					m++;
					num12++;
					num13++;
					num14++;
				}
				num12++;
				num14 -= width;
				num14 += map.Size.x;
			}
			subMesh.mesh.colors32 = array;
		}

		// Token: 0x06001552 RID: 5458 RVA: 0x0007DF0C File Offset: 0x0007C10C
		private static void MakeBaseGeometry(Map map, LayerSubMesh sm, CellRect sectRect, out int firstCenterInd, bool centered = false)
		{
			sectRect.ClipInsideMap(map);
			float num = centered ? ((float)(-(float)sectRect.minX) - (float)sectRect.Width / 2f) : 0f;
			float num2 = centered ? ((float)(-(float)sectRect.minZ) - (float)sectRect.Height / 2f) : 0f;
			int capacity = (sectRect.Width + 1) * (sectRect.Height + 1) + sectRect.Area;
			float y = AltitudeLayer.LightingOverlay.AltitudeFor();
			sm.verts.Capacity = capacity;
			for (int i = sectRect.minZ; i <= sectRect.maxZ + 1; i++)
			{
				for (int j = sectRect.minX; j <= sectRect.maxX + 1; j++)
				{
					sm.verts.Add(new Vector3((float)j + num, y, (float)i + num2));
				}
			}
			firstCenterInd = sm.verts.Count;
			for (int k = sectRect.minZ; k <= sectRect.maxZ; k++)
			{
				for (int l = sectRect.minX; l <= sectRect.maxX; l++)
				{
					sm.verts.Add(new Vector3((float)l + num + 0.5f, y, (float)k + num2 + 0.5f));
				}
			}
			sm.tris.Capacity = sectRect.Area * 4 * 3;
			for (int m = sectRect.minZ; m <= sectRect.maxZ; m++)
			{
				for (int n = sectRect.minX; n <= sectRect.maxX; n++)
				{
					int item;
					int item2;
					int item3;
					int item4;
					int item5;
					SectionLayer_LightingOverlay.CalculateVertexIndices(sectRect, firstCenterInd, n, m, out item, out item2, out item3, out item4, out item5);
					sm.tris.Add(item);
					sm.tris.Add(item5);
					sm.tris.Add(item4);
					sm.tris.Add(item);
					sm.tris.Add(item2);
					sm.tris.Add(item5);
					sm.tris.Add(item2);
					sm.tris.Add(item3);
					sm.tris.Add(item5);
					sm.tris.Add(item3);
					sm.tris.Add(item4);
					sm.tris.Add(item5);
				}
			}
			sm.FinalizeMesh(MeshParts.Verts | MeshParts.Tris);
		}

		// Token: 0x06001553 RID: 5459 RVA: 0x0007E164 File Offset: 0x0007C364
		private static void CalculateVertexIndices(CellRect sectRect, int firstCenterInd, int worldX, int worldZ, out int botLeft, out int topLeft, out int topRight, out int botRight, out int center)
		{
			int num = worldX - sectRect.minX;
			int num2 = worldZ - sectRect.minZ;
			botLeft = num2 * (sectRect.Width + 1) + num;
			topLeft = (num2 + 1) * (sectRect.Width + 1) + num;
			topRight = (num2 + 1) * (sectRect.Width + 1) + (num + 1);
			botRight = num2 * (sectRect.Width + 1) + (num + 1);
			center = firstCenterInd + (num2 * sectRect.Width + num);
		}

		// Token: 0x04001293 RID: 4755
		private int firstCenterInd;

		// Token: 0x04001294 RID: 4756
		private CellRect sectRect;

		// Token: 0x04001295 RID: 4757
		private const byte RoofedAreaMinSkyCover = 100;
	}
}
