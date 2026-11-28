using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F78 RID: 8056
	public class StatPart_NearHarbingerTree : StatPart
	{
		// Token: 0x0600CD03 RID: 52483 RVA: 0x004B6DD6 File Offset: 0x004B4FD6
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.ActiveFor(req.Thing))
			{
				val *= this.multiplier;
			}
		}

		// Token: 0x0600CD04 RID: 52484 RVA: 0x004B6DF4 File Offset: 0x004B4FF4
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				return "StatsReport_MultiplierFor".Translate(ThingDefOf.Plant_TreeHarbinger.label) + (": x" + this.multiplier.ToStringPercent());
			}
			return null;
		}

		// Token: 0x0600CD05 RID: 52485 RVA: 0x004B6E54 File Offset: 0x004B5054
		private bool ActiveFor(Thing t)
		{
			if (t == null)
			{
				return false;
			}
			CompHarbingerTreeConsumable compHarbingerTreeConsumable = t.TryGetComp<CompHarbingerTreeConsumable>();
			return compHarbingerTreeConsumable != null && compHarbingerTreeConsumable.BeingConsumed;
		}

		// Token: 0x040082F0 RID: 33520
		private float multiplier = 1f;
	}
}
