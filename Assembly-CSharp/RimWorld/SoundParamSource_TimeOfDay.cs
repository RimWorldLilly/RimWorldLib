using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000F15 RID: 3861
	public class SoundParamSource_TimeOfDay : SoundParamSource
	{
		// Token: 0x1700110F RID: 4367
		// (get) Token: 0x06006429 RID: 25641 RVA: 0x0023D65C File Offset: 0x0023B85C
		public override string Label
		{
			get
			{
				return "Time of day (hour)";
			}
		}

		// Token: 0x0600642A RID: 25642 RVA: 0x0023D663 File Offset: 0x0023B863
		public override float ValueFor(Sample samp)
		{
			if (Find.CurrentMap == null)
			{
				return 0f;
			}
			return GenLocalDate.HourFloat(Find.CurrentMap);
		}
	}
}
