using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015BD RID: 5565
	public class ReadingPolicy : Policy
	{
		// Token: 0x170017CD RID: 6093
		// (get) Token: 0x060089F3 RID: 35315 RVA: 0x0031EBE4 File Offset: 0x0031CDE4
		protected override string LoadKey
		{
			get
			{
				return "ReadingPolicy";
			}
		}

		// Token: 0x060089F4 RID: 35316 RVA: 0x0031EBEB File Offset: 0x0031CDEB
		public ReadingPolicy()
		{
		}

		// Token: 0x060089F5 RID: 35317 RVA: 0x0031EBFE File Offset: 0x0031CDFE
		public ReadingPolicy(int id, string label) : base(id, label)
		{
			this.defFilter.SetAllowAll(null, true);
			this.effectFilter = new ThingFilter(ThingCategoryDefOf.BookEffects, true);
		}

		// Token: 0x060089F6 RID: 35318 RVA: 0x0031EC34 File Offset: 0x0031CE34
		public override void CopyFrom(Policy other)
		{
			ReadingPolicy readingPolicy = other as ReadingPolicy;
			if (readingPolicy == null)
			{
				return;
			}
			this.defFilter.CopyAllowancesFrom(readingPolicy.defFilter);
			this.effectFilter.CopyAllowancesFrom(readingPolicy.effectFilter);
		}

		// Token: 0x060089F7 RID: 35319 RVA: 0x0031EC6E File Offset: 0x0031CE6E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingFilter>(ref this.defFilter, "defFilter", Array.Empty<object>());
			Scribe_Deep.Look<ThingFilter>(ref this.effectFilter, "effectFilter", Array.Empty<object>());
		}

		// Token: 0x040054E5 RID: 21733
		public ThingFilter defFilter = new ThingFilter();

		// Token: 0x040054E6 RID: 21734
		public ThingFilter effectFilter;
	}
}
