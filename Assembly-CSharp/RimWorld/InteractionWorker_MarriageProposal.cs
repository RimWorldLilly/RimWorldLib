using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200151E RID: 5406
	public class InteractionWorker_MarriageProposal : InteractionWorker
	{
		// Token: 0x06008456 RID: 33878 RVA: 0x002F9CCC File Offset: 0x002F7ECC
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			DirectPawnRelation directRelation = initiator.relations.GetDirectRelation(PawnRelationDefOf.Lover, recipient);
			if (directRelation == null)
			{
				return 0f;
			}
			if (initiator.Inhumanized())
			{
				return 0f;
			}
			HistoryEvent ev = new HistoryEvent(initiator.GetHistoryEventForSpouseAndFianceCountPlusOne(), initiator.Named(HistoryEventArgsNames.Doer));
			HistoryEvent ev2 = new HistoryEvent(recipient.GetHistoryEventForSpouseAndFianceCountPlusOne(), recipient.Named(HistoryEventArgsNames.Doer));
			if (!ev.DoerWillingToDo() || !ev2.DoerWillingToDo())
			{
				return 0f;
			}
			float num = 0.4f;
			float value = (float)(Find.TickManager.TicksGame - directRelation.startTicks) / 60000f;
			num *= Mathf.InverseLerp(0f, 60f, value);
			num *= Mathf.InverseLerp(0f, 60f, (float)initiator.relations.OpinionOf(recipient));
			if (recipient.relations.OpinionOf(initiator) < 0)
			{
				num *= 0.3f;
			}
			if (initiator.gender == Gender.Female)
			{
				num *= 0.2f;
			}
			HediffWithTarget hediffWithTarget = (HediffWithTarget)initiator.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicLove, false);
			if (hediffWithTarget != null && hediffWithTarget.target == recipient)
			{
				num *= 10f;
			}
			if (initiator.health.hediffSet.HasPregnancyHediff() || recipient.health.hediffSet.HasPregnancyHediff())
			{
				num *= 3f;
			}
			foreach (Pawn pawn in initiator.relations.Children)
			{
				if (pawn.DevelopmentalStage.Baby() && !pawn.Dead && recipient.relations.Children.Contains(pawn))
				{
					num *= 2f;
					break;
				}
			}
			return num;
		}

		// Token: 0x06008457 RID: 33879 RVA: 0x002F9E98 File Offset: 0x002F8098
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			float num = InteractionWorker_MarriageProposal.AcceptanceChance(initiator, recipient);
			bool flag = Rand.Value < num;
			bool flag2 = false;
			if (flag)
			{
				initiator.relations.RemoveDirectRelation(PawnRelationDefOf.Lover, recipient);
				initiator.relations.AddDirectRelation(PawnRelationDefOf.Fiance, recipient);
				if (recipient.needs.mood != null)
				{
					recipient.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.RejectedMyProposal, initiator);
					recipient.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.RejectedMyProposalMood, initiator);
					recipient.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.IRejectedTheirProposal, initiator);
				}
				if (initiator.needs.mood != null)
				{
					initiator.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.RejectedMyProposalMood, recipient);
					initiator.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.IRejectedTheirProposal, recipient);
					initiator.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.RejectedMyProposal, recipient);
				}
				extraSentencePacks.Add(RulePackDefOf.Sentence_MarriageProposalAccepted);
			}
			else
			{
				Need_Mood mood = initiator.needs.mood;
				if (mood != null)
				{
					mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RejectedMyProposal, recipient, null);
				}
				Need_Mood mood2 = recipient.needs.mood;
				if (mood2 != null)
				{
					mood2.thoughts.memories.TryGainMemory(ThoughtDefOf.IRejectedTheirProposal, initiator, null);
				}
				extraSentencePacks.Add(RulePackDefOf.Sentence_MarriageProposalRejected);
				if (Rand.Value < 0.4f)
				{
					initiator.relations.RemoveDirectRelation(PawnRelationDefOf.Lover, recipient);
					initiator.relations.AddDirectRelation(PawnRelationDefOf.ExLover, recipient);
					flag2 = true;
					extraSentencePacks.Add(RulePackDefOf.Sentence_MarriageProposalRejectedBrokeUp);
				}
			}
			if (PawnUtility.ShouldSendNotificationAbout(initiator) || PawnUtility.ShouldSendNotificationAbout(recipient))
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (flag)
				{
					letterLabel = "LetterLabelAcceptedProposal".Translate();
					letterDef = LetterDefOf.PositiveEvent;
					stringBuilder.AppendLine("LetterAcceptedProposal".Translate(initiator.Named("INITIATOR"), recipient.Named("RECIPIENT")));
					if (initiator.relations.nextMarriageNameChange != MarriageNameChange.NoChange)
					{
						Pawn pawn;
						Pawn pawn2;
						SpouseRelationUtility.DetermineManAndWomanSpouses(initiator, recipient, out pawn, out pawn2);
						stringBuilder.AppendLine();
						if (initiator.relations.nextMarriageNameChange == MarriageNameChange.MansName)
						{
							stringBuilder.AppendLine("LetterAcceptedProposal_NameChange".Translate(pawn2.Named("PAWN"), (pawn.Name as NameTriple).Last));
						}
						else
						{
							stringBuilder.AppendLine("LetterAcceptedProposal_NameChange".Translate(pawn.Named("PAWN"), (pawn2.Name as NameTriple).Last));
						}
					}
				}
				else
				{
					letterLabel = "LetterLabelRejectedProposal".Translate();
					letterDef = LetterDefOf.NegativeEvent;
					stringBuilder.AppendLine("LetterRejectedProposal".Translate(initiator.Named("INITIATOR"), recipient.Named("RECIPIENT")));
					if (flag2)
					{
						stringBuilder.AppendLine();
						stringBuilder.AppendLine("LetterNoLongerLovers".Translate(initiator.Named("PAWN1"), recipient.Named("PAWN2")));
					}
				}
				letterText = stringBuilder.ToString().TrimEndNewlines();
				lookTargets = new LookTargets(new TargetInfo[]
				{
					initiator,
					recipient
				});
				return;
			}
			letterLabel = null;
			letterText = null;
			letterDef = null;
			lookTargets = null;
		}

		// Token: 0x06008458 RID: 33880 RVA: 0x002FA230 File Offset: 0x002F8430
		public static float AcceptanceChance(Pawn initiator, Pawn recipient)
		{
			HistoryEvent ev = new HistoryEvent(initiator.GetHistoryEventForSpouseAndFianceCountPlusOne(), initiator.Named(HistoryEventArgsNames.Doer));
			HistoryEvent ev2 = new HistoryEvent(recipient.GetHistoryEventForSpouseAndFianceCountPlusOne(), recipient.Named(HistoryEventArgsNames.Doer));
			if (!ev.DoerWillingToDo() || !ev2.DoerWillingToDo())
			{
				return 0f;
			}
			if (recipient.Inhumanized())
			{
				return 0f;
			}
			return Mathf.Clamp01(0.9f * Mathf.Clamp01(GenMath.LerpDouble(-20f, 60f, 0f, 1f, (float)recipient.relations.OpinionOf(initiator))));
		}

		// Token: 0x040051BB RID: 20923
		private const float BaseSelectionWeight = 0.4f;

		// Token: 0x040051BC RID: 20924
		private const float BaseAcceptanceChance = 0.9f;

		// Token: 0x040051BD RID: 20925
		private const float BreakupChanceOnRejection = 0.4f;
	}
}
