using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E37 RID: 3639
	public class ThoughtWorker_IsUndergroundForUndergrounder : ThoughtWorker
	{
		// Token: 0x06006164 RID: 24932 RVA: 0x00232AE8 File Offset: 0x00230CE8
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			bool flag;
			return ThoughtWorker_IsIndoorsForUndergrounder.IsAwakeAndIndoors(p, out flag) && flag;
		}
	}
}
