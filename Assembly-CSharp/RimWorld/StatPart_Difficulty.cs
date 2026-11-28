using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F7E RID: 8062
	[Obsolete]
	public class StatPart_Difficulty : StatPart
	{
		// Token: 0x0600CD1A RID: 52506 RVA: 0x004B70A9 File Offset: 0x004B52A9
		public override void TransformValue(StatRequest req, ref float val)
		{
			val *= this.Multiplier(Find.Storyteller.difficultyDef);
		}

		// Token: 0x0600CD1B RID: 52507 RVA: 0x004B70C0 File Offset: 0x004B52C0
		public override string ExplanationPart(StatRequest req)
		{
			return "StatsReport_DifficultyMultiplier".Translate() + ": x" + this.Multiplier(Find.Storyteller.difficultyDef).ToStringPercent();
		}

		// Token: 0x0600CD1C RID: 52508 RVA: 0x004B70F5 File Offset: 0x004B52F5
		private float Multiplier(DifficultyDef d)
		{
			return 1f;
		}

		// Token: 0x040082F5 RID: 33525
		private List<float> factorsPerDifficulty = new List<float>();
	}
}
