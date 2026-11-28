using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001523 RID: 5411
	public class InteractionWorker_RomanceAttempt : InteractionWorker
	{
		// Token: 0x06008465 RID: 33893 RVA: 0x002FAE2C File Offset: 0x002F902C
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			if (TutorSystem.TutorialMode)
			{
				return 0f;
			}
			if (initiator.DevelopmentalStage.Juvenile() || recipient.DevelopmentalStage.Juvenile())
			{
				return 0f;
			}
			if (initiator.Inhumanized())
			{
				return 0f;
			}
			if (LovePartnerRelationUtility.LovePartnerRelationExists(initiator, recipient))
			{
				return 0f;
			}
			float num = initiator.relations.SecondaryRomanceChanceFactor(recipient);
			if (num < 0.15f)
			{
				return 0f;
			}
			float num2 = 5f;
			int num3 = initiator.relations.OpinionOf(recipient);
			if ((float)num3 < num2)
			{
				return 0f;
			}
			if ((float)recipient.relations.OpinionOf(initiator) < num2)
			{
				return 0f;
			}
			float num4 = 1f;
			if (!new HistoryEvent(initiator.GetHistoryEventForLoveRelationCountPlusOne(), initiator.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo())
			{
				Pawn pawn = LovePartnerRelationUtility.ExistingMostLikedLovePartner(initiator, false);
				if (pawn != null)
				{
					float value = (float)initiator.relations.OpinionOf(pawn);
					num4 = Mathf.InverseLerp(50f, -50f, value);
				}
			}
			float num5 = initiator.story.traits.HasTrait(TraitDefOf.Gay) ? 1f : ((initiator.gender == Gender.Female) ? 0.15f : 1f);
			float num6 = Mathf.InverseLerp(0.15f, 1f, num);
			float num7 = Mathf.InverseLerp(num2, 100f, (float)num3);
			float num8;
			if (initiator.gender == recipient.gender)
			{
				if (initiator.story.traits.HasTrait(TraitDefOf.Gay) && recipient.story.traits.HasTrait(TraitDefOf.Gay))
				{
					num8 = 1f;
				}
				else
				{
					num8 = 0.15f;
				}
			}
			else if (!initiator.story.traits.HasTrait(TraitDefOf.Gay) && !recipient.story.traits.HasTrait(TraitDefOf.Gay))
			{
				num8 = 1f;
			}
			else
			{
				num8 = 0.15f;
			}
			return 1.15f * num5 * num6 * num7 * num4 * num8;
		}

		// Token: 0x06008466 RID: 33894 RVA: 0x002FB01C File Offset: 0x002F921C
		public static float SuccessChance(Pawn initiator, Pawn recipient, float baseChance = 0.6f)
		{
			if (recipient.Inhumanized())
			{
				return 0f;
			}
			if (!InteractionWorker_RomanceAttempt.CanCreatePsychicBondBetween(initiator, recipient))
			{
				return Mathf.Clamp01(((ModsConfig.BiotechActive && initiator.CurJobDef == JobDefOf.TryRomance) ? 1f : baseChance) * recipient.relations.SecondaryRomanceChanceFactor(initiator) * InteractionWorker_RomanceAttempt.OpinionFactor(initiator, recipient) * InteractionWorker_RomanceAttempt.PartnerFactor(initiator, recipient));
			}
			if (initiator.IsQuestHelper() || recipient.IsQuestHelper())
			{
				return 0f;
			}
			return 1f;
		}

		// Token: 0x06008467 RID: 33895 RVA: 0x002FB09C File Offset: 0x002F929C
		private static float PartnerFactor(Pawn initiator, Pawn recipient)
		{
			float num = 1f;
			if (!new HistoryEvent(recipient.GetHistoryEventForLoveRelationCountPlusOne(), recipient.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo())
			{
				Pawn pawn = null;
				if (recipient.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Lover, (Pawn x) => !x.Dead) != null)
				{
					pawn = recipient.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Lover, null);
					num = 0.6f;
				}
				else if (recipient.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Fiance, (Pawn x) => !x.Dead) != null)
				{
					pawn = recipient.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Fiance, null);
					num = 0.1f;
				}
				else if (recipient.GetSpouseCount(false) > 0)
				{
					pawn = recipient.GetMostLikedSpouseRelation().otherPawn;
					num = 0.3f;
				}
				if (pawn != null)
				{
					num *= Mathf.InverseLerp(100f, 0f, (float)recipient.relations.OpinionOf(pawn));
					num *= Mathf.Clamp01(1f - recipient.relations.SecondaryRomanceChanceFactor(pawn));
				}
			}
			return num;
		}

		// Token: 0x06008468 RID: 33896 RVA: 0x002FB1C0 File Offset: 0x002F93C0
		private static float OpinionFactor(Pawn initiator, Pawn recipient)
		{
			return Mathf.InverseLerp(5f, 100f, (float)recipient.relations.OpinionOf(initiator));
		}

		// Token: 0x06008469 RID: 33897 RVA: 0x002FB1E0 File Offset: 0x002F93E0
		public static string RomanceFactors(Pawn romancer, Pawn romanceTarget)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(InteractionWorker_RomanceAttempt.RomanceFactorLine("RomanceChanceOpinionFactor".Translate(), InteractionWorker_RomanceAttempt.OpinionFactor(romancer, romanceTarget)));
			stringBuilder.AppendLine(InteractionWorker_RomanceAttempt.RomanceFactorLine("RomanceChanceAgeFactor".Translate(), romanceTarget.relations.LovinAgeFactor(romancer)));
			float num = InteractionWorker_RomanceAttempt.PartnerFactor(romancer, romanceTarget);
			if (num != 1f)
			{
				stringBuilder.AppendLine(InteractionWorker_RomanceAttempt.RomanceFactorLine("RomanceChancePartnerFactor".Translate(), num));
			}
			float num2 = romanceTarget.relations.PrettinessFactor(romancer);
			if (num2 != 1f)
			{
				stringBuilder.AppendLine(InteractionWorker_RomanceAttempt.RomanceFactorLine("RomanceChanceBeautyFactor".Translate(), num2));
			}
			HediffWithTarget hediffWithTarget = (HediffWithTarget)romanceTarget.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicLove, false);
			if (hediffWithTarget != null && hediffWithTarget.target == romancer)
			{
				stringBuilder.AppendLine(InteractionWorker_RomanceAttempt.RomanceFactorLine("PsychicLoveFor".Translate() + " " + romancer.LabelShort, 10f));
			}
			if (ModsConfig.BiotechActive)
			{
				if (romancer.genes != null)
				{
					List<Gene> genesListForReading = romancer.genes.GenesListForReading;
					for (int i = 0; i < genesListForReading.Count; i++)
					{
						if (genesListForReading[i].Active && genesListForReading[i].def.missingGeneRomanceChanceFactor != 1f && (romanceTarget.genes == null || !romanceTarget.genes.HasActiveGene(genesListForReading[i].def)))
						{
							float value = genesListForReading[i].def.missingGeneRomanceChanceFactor;
							string str = string.Empty;
							Pawn_StoryTracker story = romanceTarget.story;
							if (((story != null) ? story.traits : null) != null && romanceTarget.story.traits.HasTrait(TraitDefOf.Kind))
							{
								value = 1f;
								str = " (" + TraitDefOf.Kind.DataAtDegree(0).label + ")";
							}
							stringBuilder.AppendLine(InteractionWorker_RomanceAttempt.RomanceFactorLine(genesListForReading[i].def.LabelCap + " (" + romancer.NameShortColored.Resolve() + ")", value) + str);
						}
					}
				}
				if (romanceTarget.genes != null)
				{
					List<Gene> genesListForReading2 = romanceTarget.genes.GenesListForReading;
					for (int j = 0; j < genesListForReading2.Count; j++)
					{
						if (genesListForReading2[j].Active && genesListForReading2[j].def.missingGeneRomanceChanceFactor != 1f && (romancer.genes == null || !romancer.genes.HasActiveGene(genesListForReading2[j].def)))
						{
							float value2 = genesListForReading2[j].def.missingGeneRomanceChanceFactor;
							string str2 = string.Empty;
							Pawn_StoryTracker story2 = romanceTarget.story;
							if (((story2 != null) ? story2.traits : null) != null && romancer.story.traits.HasTrait(TraitDefOf.Kind))
							{
								value2 = 1f;
								str2 = " (" + TraitDefOf.Kind.DataAtDegree(0).label + ")";
							}
							stringBuilder.AppendLine(InteractionWorker_RomanceAttempt.RomanceFactorLine(genesListForReading2[j].def.LabelCap + " (" + romanceTarget.NameShortColored + ")", value2) + str2);
						}
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600846A RID: 33898 RVA: 0x002FB599 File Offset: 0x002F9799
		private static string RomanceFactorLine(string label, float value)
		{
			return " - " + label + ": x" + value.ToStringPercent();
		}

		// Token: 0x0600846B RID: 33899 RVA: 0x002FB5B4 File Offset: 0x002F97B4
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			Job curJob = initiator.CurJob;
			if (((curJob != null) ? curJob.def : null) == JobDefOf.TryRomance)
			{
				initiator.relations.romanceEnableTick = Find.TickManager.TicksGame + 900000;
			}
			if (Rand.Value < InteractionWorker_RomanceAttempt.SuccessChance(initiator, recipient, 0.6f))
			{
				List<Pawn> list;
				this.BreakLoverAndFianceRelations(initiator, out list);
				List<Pawn> list2;
				this.BreakLoverAndFianceRelations(recipient, out list2);
				this.RemoveBrokeUpAndFailedRomanceThoughts(initiator, recipient);
				this.RemoveBrokeUpAndFailedRomanceThoughts(recipient, initiator);
				for (int i = 0; i < list.Count; i++)
				{
					this.TryAddCheaterThought(list[i], initiator);
				}
				for (int j = 0; j < list2.Count; j++)
				{
					this.TryAddCheaterThought(list2[j], recipient);
				}
				initiator.relations.TryRemoveDirectRelation(PawnRelationDefOf.ExLover, recipient);
				initiator.relations.AddDirectRelation(PawnRelationDefOf.Lover, recipient);
				TaleRecorder.RecordTale(TaleDefOf.BecameLover, new object[]
				{
					initiator,
					recipient
				});
				bool createdBond = false;
				if (InteractionWorker_RomanceAttempt.CanCreatePsychicBondBetween(initiator, recipient))
				{
					createdBond = InteractionWorker_RomanceAttempt.TryCreatePsychicBondBetween(initiator, recipient);
				}
				if (PawnUtility.ShouldSendNotificationAbout(initiator) || PawnUtility.ShouldSendNotificationAbout(recipient))
				{
					this.GetNewLoversLetter(initiator, recipient, list, list2, createdBond, out letterText, out letterLabel, out letterDef, out lookTargets);
				}
				else
				{
					letterText = null;
					letterLabel = null;
					letterDef = null;
					lookTargets = null;
				}
				extraSentencePacks.Add(RulePackDefOf.Sentence_RomanceAttemptAccepted);
				LovePartnerRelationUtility.TryToShareBed(initiator, recipient);
				return;
			}
			if (initiator.needs.mood != null)
			{
				initiator.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RebuffedMyRomanceAttempt, recipient, null);
			}
			if (recipient.needs.mood != null)
			{
				recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.FailedRomanceAttemptOnMe, initiator, null);
			}
			if (recipient.needs.mood != null && recipient.relations.OpinionOf(initiator) <= 0)
			{
				recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.FailedRomanceAttemptOnMeLowOpinionMood, initiator, null);
			}
			extraSentencePacks.Add(RulePackDefOf.Sentence_RomanceAttemptRejected);
			letterText = null;
			letterLabel = null;
			letterDef = null;
			lookTargets = null;
			Job curJob2 = initiator.CurJob;
			if (((curJob2 != null) ? curJob2.def : null) == JobDefOf.TryRomance)
			{
				Messages.Message("TryRomanceFailedMessage".Translate(initiator, recipient), initiator, MessageTypeDefOf.NegativeEvent, false);
			}
		}

		// Token: 0x0600846C RID: 33900 RVA: 0x002FB800 File Offset: 0x002F9A00
		public static bool CanCreatePsychicBondBetween(Pawn initiator, Pawn recipient)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			Pawn_GeneTracker genes = initiator.genes;
			Gene_PsychicBonding gene_PsychicBonding = (genes != null) ? genes.GetFirstGeneOfType<Gene_PsychicBonding>() : null;
			Pawn_GeneTracker genes2 = recipient.genes;
			Gene_PsychicBonding gene_PsychicBonding2 = (genes2 != null) ? genes2.GetFirstGeneOfType<Gene_PsychicBonding>() : null;
			return (gene_PsychicBonding != null || gene_PsychicBonding2 != null) && (gene_PsychicBonding == null || gene_PsychicBonding.CanBondToNewPawn) && (gene_PsychicBonding2 == null || gene_PsychicBonding2.CanBondToNewPawn);
		}

		// Token: 0x0600846D RID: 33901 RVA: 0x002FB85C File Offset: 0x002F9A5C
		public static bool TryCreatePsychicBondBetween(Pawn initiator, Pawn recipient)
		{
			Pawn_GeneTracker genes = initiator.genes;
			Gene_PsychicBonding gene_PsychicBonding = (genes != null) ? genes.GetFirstGeneOfType<Gene_PsychicBonding>() : null;
			Pawn_GeneTracker genes2 = recipient.genes;
			Gene_PsychicBonding gene_PsychicBonding2 = (genes2 != null) ? genes2.GetFirstGeneOfType<Gene_PsychicBonding>() : null;
			if (gene_PsychicBonding != null && gene_PsychicBonding2 != null && (!gene_PsychicBonding.CanBondToNewPawn || !gene_PsychicBonding2.CanBondToNewPawn))
			{
				return false;
			}
			if (gene_PsychicBonding != null && gene_PsychicBonding.CanBondToNewPawn)
			{
				gene_PsychicBonding.BondTo(recipient);
				return true;
			}
			if (gene_PsychicBonding2 != null && gene_PsychicBonding2.CanBondToNewPawn)
			{
				gene_PsychicBonding2.BondTo(initiator);
				return true;
			}
			return false;
		}

		// Token: 0x0600846E RID: 33902 RVA: 0x002FB8D0 File Offset: 0x002F9AD0
		private void RemoveBrokeUpAndFailedRomanceThoughts(Pawn pawn, Pawn otherPawn)
		{
			if (pawn.needs.mood != null)
			{
				pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.BrokeUpWithMe, otherPawn);
				pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.FailedRomanceAttemptOnMe, otherPawn);
				pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.FailedRomanceAttemptOnMeLowOpinionMood, otherPawn);
			}
		}

		// Token: 0x0600846F RID: 33903 RVA: 0x002FB94C File Offset: 0x002F9B4C
		private void BreakLoverAndFianceRelations(Pawn pawn, out List<Pawn> oldLoversAndFiances)
		{
			oldLoversAndFiances = new List<Pawn>();
			int num = 200;
			while (num > 0 && !new HistoryEvent(pawn.GetHistoryEventForLoveRelationCountPlusOne(), pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo())
			{
				Pawn pawn2 = LovePartnerRelationUtility.ExistingLeastLikedPawnWithRelation(pawn, (DirectPawnRelation r) => r.def == PawnRelationDefOf.Lover);
				if (pawn2 != null)
				{
					pawn.relations.RemoveDirectRelation(PawnRelationDefOf.Lover, pawn2);
					pawn.relations.AddDirectRelation(PawnRelationDefOf.ExLover, pawn2);
					oldLoversAndFiances.Add(pawn2);
					num--;
				}
				else
				{
					Pawn pawn3 = LovePartnerRelationUtility.ExistingLeastLikedPawnWithRelation(pawn, (DirectPawnRelation r) => r.def == PawnRelationDefOf.Fiance);
					if (pawn3 == null)
					{
						break;
					}
					pawn.relations.RemoveDirectRelation(PawnRelationDefOf.Fiance, pawn3);
					pawn.relations.AddDirectRelation(PawnRelationDefOf.ExLover, pawn3);
					oldLoversAndFiances.Add(pawn3);
					num--;
				}
			}
		}

		// Token: 0x06008470 RID: 33904 RVA: 0x002FBA42 File Offset: 0x002F9C42
		private void TryAddCheaterThought(Pawn pawn, Pawn cheater)
		{
			if (pawn.Dead || pawn.needs.mood == null)
			{
				return;
			}
			pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.CheatedOnMe, cheater, null);
		}

		// Token: 0x06008471 RID: 33905 RVA: 0x002FBA7C File Offset: 0x002F9C7C
		private void GetNewLoversLetter(Pawn initiator, Pawn recipient, List<Pawn> initiatorOldLoversAndFiances, List<Pawn> recipientOldLoversAndFiances, bool createdBond, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			bool flag = false;
			HistoryEvent ev = new HistoryEvent(initiator.GetHistoryEventLoveRelationCount(), initiator.Named(HistoryEventArgsNames.Doer));
			HistoryEvent ev2 = new HistoryEvent(recipient.GetHistoryEventLoveRelationCount(), recipient.Named(HistoryEventArgsNames.Doer));
			if (!ev.DoerWillingToDo() || !ev2.DoerWillingToDo())
			{
				letterLabel = "LetterLabelAffair".Translate();
				letterDef = LetterDefOf.NegativeEvent;
				flag = true;
			}
			else
			{
				letterLabel = "LetterLabelNewLovers".Translate();
				letterDef = LetterDefOf.PositiveEvent;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (BedUtility.WillingToShareBed(initiator, recipient))
			{
				stringBuilder.AppendLineTagged("LetterNewLovers".Translate(initiator.Named("PAWN1"), recipient.Named("PAWN2")));
			}
			if (flag)
			{
				Pawn firstSpouse = initiator.GetFirstSpouse();
				if (firstSpouse != null)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLineTagged("LetterAffair".Translate(initiator.LabelShort, firstSpouse.LabelShort, recipient.LabelShort, initiator.Named("PAWN1"), recipient.Named("PAWN2"), firstSpouse.Named("SPOUSE")));
				}
				Pawn firstSpouse2 = recipient.GetFirstSpouse();
				if (firstSpouse2 != null)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLineTagged("LetterAffair".Translate(recipient.LabelShort, firstSpouse2.LabelShort, initiator.LabelShort, recipient.Named("PAWN1"), firstSpouse2.Named("SPOUSE"), initiator.Named("PAWN2")));
				}
			}
			for (int i = 0; i < initiatorOldLoversAndFiances.Count; i++)
			{
				if (!initiatorOldLoversAndFiances[i].Dead)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLineTagged("LetterNoLongerLovers".Translate(initiator.LabelShort, initiatorOldLoversAndFiances[i].LabelShort, initiator.Named("PAWN1"), initiatorOldLoversAndFiances[i].Named("PAWN2")));
				}
			}
			for (int j = 0; j < recipientOldLoversAndFiances.Count; j++)
			{
				if (!recipientOldLoversAndFiances[j].Dead)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLineTagged("LetterNoLongerLovers".Translate(recipient.LabelShort, recipientOldLoversAndFiances[j].LabelShort, recipient.Named("PAWN1"), recipientOldLoversAndFiances[j].Named("PAWN2")));
				}
			}
			if (createdBond)
			{
				Pawn pawn = (initiator.genes.GetFirstGeneOfType<Gene_PsychicBonding>() != null) ? initiator : recipient;
				Pawn arg = (pawn == initiator) ? recipient : initiator;
				stringBuilder.AppendLine();
				stringBuilder.AppendLineTagged("LetterPsychicBondCreated".Translate(pawn.Named("BONDPAWN"), arg.Named("OTHERPAWN")));
			}
			letterText = stringBuilder.ToString().TrimEndNewlines();
			lookTargets = new LookTargets(new TargetInfo[]
			{
				initiator,
				recipient
			});
		}

		// Token: 0x040051C9 RID: 20937
		private const float MinRomanceChanceForRomanceAttempt = 0.15f;

		// Token: 0x040051CA RID: 20938
		public const int MinOpinionForRomanceAttempt = 5;

		// Token: 0x040051CB RID: 20939
		private const float BaseSelectionWeight = 1.15f;

		// Token: 0x040051CC RID: 20940
		private const float BaseSuccessChance = 0.6f;

		// Token: 0x040051CD RID: 20941
		public const float TryRomanceSuccessChance = 1f;

		// Token: 0x040051CE RID: 20942
		private const int TryRomanceCooldownTicks = 900000;
	}
}
