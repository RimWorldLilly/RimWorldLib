using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x0200080D RID: 2061
	public class JobDriver_PlayWalking : JobDriver_BabyPlay
	{
		// Token: 0x17000C85 RID: 3205
		// (get) Token: 0x060040E3 RID: 16611 RVA: 0x0019C480 File Offset: 0x0019A680
		protected override JobDriver_BabyPlay.StartingConditions StartingCondition
		{
			get
			{
				return JobDriver_BabyPlay.StartingConditions.PickupBaby;
			}
		}

		// Token: 0x17000C86 RID: 3206
		// (get) Token: 0x060040E4 RID: 16612 RVA: 0x0019C484 File Offset: 0x0019A684
		public override Vector3 ForcedBodyOffset
		{
			get
			{
				if (!this.pawn.IsCarryingPawn(base.Baby))
				{
					return Vector3.zero;
				}
				float f = (float)Find.TickManager.TicksGame / 60f * 2f;
				if (this.pawn.pather.Moving)
				{
					float f2 = Mathf.Sin(f);
					float num = Mathf.Sign(f2);
					return new Vector3(EasingFunctions.EaseInOutQuad(Mathf.Abs(f2) * 0.6f) * num * 0.12f, 0f, 0f);
				}
				float z = EasingFunctions.EaseInOutQuint(Mathf.Abs(Mathf.Sin(f) * 0.6f)) * 0.12f;
				return new Vector3(0f, 0f, z);
			}
		}

		// Token: 0x060040E5 RID: 16613 RVA: 0x0019C536 File Offset: 0x0019A736
		public static IntVec3 TryFindWanderCell(Pawn pawn, IntVec3 root)
		{
			return RCellFinder.RandomWanderDestFor(pawn, root, 8f, null, PawnUtility.ResolveMaxDanger(pawn, Danger.None), false);
		}

		// Token: 0x060040E6 RID: 16614 RVA: 0x0019C550 File Offset: 0x0019A750
		private void CheckEndJobSuccess(int delta)
		{
			if (this.roomPlayGainFactor < 0f || this.pawn.IsHashIntervalTick(300, delta))
			{
				this.roomPlayGainFactor = BabyPlayUtility.GetRoomPlayGainFactors(base.Baby);
			}
			if (BabyPlayUtility.PlayTickCheckEnd(base.Baby, this.pawn, this.roomPlayGainFactor, delta, null))
			{
				this.pawn.jobs.curDriver.EndJobWith(JobCondition.Succeeded);
			}
		}

		// Token: 0x060040E7 RID: 16615 RVA: 0x0019C5BF File Offset: 0x0019A7BF
		protected override IEnumerable<Toil> Play()
		{
			JobDriver_PlayWalking.<Play>d__10 <Play>d__ = new JobDriver_PlayWalking.<Play>d__10(-2);
			<Play>d__.<>4__this = this;
			return <Play>d__;
		}

		// Token: 0x060040E8 RID: 16616 RVA: 0x0019C5D0 File Offset: 0x0019A7D0
		private Toil PlayWalking()
		{
			Toil toil = Toils_Goto.Goto(TargetIndex.B, PathEndMode.OnCell);
			toil.initAction = (Action)Delegate.Combine(toil.initAction, new Action(delegate()
			{
				this.job.locomotionUrgency = LocomotionUrgency.Amble;
			}));
			toil.tickIntervalAction = (Action<int>)Delegate.Combine(toil.tickIntervalAction, new Action<int>(this.CheckEndJobSuccess));
			ChildcareUtility.MakeBabyPlayAsLongAsToilIsActive(toil, TargetIndex.A);
			return toil;
		}

		// Token: 0x060040E9 RID: 16617 RVA: 0x0019C630 File Offset: 0x0019A830
		private Toil Interact()
		{
			Toil toil = Toils_General.Wait(600, TargetIndex.None);
			toil.WithEffect(EffecterDefOf.PlayStatic, TargetIndex.A, null);
			toil.initAction = (Action)Delegate.Combine(toil.initAction, new Action(delegate()
			{
				this.pawn.Rotation = Rot4.Random;
			}));
			toil.tickIntervalAction = (Action<int>)Delegate.Combine(toil.tickIntervalAction, new Action<int>(delegate(int delta)
			{
				if (this.pawn.IsHashIntervalTick(300, delta))
				{
					this.pawn.interactions.TryInteractWith(base.Baby, InteractionDefOf.BabyPlay);
				}
				this.CheckEndJobSuccess(delta);
			}));
			ChildcareUtility.MakeBabyPlayAsLongAsToilIsActive(toil, TargetIndex.A);
			return toil;
		}

		// Token: 0x04002B03 RID: 11011
		private const TargetIndex WanderCellInd = TargetIndex.B;

		// Token: 0x04002B04 RID: 11012
		private const int InteractionTicks = 600;

		// Token: 0x04002B05 RID: 11013
		private const int InteractionIntervalTicks = 300;

		// Token: 0x04002B06 RID: 11014
		private const float WanderRadius = 8f;
	}
}
