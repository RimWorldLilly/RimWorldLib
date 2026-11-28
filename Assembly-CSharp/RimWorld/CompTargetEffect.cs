using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A0A RID: 6666
	public abstract class CompTargetEffect : ThingComp
	{
		// Token: 0x0600A7D0 RID: 42960
		public abstract void DoEffectOn(Pawn user, Thing target);

		// Token: 0x0600A7D1 RID: 42961 RVA: 0x003BB96D File Offset: 0x003B9B6D
		public virtual bool CanApplyOn(Thing target)
		{
			return true;
		}
	}
}
