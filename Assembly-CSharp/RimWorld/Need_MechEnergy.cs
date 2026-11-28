using System;
using System.Text;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200157B RID: 5499
	public class Need_MechEnergy : Need
	{
		// Token: 0x170016FD RID: 5885
		// (get) Token: 0x060086D7 RID: 34519 RVA: 0x0030BB08 File Offset: 0x00309D08
		public override float MaxLevel
		{
			get
			{
				return (float)this.pawn.RaceProps.maxMechEnergy;
			}
		}

		// Token: 0x170016FE RID: 5886
		// (get) Token: 0x060086D8 RID: 34520 RVA: 0x0030BB1B File Offset: 0x00309D1B
		public bool IsSelfShutdown
		{
			get
			{
				return this.pawn.CurJobDef == JobDefOf.SelfShutdown && this.pawn.GetPosture().Laying();
			}
		}

		// Token: 0x170016FF RID: 5887
		// (get) Token: 0x060086D9 RID: 34521 RVA: 0x0030BB41 File Offset: 0x00309D41
		public bool IsLowEnergySelfShutdown
		{
			get
			{
				return this.IsSelfShutdown && this.selfShutdown;
			}
		}

		// Token: 0x17001700 RID: 5888
		// (get) Token: 0x060086DA RID: 34522 RVA: 0x0030BB53 File Offset: 0x00309D53
		public override int GUIChangeArrow
		{
			get
			{
				if (this.FallPerDay > 0f)
				{
					return -1;
				}
				if (this.IsSelfShutdown || this.pawn.IsCharging())
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x17001701 RID: 5889
		// (get) Token: 0x060086DB RID: 34523 RVA: 0x0030BB7C File Offset: 0x00309D7C
		private float BaseFallPerDay
		{
			get
			{
				if (this.pawn.mindState != null && !this.pawn.mindState.IsIdle && !this.pawn.IsGestating())
				{
					return 10f;
				}
				return 3f;
			}
		}

		// Token: 0x17001702 RID: 5890
		// (get) Token: 0x060086DC RID: 34524 RVA: 0x0030BBB8 File Offset: 0x00309DB8
		public float FallPerDay
		{
			get
			{
				if (this.pawn.Downed)
				{
					return 0f;
				}
				if (!this.pawn.Awake())
				{
					return 0f;
				}
				if (this.IsSelfShutdown)
				{
					return 0f;
				}
				if (this.currentCharger != null)
				{
					return 0f;
				}
				if (this.pawn.IsCaravanMember())
				{
					return 0f;
				}
				return this.BaseFallPerDay * this.pawn.GetStatValue(StatDefOf.MechEnergyUsageFactor, true, -1);
			}
		}

		// Token: 0x060086DD RID: 34525 RVA: 0x0030BC33 File Offset: 0x00309E33
		public Need_MechEnergy(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x060086DE RID: 34526 RVA: 0x0030BC3C File Offset: 0x00309E3C
		public override void NeedInterval()
		{
			float num = 400f;
			if (!this.IsSelfShutdown)
			{
				this.CurLevel -= this.FallPerDay / num;
			}
			else
			{
				this.CurLevel += 1f / num;
			}
			if (this.CurLevel <= 0f)
			{
				this.selfShutdown = true;
			}
			else if (this.CurLevel >= 15f || this.pawn.CurJobDef == JobDefOf.MechCharge)
			{
				this.selfShutdown = false;
			}
			Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.SelfShutdown, false);
			if (firstHediffOfDef != null && !this.selfShutdown)
			{
				this.pawn.health.RemoveHediff(firstHediffOfDef);
				Pawn_JobTracker jobs = this.pawn.jobs;
				if (jobs != null)
				{
					jobs.CheckForJobOverride(0f, true);
				}
			}
			else if (firstHediffOfDef == null && this.selfShutdown)
			{
				this.pawn.health.AddHediff(HediffDefOf.SelfShutdown, null, null, null);
			}
			if (this.selfShutdown && this.pawn.Spawned && !this.pawn.Downed && this.pawn.CurJobDef != JobDefOf.SelfShutdown && this.pawn.jobs != null)
			{
				IntVec3 position = this.pawn.Position;
				RCellFinder.TryFindNearbyMechSelfShutdownSpot(this.pawn.Position, this.pawn, this.pawn.Map, out position, true);
				if (this.pawn.Drafted)
				{
					this.pawn.drafter.Drafted = false;
				}
				this.pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.SelfShutdown, position), JobCondition.InterruptForced, null, false, true, null, new JobTag?(JobTag.SatisfyingNeeds), false, false, null, false, true, false);
			}
		}

		// Token: 0x060086DF RID: 34527 RVA: 0x0030BE1A File Offset: 0x0030A01A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.selfShutdown, "selfShutdown", false, false);
			Scribe_References.Look<Building_MechCharger>(ref this.currentCharger, "currentCharger", false);
		}

		// Token: 0x060086E0 RID: 34528 RVA: 0x0030BE48 File Offset: 0x0030A048
		public override string GetTipString()
		{
			StringBuilder stringBuilder = new StringBuilder(base.GetTipString());
			stringBuilder.AppendInNewLine("CurrentMechEnergyFallPerDay".Translate() + ": " + (this.FallPerDay / 100f).ToStringPercent());
			return stringBuilder.ToString();
		}

		// Token: 0x0400533C RID: 21308
		private bool selfShutdown;

		// Token: 0x0400533D RID: 21309
		public Building_MechCharger currentCharger;

		// Token: 0x0400533E RID: 21310
		public const float BaseFallPerDayActive = 10f;

		// Token: 0x0400533F RID: 21311
		public const float BaseFallPerDayIdle = 3f;

		// Token: 0x04005340 RID: 21312
		public const float BaseGainPerDaySelfShutdown = 1f;

		// Token: 0x04005341 RID: 21313
		public const float ShutdownUntil = 15f;
	}
}
