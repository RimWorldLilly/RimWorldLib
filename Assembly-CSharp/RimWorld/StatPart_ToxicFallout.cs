using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FAC RID: 8108
	public class StatPart_ToxicFallout : StatPart
	{
		// Token: 0x0600CDDE RID: 52702 RVA: 0x004BA1B9 File Offset: 0x004B83B9
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.ActiveFor(req.Thing))
			{
				val *= this.multiplier;
			}
		}

		// Token: 0x0600CDDF RID: 52703 RVA: 0x004BA1D8 File Offset: 0x004B83D8
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				return "StatsReport_MultiplierFor".Translate(GameConditionDefOf.ToxicFallout.label) + (": x" + this.multiplier.ToStringPercent());
			}
			return null;
		}

		// Token: 0x0600CDE0 RID: 52704 RVA: 0x004BA238 File Offset: 0x004B8438
		private bool ActiveFor(Thing t)
		{
			return t != null && t.def.deteriorateFromEnvironmentalEffects && t.MapHeld != null && t.MapHeld.gameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout) && t.PositionHeld.IsValid && !t.PositionHeld.Roofed(t.MapHeld);
		}

		// Token: 0x0400833E RID: 33598
		private float multiplier = 1f;
	}
}
