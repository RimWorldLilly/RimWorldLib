using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200146F RID: 5231
	public class CompAbilityEffect_WordOfLove : CompAbilityEffect_WithDest
	{
		// Token: 0x17001603 RID: 5635
		// (get) Token: 0x06008065 RID: 32869 RVA: 0x002DC91E File Offset: 0x002DAB1E
		public override bool HideTargetPawnTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001604 RID: 5636
		// (get) Token: 0x06008066 RID: 32870 RVA: 0x002DC921 File Offset: 0x002DAB21
		public override TargetingParameters targetParams
		{
			get
			{
				return new TargetingParameters
				{
					canTargetSelf = true,
					canTargetBuildings = false,
					canTargetAnimals = false,
					canTargetMechs = false
				};
			}
		}

		// Token: 0x06008067 RID: 32871 RVA: 0x002DC944 File Offset: 0x002DAB44
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Pawn;
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicLove, false);
			if (firstHediffOfDef != null)
			{
				pawn.health.RemoveHediff(firstHediffOfDef);
			}
			Hediff_PsychicLove hediff_PsychicLove = (Hediff_PsychicLove)HediffMaker.MakeHediff(HediffDefOf.PsychicLove, pawn, pawn.health.hediffSet.GetBrain());
			hediff_PsychicLove.target = dest.Thing;
			HediffComp_Disappears hediffComp_Disappears = hediff_PsychicLove.TryGetComp<HediffComp_Disappears>();
			if (hediffComp_Disappears != null)
			{
				float num = this.parent.def.EffectDuration(this.parent.pawn);
				num *= pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1);
				hediffComp_Disappears.ticksToDisappear = num.SecondsToTicks();
			}
			pawn.health.AddHediff(hediff_PsychicLove, null, null, null);
		}

		// Token: 0x06008068 RID: 32872 RVA: 0x002DCA13 File Offset: 0x002DAC13
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.Valid(target, false);
		}

		// Token: 0x06008069 RID: 32873 RVA: 0x002DCA20 File Offset: 0x002DAC20
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			Pawn pawn = this.selectedTarget.Pawn;
			Pawn pawn2 = target.Pawn;
			if (pawn == pawn2)
			{
				return false;
			}
			if (pawn.ageTracker.AgeBiologicalYearsFloat < 16f)
			{
				Messages.Message("CannotUseAbility".Translate(this.parent.def.label) + ": " + "AbilityCantApplyTooYoung".Translate(pawn), pawn, MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (pawn2.ageTracker.AgeBiologicalYearsFloat < 16f)
			{
				Messages.Message("CannotUseAbility".Translate(this.parent.def.label) + ": " + "AbilityCantApplyTooYoung".Translate(pawn2), pawn2, MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (pawn != null && pawn2 != null && !pawn.story.traits.HasTrait(TraitDefOf.Bisexual))
			{
				Gender gender = pawn.gender;
				Gender gender2 = pawn.story.traits.HasTrait(TraitDefOf.Gay) ? gender : gender.Opposite();
				if (pawn2.gender != gender2)
				{
					Messages.Message("CannotUseAbility".Translate(this.parent.def.label) + ": " + "AbilityCantApplyWrongAttractionGender".Translate(pawn, pawn2), pawn, MessageTypeDefOf.RejectInput, false);
					return false;
				}
			}
			return base.ValidateTarget(target, true);
		}

		// Token: 0x0600806A RID: 32874 RVA: 0x002DCBD4 File Offset: 0x002DADD4
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				if (pawn.story.traits.HasTrait(TraitDefOf.Asexual))
				{
					if (throwMessages)
					{
						Messages.Message("CannotUseAbility".Translate(this.parent.def.label) + ": " + "AbilityCantApplyOnAsexual".Translate(), pawn, MessageTypeDefOf.RejectInput, false);
					}
					return false;
				}
				if (!AbilityUtility.ValidateNoMentalState(pawn, throwMessages, this.parent))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600806B RID: 32875 RVA: 0x002DCC68 File Offset: 0x002DAE68
		public override string ExtraLabelMouseAttachment(LocalTargetInfo target)
		{
			if (this.selectedTarget.IsValid)
			{
				return "PsychicLoveFor".Translate();
			}
			return "PsychicLoveInduceIn".Translate();
		}

		// Token: 0x0400501D RID: 20509
		private const int MinAge = 16;
	}
}
