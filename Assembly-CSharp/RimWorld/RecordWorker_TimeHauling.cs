using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014FD RID: 5373
	public class RecordWorker_TimeHauling : RecordWorker
	{
		// Token: 0x060083BA RID: 33722 RVA: 0x002F5A2B File Offset: 0x002F3C2B
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return !pawn.Dead && pawn.carryTracker.CarriedThing != null;
		}
	}
}
