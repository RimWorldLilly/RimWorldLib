using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F83 RID: 8067
	public class StatPart_FertilityByGenderAge : StatPart
	{
		// Token: 0x0600CD2B RID: 52523 RVA: 0x004B7530 File Offset: 0x004B5730
		public override string ExplanationPart(StatRequest req)
		{
			Pawn pawn = req.Thing as Pawn;
			if (pawn != null && pawn != null)
			{
				return "StatsReport_FertilityAgeFactor".Translate() + ": x" + this.AgeFactor(pawn).ToStringPercent();
			}
			return null;
		}

		// Token: 0x0600CD2C RID: 52524 RVA: 0x004B757C File Offset: 0x004B577C
		public override void TransformValue(StatRequest req, ref float val)
		{
			Pawn pawn = req.Thing as Pawn;
			if (pawn != null && pawn != null)
			{
				val *= this.AgeFactor(pawn);
			}
		}

		// Token: 0x0600CD2D RID: 52525 RVA: 0x004B75A8 File Offset: 0x004B57A8
		private float AgeFactor(Pawn pawn)
		{
			return ((pawn.gender == Gender.Female) ? this.femaleFertilityAgeFactor : this.maleFertilityAgeFactor).Evaluate(pawn.ageTracker.AgeBiologicalYearsFloat);
		}

		// Token: 0x040082F9 RID: 33529
		protected SimpleCurve maleFertilityAgeFactor;

		// Token: 0x040082FA RID: 33530
		protected SimpleCurve femaleFertilityAgeFactor;
	}
}
