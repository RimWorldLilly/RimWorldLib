using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI.Group;

namespace Verse.AI
{
	// Token: 0x020008B0 RID: 2224
	public class Pawn_JobTracker : IExposable
	{
		// Token: 0x17000CF5 RID: 3317
		// (get) Token: 0x060044EE RID: 17646 RVA: 0x001B0AFF File Offset: 0x001AECFF
		public bool HandlingFacing
		{
			get
			{
				return this.curDriver != null && this.curDriver.HandlingFacing;
			}
		}

		// Token: 0x17000CF6 RID: 3318
		// (get) Token: 0x060044EF RID: 17647 RVA: 0x001B0B16 File Offset: 0x001AED16
		public bool DeterminingNextJob
		{
			get
			{
				return this.determiningNextJob;
			}
		}

		// Token: 0x17000CF7 RID: 3319
		// (get) Token: 0x060044F0 RID: 17648 RVA: 0x001B0B1E File Offset: 0x001AED1E
		public int StartedFormingCaravanTick
		{
			get
			{
				if (!this.pawn.IsFormingCaravan())
				{
					this.startedFormingCaravanTickInt = -1;
				}
				return this.startedFormingCaravanTickInt;
			}
		}

		// Token: 0x060044F1 RID: 17649 RVA: 0x001B0B3C File Offset: 0x001AED3C
		public Pawn_JobTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
		}

		// Token: 0x060044F2 RID: 17650 RVA: 0x001B0BA8 File Offset: 0x001AEDA8
		public virtual void ExposeData()
		{
			Scribe_Deep.Look<Job>(ref this.curJob, "curJob", Array.Empty<object>());
			Scribe_Deep.Look<JobDriver>(ref this.curDriver, "curDriver", Array.Empty<object>());
			Scribe_Deep.Look<JobQueue>(ref this.jobQueue, "jobQueue", Array.Empty<object>());
			Scribe_Values.Look<PawnPosture>(ref this.posture, "posture", PawnPosture.Standing, false);
			Scribe_Values.Look<int>(ref this.startedFormingCaravanTickInt, "formingCaravanTick", 0, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.curDriver != null)
				{
					this.curDriver.pawn = this.pawn;
					this.curDriver.job = this.curJob;
					return;
				}
			}
			else if (Scribe.mode == LoadSaveMode.PostLoadInit && this.curDriver == null && this.curJob != null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Cleaning up invalid job state on ");
				defaultInterpolatedStringHandler.AppendFormatted<Pawn>(this.pawn);
				Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				this.EndCurrentJob(JobCondition.Errored, true, true);
			}
		}

		// Token: 0x060044F3 RID: 17651 RVA: 0x001B0C9C File Offset: 0x001AEE9C
		public void Notify_WorkTypeDisabled(WorkTypeDef wType)
		{
			bool flag = this.pawn.WorkTypeIsDisabled(wType);
			this.jobQueue.RemoveAllWorkType(this.pawn, wType, flag);
			if (this.curJob != null && this.curJob.workGiverDef != null && this.curJob.workGiverDef.workType == wType && (flag || !this.curJob.playerForced))
			{
				this.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		// Token: 0x060044F4 RID: 17652 RVA: 0x001B0D0B File Offset: 0x001AEF0B
		public void Notify_JoyKindDisabled(JoyKindDef joyKind)
		{
			this.jobQueue.RemoveAllJoyKind(this.pawn, joyKind);
			if (this.curJob != null && this.curJob.def.joyKind == joyKind)
			{
				this.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		// Token: 0x060044F5 RID: 17653 RVA: 0x001B0D44 File Offset: 0x001AEF44
		public virtual void JobTrackerTick()
		{
			JobDriver jobDriver = this.curDriver;
			if (jobDriver != null)
			{
				jobDriver.DriverTick();
			}
			this.jobsGivenThisTick = 0;
			this.jobsGivenThisTickTextual = "";
		}

		// Token: 0x060044F6 RID: 17654 RVA: 0x001B0D6C File Offset: 0x001AEF6C
		public virtual void JobTrackerTickInterval(int delta)
		{
			if (this.pawn.IsHashIntervalTick(30, delta))
			{
				ThinkResult thinkResult = this.DetermineNextConstantThinkTreeJob();
				if (thinkResult.IsValid)
				{
					if (this.ShouldStartJobFromThinkTree(thinkResult))
					{
						this.CheckLeaveJoinableLordBecauseJobIssued(thinkResult);
						this.StartJob(thinkResult.Job, JobCondition.InterruptForced, thinkResult.SourceNode, false, false, this.pawn.thinker.ConstantThinkTree, thinkResult.Tag, false, false, null, false, true, false);
					}
					else if (thinkResult.Job != this.curJob && !this.jobQueue.Contains(thinkResult.Job))
					{
						JobMaker.ReturnToPool(thinkResult.Job);
					}
				}
			}
			if (this.curDriver != null)
			{
				if (GenTicks.TicksGame != this.curJob.startTick)
				{
					bool flag = false;
					if (this.curJob.intervalScalingTarget != TargetIndex.None)
					{
						int lengthManhattan = (this.curJob.GetTarget(this.curJob.intervalScalingTarget).Cell - this.pawn.Position).LengthManhattan;
						this.curJob.expiryInterval = (int)Pawn_JobTracker.DistanceIntervalCurve.Evaluate((float)lengthManhattan);
					}
					if (this.curJob.expiryInterval > 0)
					{
						flag = (this.curJob.startTick + this.curJob.expiryInterval <= GenTicks.TicksGame && this.pawn.IsHashIntervalTick(this.curJob.expiryInterval, delta));
					}
					if (flag)
					{
						if (!this.curJob.expireRequiresEnemiesNearby || PawnUtility.EnemiesAreNearby(this.pawn, 25, false, -1f, 1, false))
						{
							if (this.debugLog)
							{
								this.DebugLogEvent("Job expire");
							}
							if (!this.curJob.checkOverrideOnExpire)
							{
								this.EndCurrentJob(JobCondition.Succeeded, true, true);
							}
							else
							{
								this.CheckForJobOverride(0f, true);
							}
							this.FinalizeTick();
							return;
						}
						if (this.debugLog)
						{
							this.DebugLogEvent("Job expire skipped because there are no enemies nearby");
						}
					}
				}
				this.curDriver.DriverTickInterval(delta);
			}
			if (this.curJob == null && !this.pawn.Dead && this.pawn.mindState.Active)
			{
				if (this.debugLog)
				{
					this.DebugLogEvent("Starting job from Tick because curJob == null.");
				}
				this.TryFindAndStartJob();
			}
			this.FinalizeTick();
		}

		// Token: 0x060044F7 RID: 17655 RVA: 0x001B0FB0 File Offset: 0x001AF1B0
		private void FinalizeTick()
		{
			this.jobsGivenRecentTicks.Add(this.jobsGivenThisTick);
			this.jobsGivenRecentTicksTextual.Add(this.jobsGivenThisTickTextual);
			int num = GenTicks.TicksGame - this.lastTickFinalized;
			this.lastTickFinalized = GenTicks.TicksGame;
			int num2 = 0;
			while (num2 < num && this.jobsGivenRecentTicks.Count > 0)
			{
				this.jobsGivenRecentTicks.RemoveAt(0);
				this.jobsGivenRecentTicksTextual.RemoveAt(0);
				num2++;
			}
			if (this.jobsGivenThisTick != 0)
			{
				int num3 = 0;
				for (int i = 0; i < this.jobsGivenRecentTicks.Count; i++)
				{
					num3 += this.jobsGivenRecentTicks[i];
				}
				if (num3 >= 10)
				{
					string value = this.jobsGivenRecentTicksTextual.ToCommaList(false, false);
					this.jobsGivenRecentTicks.Clear();
					this.jobsGivenRecentTicksTextual.Clear();
					Pawn pawn = this.pawn;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 4);
					defaultInterpolatedStringHandler.AppendFormatted(this.pawn.ToStringSafe<Pawn>());
					defaultInterpolatedStringHandler.AppendLiteral(" started ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(10);
					defaultInterpolatedStringHandler.AppendLiteral(" jobs in ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(10);
					defaultInterpolatedStringHandler.AppendLiteral(" ticks. List: ");
					defaultInterpolatedStringHandler.AppendFormatted(value);
					JobUtility.TryStartErrorRecoverJob(pawn, defaultInterpolatedStringHandler.ToStringAndClear(), null, null);
				}
			}
		}

		// Token: 0x060044F8 RID: 17656 RVA: 0x001B10F7 File Offset: 0x001AF2F7
		public IEnumerable<Job> AllJobs()
		{
			Pawn_JobTracker.<AllJobs>d__32 <AllJobs>d__ = new Pawn_JobTracker.<AllJobs>d__32(-2);
			<AllJobs>d__.<>4__this = this;
			return <AllJobs>d__;
		}

		// Token: 0x060044F9 RID: 17657 RVA: 0x001B1108 File Offset: 0x001AF308
		public void StartJob(Job newJob, JobCondition lastJobEndCondition = JobCondition.None, ThinkNode jobGiver = null, bool resumeCurJobAfterwards = false, bool cancelBusyStances = true, ThinkTreeDef thinkTree = null, JobTag? tag = null, bool fromQueue = false, bool canReturnCurJobToPool = false, bool? keepCarryingThingOverride = null, bool continueSleeping = false, bool addToJobsThisTick = true, bool preToilReservationsCanFail = false)
		{
			this.startingNewJob = true;
			Job job = null;
			try
			{
				if (addToJobsThisTick && !fromQueue && (!Find.TickManager.Paused || this.lastJobGivenAtFrame == RealTime.frameCount))
				{
					this.jobsGivenThisTick++;
					if (Prefs.DevMode)
					{
						this.jobsGivenThisTickTextual = this.jobsGivenThisTickTextual + "(" + ((newJob != null) ? newJob.ToString() : null) + ") ";
					}
				}
				this.lastJobGivenAtFrame = RealTime.frameCount;
				if (this.jobsGivenThisTick > 10)
				{
					string value = this.jobsGivenThisTickTextual;
					this.jobsGivenThisTick = 0;
					this.jobsGivenThisTickTextual = "";
					this.startingNewJob = false;
					this.pawn.ClearReservationsForJob(newJob);
					Pawn pawn = this.pawn;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 4);
					defaultInterpolatedStringHandler.AppendFormatted(this.pawn.ToStringSafe<Pawn>());
					defaultInterpolatedStringHandler.AppendLiteral(" started 10 jobs in one tick. newJob=");
					defaultInterpolatedStringHandler.AppendFormatted(newJob.ToStringSafe<Job>());
					defaultInterpolatedStringHandler.AppendLiteral(" jobGiver=");
					defaultInterpolatedStringHandler.AppendFormatted(jobGiver.ToStringSafe<ThinkNode>());
					defaultInterpolatedStringHandler.AppendLiteral(" jobList=");
					defaultInterpolatedStringHandler.AppendFormatted(value);
					JobUtility.TryStartErrorRecoverJob(pawn, defaultInterpolatedStringHandler.ToStringAndClear(), null, null);
				}
				else
				{
					if (this.debugLog)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(63, 4);
						defaultInterpolatedStringHandler2.AppendLiteral("StartJob [");
						defaultInterpolatedStringHandler2.AppendFormatted<Job>(newJob);
						defaultInterpolatedStringHandler2.AppendLiteral("] lastJobEndCondition=");
						defaultInterpolatedStringHandler2.AppendFormatted<JobCondition>(lastJobEndCondition);
						defaultInterpolatedStringHandler2.AppendLiteral(", jobGiver=");
						defaultInterpolatedStringHandler2.AppendFormatted<ThinkNode>(jobGiver);
						defaultInterpolatedStringHandler2.AppendLiteral(", cancelBusyStances=");
						defaultInterpolatedStringHandler2.AppendFormatted<bool>(cancelBusyStances);
						this.DebugLogEvent(defaultInterpolatedStringHandler2.ToStringAndClear());
					}
					if (cancelBusyStances && this.pawn.stances.FullBodyBusy)
					{
						this.pawn.stances.CancelBusyStanceHard();
					}
					bool flag;
					if (continueSleeping)
					{
						JobDriver jobDriver = this.curDriver;
						flag = (jobDriver != null && jobDriver.asleep);
					}
					else
					{
						flag = false;
					}
					bool asleep = flag;
					if (this.curJob != null)
					{
						if (lastJobEndCondition == JobCondition.None)
						{
							string[] array = new string[8];
							int num = 0;
							Pawn pawn2 = this.pawn;
							array[num] = ((pawn2 != null) ? pawn2.ToString() : null);
							array[1] = " starting job ";
							array[2] = ((newJob != null) ? newJob.ToString() : null);
							array[3] = " from JobGiver ";
							int num2 = 4;
							ThinkNode jobGiver2 = newJob.jobGiver;
							array[num2] = ((jobGiver2 != null) ? jobGiver2.ToString() : null);
							array[5] = " while already having job ";
							int num3 = 6;
							Job job2 = this.curJob;
							array[num3] = ((job2 != null) ? job2.ToString() : null);
							array[7] = " without a specific job end condition.";
							Log.Warning(string.Concat(array));
							lastJobEndCondition = JobCondition.InterruptForced;
						}
						if (resumeCurJobAfterwards && this.curJob.def.suspendable)
						{
							this.SuspendCurrentJob(lastJobEndCondition, cancelBusyStances, keepCarryingThingOverride);
						}
						else
						{
							job = this.curDriver.GetFinalizerJob(lastJobEndCondition);
							if (job != null)
							{
								bool value2 = keepCarryingThingOverride.GetValueOrDefault();
								if (keepCarryingThingOverride == null)
								{
									value2 = true;
									keepCarryingThingOverride = new bool?(value2);
								}
							}
							this.CleanupCurrentJob(lastJobEndCondition, true, cancelBusyStances, canReturnCurJobToPool, keepCarryingThingOverride);
						}
					}
					if (newJob == null)
					{
						Pawn pawn3 = this.pawn;
						Log.Warning(((pawn3 != null) ? pawn3.ToString() : null) + " tried to start doing a null job.");
					}
					else
					{
						newJob.startTick = Find.TickManager.TicksGame;
						if (this.pawn.Drafted || newJob.playerForced)
						{
							newJob.ignoreForbidden = true;
							newJob.ignoreDesignations = true;
						}
						using (List<Hediff>.Enumerator enumerator = this.pawn.health.hediffSet.hediffs.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								string reportStringOverride;
								if (enumerator.Current.def.TryGetReportStringOverrideFor(newJob.def, out reportStringOverride))
								{
									newJob.reportStringOverride = reportStringOverride;
									break;
								}
							}
						}
						this.curJob = newJob;
						this.curJob.jobGiverThinkTree = thinkTree;
						this.curJob.jobGiver = jobGiver;
						this.curDriver = this.curJob.MakeDriver(this.pawn);
						this.curDriver.asleep = asleep;
						if (this.curDriver.TryMakePreToilReservations(!preToilReservationsCanFail && !fromQueue))
						{
							Job job3 = this.TryOpportunisticJob(job, newJob);
							if (job3 != null)
							{
								this.jobQueue.EnqueueFirst(newJob, null);
								this.curJob = null;
								this.ClearDriver();
								Job newJob2 = job3;
								JobCondition lastJobEndCondition2 = JobCondition.None;
								ThinkNode jobGiver3 = null;
								bool resumeCurJobAfterwards2 = false;
								bool cancelBusyStances2 = true;
								ThinkTreeDef thinkTree2 = null;
								bool? keepCarryingThingOverride2 = keepCarryingThingOverride;
								this.StartJob(newJob2, lastJobEndCondition2, jobGiver3, resumeCurJobAfterwards2, cancelBusyStances2, thinkTree2, null, false, false, keepCarryingThingOverride2, false, true, false);
							}
							else
							{
								if (tag != null)
								{
									JobTag? jobTag = tag;
									JobTag jobTag2 = JobTag.Fieldwork;
									if (jobTag.GetValueOrDefault() == jobTag2 & jobTag != null)
									{
										JobTag lastJobTag = this.pawn.mindState.lastJobTag;
										jobTag = tag;
										if (!(lastJobTag == jobTag.GetValueOrDefault() & jobTag != null))
										{
											foreach (Pawn pawn4 in PawnUtility.SpawnedMasteredPawns(this.pawn))
											{
												pawn4.jobs.Notify_MasterStartedFieldWork();
											}
										}
									}
									this.pawn.mindState.lastJobTag = tag.Value;
								}
								if (this.pawn.IsCarrying() && !(keepCarryingThingOverride ?? (!this.curJob.def.dropThingBeforeJob)))
								{
									if (DebugViewSettings.logCarriedBetweenJobs)
									{
										DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(30, 2);
										defaultInterpolatedStringHandler3.AppendLiteral("Dropping ");
										defaultInterpolatedStringHandler3.AppendFormatted<Thing>(this.pawn.carryTracker.CarriedThing);
										defaultInterpolatedStringHandler3.AppendLiteral(" before starting job ");
										defaultInterpolatedStringHandler3.AppendFormatted<Job>(newJob);
										Log.Message(defaultInterpolatedStringHandler3.ToStringAndClear());
									}
									Thing thing;
									this.pawn.carryTracker.TryDropCarriedThing(this.pawn.Position, ThingPlaceMode.Near, out thing, null);
								}
								this.curDriver.SetInitialPosture();
								this.curDriver.Notify_Starting();
								this.curDriver.SetupToils();
								this.curDriver.ReadyForNextToil();
								Pawn_FlightTracker flight = this.pawn.flight;
								if (flight != null)
								{
									flight.Notify_JobStarted(newJob);
								}
							}
						}
						else if (preToilReservationsCanFail || fromQueue)
						{
							this.EndCurrentJob(JobCondition.QueuedNoLongerValid, true, true);
						}
						else
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(145, 2);
							defaultInterpolatedStringHandler4.AppendLiteral("TryMakePreToilReservations() returned false for a non-queued job right after StartJob(). This should have been checked before. pawn = ");
							defaultInterpolatedStringHandler4.AppendFormatted<Pawn>(this.pawn);
							defaultInterpolatedStringHandler4.AppendLiteral(", curJob = ");
							defaultInterpolatedStringHandler4.AppendFormatted(this.curJob.ToStringSafe<Job>());
							Log.Warning(defaultInterpolatedStringHandler4.ToStringAndClear());
							this.EndCurrentJob(JobCondition.Errored, true, true);
						}
					}
				}
			}
			finally
			{
				this.startingNewJob = false;
			}
		}

		// Token: 0x060044FA RID: 17658 RVA: 0x001B17A8 File Offset: 0x001AF9A8
		public void EndCurrentOrQueuedJob(Job job, JobCondition condition, bool startNewJob = true, bool canReturnToPool = true)
		{
			if (this.debugLog)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 2);
				defaultInterpolatedStringHandler.AppendLiteral("EndJob [");
				defaultInterpolatedStringHandler.AppendFormatted<Job>(job);
				defaultInterpolatedStringHandler.AppendLiteral("] condition=");
				defaultInterpolatedStringHandler.AppendFormatted<JobCondition>(condition);
				this.DebugLogEvent(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			QueuedJob queuedJob = this.jobQueue.Extract(job);
			if (queuedJob != null)
			{
				queuedJob.Cleanup(this.pawn, canReturnToPool);
			}
			if (this.curJob == job)
			{
				this.EndCurrentJob(condition, startNewJob, canReturnToPool);
			}
		}

		// Token: 0x060044FB RID: 17659 RVA: 0x001B1830 File Offset: 0x001AFA30
		public void SuspendCurrentJob(JobCondition jobPauseReason, bool cancelBusyStances = true, bool? carryThingAfterJobOverride = null)
		{
			if (this.debugLog)
			{
				this.DebugLogEvent("SuspendCurrentJob " + ((this.curJob != null) ? this.curJob.ToString() : "null") + " curToil=" + Pawn_JobTracker.CurToilString(this.curDriver));
			}
			if (this.curJob.def.suspendable)
			{
				this.jobQueue.EnqueueFirst(this.curJob, null);
				this.CleanupCurrentJob(jobPauseReason, false, cancelBusyStances, false, carryThingAfterJobOverride);
				return;
			}
			this.CleanupCurrentJob(jobPauseReason, true, cancelBusyStances, false, carryThingAfterJobOverride);
		}

		// Token: 0x060044FC RID: 17660 RVA: 0x001B18C4 File Offset: 0x001AFAC4
		public void EndCurrentJob(JobCondition condition, bool startNewJob = true, bool canReturnToPool = true)
		{
			if (this.debugLog)
			{
				this.DebugLogEvent(string.Concat(new string[]
				{
					"EndCurrentJob ",
					(this.curJob != null) ? this.curJob.ToString() : "null",
					" condition=",
					condition.ToString(),
					" curToil=",
					Pawn_JobTracker.CurToilString(this.curDriver)
				}));
			}
			if (condition == JobCondition.Ongoing)
			{
				Log.Warning("Ending a job with Ongoing as the condition. This makes no sense.");
			}
			Job job = this.curJob;
			JobDef jobDef = (job != null) ? job.def : null;
			bool collideWithPawns = false;
			if (this.curJob != null)
			{
				collideWithPawns = (this.curJob.collideWithPawns || this.curJob.def.collideWithPawns);
			}
			JobDriver jobDriver = this.curDriver;
			Job job2 = (jobDriver != null) ? jobDriver.GetFinalizerJob(condition) : null;
			if (job2 != null && this.curJob != null)
			{
				job2.playerForced = this.curJob.playerForced;
			}
			bool? carryThingAfterJobOverride = null;
			if (startNewJob && job2 != null)
			{
				carryThingAfterJobOverride = new bool?(true);
			}
			this.CleanupCurrentJob(condition, true, true, canReturnToPool, carryThingAfterJobOverride);
			if (startNewJob)
			{
				if (condition == JobCondition.ErroredPather || condition == JobCondition.Errored)
				{
					Job newJob = JobMaker.MakeJob(JobDefOf.Wait, 250, false);
					JobCondition lastJobEndCondition = JobCondition.None;
					ThinkNode jobGiver = null;
					bool resumeCurJobAfterwards = false;
					bool cancelBusyStances = true;
					ThinkTreeDef thinkTree = null;
					JobTag? tag = null;
					bool fromQueue = false;
					bool canReturnCurJobToPool = false;
					bool? keepCarryingThingOverride = null;
					this.StartJob(newJob, lastJobEndCondition, jobGiver, resumeCurJobAfterwards, cancelBusyStances, thinkTree, tag, fromQueue, canReturnCurJobToPool, keepCarryingThingOverride, false, true, false);
					return;
				}
				if (job2 != null && Pawn_JobTracker.CanPawnTakeOpportunisticJob(this.pawn))
				{
					Job newJob2 = job2;
					JobCondition lastJobEndCondition2 = JobCondition.None;
					ThinkNode jobGiver2 = null;
					bool resumeCurJobAfterwards2 = false;
					bool cancelBusyStances2 = true;
					ThinkTreeDef thinkTree2 = null;
					bool? keepCarryingThingOverride = new bool?(true);
					this.StartJob(newJob2, lastJobEndCondition2, jobGiver2, resumeCurJobAfterwards2, cancelBusyStances2, thinkTree2, null, false, false, keepCarryingThingOverride, false, true, false);
					return;
				}
				if (condition == JobCondition.Succeeded && jobDef != null && jobDef != JobDefOf.Wait_MaintainPosture && jobDef != JobDefOf.Goto)
				{
					Pawn_PathFollower pather = this.pawn.pather;
					if (pather != null && !pather.Moving)
					{
						Job job3 = JobMaker.MakeJob(JobDefOf.Wait_MaintainPosture, 1, false);
						job3.collideWithPawns = collideWithPawns;
						Job newJob3 = job3;
						JobCondition lastJobEndCondition3 = JobCondition.None;
						ThinkNode jobGiver3 = null;
						bool resumeCurJobAfterwards3 = false;
						bool cancelBusyStances3 = false;
						ThinkTreeDef thinkTree3 = null;
						JobTag? tag2 = null;
						bool fromQueue2 = false;
						bool canReturnCurJobToPool2 = false;
						bool? keepCarryingThingOverride = null;
						this.StartJob(newJob3, lastJobEndCondition3, jobGiver3, resumeCurJobAfterwards3, cancelBusyStances3, thinkTree3, tag2, fromQueue2, canReturnCurJobToPool2, keepCarryingThingOverride, false, false, false);
						return;
					}
				}
				this.TryFindAndStartJob();
			}
		}

		// Token: 0x060044FD RID: 17661 RVA: 0x001B1AD0 File Offset: 0x001AFCD0
		private void CleanupCurrentJob(JobCondition condition, bool releaseReservations, bool cancelBusyStancesSoft = true, bool canReturnToPool = false, bool? carryThingAfterJobOverride = null)
		{
			if (this.debugLog)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 2);
				defaultInterpolatedStringHandler.AppendLiteral("CleanupCurrentJob ");
				defaultInterpolatedStringHandler.AppendFormatted((this.curJob != null) ? this.curJob.def.ToString() : "null");
				defaultInterpolatedStringHandler.AppendLiteral(" condition ");
				defaultInterpolatedStringHandler.AppendFormatted<JobCondition>(condition);
				this.DebugLogEvent(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (this.curJob == null)
			{
				return;
			}
			Lord lord = this.pawn.GetLord();
			if (lord != null)
			{
				lord.Notify_PawnJobDone(this.pawn, condition);
			}
			if (condition == JobCondition.Succeeded && this.curJob.def.taleOnCompletion != null)
			{
				TaleRecorder.RecordTale(this.curJob.def.taleOnCompletion, this.curDriver.TaleParameters());
			}
			if (releaseReservations)
			{
				this.pawn.ClearReservationsForJob(this.curJob);
			}
			if (this.curDriver != null)
			{
				this.curDriver.ended = true;
				this.curDriver.Cleanup(condition);
			}
			this.ClearDriver();
			Job job = this.curJob;
			this.curJob = null;
			if (!releaseReservations)
			{
				this.pawn.VerifyReservations(job);
			}
			if (cancelBusyStancesSoft)
			{
				this.pawn.stances.CancelBusyStanceSoft();
			}
			if (this.pawn.IsCarrying() && !(carryThingAfterJobOverride ?? job.def.carryThingAfterJob))
			{
				if (DebugViewSettings.logCarriedBetweenJobs)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(30, 2);
					defaultInterpolatedStringHandler2.AppendLiteral("Dropping ");
					defaultInterpolatedStringHandler2.AppendFormatted<Thing>(this.pawn.carryTracker.CarriedThing);
					defaultInterpolatedStringHandler2.AppendLiteral(" after finishing job ");
					defaultInterpolatedStringHandler2.AppendFormatted<Job>(job);
					Log.Message(defaultInterpolatedStringHandler2.ToStringAndClear());
				}
				Thing thing;
				this.pawn.carryTracker.TryDropCarriedThing(this.pawn.Position, ThingPlaceMode.Near, out thing, null);
			}
			if (releaseReservations && canReturnToPool)
			{
				JobMaker.ReturnToPool(job);
			}
		}

		// Token: 0x060044FE RID: 17662 RVA: 0x001B1CBA File Offset: 0x001AFEBA
		public JobQueue CaptureAndClearJobQueue()
		{
			JobQueue result = this.jobQueue.Capture();
			this.ClearQueuedJobs(false);
			return result;
		}

		// Token: 0x060044FF RID: 17663 RVA: 0x001B1CD0 File Offset: 0x001AFED0
		public void RestoreCapturedJobs(JobQueue incomming, bool canReturnToPool = true)
		{
			bool flag = false;
			QueuedJob queuedJob;
			while ((queuedJob = incomming.Dequeue()) != null)
			{
				if (flag)
				{
					if (canReturnToPool)
					{
						JobMaker.ReturnToPool(queuedJob.job);
					}
				}
				else if (!this.pawn.jobs.TryTakeOrderedJob(queuedJob.job, queuedJob.tag, true))
				{
					flag = true;
				}
			}
		}

		// Token: 0x06004500 RID: 17664 RVA: 0x001B1D1F File Offset: 0x001AFF1F
		public void ClearQueuedJobs(bool canReturnToPool = true)
		{
			if (this.debugLog)
			{
				this.DebugLogEvent("ClearQueuedJobs");
			}
			this.jobQueue.Clear(this.pawn, canReturnToPool);
		}

		// Token: 0x06004501 RID: 17665 RVA: 0x001B1D48 File Offset: 0x001AFF48
		public void ReleaseReservations(LocalTargetInfo reservedItem)
		{
			foreach (QueuedJob queuedJob in this.jobQueue)
			{
				if (this.pawn.MapHeld.reservationManager.ReservedBy(reservedItem, this.pawn, queuedJob.job))
				{
					this.pawn.MapHeld.reservationManager.Release(reservedItem, this.pawn, queuedJob.job);
				}
			}
		}

		// Token: 0x06004502 RID: 17666 RVA: 0x001B1DD4 File Offset: 0x001AFFD4
		public void CheckForJobOverride(float minPriority = 0f, bool ignoreQueue = true)
		{
			if (this.debugLog)
			{
				this.DebugLogEvent("CheckForJobOverride");
			}
			ThinkTreeDef thinkTree;
			ThinkResult thinkResult = this.DetermineNextJob(out thinkTree, ignoreQueue);
			if (!thinkResult.IsValid)
			{
				return;
			}
			float num;
			if (this.ShouldStartJobFromThinkTree(thinkResult) && (minPriority == 0f || (thinkResult.SourceNode.TryGetPriority(this.pawn, out num) && num >= minPriority)))
			{
				this.CheckLeaveJoinableLordBecauseJobIssued(thinkResult);
				this.StartJob(thinkResult.Job, JobCondition.InterruptOptional, thinkResult.SourceNode, false, false, thinkTree, thinkResult.Tag, thinkResult.FromQueue, false, null, false, true, false);
				return;
			}
			if (thinkResult.Job != this.curJob && !this.jobQueue.Contains(thinkResult.Job))
			{
				JobMaker.ReturnToPool(thinkResult.Job);
			}
		}

		// Token: 0x06004503 RID: 17667 RVA: 0x001B1EA0 File Offset: 0x001B00A0
		public void StopAll(bool ifLayingKeepLaying = false, bool canReturnToPool = true)
		{
			if (!RestUtility.IsLayingForJobCleanup(this.pawn) || !ifLayingKeepLaying)
			{
				this.CleanupCurrentJob(JobCondition.InterruptForced, true, true, canReturnToPool, null);
			}
			this.ClearQueuedJobs(canReturnToPool);
		}

		// Token: 0x06004504 RID: 17668 RVA: 0x001B1ED8 File Offset: 0x001B00D8
		private void TryFindAndStartJob()
		{
			if (this.pawn.thinker == null)
			{
				Pawn pawn = this.pawn;
				Log.ErrorOnce(((pawn != null) ? pawn.ToString() : null) + " did TryFindAndStartJob but had no thinker.", 8573261);
				return;
			}
			if (this.curJob != null)
			{
				Pawn pawn2 = this.pawn;
				string str = (pawn2 != null) ? pawn2.ToString() : null;
				string str2 = " doing TryFindAndStartJob while still having job ";
				Job job = this.curJob;
				Log.Warning(str + str2 + ((job != null) ? job.ToString() : null));
			}
			if (this.debugLog)
			{
				this.DebugLogEvent("TryFindAndStartJob");
			}
			ThinkTreeDef thinkTree;
			ThinkResult result = this.DetermineNextJob(out thinkTree, false);
			if (result.IsValid)
			{
				this.CheckLeaveJoinableLordBecauseJobIssued(result);
				this.StartJob(result.Job, JobCondition.None, result.SourceNode, false, false, thinkTree, result.Tag, result.FromQueue, false, null, false, true, false);
			}
		}

		// Token: 0x06004505 RID: 17669 RVA: 0x001B1FB4 File Offset: 0x001B01B4
		private static bool CanPawnTakeOpportunisticJob(Pawn pawn)
		{
			return !pawn.Drafted && !pawn.Downed && (!ModsConfig.AnomalyActive || !pawn.IsSubhuman) && !pawn.InMentalState && !pawn.IsBurning() && !SlaveRebellionUtility.IsRebelling(pawn);
		}

		// Token: 0x06004506 RID: 17670 RVA: 0x001B2004 File Offset: 0x001B0204
		public Job TryOpportunisticJob(Job finalizerJob, Job job)
		{
			if (!Pawn_JobTracker.CanPawnTakeOpportunisticJob(this.pawn))
			{
				return null;
			}
			if (!job.def.allowOpportunisticPrefix)
			{
				return null;
			}
			if (finalizerJob != null)
			{
				return finalizerJob;
			}
			IntVec3 cell = job.targetA.Cell;
			if (!cell.IsValid || cell.IsForbidden(this.pawn))
			{
				return null;
			}
			if (this.pawn.RaceProps.intelligence < Intelligence.Humanlike)
			{
				return null;
			}
			if (this.pawn.Faction != Faction.OfPlayer)
			{
				return null;
			}
			if (job.playerForced)
			{
				return null;
			}
			if (this.pawn.WorkTagIsDisabled(WorkTags.ManualDumb | WorkTags.Hauling | WorkTags.AllWork))
			{
				return null;
			}
			int num;
			if (ModsConfig.BiotechActive && this.pawn.IsWorkTypeDisabledByAge(WorkTypeDefOf.Hauling, out num))
			{
				return null;
			}
			float num2 = this.pawn.Position.DistanceTo(cell);
			if (num2 < 3f)
			{
				return null;
			}
			if (!this.pawn.Spawned)
			{
				return null;
			}
			foreach (Thing thing in this.pawn.Map.listerHaulables.ThingsPotentiallyNeedingHauling())
			{
				float num3 = this.pawn.Position.DistanceTo(thing.Position);
				if (num3 <= 30f && num3 <= num2 * 0.5f && num3 + thing.Position.DistanceTo(cell) <= num2 * 1.7f && this.pawn.Map.reservationManager.FirstRespectedReserver(thing, this.pawn, null) == null && !thing.IsForbidden(this.pawn) && HaulAIUtility.PawnCanAutomaticallyHaulFast_NewTemp(this.pawn, thing, false, true))
				{
					StoragePriority currentPriority = StoreUtility.CurrentStoragePriorityOf(thing, job.playerForced);
					IntVec3 intVec;
					IHaulDestination haulDestination;
					if (StoreUtility.TryFindBestBetterStorageFor(thing, this.pawn, this.pawn.Map, currentPriority, this.pawn.Faction, out intVec, out haulDestination, true))
					{
						IntVec3 intVec2;
						if (haulDestination is ISlotGroupParent)
						{
							intVec2 = intVec;
						}
						else
						{
							Thing thing2 = haulDestination as Thing;
							if (thing2 == null || thing2.TryGetInnerInteractableThingOwner() == null)
							{
								Log.Error("Don't know how to handle opportunistic hauling for Storage: " + haulDestination.ToStringSafe<IHaulDestination>() + ", Thing: " + thing.ToStringSafe<Thing>());
								continue;
							}
							intVec2 = thing2.Position;
						}
						float num4 = intVec2.DistanceTo(cell);
						if (num4 <= 50f && num4 <= num2 * 0.6f && num3 + thing.Position.DistanceTo(intVec2) + num4 <= num2 * 1.7f && num3 + num4 <= num2 && this.pawn.Position.WithinRegions(thing.Position, this.pawn.Map, 25, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), RegionType.Set_Passable) && intVec2.WithinRegions(cell, this.pawn.Map, 25, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), RegionType.Set_Passable))
						{
							if (DebugViewSettings.drawOpportunisticJobs)
							{
								Log.Message("Opportunistic job spawned");
								this.pawn.Map.debugDrawer.FlashLine(this.pawn.Position, thing.Position, 600, SimpleColor.Red);
								this.pawn.Map.debugDrawer.FlashLine(thing.Position, intVec2, 600, SimpleColor.Green);
								this.pawn.Map.debugDrawer.FlashLine(intVec2, cell, 600, SimpleColor.Blue);
							}
							if (haulDestination is ISlotGroupParent)
							{
								return HaulAIUtility.HaulToCellStorageJob(this.pawn, thing, intVec, false);
							}
							Thing thing3 = haulDestination as Thing;
							if (thing3 != null)
							{
								return HaulAIUtility.HaulToContainerJob(this.pawn, thing, thing3);
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06004507 RID: 17671 RVA: 0x001B23F0 File Offset: 0x001B05F0
		private ThinkResult DetermineNextJob(out ThinkTreeDef thinkTree, bool ignoreQueue = false)
		{
			if (this.determiningNextJob)
			{
				thinkTree = null;
				return ThinkResult.NoJob;
			}
			this.determiningNextJob = true;
			ThinkResult result = this.DetermineNextConstantThinkTreeJob();
			if (result.Job != null)
			{
				thinkTree = this.pawn.thinker.ConstantThinkTree;
				this.determiningNextJob = false;
				return result;
			}
			ThinkResult result2;
			try
			{
				result2 = this.pawn.thinker.MainThinkNodeRoot.TryIssueJobPackage(this.pawn, new JobIssueParams
				{
					ignoreQueue = ignoreQueue
				});
			}
			catch (Exception exception)
			{
				JobUtility.TryStartErrorRecoverJob(this.pawn, this.pawn.ToStringSafe<Pawn>() + " threw exception while determining job (main)", exception, null);
				thinkTree = null;
				this.determiningNextJob = false;
				return ThinkResult.NoJob;
			}
			finally
			{
			}
			thinkTree = this.pawn.thinker.MainThinkTree;
			this.determiningNextJob = false;
			return result2;
		}

		// Token: 0x06004508 RID: 17672 RVA: 0x001B24E0 File Offset: 0x001B06E0
		private ThinkResult DetermineNextConstantThinkTreeJob()
		{
			Pawn_Thinker thinker = this.pawn.thinker;
			if (((thinker != null) ? thinker.ConstantThinkTree : null) == null)
			{
				return ThinkResult.NoJob;
			}
			try
			{
				return this.pawn.thinker.ConstantThinkNodeRoot.TryIssueJobPackage(this.pawn, default(JobIssueParams));
			}
			catch (Exception exception)
			{
				JobUtility.TryStartErrorRecoverJob(this.pawn, this.pawn.ToStringSafe<Pawn>() + " threw exception while determining job (constant)", exception, null);
			}
			finally
			{
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x06004509 RID: 17673 RVA: 0x001B2580 File Offset: 0x001B0780
		private void CheckLeaveJoinableLordBecauseJobIssued(ThinkResult result)
		{
			if (!result.IsValid || result.SourceNode == null)
			{
				return;
			}
			Lord lord = this.pawn.GetLord();
			if (lord == null || !(lord.LordJob is LordJob_VoluntarilyJoinable))
			{
				return;
			}
			bool flag = false;
			ThinkNode thinkNode = result.SourceNode;
			while (!thinkNode.leaveJoinableLordIfIssuesJob)
			{
				thinkNode = thinkNode.parent;
				if (thinkNode == null)
				{
					IL_50:
					if (flag)
					{
						lord.Notify_PawnLost(this.pawn, PawnLostCondition.LeftVoluntarily, null);
					}
					return;
				}
			}
			flag = true;
			goto IL_50;
		}

		// Token: 0x0600450A RID: 17674 RVA: 0x001B25F8 File Offset: 0x001B07F8
		private bool ShouldStartJobFromThinkTree(ThinkResult thinkResult)
		{
			return this.curJob == null || (this.curJob != thinkResult.Job && (thinkResult.FromQueue || thinkResult.Job.def != this.curJob.def || !this.curDriver.IsContinuation(thinkResult.Job) || thinkResult.SourceNode != this.curJob.jobGiver));
		}

		// Token: 0x0600450B RID: 17675 RVA: 0x001B266F File Offset: 0x001B086F
		public void SetFormingCaravanTick(bool clear = false)
		{
			this.startedFormingCaravanTickInt = (clear ? -1 : Find.TickManager.TicksGame);
		}

		// Token: 0x0600450C RID: 17676 RVA: 0x001B2688 File Offset: 0x001B0888
		public bool IsCurrentJobPlayerInterruptible()
		{
			if (this.curJob != null)
			{
				if (!this.curJob.def.playerInterruptible)
				{
					return false;
				}
				if (this.curDriver != null && !this.curDriver.PlayerInterruptable)
				{
					return false;
				}
			}
			return !this.pawn.HasAttachment(ThingDefOf.Fire);
		}

		// Token: 0x0600450D RID: 17677 RVA: 0x001B26E0 File Offset: 0x001B08E0
		public bool TryTakeOrderedJobPrioritizedWork(Job job, WorkGiver giver, IntVec3 cell)
		{
			if (this.TryTakeOrderedJob(job, new JobTag?(giver.def.tagToGive), false))
			{
				job.workGiverDef = giver.def;
				if (giver.def.prioritizeSustains)
				{
					this.pawn.mindState.priorityWork.Set(cell, giver.def);
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600450E RID: 17678 RVA: 0x001B2740 File Offset: 0x001B0940
		public bool TryTakeOrderedJob(Job job, JobTag? tag = 0, bool requestQueueing = false)
		{
			if (this.debugLog)
			{
				this.DebugLogEvent("TryTakeOrderedJob " + ((job != null) ? job.ToString() : null));
			}
			job.playerForced = true;
			if (this.curJob != null && this.curJob.JobIsSameAs(this.pawn, job))
			{
				return true;
			}
			bool flag = this.pawn.jobs.IsCurrentJobPlayerInterruptible();
			Job job2 = this.pawn.CurJob;
			bool flag2 = job2 != null && job2.def.forceCompleteBeforeNextJob;
			bool flag3 = flag && !flag2;
			bool flag4 = this.pawn.mindState.IsIdle || this.pawn.CurJob == null || this.pawn.CurJob.def.isIdle;
			bool flag5 = KeyBindingDefOf.QueueOrder.IsDownEvent;
			if (flag5)
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.QueueOrders, KnowledgeAmount.NoteTaught);
			}
			flag5 = (flag5 || requestQueueing);
			if (flag3 && (!flag5 || flag4))
			{
				if (this.curJob != null)
				{
					this.curJob.playerInterruptedForced = true;
				}
				this.pawn.stances.CancelBusyStanceSoft();
				if (this.debugLog)
				{
					this.DebugLogEvent("    Queueing job");
				}
				this.ClearQueuedJobs(true);
				if (job.TryMakePreToilReservations(this.pawn, true))
				{
					this.jobQueue.EnqueueFirst(job, tag);
					if (this.curJob != null)
					{
						this.curDriver.EndJobWith(JobCondition.InterruptForced);
					}
					else
					{
						this.CheckForJobOverride(0f, false);
					}
					return true;
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(130, 2);
				defaultInterpolatedStringHandler.AppendLiteral("TryMakePreToilReservations() returned false right after TryTakeOrderedJob(). This should have been checked before. pawn = ");
				defaultInterpolatedStringHandler.AppendFormatted<Pawn>(this.pawn);
				defaultInterpolatedStringHandler.AppendLiteral(", job = ");
				defaultInterpolatedStringHandler.AppendFormatted(job.ToStringSafe<Job>());
				Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				this.pawn.ClearReservationsForJob(job);
				return false;
			}
			else if (flag5)
			{
				if (job.TryMakePreToilReservations(this.pawn, true))
				{
					this.jobQueue.EnqueueLast(job, tag);
					return true;
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(130, 2);
				defaultInterpolatedStringHandler2.AppendLiteral("TryMakePreToilReservations() returned false right after TryTakeOrderedJob(). This should have been checked before. pawn = ");
				defaultInterpolatedStringHandler2.AppendFormatted<Pawn>(this.pawn);
				defaultInterpolatedStringHandler2.AppendLiteral(", job = ");
				defaultInterpolatedStringHandler2.AppendFormatted(job.ToStringSafe<Job>());
				Log.Warning(defaultInterpolatedStringHandler2.ToStringAndClear());
				this.pawn.ClearReservationsForJob(job);
				return false;
			}
			else
			{
				this.ClearQueuedJobs(true);
				if (job.TryMakePreToilReservations(this.pawn, true))
				{
					this.jobQueue.EnqueueLast(job, tag);
					return true;
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(130, 2);
				defaultInterpolatedStringHandler3.AppendLiteral("TryMakePreToilReservations() returned false right after TryTakeOrderedJob(). This should have been checked before. pawn = ");
				defaultInterpolatedStringHandler3.AppendFormatted<Pawn>(this.pawn);
				defaultInterpolatedStringHandler3.AppendLiteral(", job = ");
				defaultInterpolatedStringHandler3.AppendFormatted(job.ToStringSafe<Job>());
				Log.Warning(defaultInterpolatedStringHandler3.ToStringAndClear());
				this.pawn.ClearReservationsForJob(job);
				return false;
			}
		}

		// Token: 0x0600450F RID: 17679 RVA: 0x001B2A04 File Offset: 0x001B0C04
		public void Notify_TuckedIntoBed(Building_Bed bed)
		{
			this.pawn.Position = RestUtility.GetBedSleepingSlotPosFor(this.pawn, bed);
			this.pawn.Notify_Teleported(false, true);
			this.pawn.stances.CancelBusyStanceHard();
			JobDef jobDef = this.pawn.Deathresting ? JobDefOf.Deathrest : JobDefOf.LayDown;
			this.StartJob(JobMaker.MakeJob(jobDef, bed), JobCondition.InterruptForced, null, false, true, null, new JobTag?(JobTag.TuckedIntoBed), false, false, null, true, true, false);
			if (jobDef == JobDefOf.Deathrest)
			{
				Pawn_GeneTracker genes = this.pawn.genes;
				if (genes == null)
				{
					return;
				}
				Gene_Deathrest firstGeneOfType = genes.GetFirstGeneOfType<Gene_Deathrest>();
				if (firstGeneOfType == null)
				{
					return;
				}
				firstGeneOfType.TryLinkToNearbyDeathrestBuildings();
			}
		}

		// Token: 0x06004510 RID: 17680 RVA: 0x001B2AB4 File Offset: 0x001B0CB4
		public void Notify_DamageTaken(DamageInfo dinfo)
		{
			if (this.curJob == null)
			{
				return;
			}
			Job job = this.curJob;
			this.curDriver.Notify_DamageTaken(dinfo);
			if (this.curJob == job && dinfo.Def.ExternalViolenceFor(this.pawn) && dinfo.Def.canInterruptJobs && !this.curJob.playerForced && Find.TickManager.TicksGame >= this.lastDamageCheckTick + 180)
			{
				Thing instigator = dinfo.Instigator;
				if (this.curJob.def.checkOverrideOnDamage == CheckJobOverrideOnDamageMode.Always || (this.curJob.def.checkOverrideOnDamage == CheckJobOverrideOnDamageMode.OnlyIfInstigatorNotJobTarget && !this.curJob.AnyTargetIs(instigator)))
				{
					this.lastDamageCheckTick = Find.TickManager.TicksGame;
					this.CheckForJobOverride(0f, true);
				}
			}
		}

		// Token: 0x06004511 RID: 17681 RVA: 0x001B2B94 File Offset: 0x001B0D94
		internal void Notify_MasterDraftedOrUndrafted()
		{
			Pawn master = this.pawn.playerSettings.Master;
			if (master.Spawned && master.Map == this.pawn.Map && this.pawn.playerSettings.followDrafted)
			{
				this.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		// Token: 0x06004512 RID: 17682 RVA: 0x001B2BEC File Offset: 0x001B0DEC
		public void Notify_MasterStartedFieldWork()
		{
			Pawn master = this.pawn.playerSettings.Master;
			if (master.Spawned && master.Map == this.pawn.Map && this.pawn.playerSettings.followFieldwork && this.IsCurrentJobPlayerInterruptible())
			{
				this.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		// Token: 0x06004513 RID: 17683 RVA: 0x001B2C4C File Offset: 0x001B0E4C
		public void DrawLinesBetweenTargets()
		{
			Vector3 a = this.pawn.Position.ToVector3Shifted();
			if (this.pawn.pather.curPath != null)
			{
				a = this.pawn.pather.Destination.CenterVector3;
			}
			else if (this.curJob != null && this.curJob.def != JobDefOf.LayDown && this.curJob.targetA.IsValid && (!this.curJob.targetA.HasThing || (this.curJob.targetA.Thing.Spawned && this.curJob.targetA.Thing.Map == this.pawn.Map)))
			{
				GenDraw.DrawLineBetween(a, this.curJob.targetA.CenterVector3, AltitudeLayer.Item.AltitudeFor());
				a = this.curJob.targetA.CenterVector3;
			}
			for (int i = 0; i < this.jobQueue.Count; i++)
			{
				if (this.jobQueue[i].job.targetA.IsValid)
				{
					if (!this.jobQueue[i].job.targetA.HasThing || (this.jobQueue[i].job.targetA.Thing.Spawned && this.jobQueue[i].job.targetA.Thing.Map == this.pawn.Map))
					{
						Vector3 centerVector = this.jobQueue[i].job.targetA.CenterVector3;
						GenDraw.DrawLineBetween(a, centerVector, AltitudeLayer.Item.AltitudeFor());
						a = centerVector;
					}
				}
				else
				{
					List<LocalTargetInfo> targetQueueA = this.jobQueue[i].job.targetQueueA;
					if (targetQueueA != null)
					{
						for (int j = 0; j < targetQueueA.Count; j++)
						{
							if (!targetQueueA[j].HasThing || (targetQueueA[j].Thing.Spawned && targetQueueA[j].Thing.Map == this.pawn.Map))
							{
								Vector3 centerVector2 = targetQueueA[j].CenterVector3;
								GenDraw.DrawLineBetween(a, centerVector2, AltitudeLayer.Item.AltitudeFor());
								a = centerVector2;
							}
						}
					}
				}
			}
		}

		// Token: 0x06004514 RID: 17684 RVA: 0x001B2ED8 File Offset: 0x001B10D8
		public void DebugLogEvent(string s)
		{
			if (this.debugLog)
			{
				string[] array = new string[5];
				array[0] = Find.TickManager.TicksGame.ToString();
				array[1] = " ";
				int num = 2;
				Pawn pawn = this.pawn;
				array[num] = ((pawn != null) ? pawn.ToString() : null);
				array[3] = ": ";
				array[4] = s;
				Log.Message(string.Concat(array));
			}
		}

		// Token: 0x06004515 RID: 17685 RVA: 0x001B2F3B File Offset: 0x001B113B
		public void ClearDriver()
		{
			JobDriver jobDriver = this.curDriver;
			if (jobDriver != null)
			{
				jobDriver.ClearToils();
			}
			this.curDriver = null;
		}

		// Token: 0x06004516 RID: 17686 RVA: 0x001B2F58 File Offset: 0x001B1158
		private static string CurToilString(JobDriver curDriver)
		{
			if (curDriver == null)
			{
				return "null_driver";
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 2);
			defaultInterpolatedStringHandler.AppendFormatted(curDriver.CurToilString);
			defaultInterpolatedStringHandler.AppendLiteral(" at toils[");
			defaultInterpolatedStringHandler.AppendFormatted<int>(curDriver.CurToilIndex);
			defaultInterpolatedStringHandler.AppendLiteral("]");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04002CC5 RID: 11461
		protected Pawn pawn;

		// Token: 0x04002CC6 RID: 11462
		public Job curJob;

		// Token: 0x04002CC7 RID: 11463
		public JobDriver curDriver;

		// Token: 0x04002CC8 RID: 11464
		public JobQueue jobQueue = new JobQueue();

		// Token: 0x04002CC9 RID: 11465
		public PawnPosture posture;

		// Token: 0x04002CCA RID: 11466
		private int startedFormingCaravanTickInt = -1;

		// Token: 0x04002CCB RID: 11467
		public bool startingNewJob;

		// Token: 0x04002CCC RID: 11468
		private bool determiningNextJob;

		// Token: 0x04002CCD RID: 11469
		private int jobsGivenThisTick;

		// Token: 0x04002CCE RID: 11470
		private string jobsGivenThisTickTextual = "";

		// Token: 0x04002CCF RID: 11471
		private int lastJobGivenAtFrame = -1;

		// Token: 0x04002CD0 RID: 11472
		private int lastTickFinalized = -1;

		// Token: 0x04002CD1 RID: 11473
		private List<int> jobsGivenRecentTicks = new List<int>(10);

		// Token: 0x04002CD2 RID: 11474
		private List<string> jobsGivenRecentTicksTextual = new List<string>(10);

		// Token: 0x04002CD3 RID: 11475
		public bool debugLog;

		// Token: 0x04002CD4 RID: 11476
		private const int RecentJobQueueMaxLength = 10;

		// Token: 0x04002CD5 RID: 11477
		private const int MaxRecentJobs = 10;

		// Token: 0x04002CD6 RID: 11478
		private const int NearbyEnemyMaxRegions = 25;

		// Token: 0x04002CD7 RID: 11479
		private static readonly SimpleCurve DistanceIntervalCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 30f),
				true
			},
			{
				new CurvePoint(50f, 120f),
				true
			},
			{
				new CurvePoint(100f, 600f),
				true
			}
		};

		// Token: 0x04002CD8 RID: 11480
		private int lastDamageCheckTick = -99999;

		// Token: 0x04002CD9 RID: 11481
		private const int DamageCheckMinInterval = 180;
	}
}
