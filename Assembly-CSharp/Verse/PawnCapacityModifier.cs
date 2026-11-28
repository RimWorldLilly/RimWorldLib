using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000176 RID: 374
	public class PawnCapacityModifier
	{
		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000B09 RID: 2825 RVA: 0x0003C53F File Offset: 0x0003A73F
		public bool SetMaxDefined
		{
			get
			{
				return !Mathf.Approximately(this.setMax, 999f) || (this.setMaxCurveOverride != null && this.setMaxCurveEvaluateStat != null);
			}
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x0003C568 File Offset: 0x0003A768
		public float EvaluateSetMax(Pawn pawn)
		{
			if (this.setMaxCurveOverride == null || this.setMaxCurveEvaluateStat == null)
			{
				return this.setMax;
			}
			return this.setMaxCurveOverride.Evaluate(pawn.GetStatValue(this.setMaxCurveEvaluateStat, true, -1));
		}

		// Token: 0x040008DA RID: 2266
		public PawnCapacityDef capacity;

		// Token: 0x040008DB RID: 2267
		public float offset;

		// Token: 0x040008DC RID: 2268
		public float setMax = 999f;

		// Token: 0x040008DD RID: 2269
		public float postFactor = 1f;

		// Token: 0x040008DE RID: 2270
		public StatDef statFactorMod;

		// Token: 0x040008DF RID: 2271
		public SimpleCurve setMaxCurveOverride;

		// Token: 0x040008E0 RID: 2272
		public StatDef setMaxCurveEvaluateStat;
	}
}
