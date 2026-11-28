using System;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FBA RID: 8122
	public class StatWorker_SuppressionFallRate : StatWorker
	{
		// Token: 0x0600CE56 RID: 52822 RVA: 0x004BEA78 File Offset: 0x004BCC78
		public override bool ShouldShowFor(StatRequest req)
		{
			if (!base.ShouldShowFor(req))
			{
				return false;
			}
			Pawn pawn = req.Thing as Pawn;
			return pawn != null && pawn.IsSlave;
		}

		// Token: 0x0600CE57 RID: 52823 RVA: 0x004BEAA8 File Offset: 0x004BCCA8
		private static float CurrentFallRateBasedOnSuppression(float suppression)
		{
			if (suppression > 0.3f)
			{
				return 0.2f;
			}
			if (suppression > 0.15f)
			{
				return 0.1f;
			}
			return 0.05f;
		}

		// Token: 0x0600CE58 RID: 52824 RVA: 0x004BEACC File Offset: 0x004BCCCC
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			Need_Suppression need_Suppression;
			((Pawn)req.Thing).needs.TryGetNeed<Need_Suppression>(out need_Suppression);
			return StatWorker_SuppressionFallRate.CurrentFallRateBasedOnSuppression(need_Suppression.CurLevelPercentage);
		}

		// Token: 0x0600CE59 RID: 52825 RVA: 0x004BEB00 File Offset: 0x004BCD00
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			Need_Suppression need_Suppression;
			((Pawn)req.Thing).needs.TryGetNeed<Need_Suppression>(out need_Suppression);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetExplanationUnfinalized(req, numberSense));
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 3);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("CurrentSuppression".Translate());
			defaultInterpolatedStringHandler.AppendLiteral(" (");
			defaultInterpolatedStringHandler.AppendFormatted(need_Suppression.CurLevelPercentage.ToStringPercent());
			defaultInterpolatedStringHandler.AppendLiteral("): ");
			defaultInterpolatedStringHandler.AppendFormatted(StatWorker_SuppressionFallRate.CurrentFallRateBasedOnSuppression(need_Suppression.CurLevelPercentage).ToStringPercent());
			stringBuilder.Append(defaultInterpolatedStringHandler.ToStringAndClear());
			return stringBuilder.ToString();
		}

		// Token: 0x0600CE5A RID: 52826 RVA: 0x004BEBA8 File Offset: 0x004BCDA8
		public string GetExplanationForTooltip(StatRequest req)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("SuppressionFallRate".Translate() + ": " + base.GetValue(req.Thing, true, -1).ToStringPercent());
			Need_Suppression need_Suppression;
			((Pawn)req.Thing).needs.TryGetNeed<Need_Suppression>(out need_Suppression);
			StringBuilder stringBuilder2 = stringBuilder;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 3);
			defaultInterpolatedStringHandler.AppendLiteral("   ");
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("CurrentSuppression".Translate());
			defaultInterpolatedStringHandler.AppendLiteral(" (");
			defaultInterpolatedStringHandler.AppendFormatted(need_Suppression.CurLevelPercentage.ToStringPercent());
			defaultInterpolatedStringHandler.AppendLiteral("): ");
			defaultInterpolatedStringHandler.AppendFormatted(StatWorker_SuppressionFallRate.CurrentFallRateBasedOnSuppression(need_Suppression.CurLevelPercentage).ToStringPercent());
			stringBuilder2.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
			if (this.stat.parts != null)
			{
				for (int i = 0; i < this.stat.parts.Count; i++)
				{
					string text = this.stat.parts[i].ExplanationPart(req);
					if (!text.NullOrEmpty())
					{
						stringBuilder.AppendLine("   " + text);
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04008353 RID: 33619
		public const float FastFallRate = 0.2f;

		// Token: 0x04008354 RID: 33620
		public const float MediumFallRate = 0.1f;

		// Token: 0x04008355 RID: 33621
		public const float SlowFallRate = 0.05f;

		// Token: 0x04008356 RID: 33622
		public const float FastFallRateThreshold = 0.3f;

		// Token: 0x04008357 RID: 33623
		public const float MediumFallRateThreshold = 0.15f;
	}
}
