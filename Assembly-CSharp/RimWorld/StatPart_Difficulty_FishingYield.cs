using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F80 RID: 8064
	public class StatPart_Difficulty_FishingYield : StatPart
	{
		// Token: 0x0600CD21 RID: 52513 RVA: 0x004B71A4 File Offset: 0x004B53A4
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num = (Find.Storyteller != null) ? Find.Storyteller.difficulty.fishingYieldFactor : 1f;
			val *= num;
		}

		// Token: 0x0600CD22 RID: 52514 RVA: 0x004B71D8 File Offset: 0x004B53D8
		public override string ExplanationPart(StatRequest req)
		{
			return "StatsReport_DifficultyMultiplier".Translate(Find.Storyteller.difficultyDef.label) + ": " + Find.Storyteller.difficulty.fishingYieldFactor.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor);
		}
	}
}
