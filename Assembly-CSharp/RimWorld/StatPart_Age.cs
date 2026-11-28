using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F6C RID: 8044
	public class StatPart_Age : StatPart
	{
		// Token: 0x0600CCD4 RID: 52436 RVA: 0x004B63E6 File Offset: 0x004B45E6
		private bool ActiveFor(Pawn pawn)
		{
			return pawn.ageTracker != null && (!this.humanlikeOnly || pawn.RaceProps.Humanlike);
		}

		// Token: 0x0600CCD5 RID: 52437 RVA: 0x004B640C File Offset: 0x004B460C
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && this.ActiveFor(pawn))
				{
					val *= this.AgeMultiplier(pawn);
				}
			}
		}

		// Token: 0x0600CCD6 RID: 52438 RVA: 0x004B6448 File Offset: 0x004B4648
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && this.ActiveFor(pawn))
				{
					return "StatsReport_AgeMultiplier".Translate(pawn.ageTracker.AgeBiologicalYears) + ": x" + this.AgeMultiplier(pawn).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x0600CCD7 RID: 52439 RVA: 0x004B64B4 File Offset: 0x004B46B4
		private float AgeMultiplier(Pawn pawn)
		{
			if (!this.useBiologicalYears)
			{
				return this.curve.Evaluate((float)pawn.ageTracker.AgeBiologicalYears / pawn.RaceProps.lifeExpectancy);
			}
			return this.curve.Evaluate((float)pawn.ageTracker.AgeBiologicalYears);
		}

		// Token: 0x0600CCD8 RID: 52440 RVA: 0x004B6504 File Offset: 0x004B4704
		public override IEnumerable<string> ConfigErrors()
		{
			StatPart_Age.<ConfigErrors>d__7 <ConfigErrors>d__ = new StatPart_Age.<ConfigErrors>d__7(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x040082E7 RID: 33511
		private SimpleCurve curve;

		// Token: 0x040082E8 RID: 33512
		private bool useBiologicalYears;

		// Token: 0x040082E9 RID: 33513
		private bool humanlikeOnly;
	}
}
