using System;

namespace Verse
{
	// Token: 0x020000F7 RID: 247
	public class CameraMapConfig_ContinuousPanAndZoom : CameraMapConfig_ContinuousPan
	{
		// Token: 0x060008E3 RID: 2275 RVA: 0x00032D31 File Offset: 0x00030F31
		public CameraMapConfig_ContinuousPanAndZoom()
		{
			this.zoomSpeed = 0.043f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
