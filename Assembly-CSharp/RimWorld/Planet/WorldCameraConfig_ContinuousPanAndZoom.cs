using System;

namespace RimWorld.Planet
{
	// Token: 0x02002304 RID: 8964
	public class WorldCameraConfig_ContinuousPanAndZoom : WorldCameraConfig_ContinuousPan
	{
		// Token: 0x0600DD6D RID: 56685 RVA: 0x00526F53 File Offset: 0x00525153
		public WorldCameraConfig_ContinuousPanAndZoom()
		{
			this.zoomSpeed = 0.03f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
