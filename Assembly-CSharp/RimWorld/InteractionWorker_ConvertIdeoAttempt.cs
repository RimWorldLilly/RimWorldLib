using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001516 RID: 5398
	public class InteractionWorker_ConvertIdeoAttempt : InteractionWorker
	{
		// Token: 0x06008442 RID: 33858 RVA: 0x002F91E8 File Offset: 0x002F73E8
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return 0f;
			}
			if (Find.IdeoManager.classicMode)
			{
				return 0f;
			}
			if (initiator.Ideo == null || !recipient.RaceProps.Humanlike || initiator.Ideo == recipient.Ideo)
			{
				return 0f;
			}
			if (recipient.DevelopmentalStage.Baby())
			{
				return 0f;
			}
			if (initiator.skills.GetSkill(SkillDefOf.Social).TotallyDisabled)
			{
				return 0f;
			}
			return 0.04f * initiator.GetStatValue(StatDefOf.SocialIdeoSpreadFrequencyFactor, true, -1) * this.ConversionSelectionFactor(initiator, recipient);
		}

		// Token: 0x06008443 RID: 33859 RVA: 0x002F928C File Offset: 0x002F748C
		private float ConversionSelectionFactor(Pawn initiator, Pawn recipient)
		{
			InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory pawnConversionCategory = InteractionWorker_ConvertIdeoAttempt.<ConversionSelectionFactor>g__GetConversionCategory|2_0(initiator);
			InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory pawnConversionCategory2 = InteractionWorker_ConvertIdeoAttempt.<ConversionSelectionFactor>g__GetConversionCategory|2_0(recipient);
			switch (pawnConversionCategory)
			{
			case InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory.NPC_Free:
				switch (pawnConversionCategory2)
				{
				case InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory.Colonist:
					return 0.5f;
				case InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory.NPC_Free:
					return 0.25f;
				case InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory.NPC_Prisoner:
					return 0.25f;
				case InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory.Slave:
					return 0.5f;
				}
				break;
			case InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory.NPC_Prisoner:
				switch (pawnConversionCategory2)
				{
				case InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory.Colonist:
					return 0.25f;
				case InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory.NPC_Free:
					return 0.25f;
				case InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory.NPC_Prisoner:
					return 0.5f;
				case InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory.Slave:
					return 0.5f;
				}
				break;
			case InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory.Slave:
				return 0.5f;
			}
			return 1f;
		}

		// Token: 0x06008444 RID: 33860 RVA: 0x002F9328 File Offset: 0x002F7528
		public static float CertaintyReduction(Pawn initiator, Pawn recipient)
		{
			float num = 0.06f * initiator.GetStatValue(StatDefOf.ConversionPower, true, -1) * recipient.GetStatValue(StatDefOf.CertaintyLossFactor, true, -1) * ConversionUtility.ConversionPowerFactor_MemesVsTraits(initiator, recipient, null) * ReliquaryUtility.GetRelicConvertPowerFactorForPawn(initiator, null) * Find.Storyteller.difficulty.CertaintyReductionFactor(initiator, recipient);
			Ideo ideo = recipient.Ideo;
			Precept_Role precept_Role = (ideo != null) ? ideo.GetRole(recipient) : null;
			if (precept_Role != null)
			{
				num *= precept_Role.def.certaintyLossFactor;
			}
			return num;
		}

		// Token: 0x06008445 RID: 33861 RVA: 0x002F93A0 File Offset: 0x002F75A0
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			letterLabel = null;
			letterText = null;
			letterDef = null;
			lookTargets = null;
			Ideo ideo = recipient.Ideo;
			Precept_Role role = ideo.GetRole(recipient);
			float certainty = recipient.ideo.Certainty;
			if (recipient.ideo.IdeoConversionAttempt(InteractionWorker_ConvertIdeoAttempt.CertaintyReduction(initiator, recipient), initiator.Ideo, true))
			{
				if (PawnUtility.ShouldSendNotificationAbout(initiator) || PawnUtility.ShouldSendNotificationAbout(recipient))
				{
					letterLabel = "LetterLabelConvertIdeoAttempt_Success".Translate();
					letterText = "LetterConvertIdeoAttempt_Success".Translate(initiator.Named("INITIATOR"), recipient.Named("RECIPIENT"), initiator.Ideo.Named("IDEO"), ideo.Named("OLDIDEO")).Resolve();
					letterDef = LetterDefOf.PositiveEvent;
					lookTargets = new LookTargets(new TargetInfo[]
					{
						initiator,
						recipient
					});
					if (role != null)
					{
						letterText = letterText + "\n\n" + "LetterRoleLostLetterIdeoChangedPostfix".Translate(recipient.Named("PAWN"), role.Named("ROLE"), ideo.Named("OLDIDEO")).Resolve();
					}
				}
				extraSentencePacks.Add(RulePackDefOf.Sentence_ConvertIdeoAttemptSuccess);
				return;
			}
			float num = recipient.interactions.SocialFightPossible(initiator) ? 0.02f : 0f;
			float num2 = Rand.Value * (0.97999996f + num);
			if (num2 < 0.78f || recipient.IsPrisoner)
			{
				extraSentencePacks.Add(RulePackDefOf.Sentence_ConvertIdeoAttemptFail);
				return;
			}
			if (num2 < 0.97999996f)
			{
				if (recipient.needs.mood != null)
				{
					if (PawnUtility.ShouldSendNotificationAbout(recipient))
					{
						Messages.Message("MessageFailedConvertIdeoAttempt".Translate(initiator.Named("INITIATOR"), recipient.Named("RECIPIENT"), certainty.ToStringPercent().Named("CERTAINTYBEFORE"), recipient.ideo.Certainty.ToStringPercent().Named("CERTAINTYAFTER")), recipient, MessageTypeDefOf.NeutralEvent, true);
					}
					recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.FailedConvertIdeoAttemptResentment, initiator, null);
				}
				extraSentencePacks.Add(RulePackDefOf.Sentence_ConvertIdeoAttemptFailResentment);
				return;
			}
			recipient.interactions.StartSocialFight(initiator, "MessageFailedConvertIdeoAttemptSocialFight");
			extraSentencePacks.Add(RulePackDefOf.Sentence_ConvertIdeoAttemptFailSocialFight);
		}

		// Token: 0x06008447 RID: 33863 RVA: 0x002F9601 File Offset: 0x002F7801
		[CompilerGenerated]
		internal static InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory <ConversionSelectionFactor>g__GetConversionCategory|2_0(Pawn pawn)
		{
			if (pawn.IsSlave)
			{
				return InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory.Slave;
			}
			if (pawn.IsColonist || pawn.IsPrisonerOfColony)
			{
				return InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory.Colonist;
			}
			if (pawn.IsPrisoner)
			{
				return InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory.NPC_Prisoner;
			}
			return InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory.NPC_Free;
		}

		// Token: 0x02003360 RID: 13152
		private enum PawnConversionCategory
		{
			// Token: 0x0400CE4F RID: 52815
			Colonist,
			// Token: 0x0400CE50 RID: 52816
			NPC_Free,
			// Token: 0x0400CE51 RID: 52817
			NPC_Prisoner,
			// Token: 0x0400CE52 RID: 52818
			Slave
		}
	}
}
