using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014F9 RID: 5369
	public class RecordWorker_TimeAsPrisoner : RecordWorker
	{
		// Token: 0x060083B2 RID: 33714 RVA: 0x002F59CC File Offset: 0x002F3BCC
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.IsPrisoner;
		}
	}
}
