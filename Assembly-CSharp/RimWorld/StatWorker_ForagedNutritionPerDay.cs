using System;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FD4 RID: 8148
	public class StatWorker_ForagedNutritionPerDay : StatWorker
	{
		// Token: 0x0600CEBC RID: 52924 RVA: 0x004C18E4 File Offset: 0x004BFAE4
		public override float GetBaseValueFor(StatRequest req)
		{
			Pawn pawn = req.Pawn ?? (req.Thing as Pawn);
			float result = base.GetBaseValueFor(req);
			if (ModsConfig.OdysseyActive && pawn != null)
			{
				Pawn_TrainingTracker training = pawn.training;
				if (training != null && training.HasLearned(TrainableDefOf.Forage))
				{
					result = pawn.BodySize * 0.6f;
				}
			}
			return result;
		}

		// Token: 0x0600CEBD RID: 52925 RVA: 0x004C1944 File Offset: 0x004BFB44
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			Pawn pawn = req.Pawn ?? (req.Thing as Pawn);
			StringBuilder stringBuilder = new StringBuilder();
			float num = this.GetBaseValueFor(req);
			if (ModsConfig.OdysseyActive && pawn != null && pawn.IsAnimal)
			{
				Pawn_TrainingTracker training = pawn.training;
				if (training != null && training.HasLearned(TrainableDefOf.Forage))
				{
					num = pawn.BodySize * 0.6f;
					StringBuilder stringBuilder2 = stringBuilder;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
					defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("ForageTrained".Translate());
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted(num.ToStringByStyle(this.stat.ToStringStyleUnfinalized, numberSense));
					stringBuilder2.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
					stringBuilder.AppendLine();
					goto IL_109;
				}
			}
			if (num != 0f || this.stat.showZeroBaseValue)
			{
				stringBuilder.AppendLine("StatsReport_BaseValue".Translate() + ": " + this.stat.ValueToString(num, numberSense, true));
				stringBuilder.AppendLine();
			}
			IL_109:
			base.GetOffsetsAndFactorsExplanation(req, stringBuilder, num, "");
			return stringBuilder.ToString();
		}

		// Token: 0x0600CEBE RID: 52926 RVA: 0x004C1A70 File Offset: 0x004BFC70
		public override bool ShouldShowFor(StatRequest req)
		{
			Pawn pawn = req.Pawn ?? (req.Thing as Pawn);
			if (pawn != null && pawn.IsAnimal && pawn.training != null)
			{
				return ModsConfig.OdysseyActive && pawn.training.HasLearned(TrainableDefOf.Forage);
			}
			return base.ShouldShowFor(req);
		}

		// Token: 0x04008363 RID: 33635
		private const float ForgeBodySizeFactor = 0.6f;
	}
}
