using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200193E RID: 6462
	public class CompProducesBioferrite : ThingComp
	{
		// Token: 0x17001C01 RID: 7169
		// (get) Token: 0x0600A206 RID: 41478 RVA: 0x0039E5E0 File Offset: 0x0039C7E0
		public CompProperties_ProducesBioferrite Props
		{
			get
			{
				return (CompProperties_ProducesBioferrite)this.props;
			}
		}

		// Token: 0x0600A207 RID: 41479 RVA: 0x0039E5ED File Offset: 0x0039C7ED
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			CompProducesBioferrite.<SpecialDisplayStats>d__2 <SpecialDisplayStats>d__ = new CompProducesBioferrite.<SpecialDisplayStats>d__2(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x0600A208 RID: 41480 RVA: 0x0039E600 File Offset: 0x0039C800
		public static float BioferritePerDay(Pawn pawn)
		{
			CompProducesBioferrite compProducesBioferrite = pawn.TryGetComp<CompProducesBioferrite>();
			if (compProducesBioferrite == null && (!pawn.IsMutant || !pawn.mutant.Def.producesBioferrite))
			{
				return 0f;
			}
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.BioferriteExtracted, false))
			{
				return 0f;
			}
			float num = (compProducesBioferrite != null) ? compProducesBioferrite.Props.bioferriteDensity : 1f;
			return pawn.BodySize * num;
		}

		// Token: 0x0600A209 RID: 41481 RVA: 0x0039E674 File Offset: 0x0039C874
		public static StatDrawEntry BioferriteStatDrawEntry(Pawn pawn)
		{
			CompProducesBioferrite compProducesBioferrite = pawn.TryGetComp<CompProducesBioferrite>();
			StringBuilder stringBuilder = new StringBuilder("StatsReport_BioferriteGeneration_Desc".Translate());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("StatsReport_BaseValue".Translate() + ": 1");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("StatsReport_BodySize".Translate(pawn.BodySize.ToString("F2")) + ": x" + pawn.BodySize.ToStringPercent());
			if (compProducesBioferrite != null)
			{
				stringBuilder.AppendLine("StatsReport_BioferriteDensityMultiplier".Translate() + ": x" + compProducesBioferrite.Props.bioferriteDensity.ToStringPercent());
			}
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.BioferriteExtracted, false))
			{
				stringBuilder.AppendLine("StatsReport_BioferriteExtractedMultiplier".Translate() + ": x" + 0f.ToStringPercent());
			}
			stringBuilder.AppendLine();
			stringBuilder.Append("StatsReport_FinalValue".Translate() + ": " + CompProducesBioferrite.BioferritePerDay(pawn).ToString("F1"));
			return new StatDrawEntry(StatCategoryDefOf.Containment, "StatsReport_BioferriteGeneration".Translate(), CompProducesBioferrite.BioferritePerDay(pawn).ToString(), stringBuilder.ToString(), 100, null, null, false, false);
		}
	}
}
