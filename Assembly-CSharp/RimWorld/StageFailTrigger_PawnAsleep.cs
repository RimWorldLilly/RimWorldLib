using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001676 RID: 5750
	public class StageFailTrigger_PawnAsleep : StageFailTrigger
	{
		// Token: 0x0600903C RID: 36924 RVA: 0x0034F6A1 File Offset: 0x0034D8A1
		public override bool Failed(LordJob_Ritual ritual, TargetInfo spot, TargetInfo focus)
		{
			return ritual.PawnWithRole(this.pawnId).jobs.curDriver.asleep;
		}

		// Token: 0x0600903D RID: 36925 RVA: 0x0034F6BE File Offset: 0x0034D8BE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.pawnId, "pawnId", null, false);
		}

		// Token: 0x04005805 RID: 22533
		[NoTranslate]
		public string pawnId;
	}
}
