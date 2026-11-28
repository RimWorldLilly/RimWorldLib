using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014F7 RID: 5367
	public class RecordWorker_TimeAsQuestLodger : RecordWorker
	{
		// Token: 0x060083AE RID: 33710 RVA: 0x002F596C File Offset: 0x002F3B6C
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer && pawn.HasExtraHomeFaction(null);
		}
	}
}
