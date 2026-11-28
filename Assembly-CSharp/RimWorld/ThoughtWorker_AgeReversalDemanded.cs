using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DD4 RID: 3540
	public class ThoughtWorker_AgeReversalDemanded : ThoughtWorker_Precept
	{
		// Token: 0x0600607E RID: 24702 RVA: 0x0022F870 File Offset: 0x0022DA70
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (p.Faction != Faction.OfPlayer)
			{
				return ThoughtState.Inactive;
			}
			if (p.IsSlave)
			{
				return ThoughtState.Inactive;
			}
			if (p.ageTracker == null)
			{
				return ThoughtState.Inactive;
			}
			if (p.ageTracker.AgeBiologicalYears < 25)
			{
				return ThoughtState.Inactive;
			}
			long ageReversalDemandedDeadlineTicks = p.ageTracker.AgeReversalDemandedDeadlineTicks;
			if (ageReversalDemandedDeadlineTicks > 0L)
			{
				return ThoughtState.ActiveAtStage(3);
			}
			long num = -ageReversalDemandedDeadlineTicks / 60000L;
			int stageIndex;
			if (num <= 15L)
			{
				stageIndex = 0;
			}
			else if (num <= 30L)
			{
				stageIndex = 1;
			}
			else
			{
				stageIndex = 2;
			}
			return ThoughtState.ActiveAtStage(stageIndex);
		}

		// Token: 0x0600607F RID: 24703 RVA: 0x0022F900 File Offset: 0x0022DB00
		public static bool CanHaveThought(Pawn pawn)
		{
			return ModLister.CheckIdeology("Age Reversal") && ThoughtDefOf.AgeReversalDemanded.Worker.CurrentState(pawn).Active;
		}
	}
}
