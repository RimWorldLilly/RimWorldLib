using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000872 RID: 2162
	public class MentalState_Roaming : MentalState
	{
		// Token: 0x17000CD4 RID: 3284
		// (get) Token: 0x060043A3 RID: 17315 RVA: 0x001A9CBF File Offset: 0x001A7EBF
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060043A4 RID: 17316 RVA: 0x001A9CC4 File Offset: 0x001A7EC4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.exitDest, "exitDest", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.waitAtEdgeUntilTick, "waitAtEdgeUntilTick", 0, false);
		}

		// Token: 0x060043A5 RID: 17317 RVA: 0x001A9D04 File Offset: 0x001A7F04
		public override void PreStart()
		{
			base.PreStart();
			Pawn_CallTracker caller = this.pawn.caller;
			if (caller != null)
			{
				caller.DoCall(false);
			}
			Messages.Message("MessageRoamerLeaving".Translate(this.pawn.Named("PAWN")).CapitalizeFirst(), this.pawn, MessageTypeDefOf.ThreatSmall, true);
		}

		// Token: 0x060043A6 RID: 17318 RVA: 0x001A9D6C File Offset: 0x001A7F6C
		public override void MentalStateTick(int delta)
		{
			base.MentalStateTick(delta);
			if (!this.pawn.Spawned)
			{
				return;
			}
			if (!this.exitDest.IsValid || !this.pawn.CanReach(this.exitDest, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				this.waitAtEdgeUntilTick = -1;
				this.exitDest = IntVec3.Invalid;
				if (!RCellFinder.TryFindRandomExitSpot(this.pawn, out this.exitDest, TraverseMode.ByPawn))
				{
					base.RecoverFromState();
					return;
				}
			}
			if (this.pawn.roping.IsRoped)
			{
				base.RecoverFromState();
			}
			if (this.waitAtEdgeUntilTick < 0 && this.pawn.Position.InHorDistOf(this.exitDest, 12f))
			{
				this.waitAtEdgeUntilTick = Find.TickManager.TicksGame + MentalState_Roaming.WaitAtEdgeBeforeExitingTicks.RandomInRange;
			}
		}

		// Token: 0x060043A7 RID: 17319 RVA: 0x001A9E44 File Offset: 0x001A8044
		public override void PostEnd()
		{
			base.PostEnd();
			this.pawn.mindState.lastStartRoamCooldownTick = new int?(Find.TickManager.TicksGame);
		}

		// Token: 0x060043A8 RID: 17320 RVA: 0x001A9E6B File Offset: 0x001A806B
		public bool ShouldExitMapNow()
		{
			return (this.waitAtEdgeUntilTick > 0 && Find.TickManager.TicksGame > this.waitAtEdgeUntilTick) || base.Age > 60000;
		}

		// Token: 0x04002C0B RID: 11275
		public const int WanderDistance = 12;

		// Token: 0x04002C0C RID: 11276
		private const int MaxTicksToRoamBeforeExit = 60000;

		// Token: 0x04002C0D RID: 11277
		private static readonly IntRange WaitAtEdgeBeforeExitingTicks = new IntRange(7000, 8000);

		// Token: 0x04002C0E RID: 11278
		public IntVec3 exitDest = IntVec3.Invalid;

		// Token: 0x04002C0F RID: 11279
		public int waitAtEdgeUntilTick = -1;
	}
}
