using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001719 RID: 5913
	public class RitualSpectatorFilter_WillWitnessExecution : RitualSpectatorFilter
	{
		// Token: 0x060093A8 RID: 37800 RVA: 0x0035E20E File Offset: 0x0035C40E
		public override bool Allowed(Pawn p)
		{
			return p.IsSlave || p.Ideo == null || p.Ideo.MemberWillingToDo(new HistoryEvent(HistoryEventDefOf.ExecutedPrisoner, p.Named(HistoryEventArgsNames.Doer)));
		}
	}
}
