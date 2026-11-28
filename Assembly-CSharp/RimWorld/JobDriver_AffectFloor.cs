using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009C4 RID: 2500
	public abstract class JobDriver_AffectFloor : JobDriver
	{
		// Token: 0x17000DC7 RID: 3527
		// (get) Token: 0x06004BF6 RID: 19446
		protected abstract int BaseWorkAmount { get; }

		// Token: 0x17000DC8 RID: 3528
		// (get) Token: 0x06004BF7 RID: 19447
		protected abstract DesignationDef DesDef { get; }

		// Token: 0x17000DC9 RID: 3529
		// (get) Token: 0x06004BF8 RID: 19448 RVA: 0x001D6333 File Offset: 0x001D4533
		protected virtual StatDef SpeedStat
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06004BF9 RID: 19449 RVA: 0x001D6336 File Offset: 0x001D4536
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, ReservationLayerDefOf.Floor, errorOnFailed, false);
		}

		// Token: 0x06004BFA RID: 19450 RVA: 0x001D635D File Offset: 0x001D455D
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_AffectFloor.<MakeNewToils>d__9 <MakeNewToils>d__ = new JobDriver_AffectFloor.<MakeNewToils>d__9(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004BFB RID: 19451
		protected abstract void DoEffect(IntVec3 c);

		// Token: 0x06004BFC RID: 19452 RVA: 0x001D636D File Offset: 0x001D456D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}

		// Token: 0x04002FF7 RID: 12279
		private float workLeft = -1000f;

		// Token: 0x04002FF8 RID: 12280
		protected bool clearSnow;
	}
}
