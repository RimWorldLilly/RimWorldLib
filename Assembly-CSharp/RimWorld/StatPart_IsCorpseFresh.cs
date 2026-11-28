using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F8F RID: 8079
	public class StatPart_IsCorpseFresh : StatPart
	{
		// Token: 0x0600CD66 RID: 52582 RVA: 0x004B8604 File Offset: 0x004B6804
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetIsFreshFactor(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x0600CD67 RID: 52583 RVA: 0x004B8624 File Offset: 0x004B6824
		public override string ExplanationPart(StatRequest req)
		{
			float num;
			if (this.TryGetIsFreshFactor(req, out num) && num != 1f)
			{
				return "StatsReport_NotFresh".Translate() + ": x" + num.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0600CD68 RID: 52584 RVA: 0x004B866C File Offset: 0x004B686C
		private bool TryGetIsFreshFactor(StatRequest req, out float factor)
		{
			if (!req.HasThing)
			{
				factor = 1f;
				return false;
			}
			Corpse corpse = req.Thing as Corpse;
			if (corpse == null)
			{
				factor = 1f;
				return false;
			}
			factor = ((corpse.GetRotStage() == RotStage.Fresh) ? 1f : 0f);
			return true;
		}
	}
}
