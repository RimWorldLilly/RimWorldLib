using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DDD RID: 3549
	public class ThoughtWorker_Precept_Bloodfeeder_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x0600609A RID: 24730 RVA: 0x0022FD5B File Offset: 0x0022DF5B
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			if (!ModsConfig.BiotechActive || !ModsConfig.IdeologyActive)
			{
				return ThoughtState.Inactive;
			}
			return otherPawn.IsBloodfeeder();
		}
	}
}
