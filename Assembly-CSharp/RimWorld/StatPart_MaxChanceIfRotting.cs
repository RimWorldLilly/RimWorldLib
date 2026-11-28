using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F92 RID: 8082
	public class StatPart_MaxChanceIfRotting : StatPart
	{
		// Token: 0x0600CD72 RID: 52594 RVA: 0x004B8866 File Offset: 0x004B6A66
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.IsRotting(req))
			{
				val = 1f;
			}
		}

		// Token: 0x0600CD73 RID: 52595 RVA: 0x004B8878 File Offset: 0x004B6A78
		public override string ExplanationPart(StatRequest req)
		{
			if (this.IsRotting(req))
			{
				return "StatsReport_NotFresh".Translate() + ": " + 1f.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0600CD74 RID: 52596 RVA: 0x004B88AD File Offset: 0x004B6AAD
		private bool IsRotting(StatRequest req)
		{
			return req.HasThing && req.Thing.GetRotStage() > RotStage.Fresh;
		}
	}
}
