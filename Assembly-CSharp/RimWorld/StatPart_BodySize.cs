using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F76 RID: 8054
	public class StatPart_BodySize : StatPart
	{
		// Token: 0x0600CCFE RID: 52478 RVA: 0x004B6C70 File Offset: 0x004B4E70
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetBodySize(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x0600CCFF RID: 52479 RVA: 0x004B6C90 File Offset: 0x004B4E90
		public override string ExplanationPart(StatRequest req)
		{
			float f;
			if (this.TryGetBodySize(req, out f))
			{
				return "StatsReport_BodySize".Translate(f.ToString("F2")) + ": x" + f.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0600CD00 RID: 52480 RVA: 0x004B6CE0 File Offset: 0x004B4EE0
		private bool TryGetBodySize(StatRequest req, out float bodySize)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => x.BodySize, (ThingDef x) => x.race.baseBodySize, out bodySize);
		}
	}
}
