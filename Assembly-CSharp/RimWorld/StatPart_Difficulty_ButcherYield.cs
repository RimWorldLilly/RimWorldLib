using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F7F RID: 8063
	public class StatPart_Difficulty_ButcherYield : StatPart
	{
		// Token: 0x0600CD1E RID: 52510 RVA: 0x004B7110 File Offset: 0x004B5310
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num = (Find.Storyteller != null) ? Find.Storyteller.difficulty.butcherYieldFactor : 1f;
			val *= num;
		}

		// Token: 0x0600CD1F RID: 52511 RVA: 0x004B7144 File Offset: 0x004B5344
		public override string ExplanationPart(StatRequest req)
		{
			return "StatsReport_DifficultyMultiplier".Translate(Find.Storyteller.difficultyDef.label) + ": " + Find.Storyteller.difficulty.butcherYieldFactor.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor);
		}
	}
}
