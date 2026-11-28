using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CA2 RID: 7330
	public static class CustomXenogermUtility
	{
		// Token: 0x0600BBA7 RID: 48039 RVA: 0x0042A26C File Offset: 0x0042846C
		public static AcceptanceReport SaveXenogermTemplate(string xenoTypeName, XenotypeIconDef iconDef, List<Genepack> selectedGenepacks)
		{
			if (selectedGenepacks.NullOrEmpty<Genepack>())
			{
				return "CannotSaveXenogermTemplate".Translate("GenepackPlural".Translate());
			}
			if (xenoTypeName.NullOrEmpty())
			{
				return "CannotSaveXenogermTemplate".Translate("XenotypeName".Translate());
			}
			CustomXenogerm customXenogerm = new CustomXenogerm(xenoTypeName, iconDef, selectedGenepacks);
			Find.CustomXenogermDatabase.Add(customXenogerm);
			Messages.Message("XenogermTemplateSaved".Translate(xenoTypeName.Named("NAME")), MessageTypeDefOf.PositiveEvent, false);
			return true;
		}

		// Token: 0x0600BBA8 RID: 48040 RVA: 0x0042A306 File Offset: 0x00428506
		public static IEnumerable<Genepack> GetMatchingGenepacks(IEnumerable<GeneSet> templateGenesets, IEnumerable<Genepack> genepacks)
		{
			CustomXenogermUtility.<GetMatchingGenepacks>d__2 <GetMatchingGenepacks>d__ = new CustomXenogermUtility.<GetMatchingGenepacks>d__2(-2);
			<GetMatchingGenepacks>d__.<>3__templateGenesets = templateGenesets;
			<GetMatchingGenepacks>d__.<>3__genepacks = genepacks;
			return <GetMatchingGenepacks>d__;
		}

		// Token: 0x04006AEE RID: 27374
		private static List<Genepack> tmpGenepacks = new List<Genepack>();
	}
}
