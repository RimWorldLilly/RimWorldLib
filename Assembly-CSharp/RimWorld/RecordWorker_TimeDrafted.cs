using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014FB RID: 5371
	public class RecordWorker_TimeDrafted : RecordWorker
	{
		// Token: 0x060083B6 RID: 33718 RVA: 0x002F59EC File Offset: 0x002F3BEC
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Drafted;
		}
	}
}
