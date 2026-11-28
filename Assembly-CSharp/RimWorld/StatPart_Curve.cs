using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F7C RID: 8060
	public abstract class StatPart_Curve : StatPart
	{
		// Token: 0x17002541 RID: 9537
		// (get) Token: 0x0600CD10 RID: 52496 RVA: 0x004B6F88 File Offset: 0x004B5188
		public SimpleCurve Curve
		{
			get
			{
				return this.curve;
			}
		}

		// Token: 0x0600CD11 RID: 52497
		protected abstract bool AppliesTo(StatRequest req);

		// Token: 0x0600CD12 RID: 52498
		protected abstract float CurveXGetter(StatRequest req);

		// Token: 0x0600CD13 RID: 52499
		protected abstract string ExplanationLabel(StatRequest req);

		// Token: 0x0600CD14 RID: 52500 RVA: 0x004B6F90 File Offset: 0x004B5190
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && this.AppliesTo(req))
			{
				val *= this.curve.Evaluate(this.CurveXGetter(req));
			}
		}

		// Token: 0x0600CD15 RID: 52501 RVA: 0x004B6FBB File Offset: 0x004B51BB
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && this.AppliesTo(req))
			{
				return this.ExplanationLabel(req) + ": x" + this.curve.Evaluate(this.CurveXGetter(req)).ToStringPercent();
			}
			return null;
		}

		// Token: 0x040082F3 RID: 33523
		protected SimpleCurve curve;
	}
}
