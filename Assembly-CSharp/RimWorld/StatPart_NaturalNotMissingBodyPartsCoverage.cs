using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F94 RID: 8084
	public class StatPart_NaturalNotMissingBodyPartsCoverage : StatPart
	{
		// Token: 0x0600CD7C RID: 52604 RVA: 0x004B89CC File Offset: 0x004B6BCC
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x0600CD7D RID: 52605 RVA: 0x004B89EC File Offset: 0x004B6BEC
		public override string ExplanationPart(StatRequest req)
		{
			float f;
			if (this.TryGetValue(req, out f))
			{
				return "StatsReport_MissingBodyParts".Translate() + ": x" + f.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0600CD7E RID: 52606 RVA: 0x004B8A2C File Offset: 0x004B6C2C
		private bool TryGetValue(StatRequest req, out float value)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => x.health.hediffSet.GetCoverageOfNotMissingNaturalParts(x.RaceProps.body.corePart), (ThingDef x) => 1f, out value);
		}
	}
}
