using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020004DF RID: 1247
	public class Pawn_CallTracker
	{
		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x0600249A RID: 9370 RVA: 0x000DD21C File Offset: 0x000DB41C
		private bool PawnAggressive
		{
			get
			{
				return this.pawn.InAggroMentalState || (this.pawn.mindState.enemyTarget != null && this.pawn.mindState.enemyTarget.Spawned && Find.TickManager.TicksGame - this.pawn.mindState.lastEngageTargetTick <= 360) || (this.pawn.CurJob != null && this.pawn.CurJob.def == JobDefOf.AttackMelee);
			}
		}

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x0600249B RID: 9371 RVA: 0x000DD2AC File Offset: 0x000DB4AC
		private float IdleCallVolumeFactor
		{
			get
			{
				switch (Find.TickManager.CurTimeSpeed)
				{
				case TimeSpeed.Paused:
					return 1f;
				case TimeSpeed.Normal:
					return 1f;
				case TimeSpeed.Fast:
					return 1f;
				case TimeSpeed.Superfast:
					return 0.25f;
				case TimeSpeed.Ultrafast:
					return 0.25f;
				default:
					throw new NotImplementedException();
				}
			}
		}

		// Token: 0x0600249C RID: 9372 RVA: 0x000DD303 File Offset: 0x000DB503
		public Pawn_CallTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600249D RID: 9373 RVA: 0x000DD319 File Offset: 0x000DB519
		public void CallTrackerTickInterval(int delta)
		{
			if (this.ticksToNextCall < 0)
			{
				this.ResetTicksToNextCall();
			}
			this.ticksToNextCall -= delta;
			if (this.ticksToNextCall <= 0)
			{
				this.TryDoCall();
				this.ResetTicksToNextCall();
			}
		}

		// Token: 0x0600249E RID: 9374 RVA: 0x000DD350 File Offset: 0x000DB550
		private void ResetTicksToNextCall()
		{
			if (this.pawn.IsMutant)
			{
				this.ticksToNextCall = MutantUtility.CallIntervalRange.RandomInRange;
				return;
			}
			this.ticksToNextCall = this.pawn.def.race.soundCallIntervalRange.RandomInRange;
			if (this.PawnAggressive)
			{
				this.ticksToNextCall = (int)((float)this.ticksToNextCall * this.pawn.def.race.soundCallIntervalAggressiveFactor);
				return;
			}
			if (this.pawn.Faction != null && this.pawn.Faction.IsPlayer)
			{
				this.ticksToNextCall = (int)((float)this.ticksToNextCall * this.pawn.def.race.soundCallIntervalFriendlyFactor);
			}
		}

		// Token: 0x0600249F RID: 9375 RVA: 0x000DD40C File Offset: 0x000DB60C
		private void TryDoCall()
		{
			if (!this.pawn.Spawned)
			{
				return;
			}
			if (!Find.CameraDriver.CurrentViewRect.ExpandedBy(10).Contains(this.pawn.Position))
			{
				return;
			}
			if (this.pawn.Downed || !this.pawn.Awake())
			{
				return;
			}
			if (this.pawn.IsPsychologicallyInvisible())
			{
				return;
			}
			if (this.pawn.Position.Fogged(this.pawn.MapHeld))
			{
				return;
			}
			if (this.pawn.IsColonyMech && this.pawn.IsCharging())
			{
				return;
			}
			this.DoCall(false);
		}

		// Token: 0x060024A0 RID: 9376 RVA: 0x000DD4BC File Offset: 0x000DB6BC
		public void DoCall(bool forceAggressive = false)
		{
			if (this.PawnAggressive || forceAggressive)
			{
				LifeStageUtility.PlayNearestLifestageSound(this.pawn, (LifeStageAge lifeStage) => lifeStage.soundAngry, null, (MutantDef mutant) => mutant.soundAngry, 1f);
				return;
			}
			LifeStageUtility.PlayNearestLifestageSound(this.pawn, (LifeStageAge lifeStage) => lifeStage.soundCall, (GeneDef gene) => gene.soundCall, (MutantDef mutant) => mutant.soundCall, this.IdleCallVolumeFactor);
		}

		// Token: 0x060024A1 RID: 9377 RVA: 0x000DD594 File Offset: 0x000DB794
		public void Notify_InAggroMentalState()
		{
			this.ticksToNextCall = Pawn_CallTracker.CallOnAggroDelayRange.RandomInRange;
		}

		// Token: 0x060024A2 RID: 9378 RVA: 0x000DD5B4 File Offset: 0x000DB7B4
		public void Notify_DidMeleeAttack()
		{
			if (Rand.Value < 0.5f)
			{
				this.ticksToNextCall = Pawn_CallTracker.CallOnMeleeDelayRange.RandomInRange;
			}
		}

		// Token: 0x060024A3 RID: 9379 RVA: 0x000DD5E0 File Offset: 0x000DB7E0
		public void Notify_Released()
		{
			if (Rand.Value < 0.75f)
			{
				this.ticksToNextCall = Pawn_CallTracker.CallOnAggroDelayRange.RandomInRange;
			}
		}

		// Token: 0x04001AB4 RID: 6836
		public Pawn pawn;

		// Token: 0x04001AB5 RID: 6837
		private int ticksToNextCall = -1;

		// Token: 0x04001AB6 RID: 6838
		private static readonly IntRange CallOnAggroDelayRange = new IntRange(0, 120);

		// Token: 0x04001AB7 RID: 6839
		private static readonly IntRange CallOnMeleeDelayRange = new IntRange(0, 20);

		// Token: 0x04001AB8 RID: 6840
		private const float AngryCallOnMeleeChance = 0.5f;

		// Token: 0x04001AB9 RID: 6841
		private const int AggressiveDurationAfterEngagingTarget = 360;
	}
}
