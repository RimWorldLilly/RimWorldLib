using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C1E RID: 7198
	public class Alert_Analyzable_GrayFlesh : Alert_Analyzable
	{
		// Token: 0x17002120 RID: 8480
		// (get) Token: 0x0600B7A6 RID: 47014 RVA: 0x0040BD4F File Offset: 0x00409F4F
		protected override ThingDef Def
		{
			get
			{
				return ThingDefOf.GrayFleshSample;
			}
		}

		// Token: 0x0600B7A7 RID: 47015 RVA: 0x0040BD56 File Offset: 0x00409F56
		public Alert_Analyzable_GrayFlesh()
		{
			this.requireAnomaly = true;
			this.defaultLabel = "AlertGrayFleshSample".Translate();
			this.defaultExplanation = "AlertGrayFleshSampleDesc".Translate();
		}
	}
}
