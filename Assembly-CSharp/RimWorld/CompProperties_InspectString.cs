using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018C5 RID: 6341
	public class CompProperties_InspectString : CompProperties
	{
		// Token: 0x06009F01 RID: 40705 RVA: 0x00390EAE File Offset: 0x0038F0AE
		public CompProperties_InspectString()
		{
			this.compClass = typeof(CompInspectString);
		}

		// Token: 0x04005E18 RID: 24088
		[MustTranslate]
		public string inspectString;
	}
}
