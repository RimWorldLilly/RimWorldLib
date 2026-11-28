using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DCE RID: 3534
	public class ThoughtWorker_Precept_Darklight : ThoughtWorker_Precept
	{
		// Token: 0x0600606D RID: 24685 RVA: 0x0022F3DF File Offset: 0x0022D5DF
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!p.Awake() || PawnUtility.IsBiologicallyOrArtificiallyBlind(p))
			{
				return false;
			}
			return DarklightUtility.IsDarklightAt(p.Position, p.Map);
		}
	}
}
