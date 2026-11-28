using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014FF RID: 5375
	public class RecordWorker_TimeInBedForMedicalReasons : RecordWorker
	{
		// Token: 0x060083BE RID: 33726 RVA: 0x002F5A60 File Offset: 0x002F3C60
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.InBed() && (HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn) || (HealthAIUtility.ShouldSeekMedicalRest(pawn) && (pawn.needs.rest == null || pawn.needs.rest.CurLevel >= 1f || pawn.CurJob.restUntilHealed)));
		}
	}
}
