using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200183A RID: 6202
	public class CompProperties_BiosignatureOwner : CompProperties
	{
		// Token: 0x06009B45 RID: 39749 RVA: 0x0037FD09 File Offset: 0x0037DF09
		public CompProperties_BiosignatureOwner()
		{
			this.compClass = typeof(CompBiosignatureOwner);
		}

		// Token: 0x04005C50 RID: 23632
		public bool requiresAnalysis = true;
	}
}
