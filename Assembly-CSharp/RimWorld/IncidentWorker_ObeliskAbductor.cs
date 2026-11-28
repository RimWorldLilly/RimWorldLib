using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011A9 RID: 4521
	public class IncidentWorker_ObeliskAbductor : IncidentWorker_Obelisk
	{
		// Token: 0x17001337 RID: 4919
		// (get) Token: 0x060070FE RID: 28926 RVA: 0x00278835 File Offset: 0x00276A35
		public override ThingDef ObeliskDef
		{
			get
			{
				return ThingDefOf.WarpedObelisk_Abductor;
			}
		}
	}
}
