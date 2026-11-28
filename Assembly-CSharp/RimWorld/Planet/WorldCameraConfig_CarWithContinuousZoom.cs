using System;

namespace RimWorld.Planet
{
	// Token: 0x02002306 RID: 8966
	public class WorldCameraConfig_CarWithContinuousZoom : WorldCameraConfig_Car
	{
		// Token: 0x0600DD70 RID: 56688 RVA: 0x005270AA File Offset: 0x005252AA
		public WorldCameraConfig_CarWithContinuousZoom()
		{
			this.zoomSpeed = 0.03f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
