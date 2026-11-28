using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200101D RID: 4125
	public class CustomXenogerm : IExposable
	{
		// Token: 0x060067E4 RID: 26596 RVA: 0x0024AD8C File Offset: 0x00248F8C
		public CustomXenogerm()
		{
		}

		// Token: 0x060067E5 RID: 26597 RVA: 0x0024ADA0 File Offset: 0x00248FA0
		public CustomXenogerm(string name, XenotypeIconDef iconDef, IEnumerable<Genepack> genepacks)
		{
			this.genesets.AddRange(from gp in genepacks
			select gp.GeneSet.Copy());
			this.iconDef = iconDef;
			this.name = name;
		}

		// Token: 0x060067E6 RID: 26598 RVA: 0x0024ADFC File Offset: 0x00248FFC
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Collections.Look<GeneSet>(ref this.genesets, "genesets", LookMode.Deep, Array.Empty<object>());
			Scribe_Defs.Look<XenotypeIconDef>(ref this.iconDef, "iconDef");
		}

		// Token: 0x04004245 RID: 16965
		public string name;

		// Token: 0x04004246 RID: 16966
		public List<GeneSet> genesets = new List<GeneSet>();

		// Token: 0x04004247 RID: 16967
		public XenotypeIconDef iconDef;
	}
}
