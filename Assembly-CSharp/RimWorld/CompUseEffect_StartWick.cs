using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A49 RID: 6729
	public class CompUseEffect_StartWick : CompUseEffect
	{
		// Token: 0x0600A895 RID: 43157 RVA: 0x003BDFFB File Offset: 0x003BC1FB
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			this.parent.GetComp<CompExplosive>().StartWick(null);
		}
	}
}
