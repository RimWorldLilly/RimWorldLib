using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F72 RID: 8050
	public abstract class StatPart_BiostatTotal : StatPart_Curve
	{
		// Token: 0x1700253D RID: 9533
		// (get) Token: 0x0600CCEF RID: 52463
		public abstract string ExplanationLabelBase { get; }

		// Token: 0x0600CCF0 RID: 52464 RVA: 0x004B6950 File Offset: 0x004B4B50
		protected override bool AppliesTo(StatRequest req)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					return pawn.genes != null;
				}
			}
			return false;
		}

		// Token: 0x0600CCF1 RID: 52465 RVA: 0x004B698A File Offset: 0x004B4B8A
		protected override string ExplanationLabel(StatRequest req)
		{
			return this.ExplanationLabelBase + " " + this.CurveXGetter(req).ToStringByStyle(ToStringStyle.Integer, ToStringNumberSense.Absolute);
		}
	}
}
