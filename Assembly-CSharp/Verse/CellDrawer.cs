using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000241 RID: 577
	public class CellDrawer
	{
		// Token: 0x0600110E RID: 4366 RVA: 0x000614B7 File Offset: 0x0005F6B7
		public CellDrawer(Func<int, Color?> cellIndexToColor, Material material, int mapSizeX, int mapSizeZ)
		{
			this.colorSource = cellIndexToColor;
			this.material = material;
			this.mapSizeX = mapSizeX;
			this.mapSizeZ = mapSizeZ;
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x000614EE File Offset: 0x0005F6EE
		public void MarkForDraw()
		{
			this.markedForDraw = true;
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x000614F7 File Offset: 0x0005F6F7
		public void Update()
		{
			if (!this.markedForDraw)
			{
				return;
			}
			this.Draw();
			this.markedForDraw = false;
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x00061510 File Offset: 0x0005F710
		private void Draw()
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

		// Token: 0x06001112 RID: 4370 RVA: 0x00061563 File Offset: 0x0005F763
		public void SetDirty()
		{
			this.dirty = true;
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x0006156C File Offset: 0x0005F76C
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
					name = "CellDrawer"
				};
				this.meshes.Add(item);
			}
			Mesh mesh = this.meshes[num];
			CellRect cellRect = new CellRect(0, 0, this.mapSizeX, this.mapSizeZ);
			float y = AltitudeLayer.MapDataOverlay.AltitudeFor();
			for (int j = cellRect.minX; j <= cellRect.maxX; j++)
			{
				for (int k = cellRect.minZ; k <= cellRect.maxZ; k++)
				{
					int arg = CellIndicesUtility.CellToIndex(j, k, this.mapSizeX);
					Color? color = this.colorSource(arg);
					if (color != null)
					{
						CellDrawer.verts.Add(new Vector3((float)j, y, (float)k));
						CellDrawer.verts.Add(new Vector3((float)j, y, (float)(k + 1)));
						CellDrawer.verts.Add(new Vector3((float)(j + 1), y, (float)(k + 1)));
						CellDrawer.verts.Add(new Vector3((float)(j + 1), y, (float)k));
						Color value = color.Value;
						CellDrawer.colors.Add(value);
						CellDrawer.colors.Add(value);
						CellDrawer.colors.Add(value);
						CellDrawer.colors.Add(value);
						int count = CellDrawer.verts.Count;
						CellDrawer.tris.Add(count - 4);
						CellDrawer.tris.Add(count - 3);
						CellDrawer.tris.Add(count - 2);
						CellDrawer.tris.Add(count - 4);
						CellDrawer.tris.Add(count - 2);
						CellDrawer.tris.Add(count - 1);
						num2++;
						if (num2 >= 16383)
						{
							this.FinalizeWorkingDataIntoMesh(mesh);
							num++;
							if (this.meshes.Count < num + 1)
							{
								Mesh item2 = new Mesh
								{
									name = "CellDrawer"
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
			this.dirty = false;
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x000617D0 File Offset: 0x0005F9D0
		private void FinalizeWorkingDataIntoMesh(Mesh mesh)
		{
			if (CellDrawer.verts.Count > 0)
			{
				mesh.SetVertices(CellDrawer.verts);
				CellDrawer.verts.Clear();
				mesh.SetTriangles(CellDrawer.tris, 0);
				CellDrawer.tris.Clear();
				mesh.SetColors(CellDrawer.colors);
				CellDrawer.colors.Clear();
			}
		}

		// Token: 0x04000F97 RID: 3991
		private bool markedForDraw;

		// Token: 0x04000F98 RID: 3992
		private Material material;

		// Token: 0x04000F99 RID: 3993
		private bool dirty = true;

		// Token: 0x04000F9A RID: 3994
		private List<Mesh> meshes = new List<Mesh>();

		// Token: 0x04000F9B RID: 3995
		private int mapSizeX;

		// Token: 0x04000F9C RID: 3996
		private int mapSizeZ;

		// Token: 0x04000F9D RID: 3997
		private Func<int, Color?> colorSource;

		// Token: 0x04000F9E RID: 3998
		private static List<Vector3> verts = new List<Vector3>();

		// Token: 0x04000F9F RID: 3999
		private static List<int> tris = new List<int>();

		// Token: 0x04000FA0 RID: 4000
		private static List<Color> colors = new List<Color>();

		// Token: 0x04000FA1 RID: 4001
		private const int MaxCellsPerMesh = 16383;
	}
}
