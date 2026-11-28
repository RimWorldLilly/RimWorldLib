using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F8D RID: 8077
	public class StatPart_Health : StatPart_Curve
	{
		// Token: 0x0600CD5E RID: 52574 RVA: 0x004B853E File Offset: 0x004B673E
		protected override bool AppliesTo(StatRequest req)
		{
			return req.HasThing && req.Thing.def.useHitPoints && req.Thing.def.healthAffectsPrice;
		}

		// Token: 0x0600CD5F RID: 52575 RVA: 0x004B856F File Offset: 0x004B676F
		protected override float CurveXGetter(StatRequest req)
		{
			return (float)req.Thing.HitPoints / (float)req.Thing.MaxHitPoints;
		}

		// Token: 0x0600CD60 RID: 52576 RVA: 0x004B858C File Offset: 0x004B678C
		protected override string ExplanationLabel(StatRequest req)
		{
			return "StatsReport_HealthMultiplier".Translate(req.Thing.HitPoints.ToString() + " / " + req.Thing.MaxHitPoints.ToString());
		}
	}
}
