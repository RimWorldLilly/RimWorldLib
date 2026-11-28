using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C08 RID: 3080
	public class WorkGiver_ExtractTree : WorkGiver_RemoveBuilding
	{
		// Token: 0x17000F5E RID: 3934
		// (get) Token: 0x060057FD RID: 22525 RVA: 0x00209305 File Offset: 0x00207505
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.ExtractTree;
			}
		}

		// Token: 0x17000F5F RID: 3935
		// (get) Token: 0x060057FE RID: 22526 RVA: 0x0020930C File Offset: 0x0020750C
		protected override JobDef RemoveBuildingJob
		{
			get
			{
				return JobDefOf.ExtractTree;
			}
		}
	}
}
