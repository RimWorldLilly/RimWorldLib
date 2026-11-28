using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020007D7 RID: 2007
	public class SoundParamSource_SourceAge : SoundParamSource
	{
		// Token: 0x17000C46 RID: 3142
		// (get) Token: 0x06003FE2 RID: 16354 RVA: 0x001980C4 File Offset: 0x001962C4
		public override string Label
		{
			get
			{
				return "Sustainer age";
			}
		}

		// Token: 0x06003FE3 RID: 16355 RVA: 0x001980CB File Offset: 0x001962CB
		public override float ValueFor(Sample samp)
		{
			if (this.timeType == TimeType.RealtimeSeconds)
			{
				return Time.realtimeSinceStartup - samp.ParentStartRealTime;
			}
			if (this.timeType == TimeType.Ticks && Find.TickManager != null)
			{
				return (float)Find.TickManager.TicksGame - samp.ParentStartTick;
			}
			return 0f;
		}

		// Token: 0x04002A59 RID: 10841
		public TimeType timeType;
	}
}
