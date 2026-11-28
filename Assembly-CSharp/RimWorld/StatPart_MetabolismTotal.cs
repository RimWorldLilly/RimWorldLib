using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F73 RID: 8051
	public class StatPart_MetabolismTotal : StatPart_BiostatTotal
	{
		// Token: 0x1700253E RID: 9534
		// (get) Token: 0x0600CCF3 RID: 52467 RVA: 0x004B69B2 File Offset: 0x004B4BB2
		public override string ExplanationLabelBase
		{
			get
			{
				return "FactorForMetabolism".Translate();
			}
		}

		// Token: 0x0600CCF4 RID: 52468 RVA: 0x004B69C4 File Offset: 0x004B4BC4
		protected override float CurveXGetter(StatRequest req)
		{
			Pawn pawn = (Pawn)req.Thing;
			int num = 0;
			foreach (Gene gene in pawn.genes.GenesListForReading)
			{
				if (!gene.Overridden)
				{
					num += gene.def.biostatMet;
				}
			}
			return (float)num;
		}
	}
}
