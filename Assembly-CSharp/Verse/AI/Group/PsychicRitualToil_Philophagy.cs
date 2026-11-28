using System;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200092C RID: 2348
	public class PsychicRitualToil_Philophagy : PsychicRitualToil
	{
		// Token: 0x060047F6 RID: 18422 RVA: 0x001C3764 File Offset: 0x001C1964
		protected PsychicRitualToil_Philophagy()
		{
		}

		// Token: 0x060047F7 RID: 18423 RVA: 0x001C376C File Offset: 0x001C196C
		public PsychicRitualToil_Philophagy(PsychicRitualRoleDef invokerRole, PsychicRitualRoleDef targetRole, FloatRange brainDamageRange)
		{
			this.invokerRole = invokerRole;
			this.targetRole = targetRole;
			this.brainDamageRange = brainDamageRange;
		}

		// Token: 0x060047F8 RID: 18424 RVA: 0x001C378C File Offset: 0x001C198C
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph graph)
		{
			PsychicRitualDef_Philophagy psychicRitualDef_Philophagy = (PsychicRitualDef_Philophagy)psychicRitual.def;
			Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
			Pawn pawn2 = psychicRitual.assignments.FirstAssignedPawn(this.targetRole);
			this.xpTransferPercent = psychicRitualDef_Philophagy.xpTransferFromQualityCurve.Evaluate(psychicRitual.PowerPercent);
			if (pawn != null && pawn2 != null)
			{
				this.ApplyOutcome(psychicRitual, pawn, pawn2);
			}
		}

		// Token: 0x060047F9 RID: 18425 RVA: 0x001C37F0 File Offset: 0x001C19F0
		private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker, Pawn target)
		{
			float num;
			SkillDef philophagySkillAndXpTransfer = PsychicRitualUtility.GetPhilophagySkillAndXpTransfer(invoker, target, this.xpTransferPercent, out num);
			if (philophagySkillAndXpTransfer == null)
			{
				Log.Error("Could not get skill for Philophagy ritual.");
				return;
			}
			int level = invoker.skills.GetSkill(philophagySkillAndXpTransfer).GetLevel(true);
			int level2 = target.skills.GetSkill(philophagySkillAndXpTransfer).GetLevel(true);
			invoker.skills.Learn(philophagySkillAndXpTransfer, num, true, true);
			target.skills.Learn(philophagySkillAndXpTransfer, -num, true, true);
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
							memories2.TryGainMemory(ThoughtDefOf.DrainedMySkills, invoker, null);
						}
					}
				}
			}
			foreach (Pawn otherPawn in psychicRitual.assignments.AllAssignedPawns.Except(target))
			{
				Pawn_NeedsTracker needs3 = target.needs;
				if (needs3 != null)
				{
					Need_Mood mood3 = needs3.mood;
					if (mood3 != null)
					{
						ThoughtHandler thoughts3 = mood3.thoughts;
						if (thoughts3 != null)
						{
							MemoryThoughtHandler memories3 = thoughts3.memories;
							if (memories3 != null)
							{
								memories3.TryGainMemory(ThoughtDefOf.UsedMeForPsychicRitual, otherPawn, null);
							}
						}
					}
				}
			}
			target.health.AddHediff(HediffDefOf.DarkPsychicShock, null, null, null);
			BodyPartRecord brain = target.health.hediffSet.GetBrain();
			if (brain != null)
			{
				target.TakeDamage(new DamageInfo(DamageDefOf.Psychic, this.brainDamageRange.RandomInRange, 0f, -1f, null, brain, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			}
			int level3 = invoker.skills.GetSkill(philophagySkillAndXpTransfer).GetLevel(true);
			int level4 = target.skills.GetSkill(philophagySkillAndXpTransfer).GetLevel(true);
			if (target.Dead)
			{
				PsychicRitualUtility.RegisterAsExecutionIfPrisoner(target, invoker);
			}
			PsychicRitualUtility.AddPsychicRitualGuiltToPawns(psychicRitual.def, from p in psychicRitual.Map.mapPawns.FreeColonistsSpawned
			where p != target
			select p);
			TaggedString t = "PhilophagyCompleteText".Translate(invoker.Named("INVOKER"), target.Named("TARGET"), psychicRitual.def.Named("RITUAL"));
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 1);
			defaultInterpolatedStringHandler.AppendLiteral("\n\n");
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("PawnGainedXPInSkill".Translate(invoker, num, philophagySkillAndXpTransfer));
			TaggedString t2 = t + defaultInterpolatedStringHandler.ToStringAndClear() + (", " + ((level3 > level) ? "PhilophagySkillLevelGained".Translate(level, level3).Resolve() : "PhilophagySkillLevelRemains".Translate(level3).Resolve()));
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(2, 1);
			defaultInterpolatedStringHandler2.AppendLiteral("\n\n");
			defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("PawnLostXPInSkill".Translate(target, num, philophagySkillAndXpTransfer));
			TaggedString text = t2 + defaultInterpolatedStringHandler2.ToStringAndClear() + (", " + ((level4 < level2) ? "PhilophagySkillLevelLost".Translate(level2, level4).Resolve() : "PhilophagySkillLevelRemains".Translate(level4).Resolve())) + "\n\n" + (target.Dead ? "PsychicRitualTargetBrainLiquified" : "PhilophagyDamagedTarget").Translate(target.Named("TARGET"));
			Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label), text, LetterDefOf.NeutralEvent, new LookTargets(new TargetInfo[]
			{
				invoker,
				target
			}), null, null, null, null, 0, true);
		}

		// Token: 0x060047FA RID: 18426 RVA: 0x001C3C6C File Offset: 0x001C1E6C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.targetRole, "targetRole");
			Scribe_Values.Look<FloatRange>(ref this.brainDamageRange, "brainDamageFromQualityRange", default(FloatRange), false);
			Scribe_Values.Look<float>(ref this.xpTransferPercent, "xpTransferPercent", 0f, false);
		}

		// Token: 0x04002E82 RID: 11906
		public PsychicRitualRoleDef targetRole;

		// Token: 0x04002E83 RID: 11907
		public PsychicRitualRoleDef invokerRole;

		// Token: 0x04002E84 RID: 11908
		public FloatRange brainDamageRange;

		// Token: 0x04002E85 RID: 11909
		public float xpTransferPercent;
	}
}
