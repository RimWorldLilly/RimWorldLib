using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F86 RID: 8070
	public class StatPart_GearAndInventoryMass : StatPart
	{
		// Token: 0x0600CD37 RID: 52535 RVA: 0x004B7824 File Offset: 0x004B5A24
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val += num;
			}
		}

		// Token: 0x0600CD38 RID: 52536 RVA: 0x004B7844 File Offset: 0x004B5A44
		public override string ExplanationPart(StatRequest req)
		{
			float mass;
			if (this.TryGetValue(req, out mass))
			{
				return "StatsReport_GearAndInventoryMass".Translate() + ": " + mass.ToStringMassOffset();
			}
			return null;
		}

		// Token: 0x0600CD39 RID: 52537 RVA: 0x004B7884 File Offset: 0x004B5A84
		private bool TryGetValue(StatRequest req, out float value)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => MassUtility.GearAndInventoryMass(x), (ThingDef x) => 0f, out value);
		}

		// Token: 0x0600CD3A RID: 52538 RVA: 0x004B78D6 File Offset: 0x004B5AD6
		public override bool ForceShow(StatRequest req)
		{
			return req.Pawn != null || req.Thing is Pawn;
		}
	}
}
