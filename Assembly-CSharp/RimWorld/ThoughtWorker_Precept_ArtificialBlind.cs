using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DC6 RID: 3526
	public class ThoughtWorker_Precept_ArtificialBlind : ThoughtWorker_Precept
	{
		// Token: 0x0600605D RID: 24669 RVA: 0x0022F282 File Offset: 0x0022D482
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return PawnUtility.IsArtificiallyBlind(p);
		}
	}
}
