using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000250 RID: 592
	public static class CellRenderer
	{
		// Token: 0x06001153 RID: 4435 RVA: 0x00063524 File Offset: 0x00061724
		private static void InitFrame()
		{
			if (Time.frameCount != CellRenderer.lastCameraUpdateFrame)
			{
				CellRenderer.viewRect = Find.CameraDriver.CurrentViewRect;
				CellRenderer.lastCameraUpdateFrame = Time.frameCount;
			}
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x0006354B File Offset: 0x0006174B
		private static Material MatFromColorPct(float colorPct, bool transparent)
		{
			return DebugMatsSpectrum.Mat(GenMath.PositiveMod(Mathf.RoundToInt(colorPct * 100f), 100), transparent);
		}

		// Token: 0x06001155 RID: 4437 RVA: 0x00063566 File Offset: 0x00061766
		public static void RenderCell(IntVec3 c, float colorPct = 0.5f)
		{
			CellRenderer.RenderCell(c, CellRenderer.MatFromColorPct(colorPct, true));
		}

		// Token: 0x06001156 RID: 4438 RVA: 0x00063578 File Offset: 0x00061778
		public static void RenderCell(IntVec3 c, Material mat)
		{
			CellRenderer.InitFrame();
			if (!CellRenderer.viewRect.Contains(c))
			{
				return;
			}
			Vector3 position = c.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, mat, 0);
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x000635B4 File Offset: 0x000617B4
		public static void RenderSpot(IntVec3 c, float colorPct = 0.5f, float scale = 0.15f)
		{
			CellRenderer.RenderSpot(c.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays), colorPct, scale);
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x000635C6 File Offset: 0x000617C6
		public static void RenderSpot(Vector3 loc, float colorPct = 0.5f, float scale = 0.15f)
		{
			CellRenderer.RenderSpot(loc, CellRenderer.MatFromColorPct(colorPct, false), scale);
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x000635D8 File Offset: 0x000617D8
		public static void RenderSpot(Vector3 loc, Material mat, float scale = 0.15f)
		{
			CellRenderer.InitFrame();
			if (!CellRenderer.viewRect.Contains(loc.ToIntVec3()))
			{
				return;
			}
			loc.y = AltitudeLayer.MetaOverlays.AltitudeFor();
			Vector3 s = new Vector3(scale, 1f, scale);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(loc, Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.circle, matrix, mat, 0);
		}

		// Token: 0x04000FD9 RID: 4057
		private static int lastCameraUpdateFrame = -1;

		// Token: 0x04000FDA RID: 4058
		private static CellRect viewRect;
	}
}
