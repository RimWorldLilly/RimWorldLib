using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F8C RID: 8076
	public class StatPart_HasRelic : StatPart
	{
		// Token: 0x0600CD5A RID: 52570 RVA: 0x004B8494 File Offset: 0x004B6694
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				val += this.offset;
			}
		}

		// Token: 0x0600CD5B RID: 52571 RVA: 0x004B84BC File Offset: 0x004B66BC
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				return "StatsReports_HasRelic".Translate() + ": " + this.offset.ToStringWithSign("0.##");
			}
			return null;
		}

		// Token: 0x0600CD5C RID: 52572 RVA: 0x004B8514 File Offset: 0x004B6714
		private bool ActiveFor(Thing thing)
		{
			CompRelicContainer compRelicContainer = thing.TryGetComp<CompRelicContainer>();
			return compRelicContainer != null && compRelicContainer.ContainedThing != null;
		}

		// Token: 0x04008309 RID: 33545
		public float offset;
	}
}
