using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020013E8 RID: 5096
	public class CompAbilityEffect_Counsel : CompAbilityEffect
	{
		// Token: 0x170015A9 RID: 5545
		// (get) Token: 0x06007E9F RID: 32415 RVA: 0x002D419F File Offset: 0x002D239F
		public new CompProperties_AbilityCounsel Props
		{
			get
			{
				return (CompProperties_AbilityCounsel)this.props;
			}
		}

		// Token: 0x170015AA RID: 5546
		// (get) Token: 0x06007EA0 RID: 32416 RVA: 0x002D41AC File Offset: 0x002D23AC
		public override bool HideTargetPawnTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007EA1 RID: 32417 RVA: 0x002D41B0 File Offset: 0x002D23B0
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (!ModLister.CheckIdeology("Ideoligion councel"))
			{
				return;
			}
			Pawn pawn = target.Pawn;
			if (Rand.Chance(this.ChanceForPawn(pawn)))
			{
				List<Thought> list = new List<Thought>();
				pawn.needs.mood.thoughts.GetAllMoodThoughts(list);
				Thought_Memory thought_Memory = (Thought_Memory)(from t in list
				where t is Thought_Memory && t.MoodOffset() <= this.Props.minMoodOffset
				select t).MaxByWithFallback((Thought t) => -t.MoodOffset(), null);
				if (thought_Memory != null)
				{
					Thought_Counselled thought_Counselled = (Thought_Counselled)ThoughtMaker.MakeThought(ThoughtDefOf.Counselled);
					thought_Counselled.durationTicksOverride = thought_Memory.DurationTicks - thought_Memory.age;
					thought_Counselled.moodOffsetOverride = -thought_Memory.MoodOffset();
					pawn.needs.mood.thoughts.memories.TryGainMemory(thought_Counselled, this.parent.pawn);
					Messages.Message(this.Props.successMessage.Formatted(this.parent.pawn.Named("INITIATOR"), pawn.Named("RECIPIENT"), thought_Memory.MoodOffset()), new LookTargets(new Pawn[]
					{
						this.parent.pawn,
						pawn
					}), MessageTypeDefOf.PositiveEvent, false);
				}
				else
				{
					Thought_Memory thought_Memory2 = (Thought_Memory)ThoughtMaker.MakeThought(ThoughtDefOf.Counselled_MoodBoost);
					pawn.needs.mood.thoughts.memories.TryGainMemory(thought_Memory2, this.parent.pawn);
					Messages.Message(this.Props.successMessageNoNegativeThought.Formatted(this.parent.pawn.Named("INITIATOR"), pawn.Named("RECIPIENT"), thought_Memory2.def.stages[0].baseMoodEffect.Named("MOODBONUS")), new LookTargets(new Pawn[]
					{
						this.parent.pawn,
						pawn
					}), MessageTypeDefOf.PositiveEvent, false);
				}
				PlayLogEntry_Interaction entry = new PlayLogEntry_Interaction(InteractionDefOf.Counsel_Success, this.parent.pawn, pawn, null);
				Find.PlayLog.Add(entry);
			}
			else
			{
				pawn.needs.mood.thoughts.memories.TryGainMemory(this.Props.failedThoughtRecipient, this.parent.pawn, null);
				PlayLogEntry_Interaction entry2 = new PlayLogEntry_Interaction(InteractionDefOf.Counsel_Failure, this.parent.pawn, pawn, null);
				Find.PlayLog.Add(entry2);
				Messages.Message(this.Props.failMessage.Formatted(this.parent.pawn.Named("INITIATOR"), pawn.Named("RECIPIENT")), new LookTargets(new Pawn[]
				{
					this.parent.pawn,
					pawn
				}), MessageTypeDefOf.NegativeEvent, false);
			}
			if (this.Props.sound != null)
			{
				this.Props.sound.PlayOneShot(new TargetInfo(target.Cell, this.parent.pawn.Map, false));
			}
		}

		// Token: 0x06007EA2 RID: 32418 RVA: 0x002D44E0 File Offset: 0x002D26E0
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			if (pawn == null)
			{
				return false;
			}
			if (!AbilityUtility.ValidateMustBeHuman(pawn, throwMessages, this.parent))
			{
				return false;
			}
			if (this.parent.pawn.Ideo != pawn.Ideo)
			{
				if (throwMessages)
				{
					Precept_Role role = this.parent.pawn.Ideo.GetRole(this.parent.pawn);
					Messages.Message("CannotUseAbility".Translate(this.parent.def.label) + ": " + "AbilityMustBeSameIdeoCounsel".Translate(role.LabelForPawn(this.parent.pawn), this.parent.pawn.Ideo.memberName), pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (!AbilityUtility.ValidateNoMentalState(pawn, throwMessages, this.parent))
			{
				return false;
			}
			List<Thought> list = new List<Thought>();
			pawn.needs.mood.thoughts.GetAllMoodThoughts(list);
			if (list.Any((Thought t) => t.def == ThoughtDefOf.Counselled))
			{
				if (throwMessages)
				{
					Messages.Message("AbilityCantApplyAlreadyCounselled".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06007EA3 RID: 32419 RVA: 0x002D4654 File Offset: 0x002D2854
		public float ChanceForPawn(Pawn pawn)
		{
			return CompAbilityEffect_Counsel.SuccessChanceBySocialSkill.Evaluate((float)this.parent.pawn.skills.GetSkill(SkillDefOf.Social).Level) * CompAbilityEffect_Counsel.SuccessChanceFactorByOpinion.Evaluate((float)pawn.relations.OpinionOf(this.parent.pawn));
		}

		// Token: 0x06007EA4 RID: 32420 RVA: 0x002D46B0 File Offset: 0x002D28B0
		public override string ExtraLabelMouseAttachment(LocalTargetInfo target)
		{
			Pawn pawn = target.Pawn;
			if (pawn == null)
			{
				return null;
			}
			if (!this.Valid(target, false))
			{
				return null;
			}
			Pawn pawn2 = this.parent.pawn;
			Pawn pawn3 = pawn;
			string t = this.ChanceForPawn(pawn).ToStringPercent();
			string t2 = CompAbilityEffect_Counsel.SuccessChanceBySocialSkill.Evaluate((float)pawn2.skills.GetSkill(SkillDefOf.Social).Level).ToStringPercent();
			string t3 = CompAbilityEffect_Counsel.SuccessChanceFactorByOpinion.Evaluate((float)pawn3.relations.OpinionOf(pawn2)).ToStringPercent();
			return "chance".Translate().CapitalizeFirst() + ": " + t + "\n\n" + "Factors".Translate() + ":\n" + " -  " + "AbilityIdeoConvertBreakdownSocialSkill".Translate(pawn2.Named("PAWN")) + " " + t2 + "\n" + " -  " + "AbilityIdeoConvertBreakdownOpinion".Translate(pawn2.Named("INITIATOR"), pawn3.Named("RECIPIENT")) + " " + t3;
		}

		// Token: 0x04004F6F RID: 20335
		public static readonly SimpleCurve SuccessChanceBySocialSkill = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.02f),
				true
			},
			{
				new CurvePoint(5f, 0.4f),
				true
			},
			{
				new CurvePoint(10f, 0.7f),
				true
			},
			{
				new CurvePoint(20f, 0.9f),
				true
			}
		};

		// Token: 0x04004F70 RID: 20336
		public static readonly SimpleCurve SuccessChanceFactorByOpinion = new SimpleCurve
		{
			{
				new CurvePoint(-100f, 0.7f),
				true
			},
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(100f, 1.3f),
				true
			}
		};
	}
}
