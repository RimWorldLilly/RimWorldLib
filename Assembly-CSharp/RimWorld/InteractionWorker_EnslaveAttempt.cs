using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001518 RID: 5400
	public class InteractionWorker_EnslaveAttempt : InteractionWorker
	{
		// Token: 0x0600844B RID: 33867 RVA: 0x002F96E8 File Offset: 0x002F78E8
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			letterText = null;
			letterLabel = null;
			letterDef = null;
			lookTargets = null;
			bool flag = initiator.InspirationDef == InspirationDefOf.Inspired_Recruitment && recipient.guest.IsInteractionEnabled(PrisonerInteractionModeDefOf.Enslave);
			if (recipient.guest.will > 0f && !flag)
			{
				float num = 1f;
				num *= initiator.GetStatValue(StatDefOf.NegotiationAbility, true, -1);
				num = Mathf.Min(num, recipient.guest.will);
				float will = recipient.guest.will;
				recipient.guest.will = Mathf.Max(0f, recipient.guest.will - num);
				float will2 = recipient.guest.will;
				string text = "TextMote_WillReduced".Translate(will.ToString("F1"), recipient.guest.will.ToString("F1"));
				if (recipient.needs.mood != null && recipient.needs.mood.CurLevelPercentage < 0.4f)
				{
					text += "\n(" + "lowMood".Translate() + ")";
				}
				MoteMaker.ThrowText((initiator.DrawPos + recipient.DrawPos) / 2f, initiator.Map, text, 8f);
				if (recipient.guest.will == 0f)
				{
					TaggedString taggedString = "MessagePrisonerWillBroken".Translate(initiator, recipient);
					if (recipient.guest.IsInteractionEnabled(PrisonerInteractionModeDefOf.AttemptRecruit))
					{
						taggedString += " " + "MessagePrisonerWillBroken_RecruitAttempsWillBegin".Translate();
					}
					Messages.Message(taggedString, recipient, MessageTypeDefOf.PositiveEvent, true);
					return;
				}
			}
			else if (recipient.guest.IsInteractionDisabled(PrisonerInteractionModeDefOf.ReduceWill))
			{
				QuestUtility.SendQuestTargetSignals(recipient.questTags, "Enslaved", recipient.Named("SUBJECT"));
				if (!GenGuest.TryEnslavePrisoner(initiator, recipient))
				{
					return;
				}
				if (!letterLabel.NullOrEmpty())
				{
					letterDef = LetterDefOf.PositiveEvent;
				}
				letterLabel = "LetterLabelEnslavementSuccess".Translate() + ": " + recipient.LabelCap;
				letterText = "LetterEnslavementSuccess".Translate(initiator, recipient);
				letterDef = LetterDefOf.PositiveEvent;
				lookTargets = new LookTargets(new TargetInfo[]
				{
					recipient,
					initiator
				});
				if (flag)
				{
					initiator.mindState.inspirationHandler.EndInspiration(InspirationDefOf.Inspired_Recruitment);
				}
				extraSentencePacks.Add(RulePackDefOf.Sentence_RecruitAttemptAccepted);
			}
		}

		// Token: 0x040051B2 RID: 20914
		private const float BaseWillReductionPerInteraction = 1f;

		// Token: 0x040051B3 RID: 20915
		private const float MaxMoodForWarning = 0.4f;
	}
}
