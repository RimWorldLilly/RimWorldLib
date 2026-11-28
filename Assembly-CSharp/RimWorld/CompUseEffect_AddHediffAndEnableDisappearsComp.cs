using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A30 RID: 6704
	public class CompUseEffect_AddHediffAndEnableDisappearsComp : CompUseEffect_AddHediff
	{
		// Token: 0x0600A845 RID: 43077 RVA: 0x003BCCB4 File Offset: 0x003BAEB4
		public override void DoEffect(Pawn user)
		{
			user.health.AddHediff(base.Props.hediffDef, null, null, null).TryGetComp<HediffComp_DisappearsDisableable>().disabled = false;
		}
	}
}
