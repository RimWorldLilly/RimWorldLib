using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001851 RID: 6225
	public class CompProperties_Countdown : CompProperties
	{
		// Token: 0x06009BDE RID: 39902 RVA: 0x00382A42 File Offset: 0x00380C42
		public CompProperties_Countdown()
		{
			this.compClass = typeof(CompCountdown);
		}

		// Token: 0x04005CAB RID: 23723
		[MustTranslate]
		public string label;
	}
}
