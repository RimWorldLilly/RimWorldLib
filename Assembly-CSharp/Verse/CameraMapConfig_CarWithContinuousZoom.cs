using System;

namespace Verse
{
	// Token: 0x020000F9 RID: 249
	public class CameraMapConfig_CarWithContinuousZoom : CameraMapConfig_Car
	{
		// Token: 0x060008E6 RID: 2278 RVA: 0x00032E5B File Offset: 0x0003105B
		public CameraMapConfig_CarWithContinuousZoom()
		{
			this.zoomSpeed = 0.043f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
