using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F89 RID: 8073
	public class StatPart_Genes : StatPart
	{
		// Token: 0x0600CD49 RID: 52553 RVA: 0x004B7F0C File Offset: 0x004B610C
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			if (req.HasThing)
			{
				Genepack genepack = req.Thing as Genepack;
				if (genepack != null && genepack.GeneSet != null)
				{
					val *= this.NumGenesFactor(genepack.GeneSet);
					val *= this.ArchiteCostFactor(genepack.GeneSet);
					val *= this.GenesFactorsDefined(genepack.GeneSet);
				}
			}
		}

		// Token: 0x0600CD4A RID: 52554 RVA: 0x004B7F75 File Offset: 0x004B6175
		private float NumGenesFactor(GeneSet geneSet)
		{
			return Mathf.Max(3.5f - 0.5f * (float)geneSet.GenesListForReading.Count, 0.5f);
		}

		// Token: 0x0600CD4B RID: 52555 RVA: 0x004B7F99 File Offset: 0x004B6199
		private float ArchiteCostFactor(GeneSet geneSet)
		{
			return 1f + 3f * (float)geneSet.ArchitesTotal;
		}

		// Token: 0x0600CD4C RID: 52556 RVA: 0x004B7FB0 File Offset: 0x004B61B0
		private float GenesFactorsDefined(GeneSet geneSet)
		{
			float num = 1f;
			for (int i = 0; i < geneSet.GenesListForReading.Count; i++)
			{
				num *= geneSet.GenesListForReading[i].marketValueFactor;
			}
			return num;
		}

		// Token: 0x0600CD4D RID: 52557 RVA: 0x004B7FF0 File Offset: 0x004B61F0
		public override string ExplanationPart(StatRequest req)
		{
			string text = null;
			if (!ModsConfig.BiotechActive)
			{
				return text;
			}
			if (req.HasThing)
			{
				Genepack genepack = req.Thing as Genepack;
				if (genepack != null && genepack.GeneSet != null)
				{
					int count = genepack.GeneSet.GenesListForReading.Count;
					if (count > 0)
					{
						if (!text.NullOrEmpty())
						{
							text += "\n";
						}
						string str = text;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 3);
						defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("NumberOfGenes".Translate());
						defaultInterpolatedStringHandler.AppendLiteral(" (");
						defaultInterpolatedStringHandler.AppendFormatted<int>(count);
						defaultInterpolatedStringHandler.AppendLiteral("): x");
						defaultInterpolatedStringHandler.AppendFormatted(this.NumGenesFactor(genepack.GeneSet).ToStringPercent());
						text = str + defaultInterpolatedStringHandler.ToStringAndClear();
					}
					int architesTotal = genepack.GeneSet.ArchitesTotal;
					if (architesTotal != 0)
					{
						if (!text.NullOrEmpty())
						{
							text += "\n";
						}
						string str2 = text;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(6, 3);
						defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("ArchitesTotal".Translate());
						defaultInterpolatedStringHandler2.AppendLiteral(" (");
						defaultInterpolatedStringHandler2.AppendFormatted<int>(architesTotal);
						defaultInterpolatedStringHandler2.AppendLiteral("): x");
						defaultInterpolatedStringHandler2.AppendFormatted(this.ArchiteCostFactor(genepack.GeneSet).ToStringPercent());
						text = str2 + defaultInterpolatedStringHandler2.ToStringAndClear();
					}
					if (this.GenesFactorsDefined(genepack.GeneSet) != 1f)
					{
						if (!text.NullOrEmpty())
						{
							text += "\n";
						}
						text += "GenePriceFactors".Translate() + ":";
						for (int i = 0; i < genepack.GeneSet.GenesListForReading.Count; i++)
						{
							GeneDef geneDef = genepack.GeneSet.GenesListForReading[i];
							if (geneDef.marketValueFactor != 1f)
							{
								string str3 = text;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(7, 2);
								defaultInterpolatedStringHandler3.AppendLiteral("\n  - ");
								defaultInterpolatedStringHandler3.AppendFormatted<TaggedString>(geneDef.LabelCap);
								defaultInterpolatedStringHandler3.AppendLiteral(" x");
								defaultInterpolatedStringHandler3.AppendFormatted(geneDef.marketValueFactor.ToStringPercent());
								text = str3 + defaultInterpolatedStringHandler3.ToStringAndClear();
							}
						}
					}
					return text.TrimEndNewlines();
				}
			}
			return text;
		}
	}
}
