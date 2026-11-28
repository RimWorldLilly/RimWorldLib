using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F99 RID: 8089
	public class StatPart_Pain : StatPart
	{
		// Token: 0x0600CD91 RID: 52625 RVA: 0x004B8E00 File Offset: 0x004B7000
		public override void TransformValue(StatRequest req, ref float val)
		{
			Pawn pawn = req.Thing as Pawn;
			if (pawn == null)
			{
				return;
			}
			val *= this.PainFactor(pawn);
		}

		// Token: 0x0600CD92 RID: 52626 RVA: 0x004B8E2A File Offset: 0x004B702A
		public float PainFactor(Pawn pawn)
		{
			return 1f + pawn.health.hediffSet.PainTotal * this.factor;
		}

		// Token: 0x0600CD93 RID: 52627 RVA: 0x004B8E4C File Offset: 0x004B704C
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					return "StatsReport_Pain".Translate() + (": " + this.PainFactor(pawn).ToStringPercent("F0"));
				}
			}
			return null;
		}

		// Token: 0x04008313 RID: 33555
		private float factor = 1f;
	}
}
