using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014FA RID: 5370
	public class RecordWorker_TimeDowned : RecordWorker
	{
		// Token: 0x060083B4 RID: 33716 RVA: 0x002F59DC File Offset: 0x002F3BDC
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Downed;
		}
	}
}
