using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F90 RID: 8080
	public class StatPart_IsFlesh : StatPart
	{
		// Token: 0x0600CD6A RID: 52586 RVA: 0x004B86C4 File Offset: 0x004B68C4
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetIsFleshFactor(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x0600CD6B RID: 52587 RVA: 0x004B86E4 File Offset: 0x004B68E4
		public override string ExplanationPart(StatRequest req)
		{
			float num;
			if (this.TryGetIsFleshFactor(req, out num) && num != 1f)
			{
				return "StatsReport_NotFlesh".Translate() + ": x" + num.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0600CD6C RID: 52588 RVA: 0x004B872C File Offset: 0x004B692C
		private bool TryGetIsFleshFactor(StatRequest req, out float bodySize)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, delegate(Pawn x)
			{
				if (!x.RaceProps.IsFlesh)
				{
					return 0f;
				}
				return 1f;
			}, delegate(ThingDef x)
			{
				if (!x.race.IsFlesh)
				{
					return 0f;
				}
				return 1f;
			}, out bodySize);
		}
	}
}
