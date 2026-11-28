using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DDE RID: 3550
	public class ThoughtWorker_Precept_IsBloodfeeder : ThoughtWorker_Precept
	{
		// Token: 0x0600609C RID: 24732 RVA: 0x0022FD84 File Offset: 0x0022DF84
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.BiotechActive || !ModsConfig.IdeologyActive)
			{
				return ThoughtState.Inactive;
			}
			return p.IsBloodfeeder();
		}
	}
}
