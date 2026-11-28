using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A97 RID: 2711
	public class JobDriver_PruneGauranlenTre : JobDriver
	{
		// Token: 0x17000ED4 RID: 3796
		// (get) Token: 0x06005197 RID: 20887 RVA: 0x001E766C File Offset: 0x001E586C
		private CompTreeConnection TreeConnection
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompTreeConnection>();
			}
		}

		// Token: 0x06005198 RID: 20888 RVA: 0x001E7694 File Offset: 0x001E5894
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			float num = this.TreeConnection.DesiredConnectionStrength - this.TreeConnection.ConnectionStrength;
			this.numPositions = Mathf.Min(8, Mathf.CeilToInt(num / this.TreeConnection.ConnectionStrengthGainPerHourOfPruning) + 1);
		}

		// Token: 0x06005199 RID: 20889 RVA: 0x001E76DF File Offset: 0x001E58DF
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x0600519A RID: 20890 RVA: 0x001E7702 File Offset: 0x001E5902
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_PruneGauranlenTre.<MakeNewToils>d__9 <MakeNewToils>d__ = new JobDriver_PruneGauranlenTre.<MakeNewToils>d__9(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0600519B RID: 20891 RVA: 0x001E7714 File Offset: 0x001E5914
		private IntVec3 GetAdjacentCell(Thing treeThing)
		{
			IntVec3 result;
			if ((from x in GenAdj.CellsAdjacent8Way(treeThing)
			where x.InBounds(this.pawn.Map) && !x.Fogged(this.pawn.Map) && !x.IsForbidden(this.pawn) && this.pawn.CanReserveAndReach(x, PathEndMode.OnCell, Danger.Some, 1, -1, null, false)
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return treeThing.Position;
		}

		// Token: 0x0600519C RID: 20892 RVA: 0x001E7749 File Offset: 0x001E5949
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.numPositions, "numPositions", 1, false);
		}

		// Token: 0x04003196 RID: 12694
		private int numPositions = 1;

		// Token: 0x04003197 RID: 12695
		private const TargetIndex TreeIndex = TargetIndex.A;

		// Token: 0x04003198 RID: 12696
		private const TargetIndex AdjacentCellIndex = TargetIndex.B;

		// Token: 0x04003199 RID: 12697
		private const int DurationTicks = 2500;

		// Token: 0x0400319A RID: 12698
		private const int MaxPositions = 8;
	}
}
