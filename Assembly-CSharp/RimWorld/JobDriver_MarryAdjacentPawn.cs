using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009E6 RID: 2534
	public class JobDriver_MarryAdjacentPawn : JobDriver
	{
		// Token: 0x17000DFF RID: 3583
		// (get) Token: 0x06004CCA RID: 19658 RVA: 0x001D818C File Offset: 0x001D638C
		private Pawn OtherFiance
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000E00 RID: 3584
		// (get) Token: 0x06004CCB RID: 19659 RVA: 0x001D81B2 File Offset: 0x001D63B2
		public int TicksLeftToMarry
		{
			get
			{
				return this.ticksLeftToMarry;
			}
		}

		// Token: 0x06004CCC RID: 19660 RVA: 0x001D81BA File Offset: 0x001D63BA
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004CCD RID: 19661 RVA: 0x001D81BD File Offset: 0x001D63BD
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_MarryAdjacentPawn.<MakeNewToils>d__8 <MakeNewToils>d__ = new JobDriver_MarryAdjacentPawn.<MakeNewToils>d__8(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004CCE RID: 19662 RVA: 0x001D81CD File Offset: 0x001D63CD
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeftToMarry, "ticksLeftToMarry", 0, false);
		}

		// Token: 0x0400302F RID: 12335
		private int ticksLeftToMarry = 2500;

		// Token: 0x04003030 RID: 12336
		private const TargetIndex OtherFianceInd = TargetIndex.A;

		// Token: 0x04003031 RID: 12337
		private const int Duration = 2500;
	}
}
