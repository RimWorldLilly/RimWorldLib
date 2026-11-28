using System;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020017E5 RID: 6117
	public class ActivityWorker
	{
		// Token: 0x060098A5 RID: 39077 RVA: 0x00374DE4 File Offset: 0x00372FE4
		public virtual float GetChangeRatePerDay(ThingWithComps thing)
		{
			CompActivity comp = thing.GetComp<CompActivity>();
			if (!comp.IsDormant)
			{
				return 0f;
			}
			return comp.Props.changePerDayBase;
		}

		// Token: 0x060098A6 RID: 39078 RVA: 0x00374E14 File Offset: 0x00373014
		public void GetInspectString(ThingWithComps thing, StringBuilder sb)
		{
			CompActivity comp = thing.GetComp<CompActivity>();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 4);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("ActivityLevel".Translate());
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted(comp.ActivityLevel.ToStringPercent("0"));
			defaultInterpolatedStringHandler.AppendLiteral(" (");
			defaultInterpolatedStringHandler.AppendFormatted(this.GetChangeRatePerDay(thing).ToStringPercentSigned("0"));
			defaultInterpolatedStringHandler.AppendLiteral(" / ");
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("day".Translate());
			defaultInterpolatedStringHandler.AppendLiteral(")");
			sb.Append(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060098A7 RID: 39079 RVA: 0x00374EC0 File Offset: 0x003730C0
		public virtual void GetSummary(ThingWithComps thing, StringBuilder sb)
		{
			CompActivity comp = thing.GetComp<CompActivity>();
			string str = "\n\n";
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 3);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("ActivityIncrease".Translate());
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted(this.GetChangeRatePerDay(thing).ToStringPercent("0"));
			defaultInterpolatedStringHandler.AppendLiteral(" / ");
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("day".Translate());
			sb.AppendLine(str + defaultInterpolatedStringHandler.ToStringAndClear());
			if (comp.State == ActivityState.Active)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(7, 1);
				defaultInterpolatedStringHandler2.AppendLiteral(" - ");
				defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("IsActive".Translate());
				defaultInterpolatedStringHandler2.AppendLiteral(": x0");
				sb.Append(defaultInterpolatedStringHandler2.ToStringAndClear());
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(5, 2);
			defaultInterpolatedStringHandler3.AppendLiteral(" - ");
			defaultInterpolatedStringHandler3.AppendFormatted<TaggedString>("BaseLevel".Translate());
			defaultInterpolatedStringHandler3.AppendLiteral(": ");
			defaultInterpolatedStringHandler3.AppendFormatted(comp.Props.changePerDayBase.ToStringPercent("0"));
			sb.Append(defaultInterpolatedStringHandler3.ToStringAndClear());
		}
	}
}
