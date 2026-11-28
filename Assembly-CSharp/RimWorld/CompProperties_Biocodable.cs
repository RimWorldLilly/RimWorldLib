using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001829 RID: 6185
	public class CompProperties_Biocodable : CompProperties
	{
		// Token: 0x06009AB5 RID: 39605 RVA: 0x0037CEED File Offset: 0x0037B0ED
		public CompProperties_Biocodable()
		{
			this.compClass = typeof(CompBiocodable);
		}

		// Token: 0x04005C04 RID: 23556
		public bool biocodeOnEquip;
	}
}
