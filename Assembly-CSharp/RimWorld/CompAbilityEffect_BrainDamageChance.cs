using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013DD RID: 5085
	public class CompAbilityEffect_BrainDamageChance : CompAbilityEffect
	{
		// Token: 0x170015A3 RID: 5539
		// (get) Token: 0x06007E76 RID: 32374 RVA: 0x002D2F8E File Offset: 0x002D118E
		public new CompProperties_AbilityBrainDamageChance Props
		{
			get
			{
				return (CompProperties_AbilityBrainDamageChance)this.props;
			}
		}

		// Token: 0x06007E77 RID: 32375 RVA: 0x002D2F9C File Offset: 0x002D119C
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Thing as Pawn;
			if (pawn != null)
			{
				if (pawn.Dead)
				{
					return;
				}
				if (Rand.Value <= this.Props.brainDamageChance)
				{
					BodyPartRecord brain = pawn.health.hediffSet.GetBrain();
					if (brain == null)
					{
						return;
					}
					int num = Rand.RangeInclusive(1, 5);
					pawn.TakeDamage(new DamageInfo(DamageDefOf.Flame, (float)num, 0f, -1f, this.parent.pawn, brain, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
				}
			}
		}
	}
}
