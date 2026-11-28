using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FA8 RID: 8104
	public class StatPart_Slave : StatPart
	{
		// Token: 0x0600CDCF RID: 52687 RVA: 0x004B9DD2 File Offset: 0x004B7FD2
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.ActiveFor(req.Thing))
			{
				val *= this.factor;
			}
		}

		// Token: 0x0600CDD0 RID: 52688 RVA: 0x004B9DF0 File Offset: 0x004B7FF0
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				return "StatsReport_Slave".Translate() + (": x" + this.factor.ToStringPercent());
			}
			return null;
		}

		// Token: 0x0600CDD1 RID: 52689 RVA: 0x004B9E40 File Offset: 0x004B8040
		private bool ActiveFor(Thing t)
		{
			Pawn pawn = t as Pawn;
			return pawn != null && pawn.IsSlave;
		}

		// Token: 0x0400833B RID: 33595
		private float factor = 0.75f;
	}
}
