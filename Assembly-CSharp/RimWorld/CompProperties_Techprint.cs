using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020019AD RID: 6573
	public class CompProperties_Techprint : CompProperties
	{
		// Token: 0x0600A500 RID: 42240 RVA: 0x003AEA77 File Offset: 0x003ACC77
		public CompProperties_Techprint()
		{
			this.compClass = typeof(CompTechprint);
		}

		// Token: 0x0600A501 RID: 42241 RVA: 0x003AEA90 File Offset: 0x003ACC90
		public override void ResolveReferences(ThingDef parentDef)
		{
			if (parentDef.descriptionHyperlinks == null)
			{
				parentDef.descriptionHyperlinks = new List<DefHyperlink>();
			}
			List<Def> unlockedDefs = this.project.UnlockedDefs;
			for (int i = 0; i < unlockedDefs.Count; i++)
			{
				ThingDef thingDef = unlockedDefs[i] as ThingDef;
				if (thingDef != null)
				{
					parentDef.descriptionHyperlinks.Add(thingDef);
				}
				else
				{
					RecipeDef recipeDef = unlockedDefs[i] as RecipeDef;
					if (recipeDef != null && !recipeDef.products.NullOrEmpty<ThingDefCountClass>())
					{
						for (int j = 0; j < recipeDef.products.Count; j++)
						{
							parentDef.descriptionHyperlinks.Add(recipeDef.products[j].thingDef);
						}
					}
				}
			}
			parentDef.description += "\n\n" + "Unlocks".Translate() + ": " + (from x in this.project.UnlockedDefs
			select x.label).ToCommaList(false, false).CapitalizeFirst();
		}

		// Token: 0x0400617E RID: 24958
		public ResearchProjectDef project;
	}
}
