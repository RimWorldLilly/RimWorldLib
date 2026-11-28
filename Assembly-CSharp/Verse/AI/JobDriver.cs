using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000838 RID: 2104
	public abstract class JobDriver : IExposable, IJobEndable
	{
		// Token: 0x17000C9A RID: 3226
		// (get) Token: 0x060041F6 RID: 16886 RVA: 0x001A1609 File Offset: 0x0019F809
		public virtual Rot4 ForcedLayingRotation
		{
			get
			{
				return Rot4.Invalid;
			}
		}

		// Token: 0x17000C9B RID: 3227
		// (get) Token: 0x060041F7 RID: 16887 RVA: 0x001A1610 File Offset: 0x0019F810
		public virtual Vector3 ForcedBodyOffset
		{
			get
			{
				return Vector3.zero;
			}
		}

		// Token: 0x17000C9C RID: 3228
		// (get) Token: 0x060041F8 RID: 16888 RVA: 0x001A1618 File Offset: 0x0019F818
		protected Toil CurToil
		{
			get
			{
				if (this.curToilIndex < 0 || this.job == null || this.pawn.CurJob != this.job)
				{
					return null;
				}
				if (this.curToilIndex >= this.toils.Count)
				{
					string[] array = new string[8];
					int num = 0;
					Pawn pawn = this.pawn;
					array[num] = ((pawn != null) ? pawn.ToString() : null);
					array[1] = " with job ";
					int num2 = 2;
					Job curJob = this.pawn.CurJob;
					array[num2] = ((curJob != null) ? curJob.ToString() : null);
					array[3] = " tried to get CurToil with curToilIndex=";
					array[4] = this.curToilIndex.ToString();
					array[5] = " but only has ";
					array[6] = this.toils.Count.ToString();
					array[7] = " toils.";
					Log.Error(string.Concat(array));
					return null;
				}
				return this.toils[this.curToilIndex];
			}
		}

		// Token: 0x17000C9D RID: 3229
		// (get) Token: 0x060041F9 RID: 16889 RVA: 0x001A16F7 File Offset: 0x0019F8F7
		protected bool HaveCurToil
		{
			get
			{
				return this.curToilIndex >= 0 && this.curToilIndex < this.toils.Count && this.job != null && this.pawn.CurJob == this.job;
			}
		}

		// Token: 0x17000C9E RID: 3230
		// (get) Token: 0x060041FA RID: 16890 RVA: 0x001A1734 File Offset: 0x0019F934
		private bool CanStartNextToilInBusyStance
		{
			get
			{
				int num = this.curToilIndex + 1;
				return num < this.toils.Count && this.toils[num].atomicWithPrevious;
			}
		}

		// Token: 0x17000C9F RID: 3231
		// (get) Token: 0x060041FB RID: 16891 RVA: 0x001A176B File Offset: 0x0019F96B
		public int CurToilIndex
		{
			get
			{
				return this.curToilIndex;
			}
		}

		// Token: 0x17000CA0 RID: 3232
		// (get) Token: 0x060041FC RID: 16892 RVA: 0x001A1773 File Offset: 0x0019F973
		public string CurToilString
		{
			get
			{
				if (this.curToilIndex < this.toils.Count)
				{
					return this.CurToil.ToStringSafe<Toil>();
				}
				return "<end>";
			}
		}

		// Token: 0x17000CA1 RID: 3233
		// (get) Token: 0x060041FD RID: 16893 RVA: 0x001A1799 File Offset: 0x0019F999
		public bool OnLastToil
		{
			get
			{
				return this.CurToilIndex == this.toils.Count - 1;
			}
		}

		// Token: 0x17000CA2 RID: 3234
		// (get) Token: 0x060041FE RID: 16894 RVA: 0x001A17B0 File Offset: 0x0019F9B0
		public SkillDef ActiveSkill
		{
			get
			{
				if (!this.HaveCurToil || this.CurToil.activeSkill == null)
				{
					return null;
				}
				return this.CurToil.activeSkill();
			}
		}

		// Token: 0x17000CA3 RID: 3235
		// (get) Token: 0x060041FF RID: 16895 RVA: 0x001A17D9 File Offset: 0x0019F9D9
		public bool HandlingFacing
		{
			get
			{
				return this.CurToil != null && this.CurToil.handlingFacing;
			}
		}

		// Token: 0x17000CA4 RID: 3236
		// (get) Token: 0x06004200 RID: 16896 RVA: 0x001A17F0 File Offset: 0x0019F9F0
		public virtual bool PlayerInterruptable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CA5 RID: 3237
		// (get) Token: 0x06004201 RID: 16897 RVA: 0x001A17F3 File Offset: 0x0019F9F3
		protected LocalTargetInfo TargetA
		{
			get
			{
				return this.job.targetA;
			}
		}

		// Token: 0x17000CA6 RID: 3238
		// (get) Token: 0x06004202 RID: 16898 RVA: 0x001A1800 File Offset: 0x0019FA00
		protected LocalTargetInfo TargetB
		{
			get
			{
				return this.job.targetB;
			}
		}

		// Token: 0x17000CA7 RID: 3239
		// (get) Token: 0x06004203 RID: 16899 RVA: 0x001A180D File Offset: 0x0019FA0D
		protected LocalTargetInfo TargetC
		{
			get
			{
				return this.job.targetC;
			}
		}

		// Token: 0x17000CA8 RID: 3240
		// (get) Token: 0x06004204 RID: 16900 RVA: 0x001A181A File Offset: 0x0019FA1A
		// (set) Token: 0x06004205 RID: 16901 RVA: 0x001A182C File Offset: 0x0019FA2C
		protected Thing TargetThingA
		{
			get
			{
				return this.job.targetA.Thing;
			}
			set
			{
				this.job.targetA = value;
			}
		}

		// Token: 0x17000CA9 RID: 3241
		// (get) Token: 0x06004206 RID: 16902 RVA: 0x001A183F File Offset: 0x0019FA3F
		// (set) Token: 0x06004207 RID: 16903 RVA: 0x001A1851 File Offset: 0x0019FA51
		protected Thing TargetThingB
		{
			get
			{
				return this.job.targetB.Thing;
			}
			set
			{
				this.job.targetB = value;
			}
		}

		// Token: 0x17000CAA RID: 3242
		// (get) Token: 0x06004208 RID: 16904 RVA: 0x001A1864 File Offset: 0x0019FA64
		// (set) Token: 0x06004209 RID: 16905 RVA: 0x001A1876 File Offset: 0x0019FA76
		protected Thing TargetThingC
		{
			get
			{
				return this.job.targetC.Thing;
			}
			set
			{
				this.job.targetC = value;
			}
		}

		// Token: 0x17000CAB RID: 3243
		// (get) Token: 0x0600420A RID: 16906 RVA: 0x001A1889 File Offset: 0x0019FA89
		// (set) Token: 0x0600420B RID: 16907 RVA: 0x001A18A0 File Offset: 0x0019FAA0
		protected Pawn TargetPawnA
		{
			get
			{
				return this.job.targetA.Thing as Pawn;
			}
			set
			{
				this.job.targetA = value;
			}
		}

		// Token: 0x17000CAC RID: 3244
		// (get) Token: 0x0600420C RID: 16908 RVA: 0x001A18B3 File Offset: 0x0019FAB3
		// (set) Token: 0x0600420D RID: 16909 RVA: 0x001A18CA File Offset: 0x0019FACA
		protected Pawn TargetPawnB
		{
			get
			{
				return this.job.targetB.Thing as Pawn;
			}
			set
			{
				this.job.targetB = value;
			}
		}

		// Token: 0x17000CAD RID: 3245
		// (get) Token: 0x0600420E RID: 16910 RVA: 0x001A18DD File Offset: 0x0019FADD
		// (set) Token: 0x0600420F RID: 16911 RVA: 0x001A18F4 File Offset: 0x0019FAF4
		protected Pawn TargetPawnC
		{
			get
			{
				return this.job.targetC.Thing as Pawn;
			}
			set
			{
				this.job.targetC = value;
			}
		}

		// Token: 0x17000CAE RID: 3246
		// (get) Token: 0x06004210 RID: 16912 RVA: 0x001A1907 File Offset: 0x0019FB07
		protected IntVec3 TargetLocA
		{
			get
			{
				return this.job.targetA.Cell;
			}
		}

		// Token: 0x17000CAF RID: 3247
		// (get) Token: 0x06004211 RID: 16913 RVA: 0x001A1919 File Offset: 0x0019FB19
		protected Map Map
		{
			get
			{
				return this.pawn.MapHeld;
			}
		}

		// Token: 0x06004212 RID: 16914 RVA: 0x001A1928 File Offset: 0x0019FB28
		public virtual string GetReport()
		{
			string reportStringOverride = this.GetReportStringOverride();
			if (!reportStringOverride.NullOrEmpty())
			{
				return reportStringOverride;
			}
			return this.ReportStringProcessed(this.job.def.reportString);
		}

		// Token: 0x06004213 RID: 16915 RVA: 0x001A195C File Offset: 0x0019FB5C
		public string GetReportStringOverride()
		{
			if (this.pawn.Crawling && !this.job.crawlingReportStringOverride.NullOrEmpty())
			{
				return this.job.crawlingReportStringOverride;
			}
			return this.job.reportStringOverride;
		}

		// Token: 0x06004214 RID: 16916 RVA: 0x001A1994 File Offset: 0x0019FB94
		protected virtual string ReportStringProcessed(string str)
		{
			LocalTargetInfo a = this.job.targetA.IsValid ? this.job.targetA : this.job.targetQueueA.FirstValid();
			LocalTargetInfo b = this.job.targetB.IsValid ? this.job.targetB : this.job.targetQueueB.FirstValid();
			LocalTargetInfo targetC = this.job.targetC;
			return JobUtility.GetResolvedJobReport(str, a, b, targetC);
		}

		// Token: 0x06004215 RID: 16917
		public abstract bool TryMakePreToilReservations(bool errorOnFailed);

		// Token: 0x06004216 RID: 16918
		protected abstract IEnumerable<Toil> MakeNewToils();

		// Token: 0x06004217 RID: 16919 RVA: 0x001A1A16 File Offset: 0x0019FC16
		public virtual void SetInitialPosture()
		{
			this.pawn.jobs.posture = PawnPosture.Standing;
			PortraitsCache.SetDirty(this.pawn);
		}

		// Token: 0x06004218 RID: 16920 RVA: 0x001A1A34 File Offset: 0x0019FC34
		public virtual void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.ended, "ended", false, false);
			Scribe_Values.Look<int>(ref this.curToilIndex, "curToilIndex", 0, true);
			Scribe_Values.Look<int>(ref this.ticksLeftThisToil, "ticksLeftThisToil", 0, false);
			Scribe_Values.Look<bool>(ref this.wantBeginNextToil, "wantBeginNextToil", false, false);
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
			Scribe_Values.Look<TargetIndex>(ref this.rotateToFace, "rotateToFace", TargetIndex.A, false);
			Scribe_Values.Look<bool>(ref this.asleep, "asleep", false, false);
			Scribe_Values.Look<float>(ref this.uninstallWorkLeft, "uninstallWorkLeft", 0f, false);
			Scribe_Values.Look<int>(ref this.nextToilIndex, "nextToilIndex", -1, false);
			Scribe_Values.Look<bool>(ref this.collideWithPawns, "collideWithPawns", false, false);
			Scribe_References.Look<Pawn>(ref this.locomotionUrgencySameAs, "locomotionUrgencySameAs", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.SetupToils();
			}
		}

		// Token: 0x06004219 RID: 16921 RVA: 0x001A1B18 File Offset: 0x0019FD18
		public void Cleanup(JobCondition condition)
		{
			for (int i = 0; i < this.globalFinishActions.Count; i++)
			{
				try
				{
					this.globalFinishActions[i](condition);
				}
				catch (Exception ex)
				{
					string[] array = new string[10];
					array[0] = "Pawn ";
					array[1] = this.pawn.ToStringSafe<Pawn>();
					array[2] = " threw exception while executing a global finish action (";
					array[3] = i.ToString();
					array[4] = "), jobDriver=";
					array[5] = this.ToStringSafe<JobDriver>();
					array[6] = ", job=";
					array[7] = this.job.ToStringSafe<Job>();
					array[8] = ": ";
					int num = 9;
					Exception ex2 = ex;
					array[num] = ((ex2 != null) ? ex2.ToString() : null);
					Log.Error(string.Concat(array));
				}
			}
			if (this.curToilIndex >= 0 && this.curToilIndex < this.toils.Count)
			{
				this.toils[this.curToilIndex].Cleanup(this.curToilIndex, this);
			}
		}

		// Token: 0x0600421A RID: 16922 RVA: 0x001A1C1C File Offset: 0x0019FE1C
		protected void SetFinalizerJob(Func<JobCondition, Job> finalizerJobFactory)
		{
			this.finalizerJobFactory = finalizerJobFactory;
		}

		// Token: 0x0600421B RID: 16923 RVA: 0x001A1C25 File Offset: 0x0019FE25
		public Job GetFinalizerJob(JobCondition condition)
		{
			Func<JobCondition, Job> func = this.finalizerJobFactory;
			if (func == null)
			{
				return null;
			}
			return func(condition);
		}

		// Token: 0x0600421C RID: 16924 RVA: 0x001A1C39 File Offset: 0x0019FE39
		public virtual bool CanBeginNowWhileLyingDown()
		{
			return false;
		}

		// Token: 0x0600421D RID: 16925 RVA: 0x001A1C3C File Offset: 0x0019FE3C
		internal void SetupToils()
		{
			try
			{
				this.toils.Clear();
				foreach (Toil toil in this.MakeNewToils())
				{
					if (toil.defaultCompleteMode == ToilCompleteMode.Undefined)
					{
						Log.Error("Toil has undefined complete mode.");
						toil.defaultCompleteMode = ToilCompleteMode.Instant;
					}
					toil.actor = this.pawn;
					this.toils.Add(toil);
				}
			}
			catch (Exception exception)
			{
				JobUtility.TryStartErrorRecoverJob(this.pawn, "Exception in SetupToils for pawn " + this.pawn.ToStringSafe<Pawn>(), exception, this);
			}
		}

		// Token: 0x0600421E RID: 16926 RVA: 0x001A1CF4 File Offset: 0x0019FEF4
		public void ClearToils()
		{
			foreach (Toil toil in this.toils)
			{
				if (!toil.inPool)
				{
					toil.ReturnToPool();
				}
			}
			this.toils.Clear();
		}

		// Token: 0x0600421F RID: 16927 RVA: 0x001A1D5C File Offset: 0x0019FF5C
		public void DriverTick()
		{
			try
			{
				this.ticksLeftThisToil--;
				this.debugTicksSpentThisToil++;
				if (this.CurToil == null)
				{
					if (!this.pawn.stances.FullBodyBusy || this.CanStartNextToilInBusyStance)
					{
						this.ReadyForNextToil();
					}
				}
				else if (!this.CheckCurrentToilEndOrFail())
				{
					if (this.CurToil.defaultCompleteMode == ToilCompleteMode.Instant)
					{
						this.ReadyForNextToil();
					}
					else
					{
						if (this.CurToil.defaultCompleteMode == ToilCompleteMode.Delay)
						{
							if (this.ticksLeftThisToil <= 0)
							{
								this.ReadyForNextToil();
								return;
							}
						}
						else if (this.CurToil.defaultCompleteMode == ToilCompleteMode.FinishedBusy && !this.pawn.stances.FullBodyBusy)
						{
							this.ReadyForNextToil();
							return;
						}
						if (this.wantBeginNextToil)
						{
							this.TryActuallyStartNextToil();
						}
						else
						{
							JobDriver.<>c__DisplayClass82_0 CS$<>8__locals1;
							CS$<>8__locals1.startingJob = this.pawn.CurJob;
							CS$<>8__locals1.startingJobId = CS$<>8__locals1.startingJob.loadID;
							if (this.CurToil.preTickActions != null)
							{
								Toil curToil = this.CurToil;
								for (int i = 0; i < this.CurToil.preTickActions.Count; i++)
								{
									this.CurToil.preTickActions[i]();
									if (this.<DriverTick>g__JobChanged|82_0(ref CS$<>8__locals1))
									{
										return;
									}
									if (this.CurToil != curToil || this.wantBeginNextToil)
									{
										return;
									}
								}
							}
							if (this.CurToil.tickAction != null)
							{
								this.CurToil.tickAction();
								if (this.<DriverTick>g__JobChanged|82_0(ref CS$<>8__locals1))
								{
									return;
								}
							}
							if (this.job.mote != null)
							{
								this.job.mote.Maintain();
							}
						}
					}
				}
			}
			catch (Exception exception)
			{
				JobUtility.TryStartErrorRecoverJob(this.pawn, "Exception in JobDriver fixed tick for pawn " + this.pawn.ToStringSafe<Pawn>(), exception, this);
			}
		}

		// Token: 0x06004220 RID: 16928 RVA: 0x001A1F50 File Offset: 0x001A0150
		public void DriverTickInterval(int delta)
		{
			try
			{
				if (!this.wantBeginNextToil)
				{
					JobDriver.<>c__DisplayClass83_0 CS$<>8__locals1;
					CS$<>8__locals1.startingJob = this.pawn.CurJob;
					CS$<>8__locals1.startingJobId = CS$<>8__locals1.startingJob.loadID;
					if (this.CurToil.preTickIntervalActions != null)
					{
						Toil curToil = this.CurToil;
						for (int i = 0; i < curToil.preTickIntervalActions.Count; i++)
						{
							curToil.preTickIntervalActions[i](delta);
							if (this.<DriverTickInterval>g__JobChanged|83_0(ref CS$<>8__locals1))
							{
								return;
							}
							if (this.CurToil != curToil || this.wantBeginNextToil)
							{
								return;
							}
						}
					}
					if (this.CurToil.tickIntervalAction != null)
					{
						this.CurToil.tickIntervalAction(delta);
					}
				}
			}
			catch (Exception exception)
			{
				JobUtility.TryStartErrorRecoverJob(this.pawn, "Exception in JobDriver tick for pawn " + this.pawn.ToStringSafe<Pawn>(), exception, this);
			}
		}

		// Token: 0x06004221 RID: 16929 RVA: 0x001A2040 File Offset: 0x001A0240
		public void ReadyForNextToil()
		{
			this.wantBeginNextToil = true;
			this.TryActuallyStartNextToil();
		}

		// Token: 0x06004222 RID: 16930 RVA: 0x001A2050 File Offset: 0x001A0250
		private void TryActuallyStartNextToil()
		{
			if (this.pawn.stances.FullBodyBusy && !this.CanStartNextToilInBusyStance)
			{
				return;
			}
			if (this.job == null || this.pawn.CurJob != this.job)
			{
				return;
			}
			if (this.HaveCurToil)
			{
				this.CurToil.Cleanup(this.curToilIndex, this);
			}
			if (this.nextToilIndex >= 0)
			{
				this.curToilIndex = this.nextToilIndex;
				this.nextToilIndex = -1;
			}
			else
			{
				this.curToilIndex++;
			}
			this.wantBeginNextToil = false;
			if (!this.HaveCurToil)
			{
				if (this.pawn.stances != null && this.pawn.stances.curStance.StanceBusy)
				{
					Log.ErrorOnce(this.pawn.ToStringSafe<Pawn>() + " ended job " + this.job.ToStringSafe<Job>() + " due to running out of toils during a busy stance.", 6453432);
				}
				this.EndJobWith(JobCondition.Succeeded);
				return;
			}
			this.debugTicksSpentThisToil = 0;
			this.ticksLeftThisToil = this.CurToil.defaultDuration;
			if (!this.CheckCurrentToilEndOrFail())
			{
				Toil curToil = this.CurToil;
				if (this.CurToil.preInitActions != null)
				{
					for (int i = 0; i < this.CurToil.preInitActions.Count; i++)
					{
						try
						{
							this.CurToil.preInitActions[i]();
						}
						catch (Exception exception)
						{
							Pawn pawn = this.pawn;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(63, 3);
							defaultInterpolatedStringHandler.AppendLiteral("JobDriver threw exception in toil ");
							defaultInterpolatedStringHandler.AppendFormatted(this.CurToil.ToStringSafe<Toil>());
							defaultInterpolatedStringHandler.AppendLiteral("'s preInitActions[");
							defaultInterpolatedStringHandler.AppendFormatted<int>(i);
							defaultInterpolatedStringHandler.AppendLiteral("] for pawn ");
							defaultInterpolatedStringHandler.AppendFormatted(this.pawn.ToStringSafe<Pawn>());
							JobUtility.TryStartErrorRecoverJob(pawn, defaultInterpolatedStringHandler.ToStringAndClear(), exception, this);
							return;
						}
						if (this.CurToil != curToil)
						{
							break;
						}
					}
				}
				if (this.CurToil == curToil)
				{
					if (this.CurToil.initAction != null)
					{
						try
						{
							this.CurToil.initAction();
						}
						catch (Exception exception2)
						{
							JobUtility.TryStartErrorRecoverJob(this.pawn, "JobDriver threw exception in toil " + this.CurToil.ToStringSafe<Toil>() + "'s initAction for pawn " + this.pawn.ToStringSafe<Pawn>(), exception2, this);
							return;
						}
					}
					if (!this.ended && this.CurToil.defaultCompleteMode == ToilCompleteMode.Instant && this.CurToil == curToil)
					{
						this.ReadyForNextToil();
					}
				}
			}
		}

		// Token: 0x06004223 RID: 16931 RVA: 0x001A22D8 File Offset: 0x001A04D8
		public void EndJobWith(JobCondition condition)
		{
			if (!this.pawn.Destroyed && this.job != null && this.pawn.CurJob == this.job)
			{
				this.pawn.jobs.EndCurrentJob(condition, true, true);
			}
		}

		// Token: 0x06004224 RID: 16932 RVA: 0x001A2315 File Offset: 0x001A0515
		public virtual object[] TaleParameters()
		{
			return new object[]
			{
				this.pawn
			};
		}

		// Token: 0x06004225 RID: 16933 RVA: 0x001A2328 File Offset: 0x001A0528
		private bool CheckCurrentToilEndOrFail()
		{
			bool result;
			try
			{
				Toil curToil = this.CurToil;
				if (this.globalFailConditions != null)
				{
					for (int i = 0; i < this.globalFailConditions.Count; i++)
					{
						JobCondition jobCondition = this.globalFailConditions[i]();
						if (jobCondition != JobCondition.Ongoing)
						{
							if (this.pawn.jobs.debugLog)
							{
								this.pawn.jobs.DebugLogEvent(string.Concat(new string[]
								{
									base.GetType().Name,
									" ends current job ",
									this.job.ToStringSafe<Job>(),
									" because of globalFailConditions[",
									i.ToString(),
									"]"
								}));
							}
							this.EndJobWith(jobCondition);
							return true;
						}
					}
				}
				if (curToil != null && curToil.endConditions != null)
				{
					for (int j = 0; j < curToil.endConditions.Count; j++)
					{
						JobCondition jobCondition2 = curToil.endConditions[j]();
						if (jobCondition2 != JobCondition.Ongoing)
						{
							if (this.pawn.jobs.debugLog)
							{
								Pawn_JobTracker jobs = this.pawn.jobs;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 5);
								defaultInterpolatedStringHandler.AppendFormatted(base.GetType().Name);
								defaultInterpolatedStringHandler.AppendLiteral(" ends current job ");
								defaultInterpolatedStringHandler.AppendFormatted(this.job.ToStringSafe<Job>());
								defaultInterpolatedStringHandler.AppendLiteral(" because of toil ");
								defaultInterpolatedStringHandler.AppendFormatted(curToil.ToStringSafe<Toil>());
								defaultInterpolatedStringHandler.AppendLiteral(" at toils[");
								defaultInterpolatedStringHandler.AppendFormatted<int>(this.curToilIndex);
								defaultInterpolatedStringHandler.AppendLiteral("].endConditions[");
								defaultInterpolatedStringHandler.AppendFormatted<int>(j);
								defaultInterpolatedStringHandler.AppendLiteral("]");
								jobs.DebugLogEvent(defaultInterpolatedStringHandler.ToStringAndClear());
							}
							this.EndJobWith(jobCondition2);
							return true;
						}
					}
				}
				result = false;
			}
			catch (Exception exception)
			{
				JobUtility.TryStartErrorRecoverJob(this.pawn, "Exception in CheckCurrentToilEndOrFail for pawn " + this.pawn.ToStringSafe<Pawn>(), exception, this);
				result = true;
			}
			return result;
		}

		// Token: 0x06004226 RID: 16934 RVA: 0x001A2550 File Offset: 0x001A0750
		private void SetNextToil(Toil to)
		{
			if (to != null && !this.toils.Contains(to))
			{
				Log.Warning(string.Concat(new string[]
				{
					"SetNextToil with non-existent toil (",
					to.ToStringSafe<Toil>(),
					"). pawn=",
					this.pawn.ToStringSafe<Pawn>(),
					", job=",
					this.pawn.CurJob.ToStringSafe<Job>()
				}));
			}
			this.nextToilIndex = this.toils.IndexOf(to);
		}

		// Token: 0x06004227 RID: 16935 RVA: 0x001A25D4 File Offset: 0x001A07D4
		public void JumpToToil(Toil to)
		{
			if (to == null)
			{
				Log.Warning("JumpToToil with null toil. pawn=" + this.pawn.ToStringSafe<Pawn>() + ", job=" + this.pawn.CurJob.ToStringSafe<Job>());
			}
			this.SetNextToil(to);
			this.ReadyForNextToil();
		}

		// Token: 0x06004228 RID: 16936 RVA: 0x001A2620 File Offset: 0x001A0820
		public virtual void Notify_Starting()
		{
			this.startTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06004229 RID: 16937 RVA: 0x001A2632 File Offset: 0x001A0832
		public virtual void Notify_PatherArrived()
		{
			if (this.CurToil == null)
			{
				return;
			}
			if (this.CurToil.defaultCompleteMode == ToilCompleteMode.PatherArrival)
			{
				this.ReadyForNextToil();
			}
		}

		// Token: 0x0600422A RID: 16938 RVA: 0x001A2651 File Offset: 0x001A0851
		public virtual void Notify_PatherFailed()
		{
			this.EndJobWith(JobCondition.ErroredPather);
		}

		// Token: 0x0600422B RID: 16939 RVA: 0x001A265E File Offset: 0x001A085E
		public virtual void Notify_StanceChanged()
		{
		}

		// Token: 0x0600422C RID: 16940 RVA: 0x001A2660 File Offset: 0x001A0860
		public virtual void Notify_DamageTaken(DamageInfo dinfo)
		{
		}

		// Token: 0x0600422D RID: 16941 RVA: 0x001A2662 File Offset: 0x001A0862
		public Pawn GetActor()
		{
			return this.pawn;
		}

		// Token: 0x0600422E RID: 16942 RVA: 0x001A266A File Offset: 0x001A086A
		public void AddEndCondition(Func<JobCondition> newEndCondition)
		{
			this.globalFailConditions.Add(newEndCondition);
		}

		// Token: 0x0600422F RID: 16943 RVA: 0x001A2678 File Offset: 0x001A0878
		public void AddFailCondition(Func<bool> newFailCondition)
		{
			this.globalFailConditions.Add(delegate
			{
				if (newFailCondition())
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
		}

		// Token: 0x06004230 RID: 16944 RVA: 0x001A26A9 File Offset: 0x001A08A9
		public void AddFinishAction(Action<JobCondition> newAct)
		{
			this.globalFinishActions.Add(newAct);
		}

		// Token: 0x06004231 RID: 16945 RVA: 0x001A26B7 File Offset: 0x001A08B7
		public virtual bool ModifyCarriedThingDrawPos(ref Vector3 drawPos, ref bool flip)
		{
			return false;
		}

		// Token: 0x06004232 RID: 16946 RVA: 0x001A26BA File Offset: 0x001A08BA
		public virtual RandomSocialMode DesiredSocialMode()
		{
			if (this.CurToil != null)
			{
				return this.CurToil.socialMode;
			}
			return RandomSocialMode.Normal;
		}

		// Token: 0x06004233 RID: 16947 RVA: 0x001A26D1 File Offset: 0x001A08D1
		public virtual bool IsContinuation(Job j)
		{
			return true;
		}

		// Token: 0x06004234 RID: 16948 RVA: 0x001A26D4 File Offset: 0x001A08D4
		public virtual bool? IsSameJobAs(Job j)
		{
			return null;
		}

		// Token: 0x06004236 RID: 16950 RVA: 0x001A2747 File Offset: 0x001A0947
		[CompilerGenerated]
		private bool <DriverTick>g__JobChanged|82_0(ref JobDriver.<>c__DisplayClass82_0 A_1)
		{
			return this.pawn.CurJob != A_1.startingJob || this.pawn.CurJob.loadID != A_1.startingJobId;
		}

		// Token: 0x06004237 RID: 16951 RVA: 0x001A2779 File Offset: 0x001A0979
		[CompilerGenerated]
		private bool <DriverTickInterval>g__JobChanged|83_0(ref JobDriver.<>c__DisplayClass83_0 A_1)
		{
			return this.pawn.CurJob != A_1.startingJob || this.pawn.CurJob.loadID != A_1.startingJobId;
		}

		// Token: 0x04002B3E RID: 11070
		public const int PlaceInInventoryDuration = 25;

		// Token: 0x04002B3F RID: 11071
		public Pawn pawn;

		// Token: 0x04002B40 RID: 11072
		public Job job;

		// Token: 0x04002B41 RID: 11073
		private List<Toil> toils = new List<Toil>();

		// Token: 0x04002B42 RID: 11074
		public List<Func<JobCondition>> globalFailConditions = new List<Func<JobCondition>>();

		// Token: 0x04002B43 RID: 11075
		public List<Action<JobCondition>> globalFinishActions = new List<Action<JobCondition>>();

		// Token: 0x04002B44 RID: 11076
		private Func<JobCondition, Job> finalizerJobFactory;

		// Token: 0x04002B45 RID: 11077
		public bool ended;

		// Token: 0x04002B46 RID: 11078
		private int curToilIndex = -1;

		// Token: 0x04002B47 RID: 11079
		public int ticksLeftThisToil = 99999;

		// Token: 0x04002B48 RID: 11080
		private bool wantBeginNextToil;

		// Token: 0x04002B49 RID: 11081
		protected int startTick = -1;

		// Token: 0x04002B4A RID: 11082
		public TargetIndex rotateToFace = TargetIndex.A;

		// Token: 0x04002B4B RID: 11083
		private int nextToilIndex = -1;

		// Token: 0x04002B4C RID: 11084
		public bool asleep;

		// Token: 0x04002B4D RID: 11085
		public float uninstallWorkLeft;

		// Token: 0x04002B4E RID: 11086
		public bool collideWithPawns;

		// Token: 0x04002B4F RID: 11087
		public Pawn locomotionUrgencySameAs;

		// Token: 0x04002B50 RID: 11088
		public int debugTicksSpentThisToil;
	}
}
