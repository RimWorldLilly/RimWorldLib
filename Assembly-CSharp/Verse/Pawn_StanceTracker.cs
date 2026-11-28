using System;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse
{
	// Token: 0x020004E7 RID: 1255
	public class Pawn_StanceTracker : IExposable
	{
		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x0600254A RID: 9546 RVA: 0x000E2EC9 File Offset: 0x000E10C9
		public bool FullBodyBusy
		{
			get
			{
				return this.stunner.Stunned || this.curStance.StanceBusy;
			}
		}

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x0600254B RID: 9547 RVA: 0x000E2EE5 File Offset: 0x000E10E5
		public bool FullBodyBusyRecently
		{
			get
			{
				return GenTicks.TicksGame > this.lastTickFullBodyBusy + 2;
			}
		}

		// Token: 0x0600254C RID: 9548 RVA: 0x000E2EF6 File Offset: 0x000E10F6
		public Pawn_StanceTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.stunner = new StunHandler(this.pawn);
			this.stagger = new StaggerHandler(this.pawn);
		}

		// Token: 0x0600254D RID: 9549 RVA: 0x000E2F32 File Offset: 0x000E1132
		public void StanceTrackerTick()
		{
			this.stunner.StunHandlerTick();
			this.stagger.StaggerHandlerTick();
			this.curStance.StanceTick();
			if (this.FullBodyBusy)
			{
				this.lastTickFullBodyBusy = GenTicks.TicksGame;
			}
		}

		// Token: 0x0600254E RID: 9550 RVA: 0x000E2F68 File Offset: 0x000E1168
		public void StanceTrackerDraw()
		{
			this.curStance.StanceDraw();
		}

		// Token: 0x0600254F RID: 9551 RVA: 0x000E2F78 File Offset: 0x000E1178
		public void ExposeData()
		{
			Scribe_Deep.Look<StunHandler>(ref this.stunner, "stunner", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<StaggerHandler>(ref this.stagger, "stagger", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<Stance>(ref this.curStance, "curStance", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.lastTickFullBodyBusy, "lastTickFullBodyBusy", 0, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.curStance != null)
			{
				this.curStance.stanceTracker = this;
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.stagger == null)
			{
				this.stagger = new StaggerHandler(this.pawn);
				int num = 0;
				Scribe_Values.Look<int>(ref num, "staggerUntilTick", 0, false);
				if (num > Find.TickManager.TicksGame)
				{
					this.stagger.StaggerFor(num - Find.TickManager.TicksGame, 0.17f);
				}
			}
		}

		// Token: 0x06002550 RID: 9552 RVA: 0x000E3061 File Offset: 0x000E1261
		[Obsolete("Use pawn.stances.stagger.StaggerFor instead")]
		public void StaggerFor(int ticks)
		{
			this.stagger.StaggerFor(ticks, 0.17f);
		}

		// Token: 0x06002551 RID: 9553 RVA: 0x000E3075 File Offset: 0x000E1275
		public void CancelBusyStanceSoft()
		{
			if (this.curStance is Stance_Warmup)
			{
				this.SetStance(new Stance_Mobile());
			}
		}

		// Token: 0x06002552 RID: 9554 RVA: 0x000E308F File Offset: 0x000E128F
		public void CancelBusyStanceHard()
		{
			this.SetStance(new Stance_Mobile());
		}

		// Token: 0x06002553 RID: 9555 RVA: 0x000E309C File Offset: 0x000E129C
		public void SetStance(Stance newStance)
		{
			if (this.debugLog)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(16, 4);
				defaultInterpolatedStringHandler.AppendFormatted<int>(Find.TickManager.TicksGame);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<Pawn>(this.pawn);
				defaultInterpolatedStringHandler.AppendLiteral(" SetStance ");
				defaultInterpolatedStringHandler.AppendFormatted<Stance>(this.curStance);
				defaultInterpolatedStringHandler.AppendLiteral(" -> ");
				defaultInterpolatedStringHandler.AppendFormatted<Stance>(newStance);
				Log.Message(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			newStance.stanceTracker = this;
			this.curStance = newStance;
			if (this.pawn.jobs.curDriver != null)
			{
				this.pawn.jobs.curDriver.Notify_StanceChanged();
			}
		}

		// Token: 0x06002554 RID: 9556 RVA: 0x000E3153 File Offset: 0x000E1353
		public void Notify_DamageTaken(DamageInfo dinfo)
		{
		}

		// Token: 0x04001AED RID: 6893
		public Pawn pawn;

		// Token: 0x04001AEE RID: 6894
		public Stance curStance = new Stance_Mobile();

		// Token: 0x04001AEF RID: 6895
		public int lastTickFullBodyBusy;

		// Token: 0x04001AF0 RID: 6896
		public StunHandler stunner;

		// Token: 0x04001AF1 RID: 6897
		public StaggerHandler stagger;

		// Token: 0x04001AF2 RID: 6898
		public const int StaggerMeleeAttackTicks = 95;

		// Token: 0x04001AF3 RID: 6899
		public const int StaggerBulletImpactTicks = 95;

		// Token: 0x04001AF4 RID: 6900
		public const int StaggerExplosionImpactTicks = 95;

		// Token: 0x04001AF5 RID: 6901
		public const int RecentlyDurationTicks = 2;

		// Token: 0x04001AF6 RID: 6902
		public bool debugLog;
	}
}
