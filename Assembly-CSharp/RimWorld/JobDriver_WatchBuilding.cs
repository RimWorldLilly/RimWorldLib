using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A04 RID: 2564
	public class JobDriver_WatchBuilding : JobDriver
	{
		// Token: 0x06004D8B RID: 19851 RVA: 0x001DACC8 File Offset: 0x001D8EC8
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, this.job.def.joyMaxParticipants, 0, null, errorOnFailed, false) && this.pawn.ReserveSittableOrSpot(this.job.targetB.Cell, this.job, errorOnFailed) && (!base.TargetC.HasThing || !(base.TargetC.Thing is Building_Bed) || this.pawn.Reserve(this.job.targetC, this.job, ((Building_Bed)base.TargetC.Thing).SleepingSlotsCount, 0, null, errorOnFailed, false));
		}

		// Token: 0x06004D8C RID: 19852 RVA: 0x001DAD94 File Offset: 0x001D8F94
		public override bool CanBeginNowWhileLyingDown()
		{
			return base.TargetC.HasThing && base.TargetC.Thing is Building_Bed && JobInBedUtility.InBedOrRestSpotNow(this.pawn, base.TargetC);
		}

		// Token: 0x06004D8D RID: 19853 RVA: 0x001DADD9 File Offset: 0x001D8FD9
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_WatchBuilding.<MakeNewToils>d__2 <MakeNewToils>d__ = new JobDriver_WatchBuilding.<MakeNewToils>d__2(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004D8E RID: 19854 RVA: 0x001DADEC File Offset: 0x001D8FEC
		protected virtual void WatchTickAction(int delta)
		{
			this.pawn.rotationTracker.FaceCell(base.TargetA.Cell);
			this.pawn.GainComfortFromCellIfPossible(delta, false);
			JoyUtility.JoyTickCheckEnd(this.pawn, delta, JoyTickFullJoyAction.EndJob, 1f, (Building)base.TargetThingA);
		}

		// Token: 0x06004D8F RID: 19855 RVA: 0x001DAE44 File Offset: 0x001D9044
		public override object[] TaleParameters()
		{
			return new object[]
			{
				this.pawn,
				base.TargetA.Thing.def
			};
		}
	}
}
