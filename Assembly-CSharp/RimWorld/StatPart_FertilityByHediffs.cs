using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F84 RID: 8068
	public class StatPart_FertilityByHediffs : StatPart
	{
		// Token: 0x0600CD2F RID: 52527 RVA: 0x004B75DC File Offset: 0x004B57DC
		public override string ExplanationPart(StatRequest req)
		{
			Pawn pawn = req.Thing as Pawn;
			if (pawn == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder(32);
			StatPart_FertilityByHediffs.Factor(pawn, stringBuilder);
			return stringBuilder.ToString();
		}

		// Token: 0x0600CD30 RID: 52528 RVA: 0x004B7614 File Offset: 0x004B5814
		public override void TransformValue(StatRequest req, ref float val)
		{
			Pawn pawn = req.Thing as Pawn;
			if (pawn == null)
			{
				return;
			}
			val *= StatPart_FertilityByHediffs.Factor(pawn, null);
		}

		// Token: 0x0600CD31 RID: 52529 RVA: 0x004B7640 File Offset: 0x004B5840
		public static float Factor(Pawn pawn, StringBuilder explanation = null)
		{
			float num = 1f;
			foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
			{
				HediffStage curStage = hediff.CurStage;
				if (curStage != null && curStage.fertilityFactor != 1f)
				{
					num *= curStage.fertilityFactor;
					if (explanation != null)
					{
						explanation.AppendLine(hediff.LabelBaseCap + ": x" + curStage.fertilityFactor.ToStringPercent());
					}
				}
			}
			return Mathf.Max(num, 0f);
		}
	}
}
