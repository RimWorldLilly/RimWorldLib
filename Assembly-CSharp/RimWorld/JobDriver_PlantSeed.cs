using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A90 RID: 2704
	public class JobDriver_PlantSeed : JobDriver
	{
		// Token: 0x17000ECA RID: 3786
		// (get) Token: 0x06005170 RID: 20848 RVA: 0x001E71E4 File Offset: 0x001E53E4
		private IntVec3 PlantCell
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Cell;
			}
		}

		// Token: 0x17000ECB RID: 3787
		// (get) Token: 0x06005171 RID: 20849 RVA: 0x001E7208 File Offset: 0x001E5408
		private CompPlantable PlantableComp
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompPlantable>();
			}
		}

		// Token: 0x06005172 RID: 20850 RVA: 0x001E722E File Offset: 0x001E542E
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, 1, null, errorOnFailed, false);
		}

		// Token: 0x06005173 RID: 20851 RVA: 0x001E7251 File Offset: 0x001E5451
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_PlantSeed.<MakeNewToils>d__9 <MakeNewToils>d__ = new JobDriver_PlantSeed.<MakeNewToils>d__9(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06005174 RID: 20852 RVA: 0x001E7261 File Offset: 0x001E5461
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.sowWorkDone, "sowWorkDone", 0f, false);
		}

		// Token: 0x0400318E RID: 12686
		private float sowWorkDone;

		// Token: 0x0400318F RID: 12687
		private const TargetIndex SeedIndex = TargetIndex.A;

		// Token: 0x04003190 RID: 12688
		private const TargetIndex PlantCellIndex = TargetIndex.B;

		// Token: 0x04003191 RID: 12689
		private const TargetIndex OldSeedStackIndex = TargetIndex.C;
	}
}
