using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001966 RID: 6502
	public class CompProperties_SendSignalOnCountdown : CompProperties
	{
		// Token: 0x0600A330 RID: 41776 RVA: 0x003A4663 File Offset: 0x003A2863
		public CompProperties_SendSignalOnCountdown()
		{
			this.compClass = typeof(CompSendSignalOnCountdown);
		}

		// Token: 0x04006063 RID: 24675
		public SimpleCurve countdownCurveTicks;

		// Token: 0x04006064 RID: 24676
		[NoTranslate]
		public string signalTag;
	}
}
