using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200192E RID: 6446
	public class CompProperties_Plantable : CompProperties
	{
		// Token: 0x0600A1B5 RID: 41397 RVA: 0x0039CDFC File Offset: 0x0039AFFC
		public CompProperties_Plantable()
		{
			this.compClass = typeof(CompPlantable);
		}

		// Token: 0x04005F8F RID: 24463
		public ThingDef plantDefToSpawn;
	}
}
