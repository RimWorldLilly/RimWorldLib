using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A3A RID: 6714
	public class CompUseEffect_GainAbility : CompUseEffect
	{
		// Token: 0x17001D8A RID: 7562
		// (get) Token: 0x0600A869 RID: 43113 RVA: 0x003BD57F File Offset: 0x003BB77F
		public CompProperties_UseEffect_GainAbility Props
		{
			get
			{
				return (CompProperties_UseEffect_GainAbility)this.props;
			}
		}

		// Token: 0x0600A86A RID: 43114 RVA: 0x003BD58C File Offset: 0x003BB78C
		public override void DoEffect(Pawn user)
		{
			base.DoEffect(user);
			user.abilities.GainAbility(this.Props.ability);
			if (PawnUtility.ShouldSendNotificationAbout(user))
			{
				Messages.Message("AbilityNeurotrainerUsed".Translate(user.Named("USER"), this.Props.ability.LabelCap), user, MessageTypeDefOf.PositiveEvent, true);
			}
		}

		// Token: 0x0600A86B RID: 43115 RVA: 0x003BD600 File Offset: 0x003BB800
		public override AcceptanceReport CanBeUsedBy(Pawn p)
		{
			if (!p.health.hediffSet.HasHediff(HediffDefOf.PsychicAmplifier, false))
			{
				return "PsycastNeurotrainerNoPsylink".TranslateWithBackup("PsycastNeurotrainerNoPsychicAmplifier");
			}
			if (p.abilities != null && p.abilities.abilities.Any((Ability a) => a.def == this.Props.ability))
			{
				return "PsycastNeurotrainerAbilityAlreadyLearned".Translate(p.Named("USER"), this.Props.ability.LabelCap);
			}
			return base.CanBeUsedBy(p);
		}

		// Token: 0x0600A86C RID: 43116 RVA: 0x003BD69C File Offset: 0x003BB89C
		public override TaggedString ConfirmMessage(Pawn p)
		{
			if (!this.Props.ability.IsPsycast)
			{
				return null;
			}
			Hediff firstHediffOfDef = p.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicAmplifier, false);
			if (firstHediffOfDef == null)
			{
				return null;
			}
			if (this.Props.ability.level > ((Hediff_Level)firstHediffOfDef).level)
			{
				return "PsylinkTooLowForGainAbility".Translate(p.Named("PAWN"), this.Props.ability.label.Named("ABILITY"));
			}
			return null;
		}

		// Token: 0x0600A86D RID: 43117 RVA: 0x003BD738 File Offset: 0x003BB938
		public override bool AllowStackWith(Thing other)
		{
			if (!base.AllowStackWith(other))
			{
				return false;
			}
			CompUseEffect_GainAbility compUseEffect_GainAbility = other.TryGetComp<CompUseEffect_GainAbility>();
			return compUseEffect_GainAbility != null && compUseEffect_GainAbility.Props.ability == this.Props.ability;
		}
	}
}
