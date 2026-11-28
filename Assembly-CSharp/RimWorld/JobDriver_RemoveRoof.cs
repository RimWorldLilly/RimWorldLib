using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009D1 RID: 2513
	public class JobDriver_RemoveRoof : JobDriver_AffectRoof
	{
		// Token: 0x17000DE6 RID: 3558
		// (get) Token: 0x06004C4C RID: 19532 RVA: 0x001D6C07 File Offset: 0x001D4E07
		protected override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x06004C4D RID: 19533 RVA: 0x001D6C0A File Offset: 0x001D4E0A
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_RemoveRoof.<MakeNewToils>d__2 <MakeNewToils>d__ = new JobDriver_RemoveRoof.<MakeNewToils>d__2(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004C4E RID: 19534 RVA: 0x001D6C1C File Offset: 0x001D4E1C
		protected override void DoEffect()
		{
			JobDriver_RemoveRoof.removedRoofs.Clear();
			base.Map.roofGrid.SetRoof(base.Cell, null);
			JobDriver_RemoveRoof.removedRoofs.Add(base.Cell);
			RoofCollapseCellsFinder.CheckCollapseFlyingRoofs(JobDriver_RemoveRoof.removedRoofs, base.Map, true, false);
			JobDriver_RemoveRoof.removedRoofs.Clear();
		}

		// Token: 0x06004C4F RID: 19535 RVA: 0x001D6C76 File Offset: 0x001D4E76
		protected override bool DoWorkFailOn()
		{
			return !base.Cell.Roofed(base.Map);
		}

		// Token: 0x04003006 RID: 12294
		private static List<IntVec3> removedRoofs = new List<IntVec3>();
	}
}
