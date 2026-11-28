using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014FC RID: 5372
	public class RecordWorker_TimeGettingJoy : RecordWorker
	{
		// Token: 0x060083B8 RID: 33720 RVA: 0x002F59FC File Offset: 0x002F3BFC
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			Job curJob = pawn.CurJob;
			return curJob != null && curJob.def.joyKind != null;
		}
	}
}
