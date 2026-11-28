using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B98 RID: 2968
	public class JobGiver_Berserk_Warcall : JobGiver_Berserk
	{
		// Token: 0x060055F9 RID: 22009 RVA: 0x001FFB73 File Offset: 0x001FDD73
		protected override bool IsGoodTarget(Thing thing)
		{
			return base.IsGoodTarget(thing) || (thing.Spawned && !(thing is Pawn));
		}
	}
}
