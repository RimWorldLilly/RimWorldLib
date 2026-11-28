using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E2F RID: 3631
	public class ThoughtWorker_PsychologicallyNude : ThoughtWorker
	{
		// Token: 0x06006152 RID: 24914 RVA: 0x00232828 File Offset: 0x00230A28
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (ModsConfig.IdeologyActive)
			{
				return false;
			}
			return p.apparel.PsychologicallyNude;
		}
	}
}
