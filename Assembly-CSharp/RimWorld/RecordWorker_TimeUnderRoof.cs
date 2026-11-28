using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001502 RID: 5378
	public class RecordWorker_TimeUnderRoof : RecordWorker
	{
		// Token: 0x060083C4 RID: 33732 RVA: 0x002F5AE4 File Offset: 0x002F3CE4
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Spawned && pawn.Position.Roofed(pawn.Map);
		}
	}
}
