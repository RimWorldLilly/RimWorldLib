using System;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020017E6 RID: 6118
	public class PawnActivityWorker : ActivityWorker
	{
		// Token: 0x060098A9 RID: 39081 RVA: 0x00374FF8 File Offset: 0x003731F8
		public override float GetChangeRatePerDay(ThingWithComps thing)
		{
			CompActivity comp = thing.GetComp<CompActivity>();
			CompHoldingPlatformTarget compHoldingPlatformTarget = thing.TryGetComp<CompHoldingPlatformTarget>();
			Pawn pawn = (Pawn)thing;
			if (comp.State == ActivityState.Active)
			{
				return --0f;
			}
			float num = 0f;
			if (!pawn.health.hediffSet.IsPsychicallySuppressed)
			{
				num += base.GetChangeRatePerDay(thing);
				if (thing.IsOutside() && comp.Props.changePerDayOutside != null)
				{
					num += comp.Props.changePerDayOutside.Value;
				}
				if (compHoldingPlatformTarget != null && compHoldingPlatformTarget.CurrentlyHeldOnPlatform && compHoldingPlatformTarget.HeldPlatform.HasAttachedElectroharvester)
				{
					num += comp.Props.changePerDayElectroharvester;
				}
			}
			return num;
		}

		// Token: 0x060098AA RID: 39082 RVA: 0x003750A0 File Offset: 0x003732A0
		public override void GetSummary(ThingWithComps thing, StringBuilder sb)
		{
			CompActivity comp = thing.GetComp<CompActivity>();
			CompHoldingPlatformTarget compHoldingPlatformTarget = thing.TryGetComp<CompHoldingPlatformTarget>();
			base.GetSummary(thing, sb);
			Pawn pawn = thing as Pawn;
			if (pawn != null && pawn.health.hediffSet.IsPsychicallySuppressed)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 1);
				defaultInterpolatedStringHandler.AppendLiteral(" - ");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("IsPsychicallySuppressed".Translate());
				defaultInterpolatedStringHandler.AppendLiteral(": x0");
				sb.Append(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
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
			if (compHoldingPlatformTarget != null && compHoldingPlatformTarget.CurrentlyHeldOnPlatform && compHoldingPlatformTarget.HeldPlatform.HasAttachedElectroharvester)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(6, 2);
				defaultInterpolatedStringHandler3.AppendLiteral("\n - ");
				defaultInterpolatedStringHandler3.AppendFormatted<TaggedString>("EnergyHarvesting".Translate());
				defaultInterpolatedStringHandler3.AppendLiteral(": ");
				defaultInterpolatedStringHandler3.AppendFormatted(comp.Props.changePerDayElectroharvester.ToStringPercent("0"));
				sb.Append(defaultInterpolatedStringHandler3.ToStringAndClear());
			}
		}
	}
}
