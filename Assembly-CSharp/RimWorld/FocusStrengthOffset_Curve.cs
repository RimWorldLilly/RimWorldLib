using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020018FA RID: 6394
	public abstract class FocusStrengthOffset_Curve : FocusStrengthOffset
	{
		// Token: 0x0600A051 RID: 41041
		protected abstract float SourceValue(Thing parent);

		// Token: 0x17001BA2 RID: 7074
		// (get) Token: 0x0600A052 RID: 41042
		protected abstract string ExplanationKey { get; }

		// Token: 0x0600A053 RID: 41043 RVA: 0x003964EA File Offset: 0x003946EA
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			return Mathf.Round(this.curve.Evaluate(this.SourceValue(parent)) * 100f) / 100f;
		}

		// Token: 0x0600A054 RID: 41044 RVA: 0x0039650F File Offset: 0x0039470F
		public override string GetExplanation(Thing parent)
		{
			return this.ExplanationKey.Translate() + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
		}

		// Token: 0x0600A055 RID: 41045 RVA: 0x00396544 File Offset: 0x00394744
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return this.ExplanationKey.Translate() + ": " + (this.curve[0].y.ToStringWithSign("0%") + " " + "RangeTo".Translate() + " " + this.curve[this.curve.PointsCount - 1].y.ToStringWithSign("0%"));
		}

		// Token: 0x0600A056 RID: 41046 RVA: 0x003965E0 File Offset: 0x003947E0
		public override float MaxOffset(Thing parent = null)
		{
			float num = 0f;
			for (int i = 0; i < this.curve.PointsCount; i++)
			{
				float y = this.curve[i].y;
				if (Mathf.Abs(y) > Mathf.Abs(num))
				{
					num = y;
				}
			}
			return num;
		}

		// Token: 0x04005EC7 RID: 24263
		public SimpleCurve curve;
	}
}
