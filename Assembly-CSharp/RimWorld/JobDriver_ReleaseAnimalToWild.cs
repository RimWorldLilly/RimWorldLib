using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009A7 RID: 2471
	public class JobDriver_ReleaseAnimalToWild : JobDriver
	{
		// Token: 0x17000DA9 RID: 3497
		// (get) Token: 0x06004AD8 RID: 19160 RVA: 0x001D1EAC File Offset: 0x001D00AC
		protected Pawn Animal
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x06004AD9 RID: 19161 RVA: 0x001D1EC4 File Offset: 0x001D00C4
		public static bool TryFindClosestOutsideCell(IntVec3 root, Map map, TraverseParms traverseParms, Pawn pawn, out IntVec3 cell)
		{
			cell = default(IntVec3);
			Region region;
			if (CellFinder.TryFindClosestRegionWith(root.GetRegion(map, RegionType.Set_Passable), traverseParms, (Region r) => r.District.TouchesMapEdge, 99999, out region, RegionType.Set_Passable))
			{
				float num = float.PositiveInfinity;
				foreach (IntVec3 intVec in region.Cells)
				{
					if (intVec.Standable(map) && (pawn == null || !intVec.IsForbidden(pawn)))
					{
						float num2 = (float)intVec.DistanceToSquared(root);
						if (num2 < num)
						{
							num = num2;
							cell = intVec;
						}
					}
				}
				return num != float.PositiveInfinity;
			}
			return false;
		}

		// Token: 0x06004ADA RID: 19162 RVA: 0x001D1F90 File Offset: 0x001D0190
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004ADB RID: 19163 RVA: 0x001D1FB4 File Offset: 0x001D01B4
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ReleaseAnimalToWild.<MakeNewToils>d__6 <MakeNewToils>d__ = new JobDriver_ReleaseAnimalToWild.<MakeNewToils>d__6(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04002FAC RID: 12204
		private const TargetIndex AnimalInd = TargetIndex.A;

		// Token: 0x04002FAD RID: 12205
		private const int WaitForTicks = 180;
	}
}
