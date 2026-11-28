using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BFA RID: 3066
	public class WorkGiver_ClearPollution : WorkGiver_Scanner
	{
		// Token: 0x17000F4F RID: 3919
		// (get) Token: 0x060057A1 RID: 22433 RVA: 0x0020721D File Offset: 0x0020541D
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060057A2 RID: 22434 RVA: 0x00207220 File Offset: 0x00205420
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			return pawn.Map.areaManager.PollutionClear.ActiveCells;
		}

		// Token: 0x060057A3 RID: 22435 RVA: 0x00207237 File Offset: 0x00205437
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.areaManager.PollutionClear.TrueCount == 0;
		}

		// Token: 0x060057A4 RID: 22436 RVA: 0x00207251 File Offset: 0x00205451
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return ModLister.CheckBiotech("Clear pollution") && pawn.Map.pollutionGrid.IsPolluted(c) && pawn.CanReserve(c, 1, -1, null, forced);
		}

		// Token: 0x060057A5 RID: 22437 RVA: 0x0020728B File Offset: 0x0020548B
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.ClearPollution, c);
		}
	}
}
