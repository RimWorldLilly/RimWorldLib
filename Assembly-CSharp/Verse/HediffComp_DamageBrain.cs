using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200041A RID: 1050
	public class HediffComp_DamageBrain : HediffComp
	{
		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06001ED8 RID: 7896 RVA: 0x000BCD6E File Offset: 0x000BAF6E
		public HediffCompProperties_DamageBrain Props
		{
			get
			{
				return (HediffCompProperties_DamageBrain)this.props;
			}
		}

		// Token: 0x06001ED9 RID: 7897 RVA: 0x000BCD7C File Offset: 0x000BAF7C
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			if (this.Props.mtbDaysPerStage[this.parent.CurStageIndex] <= 0f && !base.Pawn.IsHashIntervalTick(60, delta))
			{
				return;
			}
			if (Rand.MTBEventOccurs(this.Props.mtbDaysPerStage[this.parent.CurStageIndex], 60000f, 60f))
			{
				BodyPartRecord brain = base.Pawn.health.hediffSet.GetBrain();
				if (brain == null)
				{
					return;
				}
				int randomInRange = this.Props.damageAmount.RandomInRange;
				base.Pawn.TakeDamage(new DamageInfo(DamageDefOf.Burn, (float)randomInRange, 0f, -1f, null, brain, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
				Messages.Message("MessageReceivedBrainDamageFromHediff".Translate(base.Pawn.Named("PAWN"), randomInRange, this.parent.Label), base.Pawn, MessageTypeDefOf.NegativeEvent, true);
			}
		}
	}
}
