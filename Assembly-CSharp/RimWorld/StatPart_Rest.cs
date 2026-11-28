using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FA0 RID: 8096
	public class StatPart_Rest : StatPart
	{
		// Token: 0x0600CDAF RID: 52655 RVA: 0x004B9694 File Offset: 0x004B7894
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.needs.rest != null)
				{
					val *= this.RestMultiplier(pawn.needs.rest.CurCategory);
				}
			}
		}

		// Token: 0x0600CDB0 RID: 52656 RVA: 0x004B96E4 File Offset: 0x004B78E4
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.needs.rest != null)
				{
					return pawn.needs.rest.CurCategory.GetLabel() + ": x" + this.RestMultiplier(pawn.needs.rest.CurCategory).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x0600CDB1 RID: 52657 RVA: 0x004B9753 File Offset: 0x004B7953
		private float RestMultiplier(RestCategory fatigue)
		{
			switch (fatigue)
			{
			case RestCategory.Rested:
				return this.factorRested;
			case RestCategory.Tired:
				return this.factorTired;
			case RestCategory.VeryTired:
				return this.factorVeryTired;
			case RestCategory.Exhausted:
				return this.factorExhausted;
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x0400832D RID: 33581
		private float factorExhausted = 1f;

		// Token: 0x0400832E RID: 33582
		private float factorVeryTired = 1f;

		// Token: 0x0400832F RID: 33583
		private float factorTired = 1f;

		// Token: 0x04008330 RID: 33584
		private float factorRested = 1f;
	}
}
