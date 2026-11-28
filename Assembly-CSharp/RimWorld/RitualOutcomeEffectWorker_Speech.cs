using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016E0 RID: 5856
	public class RitualOutcomeEffectWorker_Speech : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x1700192C RID: 6444
		// (get) Token: 0x0600928E RID: 37518 RVA: 0x0035853E File Offset: 0x0035673E
		public override bool SupportsAttachableOutcomeEffect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600928F RID: 37519 RVA: 0x00358541 File Offset: 0x00356741
		public RitualOutcomeEffectWorker_Speech()
		{
		}

		// Token: 0x06009290 RID: 37520 RVA: 0x00358549 File Offset: 0x00356749
		public RitualOutcomeEffectWorker_Speech(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x06009291 RID: 37521 RVA: 0x00358554 File Offset: 0x00356754
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			Pawn organizer = jobRitual.Organizer;
			float quality = base.GetQuality(jobRitual, progress);
			RitualOutcomePossibility outcome = this.GetOutcome(quality, jobRitual);
			ThoughtDef memory = outcome.memory;
			LookTargets lookTargets = organizer;
			string text = null;
			if (jobRitual.Ritual != null)
			{
				this.ApplyAttachableOutcome(totalPresence, jobRitual, outcome, out text, ref lookTargets);
			}
			string text2 = "";
			string text3 = "";
			foreach (KeyValuePair<Pawn, int> keyValuePair in totalPresence)
			{
				Pawn key = keyValuePair.Key;
				if (key != organizer && organizer.Position.InHorDistOf(key.Position, 18f))
				{
					Thought_Memory thought_Memory = base.MakeMemory(key, jobRitual, memory);
					thought_Memory.otherPawn = organizer;
					thought_Memory.moodPowerFactor = ((key.Ideo == organizer.Ideo) ? 1f : 0.5f);
					key.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, null);
					if (memory == ThoughtDefOf.InspirationalSpeech)
					{
						if (Rand.Chance(RitualOutcomeEffectWorker_Speech.InspirationChanceFromInspirationalSpeech))
						{
							InspirationDef randomAvailableInspirationDef = key.mindState.inspirationHandler.GetRandomAvailableInspirationDef();
							if (randomAvailableInspirationDef != null && key.mindState.inspirationHandler.TryStartInspiration(randomAvailableInspirationDef, "LetterSpeechInspiration".Translate(key.Named("PAWN"), organizer.Named("SPEAKER")), true))
							{
								text2 = text2 + "  - " + key.NameShortColored.Resolve() + "\n";
							}
						}
						if (ModsConfig.IdeologyActive && key.Ideo != organizer.Ideo && Rand.Chance(RitualOutcomeEffectWorker_Speech.ConversionChanceFromInspirationalSpeech))
						{
							key.ideo.SetIdeo(organizer.Ideo);
							text3 = text3 + "  - " + key.NameShortColored.Resolve() + "\n";
						}
					}
				}
			}
			TaggedString taggedString = "LetterFinishedSpeech".Translate(organizer.Named("ORGANIZER")).CapitalizeFirst() + " " + ("Letter" + memory.defName).Translate() + "\n\n" + this.OutcomeQualityBreakdownDesc(quality, progress, jobRitual);
			if (!text3.NullOrEmpty())
			{
				taggedString += "\n\n" + "LetterSpeechConvertedListeners".Translate(organizer.Named("PAWN"), organizer.Ideo.Named("IDEO")).CapitalizeFirst() + ":\n\n" + text3.TrimEndNewlines();
			}
			if (!text2.NullOrEmpty())
			{
				taggedString += "\n\n" + "LetterSpeechInspiredListeners".Translate() + "\n\n" + text2.TrimEndNewlines();
			}
			if (progress < 1f)
			{
				taggedString += "\n\n" + "LetterSpeechInterrupted".Translate(progress.ToStringPercent(), organizer.Named("ORGANIZER"));
			}
			if (text != null)
			{
				taggedString += "\n\n" + text;
			}
			string text4;
			this.ApplyDevelopmentPoints(jobRitual.Ritual, outcome, out text4);
			if (text4 != null)
			{
				taggedString += "\n\n" + text4;
			}
			Find.LetterStack.ReceiveLetter("OutcomeLetterLabel".Translate(outcome.label.Named("OUTCOMELABEL"), jobRitual.Ritual.Label.Named("RITUALLABEL")), taggedString, RitualOutcomeEffectWorker_Speech.PositiveOutcome(memory) ? LetterDefOf.RitualOutcomePositive : LetterDefOf.RitualOutcomeNegative, lookTargets, null, null, null, null, 0, true);
			if (jobRitual.Ritual.def == PreceptDefOf.ThroneSpeech)
			{
				Ability ability = organizer.abilities.GetAbility(AbilityDefOf.Speech, true);
				RoyalTitle mostSeniorTitle = organizer.royalty.MostSeniorTitle;
				if (ability != null && mostSeniorTitle != null)
				{
					ability.StartCooldown(mostSeniorTitle.def.speechCooldown.RandomInRange);
				}
			}
		}

		// Token: 0x06009292 RID: 37522 RVA: 0x00358984 File Offset: 0x00356B84
		private static bool PositiveOutcome(ThoughtDef outcome)
		{
			return outcome == ThoughtDefOf.EncouragingSpeech || outcome == ThoughtDefOf.InspirationalSpeech;
		}

		// Token: 0x040058C4 RID: 22724
		private static readonly float InspirationChanceFromInspirationalSpeech = 0.05f;

		// Token: 0x040058C5 RID: 22725
		private static readonly float ConversionChanceFromInspirationalSpeech = 0.02f;
	}
}
