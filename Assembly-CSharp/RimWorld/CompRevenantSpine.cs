using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001956 RID: 6486
	public class CompRevenantSpine : CompInteractable
	{
		// Token: 0x0600A2E3 RID: 41699 RVA: 0x003A32F4 File Offset: 0x003A14F4
		protected override void OnInteracted(Pawn caster)
		{
			CompUsable comp = this.parent.GetComp<CompUsable>();
			comp.TryStartUseJob(caster, comp.GetExtraTarget(caster), false);
		}

		// Token: 0x0600A2E4 RID: 41700 RVA: 0x003A331C File Offset: 0x003A151C
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			return new CompRevenantSpine.<CompFloatMenuOptions>d__1(-2);
		}
	}
}
