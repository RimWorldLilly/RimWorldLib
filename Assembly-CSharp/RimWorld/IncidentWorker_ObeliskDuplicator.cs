using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011A8 RID: 4520
	public class IncidentWorker_ObeliskDuplicator : IncidentWorker_Obelisk
	{
		// Token: 0x17001336 RID: 4918
		// (get) Token: 0x060070FC RID: 28924 RVA: 0x00278826 File Offset: 0x00276A26
		public override ThingDef ObeliskDef
		{
			get
			{
				return ThingDefOf.WarpedObelisk_Duplicator;
			}
		}
	}
}
