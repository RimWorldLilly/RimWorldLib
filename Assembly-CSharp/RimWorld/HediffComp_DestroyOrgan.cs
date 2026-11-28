using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002087 RID: 8327
	public class HediffComp_DestroyOrgan : HediffComp
	{
		// Token: 0x170025B6 RID: 9654
		// (get) Token: 0x0600D208 RID: 53768 RVA: 0x004D28CB File Offset: 0x004D0ACB
		public HediffCompProperties_DestroyOrgan Props
		{
			get
			{
				return (HediffCompProperties_DestroyOrgan)this.props;
			}
		}

		// Token: 0x0600D209 RID: 53769 RVA: 0x004D28D8 File Offset: 0x004D0AD8
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			if (this.parent.Severity >= base.Def.maxSeverity)
			{
				this.DoDecayEffect();
			}
		}

		// Token: 0x0600D20A RID: 53770 RVA: 0x004D28F8 File Offset: 0x004D0AF8
		public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
			if (culprit != null && culprit.def == HediffDefOf.OrganDecay && culprit.Part == this.parent.Part)
			{
				this.DoDecayEffect();
			}
		}

		// Token: 0x0600D20B RID: 53771 RVA: 0x004D2924 File Offset: 0x004D0B24
		private void DoDecayEffect()
		{
			base.Pawn.health.RemoveHediff(this.parent);
			if (base.Pawn.Dead)
			{
				base.Pawn.health.AddHediff(HediffDefOf.Decayed, this.parent.Part, null, null);
			}
			else
			{
				DamageInfo dinfo = new DamageInfo(this.Props.damageType ?? DamageDefOf.Rotting, 99999f, 999f, -1f, null, this.parent.Part, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
				dinfo.SetAllowDamagePropagation(false);
				base.Pawn.TakeDamage(dinfo);
			}
			if (base.Pawn.Faction == Faction.OfPlayer && !string.IsNullOrEmpty(this.Props.messageText))
			{
				Messages.Message(this.Props.messageText.Formatted(base.Pawn.Named("PAWN"), this.parent.Part.Label), base.Pawn, MessageTypeDefOf.NegativeEvent, true);
			}
		}
	}
}
