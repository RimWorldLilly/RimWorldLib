using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C0E RID: 3086
	public class WorkGiver_FillIn : WorkGiver_RemoveBuilding
	{
		// Token: 0x17000F68 RID: 3944
		// (get) Token: 0x06005821 RID: 22561 RVA: 0x00209B84 File Offset: 0x00207D84
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.FillIn;
			}
		}

		// Token: 0x17000F69 RID: 3945
		// (get) Token: 0x06005822 RID: 22562 RVA: 0x00209B8B File Offset: 0x00207D8B
		protected override JobDef RemoveBuildingJob
		{
			get
			{
				return JobDefOf.FillIn;
			}
		}
	}
}
