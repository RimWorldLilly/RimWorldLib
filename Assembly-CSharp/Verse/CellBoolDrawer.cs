using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000240 RID: 576
	public class CellBoolDrawer
	{
		// Token: 0x06001100 RID: 4352 RVA: 0x00060FB4 File Offset: 0x0005F1B4
		private CellBoolDrawer(int mapSizeX, int mapSizeZ, float opacity = 0.33f)
		{
			this.mapSizeX = mapSizeX;
			this.mapSizeZ = mapSizeZ;
			this.opacity = opacity;
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x00061004 File Offset: 0x0005F204
		public CellBoolDrawer(ICellBoolGiver giver, int mapSizeX, int mapSizeZ, float opacity = 0.33f) : this(mapSizeX, mapSizeZ, opacity)
		{
			this.colorGetter = (() => giver.Color);
			this.extraColorGetter = new Func<int, Color>(giver.GetCellExtraColor);
			this.cellBoolGetter = new Func<int, bool>(giver.GetCellBool);
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x0006106A File Offset: 0x0005F26A
		public CellBoolDrawer(ICellBoolGiver giver, int mapSizeX, int mapSizeZ, int renderQueue, float opacity = 0.33f) : this(giver, mapSizeX, mapSizeZ, opacity)
		{
			this.renderQueue = renderQueue;
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x0006107F File Offset: 0x0005F27F
		public CellBoolDrawer(Func<int, bool> cellBoolGetter, Func<Color> colorGetter, Func<int, Color> extraColorGetter, int mapSizeX, int mapSizeZ, float opacity = 0.33f) : this(mapSizeX, mapSizeZ, opacity)
		{
			this.colorGetter = colorGetter;
			this.extraColorGetter = extraColorGetter;
			this.cellBoolGetter = cellBoolGetter;
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x000610A2 File Offset: 0x0005F2A2
		public CellBoolDrawer(Func<int, bool> cellBoolGetter, Func<Color> colorGetter, Func<int, Color> extraColorGetter, int mapSizeX, int mapSizeZ, int renderQueue, float opacity = 0.33f) : this(cellBoolGetter, colorGetter, extraColorGetter, mapSizeX, mapSizeZ, opacity)
		{
			this.renderQueue = renderQueue;
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x000610BB File Offset: 0x0005F2BB
		public void MarkForDraw()
		{
			this.wantDraw = true;
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x000610C4 File Offset: 0x0005F2C4
		public void CellBoolDrawerUpdate()
		{
			if (!this.wantDraw)
			{
				return;
			}
			this.ActuallyDraw();
			this.wantDraw = false;
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x000610DC File Offset: 0x0005F2DC
		private void ActuallyDraw()
		{
			if (this.dirty)
			{
				this.RegenerateMesh();
			}
			for (int i = 0; i < this.meshes.Count; i++)
			{
				Graphics.DrawMesh(this.meshes[i], Vector3.zero, Quaternion.identity, this.material, 0);
			}
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x0006112F File Offset: 0x0005F32F
		public void SetDirty()
		{
			this.dirty = true;
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x00061138 File Offset: 0x0005F338
		public void RegenerateMesh()
		{
			for (int i = 0; i < this.meshes.Count; i++)
			{
				this.meshes[i].Clear();
			}
			int num = 0;
			int num2 = 0;
			if (this.meshes.Count < 1)
			{
				Mesh item = new Mesh
				{
					name = "CellBoolDrawer"
				};
				this.meshes.Add(item);
			}
			Mesh mesh = this.meshes[num];
			CellRect cellRect = new CellRect(0, 0, this.mapSizeX, this.mapSizeZ);
			float y = AltitudeLayer.MapDataOverlay.AltitudeFor();
			bool careAboutVertexColors = false;
			for (int j = cellRect.minX; j <= cellRect.maxX; j++)
			{
				for (int k = cellRect.minZ; k <= cellRect.maxZ; k++)
				{
					int arg = CellIndicesUtility.CellToIndex(j, k, this.mapSizeX);
					if (this.cellBoolGetter(arg))
					{
						CellBoolDrawer.verts.Add(new Vector3((float)j, y, (float)k));
						CellBoolDrawer.verts.Add(new Vector3((float)j, y, (float)(k + 1)));
						CellBoolDrawer.verts.Add(new Vector3((float)(j + 1), y, (float)(k + 1)));
						CellBoolDrawer.verts.Add(new Vector3((float)(j + 1), y, (float)k));
						Color color = this.extraColorGetter(arg);
						CellBoolDrawer.colors.Add(color);
						CellBoolDrawer.colors.Add(color);
						CellBoolDrawer.colors.Add(color);
						CellBoolDrawer.colors.Add(color);
						if (color != Color.white)
						{
							careAboutVertexColors = true;
						}
						int count = CellBoolDrawer.verts.Count;
						CellBoolDrawer.tris.Add(count - 4);
						CellBoolDrawer.tris.Add(count - 3);
						CellBoolDrawer.tris.Add(count - 2);
						CellBoolDrawer.tris.Add(count - 4);
						CellBoolDrawer.tris.Add(count - 2);
						CellBoolDrawer.tris.Add(count - 1);
						num2++;
						if (num2 >= 16383)
						{
							this.FinalizeWorkingDataIntoMesh(mesh);
							num++;
							if (this.meshes.Count < num + 1)
							{
								Mesh item2 = new Mesh
								{
									name = "CellBoolDrawer"
								};
								this.meshes.Add(item2);
							}
							mesh = this.meshes[num];
							num2 = 0;
						}
					}
				}
			}
			this.FinalizeWorkingDataIntoMesh(mesh);
			this.CreateMaterialIfNeeded(careAboutVertexColors);
			this.dirty = false;
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x000613B4 File Offset: 0x0005F5B4
		private void FinalizeWorkingDataIntoMesh(Mesh mesh)
		{
			if (CellBoolDrawer.verts.Count > 0)
			{
				mesh.SetVertices(CellBoolDrawer.verts);
				CellBoolDrawer.verts.Clear();
				mesh.SetTriangles(CellBoolDrawer.tris, 0);
				CellBoolDrawer.tris.Clear();
				mesh.SetColors(CellBoolDrawer.colors);
				CellBoolDrawer.colors.Clear();
			}
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x00061410 File Offset: 0x0005F610
		private void CreateMaterialIfNeeded(bool careAboutVertexColors)
		{
			if (this.material == null || this.materialCaresAboutVertexColors != careAboutVertexColors)
			{
				Color color = this.colorGetter();
				this.material = SolidColorMaterials.SimpleSolidColorMaterial(new Color(color.r, color.g, color.b, this.opacity * color.a), careAboutVertexColors);
				this.materialCaresAboutVertexColors = careAboutVertexColors;
				this.material.renderQueue = this.renderQueue;
			}
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x00061488 File Offset: 0x0005F688
		public void Notify_ColorChanged()
		{
			this.material = null;
			this.SetDirty();
		}

		// Token: 0x04000F86 RID: 3974
		private bool wantDraw;

		// Token: 0x04000F87 RID: 3975
		private Material material;

		// Token: 0x04000F88 RID: 3976
		private bool materialCaresAboutVertexColors;

		// Token: 0x04000F89 RID: 3977
		private bool dirty = true;

		// Token: 0x04000F8A RID: 3978
		private List<Mesh> meshes = new List<Mesh>();

		// Token: 0x04000F8B RID: 3979
		private int mapSizeX;

		// Token: 0x04000F8C RID: 3980
		private int mapSizeZ;

		// Token: 0x04000F8D RID: 3981
		private float opacity = 0.33f;

		// Token: 0x04000F8E RID: 3982
		private int renderQueue = 3600;

		// Token: 0x04000F8F RID: 3983
		private Func<Color> colorGetter;

		// Token: 0x04000F90 RID: 3984
		private Func<int, Color> extraColorGetter;

		// Token: 0x04000F91 RID: 3985
		private Func<int, bool> cellBoolGetter;

		// Token: 0x04000F92 RID: 3986
		private static List<Vector3> verts = new List<Vector3>();

		// Token: 0x04000F93 RID: 3987
		private static List<int> tris = new List<int>();

		// Token: 0x04000F94 RID: 3988
		private static List<Color> colors = new List<Color>();

		// Token: 0x04000F95 RID: 3989
		private const float DefaultOpacity = 0.33f;

		// Token: 0x04000F96 RID: 3990
		private const int MaxCellsPerMesh = 16383;
	}
}
