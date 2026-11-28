using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A0D RID: 6669
	public class CompTargetEffect_BrainDamageChance : CompTargetEffect
	{
		// Token: 0x17001D69 RID: 7529
		// (get) Token: 0x0600A7D6 RID: 42966 RVA: 0x003BB9FF File Offset: 0x003B9BFF
		protected CompProperties_TargetEffect_BrainDamageChance PropsBrainDamageChance
		{
			get
			{
				return (CompProperties_TargetEffect_BrainDamageChance)this.props;
			}
		}

		// Token: 0x0600A7D7 RID: 42967 RVA: 0x003BBA0C File Offset: 0x003B9C0C
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = (Pawn)target;
			if (pawn.Dead)
			{
				return;
			}
			if (Rand.Value <= this.PropsBrainDamageChance.brainDamageChance)
			{
				BodyPartRecord brain = pawn.health.hediffSet.GetBrain();
				if (brain == null)
				{
					return;
				}
				int num = Rand.RangeInclusive(1, 5);
				pawn.TakeDamage(new DamageInfo(DamageDefOf.Flame, (float)num, 0f, -1f, user, brain, this.parent.def, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			}
		}
	}
}
