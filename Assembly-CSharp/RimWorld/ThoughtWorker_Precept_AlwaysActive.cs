using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DE5 RID: 3557
	public class ThoughtWorker_Precept_AlwaysActive : ThoughtWorker_Precept
	{
		// Token: 0x060060AC RID: 24748 RVA: 0x00230195 File Offset: 0x0022E395
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtState.ActiveDefault;
		}
	}
}
