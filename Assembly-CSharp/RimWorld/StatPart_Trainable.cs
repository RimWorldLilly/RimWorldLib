using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FAD RID: 8109
	public class StatPart_Trainable : StatPart
	{
		// Token: 0x0600CDE2 RID: 52706 RVA: 0x004BA2AD File Offset: 0x004B84AD
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.Applies(req))
			{
				val *= this.factor;
			}
		}

		// Token: 0x0600CDE3 RID: 52707 RVA: 0x004BA2C4 File Offset: 0x004B84C4
		public override string ExplanationPart(StatRequest req)
		{
			if (this.Applies(req))
			{
				return "StatsReport_Trainable".Translate(this.trainableDef.LabelCap) + (": " + this.factor.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor));
			}
			return null;
		}

		// Token: 0x0600CDE4 RID: 52708 RVA: 0x004BA318 File Offset: 0x004B8518
		private bool Applies(StatRequest req)
		{
			Pawn pawn = req.Thing as Pawn;
			if (pawn == null || !pawn.IsAnimal)
			{
				return false;
			}
			Pawn_TrainingTracker training = pawn.training;
			return training != null && training.HasLearned(TrainableDefOf.Dig);
		}

		// Token: 0x0600CDE5 RID: 52709 RVA: 0x004BA355 File Offset: 0x004B8555
		public override bool ForceShow(StatRequest req)
		{
			return this.Applies(req);
		}

		// Token: 0x0400833F RID: 33599
		public TrainableDef trainableDef;

		// Token: 0x04008340 RID: 33600
		public float factor = 1f;
	}
}
