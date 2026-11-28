using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BFB RID: 3067
	public class WorkGiver_ClearSnowOrSand : WorkGiver_Scanner
	{
		// Token: 0x060057A7 RID: 22439 RVA: 0x002072A5 File Offset: 0x002054A5
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			return pawn.Map.areaManager.SnowOrSandClear.ActiveCells;
		}

		// Token: 0x17000F50 RID: 3920
		// (get) Token: 0x060057A8 RID: 22440 RVA: 0x002072BC File Offset: 0x002054BC
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060057A9 RID: 22441 RVA: 0x002072BF File Offset: 0x002054BF
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.areaManager.SnowOrSandClear.TrueCount == 0;
		}

		// Token: 0x060057AA RID: 22442 RVA: 0x002072DC File Offset: 0x002054DC
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return (pawn.Map.snowGrid.GetDepth(c) >= 0.2f || c.GetSandDepth(pawn.Map) >= 0.2f) && pawn.CanReserve(c, 1, -1, null, forced);
		}

		// Token: 0x060057AB RID: 22443 RVA: 0x0020732B File Offset: 0x0020552B
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.ClearSnow, c);
		}

		// Token: 0x060057AC RID: 22444 RVA: 0x0020733D File Offset: 0x0020553D
		private bool IsSand(IntVec3 cell, Map map)
		{
			return cell.GetSandDepth(map) > map.snowGrid.GetDepth(cell);
		}

		// Token: 0x060057AD RID: 22445 RVA: 0x00207354 File Offset: 0x00205554
		public override string PostProcessedGerund(Job job)
		{
			if (this.IsSand(job.targetA.Cell, Find.CurrentMap))
			{
				return "ClearingSandGerund".Translate();
			}
			return base.PostProcessedGerund(job);
		}
	}
}
