using System;

namespace RimWorld
{
	// Token: 0x02000DD8 RID: 3544
	public class ThoughtWorker_Precept_ChildLabor_ChildAssignedWork : ThoughtWorker_Precept_ChildLabor
	{
		// Token: 0x170010A4 RID: 4260
		// (get) Token: 0x0600608A RID: 24714 RVA: 0x0022FA97 File Offset: 0x0022DC97
		protected override TimeAssignmentDef AssignmentDef
		{
			get
			{
				return TimeAssignmentDefOf.Work;
			}
		}
	}
}
