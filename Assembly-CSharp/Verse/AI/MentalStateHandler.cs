using System;
using RimWorld;
using RimWorld.Planet;
using Verse.AI.Group;

namespace Verse.AI
{
	// Token: 0x0200085F RID: 2143
	public class MentalStateHandler : IExposable
	{
		// Token: 0x17000CC7 RID: 3271
		// (get) Token: 0x0600433B RID: 17211 RVA: 0x001A84C1 File Offset: 0x001A66C1
		public bool InMentalState
		{
			get
			{
				return this.curStateInt != null;
			}
		}

		// Token: 0x17000CC8 RID: 3272
		// (get) Token: 0x0600433C RID: 17212 RVA: 0x001A84CC File Offset: 0x001A66CC
		public MentalStateDef CurStateDef
		{
			get
			{
				if (this.curStateInt == null)
				{
					return null;
				}
				return this.curStateInt.def;
			}
		}

		// Token: 0x17000CC9 RID: 3273
		// (get) Token: 0x0600433D RID: 17213 RVA: 0x001A84E3 File Offset: 0x001A66E3
		public MentalState CurState
		{
			get
			{
				return this.curStateInt;
			}
		}

		// Token: 0x0600433E RID: 17214 RVA: 0x001A84EB File Offset: 0x001A66EB
		public MentalStateHandler()
		{
		}

		// Token: 0x0600433F RID: 17215 RVA: 0x001A84F3 File Offset: 0x001A66F3
		public MentalStateHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06004340 RID: 17216 RVA: 0x001A8504 File Offset: 0x001A6704
		public void ExposeData()
		{
			Scribe_Deep.Look<MentalState>(ref this.curStateInt, "curState", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.neverFleeIndividual, "neverFleeIndividual", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.curStateInt != null)
				{
					this.curStateInt.pawn = this.pawn;
				}
				if (Current.ProgramState != ProgramState.Entry && this.pawn.Spawned)
				{
					this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
				}
			}
		}

		// Token: 0x06004341 RID: 17217 RVA: 0x001A8588 File Offset: 0x001A6788
		public void Reset()
		{
			this.ClearMentalStateDirect();
		}

		// Token: 0x06004342 RID: 17218 RVA: 0x001A8590 File Offset: 0x001A6790
		public void MentalStateHandlerTickInterval(int delta)
		{
			if (this.curStateInt != null)
			{
				if (this.pawn.Downed && this.curStateInt.def.recoverFromDowned)
				{
					string str = "In mental state while downed or deathresting, but not allowed: ";
					Pawn pawn = this.pawn;
					Log.Error(str + ((pawn != null) ? pawn.ToString() : null));
					this.CurState.RecoverFromState();
					return;
				}
				this.curStateInt.MentalStateTick(delta);
			}
		}

		// Token: 0x06004343 RID: 17219 RVA: 0x001A8600 File Offset: 0x001A6800
		public bool MentalBreaksBlocked()
		{
			foreach (Hediff hediff in this.pawn.health.hediffSet.hediffs)
			{
				if (hediff.CurStage != null && hediff.CurStage.blocksMentalBreaks)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004344 RID: 17220 RVA: 0x001A8678 File Offset: 0x001A6878
		public bool TryStartMentalState(MentalStateDef stateDef, string reason = null, bool forced = false, bool forceWake = false, bool causedByMood = false, Pawn otherPawn = null, bool transitionSilently = false, bool causedByDamage = false, bool causedByPsycast = false)
		{
			if (!forced && this.MentalBreaksBlocked())
			{
				return false;
			}
			if (this.pawn.IsMutant && this.pawn.mutant.Def.preventsMentalBreaks)
			{
				return false;
			}
			if (this.CurStateDef == stateDef || (!forceWake && !this.pawn.Awake()))
			{
				return false;
			}
			if (TutorSystem.TutorialMode && this.pawn.Faction == Faction.OfPlayer)
			{
				return false;
			}
			if (!forced && !stateDef.Worker.StateCanOccur(this.pawn))
			{
				return false;
			}
			if (this.curStateInt != null && !transitionSilently)
			{
				this.curStateInt.RecoverFromState();
			}
			MentalState mentalState = (MentalState)Activator.CreateInstance(stateDef.stateClass);
			mentalState.pawn = this.pawn;
			mentalState.def = stateDef;
			mentalState.causedByMood = causedByMood;
			mentalState.causedByDamage = causedByDamage;
			mentalState.causedByPsycast = causedByPsycast;
			mentalState.causedByPawn = otherPawn;
			MentalState_SocialFighting mentalState_SocialFighting = mentalState as MentalState_SocialFighting;
			if (mentalState_SocialFighting != null)
			{
				mentalState_SocialFighting.otherPawn = otherPawn;
			}
			mentalState.PreStart();
			if (!transitionSilently)
			{
				if ((this.pawn.IsColonist || this.pawn.HostFaction == Faction.OfPlayer) && stateDef.tale != null)
				{
					TaleRecorder.RecordTale(stateDef.tale, new object[]
					{
						this.pawn
					});
				}
				if (stateDef.IsExtreme && this.pawn.IsPlayerControlledCaravanMember())
				{
					Messages.Message("MessageCaravanMemberHasExtremeMentalBreak".Translate(), this.pawn.GetCaravan(), MessageTypeDefOf.ThreatSmall, true);
				}
				this.pawn.records.Increment(RecordDefOf.TimesInMentalState);
			}
			if (this.pawn.Drafted)
			{
				this.pawn.drafter.Drafted = false;
			}
			if (this.pawn.mechanitor != null)
			{
				this.pawn.mechanitor.UndraftAllMechs();
			}
			this.curStateInt = mentalState;
			if (this.pawn.needs.mood != null)
			{
				this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
			if (stateDef != null && stateDef.IsAggro && this.pawn.caller != null)
			{
				this.pawn.caller.Notify_InAggroMentalState();
			}
			Lord lord = this.pawn.GetLord();
			if (lord != null)
			{
				lord.Notify_InMentalState(this.pawn, stateDef);
			}
			if (this.curStateInt != null)
			{
				this.curStateInt.PostStart(reason);
			}
			if (stateDef.stopsJobs && this.pawn.CurJob != null)
			{
				this.pawn.jobs.StopAll(false, true);
				if (this.pawn.IsCarrying())
				{
					Thing thing;
					this.pawn.carryTracker.TryDropCarriedThing(this.pawn.PositionHeld, ThingPlaceMode.Near, out thing, null);
				}
			}
			if (this.pawn.Spawned)
			{
				this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
			}
			if (this.pawn.Spawned && forceWake)
			{
				if (lord != null)
				{
					lord.Notify_DormancyWakeup();
				}
				if (!this.pawn.Awake())
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
			CompTransporter compTransporter = this.pawn.ParentHolder as CompTransporter;
			if (compTransporter != null)
			{
				Thing thing;
				compTransporter.innerContainer.TryDrop(this.pawn, ThingPlaceMode.Near, out thing, null, null);
			}
			if (!transitionSilently && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				TaggedString taggedString = mentalState.GetBeginLetterText();
				if (!taggedString.NullOrEmpty())
				{
					string str = (stateDef.beginLetterLabel ?? stateDef.LabelCap).CapitalizeFirst() + ": " + this.pawn.LabelShortCap;
					if (!reason.NullOrEmpty())
					{
						taggedString += "\n\n" + reason;
					}
					Find.LetterStack.ReceiveLetter(str, taggedString, stateDef.beginLetterDef, this.pawn, null, null, null, null, 0, true);
				}
			}
			return true;
		}

		// Token: 0x06004345 RID: 17221 RVA: 0x001A8A64 File Offset: 0x001A6C64
		public void Notify_DamageTaken(DamageInfo dinfo)
		{
			if (!this.neverFleeIndividual && this.pawn.Spawned && this.pawn.MentalStateDef == null && !this.pawn.Downed && dinfo.Def.ExternalViolenceFor(this.pawn) && this.pawn.RaceProps.Humanlike && this.pawn.mindState.canFleeIndividual)
			{
				float lerpPct = (float)(this.pawn.HashOffset() % 100) / 100f;
				float num = this.pawn.kindDef.fleeHealthThresholdRange.LerpThroughRange(lerpPct);
				if (this.pawn.health.summaryHealth.SummaryHealthPercent < num && this.pawn.Faction != Faction.OfPlayer && this.pawn.HostFaction == null)
				{
					this.TryStartMentalState(MentalStateDefOf.PanicFlee, null, false, false, false, null, false, true, false);
				}
			}
		}

		// Token: 0x06004346 RID: 17222 RVA: 0x001A8B64 File Offset: 0x001A6D64
		internal void ClearMentalStateDirect()
		{
			if (this.curStateInt == null)
			{
				return;
			}
			this.curStateInt = null;
			QuestUtility.SendQuestTargetSignals(this.pawn.questTags, "ExitMentalState", this.pawn.Named("SUBJECT"));
			if (this.pawn.Spawned)
			{
				this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
			}
		}

		// Token: 0x04002BF0 RID: 11248
		private Pawn pawn;

		// Token: 0x04002BF1 RID: 11249
		private MentalState curStateInt;

		// Token: 0x04002BF2 RID: 11250
		public bool neverFleeIndividual;
	}
}
