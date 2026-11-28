using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FB1 RID: 8113
	public class StatPart_WornByCorpse : StatPart
	{
		// Token: 0x0600CDF2 RID: 52722 RVA: 0x004BA5D8 File Offset: 0x004B87D8
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Apparel apparel = req.Thing as Apparel;
				if (apparel != null && apparel.WornByCorpse)
				{
					val *= 0.1f;
				}
			}
		}

		// Token: 0x0600CDF3 RID: 52723 RVA: 0x004BA610 File Offset: 0x004B8810
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Apparel apparel = req.Thing as Apparel;
				if (apparel != null && apparel.WornByCorpse)
				{
					return "StatsReport_WornByCorpse".Translate() + ": x" + 0.1f.ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x04008342 RID: 33602
		private const float Factor = 0.1f;
	}
}
