using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x02002301 RID: 8961
	public abstract class WorldCameraConfig
	{
		// Token: 0x0600DD68 RID: 56680 RVA: 0x00526ED4 File Offset: 0x005250D4
		public virtual void ConfigFixedUpdate_60(ref Vector2 rotationVelocity)
		{
		}

		// Token: 0x0600DD69 RID: 56681 RVA: 0x00526ED6 File Offset: 0x005250D6
		public virtual void ConfigOnGUI()
		{
		}

		// Token: 0x04008E0B RID: 36363
		public float dollyRateKeys = 170f;

		// Token: 0x04008E0C RID: 36364
		public float dollyRateScreenEdge = 125f;

		// Token: 0x04008E0D RID: 36365
		public float camRotationDecayFactor = 0.9f;

		// Token: 0x04008E0E RID: 36366
		public float rotationSpeedScale = 0.3f;

		// Token: 0x04008E0F RID: 36367
		public float zoomSpeed = 2.6f;

		// Token: 0x04008E10 RID: 36368
		public float zoomPreserveFactor;

		// Token: 0x04008E11 RID: 36369
		public bool smoothZoom;
	}
}
