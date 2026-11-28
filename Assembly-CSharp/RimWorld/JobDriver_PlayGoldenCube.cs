using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009F7 RID: 2551
	public class JobDriver_PlayGoldenCube : JobDriver
	{
		// Token: 0x06004D43 RID: 19779 RVA: 0x001D9EAA File Offset: 0x001D80AA
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, 1, null, errorOnFailed, false);
		}

		// Token: 0x06004D44 RID: 19780 RVA: 0x001D9ECD File Offset: 0x001D80CD
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_PlayGoldenCube.<MakeNewToils>d__13 <MakeNewToils>d__ = new JobDriver_PlayGoldenCube.<MakeNewToils>d__13(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004D45 RID: 19781 RVA: 0x001D9EE0 File Offset: 0x001D80E0
		private bool TryGetNextCell(out IntVec3 dest)
		{
			dest = IntVec3.Invalid;
			IntVec3 a = this.position;
			int num = 0;
			int num2 = 0;
			if (this.stepsTillRotSwitch <= 0)
			{
				this.stepsTillRotSwitch = JobDriver_PlayGoldenCube.SwitchDirStepsRange.RandomInRange;
				this.clockwise = !this.clockwise;
			}
			do
			{
				this.angle = (this.angle + Rand.Range(45f, 90f) * (float)(this.clockwise ? 1 : -1)) % 360f;
				int num3 = 1;
				while (num3 <= 3 && (a + IntVec3.FromPolar(this.angle, (float)num3)).Walkable(this.pawn.Map))
				{
					num = num3;
					num3++;
				}
			}
			while (num == 0 && num2++ < 10);
			if (num2 >= 10 || num == 0)
			{
				return false;
			}
			int num4 = Rand.Range(1, num + 1);
			dest = a + IntVec3.FromPolar(this.angle, (float)num4);
			this.lastDist = num4;
			this.stepsTillRotSwitch--;
			return true;
		}

		// Token: 0x06004D46 RID: 19782 RVA: 0x001D9FE8 File Offset: 0x001D81E8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.position, "position", default(IntVec3), false);
			Scribe_Values.Look<IntVec3>(ref this.nextGoal, "nextGoal", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.lastDist, "lastDist", 0, false);
			Scribe_Values.Look<int>(ref this.stepsTillRotSwitch, "stepsTillSwitch", 0, false);
			Scribe_Values.Look<float>(ref this.angle, "angle", 0f, false);
			Scribe_Values.Look<bool>(ref this.clockwise, "clockwise", false, false);
		}

		// Token: 0x04003055 RID: 12373
		private IntVec3 nextGoal;

		// Token: 0x04003056 RID: 12374
		private IntVec3 position;

		// Token: 0x04003057 RID: 12375
		private int lastDist;

		// Token: 0x04003058 RID: 12376
		private int nextMoveTick;

		// Token: 0x04003059 RID: 12377
		private int stepsTillRotSwitch = JobDriver_PlayGoldenCube.SwitchDirStepsRange.RandomInRange;

		// Token: 0x0400305A RID: 12378
		private bool clockwise;

		// Token: 0x0400305B RID: 12379
		private float angle;

		// Token: 0x0400305C RID: 12380
		private const int MinCircleDist = 1;

		// Token: 0x0400305D RID: 12381
		private const int MaxCircleDist = 3;

		// Token: 0x0400305E RID: 12382
		private static readonly IntRange SwitchDirStepsRange = new IntRange(6, 12);

		// Token: 0x0400305F RID: 12383
		private static readonly IntRange WaitTimeTicksRange = new IntRange(180, 360);

		// Token: 0x04003060 RID: 12384
		private const TargetIndex CubeIndex = TargetIndex.A;
	}
}
