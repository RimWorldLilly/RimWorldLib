using System;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x0200092E RID: 2350
	public class PsychicRitualToil_Psychophagy : PsychicRitualToil
	{
		// Token: 0x06004800 RID: 18432 RVA: 0x001C3E9C File Offset: 0x001C209C
		protected PsychicRitualToil_Psychophagy()
		{
		}

		// Token: 0x06004801 RID: 18433 RVA: 0x001C3EA4 File Offset: 0x001C20A4
		public PsychicRitualToil_Psychophagy(PsychicRitualRoleDef invokerRole, PsychicRitualRoleDef targetRole, FloatRange brainDamageRange)
		{
			this.invokerRole = invokerRole;
			this.targetRole = targetRole;
			this.brainDamageRange = brainDamageRange;
		}

		// Token: 0x06004802 RID: 18434 RVA: 0x001C3EC4 File Offset: 0x001C20C4
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			PsychicRitualDef_Psychophagy psychicRitualDef_Psychophagy = (PsychicRitualDef_Psychophagy)psychicRitual.def;
			Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
			Pawn pawn2 = psychicRitual.assignments.FirstAssignedPawn(this.targetRole);
			this.effectDurationTicks = Mathf.RoundToInt(psychicRitualDef_Psychophagy.effectDurationDaysFromQualityCurve.Evaluate(psychicRitual.PowerPercent) * 60000f);
			if (pawn != null && pawn2 != null)
			{
				this.ApplyOutcome(psychicRitual, pawn, pawn2);
			}
		}

		// Token: 0x06004803 RID: 18435 RVA: 0x001C3F34 File Offset: 0x001C2134
		private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker, Pawn target)
		{
			Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.DarkPsychicShock, target, null);
			target.health.AddHediff(hediff, null, null, null);
			Hediff hediff2 = HediffMaker.MakeHediff(HediffDefOf.PsychicallyDead, target, null);
			target.health.AddHediff(hediff2, null, null, null);
			Pawn_NeedsTracker needs = target.needs;
			if (needs != null)
			{
				Need_Mood mood = needs.mood;
				if (mood != null)
				{
					ThoughtHandler thoughts = mood.thoughts;
					if (thoughts != null)
					{
						MemoryThoughtHandler memories = thoughts.memories;
						if (memories != null)
						{
							memories.TryGainMemory(ThoughtDefOf.PsychicRitualVictim, null, null);
						}
					}
				}
			}
			foreach (Pawn otherPawn in psychicRitual.assignments.AllAssignedPawns.Except(target))
			{
				Pawn_NeedsTracker needs2 = target.needs;
				if (needs2 != null)
				{
					Need_Mood mood2 = needs2.mood;
					if (mood2 != null)
					{
						ThoughtHandler thoughts2 = mood2.thoughts;
						if (thoughts2 != null)
						{
							MemoryThoughtHandler memories2 = thoughts2.memories;
							if (memories2 != null)
							{
								memories2.TryGainMemory(ThoughtDefOf.UsedMeForPsychicRitual, otherPawn, null);
							}
						}
					}
				}
			}
			BodyPartRecord brain = target.health.hediffSet.GetBrain();
			if (brain != null)
			{
				target.TakeDamage(new DamageInfo(DamageDefOf.Psychic, this.brainDamageRange.RandomInRange, 0f, -1f, null, brain, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			}
			Hediff hediff3 = invoker.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Psychophage, false);
			if (hediff3 == null)
			{
				hediff3 = HediffMaker.MakeHediff(HediffDefOf.Psychophage, invoker, null);
				HediffComp_Disappears hediffComp_Disappears = hediff3.TryGetComp<HediffComp_Disappears>();
				if (hediffComp_Disappears != null)
				{
					hediffComp_Disappears.ticksToDisappear = this.effectDurationTicks;
				}
				invoker.health.AddHediff(hediff3, null, null, null);
			}
			else
			{
				HediffComp_Disappears hediffComp_Disappears2 = hediff3.TryGetComp<HediffComp_Disappears>();
				if (hediffComp_Disappears2 != null)
				{
					hediffComp_Disappears2.ticksToDisappear += this.effectDurationTicks;
				}
			}
			if (target.Dead)
			{
				PsychicRitualUtility.RegisterAsExecutionIfPrisoner(target, invoker);
			}
			PsychicRitualUtility.AddPsychicRitualGuiltToPawns(psychicRitual.def, from p in psychicRitual.Map.mapPawns.FreeColonistsSpawned
			where p != target
			select p);
			TaggedString taggedString = "PsychophagyCompleteText".Translate(invoker.Named("INVOKER"), psychicRitual.def.Named("RITUAL"), target.Named("TARGET"));
			if (target.Dead)
			{
				taggedString += "\n\n" + "PsychicRitualTargetBrainLiquified".Translate(target.Named("TARGET"));
			}
			else
			{
				taggedString += "\n\n" + "PsychophagyTargetSurvived".Translate(target.Named("TARGET"));
			}
			Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label), taggedString, LetterDefOf.NeutralEvent, new LookTargets(new TargetInfo[]
			{
				invoker,
				target
			}), null, null, null, null, 0, true);
		}

		// Token: 0x06004804 RID: 18436 RVA: 0x001C4288 File Offset: 0x001C2488
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.targetRole, "targetRole");
			Scribe_Values.Look<FloatRange>(ref this.brainDamageRange, "brainDamageRange", default(FloatRange), false);
			Scribe_Values.Look<int>(ref this.effectDurationTicks, "effectDurationTicks", 0, false);
		}

		// Token: 0x04002E87 RID: 11911
		public PsychicRitualRoleDef targetRole;

		// Token: 0x04002E88 RID: 11912
		public PsychicRitualRoleDef invokerRole;

		// Token: 0x04002E89 RID: 11913
		public FloatRange brainDamageRange;

		// Token: 0x04002E8A RID: 11914
		public int effectDurationTicks;
	}
}
