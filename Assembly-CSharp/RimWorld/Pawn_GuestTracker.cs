using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001555 RID: 5461
	public class Pawn_GuestTracker : IExposable
	{
		// Token: 0x17001689 RID: 5769
		// (get) Token: 0x0600856A RID: 34154 RVA: 0x0030185B File Offset: 0x002FFA5B
		public Faction HostFaction
		{
			get
			{
				return this.hostFactionInt;
			}
		}

		// Token: 0x1700168A RID: 5770
		// (get) Token: 0x0600856B RID: 34155 RVA: 0x00301863 File Offset: 0x002FFA63
		public Faction SlaveFaction
		{
			get
			{
				return this.slaveFactionInt;
			}
		}

		// Token: 0x1700168B RID: 5771
		// (get) Token: 0x0600856C RID: 34156 RVA: 0x0030186B File Offset: 0x002FFA6B
		public GuestStatus GuestStatus
		{
			get
			{
				return this.guestStatusInt;
			}
		}

		// Token: 0x1700168C RID: 5772
		// (get) Token: 0x0600856D RID: 34157 RVA: 0x00301873 File Offset: 0x002FFA73
		public PrisonerInteractionModeDef ExclusiveInteractionMode
		{
			get
			{
				return this.interactionMode;
			}
		}

		// Token: 0x1700168D RID: 5773
		// (get) Token: 0x0600856E RID: 34158 RVA: 0x0030187B File Offset: 0x002FFA7B
		public bool CanBeBroughtFood
		{
			get
			{
				return this.interactionMode != PrisonerInteractionModeDefOf.Execution && (this.interactionMode != PrisonerInteractionModeDefOf.Release || this.pawn.Downed);
			}
		}

		// Token: 0x1700168E RID: 5774
		// (get) Token: 0x0600856F RID: 34159 RVA: 0x003018A6 File Offset: 0x002FFAA6
		public bool IsPrisoner
		{
			get
			{
				return this.guestStatusInt == GuestStatus.Prisoner;
			}
		}

		// Token: 0x1700168F RID: 5775
		// (get) Token: 0x06008570 RID: 34160 RVA: 0x003018B1 File Offset: 0x002FFAB1
		public bool IsSlave
		{
			get
			{
				return this.guestStatusInt == GuestStatus.Slave;
			}
		}

		// Token: 0x17001690 RID: 5776
		// (get) Token: 0x06008571 RID: 34161 RVA: 0x003018BC File Offset: 0x002FFABC
		public bool ScheduledForInteraction
		{
			get
			{
				return this.pawn.mindState.lastAssignedInteractTime < Find.TickManager.TicksGame - 10000 && this.pawn.mindState.interactionsToday < 2;
			}
		}

		// Token: 0x17001691 RID: 5777
		// (get) Token: 0x06008572 RID: 34162 RVA: 0x003018F5 File Offset: 0x002FFAF5
		// (set) Token: 0x06008573 RID: 34163 RVA: 0x003018FD File Offset: 0x002FFAFD
		public bool Released
		{
			get
			{
				return this.releasedInt;
			}
			set
			{
				if (value == this.releasedInt)
				{
					return;
				}
				this.releasedInt = value;
				ReachabilityUtility.ClearCacheFor(this.pawn);
			}
		}

		// Token: 0x17001692 RID: 5778
		// (get) Token: 0x06008574 RID: 34164 RVA: 0x0030191C File Offset: 0x002FFB1C
		public bool PrisonerIsSecure
		{
			get
			{
				if (this.Released)
				{
					return false;
				}
				if (this.pawn.HostFaction == null)
				{
					return false;
				}
				if (this.pawn.InAggroMentalState)
				{
					return false;
				}
				if (this.pawn.Spawned)
				{
					if (this.pawn.jobs.curJob != null && this.pawn.jobs.curJob.exitMapOnArrival)
					{
						return false;
					}
					if (PrisonBreakUtility.IsPrisonBreaking(this.pawn))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17001693 RID: 5779
		// (get) Token: 0x06008575 RID: 34165 RVA: 0x0030199C File Offset: 0x002FFB9C
		public bool SlaveIsSecure
		{
			get
			{
				if (this.Released)
				{
					return false;
				}
				if (this.pawn.InMentalState)
				{
					return false;
				}
				if (this.pawn.Spawned)
				{
					if (this.pawn.jobs.curJob != null && this.pawn.jobs.curJob.exitMapOnArrival)
					{
						return false;
					}
					if (SlaveRebellionUtility.IsRebelling(this.pawn))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17001694 RID: 5780
		// (get) Token: 0x06008576 RID: 34166 RVA: 0x00301A0C File Offset: 0x002FFC0C
		public bool ShouldWaitInsteadOfEscaping
		{
			get
			{
				if (!this.IsPrisoner)
				{
					return false;
				}
				Map mapHeld = this.pawn.MapHeld;
				return mapHeld != null && mapHeld.mapPawns.FreeColonistsSpawnedCount != 0 && Find.TickManager.TicksGame < this.ticksWhenAllowedToEscapeAgain;
			}
		}

		// Token: 0x17001695 RID: 5781
		// (get) Token: 0x06008577 RID: 34167 RVA: 0x00301A55 File Offset: 0x002FFC55
		public float Resistance
		{
			get
			{
				return this.resistance;
			}
		}

		// Token: 0x17001696 RID: 5782
		// (get) Token: 0x06008578 RID: 34168 RVA: 0x00301A60 File Offset: 0x002FFC60
		// (set) Token: 0x06008579 RID: 34169 RVA: 0x00301B1C File Offset: 0x002FFD1C
		public bool Recruitable
		{
			get
			{
				if (!Find.Storyteller.difficulty.unwaveringPrisoners)
				{
					return true;
				}
				if (PawnUtility.EverBeenColonistOrTameAnimal(this.pawn))
				{
					return true;
				}
				if (this.pawn.Faction != null && this.pawn.Faction == Faction.OfPlayerSilentFail && !this.pawn.IsSlave && !this.pawn.IsPrisoner)
				{
					return true;
				}
				if (this.pawn.IsSubhuman)
				{
					return false;
				}
				if (this.pawn.IsWildMan())
				{
					return true;
				}
				Lord lord = this.pawn.GetLord();
				return (((lord != null) ? lord.LordJob : null) is LordJob_TradeWithColony && this.pawn.GetTraderCaravanRole() == TraderCaravanRole.Chattel) || this.recruitable;
			}
			set
			{
				this.recruitable = value;
			}
		}

		// Token: 0x17001697 RID: 5783
		// (get) Token: 0x0600857A RID: 34170 RVA: 0x00301B25 File Offset: 0x002FFD25
		public bool ScheduledForSlaveSuppression
		{
			get
			{
				return this.pawn.mindState.lastSlaveSuppressedTick < Find.TickManager.TicksGame - 60000;
			}
		}

		// Token: 0x17001698 RID: 5784
		// (get) Token: 0x0600857B RID: 34171 RVA: 0x00301B49 File Offset: 0x002FFD49
		public bool EverEnslaved
		{
			get
			{
				return this.everEnslaved;
			}
		}

		// Token: 0x0600857C RID: 34172 RVA: 0x00301B54 File Offset: 0x002FFD54
		public Pawn_GuestTracker()
		{
		}

		// Token: 0x0600857D RID: 34173 RVA: 0x00301BB8 File Offset: 0x002FFDB8
		public Pawn_GuestTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600857E RID: 34174 RVA: 0x00301C22 File Offset: 0x002FFE22
		public void SetupRecruitable()
		{
			this.recruitable = !Rand.Chance(HealthTuning.NonRecruitableChanceOverPopulationIntentCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntent));
		}

		// Token: 0x0600857F RID: 34175 RVA: 0x00301C44 File Offset: 0x002FFE44
		public void GuestTrackerTickInterval(int delta)
		{
			if (ModsConfig.BiotechActive && this.pawn.IsHashIntervalTick(15000, delta) && SanguophageUtility.CanSafelyBeQueuedForHemogenExtraction(this.pawn) && this.guestStatusInt == GuestStatus.Prisoner && this.IsInteractionEnabled(PrisonerInteractionModeDefOf.HemogenFarm))
			{
				HealthCardUtility.CreateSurgeryBill(this.pawn, RecipeDefOf.ExtractHemogenPack, null, null, false);
			}
			if (this.pawn.IsHashIntervalTick(2500, delta))
			{
				float num = PrisonBreakUtility.InitiatePrisonBreakMtbDays(this.pawn, null, false);
				if (num >= 0f && Rand.MTBEventOccurs(num, 60000f, 2500f))
				{
					PrisonBreakUtility.StartPrisonBreak(this.pawn);
				}
			}
			if (this.pawn.IsHashIntervalTick(2500, delta))
			{
				float num2 = SlaveRebellionUtility.InitiateSlaveRebellionMtbDays(this.pawn);
				if (num2 >= 0f && Rand.MTBEventOccurs(num2, 60000f, 2500f))
				{
					SlaveRebellionUtility.StartSlaveRebellion(this.pawn, false);
				}
			}
		}

		// Token: 0x06008580 RID: 34176 RVA: 0x00301D2E File Offset: 0x002FFF2E
		public void RandomizeJoinStatus()
		{
			if (ModsConfig.IdeologyActive)
			{
				this.joinStatus = ((!this.Recruitable || Rand.Value < 0.75f) ? JoinStatus.JoinAsSlave : JoinStatus.JoinAsColonist);
				return;
			}
			this.joinStatus = JoinStatus.JoinAsColonist;
		}

		// Token: 0x06008581 RID: 34177 RVA: 0x00301D60 File Offset: 0x002FFF60
		public void SetExclusiveInteraction(PrisonerInteractionModeDef def)
		{
			if (def.isNonExclusiveInteraction)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(130, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Attempted to set guest exclusive interaction to a non-exclusive interaction type: ");
				defaultInterpolatedStringHandler.AppendFormatted(def.defName);
				defaultInterpolatedStringHandler.AppendLiteral(", use ");
				defaultInterpolatedStringHandler.AppendFormatted("ToggleNonExclusiveInteraction");
				defaultInterpolatedStringHandler.AppendLiteral(" to change non-exclusive interaction type.");
				Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), 746345623);
				return;
			}
			this.interactionMode = def;
		}

		// Token: 0x06008582 RID: 34178 RVA: 0x00301DD8 File Offset: 0x002FFFD8
		public void ToggleNonExclusiveInteraction(PrisonerInteractionModeDef def, bool enabled)
		{
			if (!def.isNonExclusiveInteraction)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(115, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Attempted to ");
				defaultInterpolatedStringHandler.AppendFormatted(enabled ? "enable" : "disable");
				defaultInterpolatedStringHandler.AppendLiteral(" an exclusive interaction type (");
				defaultInterpolatedStringHandler.AppendFormatted(def.defName);
				defaultInterpolatedStringHandler.AppendLiteral(") via non-exclusive method, use ");
				defaultInterpolatedStringHandler.AppendFormatted("SetExclusiveInteraction");
				defaultInterpolatedStringHandler.AppendLiteral(" to change exclusive interaction type.");
				Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), 4635128);
				return;
			}
			if (enabled && !this.enabledNonExclusiveInteractions.Contains(def))
			{
				this.enabledNonExclusiveInteractions.Add(def);
				return;
			}
			if (!enabled && this.enabledNonExclusiveInteractions.Contains(def))
			{
				this.enabledNonExclusiveInteractions.Remove(def);
			}
		}

		// Token: 0x06008583 RID: 34179 RVA: 0x00301EA4 File Offset: 0x003000A4
		public bool IsInteractionEnabled(PrisonerInteractionModeDef def)
		{
			return this.interactionMode == def || this.enabledNonExclusiveInteractions.Contains(def);
		}

		// Token: 0x06008584 RID: 34180 RVA: 0x00301EC0 File Offset: 0x003000C0
		public bool HasInteractionWith(Func<PrisonerInteractionModeDef, bool> validator)
		{
			if (validator(this.interactionMode))
			{
				return true;
			}
			foreach (PrisonerInteractionModeDef arg in this.enabledNonExclusiveInteractions)
			{
				if (validator(arg))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008585 RID: 34181 RVA: 0x00301F2C File Offset: 0x0030012C
		public bool IsInteractionDisabled(PrisonerInteractionModeDef def)
		{
			return !this.IsInteractionEnabled(def);
		}

		// Token: 0x06008586 RID: 34182 RVA: 0x00301F38 File Offset: 0x00300138
		public void SetNoInteraction()
		{
			this.SetExclusiveInteraction(PrisonerInteractionModeDefOf.MaintainOnly);
			this.enabledNonExclusiveInteractions.Clear();
		}

		// Token: 0x06008587 RID: 34183 RVA: 0x00301F50 File Offset: 0x00300150
		public void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.hostFactionInt, "hostFaction", false);
			Scribe_References.Look<Faction>(ref this.slaveFactionInt, "slaveFaction", false);
			Scribe_Values.Look<GuestStatus>(ref this.guestStatusInt, "guestStatus", GuestStatus.Guest, false);
			Scribe_Values.Look<JoinStatus>(ref this.joinStatus, "joinStatus", JoinStatus.Undefined, false);
			Scribe_Defs.Look<PrisonerInteractionModeDef>(ref this.interactionMode, "interactionMode");
			Scribe_Defs.Look<SlaveInteractionModeDef>(ref this.slaveInteractionMode, "slaveInteractionMode");
			Scribe_Values.Look<bool>(ref this.releasedInt, "released", false, false);
			Scribe_Values.Look<int>(ref this.ticksWhenAllowedToEscapeAgain, "ticksWhenAllowedToEscapeAgain", 0, false);
			Scribe_Values.Look<IntVec3>(ref this.spotToWaitInsteadOfEscaping, "spotToWaitInsteadOfEscaping", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.lastPrisonBreakTicks, "lastPrisonBreakTicks", 0, false);
			Scribe_Values.Look<bool>(ref this.everParticipatedInPrisonBreak, "everParticipatedInPrisonBreak", false, false);
			Scribe_Values.Look<bool>(ref this.getRescuedThoughtOnUndownedBecauseOfPlayer, "getRescuedThoughtOnUndownedBecauseOfPlayer", false, false);
			Scribe_Values.Look<float>(ref this.resistance, "resistance", -1f, false);
			Scribe_Values.Look<float>(ref this.will, "will", -1f, false);
			Scribe_Values.Look<int>(ref this.lastRecruiterOpinion, "lastRecruiterOpinion", 0, false);
			Scribe_Values.Look<string>(ref this.lastRecruiterName, "lastRecruiterName", null, false);
			Scribe_Values.Look<bool>(ref this.hasOpinionOfLastRecruiter, "hasOpinionOfLastRecruiter", false, false);
			Scribe_Values.Look<bool>(ref this.everEnslaved, "everEnslaved", false, false);
			Scribe_References.Look<Ideo>(ref this.ideoForConversion, "ideoForConversion", false);
			Scribe_Values.Look<bool>(ref this.recruitable, "recruitable", true, false);
			Scribe_Values.Look<bool>(ref this.leftAfterRescue, "leftAfterRescue", false, false);
			Scribe_Collections.Look<PrisonerInteractionModeDef>(ref this.enabledNonExclusiveInteractions, "enabledNonExclusiveInteractions", LookMode.Def, Array.Empty<object>());
			Scribe_Deep.Look<ResistanceInteractionData>(ref this.lastResistanceInteractionData, "lastResistanceInteractionData", Array.Empty<object>());
			Scribe_Deep.Look<ResistanceInteractionData>(ref this.finalResistanceInteractionData, "finalResistanceInteractionData", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.interactionMode == null || (!this.interactionMode.allowOnWildMan && this.pawn.IsWildMan()))
				{
					this.interactionMode = PrisonerInteractionModeDefOf.MaintainOnly;
				}
				if (this.joinStatus == JoinStatus.Undefined)
				{
					this.RandomizeJoinStatus();
				}
				if (this.enabledNonExclusiveInteractions == null)
				{
					this.enabledNonExclusiveInteractions = new List<PrisonerInteractionModeDef>();
				}
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06008588 RID: 34184 RVA: 0x0030217C File Offset: 0x0030037C
		public void SetRecruitmentData(Pawn recruiter)
		{
			this.lastRecruiterName = recruiter.LabelShort;
			this.hasOpinionOfLastRecruiter = (this.pawn.relations != null);
			this.lastRecruiterOpinion = (this.hasOpinionOfLastRecruiter ? this.pawn.relations.OpinionOf(recruiter) : 0);
			this.finalResistanceInteractionData = this.lastResistanceInteractionData;
			this.lastResistanceInteractionData = null;
		}

		// Token: 0x06008589 RID: 34185 RVA: 0x003021DE File Offset: 0x003003DE
		public void SetLastResistanceReduceData(Pawn initiator, float resistanceReduce, float negotiationFactor, float moodFactor, float opinionFactor)
		{
			this.lastResistanceInteractionData = new ResistanceInteractionData
			{
				initiatorNegotiationAbilityFactor = negotiationFactor,
				recruiteeMoodFactor = moodFactor,
				resistanceReduction = resistanceReduce,
				initiatorName = initiator.LabelShort,
				recruiterOpinionFactor = opinionFactor
			};
		}

		// Token: 0x0600858A RID: 34186 RVA: 0x00302218 File Offset: 0x00300418
		public void Notify_WardensOfIdeoLost(Ideo ideo)
		{
			if (!ModsConfig.IdeologyActive || this.interactionMode != PrisonerInteractionModeDefOf.Convert || this.ideoForConversion == null || this.ideoForConversion != ideo)
			{
				return;
			}
			this.interactionMode = PrisonerInteractionModeDefOf.MaintainOnly;
			this.ideoForConversion = null;
			Messages.Message("MessageNoWardenOfIdeo".Translate(this.pawn.Named("PRISONER"), ideo.memberName.Named("MEMBERNAME")), new LookTargets(this.pawn), MessageTypeDefOf.NeutralEvent, false);
		}

		// Token: 0x0600858B RID: 34187 RVA: 0x003022A4 File Offset: 0x003004A4
		public void SetGuestStatus(Faction newHost, GuestStatus guestStatus = GuestStatus.Guest)
		{
			if (newHost != null)
			{
				this.Released = false;
			}
			switch (guestStatus)
			{
			case GuestStatus.Guest:
				if (this.pawn.Faction.HostileTo(newHost))
				{
					string[] array = new string[8];
					array[0] = "Tried to make ";
					int num = 1;
					Pawn pawn = this.pawn;
					array[num] = ((pawn != null) ? pawn.ToString() : null);
					array[2] = " a guest of ";
					array[3] = ((newHost != null) ? newHost.ToString() : null);
					array[4] = " but their faction ";
					int num2 = 5;
					Faction faction = this.pawn.Faction;
					array[num2] = ((faction != null) ? faction.ToString() : null);
					array[6] = " is hostile to ";
					array[7] = ((newHost != null) ? newHost.ToString() : null);
					Log.Error(string.Concat(array));
					return;
				}
				if (newHost != null && newHost == this.pawn.Faction)
				{
					string str = "Tried to make ";
					Pawn pawn2 = this.pawn;
					string str2 = (pawn2 != null) ? pawn2.ToString() : null;
					string str3 = " a guest of their own faction ";
					Faction faction2 = this.pawn.Faction;
					Log.Error(str + str2 + str3 + ((faction2 != null) ? faction2.ToString() : null));
					return;
				}
				if (newHost == null)
				{
					this.slaveFactionInt = null;
				}
				break;
			case GuestStatus.Prisoner:
			{
				if (newHost == this.HostFaction && this.IsPrisoner)
				{
					return;
				}
				this.pawn.workSettings.EnableAndInitializeIfNotAlreadyInitialized();
				Pawn pawn3 = this.pawn;
				MentalStateDef mentalStateDef = this.pawn.MentalStateDef;
				bool clearMentalState = mentalStateDef == null || mentalStateDef.recoverFromCaptured;
				pawn3.ClearMind_NewTemp(newHost != null, false, clearMentalState, false);
				this.pawn.DropAndForbidEverything(false, false);
				Lord lord = this.pawn.GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(this.pawn, PawnLostCondition.MadePrisoner, null);
				}
				if (this.pawn.Drafted)
				{
					this.pawn.drafter.Drafted = false;
				}
				float num3 = this.pawn.kindDef.initialResistanceRange.Value.RandomInRange;
				Pawn_RoyaltyTracker royalty = this.pawn.royalty;
				RoyalTitle royalTitle = (royalty != null) ? royalty.MostSeniorTitle : null;
				if (royalTitle != null)
				{
					num3 += royalTitle.def.recruitmentResistanceOffset;
				}
				this.resistance = (float)GenMath.RoundRandom(num3);
				this.will = (this.everEnslaved ? 2.5f : this.pawn.kindDef.initialWillRange.Value.RandomInRange);
				if (ModsConfig.IdeologyActive && this.interactionMode == PrisonerInteractionModeDefOf.Enslave)
				{
					this.interactionMode = PrisonerInteractionModeDefOf.MaintainOnly;
				}
				if (this.guestStatusInt == GuestStatus.Slave && this.slaveFactionInt != null)
				{
					this.pawn.SetFaction(this.slaveFactionInt, null);
				}
				break;
			}
			case GuestStatus.Slave:
			{
				if (newHost == this.pawn.Faction && this.IsSlave)
				{
					return;
				}
				Lord lord2 = this.pawn.GetLord();
				if (lord2 != null)
				{
					lord2.Notify_PawnLost(this.pawn, PawnLostCondition.MadeSlave, null);
				}
				Faction faction3 = this.pawn.Faction;
				if (this.pawn.Faction != newHost)
				{
					this.pawn.SetFaction(newHost, null);
				}
				if (this.slaveFactionInt == null)
				{
					this.slaveFactionInt = faction3;
				}
				if (!this.everEnslaved || this.slaveInteractionMode == SlaveInteractionModeDefOf.Imprison || this.slaveInteractionMode == SlaveInteractionModeDefOf.Emancipate)
				{
					this.slaveInteractionMode = SlaveInteractionModeDefOf.Suppress;
				}
				this.everEnslaved = true;
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unknown GuestStatus type ");
				defaultInterpolatedStringHandler.AppendFormatted<GuestStatus>(guestStatus);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			}
			this.guestStatusInt = guestStatus;
			Faction faction4 = this.hostFactionInt;
			this.hostFactionInt = ((guestStatus != GuestStatus.Slave) ? newHost : null);
			this.pawn.Notify_DisabledWorkTypesChanged();
			PawnComponentsUtility.AddAndRemoveDynamicComponents(this.pawn, false);
			this.pawn.health.surgeryBills.Clear();
			Pawn_Ownership ownership = this.pawn.ownership;
			if (ownership != null)
			{
				ownership.Notify_ChangedGuestStatus();
			}
			Ideo ideo = this.pawn.Ideo;
			if (ideo != null)
			{
				ideo.Notify_MemberGuestStatusChanged(this.pawn);
			}
			if (ModsConfig.BiotechActive && this.pawn.mechanitor != null)
			{
				this.pawn.mechanitor.Notify_ChangedGuestStatus();
			}
			ReachabilityUtility.ClearCacheFor(this.pawn);
			if (this.pawn.Spawned)
			{
				this.pawn.Map.mapPawns.UpdateRegistryForPawn(this.pawn);
				this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
			}
			AddictionUtility.CheckDrugAddictionTeachOpportunity(this.pawn);
			if ((guestStatus == GuestStatus.Prisoner || guestStatus == GuestStatus.Slave) && this.pawn.playerSettings != null)
			{
				this.pawn.playerSettings.ResetMedicalCare();
			}
			if (faction4 != this.hostFactionInt)
			{
				QuestUtility.SendQuestTargetSignals(this.pawn.questTags, "ChangedHostFaction", this.pawn.Named("SUBJECT"), this.hostFactionInt.Named("FACTION"));
			}
			if ((this.pawn.Inhumanized() || this.pawn.kindDef.studiableAsPrisoner) && this.pawn.MapHeld != null)
			{
				Find.StudyManager.UpdateStudiableCache(this.pawn, this.pawn.MapHeld);
			}
		}

		// Token: 0x0600858C RID: 34188 RVA: 0x003027B0 File Offset: 0x003009B0
		public void CapturedBy(Faction by, Pawn byPawn = null)
		{
			Faction homeFaction = this.pawn.HomeFaction;
			if (homeFaction != null)
			{
				homeFaction.Notify_MemberCaptured(this.pawn, by);
			}
			this.SetGuestStatus(by, GuestStatus.Prisoner);
			if (this.IsPrisoner && byPawn != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.Captured, new object[]
				{
					byPawn,
					this.pawn
				});
				byPawn.records.Increment(RecordDefOf.PeopleCaptured);
			}
			if (by == Faction.OfPlayer && !this.Recruitable && !Find.History.everCapturedUnrecruitablePawn)
			{
				Find.History.everCapturedUnrecruitablePawn = true;
				TaggedString taggedString = ModsConfig.IdeologyActive ? ("OptionalEnslaveDesc".Translate(this.pawn.Named("PAWN")) + ", ") : TaggedString.Empty;
				Find.LetterStack.ReceiveLetter("LetterLabelUnrecruitablePawnCaptured".Translate(), "LetterTextUnrecruitablePawnCaptured".Translate(this.pawn.Named("PAWN"), taggedString.Named("OPTIONALENSLAVEDESC")), LetterDefOf.NeutralEvent, this.pawn, null, null, null, null, 0, true);
			}
		}

		// Token: 0x0600858D RID: 34189 RVA: 0x003028D2 File Offset: 0x00300AD2
		public void WaitInsteadOfEscapingForDefaultTicks()
		{
			this.WaitInsteadOfEscapingFor(25000);
		}

		// Token: 0x0600858E RID: 34190 RVA: 0x003028DF File Offset: 0x00300ADF
		public void WaitInsteadOfEscapingFor(int ticks)
		{
			if (!this.IsPrisoner)
			{
				return;
			}
			this.ticksWhenAllowedToEscapeAgain = Find.TickManager.TicksGame + ticks;
			this.spotToWaitInsteadOfEscaping = IntVec3.Invalid;
		}

		// Token: 0x0600858F RID: 34191 RVA: 0x00302907 File Offset: 0x00300B07
		public Texture2D GetIcon()
		{
			return GuestUtility.GetGuestIcon(this.guestStatusInt);
		}

		// Token: 0x06008590 RID: 34192 RVA: 0x00302914 File Offset: 0x00300B14
		public string GetLabel()
		{
			if (this.IsSlave)
			{
				return "Slave".Translate().CapitalizeFirst();
			}
			return null;
		}

		// Token: 0x06008591 RID: 34193 RVA: 0x00302944 File Offset: 0x00300B44
		internal void Notify_PawnUndowned()
		{
			if (this.pawn.RaceProps.Humanlike && (this.HostFaction.IsPlayerSafe() || (this.pawn.IsWildMan() && this.pawn.InBed() && this.pawn.CurrentBed().Faction == Faction.OfPlayer)) && !this.IsPrisoner && this.pawn.SpawnedOrAnyParentSpawned && !this.leftAfterRescue)
			{
				if (this.getRescuedThoughtOnUndownedBecauseOfPlayer)
				{
					Pawn_NeedsTracker needs = this.pawn.needs;
					if (((needs != null) ? needs.mood : null) != null)
					{
						this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.Rescued, null, null);
					}
				}
				if (this.pawn.Faction == null || this.pawn.Faction.def.rescueesCanJoin)
				{
					Map mapHeld = this.pawn.MapHeld;
					float num;
					if (!this.pawn.SafeTemperatureRange().Includes(mapHeld.mapTemperature.OutdoorTemp) || mapHeld.gameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout) || (ModsConfig.BiotechActive && mapHeld.gameConditionManager.ConditionIsActive(GameConditionDefOf.NoxiousHaze)))
					{
						num = 1f;
					}
					else
					{
						num = 0.5f;
					}
					if (Rand.ValueSeeded(this.pawn.thingIDNumber ^ 8976612) < num)
					{
						this.pawn.SetFaction(Faction.OfPlayer, null);
						Find.LetterStack.ReceiveLetter("LetterLabelRescueeJoins".Translate(this.pawn.Named("PAWN")), "LetterRescueeJoins".Translate(this.pawn.Named("PAWN")), LetterDefOf.PositiveEvent, this.pawn, null, null, null, null, 0, true);
					}
					else
					{
						Messages.Message("MessageRescueeDidntJoin".Translate().AdjustedFor(this.pawn, "PAWN", true), this.pawn, MessageTypeDefOf.NeutralEvent, true);
						this.leftAfterRescue = true;
					}
				}
			}
			this.getRescuedThoughtOnUndownedBecauseOfPlayer = false;
		}

		// Token: 0x06008592 RID: 34194 RVA: 0x00302B6D File Offset: 0x00300D6D
		public void Notify_PawnRecruited()
		{
			this.slaveFactionInt = null;
		}

		// Token: 0x0400521E RID: 21022
		private Pawn pawn;

		// Token: 0x0400521F RID: 21023
		private PrisonerInteractionModeDef interactionMode = PrisonerInteractionModeDefOf.MaintainOnly;

		// Token: 0x04005220 RID: 21024
		private List<PrisonerInteractionModeDef> enabledNonExclusiveInteractions = new List<PrisonerInteractionModeDef>();

		// Token: 0x04005221 RID: 21025
		public SlaveInteractionModeDef slaveInteractionMode = SlaveInteractionModeDefOf.NoInteraction;

		// Token: 0x04005222 RID: 21026
		private Faction hostFactionInt;

		// Token: 0x04005223 RID: 21027
		public GuestStatus guestStatusInt;

		// Token: 0x04005224 RID: 21028
		public JoinStatus joinStatus;

		// Token: 0x04005225 RID: 21029
		private Faction slaveFactionInt;

		// Token: 0x04005226 RID: 21030
		public string lastRecruiterName;

		// Token: 0x04005227 RID: 21031
		public int lastRecruiterOpinion;

		// Token: 0x04005228 RID: 21032
		public bool hasOpinionOfLastRecruiter;

		// Token: 0x04005229 RID: 21033
		public ResistanceInteractionData lastResistanceInteractionData;

		// Token: 0x0400522A RID: 21034
		public ResistanceInteractionData finalResistanceInteractionData;

		// Token: 0x0400522B RID: 21035
		private bool releasedInt;

		// Token: 0x0400522C RID: 21036
		private int ticksWhenAllowedToEscapeAgain;

		// Token: 0x0400522D RID: 21037
		public IntVec3 spotToWaitInsteadOfEscaping = IntVec3.Invalid;

		// Token: 0x0400522E RID: 21038
		public int lastPrisonBreakTicks = -1;

		// Token: 0x0400522F RID: 21039
		public bool everParticipatedInPrisonBreak;

		// Token: 0x04005230 RID: 21040
		public float resistance = -1f;

		// Token: 0x04005231 RID: 21041
		public float will = -1f;

		// Token: 0x04005232 RID: 21042
		public Ideo ideoForConversion;

		// Token: 0x04005233 RID: 21043
		private bool recruitable = true;

		// Token: 0x04005234 RID: 21044
		private bool everEnslaved;

		// Token: 0x04005235 RID: 21045
		public bool getRescuedThoughtOnUndownedBecauseOfPlayer;

		// Token: 0x04005236 RID: 21046
		public bool leftAfterRescue;

		// Token: 0x04005237 RID: 21047
		private const int DefaultWaitInsteadOfEscapingTicks = 25000;

		// Token: 0x04005238 RID: 21048
		public const int MinInteractionInterval = 10000;

		// Token: 0x04005239 RID: 21049
		public const int MaxInteractionsPerDay = 2;

		// Token: 0x0400523A RID: 21050
		private const int CheckInitiatePrisonBreakIntervalTicks = 2500;

		// Token: 0x0400523B RID: 21051
		private const int CheckInitiateSlaveRebellionIntervalTicks = 2500;

		// Token: 0x0400523C RID: 21052
		private const int MinSlaveSuppressionIntervalTicks = 60000;

		// Token: 0x0400523D RID: 21053
		public const float DefaultWillIfPreviouslyEnslaved = 2.5f;

		// Token: 0x0400523E RID: 21054
		private const int CheckHemogenBillInterval = 15000;
	}
}
