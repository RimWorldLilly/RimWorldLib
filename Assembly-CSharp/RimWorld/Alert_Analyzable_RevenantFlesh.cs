using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C20 RID: 7200
	public class Alert_Analyzable_RevenantFlesh : Alert_Analyzable
	{
		// Token: 0x17002122 RID: 8482
		// (get) Token: 0x0600B7AA RID: 47018 RVA: 0x0040BDCF File Offset: 0x00409FCF
		protected override ThingDef Def
		{
			get
			{
				return ThingDefOf.RevenantFleshChunk;
			}
		}

		// Token: 0x0600B7AB RID: 47019 RVA: 0x0040BDD6 File Offset: 0x00409FD6
		public Alert_Analyzable_RevenantFlesh()
		{
			this.requireAnomaly = true;
			this.defaultLabel = "AlertRevenantFlesh".Translate();
			this.defaultExplanation = "AlertRevenantFleshDesc".Translate();
		}
	}
}
