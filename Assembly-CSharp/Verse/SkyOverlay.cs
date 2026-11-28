using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000322 RID: 802
	public abstract class SkyOverlay
	{
		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x0600189C RID: 6300 RVA: 0x000942C9 File Offset: 0x000924C9
		// (set) Token: 0x0600189D RID: 6301 RVA: 0x000942D1 File Offset: 0x000924D1
		public Color? ForcedOverlayColor { get; protected set; }

		// Token: 0x0600189E RID: 6302 RVA: 0x000942DA File Offset: 0x000924DA
		public static void DrawWorldOverlay(Map map, Material mat, int layer = 0)
		{
			SkyOverlay.DrawWorldOverlay(map, mat, AltitudeLayer.Weather.AltitudeFor(), layer);
		}

		// Token: 0x0600189F RID: 6303 RVA: 0x000942EC File Offset: 0x000924EC
		public static void DrawWorldOverlay(Map map, Material mat, float altitude, int layer = 0)
		{
			Vector3 position = map.Center.ToVector3Shifted().WithY(altitude);
			Graphics.DrawMesh(MeshPool.wholeMapPlane, position, Quaternion.identity, mat, layer);
		}

		// Token: 0x060018A0 RID: 6304 RVA: 0x00094320 File Offset: 0x00092520
		public static void DrawScreenOverlay(Material mat, int layer = 0, Camera camera = null)
		{
			SkyOverlay.DrawScreenOverlay(mat, AltitudeLayer.Weather.AltitudeFor() + 0.03658537f, layer, camera);
		}

		// Token: 0x060018A1 RID: 6305 RVA: 0x00094337 File Offset: 0x00092537
		public virtual void Reset()
		{
		}

		// Token: 0x060018A2 RID: 6306 RVA: 0x0009433C File Offset: 0x0009253C
		public static void DrawScreenOverlay(Material mat, float altitude, int layer = 0, Camera camera = null)
		{
			if (!camera)
			{
				camera = Find.Camera;
			}
			float num = camera.orthographicSize * 2f;
			Vector3 s = new Vector3(num * camera.aspect, 1f, num);
			Vector3 position = camera.transform.position;
			position.y = altitude;
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(position, Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, mat, layer);
		}

		// Token: 0x060018A3 RID: 6307
		public abstract void TickOverlay(Map map, float lerpFactor);

		// Token: 0x060018A4 RID: 6308
		public abstract void DrawOverlay(Map map);

		// Token: 0x060018A5 RID: 6309
		public abstract void SetOverlayColor(Color color);

		// Token: 0x060018A6 RID: 6310 RVA: 0x000943B1 File Offset: 0x000925B1
		public override string ToString()
		{
			return "NoOverlayOverlay";
		}
	}
}
