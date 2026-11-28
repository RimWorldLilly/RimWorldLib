using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F7D RID: 8061
	public class StatPart_Deathresting : StatPart
	{
		// Token: 0x0600CD17 RID: 52503 RVA: 0x004B7004 File Offset: 0x004B5204
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.Deathresting)
				{
					val *= this.factor;
				}
			}
		}

		// Token: 0x0600CD18 RID: 52504 RVA: 0x004B7040 File Offset: 0x004B5240
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.Deathresting)
				{
					return "Deathresting".Translate().CapitalizeFirst() + ": x" + this.factor.ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x040082F4 RID: 33524
		public float factor;
	}
}
