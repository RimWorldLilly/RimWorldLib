using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001500 RID: 5376
	public class RecordWorker_TimeInMentalState : RecordWorker
	{
		// Token: 0x060083C0 RID: 33728 RVA: 0x002F5AC4 File Offset: 0x002F3CC4
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.InMentalState;
		}
	}
}
