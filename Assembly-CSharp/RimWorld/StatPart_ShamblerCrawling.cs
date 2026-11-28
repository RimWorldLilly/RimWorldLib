using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FA6 RID: 8102
	public class StatPart_ShamblerCrawling : StatPart
	{
		// Token: 0x0600CDC7 RID: 52679 RVA: 0x004B9C0C File Offset: 0x004B7E0C
		private bool ActiveFor(Thing t)
		{
			if (!t.Spawned)
			{
				return false;
			}
			Pawn pawn = t as Pawn;
			return pawn != null && pawn.IsShambler && pawn.Crawling;
		}

		// Token: 0x0600CDC8 RID: 52680 RVA: 0x004B9C3D File Offset: 0x004B7E3D
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (!req.HasThing)
			{
				return;
			}
			if (this.ActiveFor(req.Thing))
			{
				val *= this.factor;
			}
		}

		// Token: 0x0600CDC9 RID: 52681 RVA: 0x004B9C64 File Offset: 0x004B7E64
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				return "StatsReport_ShamblerCrawling".Translate() + ": x" + this.factor.ToStringPercent();
			}
			return null;
		}

		// Token: 0x04008336 RID: 33590
		public float factor;
	}
}
