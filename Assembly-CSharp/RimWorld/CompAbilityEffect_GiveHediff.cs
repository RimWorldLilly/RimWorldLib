using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200140D RID: 5133
	public class CompAbilityEffect_GiveHediff : CompAbilityEffect_WithDuration
	{
		// Token: 0x170015BE RID: 5566
		// (get) Token: 0x06007F14 RID: 32532 RVA: 0x002D6B0B File Offset: 0x002D4D0B
		public new CompProperties_AbilityGiveHediff Props
		{
			get
			{
				return (CompProperties_AbilityGiveHediff)this.props;
			}
		}

		// Token: 0x06007F15 RID: 32533 RVA: 0x002D6B18 File Offset: 0x002D4D18
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			if (this.Props.ignoreSelf && target.Pawn == this.parent.pawn)
			{
				return;
			}
			if (!this.Props.onlyApplyToSelf && this.Props.applyToTarget)
			{
				this.ApplyInner(target.Pawn, this.parent.pawn);
			}
			if (this.Props.applyToSelf || this.Props.onlyApplyToSelf)
			{
				this.ApplyInner(this.parent.pawn, target.Pawn);
			}
		}

		// Token: 0x06007F16 RID: 32534 RVA: 0x002D6BB4 File Offset: 0x002D4DB4
		protected void ApplyInner(Pawn target, Pawn other)
		{
			if (target != null)
			{
				if (this.TryResist(target))
				{
					MoteMaker.ThrowText(target.DrawPos, target.Map, "Resisted".Translate(), -1f);
					return;
				}
				if (this.Props.replaceExisting)
				{
					Hediff firstHediffOfDef = target.health.hediffSet.GetFirstHediffOfDef(this.Props.hediffDef, false);
					if (firstHediffOfDef != null)
					{
						target.health.RemoveHediff(firstHediffOfDef);
					}
				}
				Hediff hediff = HediffMaker.MakeHediff(this.Props.hediffDef, target, this.Props.onlyBrain ? target.health.hediffSet.GetBrain() : null);
				HediffComp_Disappears hediffComp_Disappears = hediff.TryGetComp<HediffComp_Disappears>();
				if (hediffComp_Disappears != null)
				{
					hediffComp_Disappears.ticksToDisappear = base.GetDurationSeconds(target).SecondsToTicks();
				}
				if (this.Props.severity >= 0f)
				{
					hediff.Severity = this.Props.severity;
				}
				HediffComp_Link hediffComp_Link = hediff.TryGetComp<HediffComp_Link>();
				if (hediffComp_Link != null)
				{
					hediffComp_Link.other = other;
					hediffComp_Link.drawConnection = (target == this.parent.pawn);
				}
				target.health.AddHediff(hediff, null, null, null);
			}
		}

		// Token: 0x06007F17 RID: 32535 RVA: 0x002D6CDD File Offset: 0x002D4EDD
		protected virtual bool TryResist(Pawn pawn)
		{
			return false;
		}

		// Token: 0x06007F18 RID: 32536 RVA: 0x002D6CE0 File Offset: 0x002D4EE0
		public override bool AICanTargetNow(LocalTargetInfo target)
		{
			return this.parent.pawn.Faction != Faction.OfPlayer && target.Pawn != null;
		}
	}
}
