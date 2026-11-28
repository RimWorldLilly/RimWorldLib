using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C1F RID: 7199
	public class Alert_Analyzable_NeuralLump : Alert_Analyzable
	{
		// Token: 0x17002121 RID: 8481
		// (get) Token: 0x0600B7A8 RID: 47016 RVA: 0x0040BD8F File Offset: 0x00409F8F
		protected override ThingDef Def
		{
			get
			{
				return ThingDefOf.FleshmassNeuralLump;
			}
		}

		// Token: 0x0600B7A9 RID: 47017 RVA: 0x0040BD96 File Offset: 0x00409F96
		public Alert_Analyzable_NeuralLump()
		{
			this.requireAnomaly = true;
			this.defaultLabel = "AlertNeuralLump".Translate();
			this.defaultExplanation = "AlertNeuralLumpDesc".Translate();
		}
	}
}
