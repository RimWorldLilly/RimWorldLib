using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001962 RID: 6498
	public class CompProperties_Sealable : CompProperties
	{
		// Token: 0x0600A316 RID: 41750 RVA: 0x003A4075 File Offset: 0x003A2275
		public CompProperties_Sealable()
		{
			this.compClass = typeof(CompSealable);
		}

		// Token: 0x0600A317 RID: 41751 RVA: 0x003A408D File Offset: 0x003A228D
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_Sealable.<ConfigErrors>d__9 <ConfigErrors>d__ = new CompProperties_Sealable.<ConfigErrors>d__9(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x04006056 RID: 24662
		[NoTranslate]
		public string sealTexPath;

		// Token: 0x04006057 RID: 24663
		[MustTranslate]
		public string sealCommandLabel;

		// Token: 0x04006058 RID: 24664
		[MustTranslate]
		public string sealCommandDesc;

		// Token: 0x04006059 RID: 24665
		[MustTranslate]
		public string cannotSealLabel;

		// Token: 0x0400605A RID: 24666
		[MustTranslate]
		public string confirmSealText;

		// Token: 0x0400605B RID: 24667
		[MustTranslate]
		public string sealedInspectString;

		// Token: 0x0400605C RID: 24668
		[MustTranslate]
		public string cannotEnterLabel;

		// Token: 0x0400605D RID: 24669
		public bool destroyPortal;
	}
}
