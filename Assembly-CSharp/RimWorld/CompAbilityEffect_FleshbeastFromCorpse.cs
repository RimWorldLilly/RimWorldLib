using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001409 RID: 5129
	public class CompAbilityEffect_FleshbeastFromCorpse : CompAbilityEffect
	{
		// Token: 0x170015BC RID: 5564
		// (get) Token: 0x06007F0B RID: 32523 RVA: 0x002D697B File Offset: 0x002D4B7B
		public new CompProperties_FleshbeastFromCorpse Props
		{
			get
			{
				return (CompProperties_FleshbeastFromCorpse)this.props;
			}
		}

		// Token: 0x06007F0C RID: 32524 RVA: 0x002D6988 File Offset: 0x002D4B88
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Corpse corpse = target.Thing as Corpse;
			if (corpse == null || corpse.InnerPawn == null)
			{
				return;
			}
			FleshbeastUtility.SpawnFleshbeastFromPawn(corpse.InnerPawn, false, true, Array.Empty<PawnKindDef>());
		}

		// Token: 0x06007F0D RID: 32525 RVA: 0x002D69C9 File Offset: 0x002D4BC9
		public override bool AICanTargetNow(LocalTargetInfo target)
		{
			return false;
		}
	}
}
