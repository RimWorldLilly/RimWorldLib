using System;

namespace Verse
{
	// Token: 0x020000F6 RID: 246
	public class CameraMapConfig_ContinuousPan : CameraMapConfig
	{
		// Token: 0x060008E2 RID: 2274 RVA: 0x00032CFD File Offset: 0x00030EFD
		public CameraMapConfig_ContinuousPan()
		{
			this.dollyRateKeys = 10f;
			this.dollyRateScreenEdge = 5f;
			this.camSpeedDecayFactor = 1f;
			this.moveSpeedScale = 1f;
		}
	}
}
