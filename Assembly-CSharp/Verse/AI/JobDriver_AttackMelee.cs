using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000823 RID: 2083
	public class JobDriver_AttackMelee : JobDriver
	{
		// Token: 0x0600416F RID: 16751 RVA: 0x0019F274 File Offset: 0x0019D474
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.numMeleeAttacksMade, "numMeleeAttacksMade", 0, false);
		}

		// Token: 0x06004170 RID: 16752 RVA: 0x0019F290 File Offset: 0x0019D490
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			IAttackTarget attackTarget = this.job.targetA.Thing as IAttackTarget;
			if (attackTarget != null)
			{
				this.pawn.Map.attackTargetReservationManager.Reserve(this.pawn, this.job, attackTarget);
			}
			return true;
		}

		// Token: 0x06004171 RID: 16753 RVA: 0x0019F2D9 File Offset: 0x0019D4D9
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_AttackMelee.<MakeNewToils>d__4 <MakeNewToils>d__ = new JobDriver_AttackMelee.<MakeNewToils>d__4(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004172 RID: 16754 RVA: 0x0019F2EC File Offset: 0x0019D4EC
		public override void Notify_PatherFailed()
		{
			if (this.job.attackDoorIfTargetLost)
			{
				Thing thing;
				using (PawnPath pawnPath = base.Map.pathFinder.FindPathNow(this.pawn.Position, base.TargetA.Cell, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.PassDoors, false, false, false, true), null, PathEndMode.OnCell, null))
				{
					if (!pawnPath.Found)
					{
						return;
					}
					IntVec3 position;
					thing = pawnPath.FirstBlockingBuilding(out position, this.pawn);
				}
				if (thing != null)
				{
					IntVec3 position = thing.Position;
					if (position.InHorDistOf(this.pawn.Position, 6f))
					{
						this.job.targetA = thing;
						this.job.maxNumMeleeAttacks = Rand.RangeInclusive(2, 5);
						this.job.expiryInterval = Rand.Range(2000, 4000);
						return;
					}
				}
			}
			base.Notify_PatherFailed();
		}

		// Token: 0x06004173 RID: 16755 RVA: 0x0019F3F4 File Offset: 0x0019D5F4
		public override bool IsContinuation(Job j)
		{
			return this.job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
		}

		// Token: 0x04002B27 RID: 11047
		private const int AutotargetRadius = 4;

		// Token: 0x04002B28 RID: 11048
		private int numMeleeAttacksMade;
	}
}
