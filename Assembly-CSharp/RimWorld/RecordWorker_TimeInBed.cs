using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014FE RID: 5374
	public class RecordWorker_TimeInBed : RecordWorker
	{
		// Token: 0x060083BC RID: 33724 RVA: 0x002F5A4D File Offset: 0x002F3C4D
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.InBed();
		}
	}
}
