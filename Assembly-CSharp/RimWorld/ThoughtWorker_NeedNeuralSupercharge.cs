using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DD3 RID: 3539
	public class ThoughtWorker_NeedNeuralSupercharge : ThoughtWorker_Precept
	{
		// Token: 0x0600607C RID: 24700 RVA: 0x0022F80C File Offset: 0x0022DA0C
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (p.health == null)
			{
				return false;
			}
			if (!ResearchProjectDefOf.MicroelectronicsBasics.IsFinished)
			{
				return false;
			}
			int lastReceivedNeuralSuperchargeTick = p.health.lastReceivedNeuralSuperchargeTick;
			return Find.TickManager.TicksGame - lastReceivedNeuralSuperchargeTick >= 30000 || lastReceivedNeuralSuperchargeTick == -1;
		}

		// Token: 0x040035F9 RID: 13817
		public const int TicksUntilNeed = 30000;
	}
}
