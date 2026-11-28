using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FAB RID: 8107
	public class StatPart_Terror : StatPart
	{
		// Token: 0x0600CDDB RID: 52699 RVA: 0x004BA0F0 File Offset: 0x004B82F0
		public override void TransformValue(StatRequest req, ref float val)
		{
			Pawn pawn = req.Thing as Pawn;
			if (pawn == null)
			{
				return;
			}
			val += TerrorUtility.SuppressionFallRateOverTerror.Evaluate(pawn.GetStatValue(StatDefOf.Terror, true, -1));
		}

		// Token: 0x0600CDDC RID: 52700 RVA: 0x004BA12C File Offset: 0x004B832C
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && !Mathf.Approximately(TerrorUtility.SuppressionFallRateOverTerror.Evaluate(pawn.GetStatValue(StatDefOf.Terror, true, -1)), 0f))
				{
					return "StatsReport_Terror".Translate() + (": " + TerrorUtility.SuppressionFallRateOverTerror.Evaluate(pawn.GetStatValue(StatDefOf.Terror, true, -1)).ToStringPercent());
				}
			}
			return null;
		}
	}
}
