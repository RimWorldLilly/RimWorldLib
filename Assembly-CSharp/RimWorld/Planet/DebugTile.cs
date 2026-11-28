using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200230F RID: 8975
	public class DebugTile
	{
		// Token: 0x170026BF RID: 9919
		// (get) Token: 0x0600DDA4 RID: 56740 RVA: 0x00528680 File Offset: 0x00526880
		private Vector2 ScreenPos
		{
			get
			{
				return GenWorldUI.WorldToUIPosition(Find.WorldGrid.GetTileCenter(this.tile));
			}
		}

		// Token: 0x170026C0 RID: 9920
		// (get) Token: 0x0600DDA5 RID: 56741 RVA: 0x00528698 File Offset: 0x00526898
		private bool VisibleForCamera
		{
			get
			{
				return new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight).Contains(this.ScreenPos);
			}
		}

		// Token: 0x170026C1 RID: 9921
		// (get) Token: 0x0600DDA6 RID: 56742 RVA: 0x005286D0 File Offset: 0x005268D0
		public float DistanceToCamera
		{
			get
			{
				Vector3 tileCenter = Find.WorldGrid.GetTileCenter(this.tile);
				return Vector3.Distance(Find.WorldCamera.transform.position, tileCenter);
			}
		}

		// Token: 0x0600DDA7 RID: 56743 RVA: 0x00528704 File Offset: 0x00526904
		public void Draw()
		{
			if (!this.VisibleForCamera)
			{
				return;
			}
			if (this.mesh == null)
			{
				Find.WorldGrid.GetTileVertices(this.tile, DebugTile.tmpVerts);
				for (int i = 0; i < DebugTile.tmpVerts.Count; i++)
				{
					Vector3 a = DebugTile.tmpVerts[i];
					DebugTile.tmpVerts[i] = a + a.normalized * 0.02f;
				}
				this.mesh = new Mesh();
				this.mesh.name = "DebugTile";
				this.mesh.SetVertices(DebugTile.tmpVerts);
				DebugTile.tmpIndices.Clear();
				for (int j = 0; j < DebugTile.tmpVerts.Count - 2; j++)
				{
					DebugTile.tmpIndices.Add(j + 2);
					DebugTile.tmpIndices.Add(j + 1);
					DebugTile.tmpIndices.Add(0);
				}
				this.mesh.SetTriangles(DebugTile.tmpIndices, 0);
			}
			Material material;
			if (this.customMat != null)
			{
				material = this.customMat;
			}
			else
			{
				material = WorldDebugMatsSpectrum.Mat(Mathf.RoundToInt(this.colorPct * 100f) % 100);
			}
			Graphics.DrawMesh(this.mesh, Vector3.zero, Quaternion.identity, material, WorldCameraManager.WorldLayer);
		}

		// Token: 0x0600DDA8 RID: 56744 RVA: 0x00528854 File Offset: 0x00526A54
		public void OnGUI()
		{
			if (!this.VisibleForCamera)
			{
				return;
			}
			Vector2 screenPos = this.ScreenPos;
			Rect rect = new Rect(screenPos.x - 20f, screenPos.y - 20f, 40f, 40f);
			if (this.displayString != null)
			{
				Widgets.Label(rect, this.displayString);
			}
		}

		// Token: 0x04008E57 RID: 36439
		public PlanetTile tile;

		// Token: 0x04008E58 RID: 36440
		public string displayString;

		// Token: 0x04008E59 RID: 36441
		public float colorPct;

		// Token: 0x04008E5A RID: 36442
		public int ticksLeft;

		// Token: 0x04008E5B RID: 36443
		public Material customMat;

		// Token: 0x04008E5C RID: 36444
		private Mesh mesh;

		// Token: 0x04008E5D RID: 36445
		private static List<Vector3> tmpVerts = new List<Vector3>();

		// Token: 0x04008E5E RID: 36446
		private static List<int> tmpIndices = new List<int>();
	}
}
