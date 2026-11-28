using System;

namespace RimWorld
{
	// Token: 0x02000DD7 RID: 3543
	public class ThoughtWorker_Precept_ChildLabor_ChildAssignedRecreation : ThoughtWorker_Precept_ChildLabor
	{
		// Token: 0x170010A3 RID: 4259
		// (get) Token: 0x06006088 RID: 24712 RVA: 0x0022FA88 File Offset: 0x0022DC88
		protected override TimeAssignmentDef AssignmentDef
		{
			get
			{
				return TimeAssignmentDefOf.Joy;
			}
		}
	}
}
