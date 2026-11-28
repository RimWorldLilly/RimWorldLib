using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E34 RID: 3636
	public class ThoughtWorker_DrugDesireSatisfied : ThoughtWorker
	{
		// Token: 0x0600615D RID: 24925 RVA: 0x002329D8 File Offset: 0x00230BD8
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Need_Chemical_Any drugsDesire = p.needs.drugsDesire;
			if (drugsDesire == null)
			{
				return false;
			}
			int moodBuffForCurrentLevel = (int)drugsDesire.MoodBuffForCurrentLevel;
			if (moodBuffForCurrentLevel > 3)
			{
				return ThoughtState.ActiveAtStage(moodBuffForCurrentLevel - 3 - 1);
			}
			return false;
		}

		// Token: 0x04003602 RID: 13826
		private const int Neutral = 3;
	}
}
