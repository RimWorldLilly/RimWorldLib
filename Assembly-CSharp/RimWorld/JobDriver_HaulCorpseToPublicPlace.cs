using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A3E RID: 2622
	public class JobDriver_HaulCorpseToPublicPlace : JobDriver
	{
		// Token: 0x17000E5C RID: 3676
		// (get) Token: 0x06004F28 RID: 20264 RVA: 0x001DFF8C File Offset: 0x001DE18C
		private Corpse Corpse
		{
			get
			{
				return (Corpse)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000E5D RID: 3677
		// (get) Token: 0x06004F29 RID: 20265 RVA: 0x001DFFB4 File Offset: 0x001DE1B4
		private Building_Grave Grave
		{
			get
			{
				return (Building_Grave)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x17000E5E RID: 3678
		// (get) Token: 0x06004F2A RID: 20266 RVA: 0x001DFFDA File Offset: 0x001DE1DA
		private bool InGrave
		{
			get
			{
				return this.Grave != null;
			}
		}

		// Token: 0x17000E5F RID: 3679
		// (get) Token: 0x06004F2B RID: 20267 RVA: 0x001DFFE5 File Offset: 0x001DE1E5
		private Thing Target
		{
			get
			{
				return this.Grave ?? this.Corpse;
			}
		}

		// Token: 0x06004F2C RID: 20268 RVA: 0x001DFFF7 File Offset: 0x001DE1F7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Target, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004F2D RID: 20269 RVA: 0x001E001A File Offset: 0x001DE21A
		public override string GetReport()
		{
			if (this.InGrave && this.Grave.def == ThingDefOf.Grave)
			{
				return "ReportDiggingUpCorpse".Translate();
			}
			return base.GetReport();
		}

		// Token: 0x06004F2E RID: 20270 RVA: 0x001E004C File Offset: 0x001DE24C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_HaulCorpseToPublicPlace.<MakeNewToils>d__13 <MakeNewToils>d__ = new JobDriver_HaulCorpseToPublicPlace.<MakeNewToils>d__13(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004F2F RID: 20271 RVA: 0x001E005C File Offset: 0x001DE25C
		private Toil FindCellToDropCorpseToil()
		{
			Toil toil = ToilMaker.MakeToil("FindCellToDropCorpseToil");
			toil.initAction = delegate()
			{
				IntVec3 c = IntVec3.Invalid;
				if (!Rand.Chance(0.8f) || !this.TryFindTableCell(out c))
				{
					bool flag = false;
					IntVec3 root;
					if (RCellFinder.TryFindRandomSpotJustOutsideColony(this.pawn, out root) && CellFinder.TryRandomClosewalkCellNear(root, this.pawn.Map, 5, out c, (IntVec3 x) => this.pawn.CanReserve(x, 1, -1, null, false) && x.GetFirstItem(this.pawn.Map) == null))
					{
						flag = true;
					}
					if (!flag)
					{
						c = CellFinder.RandomClosewalkCellNear(this.pawn.Position, this.pawn.Map, 10, (IntVec3 x) => this.pawn.CanReserve(x, 1, -1, null, false) && x.GetFirstItem(this.pawn.Map) == null);
					}
				}
				this.job.SetTarget(TargetIndex.C, c);
			};
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x06004F30 RID: 20272 RVA: 0x001E0081 File Offset: 0x001DE281
		private Toil ForbidAndNotifyMentalStateToil()
		{
			Toil toil = ToilMaker.MakeToil("ForbidAndNotifyMentalStateToil");
			toil.initAction = delegate()
			{
				Corpse corpse = this.Corpse;
				if (corpse != null)
				{
					corpse.SetForbidden(true, true);
				}
				MentalState_CorpseObsession mentalState_CorpseObsession = this.pawn.MentalState as MentalState_CorpseObsession;
				if (mentalState_CorpseObsession != null)
				{
					mentalState_CorpseObsession.Notify_CorpseHauled();
				}
			};
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x06004F31 RID: 20273 RVA: 0x001E00A8 File Offset: 0x001DE2A8
		private bool TryFindTableCell(out IntVec3 cell)
		{
			JobDriver_HaulCorpseToPublicPlace.tmpCells.Clear();
			List<Building> allBuildingsColonist = this.pawn.Map.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				Building building = allBuildingsColonist[i];
				if (building.def.IsTable)
				{
					foreach (IntVec3 intVec in building.OccupiedRect())
					{
						if (this.pawn.CanReserveAndReach(intVec, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, false) && intVec.GetFirstItem(this.pawn.Map) == null)
						{
							JobDriver_HaulCorpseToPublicPlace.tmpCells.Add(intVec);
						}
					}
				}
			}
			if (!JobDriver_HaulCorpseToPublicPlace.tmpCells.Any<IntVec3>())
			{
				cell = IntVec3.Invalid;
				return false;
			}
			cell = JobDriver_HaulCorpseToPublicPlace.tmpCells.RandomElement<IntVec3>();
			return true;
		}

		// Token: 0x040030E2 RID: 12514
		private const TargetIndex CorpseInd = TargetIndex.A;

		// Token: 0x040030E3 RID: 12515
		private const TargetIndex GraveInd = TargetIndex.B;

		// Token: 0x040030E4 RID: 12516
		private const TargetIndex CellInd = TargetIndex.C;

		// Token: 0x040030E5 RID: 12517
		private static List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
