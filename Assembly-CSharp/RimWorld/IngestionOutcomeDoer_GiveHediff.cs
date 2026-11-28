using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F02 RID: 3842
	public class IngestionOutcomeDoer_GiveHediff : IngestionOutcomeDoer
	{
		// Token: 0x060063EC RID: 25580 RVA: 0x0023CD1C File Offset: 0x0023AF1C
		protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
		{
			Hediff hediff = HediffMaker.MakeHediff(this.hediffDef, pawn, null);
			float initialSeverity;
			if (this.severity > 0f)
			{
				initialSeverity = this.severity;
			}
			else
			{
				initialSeverity = this.hediffDef.initialSeverity;
			}
			AddictionUtility.ModifyChemicalEffectForToleranceAndBodySize(pawn, this.toleranceChemical, ref initialSeverity, this.multiplyByGeneToleranceFactors, this.divideByBodySize);
			hediff.Severity = initialSeverity;
			pawn.health.AddHediff(hediff, null, null, null);
		}

		// Token: 0x060063ED RID: 25581 RVA: 0x0023CD91 File Offset: 0x0023AF91
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			IngestionOutcomeDoer_GiveHediff.<SpecialDisplayStats>d__6 <SpecialDisplayStats>d__ = new IngestionOutcomeDoer_GiveHediff.<SpecialDisplayStats>d__6(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			<SpecialDisplayStats>d__.<>3__parentDef = parentDef;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x040038C0 RID: 14528
		public HediffDef hediffDef;

		// Token: 0x040038C1 RID: 14529
		public float severity = -1f;

		// Token: 0x040038C2 RID: 14530
		public ChemicalDef toleranceChemical;

		// Token: 0x040038C3 RID: 14531
		private bool divideByBodySize;

		// Token: 0x040038C4 RID: 14532
		public bool multiplyByGeneToleranceFactors;
	}
}
