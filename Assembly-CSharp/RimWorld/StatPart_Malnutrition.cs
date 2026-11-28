using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F91 RID: 8081
	public class StatPart_Malnutrition : StatPart
	{
		// Token: 0x0600CD6E RID: 52590 RVA: 0x004B8788 File Offset: 0x004B6988
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			float num2;
			if (this.TryGetMalnutritionFactor(req, out num, out num2))
			{
				val *= num2;
			}
		}

		// Token: 0x0600CD6F RID: 52591 RVA: 0x004B87A8 File Offset: 0x004B69A8
		public override string ExplanationPart(StatRequest req)
		{
			float f;
			float f2;
			if (this.TryGetMalnutritionFactor(req, out f, out f2))
			{
				return "StatsReport_Malnutrition".Translate(f.ToStringPercent()) + ": x" + f2.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0600CD70 RID: 52592 RVA: 0x004B87F4 File Offset: 0x004B69F4
		private bool TryGetMalnutritionFactor(StatRequest req, out float malnutritionSeverity, out float factor)
		{
			factor = 0f;
			malnutritionSeverity = 0f;
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Malnutrition, false);
					if (firstHediffOfDef == null)
					{
						return false;
					}
					malnutritionSeverity = firstHediffOfDef.Severity;
					factor = this.curve.Evaluate(malnutritionSeverity);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400830B RID: 33547
		private SimpleCurve curve;
	}
}
