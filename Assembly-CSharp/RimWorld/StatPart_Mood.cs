using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F93 RID: 8083
	public class StatPart_Mood : StatPart
	{
		// Token: 0x0600CD76 RID: 52598 RVA: 0x004B88D1 File Offset: 0x004B6AD1
		public override IEnumerable<string> ConfigErrors()
		{
			StatPart_Mood.<ConfigErrors>d__1 <ConfigErrors>d__ = new StatPart_Mood.<ConfigErrors>d__1(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x0600CD77 RID: 52599 RVA: 0x004B88E4 File Offset: 0x004B6AE4
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && this.ActiveFor(pawn))
				{
					val *= this.FactorFromMood(pawn);
				}
			}
		}

		// Token: 0x0600CD78 RID: 52600 RVA: 0x004B8920 File Offset: 0x004B6B20
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && this.ActiveFor(pawn))
				{
					return "StatsReport_MoodMultiplier".Translate(pawn.needs.mood.CurLevel.ToStringPercent()) + ": x" + this.FactorFromMood(pawn).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x0600CD79 RID: 52601 RVA: 0x004B8995 File Offset: 0x004B6B95
		private bool ActiveFor(Pawn pawn)
		{
			return pawn.needs.mood != null;
		}

		// Token: 0x0600CD7A RID: 52602 RVA: 0x004B89A5 File Offset: 0x004B6BA5
		private float FactorFromMood(Pawn pawn)
		{
			return this.factorFromMoodCurve.Evaluate(pawn.needs.mood.CurLevel);
		}

		// Token: 0x0400830C RID: 33548
		private SimpleCurve factorFromMoodCurve;
	}
}
