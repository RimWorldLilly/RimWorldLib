using System;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020017E7 RID: 6119
	public class ObeliskActivityWorker : ActivityWorker
	{
		// Token: 0x060098AC RID: 39084 RVA: 0x00375218 File Offset: 0x00373418
		public override float GetChangeRatePerDay(ThingWithComps thing)
		{
			CompActivity comp = thing.GetComp<CompActivity>();
			float num = base.GetChangeRatePerDay(thing);
			if (thing.IsOutside() && comp.Props.changePerDayOutside != null)
			{
				num += comp.Props.changePerDayOutside.Value;
			}
			return num + this.ActivityGainPerDayFromHealthCurve.Evaluate((float)thing.HitPoints / (float)thing.MaxHitPoints);
		}

		// Token: 0x060098AD RID: 39085 RVA: 0x00375280 File Offset: 0x00373480
		public override void GetSummary(ThingWithComps thing, StringBuilder sb)
		{
			CompActivity comp = thing.GetComp<CompActivity>();
			base.GetSummary(thing, sb);
			float num = this.ActivityGainPerDayFromHealthCurve.Evaluate((float)thing.HitPoints / (float)thing.MaxHitPoints);
			if (num > 0f)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 2);
				defaultInterpolatedStringHandler.AppendLiteral("\n - ");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("HealthFactor".Translate());
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted(num.ToStringPercent("0"));
				sb.Append(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (thing.IsOutside() && comp.Props.changePerDayOutside != null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(6, 2);
				defaultInterpolatedStringHandler2.AppendLiteral("\n - ");
				defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("NotInSealedRoom".Translate());
				defaultInterpolatedStringHandler2.AppendLiteral(": ");
				defaultInterpolatedStringHandler2.AppendFormatted(comp.Props.changePerDayOutside.Value.ToStringPercent("0"));
				sb.Append(defaultInterpolatedStringHandler2.ToStringAndClear());
			}
		}

		// Token: 0x04005B39 RID: 23353
		private SimpleCurve ActivityGainPerDayFromHealthCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 47.9f),
				true
			},
			{
				new CurvePoint(0.1f, 23.9f),
				true
			},
			{
				new CurvePoint(0.8f, 0f),
				true
			}
		};
	}
}
