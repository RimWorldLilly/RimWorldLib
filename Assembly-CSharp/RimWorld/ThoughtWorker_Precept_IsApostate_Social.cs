using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DBC RID: 3516
	public class ThoughtWorker_Precept_IsApostate_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06006036 RID: 24630 RVA: 0x0022EADF File Offset: 0x0022CCDF
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return otherPawn.ideo != null && otherPawn.Ideo != p.Ideo && otherPawn.ideo.PreviousIdeos.Contains(p.Ideo);
		}
	}
}
