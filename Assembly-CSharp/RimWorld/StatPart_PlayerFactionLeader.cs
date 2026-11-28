using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F9B RID: 8091
	public class StatPart_PlayerFactionLeader : StatPart
	{
		// Token: 0x0600CD99 RID: 52633 RVA: 0x004B8FC0 File Offset: 0x004B71C0
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.PawnIsLeader(req))
			{
				val += this.offset;
			}
		}

		// Token: 0x0600CD9A RID: 52634 RVA: 0x004B8FD8 File Offset: 0x004B71D8
		private bool PawnIsLeader(StatRequest req)
		{
			Thing thing = req.Thing;
			if (thing == null)
			{
				return false;
			}
			Faction faction = thing.Faction;
			return faction != null && faction.IsPlayer && faction.leader == thing;
		}

		// Token: 0x0600CD9B RID: 52635 RVA: 0x004B900F File Offset: 0x004B720F
		public override string ExplanationPart(StatRequest req)
		{
			if (this.PawnIsLeader(req))
			{
				return "StatsReport_LeaderOffset".Translate() + ": " + this.offset.ToStringWithSign("0.#%");
			}
			return null;
		}

		// Token: 0x04008314 RID: 33556
		private float offset;
	}
}
