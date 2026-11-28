using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011A7 RID: 4519
	public class IncidentWorker_ObeliskMutator : IncidentWorker_Obelisk
	{
		// Token: 0x17001335 RID: 4917
		// (get) Token: 0x060070FA RID: 28922 RVA: 0x00278817 File Offset: 0x00276A17
		public override ThingDef ObeliskDef
		{
			get
			{
				return ThingDefOf.WarpedObelisk_Mutator;
			}
		}
	}
}
