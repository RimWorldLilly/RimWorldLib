using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018BB RID: 6331
	public class CompProperties_Hatcher : CompProperties
	{
		// Token: 0x06009EB4 RID: 40628 RVA: 0x0038F35F File Offset: 0x0038D55F
		public CompProperties_Hatcher()
		{
			this.compClass = typeof(CompHatcher);
		}

		// Token: 0x04005DE8 RID: 24040
		public float hatcherDaystoHatch = 1f;

		// Token: 0x04005DE9 RID: 24041
		public PawnKindDef hatcherPawn;
	}
}
