using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001501 RID: 5377
	public class RecordWorker_TimeOnFire : RecordWorker
	{
		// Token: 0x060083C2 RID: 33730 RVA: 0x002F5AD4 File Offset: 0x002F3CD4
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.IsBurning();
		}
	}
}
