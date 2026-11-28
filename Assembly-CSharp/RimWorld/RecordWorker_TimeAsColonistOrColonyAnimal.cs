using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014F6 RID: 5366
	public class RecordWorker_TimeAsColonistOrColonyAnimal : RecordWorker
	{
		// Token: 0x060083AC RID: 33708 RVA: 0x002F5939 File Offset: 0x002F3B39
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer && !pawn.HasExtraHomeFaction(null) && !pawn.IsSlave && !pawn.IsPrisoner;
		}
	}
}
