using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014F8 RID: 5368
	public class RecordWorker_TimeAsChildInColony : RecordWorker
	{
		// Token: 0x060083B0 RID: 33712 RVA: 0x002F598C File Offset: 0x002F3B8C
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return ModsConfig.BiotechActive && pawn.Spawned && pawn.RaceProps.Humanlike && pawn.Map.IsPlayerHome && pawn.DevelopmentalStage.Juvenile();
		}
	}
}
