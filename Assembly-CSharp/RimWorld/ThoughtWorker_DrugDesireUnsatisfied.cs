using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E35 RID: 3637
	public class ThoughtWorker_DrugDesireUnsatisfied : ThoughtWorker
	{
		// Token: 0x0600615F RID: 24927 RVA: 0x00232A20 File Offset: 0x00230C20
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Need_Chemical_Any drugsDesire = p.needs.drugsDesire;
			if (drugsDesire == null)
			{
				return false;
			}
			int moodBuffForCurrentLevel = (int)drugsDesire.MoodBuffForCurrentLevel;
			if (moodBuffForCurrentLevel < 3)
			{
				return ThoughtState.ActiveAtStage(3 - moodBuffForCurrentLevel - 1);
			}
			return false;
		}

		// Token: 0x04003603 RID: 13827
		private const int Neutral = 3;
	}
}
