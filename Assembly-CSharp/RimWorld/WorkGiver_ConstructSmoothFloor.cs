using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BC1 RID: 3009
	public class WorkGiver_ConstructSmoothFloor : WorkGiver_ConstructAffectFloor
	{
		// Token: 0x17000F25 RID: 3877
		// (get) Token: 0x060056AC RID: 22188 RVA: 0x00203275 File Offset: 0x00201475
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.SmoothFloor;
			}
		}

		// Token: 0x060056AD RID: 22189 RVA: 0x0020327C File Offset: 0x0020147C
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.SmoothFloor, c);
		}
	}
}
