using System;

namespace Verse
{
	// Token: 0x0200041B RID: 1051
	public class HediffCompProperties_Disappears : HediffCompProperties
	{
		// Token: 0x06001EDB RID: 7899 RVA: 0x000BCE95 File Offset: 0x000BB095
		public HediffCompProperties_Disappears()
		{
			this.compClass = typeof(HediffComp_Disappears);
		}

		// Token: 0x04001833 RID: 6195
		public IntRange disappearsAfterTicks;

		// Token: 0x04001834 RID: 6196
		public bool showRemainingTime;

		// Token: 0x04001835 RID: 6197
		public bool canUseDecimalsShortForm;

		// Token: 0x04001836 RID: 6198
		public MentalStateDef requiredMentalState;

		// Token: 0x04001837 RID: 6199
		[MustTranslate]
		public string messageOnDisappear;

		// Token: 0x04001838 RID: 6200
		[MustTranslate]
		public string letterTextOnDisappear;

		// Token: 0x04001839 RID: 6201
		[MustTranslate]
		public string letterLabelOnDisappear;

		// Token: 0x0400183A RID: 6202
		public bool sendLetterOnDisappearIfDead = true;

		// Token: 0x0400183B RID: 6203
		public bool leaveFreshWounds = true;
	}
}
