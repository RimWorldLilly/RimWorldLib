using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C22 RID: 7202
	public class Alert_TatteredApparel : Alert_Thought
	{
		// Token: 0x17002125 RID: 8485
		// (get) Token: 0x0600B7B3 RID: 47027 RVA: 0x0040BFFB File Offset: 0x0040A1FB
		protected override ThoughtDef Thought
		{
			get
			{
				return ThoughtDefOf.ApparelDamaged;
			}
		}

		// Token: 0x0600B7B4 RID: 47028 RVA: 0x0040C002 File Offset: 0x0040A202
		public Alert_TatteredApparel()
		{
			this.defaultLabel = "AlertTatteredApparel".Translate();
			this.explanationKey = "AlertTatteredApparelDesc";
		}
	}
}
