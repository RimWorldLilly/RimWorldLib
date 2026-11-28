using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000863 RID: 2147
	public class MentalState : IExposable
	{
		// Token: 0x17000CCC RID: 3276
		// (get) Token: 0x0600435E RID: 17246 RVA: 0x001A90E0 File Offset: 0x001A72E0
		public int Age
		{
			get
			{
				return this.age;
			}
		}

		// Token: 0x17000CCD RID: 3277
		// (get) Token: 0x0600435F RID: 17247 RVA: 0x001A90E8 File Offset: 0x001A72E8
		public virtual string InspectLine
		{
			get
			{
				return this.def.baseInspectLine;
			}
		}

		// Token: 0x17000CCE RID: 3278
		// (get) Token: 0x06004360 RID: 17248 RVA: 0x001A90F5 File Offset: 0x001A72F5
		protected virtual bool CanEndBeforeMaxDurationNow
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CCF RID: 3279
		// (get) Token: 0x06004361 RID: 17249 RVA: 0x001A90F8 File Offset: 0x001A72F8
		public virtual bool AllowRestingInBed
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004362 RID: 17250 RVA: 0x001A90FC File Offset: 0x001A72FC
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<MentalStateDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Values.Look<bool>(ref this.causedByMood, "causedByMood", false, false);
			Scribe_Values.Look<bool>(ref this.causedByDamage, "causedByDamage", false, false);
			Scribe_Values.Look<bool>(ref this.causedByPsycast, "causedByPsycast", false, false);
			Scribe_References.Look<Pawn>(ref this.causedByPawn, "causedByPawn", false);
			Scribe_Values.Look<int>(ref this.forceRecoverAfterTicks, "forceRecoverAfterTicks", 0, false);
			Scribe_References.Look<Faction>(ref this.sourceFaction, "sourceFaction", false);
		}

		// Token: 0x06004363 RID: 17251 RVA: 0x001A9195 File Offset: 0x001A7395
		public virtual void PostStart(string reason)
		{
		}

		// Token: 0x06004364 RID: 17252 RVA: 0x001A9197 File Offset: 0x001A7397
		public virtual void PreStart()
		{
		}

		// Token: 0x06004365 RID: 17253 RVA: 0x001A919C File Offset: 0x001A739C
		public virtual void PostEnd()
		{
			if (!this.def.recoveryMessage.NullOrEmpty() && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				TaggedString taggedString = this.def.recoveryMessage.Formatted(this.pawn.LabelShort, this.pawn.Named("PAWN"));
				if (!taggedString.NullOrEmpty())
				{
					Messages.Message(taggedString.AdjustedFor(this.pawn, "PAWN", true).CapitalizeFirst(), this.pawn, MessageTypeDefOf.SituationResolved, true);
				}
			}
		}

		// Token: 0x06004366 RID: 17254 RVA: 0x001A9238 File Offset: 0x001A7438
		public virtual void MentalStateTick(int delta)
		{
			if (this.pawn.IsHashIntervalTick(30, delta))
			{
				this.age += 30;
				if (this.age >= this.def.maxTicksBeforeRecovery || (this.age >= this.def.minTicksBeforeRecovery && this.CanEndBeforeMaxDurationNow && Rand.MTBEventOccurs(this.def.recoveryMtbDays, 60000f, 30f)) || (this.forceRecoverAfterTicks != -1 && this.age >= this.forceRecoverAfterTicks))
				{
					this.RecoverFromState();
					return;
				}
				if (this.def.recoverFromSleep && !this.pawn.Awake())
				{
					this.RecoverFromState();
					return;
				}
			}
		}

		// Token: 0x06004367 RID: 17255 RVA: 0x001A92F0 File Offset: 0x001A74F0
		public void RecoverFromState()
		{
			if (this.pawn.MentalState != this)
			{
				string str = "Recovered from ";
				MentalStateDef mentalStateDef = this.def;
				string str2 = (mentalStateDef != null) ? mentalStateDef.ToString() : null;
				string str3 = " but pawn's mental state is not this, it is ";
				MentalState mentalState = this.pawn.MentalState;
				Log.Error(str + str2 + str3 + ((mentalState != null) ? mentalState.ToString() : null));
			}
			if (!this.pawn.Dead)
			{
				this.pawn.mindState.mentalStateHandler.ClearMentalStateDirect();
				if (this.causedByMood && this.def.moodRecoveryThought != null && this.pawn.needs.mood != null)
				{
					this.pawn.needs.mood.thoughts.memories.TryGainMemory(this.def.moodRecoveryThought, null, null);
				}
				this.pawn.mindState.mentalBreaker.Notify_RecoveredFromMentalState();
				this.pawn.mindState.mentalFitGenerator.Notify_RecoveredFromMentalState();
				if (this.pawn.story != null && this.pawn.story.traits != null)
				{
					foreach (Trait trait in this.pawn.story.traits.allTraits)
					{
						if (!trait.Suppressed)
						{
							trait.Notify_MentalStateEndedOn(this.pawn, this.causedByMood);
						}
					}
				}
				if (this.def.IsAggro)
				{
					this.pawn.mindState.enemyTarget = null;
				}
			}
			if (this.def.stopsJobs && this.pawn.Spawned)
			{
				this.pawn.jobs.StopAll(true, true);
			}
			this.PostEnd();
		}

		// Token: 0x06004368 RID: 17256 RVA: 0x001A94C4 File Offset: 0x001A76C4
		public virtual bool ForceHostileTo(Thing t)
		{
			return false;
		}

		// Token: 0x06004369 RID: 17257 RVA: 0x001A94C7 File Offset: 0x001A76C7
		public virtual bool ForceHostileTo(Faction f)
		{
			return false;
		}

		// Token: 0x0600436A RID: 17258 RVA: 0x001A94CA File Offset: 0x001A76CA
		public EffecterDef CurrentStateEffecter()
		{
			return this.def.stateEffecter;
		}

		// Token: 0x0600436B RID: 17259 RVA: 0x001A94D7 File Offset: 0x001A76D7
		public virtual RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.SuperActive;
		}

		// Token: 0x0600436C RID: 17260 RVA: 0x001A94DC File Offset: 0x001A76DC
		public virtual TaggedString GetBeginLetterText()
		{
			if (this.def.beginLetter.NullOrEmpty())
			{
				return null;
			}
			return this.def.beginLetter.Formatted(this.pawn.NameShortColored, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true).Resolve().CapitalizeFirst();
		}

		// Token: 0x0600436D RID: 17261 RVA: 0x001A9558 File Offset: 0x001A7758
		public virtual void Notify_AttackedTarget(LocalTargetInfo hitTarget)
		{
		}

		// Token: 0x0600436E RID: 17262 RVA: 0x001A955A File Offset: 0x001A775A
		public virtual void Notify_SlaughteredTarget()
		{
		}

		// Token: 0x0600436F RID: 17263 RVA: 0x001A955C File Offset: 0x001A775C
		public virtual void Notify_ReleasedTarget()
		{
		}

		// Token: 0x04002BFB RID: 11259
		public Pawn pawn;

		// Token: 0x04002BFC RID: 11260
		public MentalStateDef def;

		// Token: 0x04002BFD RID: 11261
		protected int age;

		// Token: 0x04002BFE RID: 11262
		public bool causedByMood;

		// Token: 0x04002BFF RID: 11263
		public bool causedByDamage;

		// Token: 0x04002C00 RID: 11264
		public bool causedByPsycast;

		// Token: 0x04002C01 RID: 11265
		public Pawn causedByPawn;

		// Token: 0x04002C02 RID: 11266
		public int forceRecoverAfterTicks = -1;

		// Token: 0x04002C03 RID: 11267
		public Faction sourceFaction;

		// Token: 0x04002C04 RID: 11268
		protected const int MentalStateTickInterval = 30;
	}
}
