using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E29 RID: 3625
	public class ThoughtWorker_Sick : ThoughtWorker
	{
		// Token: 0x06006145 RID: 24901 RVA: 0x00232520 File Offset: 0x00230720
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.health.hediffSet.AnyHediffMakesSickThought;
		}
	}
}
