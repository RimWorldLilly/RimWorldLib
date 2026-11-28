using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E20 RID: 3616
	public class ThoughtWorker_Dark : ThoughtWorker
	{
		// Token: 0x0600612E RID: 24878 RVA: 0x00232054 File Offset: 0x00230254
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Awake() && p.needs.mood.recentMemory.TicksSinceLastLight > 240 && !PawnUtility.IsBiologicallyOrArtificiallyBlind(p) && (p.Ideo == null || !p.Ideo.IdeoPrefersDarkness());
		}
	}
}
