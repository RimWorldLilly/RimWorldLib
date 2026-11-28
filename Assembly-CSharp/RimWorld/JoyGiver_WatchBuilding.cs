using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B72 RID: 2930
	public class JoyGiver_WatchBuilding : JoyGiver_InteractBuilding
	{
		// Token: 0x0600553A RID: 21818 RVA: 0x001FAE98 File Offset: 0x001F9098
		protected override bool CanInteractWith(Pawn pawn, Thing t, bool inBed)
		{
			if (!base.CanInteractWith(pawn, t, inBed))
			{
				return false;
			}
			if (inBed)
			{
				Building_Bed bed = pawn.CurrentBed();
				return WatchBuildingUtility.CanWatchFromBed(pawn, bed, t);
			}
			return true;
		}

		// Token: 0x0600553B RID: 21819 RVA: 0x001FAEC8 File Offset: 0x001F90C8
		protected override Job TryGivePlayJob(Pawn pawn, Thing t)
		{
			IntVec3 c;
			Building t2;
			if (!WatchBuildingUtility.TryFindBestWatchCell(t, pawn, this.def.desireSit, out c, out t2))
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, t, c, t2);
		}
	}
}
