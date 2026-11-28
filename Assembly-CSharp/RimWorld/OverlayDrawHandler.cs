using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200128A RID: 4746
	public static class OverlayDrawHandler
	{
		// Token: 0x060075A5 RID: 30117 RVA: 0x00294FBB File Offset: 0x002931BB
		public static void DrawPowerGridOverlayThisFrame()
		{
			OverlayDrawHandler.lastPowerGridDrawFrame = Time.frameCount;
		}

		// Token: 0x170013E3 RID: 5091
		// (get) Token: 0x060075A6 RID: 30118 RVA: 0x00294FC7 File Offset: 0x002931C7
		public static bool ShouldDrawPowerGrid
		{
			get
			{
				return OverlayDrawHandler.lastPowerGridDrawFrame + 1 >= Time.frameCount;
			}
		}

		// Token: 0x060075A7 RID: 30119 RVA: 0x00294FDA File Offset: 0x002931DA
		public static void DrawZonesThisFrame()
		{
			OverlayDrawHandler.lastZoneDrawFrame = Time.frameCount;
		}

		// Token: 0x170013E4 RID: 5092
		// (get) Token: 0x060075A8 RID: 30120 RVA: 0x00294FE6 File Offset: 0x002931E6
		public static bool ShouldDrawZones
		{
			get
			{
				return Find.PlaySettings.showZones || Time.frameCount <= OverlayDrawHandler.lastZoneDrawFrame + 1;
			}
		}

		// Token: 0x040049B7 RID: 18871
		private static int lastPowerGridDrawFrame;

		// Token: 0x040049B8 RID: 18872
		private static int lastZoneDrawFrame;
	}
}
