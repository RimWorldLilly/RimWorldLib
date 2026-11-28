using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FB0 RID: 8112
	public class StatPart_WildManOffset : StatPart
	{
		// Token: 0x0600CDEE RID: 52718 RVA: 0x004BA555 File Offset: 0x004B8755
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.IsWildMan(req))
			{
				val += this.offset;
			}
		}

		// Token: 0x0600CDEF RID: 52719 RVA: 0x004BA56B File Offset: 0x004B876B
		public override string ExplanationPart(StatRequest req)
		{
			if (this.IsWildMan(req))
			{
				return "StatsReport_WildMan".Translate() + ": " + this.offset.ToStringWithSign("0.##");
			}
			return null;
		}

		// Token: 0x0600CDF0 RID: 52720 RVA: 0x004BA5A8 File Offset: 0x004B87A8
		private bool IsWildMan(StatRequest req)
		{
			Pawn pawn = req.Thing as Pawn;
			return pawn != null && pawn.IsWildMan();
		}

		// Token: 0x04008341 RID: 33601
		public float offset;
	}
}
