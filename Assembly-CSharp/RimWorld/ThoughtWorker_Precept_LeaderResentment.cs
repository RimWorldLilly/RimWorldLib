using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DB3 RID: 3507
	public class ThoughtWorker_Precept_LeaderResentment : ThoughtWorker_Precept
	{
		// Token: 0x0600601D RID: 24605 RVA: 0x0022E60C File Offset: 0x0022C80C
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			Faction faction = p.Faction;
			Pawn pawn = (faction != null) ? faction.leader : null;
			return pawn != null && p.Ideo != pawn.Ideo;
		}
	}
}
