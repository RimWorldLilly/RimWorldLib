using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001FC RID: 508
	public class TimeSlower
	{
		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000EA2 RID: 3746 RVA: 0x0004BDEA File Offset: 0x00049FEA
		public bool ForcedNormalSpeed
		{
			get
			{
				return !DebugViewSettings.neverForceNormalSpeed && Find.TickManager.TicksGame < this.forceNormalSpeedUntil;
			}
		}

		// Token: 0x06000EA3 RID: 3747 RVA: 0x0004BE07 File Offset: 0x0004A007
		public void SignalForceNormalSpeed()
		{
			this.forceNormalSpeedUntil = Mathf.Max(new int[]
			{
				Find.TickManager.TicksGame + 800
			});
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x0004BE2D File Offset: 0x0004A02D
		public void SignalForceNormalSpeedShort()
		{
			this.forceNormalSpeedUntil = Mathf.Max(this.forceNormalSpeedUntil, Find.TickManager.TicksGame + 240);
		}

		// Token: 0x04000E63 RID: 3683
		private int forceNormalSpeedUntil;

		// Token: 0x04000E64 RID: 3684
		private const int ForceTicksStandard = 800;

		// Token: 0x04000E65 RID: 3685
		private const int ForceTicksShort = 240;
	}
}
