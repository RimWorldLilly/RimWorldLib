using System;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FD5 RID: 8149
	public class StatWorker_Wildness : StatWorker
	{
		// Token: 0x0600CEC0 RID: 52928 RVA: 0x004C1AD4 File Offset: 0x004BFCD4
		public override bool ShouldShowFor(StatRequest req)
		{
			Pawn pawn = req.Pawn ?? (req.Thing as Pawn);
			if (pawn != null && (pawn.IsWildMan() || pawn.IsAnimal))
			{
				return true;
			}
			ThingDef thingDef = req.Def as ThingDef;
			if (thingDef != null)
			{
				RaceProperties race = thingDef.race;
				if (race != null && race.Animal)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600CEC1 RID: 52929 RVA: 0x004C1B34 File Offset: 0x004BFD34
		public override string GetExplanationFinalizePart(StatRequest req, ToStringNumberSense numberSense, float finalVal)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.GetExplanationFinalizePart(req, numberSense, finalVal));
			stringBuilder.AppendLine();
			ThingDef thingDef = req.Def as ThingDef;
			if (thingDef == null)
			{
				return stringBuilder.ToString();
			}
			Pawn pawn = req.Pawn ?? (req.Thing as Pawn);
			if (pawn != null)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("TrainingDecayInterval".Translate());
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted(TrainableUtility.DegradationPeriodTicks(pawn).ToStringTicksToDays("F1"));
				stringBuilder2.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
				stringBuilder.AppendLine();
			}
			else
			{
				RaceProperties race = thingDef.race;
				if (race != null && !race.Humanlike)
				{
					StringBuilder stringBuilder3 = stringBuilder;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(2, 2);
					defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("TrainingDecayInterval".Translate());
					defaultInterpolatedStringHandler2.AppendLiteral(": ");
					defaultInterpolatedStringHandler2.AppendFormatted(TrainableUtility.DegradationPeriodTicks(thingDef).ToStringTicksToDays("F1"));
					stringBuilder3.AppendLine(defaultInterpolatedStringHandler2.ToStringAndClear());
					stringBuilder.AppendLine();
				}
			}
			if (pawn != null && !TrainableUtility.TamenessCanDecay(pawn))
			{
				string key = pawn.FenceBlocked ? "TamenessWillNotDecayFenceBlocked" : "TamenessWillNotDecay";
				stringBuilder.AppendLine(key.Translate());
			}
			else if (!TrainableUtility.TamenessCanDecay(thingDef))
			{
				string key2 = thingDef.race.FenceBlocked ? "TamenessWillNotDecayFenceBlocked" : "TamenessWillNotDecay";
				stringBuilder.AppendLine(key2.Translate());
			}
			return stringBuilder.ToString();
		}
	}
}
