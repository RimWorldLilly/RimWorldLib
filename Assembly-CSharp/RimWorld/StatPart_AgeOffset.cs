using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F6D RID: 8045
	public class StatPart_AgeOffset : StatPart
	{
		// Token: 0x0600CCDA RID: 52442 RVA: 0x004B651C File Offset: 0x004B471C
		private bool ActiveFor(Pawn pawn)
		{
			return pawn.ageTracker != null && (!this.humanlikeOnly || pawn.RaceProps.Humanlike);
		}

		// Token: 0x0600CCDB RID: 52443 RVA: 0x004B6540 File Offset: 0x004B4740
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && this.ActiveFor(pawn))
				{
					val += this.AgeOffset(pawn);
				}
			}
		}

		// Token: 0x0600CCDC RID: 52444 RVA: 0x004B657C File Offset: 0x004B477C
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && this.ActiveFor(pawn))
				{
					return "StatsReport_AgeOffset".Translate(pawn.ageTracker.AgeBiologicalYears) + ": " + this.AgeOffset(pawn).ToStringByStyle(this.parentStat.toStringStyle, this.parentStat.toStringNumberSense);
				}
			}
			return null;
		}

		// Token: 0x0600CCDD RID: 52445 RVA: 0x004B6600 File Offset: 0x004B4800
		private float AgeOffset(Pawn pawn)
		{
			if (!this.useBiologicalYears)
			{
				return this.curve.Evaluate((float)pawn.ageTracker.AgeBiologicalYears / pawn.RaceProps.lifeExpectancy);
			}
			return this.curve.Evaluate((float)pawn.ageTracker.AgeBiologicalYears);
		}

		// Token: 0x0600CCDE RID: 52446 RVA: 0x004B6650 File Offset: 0x004B4850
		public override IEnumerable<string> ConfigErrors()
		{
			StatPart_AgeOffset.<ConfigErrors>d__7 <ConfigErrors>d__ = new StatPart_AgeOffset.<ConfigErrors>d__7(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x040082EA RID: 33514
		private SimpleCurve curve;

		// Token: 0x040082EB RID: 33515
		private bool useBiologicalYears;

		// Token: 0x040082EC RID: 33516
		private bool humanlikeOnly;
	}
}
