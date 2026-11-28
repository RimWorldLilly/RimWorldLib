using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015C6 RID: 5574
	public class Pawn_InteractionsTracker : IExposable
	{
		// Token: 0x170017DC RID: 6108
		// (get) Token: 0x06008A2C RID: 35372 RVA: 0x0031FF38 File Offset: 0x0031E138
		public InteractionDef LastInteractionDef
		{
			get
			{
				return this.lastInteractionDef;
			}
		}

		// Token: 0x170017DD RID: 6109
		// (get) Token: 0x06008A2D RID: 35373 RVA: 0x0031FF40 File Offset: 0x0031E140
		private RandomSocialMode CurrentSocialMode
		{
			get
			{
				if (!SocialInteractionUtility.CanInitiateRandomInteraction(this.pawn))
				{
					return RandomSocialMode.Off;
				}
				RandomSocialMode randomSocialMode = RandomSocialMode.Normal;
				JobDriver curDriver = this.pawn.jobs.curDriver;
				if (curDriver != null)
				{
					randomSocialMode = curDriver.DesiredSocialMode();
				}
				PawnDuty duty = this.pawn.mindState.duty;
				if (duty != null && duty.SocialModeMax < randomSocialMode)
				{
					randomSocialMode = duty.SocialModeMax;
				}
				if (this.pawn.Drafted && randomSocialMode > RandomSocialMode.Quiet)
				{
					randomSocialMode = RandomSocialMode.Quiet;
				}
				if (this.pawn.InMentalState && randomSocialMode > this.pawn.MentalState.SocialModeMax())
				{
					randomSocialMode = this.pawn.MentalState.SocialModeMax();
				}
				return randomSocialMode;
			}
		}

		// Token: 0x06008A2E RID: 35374 RVA: 0x0031FFE2 File Offset: 0x0031E1E2
		public Pawn_InteractionsTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06008A2F RID: 35375 RVA: 0x00320014 File Offset: 0x0031E214
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.wantsRandomInteract, "wantsRandomInteract", false, false);
			Scribe_Values.Look<string>(ref this.lastInteraction, "lastInteraction", null, false);
			Scribe_Values.Look<int>(ref this.lastInteractionTime, "lastInteractionTime", -9999, false);
			Scribe_Values.Look<int>(ref this.lastOccultInteraction, "lastOccultInteraction", -9999, false);
			Scribe_Values.Look<int>(ref this.lastRambling, "lastRambling", -9999, false);
			Scribe_Defs.Look<InteractionDef>(ref this.lastInteractionDef, "lastInteractionDef");
		}

		// Token: 0x06008A30 RID: 35376 RVA: 0x00320098 File Offset: 0x0031E298
		public void InteractionsTrackerTickInterval(int delta)
		{
			RandomSocialMode currentSocialMode = this.CurrentSocialMode;
			if (currentSocialMode == RandomSocialMode.Off)
			{
				this.wantsRandomInteract = false;
				return;
			}
			if (currentSocialMode == RandomSocialMode.Quiet)
			{
				this.wantsRandomInteract = false;
			}
			if (!this.wantsRandomInteract)
			{
				if (Find.TickManager.TicksGame > this.lastInteractionTime + 320 && this.pawn.IsHashIntervalTick(60, delta))
				{
					int num = 0;
					switch (currentSocialMode)
					{
					case RandomSocialMode.Quiet:
						num = 22000;
						break;
					case RandomSocialMode.Normal:
						num = 6600;
						break;
					case RandomSocialMode.SuperActive:
						num = 550;
						break;
					}
					if (Rand.MTBEventOccurs((float)num, 1f, 60f) && !this.TryInteractRandomly())
					{
						this.wantsRandomInteract = true;
						return;
					}
				}
			}
			else if (this.pawn.IsHashIntervalTick(91, delta) && this.TryInteractRandomly())
			{
				this.wantsRandomInteract = false;
			}
		}

		// Token: 0x06008A31 RID: 35377 RVA: 0x00320165 File Offset: 0x0031E365
		public bool InteractedTooRecentlyToInteract()
		{
			return Find.TickManager.TicksGame < this.lastInteractionTime + 120;
		}

		// Token: 0x06008A32 RID: 35378 RVA: 0x0032017C File Offset: 0x0031E37C
		public bool CanInteractNowWith(Pawn recipient, InteractionDef interactionDef = null)
		{
			if (this.InteractedTooRecentlyToInteract())
			{
				return false;
			}
			if (!this.pawn.IsCarryingPawn(recipient))
			{
				if (!recipient.Spawned)
				{
					return false;
				}
				if (!SocialInteractionUtility.IsGoodPositionForInteraction(this.pawn, recipient))
				{
					return false;
				}
			}
			return SocialInteractionUtility.CanInitiateInteraction(this.pawn, interactionDef) && SocialInteractionUtility.CanReceiveInteraction(recipient, interactionDef);
		}

		// Token: 0x06008A33 RID: 35379 RVA: 0x003201D8 File Offset: 0x0031E3D8
		public bool TryInteractWith(Pawn recipient, InteractionDef intDef)
		{
			if (DebugSettings.alwaysSocialFight)
			{
				intDef = InteractionDefOf.Insult;
			}
			if (this.pawn == recipient)
			{
				Pawn pawn = this.pawn;
				Log.Warning(((pawn != null) ? pawn.ToString() : null) + " tried to interact with self, interaction=" + intDef.defName);
				return false;
			}
			if (!this.CanInteractNowWith(recipient, intDef))
			{
				return false;
			}
			if (!intDef.ignoreTimeSinceLastInteraction && this.InteractedTooRecentlyToInteract())
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(76, 6);
				defaultInterpolatedStringHandler.AppendFormatted<Pawn>(this.pawn);
				defaultInterpolatedStringHandler.AppendLiteral(" tried to do interaction ");
				defaultInterpolatedStringHandler.AppendFormatted<InteractionDef>(intDef);
				defaultInterpolatedStringHandler.AppendLiteral(" to ");
				defaultInterpolatedStringHandler.AppendFormatted<Pawn>(recipient);
				defaultInterpolatedStringHandler.AppendLiteral(" only ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(Find.TickManager.TicksGame - this.lastInteractionTime);
				defaultInterpolatedStringHandler.AppendLiteral(" ticks since last interaction ");
				defaultInterpolatedStringHandler.AppendFormatted(this.lastInteraction.ToStringSafe<string>());
				defaultInterpolatedStringHandler.AppendLiteral(" (min is ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(120);
				defaultInterpolatedStringHandler.AppendLiteral(").");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return false;
			}
			List<RulePackDef> list = new List<RulePackDef>();
			if (intDef.initiatorThought != null)
			{
				Pawn_InteractionsTracker.AddInteractionThought(this.pawn, recipient, intDef.initiatorThought);
			}
			if (intDef.recipientThought != null && recipient.needs.mood != null)
			{
				Pawn_InteractionsTracker.AddInteractionThought(recipient, this.pawn, intDef.recipientThought);
			}
			if (intDef.initiatorXpGainSkill != null)
			{
				this.pawn.skills.Learn(intDef.initiatorXpGainSkill, (float)intDef.initiatorXpGainAmount, false, false);
			}
			if (intDef.recipientXpGainSkill != null && recipient.RaceProps.Humanlike)
			{
				recipient.skills.Learn(intDef.recipientXpGainSkill, (float)intDef.recipientXpGainAmount, false, false);
			}
			Pawn_IdeoTracker ideo = recipient.ideo;
			if (ideo != null)
			{
				ideo.IncreaseIdeoExposureIfBaby(this.pawn.Ideo, 0.5f);
			}
			bool flag = false;
			if (recipient.RaceProps.Humanlike && recipient.Spawned)
			{
				flag = recipient.interactions.CheckSocialFightStart(intDef, this.pawn);
			}
			string text;
			string str;
			LetterDef letterDef;
			LookTargets lookTargets;
			if (!flag)
			{
				intDef.Worker.Interacted(this.pawn, recipient, list, out text, out str, out letterDef, out lookTargets);
			}
			else
			{
				text = null;
				str = null;
				letterDef = null;
				lookTargets = null;
			}
			MoteMaker.MakeInteractionBubble(this.pawn, recipient, intDef.interactionMote, intDef.GetSymbol(this.pawn.Faction, this.pawn.Ideo), intDef.GetSymbolColor(this.pawn.Faction));
			this.lastInteractionTime = Find.TickManager.TicksGame;
			this.lastInteraction = intDef.defName;
			this.lastInteractionDef = intDef;
			if (recipient.interactions != null)
			{
				recipient.interactions.lastInteractionTime = Find.TickManager.TicksGame;
				recipient.interactions.lastInteraction = intDef.defName;
				recipient.interactions.lastInteractionDef = intDef;
			}
			if (flag)
			{
				list.Add(RulePackDefOf.Sentence_SocialFightStarted);
			}
			PlayLogEntry_Interaction playLogEntry_Interaction = new PlayLogEntry_Interaction(intDef, this.pawn, recipient, list);
			Find.PlayLog.Add(playLogEntry_Interaction);
			if (letterDef != null)
			{
				string text2 = playLogEntry_Interaction.ToGameStringFromPOV(this.pawn, false);
				if (!text.NullOrEmpty())
				{
					text2 = text2 + "\n\n" + text;
				}
				Find.LetterStack.ReceiveLetter(str, text2, letterDef, lookTargets ?? this.pawn, null, null, null, null, 0, true);
			}
			if (ModsConfig.AnomalyActive)
			{
				if (intDef == InteractionDefOf.OccultTeaching)
				{
					Find.ResearchManager.ApplyKnowledge(KnowledgeCategoryDefOf.Basic, 1f);
					this.lastOccultInteraction = GenTicks.TicksAbs;
				}
				this.MetalhorrorDetectionCheck(recipient);
			}
			return true;
		}

		// Token: 0x06008A34 RID: 35380 RVA: 0x00320564 File Offset: 0x0031E764
		private void MetalhorrorDetectionCheck(Pawn recipient)
		{
			bool flag = MetalhorrorUtility.IsInfected(this.pawn);
			Hediff_MetalhorrorImplant firstHediff = recipient.health.hediffSet.GetFirstHediff<Hediff_MetalhorrorImplant>();
			if (!flag && firstHediff != null && !firstHediff.Visible && (Rand.Chance(0.001f) || firstHediff.debugDiscoverNextInteraction))
			{
				TaggedString taggedString = "MetalhorrorReasonInteraction".Translate(this.pawn.Named("PAWN"), recipient.Named("INFECTED"));
				TaggedString value = "MetalhorrorNoticedDetailsAppended".Translate(this.pawn.Named("PAWN"), recipient.Named("INFECTED"));
				TaggedString taggedString2 = "InteractionDetectedDesc".Translate(this.pawn.Named("PAWN"), recipient.Named("INFECTED"));
				TaggedString t = taggedString2;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 1);
				defaultInterpolatedStringHandler.AppendLiteral("\n\n");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(value);
				taggedString2 = t + defaultInterpolatedStringHandler.ToStringAndClear();
				MetalhorrorUtility.Detect(recipient, taggedString, taggedString2, 0.3f);
			}
		}

		// Token: 0x06008A35 RID: 35381 RVA: 0x00320670 File Offset: 0x0031E870
		public static void AddInteractionThought(Pawn pawn, Pawn otherPawn, ThoughtDef thoughtDef)
		{
			if (pawn.needs.mood == null)
			{
				return;
			}
			float statValue = otherPawn.GetStatValue(StatDefOf.SocialImpact, true, -1);
			Thought_Memory thought_Memory = (Thought_Memory)ThoughtMaker.MakeThought(thoughtDef);
			thought_Memory.moodPowerFactor = statValue;
			Thought_MemorySocial thought_MemorySocial = thought_Memory as Thought_MemorySocial;
			if (thought_MemorySocial != null)
			{
				thought_MemorySocial.opinionOffset *= statValue;
			}
			pawn.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, otherPawn);
		}

		// Token: 0x06008A36 RID: 35382 RVA: 0x003206E0 File Offset: 0x0031E8E0
		private bool TryInteractRandomly()
		{
			if (this.InteractedTooRecentlyToInteract())
			{
				return false;
			}
			if (!SocialInteractionUtility.CanInitiateRandomInteraction(this.pawn))
			{
				return false;
			}
			List<Pawn> collection = this.pawn.Map.mapPawns.SpawnedPawnsInFaction(this.pawn.Faction);
			Pawn_InteractionsTracker.workingList.Clear();
			Pawn_InteractionsTracker.workingList.AddRange(collection);
			Pawn_InteractionsTracker.workingList.Shuffle<Pawn>();
			List<InteractionDef> allDefsListForReading = DefDatabase<InteractionDef>.AllDefsListForReading;
			for (int i = 0; i < Pawn_InteractionsTracker.workingList.Count; i++)
			{
				Pawn p = Pawn_InteractionsTracker.workingList[i];
				if (p != this.pawn && this.CanInteractNowWith(p, null) && SocialInteractionUtility.CanReceiveRandomInteraction(p) && !this.pawn.HostileTo(p))
				{
					InteractionDef interactionDef = null;
					if (ModsConfig.AnomalyActive)
					{
						if (this.pawn.story.traits.HasTrait(TraitDefOf.Occultist) && (float)GenTicks.TicksAbs >= (float)this.lastOccultInteraction + 60000f)
						{
							interactionDef = InteractionDefOf.OccultTeaching;
						}
						else if (this.pawn.story.IsDisturbing)
						{
							if (Rand.MTBEventOccurs(1f, 60000f, (float)(GenTicks.TicksGame - this.lastRambling)))
							{
								MentalState_InhumanRambling.DoInhumanRambling(this.pawn);
								this.lastRambling = GenTicks.TicksGame;
								Pawn_InteractionsTracker.workingList.Clear();
								return true;
							}
							interactionDef = InteractionDefOf.DisturbingChat;
						}
					}
					if ((interactionDef == null || this.CanInteractNowWith(p, interactionDef)) && (interactionDef != null || allDefsListForReading.TryRandomElementByWeight(delegate(InteractionDef x)
					{
						if (!this.CanInteractNowWith(p, x))
						{
							return 0f;
						}
						return x.Worker.RandomSelectionWeight(this.pawn, p);
					}, out interactionDef)))
					{
						if (this.TryInteractWith(p, interactionDef))
						{
							Pawn_InteractionsTracker.workingList.Clear();
							return true;
						}
						Pawn pawn = this.pawn;
						string str = (pawn != null) ? pawn.ToString() : null;
						string str2 = " failed to interact with ";
						Pawn p2 = p;
						Log.Error(str + str2 + ((p2 != null) ? p2.ToString() : null));
					}
				}
			}
			Pawn_InteractionsTracker.workingList.Clear();
			return false;
		}

		// Token: 0x06008A37 RID: 35383 RVA: 0x003208F8 File Offset: 0x0031EAF8
		public bool CheckSocialFightStart(InteractionDef interaction, Pawn initiator)
		{
			if (!DebugSettings.enableRandomMentalStates)
			{
				return false;
			}
			if (this.pawn.needs.mood == null || TutorSystem.TutorialMode)
			{
				return false;
			}
			if (DebugSettings.alwaysSocialFight || Rand.Value < this.SocialFightChance(interaction, initiator))
			{
				this.StartSocialFight(initiator, "MessageSocialFight");
				return true;
			}
			return false;
		}

		// Token: 0x06008A38 RID: 35384 RVA: 0x00320950 File Offset: 0x0031EB50
		public void StartSocialFight(Pawn otherPawn, string messageKey = "MessageSocialFight")
		{
			if (PawnUtility.ShouldSendNotificationAbout(this.pawn) || PawnUtility.ShouldSendNotificationAbout(otherPawn))
			{
				Messages.Message(messageKey.Translate(this.pawn.LabelShort, otherPawn.LabelShort, this.pawn.Named("PAWN1"), otherPawn.Named("PAWN2")), this.pawn, MessageTypeDefOf.ThreatSmall, true);
			}
			this.pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.SocialFighting, null, false, false, false, otherPawn, false, false, false);
			otherPawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.SocialFighting, null, false, false, false, this.pawn, false, false, false);
			TaleRecorder.RecordTale(TaleDefOf.SocialFight, new object[]
			{
				this.pawn,
				otherPawn
			});
		}

		// Token: 0x06008A39 RID: 35385 RVA: 0x00320A30 File Offset: 0x0031EC30
		public bool SocialFightPossible(Pawn otherPawn)
		{
			if (!this.pawn.RaceProps.Humanlike || !otherPawn.RaceProps.Humanlike)
			{
				return false;
			}
			if (!SocialInteractionUtility.HasAnyVerbForSocialFight(this.pawn) || !SocialInteractionUtility.HasAnyVerbForSocialFight(otherPawn))
			{
				return false;
			}
			if (this.pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				return false;
			}
			if (otherPawn.Downed || this.pawn.Downed)
			{
				return false;
			}
			if (this.pawn.IsPrisoner && !otherPawn.IsPrisoner)
			{
				return false;
			}
			if (this.pawn.IsSlave && !otherPawn.IsSlave)
			{
				return false;
			}
			DevelopmentalStage developmentalStage = this.pawn.ageTracker.CurLifeStage.developmentalStage;
			return developmentalStage != DevelopmentalStage.Baby && (Mathf.Abs(this.pawn.ageTracker.AgeBiologicalYears - otherPawn.ageTracker.AgeBiologicalYears) <= 6 || developmentalStage != DevelopmentalStage.Child) && (developmentalStage != DevelopmentalStage.Adult || otherPawn.ageTracker.AgeBiologicalYears >= 13) && (this.pawn.genes == null || this.pawn.genes.SocialFightChanceFactor > 0f) && (otherPawn.genes == null || otherPawn.genes.SocialFightChanceFactor > 0f);
		}

		// Token: 0x06008A3A RID: 35386 RVA: 0x00320B68 File Offset: 0x0031ED68
		public float SocialFightChance(InteractionDef interaction, Pawn initiator)
		{
			if (!this.SocialFightPossible(initiator))
			{
				return 0f;
			}
			float num = interaction.socialFightBaseChance;
			num *= Mathf.InverseLerp(0.3f, 1f, this.pawn.health.capacities.GetLevel(PawnCapacityDefOf.Manipulation));
			num *= Mathf.InverseLerp(0.3f, 1f, this.pawn.health.capacities.GetLevel(PawnCapacityDefOf.Moving));
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].CurStage != null)
				{
					num *= hediffs[i].CurStage.socialFightChanceFactor;
				}
			}
			float num2 = (float)this.pawn.relations.OpinionOf(initiator);
			if (num2 < 0f)
			{
				num *= GenMath.LerpDouble(-100f, 0f, 4f, 1f, num2);
			}
			else
			{
				num *= GenMath.LerpDouble(0f, 100f, 1f, 0.6f, num2);
			}
			if (this.pawn.RaceProps.Humanlike)
			{
				List<Trait> allTraits = this.pawn.story.traits.allTraits;
				for (int j = 0; j < allTraits.Count; j++)
				{
					if (!allTraits[j].Suppressed)
					{
						num *= allTraits[j].CurrentData.socialFightChanceFactor;
					}
				}
			}
			int num3 = Mathf.Abs(this.pawn.ageTracker.AgeBiologicalYears - initiator.ageTracker.AgeBiologicalYears);
			if (num3 > 10)
			{
				if (num3 > 50)
				{
					num3 = 50;
				}
				num *= GenMath.LerpDouble(10f, 50f, 1f, 0.25f, (float)num3);
			}
			if (this.pawn.IsSlave)
			{
				num *= 0.5f;
			}
			if (this.pawn.genes != null)
			{
				num *= this.pawn.genes.SocialFightChanceFactor;
			}
			if (initiator.genes != null)
			{
				num *= initiator.genes.SocialFightChanceFactor;
			}
			return Mathf.Clamp01(num);
		}

		// Token: 0x04005525 RID: 21797
		private Pawn pawn;

		// Token: 0x04005526 RID: 21798
		private bool wantsRandomInteract;

		// Token: 0x04005527 RID: 21799
		private int lastInteractionTime = -9999;

		// Token: 0x04005528 RID: 21800
		private string lastInteraction;

		// Token: 0x04005529 RID: 21801
		private InteractionDef lastInteractionDef;

		// Token: 0x0400552A RID: 21802
		private int lastOccultInteraction = -9999;

		// Token: 0x0400552B RID: 21803
		private int lastRambling = -9999;

		// Token: 0x0400552C RID: 21804
		private const int RandomInteractMTBTicks_Quiet = 22000;

		// Token: 0x0400552D RID: 21805
		private const int RandomInteractMTBTicks_Normal = 6600;

		// Token: 0x0400552E RID: 21806
		private const int RandomInteractMTBTicks_SuperActive = 550;

		// Token: 0x0400552F RID: 21807
		public const int RandomInteractIntervalMin = 320;

		// Token: 0x04005530 RID: 21808
		private const int RandomInteractCheckInterval = 60;

		// Token: 0x04005531 RID: 21809
		private const float SlaveSocialFightFactor = 0.5f;

		// Token: 0x04005532 RID: 21810
		private const int ChildSocialFightAgeRange = 6;

		// Token: 0x04005533 RID: 21811
		private const int InteractIntervalAbsoluteMin = 120;

		// Token: 0x04005534 RID: 21812
		public const int DirectTalkInteractInterval = 320;

		// Token: 0x04005535 RID: 21813
		public const float IdeoExposurePointsInteraction = 0.5f;

		// Token: 0x04005536 RID: 21814
		private const float MetalhorrorDetectionChance = 0.001f;

		// Token: 0x04005537 RID: 21815
		private const float MetalhorrorDetectionNoticed = 0.3f;

		// Token: 0x04005538 RID: 21816
		private const float DisturbingInhumanRamblingDaysMTB = 1f;

		// Token: 0x04005539 RID: 21817
		private const float OccultInteractionWait = 60000f;

		// Token: 0x0400553A RID: 21818
		private List<Pawn> tmpPawns;

		// Token: 0x0400553B RID: 21819
		private List<int> tmpTicks;

		// Token: 0x0400553C RID: 21820
		private static readonly List<Pawn> workingList = new List<Pawn>();
	}
}
