using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001583 RID: 5507
	public class Need_Rest : Need
	{
		// Token: 0x1700170B RID: 5899
		// (get) Token: 0x060086F5 RID: 34549 RVA: 0x0030C6E2 File Offset: 0x0030A8E2
		public RestCategory CurCategory
		{
			get
			{
				if (this.CurLevel < 0.01f)
				{
					return RestCategory.Exhausted;
				}
				if (this.CurLevel < 0.14f)
				{
					return RestCategory.VeryTired;
				}
				if (this.CurLevel < 0.28f)
				{
					return RestCategory.Tired;
				}
				return RestCategory.Rested;
			}
		}

		// Token: 0x1700170C RID: 5900
		// (get) Token: 0x060086F6 RID: 34550 RVA: 0x0030C714 File Offset: 0x0030A914
		public float RestFallPerTick
		{
			get
			{
				switch (this.CurCategory)
				{
				case RestCategory.Rested:
					return 1.5833333E-05f * this.RestFallFactor;
				case RestCategory.Tired:
					return 1.5833333E-05f * this.RestFallFactor * 0.7f;
				case RestCategory.VeryTired:
					return 1.5833333E-05f * this.RestFallFactor * 0.3f;
				case RestCategory.Exhausted:
					return 1.5833333E-05f * this.RestFallFactor * 0.6f;
				default:
					return 999f;
				}
			}
		}

		// Token: 0x1700170D RID: 5901
		// (get) Token: 0x060086F7 RID: 34551 RVA: 0x0030C78B File Offset: 0x0030A98B
		private float RestFallFactor
		{
			get
			{
				return this.pawn.health.hediffSet.RestFallFactor;
			}
		}

		// Token: 0x1700170E RID: 5902
		// (get) Token: 0x060086F8 RID: 34552 RVA: 0x0030C7A2 File Offset: 0x0030A9A2
		public override int GUIChangeArrow
		{
			get
			{
				if (!this.Resting)
				{
					return -1;
				}
				return 1;
			}
		}

		// Token: 0x1700170F RID: 5903
		// (get) Token: 0x060086F9 RID: 34553 RVA: 0x0030C7AF File Offset: 0x0030A9AF
		public int TicksAtZero
		{
			get
			{
				return this.ticksAtZero;
			}
		}

		// Token: 0x17001710 RID: 5904
		// (get) Token: 0x060086FA RID: 34554 RVA: 0x0030C7B7 File Offset: 0x0030A9B7
		public bool Resting
		{
			get
			{
				return Find.TickManager.TicksGame <= this.lastRestTick + this.pawn.UpdateRateTicks;
			}
		}

		// Token: 0x060086FB RID: 34555 RVA: 0x0030C7DC File Offset: 0x0030A9DC
		public Need_Rest(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.28f);
			this.threshPercents.Add(0.14f);
		}

		// Token: 0x060086FC RID: 34556 RVA: 0x0030C831 File Offset: 0x0030AA31
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksAtZero, "ticksAtZero", 0, false);
		}

		// Token: 0x060086FD RID: 34557 RVA: 0x0030C84B File Offset: 0x0030AA4B
		public override void SetInitialLevel()
		{
			this.CurLevel = Rand.Range(0.9f, 1f);
		}

		// Token: 0x060086FE RID: 34558 RVA: 0x0030C864 File Offset: 0x0030AA64
		public override void NeedInterval()
		{
			if (!this.IsFrozen)
			{
				if (this.Resting)
				{
					float num = this.lastRestEffectiveness;
					num *= this.pawn.GetStatValue(StatDefOf.RestRateMultiplier, true, -1);
					if (num > 0f)
					{
						this.CurLevel += 0.005714286f * num;
					}
				}
				else
				{
					float num2 = this.RestFallPerTick * 150f * this.pawn.GetStatValue(StatDefOf.RestFallRateFactor, true, -1);
					this.CurLevel -= num2;
				}
			}
			if (this.CurLevel < 0.0001f)
			{
				this.ticksAtZero += 150;
			}
			else
			{
				this.ticksAtZero = 0;
			}
			if (Need_Rest.CanInvoluntarilySleep(this.pawn) && this.ShouldInvoluntarySleepFromMTB())
			{
				Building_Bed building_Bed = this.pawn.CurrentBed();
				LocalTargetInfo targetA;
				if (building_Bed != null)
				{
					targetA = building_Bed;
				}
				else
				{
					Thing spawnedParentOrMe = this.pawn.SpawnedParentOrMe;
					if (spawnedParentOrMe != null && spawnedParentOrMe != this.pawn)
					{
						targetA = spawnedParentOrMe;
					}
					else
					{
						targetA = this.pawn.Position;
					}
				}
				Job job = JobMaker.MakeJob(JobDefOf.LayDown, targetA);
				job.startInvoluntarySleep = true;
				this.pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, new JobTag?(JobTag.SatisfyingNeeds), false, false, null, false, true, true);
				if (this.pawn.InMentalState && this.pawn.MentalStateDef.recoverFromCollapsingExhausted)
				{
					this.pawn.mindState.mentalStateHandler.CurState.RecoverFromState();
				}
				LifeStageDef curLifeStage = this.pawn.ageTracker.CurLifeStage;
				if (curLifeStage == null || curLifeStage.involuntarySleepIsNegativeEvent)
				{
					if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
					{
						Messages.Message("MessageInvoluntarySleep".Translate(this.pawn.LabelShort, this.pawn), this.pawn, MessageTypeDefOf.NegativeEvent, true);
					}
					TaleRecorder.RecordTale(TaleDefOf.Exhausted, new object[]
					{
						this.pawn
					});
				}
			}
		}

		// Token: 0x060086FF RID: 34559 RVA: 0x0030CA7A File Offset: 0x0030AC7A
		public void TickResting(float restEffectiveness)
		{
			if (restEffectiveness <= 0f)
			{
				return;
			}
			this.lastRestTick = Find.TickManager.TicksGame;
			this.lastRestEffectiveness = restEffectiveness;
		}

		// Token: 0x06008700 RID: 34560 RVA: 0x0030CA9C File Offset: 0x0030AC9C
		private bool ShouldInvoluntarySleepFromMTB()
		{
			float num = float.PositiveInfinity;
			if (this.ticksAtZero > 1000)
			{
				if (this.ticksAtZero < 15000)
				{
					num = 0.25f;
				}
				else if (this.ticksAtZero < 30000)
				{
					num = 0.125f;
				}
				else if (this.ticksAtZero < 45000)
				{
					num = 0.083333336f;
				}
				else
				{
					num = 0.0625f;
				}
			}
			LifeStageDef curLifeStage = this.pawn.ageTracker.CurLifeStage;
			SimpleCurve simpleCurve = (curLifeStage != null) ? curLifeStage.involuntarySleepMTBDaysFromRest : null;
			if (simpleCurve != null)
			{
				num = Rand.CombineMTBs(num, simpleCurve.Evaluate(base.CurLevelPercentage));
			}
			return Rand.MTBEventOccurs(num, 60000f, 150f);
		}

		// Token: 0x06008701 RID: 34561 RVA: 0x0030CB44 File Offset: 0x0030AD44
		private static bool CanInvoluntarilySleep(Pawn pawn)
		{
			Pawn_JobTracker jobs = pawn.jobs;
			bool flag;
			if (jobs == null)
			{
				flag = false;
			}
			else
			{
				JobDriver curDriver = jobs.curDriver;
				flag = ((curDriver != null) ? new bool?(curDriver.asleep) : null).GetValueOrDefault();
			}
			if (flag)
			{
				return false;
			}
			if (!RestUtility.CanFallAsleep(pawn))
			{
				return false;
			}
			if (!pawn.Spawned)
			{
				if (!pawn.ageTracker.CurLifeStage.canSleepWhileHeld)
				{
					return false;
				}
				if (!(pawn.SpawnedParentOrMe is Pawn))
				{
					return false;
				}
				if (pawn.IsWorldPawn())
				{
					return false;
				}
				if (pawn.IsCaravanMember())
				{
					return false;
				}
			}
			return !pawn.ageTracker.CurLifeStage.canVoluntarilySleep || pawn.CurJobDef != JobDefOf.LayDown;
		}

		// Token: 0x04005369 RID: 21353
		private int lastRestTick = -999;

		// Token: 0x0400536A RID: 21354
		private float lastRestEffectiveness = 1f;

		// Token: 0x0400536B RID: 21355
		private int ticksAtZero;

		// Token: 0x0400536C RID: 21356
		private const float FullSleepHours = 10.5f;

		// Token: 0x0400536D RID: 21357
		public const float BaseRestGainPerTick = 3.809524E-05f;

		// Token: 0x0400536E RID: 21358
		private const float BaseRestFallPerTick = 1.5833333E-05f;

		// Token: 0x0400536F RID: 21359
		public const float ThreshTired = 0.28f;

		// Token: 0x04005370 RID: 21360
		public const float ThreshVeryTired = 0.14f;

		// Token: 0x04005371 RID: 21361
		public const float DefaultFallAsleepMaxLevel = 0.75f;

		// Token: 0x04005372 RID: 21362
		public const float DefaultNaturalWakeThreshold = 1f;

		// Token: 0x04005373 RID: 21363
		public const float CanWakeThreshold = 0.2f;

		// Token: 0x04005374 RID: 21364
		private const float BaseInvoluntarySleepMTBDays = 0.25f;
	}
}
